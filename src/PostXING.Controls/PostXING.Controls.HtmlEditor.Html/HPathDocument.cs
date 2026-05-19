using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace PostXING.Controls.HtmlEditor.Html;

public class HPathDocument : IXPathNavigable
{
	private enum ParseState
	{
		Text,
		WhichTag,
		Tag,
		BetweenAttributes,
		EmptyTag,
		AttributeName,
		AttributeBeforeEquals,
		AttributeAfterEquals,
		AttributeValue,
		Comment,
		QuotedAttributeValue,
		ServerSideCode,
		PcData
	}

	internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";

	internal static readonly string HtmlExceptionUseIdAttributeFalse = "You need to set UseIdAttribute property to true to enable this feature";

	internal Hashtable _openednodes;

	internal Hashtable _lastnodes = new Hashtable();

	internal Hashtable _nodesid;

	private HtmlNode _documentnode;

	internal string _text;

	private HtmlNode _currentnode;

	private HtmlNode _lastparentnode;

	private HtmlAttribute _currentattribute;

	private int _index;

	private int _line;

	private int _lineposition;

	private int _maxlineposition;

	private int _c;

	private bool _fullcomment;

	private Encoding _streamencoding;

	private Encoding _declaredencoding;

	private ArrayList _parseerrors = new ArrayList();

	private ParseState _state;

	private ParseState _oldstate;

	private Crc32 _crc32;

	private bool _onlyDetectEncoding;

	public bool OptionComputeChecksum;

	public bool OptionReadEncoding = true;

	public bool OptionCheckSyntax = true;

	public bool OptionUseIdAttribute = true;

	public bool OptionWriteEmptyNodes;

	public bool OptionOutputAsXml;

	public bool OptionOutputUpperCase;

	public bool OptionOutputOptimizeAttributeValues;

	public bool OptionAddDebuggingAttributes;

	public bool OptionExtractErrorSourceText;

	public bool OptionAutoCloseOnEnd;

	public bool OptionFixNestedTags;

	public int OptionExtractErrorSourceTextMaxLength = 100;

	public Encoding OptionDefaultStreamEncoding = Encoding.Default;

	public ArrayList ParseErrors => _parseerrors;

	public Encoding StreamEncoding => _streamencoding;

	public Encoding DeclaredEncoding => _declaredencoding;

	public Encoding Encoding => GetOutEncoding();

	public HtmlNode DocumentNode => _documentnode;

	public int CheckSum
	{
		get
		{
			if (_crc32 == null)
			{
				return 0;
			}
			return (int)_crc32.CheckSum;
		}
	}

	public HPathDocument()
	{
		_documentnode = CreateNode(HtmlNodeType.Document, 0);
	}

	internal HtmlNode GetXmlDeclaration()
	{
		if (!_documentnode.HasChildNodes)
		{
			return null;
		}
		foreach (HtmlNode childnode in _documentnode._childnodes)
		{
			if (childnode.Name == "?xml")
			{
				return childnode;
			}
		}
		return null;
	}

	public static string HtmlEncode(string html)
	{
		if (html == null)
		{
			throw new ArgumentNullException("html");
		}
		Regex regex = new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase);
		return regex.Replace(html, "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
			.Replace("\"", "&quot;");
	}

	public Encoding DetectEncoding(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		return DetectEncoding(new StreamReader(stream));
	}

	public Encoding DetectEncoding(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		StreamReader streamReader = new StreamReader(path, OptionDefaultStreamEncoding);
		Encoding result = DetectEncoding(streamReader);
		streamReader.Close();
		return result;
	}

	public Encoding DetectEncodingHtml(string html)
	{
		if (html == null)
		{
			throw new ArgumentNullException("html");
		}
		StringReader stringReader = new StringReader(html);
		Encoding result = DetectEncoding(stringReader);
		stringReader.Close();
		return result;
	}

	public Encoding DetectEncoding(TextReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		_onlyDetectEncoding = true;
		if (OptionCheckSyntax)
		{
			_openednodes = new Hashtable();
		}
		else
		{
			_openednodes = null;
		}
		if (OptionUseIdAttribute)
		{
			_nodesid = new Hashtable();
		}
		else
		{
			_nodesid = null;
		}
		if (reader is StreamReader streamReader)
		{
			_streamencoding = streamReader.CurrentEncoding;
		}
		else
		{
			_streamencoding = null;
		}
		_declaredencoding = null;
		_text = reader.ReadToEnd();
		_documentnode = CreateNode(HtmlNodeType.Document, 0);
		try
		{
			Parse();
		}
		catch (EncodingFoundException ex)
		{
			return ex.Encoding;
		}
		return null;
	}

	public void Load(Stream stream)
	{
		Load(new StreamReader(stream, OptionDefaultStreamEncoding));
	}

	public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
	{
		Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
	}

	public void Load(Stream stream, Encoding encoding)
	{
		Load(new StreamReader(stream, encoding));
	}

	public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
		Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
	}

	public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
	{
		Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
	}

	public void Load(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		StreamReader streamReader = new StreamReader(path, OptionDefaultStreamEncoding);
		Load(streamReader);
		streamReader.Close();
	}

	public void Load(string path, bool detectEncodingFromByteOrderMarks)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		StreamReader streamReader = new StreamReader(path, detectEncodingFromByteOrderMarks);
		Load(streamReader);
		streamReader.Close();
	}

	public void Load(string path, Encoding encoding)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding");
		}
		StreamReader streamReader = new StreamReader(path, encoding);
		Load(streamReader);
		streamReader.Close();
	}

	public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding");
		}
		StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks);
		Load(streamReader);
		streamReader.Close();
	}

	public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding");
		}
		StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize);
		Load(streamReader);
		streamReader.Close();
	}

	public void LoadHtml(string html)
	{
		if (html == null)
		{
			throw new ArgumentNullException("html");
		}
		StringReader stringReader = new StringReader(html);
		Load(stringReader);
		stringReader.Close();
	}

	public void DetectEncodingAndLoad(string path)
	{
		DetectEncodingAndLoad(path, detectEncoding: true);
	}

	public void DetectEncodingAndLoad(string path, bool detectEncoding)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		Encoding encoding = ((!detectEncoding) ? null : DetectEncoding(path));
		if (encoding == null)
		{
			Load(path);
		}
		else
		{
			Load(path, encoding);
		}
	}

	public void Load(TextReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		_onlyDetectEncoding = false;
		if (OptionCheckSyntax)
		{
			_openednodes = new Hashtable();
		}
		else
		{
			_openednodes = null;
		}
		if (OptionUseIdAttribute)
		{
			_nodesid = new Hashtable();
		}
		else
		{
			_nodesid = null;
		}
		if (reader is StreamReader streamReader)
		{
			_streamencoding = streamReader.CurrentEncoding;
		}
		else
		{
			_streamencoding = null;
		}
		_declaredencoding = null;
		_text = reader.ReadToEnd();
		_documentnode = CreateNode(HtmlNodeType.Document, 0);
		Parse();
		if (!OptionCheckSyntax)
		{
			return;
		}
		foreach (HtmlNode value in _openednodes.Values)
		{
			if (!value._starttag)
			{
				continue;
			}
			string text;
			if (OptionExtractErrorSourceText)
			{
				text = value.OuterHtml;
				if (text.Length > OptionExtractErrorSourceTextMaxLength)
				{
					text = text.Substring(0, OptionExtractErrorSourceTextMaxLength);
				}
			}
			else
			{
				text = string.Empty;
			}
			AddError(HtmlParseErrorCode.TagNotClosed, value._line, value._lineposition, value._streamposition, text, "End tag </" + value.Name + "> was not found");
		}
		_openednodes.Clear();
	}

	internal Encoding GetOutEncoding()
	{
		if (_declaredencoding != null)
		{
			return _declaredencoding;
		}
		if (_streamencoding != null)
		{
			return _streamencoding;
		}
		return OptionDefaultStreamEncoding;
	}

	public void Save(Stream outStream)
	{
		StreamWriter writer = new StreamWriter(outStream, GetOutEncoding());
		Save(writer);
	}

	public void Save(Stream outStream, Encoding encoding)
	{
		if (outStream == null)
		{
			throw new ArgumentNullException("outStream");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding");
		}
		StreamWriter writer = new StreamWriter(outStream, encoding);
		Save(writer);
	}

	public void Save(string filename)
	{
		StreamWriter streamWriter = new StreamWriter(filename, append: false, GetOutEncoding());
		Save(streamWriter);
		streamWriter.Close();
	}

	public void Save(string filename, Encoding encoding)
	{
		if (filename == null)
		{
			throw new ArgumentNullException("filename");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding");
		}
		StreamWriter streamWriter = new StreamWriter(filename, append: false, encoding);
		Save(streamWriter);
		streamWriter.Close();
	}

	public void Save(StreamWriter writer)
	{
		Save((TextWriter)writer);
	}

	public void Save(TextWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		DocumentNode.WriteTo(writer);
	}

	public void Save(XmlWriter writer)
	{
		DocumentNode.WriteTo(writer);
		writer.Flush();
	}

	public XPathNavigator CreateNavigator()
	{
		return new HtmlNodeNavigator(this, _documentnode);
	}

	internal void SetIdForNode(HtmlNode node, string id)
	{
		if (OptionUseIdAttribute && _nodesid != null && id != null)
		{
			if (node == null)
			{
				_nodesid.Remove(id.ToLower());
			}
			else
			{
				_nodesid[id.ToLower()] = node;
			}
		}
	}

	public HtmlNode GetElementbyId(string id)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (_nodesid == null)
		{
			throw new Exception(HtmlExceptionUseIdAttributeFalse);
		}
		return _nodesid[id.ToLower()] as HtmlNode;
	}

	public HtmlNode CreateElement(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		HtmlNode htmlNode = CreateNode(HtmlNodeType.Element);
		htmlNode._name = name;
		return htmlNode;
	}

	public HtmlCommentNode CreateComment()
	{
		return (HtmlCommentNode)CreateNode(HtmlNodeType.Comment);
	}

	public HtmlCommentNode CreateComment(string comment)
	{
		if (comment == null)
		{
			throw new ArgumentNullException("comment");
		}
		HtmlCommentNode htmlCommentNode = CreateComment();
		htmlCommentNode.Comment = comment;
		return htmlCommentNode;
	}

	public HtmlTextNode CreateTextNode()
	{
		return (HtmlTextNode)CreateNode(HtmlNodeType.Text);
	}

	public HtmlTextNode CreateTextNode(string text)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		HtmlTextNode htmlTextNode = CreateTextNode();
		htmlTextNode.Text = text;
		return htmlTextNode;
	}

	internal HtmlNode CreateNode(HtmlNodeType type)
	{
		return CreateNode(type, -1);
	}

	internal HtmlNode CreateNode(HtmlNodeType type, int index)
	{
		return type switch
		{
			HtmlNodeType.Comment => new HtmlCommentNode(this, index), 
			HtmlNodeType.Text => new HtmlTextNode(this, index), 
			_ => new HtmlNode(type, this, index), 
		};
	}

	internal HtmlAttribute CreateAttribute()
	{
		return new HtmlAttribute(this);
	}

	public HtmlAttribute CreateAttribute(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		HtmlAttribute htmlAttribute = CreateAttribute();
		htmlAttribute.Name = name;
		return htmlAttribute;
	}

	public HtmlAttribute CreateAttribute(string name, string value)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		HtmlAttribute htmlAttribute = CreateAttribute(name);
		htmlAttribute.Value = value;
		return htmlAttribute;
	}

	private HtmlParseError AddError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText, string reason)
	{
		HtmlParseError htmlParseError = new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason);
		_parseerrors.Add(htmlParseError);
		return htmlParseError;
	}

	private void IncrementPosition()
	{
		if (_crc32 != null)
		{
			_crc32.AddToCRC32(_c);
		}
		_index++;
		_maxlineposition = _lineposition;
		if (_c == 10)
		{
			_lineposition = 1;
			_line++;
		}
		else
		{
			_lineposition++;
		}
	}

	private void DecrementPosition()
	{
		_index--;
		if (_lineposition == 1)
		{
			_lineposition = _maxlineposition;
			_line--;
		}
		else
		{
			_lineposition--;
		}
	}

	private void Parse()
	{
		int num = 0;
		if (OptionComputeChecksum)
		{
			_crc32 = new Crc32();
		}
		_lastnodes = new Hashtable();
		_c = 0;
		_fullcomment = false;
		_parseerrors = new ArrayList();
		_line = 1;
		_lineposition = 1;
		_maxlineposition = 1;
		_state = ParseState.Text;
		_oldstate = _state;
		_documentnode._innerlength = _text.Length;
		_documentnode._outerlength = _text.Length;
		_lastparentnode = _documentnode;
		_currentnode = CreateNode(HtmlNodeType.Text, 0);
		_currentattribute = null;
		_index = 0;
		PushNodeStart(HtmlNodeType.Text, 0);
		while (_index < _text.Length)
		{
			_c = _text[_index];
			IncrementPosition();
			switch (_state)
			{
			case ParseState.Text:
				if (!NewCheck())
				{
				}
				break;
			case ParseState.WhichTag:
				if (!NewCheck())
				{
					if (_c == 47)
					{
						PushNodeNameStart(starttag: false, _index);
					}
					else
					{
						PushNodeNameStart(starttag: true, _index - 1);
						DecrementPosition();
					}
					_state = ParseState.Tag;
				}
				break;
			case ParseState.Tag:
				if (NewCheck())
				{
					break;
				}
				if (IsWhiteSpace(_c))
				{
					PushNodeNameEnd(_index - 1);
					if (_state == ParseState.Tag)
					{
						_state = ParseState.BetweenAttributes;
					}
				}
				else if (_c == 47)
				{
					PushNodeNameEnd(_index - 1);
					if (_state == ParseState.Tag)
					{
						_state = ParseState.EmptyTag;
					}
				}
				else
				{
					if (_c != 62)
					{
						break;
					}
					PushNodeNameEnd(_index - 1);
					if (_state == ParseState.Tag)
					{
						PushNodeEnd(_index, close: false);
						if (_state == ParseState.Tag)
						{
							_state = ParseState.Text;
							PushNodeStart(HtmlNodeType.Text, _index);
						}
					}
				}
				break;
			case ParseState.BetweenAttributes:
				if (NewCheck() || IsWhiteSpace(_c))
				{
					break;
				}
				if (_c == 47 || _c == 63)
				{
					_state = ParseState.EmptyTag;
				}
				else if (_c == 62)
				{
					PushNodeEnd(_index, close: false);
					if (_state == ParseState.BetweenAttributes)
					{
						_state = ParseState.Text;
						PushNodeStart(HtmlNodeType.Text, _index);
					}
				}
				else
				{
					PushAttributeNameStart(_index - 1);
					_state = ParseState.AttributeName;
				}
				break;
			case ParseState.EmptyTag:
				if (NewCheck())
				{
					break;
				}
				if (_c == 62)
				{
					PushNodeEnd(_index, close: true);
					if (_state == ParseState.EmptyTag)
					{
						_state = ParseState.Text;
						PushNodeStart(HtmlNodeType.Text, _index);
					}
				}
				else
				{
					_state = ParseState.BetweenAttributes;
				}
				break;
			case ParseState.AttributeName:
				if (NewCheck())
				{
					break;
				}
				if (IsWhiteSpace(_c))
				{
					PushAttributeNameEnd(_index - 1);
					_state = ParseState.AttributeBeforeEquals;
				}
				else if (_c == 61)
				{
					PushAttributeNameEnd(_index - 1);
					_state = ParseState.AttributeAfterEquals;
				}
				else if (_c == 62)
				{
					PushAttributeNameEnd(_index - 1);
					PushNodeEnd(_index, close: false);
					if (_state == ParseState.AttributeName)
					{
						_state = ParseState.Text;
						PushNodeStart(HtmlNodeType.Text, _index);
					}
				}
				break;
			case ParseState.AttributeBeforeEquals:
				if (NewCheck() || IsWhiteSpace(_c))
				{
					break;
				}
				if (_c == 62)
				{
					PushNodeEnd(_index, close: false);
					if (_state == ParseState.AttributeBeforeEquals)
					{
						_state = ParseState.Text;
						PushNodeStart(HtmlNodeType.Text, _index);
					}
				}
				else if (_c == 61)
				{
					_state = ParseState.AttributeAfterEquals;
				}
				else
				{
					_state = ParseState.BetweenAttributes;
					DecrementPosition();
				}
				break;
			case ParseState.AttributeAfterEquals:
				if (NewCheck() || IsWhiteSpace(_c))
				{
					break;
				}
				if (_c == 39 || _c == 34)
				{
					_state = ParseState.QuotedAttributeValue;
					PushAttributeValueStart(_index);
					num = _c;
				}
				else if (_c == 62)
				{
					PushNodeEnd(_index, close: false);
					if (_state == ParseState.AttributeAfterEquals)
					{
						_state = ParseState.Text;
						PushNodeStart(HtmlNodeType.Text, _index);
					}
				}
				else
				{
					PushAttributeValueStart(_index - 1);
					_state = ParseState.AttributeValue;
				}
				break;
			case ParseState.AttributeValue:
				if (NewCheck())
				{
					break;
				}
				if (IsWhiteSpace(_c))
				{
					PushAttributeValueEnd(_index - 1);
					_state = ParseState.BetweenAttributes;
				}
				else if (_c == 62)
				{
					PushAttributeValueEnd(_index - 1);
					PushNodeEnd(_index, close: false);
					if (_state == ParseState.AttributeValue)
					{
						_state = ParseState.Text;
						PushNodeStart(HtmlNodeType.Text, _index);
					}
				}
				break;
			case ParseState.QuotedAttributeValue:
				if (_c == num)
				{
					PushAttributeValueEnd(_index - 1);
					_state = ParseState.BetweenAttributes;
				}
				else if (_c == 60 && _index < _text.Length && _text[_index] == '%')
				{
					_oldstate = _state;
					_state = ParseState.ServerSideCode;
				}
				break;
			case ParseState.Comment:
				if (_c == 62 && (!_fullcomment || (_text[_index - 2] == '-' && _text[_index - 3] == '-')))
				{
					PushNodeEnd(_index, close: false);
					_state = ParseState.Text;
					PushNodeStart(HtmlNodeType.Text, _index);
				}
				break;
			case ParseState.ServerSideCode:
				if (_c == 37 && _index < _text.Length && _text[_index] == '>')
				{
					switch (_oldstate)
					{
					case ParseState.AttributeAfterEquals:
						_state = ParseState.AttributeValue;
						break;
					case ParseState.BetweenAttributes:
						PushAttributeNameEnd(_index + 1);
						_state = ParseState.BetweenAttributes;
						break;
					default:
						_state = _oldstate;
						break;
					}
					IncrementPosition();
				}
				break;
			case ParseState.PcData:
				if (_currentnode._namelength + 3 <= _text.Length - (_index - 1) && string.Compare(_text.Substring(_index - 1, _currentnode._namelength + 2), "</" + _currentnode.Name, ignoreCase: true) == 0)
				{
					int num2 = _text[_index - 1 + 2 + _currentnode.Name.Length];
					if (num2 == 62 || IsWhiteSpace(num2))
					{
						HtmlNode htmlNode = CreateNode(HtmlNodeType.Text, _currentnode._outerstartindex + _currentnode._outerlength);
						htmlNode._outerlength = _index - 1 - htmlNode._outerstartindex;
						_currentnode.AppendChild(htmlNode);
						PushNodeStart(HtmlNodeType.Element, _index - 1);
						PushNodeNameStart(starttag: false, _index - 1 + 2);
						_state = ParseState.Tag;
						IncrementPosition();
					}
				}
				break;
			}
		}
		if (_currentnode._namestartindex > 0)
		{
			PushNodeNameEnd(_index);
		}
		PushNodeEnd(_index, close: false);
		_lastnodes.Clear();
	}

	private bool NewCheck()
	{
		if (_c != 60)
		{
			return false;
		}
		if (_index < _text.Length && _text[_index] == '%')
		{
			switch (_state)
			{
			case ParseState.AttributeAfterEquals:
				PushAttributeValueStart(_index - 1);
				break;
			case ParseState.BetweenAttributes:
				PushAttributeNameStart(_index - 1);
				break;
			case ParseState.WhichTag:
				PushNodeNameStart(starttag: true, _index - 1);
				_state = ParseState.Tag;
				break;
			}
			_oldstate = _state;
			_state = ParseState.ServerSideCode;
			return true;
		}
		PushNodeEnd(_index - 1, close: true);
		_state = ParseState.WhichTag;
		if (_index - 1 <= _text.Length - 2 && _text[_index] == '!')
		{
			PushNodeStart(HtmlNodeType.Comment, _index - 1);
			PushNodeNameStart(starttag: true, _index);
			PushNodeNameEnd(_index + 1);
			_state = ParseState.Comment;
			if (_index < _text.Length - 2)
			{
				if (_text[_index + 1] == '-' && _text[_index + 2] == '-')
				{
					_fullcomment = true;
				}
				else
				{
					_fullcomment = false;
				}
			}
			return true;
		}
		PushNodeStart(HtmlNodeType.Element, _index - 1);
		return true;
	}

	private void ReadDocumentEncoding(HtmlNode node)
	{
		if (!OptionReadEncoding || node._namelength != 4 || !(node.Name == "meta"))
		{
			return;
		}
		HtmlAttribute htmlAttribute = node.Attributes["http-equiv"];
		if (htmlAttribute == null || string.Compare(htmlAttribute.Value, "content-type", ignoreCase: true) != 0)
		{
			return;
		}
		HtmlAttribute htmlAttribute2 = node.Attributes["content"];
		if (htmlAttribute2 == null)
		{
			return;
		}
		string nameValuePairsValue = NameValuePairList.GetNameValuePairsValue(htmlAttribute2.Value, "charset");
		if (nameValuePairsValue != null)
		{
			_declaredencoding = Encoding.GetEncoding(nameValuePairsValue);
			if (_onlyDetectEncoding)
			{
				throw new EncodingFoundException(_declaredencoding);
			}
			if (_streamencoding != null && _declaredencoding.WindowsCodePage != _streamencoding.WindowsCodePage)
			{
				AddError(HtmlParseErrorCode.CharsetMismatch, _line, _lineposition, _index, node.OuterHtml, "Encoding mismatch between StreamEncoding: " + _streamencoding.WebName + " and DeclaredEncoding: " + _declaredencoding.WebName);
			}
		}
	}

	private void PushAttributeNameStart(int index)
	{
		_currentattribute = CreateAttribute();
		_currentattribute._namestartindex = index;
		_currentattribute._line = _line;
		_currentattribute._lineposition = _lineposition;
		_currentattribute._streamposition = index;
	}

	private void PushAttributeNameEnd(int index)
	{
		_currentattribute._namelength = index - _currentattribute._namestartindex;
		_currentnode.Attributes.Append(_currentattribute);
	}

	private void PushAttributeValueStart(int index)
	{
		_currentattribute._valuestartindex = index;
	}

	private void PushAttributeValueEnd(int index)
	{
		_currentattribute._valuelength = index - _currentattribute._valuestartindex;
	}

	private void PushNodeStart(HtmlNodeType type, int index)
	{
		_currentnode = CreateNode(type, index);
		_currentnode._line = _line;
		_currentnode._lineposition = _lineposition;
		if (type == HtmlNodeType.Element)
		{
			_currentnode._lineposition--;
		}
		_currentnode._streamposition = index;
	}

	private void PushNodeEnd(int index, bool close)
	{
		_currentnode._outerlength = index - _currentnode._outerstartindex;
		if (_currentnode._nodetype == HtmlNodeType.Text || _currentnode._nodetype == HtmlNodeType.Comment)
		{
			if (_currentnode._outerlength > 0)
			{
				_currentnode._innerlength = _currentnode._outerlength;
				_currentnode._innerstartindex = _currentnode._outerstartindex;
				if (_lastparentnode != null)
				{
					_lastparentnode.AppendChild(_currentnode);
				}
			}
		}
		else if (_currentnode._starttag && _lastparentnode != _currentnode)
		{
			if (_lastparentnode != null)
			{
				_lastparentnode.AppendChild(_currentnode);
			}
			ReadDocumentEncoding(_currentnode);
			HtmlNode prevwithsamename = (HtmlNode)_lastnodes[_currentnode.Name];
			_currentnode._prevwithsamename = prevwithsamename;
			_lastnodes[_currentnode.Name] = _currentnode;
			if (_currentnode.NodeType == HtmlNodeType.Document || _currentnode.NodeType == HtmlNodeType.Element)
			{
				_lastparentnode = _currentnode;
			}
			if (HtmlNode.IsCDataElement(CurrentNodeName()))
			{
				_state = ParseState.PcData;
				return;
			}
			if (HtmlNode.IsClosedElement(_currentnode.Name) || HtmlNode.IsEmptyElement(_currentnode.Name))
			{
				close = true;
			}
		}
		if (close || !_currentnode._starttag)
		{
			CloseCurrentNode();
		}
	}

	private void PushNodeNameStart(bool starttag, int index)
	{
		_currentnode._starttag = starttag;
		_currentnode._namestartindex = index;
	}

	private string[] GetResetters(string name)
	{
		switch (name)
		{
		case "li":
			return new string[1] { "ul" };
		case "tr":
			return new string[1] { "table" };
		case "th":
		case "td":
			return new string[2] { "tr", "table" };
		default:
			return null;
		}
	}

	private void FixNestedTags()
	{
		if (_currentnode._starttag)
		{
			string name = CurrentNodeName().ToLower();
			FixNestedTag(name, GetResetters(name));
		}
	}

	private void FixNestedTag(string name, string[] resetters)
	{
		if (resetters != null)
		{
			HtmlNode htmlNode = (HtmlNode)_lastnodes[name];
			if (htmlNode != null && !htmlNode.Closed && !FindResetterNodes(htmlNode, resetters))
			{
				HtmlNode htmlNode2 = new HtmlNode(htmlNode.NodeType, this, -1);
				htmlNode2._endnode = htmlNode2;
				htmlNode.CloseNode(htmlNode2);
			}
		}
	}

	private bool FindResetterNodes(HtmlNode node, string[] names)
	{
		if (names == null)
		{
			return false;
		}
		for (int i = 0; i < names.Length; i++)
		{
			if (FindResetterNode(node, names[i]) != null)
			{
				return true;
			}
		}
		return false;
	}

	private HtmlNode FindResetterNode(HtmlNode node, string name)
	{
		HtmlNode htmlNode = (HtmlNode)_lastnodes[name];
		if (htmlNode == null)
		{
			return null;
		}
		if (htmlNode.Closed)
		{
			return null;
		}
		if (htmlNode._streamposition < node._streamposition)
		{
			return null;
		}
		return htmlNode;
	}

	private void PushNodeNameEnd(int index)
	{
		_currentnode._namelength = index - _currentnode._namestartindex;
		if (OptionFixNestedTags)
		{
			FixNestedTags();
		}
	}

	private void CloseCurrentNode()
	{
		if (_currentnode.Closed)
		{
			return;
		}
		bool flag = false;
		HtmlNode htmlNode = (HtmlNode)_lastnodes[_currentnode.Name];
		if (htmlNode == null)
		{
			if (HtmlNode.IsClosedElement(_currentnode.Name))
			{
				_currentnode.CloseNode(_currentnode);
				if (_lastparentnode != null)
				{
					HtmlNode htmlNode2 = null;
					Stack stack = new Stack();
					for (HtmlNode htmlNode3 = _lastparentnode.LastChild; htmlNode3 != null; htmlNode3 = htmlNode3.PreviousSibling)
					{
						if (htmlNode3.Name == _currentnode.Name && !htmlNode3.HasChildNodes)
						{
							htmlNode2 = htmlNode3;
							break;
						}
						stack.Push(htmlNode3);
					}
					if (htmlNode2 != null)
					{
						HtmlNode htmlNode4 = null;
						while (stack.Count != 0)
						{
							htmlNode4 = (HtmlNode)stack.Pop();
							_lastparentnode.RemoveChild(htmlNode4);
							htmlNode2.AppendChild(htmlNode4);
						}
					}
					else
					{
						_lastparentnode.AppendChild(_currentnode);
					}
				}
			}
			else if (HtmlNode.CanOverlapElement(_currentnode.Name))
			{
				HtmlNode htmlNode5 = CreateNode(HtmlNodeType.Text, _currentnode._outerstartindex);
				htmlNode5._outerlength = _currentnode._outerlength;
				((HtmlTextNode)htmlNode5).Text = ((HtmlTextNode)htmlNode5).Text.ToLower();
				if (_lastparentnode != null)
				{
					_lastparentnode.AppendChild(htmlNode5);
				}
			}
			else if (HtmlNode.IsEmptyElement(_currentnode.Name))
			{
				AddError(HtmlParseErrorCode.EndTagNotRequired, _currentnode._line, _currentnode._lineposition, _currentnode._streamposition, _currentnode.OuterHtml, "End tag </" + _currentnode.Name + "> is not required");
			}
			else
			{
				AddError(HtmlParseErrorCode.TagNotOpened, _currentnode._line, _currentnode._lineposition, _currentnode._streamposition, _currentnode.OuterHtml, "Start tag <" + _currentnode.Name + "> was not found");
				flag = true;
			}
		}
		else
		{
			if (OptionFixNestedTags && FindResetterNodes(htmlNode, GetResetters(_currentnode.Name)))
			{
				AddError(HtmlParseErrorCode.EndTagInvalidHere, _currentnode._line, _currentnode._lineposition, _currentnode._streamposition, _currentnode.OuterHtml, "End tag </" + _currentnode.Name + "> invalid here");
				flag = true;
			}
			if (!flag)
			{
				_lastnodes[_currentnode.Name] = htmlNode._prevwithsamename;
				htmlNode.CloseNode(_currentnode);
			}
		}
		if (!flag && _lastparentnode != null && (!HtmlNode.IsClosedElement(_currentnode.Name) || _currentnode._starttag))
		{
			UpdateLastParentNode();
		}
	}

	internal void UpdateLastParentNode()
	{
		do
		{
			if (_lastparentnode.Closed)
			{
				_lastparentnode = _lastparentnode.ParentNode;
			}
		}
		while (_lastparentnode != null && _lastparentnode.Closed);
		if (_lastparentnode == null)
		{
			_lastparentnode = _documentnode;
		}
	}

	private string CurrentAttributeName()
	{
		return _text.Substring(_currentattribute._namestartindex, _currentattribute._namelength);
	}

	private string CurrentAttributeValue()
	{
		return _text.Substring(_currentattribute._valuestartindex, _currentattribute._valuelength);
	}

	private string CurrentNodeName()
	{
		return _text.Substring(_currentnode._namestartindex, _currentnode._namelength);
	}

	private string CurrentNodeOuter()
	{
		return _text.Substring(_currentnode._outerstartindex, _currentnode._outerlength);
	}

	private string CurrentNodeInner()
	{
		return _text.Substring(_currentnode._innerstartindex, _currentnode._innerlength);
	}

	public static bool IsWhiteSpace(int c)
	{
		if (c == 10 || c == 13 || c == 32 || c == 9)
		{
			return true;
		}
		return false;
	}
}
