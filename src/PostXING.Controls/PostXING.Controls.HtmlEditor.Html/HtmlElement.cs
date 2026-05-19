using System;
using System.ComponentModel;
using System.Drawing;

namespace PostXING.Controls.HtmlEditor.Html;

[DesignOnly(true)]
public class HtmlElement
{
	private NativeMethods.IHTMLElement peer;

	private HtmlControl owner;

	[Browsable(false)]
	public string InnerHtml
	{
		get
		{
			try
			{
				return peer.GetInnerHTML();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
		set
		{
			try
			{
				peer.SetInnerHTML(value);
			}
			catch (Exception)
			{
			}
		}
	}

	[Browsable(false)]
	public string OuterHtml
	{
		get
		{
			try
			{
				return peer.GetOuterHTML();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
		set
		{
			try
			{
				peer.SetOuterHTML(value);
			}
			catch (Exception)
			{
			}
		}
	}

	[Browsable(false)]
	public string Name
	{
		get
		{
			try
			{
				return peer.GetTagName();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}

	internal NativeMethods.IHTMLElement Peer => peer;

	public HtmlElement Parent
	{
		get
		{
			NativeMethods.IHTMLElement parentElement = peer.GetParentElement();
			return new HtmlElement(parentElement, owner);
		}
	}

	internal HtmlElement(NativeMethods.IHTMLElement peer, HtmlControl owner)
	{
		this.peer = peer;
		this.owner = owner;
	}

	public object GetAttribute(string attribute)
	{
		try
		{
			object[] array = new object[1];
			peer.GetAttribute(attribute, 0, array);
			object obj = array[0];
			if (obj is DBNull)
			{
				obj = null;
			}
			return obj;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public HtmlElement GetChild(int index)
	{
		NativeMethods.IHTMLElementCollection iHTMLElementCollection = (NativeMethods.IHTMLElementCollection)peer.GetChildren();
		NativeMethods.IHTMLElement iHTMLElement = (NativeMethods.IHTMLElement)iHTMLElementCollection.Item(null, index);
		return new HtmlElement(iHTMLElement, owner);
	}

	public HtmlElement GetChild(string name)
	{
		NativeMethods.IHTMLElementCollection iHTMLElementCollection = (NativeMethods.IHTMLElementCollection)peer.GetChildren();
		NativeMethods.IHTMLElement iHTMLElement = (NativeMethods.IHTMLElement)iHTMLElementCollection.Item(name, null);
		return new HtmlElement(iHTMLElement, owner);
	}

	protected string GetRelativeUrl(string absoluteUrl)
	{
		if (absoluteUrl == null || absoluteUrl.Length == 0)
		{
			return string.Empty;
		}
		string text = absoluteUrl;
		if (owner != null)
		{
			string url = owner.Url;
			if (url.Length != 0)
			{
				try
				{
					Uri uri = new Uri(url);
					Uri uri2 = new Uri(text);
					text = uri.MakeRelativeUri(uri2).ToString();
				}
				catch
				{
				}
			}
		}
		return text;
	}

	protected internal string GetStringAttribute(string attribute)
	{
		return GetStringAttribute(attribute, string.Empty);
	}

	protected internal string GetStringAttribute(string attribute, string defaultValue)
	{
		object attribute2 = GetAttribute(attribute);
		if (attribute2 == null)
		{
			return defaultValue;
		}
		if (attribute2 is string)
		{
			return (string)attribute2;
		}
		return defaultValue;
	}

	public void RemoveAttribute(string attribute)
	{
		try
		{
			peer.RemoveAttribute(attribute, 0);
		}
		catch (Exception)
		{
		}
	}

	public void SetAttribute(string attribute, object value)
	{
		try
		{
			peer.SetAttribute(attribute, value, 0);
		}
		catch (Exception)
		{
		}
	}

	protected internal void SetBooleanAttribute(string attribute, bool value)
	{
		if (value)
		{
			SetAttribute(attribute, true);
		}
		else
		{
			RemoveAttribute(attribute);
		}
	}

	protected internal void SetColorAttribute(string attribute, Color value)
	{
		if (value.IsEmpty)
		{
			RemoveAttribute(attribute);
		}
		else
		{
			SetAttribute(attribute, ColorTranslator.ToHtml(value));
		}
	}

	protected internal void SetEnumAttribute(string attribute, Enum value, Enum defaultValue)
	{
		if (value.Equals(defaultValue))
		{
			RemoveAttribute(attribute);
		}
		else
		{
			SetAttribute(attribute, value.ToString());
		}
	}

	protected internal void SetIntegerAttribute(string attribute, int value, int defaultValue)
	{
		if (value == defaultValue)
		{
			RemoveAttribute(attribute);
		}
		else
		{
			SetAttribute(attribute, value);
		}
	}

	protected internal void SetStringAttribute(string attribute, string value)
	{
		SetStringAttribute(attribute, value, string.Empty);
	}

	protected internal void SetStringAttribute(string attribute, string value, string defaultValue)
	{
		if (value == null || value.Equals(defaultValue))
		{
			RemoveAttribute(attribute);
		}
		else
		{
			SetAttribute(attribute, value);
		}
	}

	public override string ToString()
	{
		if (peer != null)
		{
			try
			{
				return "<" + peer.GetTagName() + ">";
			}
			catch
			{
			}
		}
		return string.Empty;
	}
}
