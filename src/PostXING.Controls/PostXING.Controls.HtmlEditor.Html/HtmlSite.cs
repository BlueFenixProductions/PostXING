using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PostXING.Controls.HtmlEditor.Html;

[ClassInterface(ClassInterfaceType.None)]
internal class HtmlSite : NativeMethods.IOleClientSite, NativeMethods.IOleContainer, NativeMethods.IOleDocumentSite, NativeMethods.IOleInPlaceSite, NativeMethods.IOleInPlaceFrame, NativeMethods.IDocHostUIHandler, NativeMethods.IPropertyNotifySink, NativeMethods.IAdviseSink, NativeMethods.IOleServiceProvider
{
	internal enum ConverterInfo
	{
		No,
		Yes,
		Unknown
	}

	internal class DataObjectConverter
	{
		public ConverterInfo CanConvertToHtml(IDataObject dataObject)
		{
			if (dataObject.GetDataPresent("FileDrop"))
			{
				return ConverterInfo.Yes;
			}
			return ConverterInfo.Unknown;
		}

		public bool ConvertToHtml(IDataObject originalDataObject, DataObject newDataObject)
		{
			if (originalDataObject.GetDataPresent("FileDrop"))
			{
				string[] array = (string[])originalDataObject.GetData("FileDrop");
				if (array.Length == 1)
				{
					StreamReader streamReader = new StreamReader(array[0]);
					string data = streamReader.ReadToEnd();
					streamReader.Close();
					newDataObject.SetData("HTML", data);
				}
				return true;
			}
			return false;
		}
	}

	private sealed class DropTarget : NativeMethods.IOleDropTarget
	{
		private DataObject currentDataObj;

		private IntPtr currentDataObjPtr;

		private NativeMethods.IOleDropTarget originalDropTarget;

		private DataObjectConverter converter;

		private ConverterInfo converterInfo;

		public DropTarget(DataObjectConverter converter, NativeMethods.IOleDropTarget originalDropTarget)
		{
			this.converter = converter;
			this.originalDropTarget = originalDropTarget;
		}

		public int OleDragEnter(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
		{
			object objectForIUnknown = Marshal.GetObjectForIUnknown(pDataObj);
			DataObject dataObject = new DataObject(objectForIUnknown);
			converterInfo = converter.CanConvertToHtml(dataObject);
			if (converterInfo == ConverterInfo.Yes)
			{
				currentDataObj = new DataObject(DataFormats.Html, string.Empty);
				IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(currentDataObj);
				Guid iid = new Guid("0000010E-0000-0000-C000-000000000046");
				Marshal.QueryInterface(iUnknownForObject, ref iid, out currentDataObjPtr);
				Marshal.Release(iUnknownForObject);
				return originalDropTarget.OleDragEnter(currentDataObjPtr, grfKeyState, pt, ref pdwEffect);
			}
			if (converterInfo == ConverterInfo.No)
			{
				pdwEffect = 0;
			}
			else if (converterInfo == ConverterInfo.Unknown)
			{
				return originalDropTarget.OleDragEnter(pDataObj, grfKeyState, pt, ref pdwEffect);
			}
			return 0;
		}

		public int OleDragOver(int grfKeyState, long pt, ref int pdwEffect)
		{
			if (converterInfo != ConverterInfo.No)
			{
				return originalDropTarget.OleDragOver(grfKeyState, pt, ref pdwEffect);
			}
			pdwEffect = 0;
			return 0;
		}

		public int OleDragLeave()
		{
			converterInfo = ConverterInfo.No;
			if (currentDataObj != null)
			{
				currentDataObj = null;
				Marshal.Release(currentDataObjPtr);
				currentDataObjPtr = IntPtr.Zero;
				return originalDropTarget.OleDragLeave();
			}
			return 0;
		}

		public int OleDrop(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
		{
			int result = 0;
			if (converterInfo == ConverterInfo.Yes)
			{
				object objectForIUnknown = Marshal.GetObjectForIUnknown(pDataObj);
				DataObject originalDataObject = new DataObject(objectForIUnknown);
				if (converter.ConvertToHtml(originalDataObject, currentDataObj))
				{
					IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(currentDataObj);
					Guid iid = new Guid("0000010E-0000-0000-C000-000000000046");
					Marshal.QueryInterface(iUnknownForObject, ref iid, out currentDataObjPtr);
					result = originalDropTarget.OleDrop(currentDataObjPtr, grfKeyState, pt, ref pdwEffect);
					currentDataObj = null;
					currentDataObjPtr = IntPtr.Zero;
					Marshal.Release(currentDataObjPtr);
				}
				else
				{
					pdwEffect = 0;
				}
			}
			else if (converterInfo == ConverterInfo.Unknown)
			{
				result = originalDropTarget.OleDrop(pDataObj, grfKeyState, pt, ref pdwEffect);
			}
			converterInfo = ConverterInfo.No;
			return result;
		}
	}

	private HtmlControl hostControl;

	private NativeMethods.IOleObject oleObject;

	private NativeMethods.IHTMLDocument2 document;

	private NativeMethods.IOleDocumentView documentView;

	private NativeMethods.IOleCommandTarget commandTarget;

	private NativeMethods.IOleInPlaceActiveObject activeObject;

	private NativeMethods.ConnectionPointCookie propertyNotifySinkCookie;

	private int adviseSinkCookie;

	public NativeMethods.IOleCommandTarget CommandTarget => commandTarget;

	public NativeMethods.IHTMLDocument2 Document => document;

	public HtmlSite(HtmlControl hostControl)
	{
		if (hostControl == null || !hostControl.IsHandleCreated)
		{
			throw new ArgumentException("hostControl");
		}
		this.hostControl = hostControl;
		this.hostControl.Resize += HostControl_Resize;
	}

	public void ActivateHtml()
	{
		try
		{
			NativeMethods.COMRECT cOMRECT = new NativeMethods.COMRECT();
			NativeMethods.GetClientRect(hostControl.Handle, cOMRECT);
			oleObject.DoVerb(-4, NativeMethods.NullIntPtr, this, 0, hostControl.Handle, cOMRECT);
		}
		catch (Exception)
		{
		}
	}

	public void CloseHtml()
	{
		hostControl.Resize -= HostControl_Resize;
		try
		{
			if (propertyNotifySinkCookie != null)
			{
				propertyNotifySinkCookie.Disconnect();
				propertyNotifySinkCookie = null;
			}
			if (document != null)
			{
				documentView = null;
				document = null;
				commandTarget = null;
				activeObject = null;
				if (adviseSinkCookie != 0)
				{
					oleObject.Unadvise(adviseSinkCookie);
					adviseSinkCookie = 0;
				}
				oleObject.Close(1);
				oleObject.SetClientSite(null);
				oleObject = null;
			}
		}
		catch (Exception)
		{
		}
	}

	public void CreateHtml()
	{
		bool flag = false;
		try
		{
			document = (NativeMethods.IHTMLDocument2)new NativeMethods.HTMLDocument();
			oleObject = (NativeMethods.IOleObject)document;
			oleObject.SetClientSite(this);
			flag = true;
			propertyNotifySinkCookie = new NativeMethods.ConnectionPointCookie(document, this, typeof(NativeMethods.IPropertyNotifySink), throwException: false);
			oleObject.Advise(this, out adviseSinkCookie);
			commandTarget = (NativeMethods.IOleCommandTarget)document;
		}
		finally
		{
			if (!flag)
			{
				document = null;
				oleObject = null;
				commandTarget = null;
			}
		}
	}

	public void DeactivateHtml()
	{
	}

	private void HostControl_Resize(object src, EventArgs e)
	{
		if (documentView != null)
		{
			NativeMethods.COMRECT rect = new NativeMethods.COMRECT();
			NativeMethods.GetClientRect(hostControl.Handle, rect);
			documentView.SetRect(rect);
		}
	}

	private void OnReadyStateChanged()
	{
		string readyState = document.GetReadyState();
		if (string.Compare(readyState, "complete", ignoreCase: true) == 0)
		{
			OnReadyStateComplete();
		}
	}

	private void OnReadyStateComplete()
	{
		hostControl.OnReadyStateComplete(EventArgs.Empty);
	}

	internal void SetFocus()
	{
		if (activeObject != null)
		{
			IntPtr hwnd = IntPtr.Zero;
			if (activeObject.GetWindow(out hwnd) == 0)
			{
				NativeMethods.SetFocus(hwnd);
			}
		}
	}

	public bool TranslateAccelarator(NativeMethods.COMMSG msg)
	{
		if (activeObject != null && activeObject.TranslateAccelerator(msg) != 1)
		{
			return true;
		}
		return false;
	}

	int NativeMethods.IOleClientSite.SaveObject()
	{
		return 0;
	}

	int NativeMethods.IOleClientSite.GetMoniker(int dwAssign, int dwWhichMoniker, out object ppmk)
	{
		ppmk = null;
		return -2147467263;
	}

	int NativeMethods.IOleClientSite.GetContainer(out NativeMethods.IOleContainer ppContainer)
	{
		ppContainer = this;
		return 0;
	}

	int NativeMethods.IOleClientSite.ShowObject()
	{
		return 0;
	}

	int NativeMethods.IOleClientSite.OnShowWindow(int fShow)
	{
		return 0;
	}

	int NativeMethods.IOleClientSite.RequestNewObjectLayout()
	{
		return 0;
	}

	void NativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleContainer.EnumObjects(int grfFlags, object[] ppenum)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleContainer.LockContainer(int fLock)
	{
	}

	int NativeMethods.IOleDocumentSite.ActivateMe(NativeMethods.IOleDocumentView viewToActivate)
	{
		if (viewToActivate == null)
		{
			return -2147024809;
		}
		NativeMethods.COMRECT rect = new NativeMethods.COMRECT();
		NativeMethods.GetClientRect(hostControl.Handle, rect);
		documentView = viewToActivate;
		documentView.SetInPlaceSite(this);
		documentView.UIActivate(1);
		documentView.SetRect(rect);
		documentView.Show(1);
		return 0;
	}

	IntPtr NativeMethods.IOleInPlaceSite.GetWindow()
	{
		return hostControl.Handle;
	}

	void NativeMethods.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	int NativeMethods.IOleInPlaceSite.CanInPlaceActivate()
	{
		return 0;
	}

	void NativeMethods.IOleInPlaceSite.OnInPlaceActivate()
	{
	}

	void NativeMethods.IOleInPlaceSite.OnUIActivate()
	{
	}

	void NativeMethods.IOleInPlaceSite.GetWindowContext(out NativeMethods.IOleInPlaceFrame ppFrame, out NativeMethods.IOleInPlaceUIWindow ppDoc, NativeMethods.COMRECT lprcPosRect, NativeMethods.COMRECT lprcClipRect, NativeMethods.tagOIFI lpFrameInfo)
	{
		ppFrame = this;
		ppDoc = null;
		NativeMethods.GetClientRect(hostControl.Handle, lprcPosRect);
		NativeMethods.GetClientRect(hostControl.Handle, lprcClipRect);
		lpFrameInfo.cb = Marshal.SizeOf(typeof(NativeMethods.tagOIFI));
		lpFrameInfo.fMDIApp = 0;
		lpFrameInfo.hwndFrame = hostControl.Handle;
		lpFrameInfo.hAccel = NativeMethods.NullIntPtr;
		lpFrameInfo.cAccelEntries = 0;
	}

	int NativeMethods.IOleInPlaceSite.Scroll(NativeMethods.tagSIZE scrollExtant)
	{
		return -2147467263;
	}

	void NativeMethods.IOleInPlaceSite.OnUIDeactivate(int fUndoable)
	{
	}

	void NativeMethods.IOleInPlaceSite.OnInPlaceDeactivate()
	{
	}

	void NativeMethods.IOleInPlaceSite.DiscardUndoState()
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceSite.DeactivateAndUndo()
	{
	}

	int NativeMethods.IOleInPlaceSite.OnPosRectChange(NativeMethods.COMRECT lprcPosRect)
	{
		return 0;
	}

	IntPtr NativeMethods.IOleInPlaceFrame.GetWindow()
	{
		return hostControl.Handle;
	}

	void NativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.SetActiveObject(NativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
	{
		activeObject = pActiveObject;
	}

	void NativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
	{
		throw new COMException(string.Empty, -2147467263);
	}

	void NativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
	{
	}

	void NativeMethods.IOleInPlaceFrame.EnableModeless(int fEnable)
	{
	}

	int NativeMethods.IOleInPlaceFrame.TranslateAccelerator(NativeMethods.COMMSG lpmsg, short wID)
	{
		return 1;
	}

	int NativeMethods.IDocHostUIHandler.ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
	{
		if (hostControl != null)
		{
			Point pos = hostControl.PointToClient(new Point(pt.x, pt.y));
			hostControl.ContextMenu.Show(hostControl, pos);
		}
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
	{
		info.dwDoubleClick = 0;
		int num = 132;
		if (!hostControl.ScriptEnabled)
		{
			num |= 0x10;
		}
		info.dwFlags = num;
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.EnableModeless(bool fEnable)
	{
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.ShowUI(int dwID, NativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, NativeMethods.IOleInPlaceFrame frame, NativeMethods.IOleInPlaceUIWindow doc)
	{
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.HideUI()
	{
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.UpdateUI()
	{
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.OnDocWindowActivate(bool fActivate)
	{
		return -2147467263;
	}

	int NativeMethods.IDocHostUIHandler.OnFrameWindowActivate(bool fActivate)
	{
		return -2147467263;
	}

	int NativeMethods.IDocHostUIHandler.ResizeBorder(NativeMethods.COMRECT rect, NativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
	{
		return -2147467263;
	}

	int NativeMethods.IDocHostUIHandler.GetOptionKeyPath(string[] pbstrKey, int dw)
	{
		pbstrKey[0] = null;
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.GetDropTarget(NativeMethods.IOleDropTarget pDropTarget, out NativeMethods.IOleDropTarget ppDropTarget)
	{
		ppDropTarget = new DropTarget(new DataObjectConverter(), pDropTarget);
		if (ppDropTarget == null)
		{
			return -2147467263;
		}
		return 0;
	}

	int NativeMethods.IDocHostUIHandler.GetExternal(out object ppDispatch)
	{
		ppDispatch = hostControl.ScriptObject;
		if (ppDispatch != null)
		{
			return 0;
		}
		return -2147467263;
	}

	int NativeMethods.IDocHostUIHandler.TranslateAccelerator(NativeMethods.COMMSG msg, ref Guid group, int nCmdID)
	{
		return 1;
	}

	int NativeMethods.IDocHostUIHandler.TranslateUrl(int dwTranslate, string strURLIn, out string pstrURLOut)
	{
		pstrURLOut = null;
		return -2147467263;
	}

	int NativeMethods.IDocHostUIHandler.FilterDataObject(NativeMethods.IOleDataObject pDO, out NativeMethods.IOleDataObject ppDORet)
	{
		ppDORet = null;
		return -2147467263;
	}

	void NativeMethods.IPropertyNotifySink.OnChanged(int dispID)
	{
		if (dispID == -525)
		{
			OnReadyStateChanged();
		}
	}

	void NativeMethods.IPropertyNotifySink.OnRequestEdit(int dispID)
	{
	}

	void NativeMethods.IAdviseSink.OnDataChange(NativeMethods.FORMATETC pFormat, NativeMethods.STGMEDIUM pStg)
	{
	}

	void NativeMethods.IAdviseSink.OnViewChange(int dwAspect, int index)
	{
	}

	void NativeMethods.IAdviseSink.OnRename(object pmk)
	{
	}

	void NativeMethods.IAdviseSink.OnSave()
	{
	}

	void NativeMethods.IAdviseSink.OnClose()
	{
	}

	int NativeMethods.IOleServiceProvider.QueryService(ref Guid sid, ref Guid iid, out IntPtr ppvObject)
	{
		int result = -2147467262;
		ppvObject = NativeMethods.NullIntPtr;
		return result;
	}
}
