#define TRACE
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlNodeNavigator : XPathNavigator
{
	private HPathDocument _doc = new HPathDocument();

	private HtmlNode _currentnode;

	private int _attindex;

	private HtmlNameTable _nametable = new HtmlNameTable();

	internal bool Trace;

	public override string LocalName
	{
		get
		{
			if (_attindex != -1)
			{
				InternalTrace("att>" + _currentnode.Attributes[_attindex].Name);
				return _nametable.GetOrAdd(_currentnode.Attributes[_attindex].Name);
			}
			InternalTrace("node>" + _currentnode.Name);
			return _nametable.GetOrAdd(_currentnode.Name);
		}
	}

	public override string NamespaceURI
	{
		get
		{
			InternalTrace(">");
			return _nametable.GetOrAdd(string.Empty);
		}
	}

	public override string Name
	{
		get
		{
			InternalTrace(">" + _currentnode.Name);
			return _nametable.GetOrAdd(_currentnode.Name);
		}
	}

	public override string Prefix
	{
		get
		{
			InternalTrace(null);
			return _nametable.GetOrAdd(string.Empty);
		}
	}

	public override XPathNodeType NodeType
	{
		get
		{
			switch (_currentnode.NodeType)
			{
			case HtmlNodeType.Comment:
				InternalTrace(">" + XPathNodeType.Comment);
				return XPathNodeType.Comment;
			case HtmlNodeType.Document:
				InternalTrace(">" + XPathNodeType.Root);
				return XPathNodeType.Root;
			case HtmlNodeType.Text:
				InternalTrace(">" + XPathNodeType.Text);
				return XPathNodeType.Text;
			case HtmlNodeType.Element:
				if (_attindex != -1)
				{
					InternalTrace(">" + XPathNodeType.Attribute);
					return XPathNodeType.Attribute;
				}
				InternalTrace(">" + XPathNodeType.Element);
				return XPathNodeType.Element;
			default:
				throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " + _currentnode.NodeType);
			}
		}
	}

	public override string Value
	{
		get
		{
			InternalTrace("nt=" + _currentnode.NodeType);
			switch (_currentnode.NodeType)
			{
			case HtmlNodeType.Comment:
				InternalTrace(">" + ((HtmlCommentNode)_currentnode).Comment);
				return ((HtmlCommentNode)_currentnode).Comment;
			case HtmlNodeType.Document:
				InternalTrace(">");
				return "";
			case HtmlNodeType.Text:
				InternalTrace(">" + ((HtmlTextNode)_currentnode).Text);
				return ((HtmlTextNode)_currentnode).Text;
			case HtmlNodeType.Element:
				if (_attindex != -1)
				{
					InternalTrace(">" + _currentnode.Attributes[_attindex].Value);
					return _currentnode.Attributes[_attindex].Value;
				}
				return _currentnode.InnerText;
			default:
				throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " + _currentnode.NodeType);
			}
		}
	}

	public override string BaseURI
	{
		get
		{
			InternalTrace(">");
			return _nametable.GetOrAdd(string.Empty);
		}
	}

	public override string XmlLang
	{
		get
		{
			InternalTrace(null);
			return _nametable.GetOrAdd(string.Empty);
		}
	}

	public override bool IsEmptyElement
	{
		get
		{
			InternalTrace(">" + !HasChildren);
			return !HasChildren;
		}
	}

	public override XmlNameTable NameTable
	{
		get
		{
			InternalTrace(null);
			return _nametable;
		}
	}

	public override bool HasAttributes
	{
		get
		{
			InternalTrace(">" + (_currentnode.Attributes.Count > 0));
			return _currentnode.Attributes.Count > 0;
		}
	}

	public override bool HasChildren
	{
		get
		{
			InternalTrace(">" + (_currentnode.ChildNodes.Count > 0));
			return _currentnode.ChildNodes.Count > 0;
		}
	}

	public HtmlNode CurrentNode => _currentnode;

	public HPathDocument CurrentDocument => _doc;

	internal HtmlNodeNavigator()
	{
		Reset();
	}

	private void Reset()
	{
		InternalTrace(null);
		_currentnode = _doc.DocumentNode;
		_attindex = -1;
	}

	[Conditional("TRACE")]
	internal void InternalTrace(object Value)
	{
		if (Trace)
		{
			string text = null;
			StackFrame stackFrame = new StackFrame(1, fNeedFileInfo: true);
			text = stackFrame.GetMethod().Name;
			string text2 = ((_currentnode != null) ? _currentnode.Name : "(null)");
			string text3 = ((_currentnode == null) ? "(null)" : (_currentnode.NodeType switch
			{
				HtmlNodeType.Comment => ((HtmlCommentNode)_currentnode).Comment, 
				HtmlNodeType.Document => "", 
				HtmlNodeType.Text => ((HtmlTextNode)_currentnode).Text, 
				_ => _currentnode.CloneNode(deep: false).OuterHtml, 
			}));
			System.Diagnostics.Trace.WriteLine("oid=" + GetHashCode() + ",n=" + text2 + ",a=" + _attindex + ",,v=" + text3 + "," + Value, "N!" + text);
		}
	}

	internal HtmlNodeNavigator(HPathDocument doc, HtmlNode currentNode)
	{
		if (currentNode == null)
		{
			throw new ArgumentNullException("currentNode");
		}
		if (currentNode.OwnerDocument != doc)
		{
			throw new ArgumentException(HPathDocument.HtmlExceptionRefNotChild);
		}
		InternalTrace(null);
		_doc = doc;
		Reset();
		_currentnode = currentNode;
	}

	private HtmlNodeNavigator(HtmlNodeNavigator nav)
	{
		if (nav == null)
		{
			throw new ArgumentNullException("nav");
		}
		InternalTrace(null);
		_doc = nav._doc;
		_currentnode = nav._currentnode;
		_attindex = nav._attindex;
		_nametable = nav._nametable;
	}

	public HtmlNodeNavigator(Stream stream)
	{
		_doc.Load(stream);
		Reset();
	}

	public HtmlNodeNavigator(Stream stream, bool detectEncodingFromByteOrderMarks)
	{
		_doc.Load(stream, detectEncodingFromByteOrderMarks);
		Reset();
	}

	public HtmlNodeNavigator(Stream stream, Encoding encoding)
	{
		_doc.Load(stream, encoding);
		Reset();
	}

	public HtmlNodeNavigator(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
		_doc.Load(stream, encoding, detectEncodingFromByteOrderMarks);
		Reset();
	}

	public HtmlNodeNavigator(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
	{
		_doc.Load(stream, encoding, detectEncodingFromByteOrderMarks, buffersize);
		Reset();
	}

	public HtmlNodeNavigator(TextReader reader)
	{
		_doc.Load(reader);
		Reset();
	}

	public HtmlNodeNavigator(string path)
	{
		_doc.Load(path);
		Reset();
	}

	public HtmlNodeNavigator(string path, bool detectEncodingFromByteOrderMarks)
	{
		_doc.Load(path, detectEncodingFromByteOrderMarks);
		Reset();
	}

	public HtmlNodeNavigator(string path, Encoding encoding)
	{
		_doc.Load(path, encoding);
		Reset();
	}

	public HtmlNodeNavigator(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
		_doc.Load(path, encoding, detectEncodingFromByteOrderMarks);
		Reset();
	}

	public HtmlNodeNavigator(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
	{
		_doc.Load(path, encoding, detectEncodingFromByteOrderMarks, buffersize);
		Reset();
	}

	public override bool MoveToNext()
	{
		if (_currentnode.NextSibling == null)
		{
			InternalTrace(">false");
			return false;
		}
		InternalTrace("_c=" + _currentnode.CloneNode(deep: false).OuterHtml);
		InternalTrace("_n=" + _currentnode.NextSibling.CloneNode(deep: false).OuterHtml);
		_currentnode = _currentnode.NextSibling;
		InternalTrace(">true");
		return true;
	}

	public override bool MoveToPrevious()
	{
		if (_currentnode.PreviousSibling == null)
		{
			InternalTrace(">false");
			return false;
		}
		_currentnode = _currentnode.PreviousSibling;
		InternalTrace(">true");
		return true;
	}

	public override bool MoveToFirst()
	{
		if (_currentnode.ParentNode == null)
		{
			InternalTrace(">false");
			return false;
		}
		if (_currentnode.ParentNode.FirstChild == null)
		{
			InternalTrace(">false");
			return false;
		}
		_currentnode = _currentnode.ParentNode.FirstChild;
		InternalTrace(">true");
		return true;
	}

	public override bool MoveToFirstChild()
	{
		if (!_currentnode.HasChildNodes)
		{
			InternalTrace(">false");
			return false;
		}
		_currentnode = _currentnode.ChildNodes[0];
		InternalTrace(">true");
		return true;
	}

	public override bool MoveToParent()
	{
		if (_currentnode.ParentNode == null)
		{
			InternalTrace(">false");
			return false;
		}
		_currentnode = _currentnode.ParentNode;
		InternalTrace(">true");
		return true;
	}

	public override void MoveToRoot()
	{
		_currentnode = _doc.DocumentNode;
		InternalTrace(null);
	}

	public override bool MoveTo(XPathNavigator other)
	{
		if (!(other is HtmlNodeNavigator htmlNodeNavigator))
		{
			InternalTrace(">false (nav is not an HtmlNodeNavigator)");
			return false;
		}
		InternalTrace("moveto oid=" + htmlNodeNavigator.GetHashCode() + ", n:" + htmlNodeNavigator._currentnode.Name + ", a:" + htmlNodeNavigator._attindex);
		if (htmlNodeNavigator._doc == _doc)
		{
			_currentnode = htmlNodeNavigator._currentnode;
			_attindex = htmlNodeNavigator._attindex;
			InternalTrace(">true");
			return true;
		}
		InternalTrace(">false (???)");
		return false;
	}

	public override bool MoveToId(string id)
	{
		InternalTrace("id=" + id);
		HtmlNode elementbyId = _doc.GetElementbyId(id);
		if (elementbyId == null)
		{
			InternalTrace(">false");
			return false;
		}
		_currentnode = elementbyId;
		InternalTrace(">true");
		return true;
	}

	public override bool IsSamePosition(XPathNavigator other)
	{
		if (!(other is HtmlNodeNavigator htmlNodeNavigator))
		{
			InternalTrace(">false");
			return false;
		}
		InternalTrace(">" + (htmlNodeNavigator._currentnode == _currentnode));
		return htmlNodeNavigator._currentnode == _currentnode;
	}

	public override XPathNavigator Clone()
	{
		InternalTrace(null);
		return new HtmlNodeNavigator(this);
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		InternalTrace("localName=" + localName + ", namespaceURI=" + namespaceURI);
		HtmlAttribute htmlAttribute = _currentnode.Attributes[localName];
		if (htmlAttribute == null)
		{
			InternalTrace(">null");
			return null;
		}
		InternalTrace(">" + htmlAttribute.Value);
		return htmlAttribute.Value;
	}

	public override bool MoveToAttribute(string localName, string namespaceURI)
	{
		InternalTrace("localName=" + localName + ", namespaceURI=" + namespaceURI);
		int attributeIndex = _currentnode.Attributes.GetAttributeIndex(localName);
		if (attributeIndex == -1)
		{
			InternalTrace(">false");
			return false;
		}
		_attindex = attributeIndex;
		InternalTrace(">true");
		return true;
	}

	public override bool MoveToFirstAttribute()
	{
		if (!HasAttributes)
		{
			InternalTrace(">false");
			return false;
		}
		_attindex = 0;
		InternalTrace(">true");
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		InternalTrace(null);
		if (_attindex >= _currentnode.Attributes.Count - 1)
		{
			InternalTrace(">false");
			return false;
		}
		_attindex++;
		InternalTrace(">true");
		return true;
	}

	public override string GetNamespace(string name)
	{
		InternalTrace("name=" + name);
		return string.Empty;
	}

	public override bool MoveToNamespace(string name)
	{
		InternalTrace("name=" + name);
		return false;
	}

	public override bool MoveToFirstNamespace(XPathNamespaceScope scope)
	{
		InternalTrace(null);
		return false;
	}

	public override bool MoveToNextNamespace(XPathNamespaceScope scope)
	{
		InternalTrace(null);
		return false;
	}
}
