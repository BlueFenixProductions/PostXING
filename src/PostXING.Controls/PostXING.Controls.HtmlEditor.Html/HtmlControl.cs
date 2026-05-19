using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlControl : Control
{
	private static readonly object readyStateCompleteEvent = new object();

	private bool scriptEnabled;

	private bool firstActivation;

	private bool isReady;

	private bool isCreated;

	private bool desiredLoad;

	private string desiredContent;

	private string desiredUrl;

	private bool absolutePositioningEnabled;

	private bool absolutePositioningDesired;

	private bool multipleSelectionEnabled;

	private bool multipleSelectionDesired;

	private bool focusDesired;

	private string url;

	private object scriptObject;

	private HtmlSite site;

	private static IDictionary urlMap;

	private bool isDesignMode;

	private bool designModeDesired;

	private HtmlSelection selection;

	private HtmlTextFormatting textFormatting;

	private NativeMethods.IPersistStreamInit persistStream;

	private NativeMethods.IOleUndoManager undoManager;

	private NativeMethods.IHTMLEditServices editServices;

	protected bool IsCreated => isCreated;

	public bool IsReady => isReady;

	internal NativeMethods.IHTMLDocument2 HtmlDocument => site.Document;

	internal NativeMethods.IOleCommandTarget CommandTarget => site.CommandTarget;

	public bool ScriptEnabled
	{
		get
		{
			return scriptEnabled;
		}
		set
		{
			scriptEnabled = value;
		}
	}

	public object ScriptObject
	{
		get
		{
			return scriptObject;
		}
		set
		{
			scriptObject = value;
		}
	}

	public virtual string Url => url;

	internal static IDictionary UrlMap
	{
		get
		{
			if (urlMap == null)
			{
				urlMap = new HybridDictionary(caseInsensitive: true);
			}
			return urlMap;
		}
	}

	public bool CanPrint => IsEnabled(27);

	public bool CanPrintPreview => IsEnabled(2003);

	public bool CanCopy => IsEnabled(15);

	public bool CanCut => IsEnabled(16);

	public bool CanPaste => IsEnabled(26);

	public bool CanDelete => IsEnabled(17);

	public bool CanRedo => IsEnabled(29);

	public string RedoDescription
	{
		get
		{
			if (!CanRedo)
			{
				return string.Empty;
			}
			return UndoManager.GetLastRedoDescription();
		}
	}

	public bool CanUndo => IsEnabled(43);

	public string UndoDescription
	{
		get
		{
			if (!CanUndo)
			{
				return string.Empty;
			}
			return UndoManager.GetLastUndoDescription();
		}
	}

	public bool CanSelectAll => IsEnabled(31);

	public bool AbsolutePositioningEnabled
	{
		get
		{
			return absolutePositioningEnabled;
		}
		set
		{
			absolutePositioningDesired = value;
			if (IsCreated)
			{
				absolutePositioningEnabled = value;
				object[] arguments = new object[1] { absolutePositioningEnabled };
				Execute(2394, arguments);
			}
		}
	}

	public bool IsDesignMode
	{
		get
		{
			return isDesignMode;
		}
		set
		{
			if (isDesignMode != value)
			{
				if (!IsCreated)
				{
					designModeDesired = value;
					return;
				}
				isDesignMode = value;
				HtmlDocument.SetDesignMode(isDesignMode ? "on" : "off");
			}
		}
	}

	private NativeMethods.IHTMLEditServices MSHTMLEditServices
	{
		get
		{
			if (editServices == null)
			{
				NativeMethods.IServiceProvider serviceProvider = HtmlDocument as NativeMethods.IServiceProvider;
				Guid sid = new Guid(810612729u, 39093, 4559, 187, 130, 0, 170, 0, 189, 206, 11);
				Guid iid = typeof(NativeMethods.IHTMLEditServices).GUID;
				IntPtr service = NativeMethods.NullIntPtr;
				if (serviceProvider.QueryService(ref sid, ref iid, out service) == 0 && service != NativeMethods.NullIntPtr)
				{
					editServices = (NativeMethods.IHTMLEditServices)Marshal.GetObjectForIUnknown(service);
					Marshal.Release(service);
				}
			}
			return editServices;
		}
	}

	public virtual bool IsDirty
	{
		get
		{
			if (IsDesignMode && IsReady && persistStream != null && persistStream.IsDirty() == 0)
			{
				return true;
			}
			return false;
		}
		set
		{
			if (IsReady)
			{
				Execute(2342, new object[1] { value });
			}
		}
	}

	public bool MultipleSelectionEnabled
	{
		get
		{
			return multipleSelectionEnabled;
		}
		set
		{
			multipleSelectionDesired = value;
			if (IsReady)
			{
				multipleSelectionEnabled = value;
				object[] pvaIn = new object[1] { multipleSelectionEnabled };
				CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 2393, 0, pvaIn, null);
			}
		}
	}

	private NativeMethods.IOleUndoManager UndoManager
	{
		get
		{
			if (undoManager == null)
			{
				NativeMethods.IServiceProvider serviceProvider = HtmlDocument as NativeMethods.IServiceProvider;
				Guid iid = typeof(NativeMethods.IOleUndoManager).GUID;
				Guid sid = typeof(NativeMethods.IOleUndoManager).GUID;
				IntPtr service = NativeMethods.NullIntPtr;
				if (serviceProvider.QueryService(ref sid, ref iid, out service) == 0 && service != NativeMethods.NullIntPtr)
				{
					undoManager = (NativeMethods.IOleUndoManager)Marshal.GetObjectForIUnknown(service);
					Marshal.Release(service);
				}
			}
			return undoManager;
		}
	}

	public bool CanInsertHyperlink
	{
		get
		{
			if ((Selection.Type == HtmlSelectionType.TextSelection || Selection.Type == HtmlSelectionType.Empty) && Selection.Length == 0)
			{
				return CanInsertHtml;
			}
			return IsEnabled(2124);
		}
	}

	public bool CanInsertHtml
	{
		get
		{
			if (Selection.Type == HtmlSelectionType.ElementSelection)
			{
				NativeMethods.IHtmlControlRange htmlControlRange = (NativeMethods.IHtmlControlRange)Selection.Selection;
				int length = htmlControlRange.GetLength();
				if (length == 1)
				{
					NativeMethods.IHTMLElement iHTMLElement = htmlControlRange.Item(0);
					if (string.Compare(iHTMLElement.GetTagName(), "div", ignoreCase: true) == 0 || string.Compare(iHTMLElement.GetTagName(), "td", ignoreCase: true) == 0)
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}
	}

	public HtmlSelection Selection
	{
		get
		{
			if (selection == null)
			{
				selection = new HtmlSelection(this);
			}
			return selection;
		}
	}

	public HtmlTextFormatting TextFormatting
	{
		get
		{
			if (textFormatting == null)
			{
				textFormatting = new HtmlTextFormatting(this);
			}
			return textFormatting;
		}
	}

	public event EventHandler ReadyStateComplete
	{
		add
		{
			base.Events.AddHandler(readyStateCompleteEvent, value);
		}
		remove
		{
			base.Events.RemoveHandler(readyStateCompleteEvent, value);
		}
	}

	public HtmlControl()
	{
		firstActivation = true;
		base.TabStop = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && url != null)
		{
			UrlMap[url] = null;
		}
		base.Dispose(disposing);
	}

	public new void Focus()
	{
		OnGotFocus(EventArgs.Empty);
	}

	public void SetCaretPositionToEnd()
	{
		SelectAll();
		Selection.SynchronizeSelection();
		if (Selection.Selection is NativeMethods.IHTMLTxtRange iHTMLTxtRange)
		{
			iHTMLTxtRange.Collapse(Start: false);
			iHTMLTxtRange.Select();
		}
	}

	internal object Execute(int command)
	{
		return Execute(command, null);
	}

	internal object Execute(int command, object[] arguments)
	{
		object[] array = new object[1];
		object[] array2 = array;
		if (CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, command, 2, arguments, array2) != 0)
		{
			throw new Exception("Execution of MSHTML command ID '" + command + "' failed.");
		}
		return array2[0];
	}

	internal object ExecuteWithUserInterface(int command, object[] arguments)
	{
		object[] array = new object[1];
		object[] array2 = array;
		if (CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, command, 1, arguments, array2) != 0)
		{
			throw new Exception("Execution of MSHTML command ID '" + command + "' failed.");
		}
		return array2[0];
	}

	internal bool IsEnabled(int commandId)
	{
		return (GetCommandInfo(commandId) & 1) != 0;
	}

	internal bool IsChecked(int commandId)
	{
		return (GetCommandInfo(commandId) & 2) != 0;
	}

	internal int GetCommandInfo(int commandId)
	{
		NativeMethods.tagOLECMD tagOLECMD = new NativeMethods.tagOLECMD();
		tagOLECMD.cmdID = commandId;
		CommandTarget.QueryStatus(ref NativeMethods.Guid_MSHTML, 1, tagOLECMD, 0);
		return tagOLECMD.cmdf >> 1;
	}

	public HtmlElement GetElementByID(string id)
	{
		NativeMethods.IHTMLElement body = site.Document.GetBody();
		NativeMethods.IHTMLElementCollection iHTMLElementCollection = (NativeMethods.IHTMLElementCollection)body.GetAll();
		NativeMethods.IHTMLElement iHTMLElement = (NativeMethods.IHTMLElement)iHTMLElementCollection.Item(id, 0);
		if (iHTMLElement == null)
		{
			return null;
		}
		return new HtmlElement(iHTMLElement, this);
	}

	public string GetBodyHtml()
	{
		NativeMethods.IHTMLElement body = site.Document.GetBody();
		return body.GetInnerHTML();
	}

	public void LoadHtml(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("LoadHtml : You must specify a non-null stream for content");
		}
		StreamReader streamReader = new StreamReader(stream);
		LoadHtml(streamReader.ReadToEnd());
	}

	public void LoadHtml(string content)
	{
		LoadHtml(content, null);
	}

	public void LoadHtml(string content, string url)
	{
		if (content == null)
		{
			content = "";
		}
		if (!isCreated)
		{
			desiredContent = content;
			desiredUrl = url;
			desiredLoad = true;
			return;
		}
		NativeMethods.IStream pStream = null;
		IntPtr hGlobal = Marshal.StringToHGlobalUni(content);
		NativeMethods.CreateStreamOnHGlobal(hGlobal, fDeleteOnRelease: true, out pStream);
		if (pStream == null)
		{
			NativeMethods.IPersistStreamInit persistStreamInit = (NativeMethods.IPersistStreamInit)site.Document;
			persistStreamInit.InitNew();
			persistStreamInit = null;
		}
		else
		{
			NativeMethods.IHTMLDocument2 document = site.Document;
			if (url == null)
			{
				NativeMethods.IPersistStreamInit persistStreamInit2 = (NativeMethods.IPersistStreamInit)document;
				persistStreamInit2.Load(pStream);
				persistStreamInit2 = null;
			}
			else
			{
				NativeMethods.IPersistMoniker persistMoniker = (NativeMethods.IPersistMoniker)document;
				NativeMethods.IMoniker ppmk = null;
				NativeMethods.CreateURLMoniker(null, url, out ppmk);
				NativeMethods.IBindCtx ppbc = null;
				NativeMethods.CreateBindCtx(0, out ppbc);
				persistMoniker.Load(1, ppmk, ppbc, 0);
				persistMoniker = null;
				ppmk = null;
				ppbc = null;
			}
		}
		this.url = url;
	}

	protected virtual void OnCreated(EventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("You must specify a non-null EventArgs for OnCreated");
		}
		object[] array = new object[1] { true };
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 7100, 0, array, null);
		array[0] = true;
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 7101, 0, array, null);
		array[0] = true;
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 6049, 0, array, null);
		array[0] = true;
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 2332, 0, array, null);
		array[0] = true;
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 2333, 0, array, null);
		array[0] = true;
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 2334, 0, array, null);
		array[0] = true;
		CommandTarget.Exec(ref NativeMethods.Guid_MSHTML, 2335, 0, array, null);
		if (designModeDesired)
		{
			IsDesignMode = designModeDesired;
			designModeDesired = false;
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		if (IsReady)
		{
			site.SetFocus();
		}
		else
		{
			focusDesired = true;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		if (firstActivation)
		{
			site = new HtmlSite(this);
			site.CreateHtml();
			isCreated = true;
			OnCreated(EventArgs.Empty);
			site.ActivateHtml();
			firstActivation = false;
			if (desiredLoad)
			{
				LoadHtml(desiredContent, desiredUrl);
				desiredLoad = false;
			}
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		if (!base.DesignMode && site != null)
		{
			site.DeactivateHtml();
			site.CloseHtml();
			site = null;
		}
		base.OnHandleDestroyed(e);
	}

	protected internal virtual void OnReadyStateComplete(EventArgs e)
	{
		isReady = true;
		((EventHandler)base.Events[readyStateCompleteEvent])?.Invoke(this, e);
		if (focusDesired)
		{
			focusDesired = false;
			site.ActivateHtml();
			site.SetFocus();
		}
		persistStream = (NativeMethods.IPersistStreamInit)HtmlDocument;
		Selection.SynchronizeSelection();
		if (multipleSelectionDesired)
		{
			MultipleSelectionEnabled = multipleSelectionDesired;
		}
		if (absolutePositioningDesired)
		{
			AbsolutePositioningEnabled = absolutePositioningDesired;
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public override bool PreProcessMessage(ref Message m)
	{
		bool flag = false;
		if (m.Msg >= 256 && m.Msg <= 264)
		{
			if (m.Msg == 256)
			{
				flag = ProcessCmdKey(ref m, (Keys)((int)m.WParam | (int)Control.ModifierKeys));
			}
			if (!flag)
			{
				int num = (int)m.WParam;
				if ((num != 33 && num != 34) || (Control.ModifierKeys & Keys.Control) == 0)
				{
					NativeMethods.COMMSG cOMMSG = new NativeMethods.COMMSG();
					cOMMSG.hwnd = m.HWnd;
					cOMMSG.message = m.Msg;
					cOMMSG.wParam = m.WParam;
					cOMMSG.lParam = m.LParam;
					flag = site.TranslateAccelarator(cOMMSG);
				}
				else
				{
					WndProc(ref m);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			flag = base.PreProcessMessage(ref m);
		}
		return flag;
	}

	public string SaveHtml()
	{
		if (!IsCreated)
		{
			throw new Exception("HtmlControl.SaveHtml : No HTML to save!");
		}
		string text = string.Empty;
		try
		{
			NativeMethods.IHTMLDocument2 document = site.Document;
			NativeMethods.IPersistStreamInit persistStreamInit = (NativeMethods.IPersistStreamInit)document;
			NativeMethods.IStream pStream = null;
			NativeMethods.CreateStreamOnHGlobal(NativeMethods.NullIntPtr, fDeleteOnRelease: true, out pStream);
			persistStreamInit.Save(pStream, 1);
			NativeMethods.STATSTG sTATSTG = new NativeMethods.STATSTG();
			pStream.Stat(sTATSTG, 1);
			int num = (int)sTATSTG.cbSize;
			byte[] array = new byte[num];
			NativeMethods.GetHGlobalFromStream(pStream, out var pHGlobal);
			IntPtr intPtr = NativeMethods.GlobalLock(pHGlobal);
			if (intPtr != NativeMethods.NullIntPtr)
			{
				Marshal.Copy(intPtr, array, 0, num);
				NativeMethods.GlobalUnlock(pHGlobal);
				StreamReader streamReader = null;
				try
				{
					streamReader = new StreamReader(new MemoryStream(array), Encoding.Default);
					text = streamReader.ReadToEnd();
				}
				finally
				{
					streamReader?.Close();
				}
			}
		}
		catch (Exception)
		{
			text = string.Empty;
		}
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}

	public void SaveHtml(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("SaveHtml : Must specify a non-null stream to which to save");
		}
		string value = SaveHtml();
		StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
		streamWriter.Write(value);
		streamWriter.Flush();
	}

	public void Print()
	{
		ExecuteWithUserInterface(27, null);
	}

	public void PrintPreview()
	{
		ExecuteWithUserInterface(2003, null);
	}

	public void Copy()
	{
		if (!CanCopy)
		{
			throw new InvalidOperationException();
		}
		Execute(15);
	}

	public void Cut()
	{
		if (!CanCut)
		{
			throw new InvalidOperationException();
		}
		Execute(16);
	}

	public void Paste()
	{
		if (!CanPaste)
		{
			throw new InvalidOperationException();
		}
		Execute(26);
	}

	public void Delete()
	{
		if (!CanDelete)
		{
			throw new InvalidOperationException();
		}
		Execute(17);
	}

	public void Redo()
	{
		if (!CanRedo)
		{
			throw new InvalidOperationException();
		}
		Execute(29);
	}

	public void Undo()
	{
		if (!CanUndo)
		{
			throw new InvalidOperationException();
		}
		Execute(43);
	}

	public void SelectAll()
	{
		Execute(31);
	}

	public bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp)
	{
		NativeMethods.IHTMLSelectionObject iHTMLSelectionObject = site.Document.GetSelection();
		bool flag = false;
		if (iHTMLSelectionObject != null)
		{
			flag = iHTMLSelectionObject.GetSelectionType().Equals("Text");
		}
		NativeMethods.IHTMLTxtRange iHTMLTxtRange = null;
		if (flag)
		{
			object obj = iHTMLSelectionObject.CreateRange();
			iHTMLTxtRange = obj as NativeMethods.IHTMLTxtRange;
		}
		if (iHTMLTxtRange == null)
		{
			NativeMethods.IHtmlBodyElement htmlBodyElement = site.Document.GetBody() as NativeMethods.IHtmlBodyElement;
			flag = false;
			iHTMLTxtRange = htmlBodyElement.createTextRange();
		}
		if (searchUp)
		{
			if (flag)
			{
				iHTMLTxtRange.MoveEnd("character", -1);
			}
			for (int num = 1; num == 1; num = iHTMLTxtRange.MoveStart("textedit", -1))
			{
			}
		}
		else
		{
			if (flag)
			{
				iHTMLTxtRange.MoveStart("character", 1);
			}
			for (int num2 = 1; num2 == 1; num2 = iHTMLTxtRange.MoveEnd("textedit", 1))
			{
			}
		}
		int flags = (matchCase ? 4 : 0) | (wholeWord ? 2 : 0);
		int count = (searchUp ? (-10000000) : 10000000);
		if (iHTMLTxtRange.FindText(searchString, count, flags))
		{
			iHTMLTxtRange.Select();
			iHTMLTxtRange.ScrollIntoView(fStart: true);
			return true;
		}
		if (flag)
		{
			iHTMLTxtRange = iHTMLSelectionObject.CreateRange() as NativeMethods.IHTMLTxtRange;
			if (searchUp)
			{
				iHTMLTxtRange.MoveStart("character", 1);
				for (int num3 = 1; num3 == 1; num3 = iHTMLTxtRange.MoveEnd("textedit", 1))
				{
				}
			}
			else
			{
				iHTMLTxtRange.MoveEnd("character", -1);
				for (int num4 = 1; num4 == 1; num4 = iHTMLTxtRange.MoveStart("textedit", -1))
				{
				}
			}
			if (iHTMLTxtRange.FindText(searchString, count, flags))
			{
				iHTMLTxtRange.Select();
				iHTMLTxtRange.ScrollIntoView(fStart: true);
				return true;
			}
		}
		return false;
	}

	public bool Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp)
	{
		Selection.SynchronizeSelection();
		if (Selection.Type == HtmlSelectionType.TextSelection && Selection.Length > 0)
		{
			NativeMethods.IHTMLTxtRange iHTMLTxtRange = Selection.Selection as NativeMethods.IHTMLTxtRange;
			int flags = (matchCase ? 4 : 0) | (wholeWord ? 2 : 0);
			int count = (searchUp ? (-10000000) : 10000000);
			if (iHTMLTxtRange.FindText(searchString, count, flags))
			{
				iHTMLTxtRange.SetText(replaceString);
			}
		}
		return Find(searchString, matchCase, wholeWord, searchUp);
	}

	public void InsertHyperlink(string url, string description)
	{
		Selection.SynchronizeSelection();
		if (url == null)
		{
			try
			{
				Execute(2124);
				return;
			}
			catch
			{
				return;
			}
		}
		if ((Selection.Type == HtmlSelectionType.TextSelection || Selection.Type == HtmlSelectionType.Empty) && Selection.Length == 0)
		{
			InsertHtml("<a href=\"" + url + "\">" + description + "</a>");
		}
		else
		{
			ExecuteWithUserInterface(2124, new object[1] { url });
		}
	}

	public void InsertImage(string url)
	{
		Selection.SynchronizeSelection();
		if ((Selection.Type == HtmlSelectionType.TextSelection || Selection.Type == HtmlSelectionType.Empty) && Selection.Length == 0)
		{
			InsertHtml("<img src=\"" + url + "\"/>");
			return;
		}
		Execute(2168, new object[1] { url });
	}

	public void InsertImage()
	{
		ExecuteWithUserInterface(2168, null);
	}

	public void InsertHtml(string html)
	{
		Selection.SynchronizeSelection();
		if (Selection.Type == HtmlSelectionType.ElementSelection)
		{
			NativeMethods.IHtmlControlRange htmlControlRange = (NativeMethods.IHtmlControlRange)Selection.Selection;
			int length = htmlControlRange.GetLength();
			if (length == 1)
			{
				NativeMethods.IHTMLElement iHTMLElement = htmlControlRange.Item(0);
				if (string.Compare(iHTMLElement.GetTagName(), "div", ignoreCase: true) == 0 || string.Compare(iHTMLElement.GetTagName(), "td", ignoreCase: true) == 0)
				{
					iHTMLElement.InsertAdjacentHTML("beforeEnd", html);
				}
			}
		}
		else
		{
			NativeMethods.IHTMLTxtRange iHTMLTxtRange = (NativeMethods.IHTMLTxtRange)Selection.Selection;
			iHTMLTxtRange.PasteHTML(html);
		}
	}
}
