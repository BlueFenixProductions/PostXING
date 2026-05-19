using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlSelection
{
	private static readonly string DesignTimeLockAttribute = "Design_Time_Lock";

	private HtmlControl control;

	private NativeMethods.IHTMLDocument2 document;

	private HtmlSelectionType type;

	private int selectionLength;

	private string text;

	private object selection;

	private ArrayList items;

	private ArrayList elements;

	private bool sameParentValid;

	private int maxZIndex;

	public bool CanAlign
	{
		get
		{
			if (items.Count < 2)
			{
				return false;
			}
			if (type == HtmlSelectionType.ElementSelection)
			{
				foreach (NativeMethods.IHTMLElement item in items)
				{
					if (!IsElement2DPositioned(item))
					{
						return false;
					}
					if (IsElementLocked(item))
					{
						return false;
					}
				}
				if (!SameParent)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public bool CanMatchSize
	{
		get
		{
			if (items.Count < 2)
			{
				return false;
			}
			if (type == HtmlSelectionType.ElementSelection)
			{
				foreach (NativeMethods.IHTMLElement item in items)
				{
					if (IsElementLocked(item))
					{
						return false;
					}
				}
				if (!SameParent)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public bool CanChangeZIndex
	{
		get
		{
			if (items.Count == 0)
			{
				return false;
			}
			if (type == HtmlSelectionType.ElementSelection)
			{
				foreach (NativeMethods.IHTMLElement item in items)
				{
					if (!IsElement2DPositioned(item))
					{
						return false;
					}
				}
				if (!SameParent)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public bool CanWrapSelection
	{
		get
		{
			if (selectionLength != 0 && Type == HtmlSelectionType.TextSelection)
			{
				return true;
			}
			return false;
		}
	}

	protected HtmlControl Control => control;

	public ICollection Elements
	{
		get
		{
			if (elements == null)
			{
				elements = new ArrayList();
				foreach (NativeMethods.IHTMLElement item in items)
				{
					object obj = CreateElementWrapper(item);
					if (obj != null)
					{
						elements.Add(obj);
					}
				}
			}
			return elements;
		}
	}

	internal ICollection Items => items;

	public int Length => selectionLength;

	private bool SameParent
	{
		get
		{
			if (!sameParentValid)
			{
				IntPtr intPtr = NativeMethods.NullIntPtr;
				foreach (NativeMethods.IHTMLElement item in items)
				{
					NativeMethods.IHTMLElement parentElement = item.GetParentElement();
					IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(parentElement);
					if (intPtr == NativeMethods.NullIntPtr)
					{
						intPtr = iUnknownForObject;
						continue;
					}
					if (intPtr != iUnknownForObject)
					{
						Marshal.Release(iUnknownForObject);
						if (intPtr != NativeMethods.NullIntPtr)
						{
							Marshal.Release(intPtr);
						}
						sameParentValid = false;
						return sameParentValid;
					}
					Marshal.Release(iUnknownForObject);
				}
				if (intPtr != NativeMethods.NullIntPtr)
				{
					Marshal.Release(intPtr);
				}
				sameParentValid = true;
			}
			return sameParentValid;
		}
	}

	protected internal object Selection => selection;

	public string Text
	{
		get
		{
			if (type == HtmlSelectionType.TextSelection)
			{
				return text;
			}
			return null;
		}
	}

	public HtmlSelectionType Type => type;

	public bool CanRemoveHyperlink => control.IsEnabled(2125);

	public event EventHandler SelectionChanged;

	public HtmlSelection(HtmlControl control)
	{
		this.control = control;
		maxZIndex = 99;
	}

	public void ClearSelection()
	{
		control.Execute(2007);
	}

	internal object CreateElementWrapper(NativeMethods.IHTMLElement element)
	{
		return new HtmlElement(element, control);
	}

	public string GetOuterHtml()
	{
		string result = string.Empty;
		try
		{
			result = ((NativeMethods.IHTMLElement)items[0]).GetOuterHTML();
			result = ((NativeMethods.IHTMLElement)items[0]).GetOuterHTML();
		}
		catch
		{
		}
		return result;
	}

	public ArrayList GetParentHierarchy(object o)
	{
		NativeMethods.IHTMLElement htmlElement = GetHtmlElement(o);
		if (htmlElement == null)
		{
			return null;
		}
		string text = htmlElement.GetTagName().ToLower();
		if (text.Equals("body"))
		{
			return null;
		}
		ArrayList arrayList = new ArrayList();
		htmlElement = htmlElement.GetParentElement();
		while (htmlElement != null && !htmlElement.GetTagName().ToLower().Equals("body"))
		{
			HtmlElement htmlElement2 = new HtmlElement(htmlElement, control);
			if (IsSelectableElement(htmlElement2))
			{
				arrayList.Add(htmlElement2);
			}
			htmlElement = htmlElement.GetParentElement();
		}
		if (htmlElement != null)
		{
			HtmlElement htmlElement3 = new HtmlElement(htmlElement, control);
			if (IsSelectableElement(htmlElement3))
			{
				arrayList.Add(htmlElement3);
			}
		}
		return arrayList;
	}

	internal NativeMethods.IHTMLElement GetHtmlElement(object o)
	{
		if (o is HtmlElement)
		{
			return ((HtmlElement)o).Peer;
		}
		return null;
	}

	private bool IsElement2DPositioned(NativeMethods.IHTMLElement element)
	{
		NativeMethods.IHTMLElement2 iHTMLElement = (NativeMethods.IHTMLElement2)element;
		NativeMethods.IHTMLCurrentStyle currentStyle = iHTMLElement.GetCurrentStyle();
		string position = currentStyle.GetPosition();
		if (position != null)
		{
			return position.ToLower() == "absolute";
		}
		return false;
	}

	private bool IsElementLocked(NativeMethods.IHTMLElement element)
	{
		object[] array = new object[1];
		element.GetAttribute(DesignTimeLockAttribute, 0, array);
		if (array[0] == null)
		{
			NativeMethods.IHTMLStyle style = element.GetStyle();
			array[0] = style.GetAttribute(DesignTimeLockAttribute, 0);
		}
		if (array[0] != null)
		{
			return array[0] is string;
		}
		return false;
	}

	protected virtual bool IsSelectableElement(HtmlElement element)
	{
		_ = element.Name.ToLower() != "body";
		return true;
	}

	protected virtual void OnSelectionChanged(EventArgs e)
	{
		if (this.SelectionChanged != null)
		{
			this.SelectionChanged(this, e);
		}
	}

	public bool SelectElement(object o)
	{
		ArrayList arrayList = new ArrayList(1);
		arrayList.Add(o);
		return SelectElements(arrayList);
	}

	public bool SelectElements(ICollection elements)
	{
		NativeMethods.IHTMLElement body = control.HtmlDocument.GetBody();
		NativeMethods.IHTMLTextContainer iHTMLTextContainer = body as NativeMethods.IHTMLTextContainer;
		object obj = iHTMLTextContainer.createControlRange();
		if (!(obj is NativeMethods.IHtmlControlRange htmlControlRange))
		{
			return false;
		}
		if (!(obj is NativeMethods.IHtmlControlRange2 htmlControlRange2))
		{
			return false;
		}
		int num = 0;
		foreach (object element in elements)
		{
			NativeMethods.IHTMLElement htmlElement = GetHtmlElement(element);
			if (htmlElement == null)
			{
				return false;
			}
			num = htmlControlRange2.addElement(htmlElement);
			if (num != 0)
			{
				break;
			}
		}
		if (num == 0)
		{
			htmlControlRange.Select();
		}
		else
		{
			NativeMethods.IHtmlBodyElement htmlBodyElement = (NativeMethods.IHtmlBodyElement)body;
			NativeMethods.IHTMLTxtRange iHTMLTxtRange = htmlBodyElement.createTextRange();
			if (iHTMLTxtRange != null)
			{
				foreach (object element2 in elements)
				{
					try
					{
						NativeMethods.IHTMLElement htmlElement2 = GetHtmlElement(element2);
						if (htmlElement2 == null)
						{
							return false;
						}
						iHTMLTxtRange.MoveToElementText(htmlElement2);
					}
					catch
					{
					}
				}
				iHTMLTxtRange.Select();
			}
		}
		return true;
	}

	public void SetOuterHtml(string outerHtml)
	{
		NativeMethods.IHTMLElement iHTMLElement = (NativeMethods.IHTMLElement)items[0];
		iHTMLElement.SetOuterHTML(outerHtml);
	}

	public bool SynchronizeSelection()
	{
		if (document == null)
		{
			document = control.HtmlDocument;
		}
		NativeMethods.IHTMLSelectionObject iHTMLSelectionObject = document.GetSelection();
		object obj = null;
		try
		{
			obj = iHTMLSelectionObject.CreateRange();
		}
		catch
		{
		}
		ArrayList arrayList = items;
		HtmlSelectionType htmlSelectionType = type;
		int num = selectionLength;
		type = HtmlSelectionType.Empty;
		selectionLength = 0;
		if (obj != null)
		{
			selection = obj;
			items = new ArrayList();
			if (obj is NativeMethods.IHTMLTxtRange)
			{
				NativeMethods.IHTMLTxtRange iHTMLTxtRange = (NativeMethods.IHTMLTxtRange)obj;
				NativeMethods.IHTMLElement iHTMLElement = iHTMLTxtRange.ParentElement();
				if (IsSelectableElement(new HtmlElement(iHTMLElement, control)) && iHTMLElement != null)
				{
					text = iHTMLTxtRange.GetText();
					if (text != null)
					{
						selectionLength = text.Length;
					}
					else
					{
						selectionLength = 0;
					}
					type = HtmlSelectionType.TextSelection;
					items.Add(iHTMLElement);
				}
			}
			else if (obj is NativeMethods.IHtmlControlRange)
			{
				NativeMethods.IHtmlControlRange htmlControlRange = (NativeMethods.IHtmlControlRange)obj;
				int length = htmlControlRange.GetLength();
				if (length > 0)
				{
					type = HtmlSelectionType.ElementSelection;
					for (int i = 0; i < length; i++)
					{
						NativeMethods.IHTMLElement value = htmlControlRange.Item(i);
						items.Add(value);
					}
					selectionLength = length;
				}
			}
		}
		sameParentValid = false;
		bool flag = false;
		if (type != htmlSelectionType)
		{
			flag = true;
		}
		else if (selectionLength != num)
		{
			flag = true;
		}
		else if (items != null)
		{
			for (int j = 0; j < items.Count; j++)
			{
				if (items[j] != arrayList[j])
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			elements = null;
			OnSelectionChanged(EventArgs.Empty);
			return true;
		}
		return false;
	}

	public void ToggleAbsolutePosition()
	{
		control.Execute(2397, new object[1] { !control.IsChecked(2397) });
		SynchronizeSelection();
		if (type != HtmlSelectionType.ElementSelection)
		{
			return;
		}
		foreach (NativeMethods.IHTMLElement item in items)
		{
			item.GetStyle().SetZIndex(++maxZIndex);
		}
	}

	public void ToggleLock()
	{
		foreach (NativeMethods.IHTMLElement item in items)
		{
			NativeMethods.IHTMLStyle style = item.GetStyle();
			if (IsElementLocked(item))
			{
				item.RemoveAttribute(DesignTimeLockAttribute, 0);
				style.RemoveAttribute(DesignTimeLockAttribute, 0);
			}
			else
			{
				item.SetAttribute(DesignTimeLockAttribute, "true", 0);
				style.SetAttribute(DesignTimeLockAttribute, "true", 0);
			}
		}
	}

	public void WrapSelection(string tag)
	{
		WrapSelection(tag, null);
	}

	public void WrapSelection(string elementName, IDictionary attributes)
	{
		string text = string.Empty;
		if (attributes != null)
		{
			foreach (string key in attributes.Keys)
			{
				object obj = text;
				text = string.Concat(obj, key, "=\"", attributes[key], "\" ");
			}
		}
		SynchronizeSelection();
		if (type == HtmlSelectionType.TextSelection)
		{
			NativeMethods.IHTMLTxtRange iHTMLTxtRange = (NativeMethods.IHTMLTxtRange)Selection;
			string text3 = iHTMLTxtRange.GetHtmlText();
			if (text3 == null)
			{
				text3 = string.Empty;
			}
			string html = "<" + elementName + " " + text + ">" + text3 + "</" + elementName + ">";
			iHTMLTxtRange.PasteHTML(html);
		}
	}

	public void WrapSelectionInDiv()
	{
		WrapSelection("div");
	}

	public void WrapSelectionInSpan()
	{
		WrapSelection("span");
	}

	public void WrapSelectionInBlockQuote()
	{
		WrapSelection("blockquote");
	}

	public void WrapSelectionInHyperlink(string url)
	{
		control.Execute(2124, new object[1] { url });
	}

	public void RemoveHyperlink()
	{
		control.Execute(2125);
	}
}
