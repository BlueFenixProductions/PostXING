using System;
using System.Text;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlAttribute : IComparable
{
	internal int _line;

	internal int _lineposition;

	internal int _streamposition;

	internal int _namestartindex;

	internal int _namelength;

	internal int _valuestartindex;

	internal int _valuelength;

	internal HPathDocument _ownerdocument;

	internal HtmlNode _ownernode;

	internal string _name;

	internal string _value;

	internal string XmlName => GetXmlName(Name);

	internal string XmlValue => Value;

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
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_name = value.ToLower();
			if (_ownernode != null)
			{
				_ownernode._innerchanged = true;
				_ownernode._outerchanged = true;
			}
		}
	}

	public string Value
	{
		get
		{
			if (_value == null)
			{
				_value = _ownerdocument._text.Substring(_valuestartindex, _valuelength);
			}
			return _value;
		}
		set
		{
			_value = value;
			if (_ownernode != null)
			{
				_ownernode._innerchanged = true;
				_ownernode._outerchanged = true;
			}
		}
	}

	public int Line => _line;

	public int LinePosition => _lineposition;

	public int StreamPosition => _streamposition;

	public HtmlNode OwnerNode => _ownernode;

	public HPathDocument OwnerDocument => _ownerdocument;

	internal HtmlAttribute(HPathDocument ownerdocument)
	{
		_ownerdocument = ownerdocument;
	}

	public HtmlAttribute Clone()
	{
		HtmlAttribute htmlAttribute = new HtmlAttribute(_ownerdocument);
		htmlAttribute.Name = Name;
		htmlAttribute.Value = Value;
		return htmlAttribute;
	}

	public int CompareTo(object obj)
	{
		if (!(obj is HtmlAttribute htmlAttribute))
		{
			throw new ArgumentException("obj");
		}
		return Name.CompareTo(htmlAttribute.Name);
	}

	internal static string GetXmlName(string name)
	{
		string text = string.Empty;
		bool flag = true;
		for (int i = 0; i < name.Length; i++)
		{
			if ((name[i] >= 'a' && name[i] <= 'z') || (name[i] >= '0' && name[i] <= '9') || name[i] == '_' || name[i] == '-' || name[i] == '.')
			{
				text += name[i];
				continue;
			}
			flag = false;
			byte[] bytes = Encoding.UTF8.GetBytes(new char[1] { name[i] });
			for (int j = 0; j < bytes.Length; j++)
			{
				text += bytes[j].ToString("x2");
			}
			text += "_";
		}
		if (flag)
		{
			return text;
		}
		return "_" + text;
	}
}
