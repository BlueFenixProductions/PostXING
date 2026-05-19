using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlNode : IXPathNavigable
{
	public static readonly string HtmlNodeTypeNameComment;

	public static readonly string HtmlNodeTypeNameDocument;

	public static readonly string HtmlNodeTypeNameText;

	public static Hashtable ElementsFlags;

	internal HtmlNodeType _nodetype;

	internal HtmlNode _nextnode;

	internal HtmlNode _prevnode;

	internal HtmlNode _parentnode;

	internal HPathDocument _ownerdocument;

	internal HtmlNodeCollection _childnodes;

	internal HtmlAttributeCollection _attributes;

	internal int _line;

	internal int _lineposition;

	internal int _streamposition;

	internal int _innerstartindex;

	internal int _innerlength;

	internal int _outerstartindex;

	internal int _outerlength;

	internal int _namestartindex;

	internal int _namelength;

	internal bool _starttag;

	internal string _name;

	internal HtmlNode _prevwithsamename;

	internal HtmlNode _endnode;

	internal bool _innerchanged;

	internal bool _outerchanged;

	internal string _innerhtml;

	internal string _outerhtml;

	internal HtmlNode EndNode => _endnode;

	public string Id
	{
		get
		{
			if (_ownerdocument._nodesid == null)
			{
				throw new Exception(HPathDocument.HtmlExceptionUseIdAttributeFalse);
			}
			return GetId();
		}
		set
		{
			if (_ownerdocument._nodesid == null)
			{
				throw new Exception(HPathDocument.HtmlExceptionUseIdAttributeFalse);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			SetId(value);
		}
	}

	public int Line => _line;

	public int LinePosition => _lineposition;

	public int StreamPosition => _streamposition;

	public bool Closed => _endnode != null;

	public string Name
	{
		get
		{
			if (_name == null)
			{
				_name = _ownerdocument._text.Substring(_namestartindex, _namelength).ToLower();
			}
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public virtual string InnerText
	{
		get
		{
			if (_nodetype == HtmlNodeType.Text)
			{
				return ((HtmlTextNode)this).Text;
			}
			if (_nodetype == HtmlNodeType.Comment)
			{
				return ((HtmlCommentNode)this).Comment;
			}
			if (!HasChildNodes)
			{
				return string.Empty;
			}
			string text = null;
			foreach (HtmlNode childNode in ChildNodes)
			{
				text += childNode.InnerText;
			}
			return text;
		}
	}

	public virtual string InnerHtml
	{
		get
		{
			if (_innerchanged)
			{
				_innerhtml = WriteContentTo();
				_innerchanged = false;
				return _innerhtml;
			}
			if (_innerhtml != null)
			{
				return _innerhtml;
			}
			if (_innerstartindex < 0)
			{
				return string.Empty;
			}
			return _ownerdocument._text.Substring(_innerstartindex, _innerlength);
		}
		set
		{
			HPathDocument hPathDocument = new HPathDocument();
			hPathDocument.LoadHtml(value);
			RemoveAllChildren();
			AppendChildren(hPathDocument.DocumentNode.ChildNodes);
		}
	}

	public virtual string OuterHtml
	{
		get
		{
			if (_outerchanged)
			{
				_outerhtml = WriteTo();
				_outerchanged = false;
				return _outerhtml;
			}
			if (_outerhtml != null)
			{
				return _outerhtml;
			}
			if (_outerstartindex < 0)
			{
				return string.Empty;
			}
			return _ownerdocument._text.Substring(_outerstartindex, _outerlength);
		}
	}

	public HtmlNode NextSibling => _nextnode;

	public HtmlNode PreviousSibling => _prevnode;

	public HtmlNode FirstChild
	{
		get
		{
			if (!HasChildNodes)
			{
				return null;
			}
			return _childnodes[0];
		}
	}

	public HtmlNode LastChild
	{
		get
		{
			if (!HasChildNodes)
			{
				return null;
			}
			return _childnodes[_childnodes.Count - 1];
		}
	}

	public HtmlNodeType NodeType => _nodetype;

	public HtmlNode ParentNode => _parentnode;

	public HPathDocument OwnerDocument => _ownerdocument;

	public HtmlNodeCollection ChildNodes
	{
		get
		{
			if (_childnodes == null)
			{
				_childnodes = new HtmlNodeCollection(this);
			}
			return _childnodes;
		}
	}

	public bool HasAttributes
	{
		get
		{
			if (_attributes == null)
			{
				return false;
			}
			if (_attributes.Count <= 0)
			{
				return false;
			}
			return true;
		}
	}

	public bool HasClosingAttributes
	{
		get
		{
			if (_endnode == null || _endnode == this)
			{
				return false;
			}
			if (_endnode._attributes == null)
			{
				return false;
			}
			if (_endnode._attributes.Count <= 0)
			{
				return false;
			}
			return true;
		}
	}

	public bool HasChildNodes
	{
		get
		{
			if (_childnodes == null)
			{
				return false;
			}
			if (_childnodes.Count <= 0)
			{
				return false;
			}
			return true;
		}
	}

	public HtmlAttributeCollection Attributes
	{
		get
		{
			if (!HasAttributes)
			{
				_attributes = new HtmlAttributeCollection(this);
			}
			return _attributes;
		}
	}

	public HtmlAttributeCollection ClosingAttributes
	{
		get
		{
			if (!HasClosingAttributes)
			{
				return new HtmlAttributeCollection(this);
			}
			return _endnode.Attributes;
		}
	}

	static HtmlNode()
	{
		HtmlNodeTypeNameComment = "#comment";
		HtmlNodeTypeNameDocument = "#document";
		HtmlNodeTypeNameText = "#text";
		ElementsFlags = new Hashtable();
		ElementsFlags.Add("script", HtmlElementFlag.CData);
		ElementsFlags.Add("style", HtmlElementFlag.CData);
		ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);
		ElementsFlags.Add("base", HtmlElementFlag.Empty);
		ElementsFlags.Add("link", HtmlElementFlag.Empty);
		ElementsFlags.Add("meta", HtmlElementFlag.Empty);
		ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
		ElementsFlags.Add("hr", HtmlElementFlag.Empty);
		ElementsFlags.Add("col", HtmlElementFlag.Empty);
		ElementsFlags.Add("img", HtmlElementFlag.Empty);
		ElementsFlags.Add("param", HtmlElementFlag.Empty);
		ElementsFlags.Add("embed", HtmlElementFlag.Empty);
		ElementsFlags.Add("frame", HtmlElementFlag.Empty);
		ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
		ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
		ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
		ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
		ElementsFlags.Add("area", HtmlElementFlag.Empty);
		ElementsFlags.Add("input", HtmlElementFlag.Empty);
		ElementsFlags.Add("basefont", HtmlElementFlag.Empty);
		ElementsFlags.Add("form", (HtmlElementFlag)10);
		ElementsFlags.Add("option", HtmlElementFlag.Empty);
		ElementsFlags.Add("br", (HtmlElementFlag)6);
		ElementsFlags.Add("p", (HtmlElementFlag)6);
	}

	public static bool IsClosedElement(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		object obj = ElementsFlags[name.ToLower()];
		if (obj == null)
		{
			return false;
		}
		return ((HtmlElementFlag)obj & HtmlElementFlag.Closed) != 0;
	}

	public static bool CanOverlapElement(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		object obj = ElementsFlags[name.ToLower()];
		if (obj == null)
		{
			return false;
		}
		return ((HtmlElementFlag)obj & HtmlElementFlag.CanOverlap) != 0;
	}

	public static bool IsOverlappedClosingElement(string text)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		if (text.Length <= 4)
		{
			return false;
		}
		if (text[0] != '<' || text[text.Length - 1] != '>' || text[1] != '/')
		{
			return false;
		}
		string name = text.Substring(2, text.Length - 3);
		return CanOverlapElement(name);
	}

	public static bool IsCDataElement(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		object obj = ElementsFlags[name.ToLower()];
		if (obj == null)
		{
			return false;
		}
		return ((HtmlElementFlag)obj & HtmlElementFlag.CData) != 0;
	}

	public static bool IsEmptyElement(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			return true;
		}
		if ('!' == name[0])
		{
			return true;
		}
		if ('?' == name[0])
		{
			return true;
		}
		object obj = ElementsFlags[name.ToLower()];
		if (obj == null)
		{
			return false;
		}
		return ((HtmlElementFlag)obj & HtmlElementFlag.Empty) != 0;
	}

	public static HtmlNode CreateNode(string html)
	{
		HPathDocument hPathDocument = new HPathDocument();
		hPathDocument.LoadHtml(html);
		return hPathDocument.DocumentNode.FirstChild;
	}

	public void CopyFrom(HtmlNode node)
	{
		CopyFrom(node, deep: true);
	}

	public void CopyFrom(HtmlNode node, bool deep)
	{
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		Attributes.RemoveAll();
		if (node.HasAttributes)
		{
			foreach (HtmlAttribute attribute in node.Attributes)
			{
				SetAttributeValue(attribute.Name, attribute.Value);
			}
		}
		if (deep)
		{
			return;
		}
		RemoveAllChildren();
		if (!node.HasChildNodes)
		{
			return;
		}
		foreach (HtmlNode childNode in node.ChildNodes)
		{
			AppendChild(childNode.CloneNode(deep: true));
		}
	}

	internal HtmlNode(HtmlNodeType type, HPathDocument ownerdocument, int index)
	{
		_nodetype = type;
		_ownerdocument = ownerdocument;
		_outerstartindex = index;
		switch (type)
		{
		case HtmlNodeType.Comment:
			_name = HtmlNodeTypeNameComment;
			_endnode = this;
			break;
		case HtmlNodeType.Document:
			_name = HtmlNodeTypeNameDocument;
			_endnode = this;
			break;
		case HtmlNodeType.Text:
			_name = HtmlNodeTypeNameText;
			_endnode = this;
			break;
		}
		if (_ownerdocument._openednodes != null && !Closed && -1 != index)
		{
			_ownerdocument._openednodes.Add(index, this);
		}
		if (-1 == index && type != HtmlNodeType.Comment && type != HtmlNodeType.Text)
		{
			_outerchanged = true;
			_innerchanged = true;
		}
	}

	internal void CloseNode(HtmlNode endnode)
	{
		if (!_ownerdocument.OptionAutoCloseOnEnd && _childnodes != null)
		{
			foreach (HtmlNode childnode in _childnodes)
			{
				if (!childnode.Closed)
				{
					HtmlNode htmlNode = new HtmlNode(NodeType, _ownerdocument, -1);
					htmlNode._endnode = htmlNode;
					childnode.CloseNode(htmlNode);
				}
			}
		}
		if (!Closed)
		{
			_endnode = endnode;
			if (_ownerdocument._openednodes != null)
			{
				_ownerdocument._openednodes.Remove(_outerstartindex);
			}
			HtmlNode htmlNode2 = _ownerdocument._lastnodes[Name] as HtmlNode;
			if (htmlNode2 == this)
			{
				_ownerdocument._lastnodes.Remove(Name);
				_ownerdocument.UpdateLastParentNode();
			}
			if (endnode != this)
			{
				_innerstartindex = _outerstartindex + _outerlength;
				_innerlength = endnode._outerstartindex - _innerstartindex;
				_outerlength = endnode._outerstartindex + endnode._outerlength - _outerstartindex;
			}
		}
	}

	internal string GetId()
	{
		return Attributes["id"]?.Value;
	}

	internal void SetId(string id)
	{
		HtmlAttribute htmlAttribute = Attributes["id"];
		if (htmlAttribute == null)
		{
			htmlAttribute = _ownerdocument.CreateAttribute("id");
		}
		htmlAttribute.Value = id;
		_ownerdocument.SetIdForNode(this, htmlAttribute.Value);
		_outerchanged = true;
	}

	public XPathNavigator CreateNavigator()
	{
		return new HtmlNodeNavigator(_ownerdocument, this);
	}

	public HtmlNode SelectSingleNode(string xpath)
	{
		if (xpath == null)
		{
			throw new ArgumentNullException("xpath");
		}
		HtmlNodeNavigator htmlNodeNavigator = new HtmlNodeNavigator(_ownerdocument, this);
		XPathNodeIterator xPathNodeIterator = htmlNodeNavigator.Select(xpath);
		if (!xPathNodeIterator.MoveNext())
		{
			return null;
		}
		HtmlNodeNavigator htmlNodeNavigator2 = (HtmlNodeNavigator)xPathNodeIterator.Current;
		return htmlNodeNavigator2.CurrentNode;
	}

	public HtmlNodeCollection SelectNodes(string xpath)
	{
		HtmlNodeCollection htmlNodeCollection = new HtmlNodeCollection(null);
		HtmlNodeNavigator htmlNodeNavigator = new HtmlNodeNavigator(_ownerdocument, this);
		XPathNodeIterator xPathNodeIterator = htmlNodeNavigator.Select(xpath);
		while (xPathNodeIterator.MoveNext())
		{
			HtmlNodeNavigator htmlNodeNavigator2 = (HtmlNodeNavigator)xPathNodeIterator.Current;
			htmlNodeCollection.Add(htmlNodeNavigator2.CurrentNode);
		}
		if (htmlNodeCollection.Count == 0)
		{
			return null;
		}
		return htmlNodeCollection;
	}

	public HtmlNode Clone()
	{
		return CloneNode(deep: true);
	}

	public HtmlNode CloneNode(string newName)
	{
		return CloneNode(newName, deep: true);
	}

	public HtmlNode CloneNode(string newName, bool deep)
	{
		if (newName == null)
		{
			throw new ArgumentNullException("newName");
		}
		HtmlNode htmlNode = CloneNode(deep);
		htmlNode._name = newName;
		return htmlNode;
	}

	public HtmlNode CloneNode(bool deep)
	{
		HtmlNode htmlNode = _ownerdocument.CreateNode(_nodetype);
		htmlNode._name = Name;
		switch (_nodetype)
		{
		case HtmlNodeType.Comment:
			((HtmlCommentNode)htmlNode).Comment = ((HtmlCommentNode)this).Comment;
			return htmlNode;
		case HtmlNodeType.Text:
			((HtmlTextNode)htmlNode).Text = ((HtmlTextNode)this).Text;
			return htmlNode;
		default:
			if (HasAttributes)
			{
				foreach (HtmlAttribute attribute in _attributes)
				{
					HtmlAttribute newAttribute = attribute.Clone();
					htmlNode.Attributes.Append(newAttribute);
				}
			}
			if (HasClosingAttributes)
			{
				htmlNode._endnode = _endnode.CloneNode(deep: false);
				foreach (HtmlAttribute attribute2 in _endnode._attributes)
				{
					HtmlAttribute newAttribute2 = attribute2.Clone();
					htmlNode._endnode._attributes.Append(newAttribute2);
				}
			}
			if (!deep)
			{
				return htmlNode;
			}
			if (!HasChildNodes)
			{
				return htmlNode;
			}
			{
				foreach (HtmlNode childnode in _childnodes)
				{
					HtmlNode newChild = childnode.Clone();
					htmlNode.AppendChild(newChild);
				}
				return htmlNode;
			}
		}
	}

	public void RemoveAll()
	{
		RemoveAllChildren();
		if (HasAttributes)
		{
			_attributes.Clear();
		}
		if (_endnode != null && _endnode != this && _endnode._attributes != null)
		{
			_endnode._attributes.Clear();
		}
		_outerchanged = true;
		_innerchanged = true;
	}

	public void RemoveAllChildren()
	{
		if (!HasChildNodes)
		{
			return;
		}
		if (_ownerdocument.OptionUseIdAttribute)
		{
			foreach (HtmlNode childnode in _childnodes)
			{
				_ownerdocument.SetIdForNode(null, childnode.GetId());
			}
		}
		_childnodes.Clear();
		_outerchanged = true;
		_innerchanged = true;
	}

	public HtmlNode RemoveChild(HtmlNode oldChild)
	{
		if (oldChild == null)
		{
			throw new ArgumentNullException("oldChild");
		}
		int num = -1;
		if (_childnodes != null)
		{
			num = _childnodes[oldChild];
		}
		if (num == -1)
		{
			throw new ArgumentException(HPathDocument.HtmlExceptionRefNotChild);
		}
		_childnodes.Remove(num);
		_ownerdocument.SetIdForNode(null, oldChild.GetId());
		_outerchanged = true;
		_innerchanged = true;
		return oldChild;
	}

	public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren)
	{
		if (oldChild == null)
		{
			throw new ArgumentNullException("oldChild");
		}
		if (oldChild._childnodes != null && keepGrandChildren)
		{
			HtmlNode previousSibling = oldChild.PreviousSibling;
			foreach (HtmlNode childnode in oldChild._childnodes)
			{
				InsertAfter(childnode, previousSibling);
			}
		}
		RemoveChild(oldChild);
		_outerchanged = true;
		_innerchanged = true;
		return oldChild;
	}

	public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild)
	{
		if (newChild == null)
		{
			return RemoveChild(oldChild);
		}
		if (oldChild == null)
		{
			return AppendChild(newChild);
		}
		int num = -1;
		if (_childnodes != null)
		{
			num = _childnodes[oldChild];
		}
		if (num == -1)
		{
			throw new ArgumentException(HPathDocument.HtmlExceptionRefNotChild);
		}
		_childnodes.Replace(num, newChild);
		_ownerdocument.SetIdForNode(null, oldChild.GetId());
		_ownerdocument.SetIdForNode(newChild, newChild.GetId());
		_outerchanged = true;
		_innerchanged = true;
		return newChild;
	}

	public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild)
	{
		if (newChild == null)
		{
			throw new ArgumentNullException("newChild");
		}
		if (refChild == null)
		{
			return AppendChild(newChild);
		}
		if (newChild == refChild)
		{
			return newChild;
		}
		int num = -1;
		if (_childnodes != null)
		{
			num = _childnodes[refChild];
		}
		if (num == -1)
		{
			throw new ArgumentException(HPathDocument.HtmlExceptionRefNotChild);
		}
		_childnodes.Insert(num, newChild);
		_ownerdocument.SetIdForNode(newChild, newChild.GetId());
		_outerchanged = true;
		_innerchanged = true;
		return newChild;
	}

	public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild)
	{
		if (newChild == null)
		{
			throw new ArgumentNullException("newChild");
		}
		if (refChild == null)
		{
			return PrependChild(newChild);
		}
		if (newChild == refChild)
		{
			return newChild;
		}
		int num = -1;
		if (_childnodes != null)
		{
			num = _childnodes[refChild];
		}
		if (num == -1)
		{
			throw new ArgumentException(HPathDocument.HtmlExceptionRefNotChild);
		}
		_childnodes.Insert(num + 1, newChild);
		_ownerdocument.SetIdForNode(newChild, newChild.GetId());
		_outerchanged = true;
		_innerchanged = true;
		return newChild;
	}

	public HtmlNode PrependChild(HtmlNode newChild)
	{
		if (newChild == null)
		{
			throw new ArgumentNullException("newChild");
		}
		ChildNodes.Prepend(newChild);
		_ownerdocument.SetIdForNode(newChild, newChild.GetId());
		_outerchanged = true;
		_innerchanged = true;
		return newChild;
	}

	public void PrependChildren(HtmlNodeCollection newChildren)
	{
		if (newChildren == null)
		{
			throw new ArgumentNullException("newChildren");
		}
		foreach (HtmlNode newChild in newChildren)
		{
			PrependChild(newChild);
		}
	}

	public HtmlNode AppendChild(HtmlNode newChild)
	{
		if (newChild == null)
		{
			throw new ArgumentNullException("newChild");
		}
		ChildNodes.Append(newChild);
		_ownerdocument.SetIdForNode(newChild, newChild.GetId());
		_outerchanged = true;
		_innerchanged = true;
		return newChild;
	}

	public void AppendChildren(HtmlNodeCollection newChildren)
	{
		if (newChildren == null)
		{
			throw new ArgumentNullException("newChildrend");
		}
		foreach (HtmlNode newChild in newChildren)
		{
			AppendChild(newChild);
		}
	}

	public string GetAttributeValue(string name, string def)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (!HasAttributes)
		{
			return def;
		}
		HtmlAttribute htmlAttribute = Attributes[name];
		if (htmlAttribute == null)
		{
			return def;
		}
		return htmlAttribute.Value;
	}

	public int GetAttributeValue(string name, int def)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (!HasAttributes)
		{
			return def;
		}
		HtmlAttribute htmlAttribute = Attributes[name];
		if (htmlAttribute == null)
		{
			return def;
		}
		try
		{
			return Convert.ToInt32(htmlAttribute.Value);
		}
		catch
		{
			return def;
		}
	}

	public bool GetAttributeValue(string name, bool def)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (!HasAttributes)
		{
			return def;
		}
		HtmlAttribute htmlAttribute = Attributes[name];
		if (htmlAttribute == null)
		{
			return def;
		}
		try
		{
			return Convert.ToBoolean(htmlAttribute.Value);
		}
		catch
		{
			return def;
		}
	}

	public HtmlAttribute SetAttributeValue(string name, string value)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		HtmlAttribute htmlAttribute = Attributes[name];
		if (htmlAttribute == null)
		{
			return Attributes.Append(_ownerdocument.CreateAttribute(name, value));
		}
		htmlAttribute.Value = value;
		return htmlAttribute;
	}

	internal void WriteAttribute(TextWriter outText, HtmlAttribute att)
	{
		string text;
		if (_ownerdocument.OptionOutputAsXml)
		{
			text = ((!_ownerdocument.OptionOutputUpperCase) ? att.XmlName : att.XmlName.ToUpper());
			outText.Write(" " + text + "=\"" + HPathDocument.HtmlEncode(att.XmlValue) + "\"");
			return;
		}
		text = ((!_ownerdocument.OptionOutputUpperCase) ? att.Name : att.Name.ToUpper());
		if (att.Name.Length >= 4 && att.Name[0] == '<' && att.Name[1] == '%' && att.Name[att.Name.Length - 1] == '>' && att.Name[att.Name.Length - 2] == '%')
		{
			outText.Write(" " + text);
		}
		else if (_ownerdocument.OptionOutputOptimizeAttributeValues)
		{
			if (att.Value.IndexOfAny(new char[4] { '\n', '\r', '\t', ' ' }) < 0)
			{
				outText.Write(" " + text + "=" + att.Value);
				return;
			}
			outText.Write(" " + text + "=\"" + att.Value + "\"");
		}
		else
		{
			outText.Write(" " + text + "=\"" + att.Value + "\"");
		}
	}

	internal static void WriteAttributes(XmlWriter writer, HtmlNode node)
	{
		if (!node.HasAttributes)
		{
			return;
		}
		foreach (HtmlAttribute value in node.Attributes._hashitems.Values)
		{
			writer.WriteAttributeString(value.XmlName, value.Value);
		}
	}

	internal void WriteAttributes(TextWriter outText, bool closing)
	{
		if (_ownerdocument.OptionOutputAsXml)
		{
			if (_attributes == null)
			{
				return;
			}
			{
				foreach (HtmlAttribute value in _attributes._hashitems.Values)
				{
					WriteAttribute(outText, value);
				}
				return;
			}
		}
		if (!closing)
		{
			if (_attributes != null)
			{
				foreach (HtmlAttribute attribute in _attributes)
				{
					WriteAttribute(outText, attribute);
				}
			}
			if (!_ownerdocument.OptionAddDebuggingAttributes)
			{
				return;
			}
			WriteAttribute(outText, _ownerdocument.CreateAttribute("_closed", Closed.ToString()));
			WriteAttribute(outText, _ownerdocument.CreateAttribute("_children", ChildNodes.Count.ToString()));
			int num = 0;
			{
				foreach (HtmlNode childNode in ChildNodes)
				{
					WriteAttribute(outText, _ownerdocument.CreateAttribute("_child_" + num, childNode.Name));
					num++;
				}
				return;
			}
		}
		if (_endnode == null || _endnode._attributes == null || _endnode == this)
		{
			return;
		}
		foreach (HtmlAttribute attribute2 in _endnode._attributes)
		{
			WriteAttribute(outText, attribute2);
		}
		if (_ownerdocument.OptionAddDebuggingAttributes)
		{
			WriteAttribute(outText, _ownerdocument.CreateAttribute("_closed", Closed.ToString()));
			WriteAttribute(outText, _ownerdocument.CreateAttribute("_children", ChildNodes.Count.ToString()));
		}
	}

	internal static string GetXmlComment(HtmlCommentNode comment)
	{
		string comment2 = comment.Comment;
		return comment2.Substring(4, comment2.Length - 7).Replace("--", " - -");
	}

	public void WriteTo(TextWriter outText)
	{
		switch (_nodetype)
		{
		case HtmlNodeType.Comment:
		{
			string comment = ((HtmlCommentNode)this).Comment;
			if (_ownerdocument.OptionOutputAsXml)
			{
				outText.Write("<!--" + GetXmlComment((HtmlCommentNode)this) + " -->");
			}
			else
			{
				outText.Write(comment);
			}
			break;
		}
		case HtmlNodeType.Document:
			if (_ownerdocument.OptionOutputAsXml)
			{
				outText.Write("<?xml version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().BodyName + "\"?>");
				if (_ownerdocument.DocumentNode.HasChildNodes)
				{
					int num = _ownerdocument.DocumentNode._childnodes.Count;
					if (num > 0)
					{
						HtmlNode xmlDeclaration = _ownerdocument.GetXmlDeclaration();
						if (xmlDeclaration != null)
						{
							num--;
						}
						if (num > 1)
						{
							if (_ownerdocument.OptionOutputUpperCase)
							{
								outText.Write("<SPAN>");
								WriteContentTo(outText);
								outText.Write("</SPAN>");
							}
							else
							{
								outText.Write("<span>");
								WriteContentTo(outText);
								outText.Write("</span>");
							}
							break;
						}
					}
				}
			}
			WriteContentTo(outText);
			break;
		case HtmlNodeType.Text:
		{
			string comment = ((HtmlTextNode)this).Text;
			if (_ownerdocument.OptionOutputAsXml)
			{
				outText.Write(HPathDocument.HtmlEncode(comment));
			}
			else
			{
				outText.Write(comment);
			}
			break;
		}
		case HtmlNodeType.Element:
		{
			string text = ((!_ownerdocument.OptionOutputUpperCase) ? Name : Name.ToUpper());
			if (_ownerdocument.OptionOutputAsXml)
			{
				if (text.Length <= 0 || text[0] == '?' || text.Trim().Length == 0)
				{
					break;
				}
				text = HtmlAttribute.GetXmlName(text);
			}
			outText.Write("<" + text);
			WriteAttributes(outText, closing: false);
			if (!HasChildNodes)
			{
				if (IsEmptyElement(Name))
				{
					if (_ownerdocument.OptionWriteEmptyNodes || _ownerdocument.OptionOutputAsXml)
					{
						outText.Write(" />");
						break;
					}
					if (Name.Length > 0 && Name[0] == '?')
					{
						outText.Write("?");
					}
					outText.Write(">");
				}
				else
				{
					outText.Write("></" + text + ">");
				}
				break;
			}
			outText.Write(">");
			bool flag = false;
			if (_ownerdocument.OptionOutputAsXml && IsCDataElement(Name))
			{
				flag = true;
				outText.Write("\r\n//<![CDATA[\r\n");
			}
			if (flag)
			{
				if (HasChildNodes)
				{
					ChildNodes[0].WriteTo(outText);
				}
				outText.Write("\r\n//]]>//\r\n");
			}
			else
			{
				WriteContentTo(outText);
			}
			outText.Write("</" + text);
			if (!_ownerdocument.OptionOutputAsXml)
			{
				WriteAttributes(outText, closing: true);
			}
			outText.Write(">");
			break;
		}
		}
	}

	public void WriteTo(XmlWriter writer)
	{
		switch (_nodetype)
		{
		case HtmlNodeType.Comment:
			writer.WriteComment(GetXmlComment((HtmlCommentNode)this));
			break;
		case HtmlNodeType.Document:
			writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().BodyName + "\"");
			if (!HasChildNodes)
			{
				break;
			}
			{
				foreach (HtmlNode childNode in ChildNodes)
				{
					childNode.WriteTo(writer);
				}
				break;
			}
		case HtmlNodeType.Text:
		{
			string text = ((HtmlTextNode)this).Text;
			writer.WriteString(text);
			break;
		}
		case HtmlNodeType.Element:
		{
			string localName = ((!_ownerdocument.OptionOutputUpperCase) ? Name : Name.ToUpper());
			writer.WriteStartElement(localName);
			WriteAttributes(writer, this);
			if (HasChildNodes)
			{
				foreach (HtmlNode childNode2 in ChildNodes)
				{
					childNode2.WriteTo(writer);
				}
			}
			writer.WriteEndElement();
			break;
		}
		}
	}

	public void WriteContentTo(TextWriter outText)
	{
		if (_childnodes == null)
		{
			return;
		}
		foreach (HtmlNode childnode in _childnodes)
		{
			childnode.WriteTo(outText);
		}
	}

	public string WriteTo()
	{
		StringWriter stringWriter = new StringWriter();
		WriteTo(stringWriter);
		stringWriter.Flush();
		return stringWriter.ToString();
	}

	public string WriteContentTo()
	{
		StringWriter stringWriter = new StringWriter();
		WriteContentTo(stringWriter);
		stringWriter.Flush();
		return stringWriter.ToString();
	}
}
