using System;
using System.Runtime.InteropServices;
using System.Security;

namespace PostXING.Controls.HtmlEditor.Html;

[SuppressUnmanagedCodeSecurity]
public sealed class NativeMethods
{
	[Flags]
	public enum BSCFlags
	{
		BSCF_FIRSTDATANOTIFICATION = 1,
		BSCF_INTERMEDIATEDATANOTIFICATION = 2,
		BSCF_LASTDATANOTIFICATION = 4,
		BSCF_DATAFULLYAVAILABLE = 8,
		BSCF_AVAILABLEDATASIZEUNKNOWN = 0x10
	}

	[ComVisible(false)]
	internal sealed class OLECMDEXECOPT
	{
		public const int OLECMDEXECOPT_DODEFAULT = 0;

		public const int OLECMDEXECOPT_PROMPTUSER = 1;

		public const int OLECMDEXECOPT_DONTPROMPTUSER = 2;

		public const int OLECMDEXECOPT_SHOWHELP = 3;
	}

	[ComVisible(false)]
	internal sealed class OLECMDF
	{
		public const int OLECMDF_SUPPORTED = 1;

		public const int OLECMDF_ENABLED = 2;

		public const int OLECMDF_LATCHED = 4;

		public const int OLECMDF_NINCHED = 8;
	}

	[ComVisible(false)]
	internal sealed class StreamConsts
	{
		public const int LOCK_WRITE = 1;

		public const int LOCK_EXCLUSIVE = 2;

		public const int LOCK_ONLYONCE = 4;

		public const int STATFLAG_DEFAULT = 0;

		public const int STATFLAG_NONAME = 1;

		public const int STATFLAG_NOOPEN = 2;

		public const int STGC_DEFAULT = 0;

		public const int STGC_OVERWRITE = 1;

		public const int STGC_ONLYIFCURRENT = 2;

		public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4;

		public const int STREAM_SEEK_SET = 0;

		public const int STREAM_SEEK_CUR = 1;

		public const int STREAM_SEEK_END = 2;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class tagLOGPALETTE
	{
		[MarshalAs(UnmanagedType.U2)]
		public short palVersion;

		[MarshalAs(UnmanagedType.U2)]
		public short palNumEntries;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class tagOIFI
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cb;

		[MarshalAs(UnmanagedType.I4)]
		public int fMDIApp;

		public IntPtr hwndFrame;

		public IntPtr hAccel;

		[MarshalAs(UnmanagedType.U4)]
		public int cAccelEntries;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class tagOLECMD
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cmdID;

		[MarshalAs(UnmanagedType.U4)]
		public int cmdf;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class tagOleMenuGroupWidths
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public int[] widths = new int[6];
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class tagOLEVERB
	{
		[MarshalAs(UnmanagedType.I4)]
		public int lVerb;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszVerbName;

		[MarshalAs(UnmanagedType.U4)]
		public int fuFlags;

		[MarshalAs(UnmanagedType.U4)]
		public int grfAttribs;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public class COMMSG
	{
		public IntPtr hwnd;

		public int message;

		public IntPtr wParam;

		public IntPtr lParam;

		public int time;

		public int pt_x;

		public int pt_y;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public class COMRECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public COMRECT()
		{
		}

		public COMRECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public static COMRECT FromXYWH(int x, int y, int width, int height)
		{
			return new COMRECT(x, y, x + width, y + height);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class DISPPARAMS
	{
		public IntPtr rgvarg;

		public IntPtr rgdispidNamedArgs;

		[MarshalAs(UnmanagedType.U4)]
		public int cArgs;

		[MarshalAs(UnmanagedType.U4)]
		public int cNamedArgs;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class EXCEPINFO
	{
		[MarshalAs(UnmanagedType.U2)]
		public short wCode;

		[MarshalAs(UnmanagedType.U2)]
		public short wReserved;

		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		[MarshalAs(UnmanagedType.U4)]
		public int dwHelpContext;

		public IntPtr dwReserved;

		public IntPtr dwFillIn;

		[MarshalAs(UnmanagedType.I4)]
		public int scode;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public class DOCHOSTUIINFO
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cbSize;

		[MarshalAs(UnmanagedType.I4)]
		public int dwFlags;

		[MarshalAs(UnmanagedType.I4)]
		public int dwDoubleClick;

		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved1;

		[MarshalAs(UnmanagedType.I4)]
		public int dwReserved2;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class FORMATETC
	{
		[MarshalAs(UnmanagedType.I4)]
		public int cfFormat;

		public IntPtr ptd;

		[MarshalAs(UnmanagedType.I4)]
		public int dwAspect;

		[MarshalAs(UnmanagedType.I4)]
		public int lindex;

		[MarshalAs(UnmanagedType.I4)]
		public int tymed;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public class HTML_PAINTER_INFO
	{
		[MarshalAs(UnmanagedType.I4)]
		public int lFlags;

		[MarshalAs(UnmanagedType.I4)]
		public int lZOrder;

		[MarshalAs(UnmanagedType.Struct)]
		public Guid iidDrawObject;

		[MarshalAs(UnmanagedType.Struct)]
		public RECT rcBounds;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public class NMHDR
	{
		public IntPtr hwndFrom;

		public int idFrom;

		public int code;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public class NMCUSTOMDRAW
	{
		public NMHDR nmcd;

		public int dwDrawStage;

		public IntPtr hdc;

		public RECT rc;

		public int dwItemSpec;

		public int uItemState;

		public IntPtr lItemlParam;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public class POINT
	{
		public int x;

		public int y;

		public POINT()
		{
		}

		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	[ComVisible(true)]
	public struct RECT(int left, int top, int right, int bottom)
	{
		public int left = left;

		public int top = top;

		public int right = right;

		public int bottom = bottom;

		public static RECT FromXYWH(int x, int y, int width, int height)
		{
			return new RECT(x, y, x + width, y + height);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class STATDATA
	{
		[MarshalAs(UnmanagedType.U4)]
		public int advf;

		[MarshalAs(UnmanagedType.U4)]
		public int dwConnection;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public class STGMEDIUM
	{
		[MarshalAs(UnmanagedType.I4)]
		public int tymed;

		public IntPtr unionmember;

		public IntPtr pUnkForRelease;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public class STATSTG
	{
		[MarshalAs(UnmanagedType.I4)]
		public int pwcsName;

		[MarshalAs(UnmanagedType.I4)]
		public int type;

		[MarshalAs(UnmanagedType.I8)]
		public long cbSize;

		[MarshalAs(UnmanagedType.I8)]
		public long mtime;

		[MarshalAs(UnmanagedType.I8)]
		public long ctime;

		[MarshalAs(UnmanagedType.I8)]
		public long atime;

		[MarshalAs(UnmanagedType.I8)]
		public long grfMode;

		[MarshalAs(UnmanagedType.I8)]
		public long grfLocksSupported;

		[MarshalAs(UnmanagedType.I4)]
		public int clsid_data1;

		[MarshalAs(UnmanagedType.I2)]
		public short clsid_data2;

		[MarshalAs(UnmanagedType.I2)]
		public short clsid_data3;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b0;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b1;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b2;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b3;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b4;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b5;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b6;

		[MarshalAs(UnmanagedType.U1)]
		public byte clsid_b7;

		[MarshalAs(UnmanagedType.I8)]
		public long grfStateBits;

		[MarshalAs(UnmanagedType.I8)]
		public long reserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public sealed class tagSIZE
	{
		[MarshalAs(UnmanagedType.I4)]
		public int cx;

		[MarshalAs(UnmanagedType.I4)]
		public int cy;
	}

	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(true)]
	public sealed class tagSIZEL
	{
		[MarshalAs(UnmanagedType.I4)]
		public int cx;

		[MarshalAs(UnmanagedType.I4)]
		public int cy;
	}

	[ComVisible(false)]
	internal class ConnectionPointCookie
	{
		private IConnectionPoint connectionPoint;

		private int cookie;

		public ConnectionPointCookie(object source, object sink, Type eventInterface)
			: this(source, sink, eventInterface, throwException: true)
		{
		}

		public ConnectionPointCookie(object source, object sink, Type eventInterface, bool throwException)
		{
			Exception ex = null;
			if (source is IConnectionPointContainer)
			{
				IConnectionPointContainer connectionPointContainer = (IConnectionPointContainer)source;
				try
				{
					Guid riid = eventInterface.GUID;
					connectionPointContainer.FindConnectionPoint(ref riid, out connectionPoint);
				}
				catch (Exception)
				{
					connectionPoint = null;
				}
				if (connectionPoint == null)
				{
					ex = new ArgumentException("The source object does not expose the " + eventInterface.Name + " event inteface");
				}
				else if (!eventInterface.IsInstanceOfType(sink))
				{
					ex = new InvalidCastException("The sink object does not implement the eventInterface");
				}
				else
				{
					try
					{
						connectionPoint.Advise(sink, out cookie);
					}
					catch
					{
						cookie = 0;
						connectionPoint = null;
						ex = new Exception("IConnectionPoint::Advise failed for event interface '" + eventInterface.Name + "'");
					}
				}
			}
			else
			{
				ex = new InvalidCastException("The source object does not expost IConnectionPointContainer");
			}
			if (throwException && (connectionPoint == null || cookie == 0))
			{
				if (ex == null)
				{
					throw new ArgumentException("Could not create connection point for event interface '" + eventInterface.Name + "'");
				}
				throw ex;
			}
		}

		public void Disconnect()
		{
			if (connectionPoint != null && cookie != 0)
			{
				connectionPoint.Unadvise(cookie);
				cookie = 0;
				connectionPoint = null;
			}
		}

		~ConnectionPointCookie()
		{
			Disconnect();
		}
	}

	[ComVisible(true)]
	[Guid("0000010F-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAdviseSink
	{
		void OnDataChange([In] FORMATETC pFormatetc, [In] STGMEDIUM pStgmed);

		void OnViewChange([In][MarshalAs(UnmanagedType.U4)] int dwAspect, [In][MarshalAs(UnmanagedType.I4)] int lindex);

		void OnRename([In][MarshalAs(UnmanagedType.Interface)] object pmk);

		void OnSave();

		void OnClose();
	}

	[Guid("0000000e-0000-0000-C000-000000000046")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IBindCtx
	{
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000001-0000-0000-C000-000000000046")]
	internal interface IClassFactory
	{
		[PreserveSig]
		int CreateInstance([In][MarshalAs(UnmanagedType.Interface)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object obj);

		[PreserveSig]
		int LockServer([In] bool fLock);
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
	internal interface IDocHostUIHandler
	{
		[PreserveSig]
		int ShowContextMenu([In][MarshalAs(UnmanagedType.U4)] int dwID, [In] POINT pt, [In][MarshalAs(UnmanagedType.Interface)] object pcmdtReserved, [In][MarshalAs(UnmanagedType.Interface)] object pdispReserved);

		[PreserveSig]
		int GetHostInfo([In][Out] DOCHOSTUIINFO info);

		[PreserveSig]
		int ShowUI([In][MarshalAs(UnmanagedType.I4)] int dwID, [In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject activeObject, [In][MarshalAs(UnmanagedType.Interface)] IOleCommandTarget commandTarget, [In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceFrame frame, [In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceUIWindow doc);

		[PreserveSig]
		int HideUI();

		[PreserveSig]
		int UpdateUI();

		[PreserveSig]
		int EnableModeless([In][MarshalAs(UnmanagedType.Bool)] bool fEnable);

		[PreserveSig]
		int OnDocWindowActivate([In][MarshalAs(UnmanagedType.Bool)] bool fActivate);

		[PreserveSig]
		int OnFrameWindowActivate([In][MarshalAs(UnmanagedType.Bool)] bool fActivate);

		[PreserveSig]
		int ResizeBorder([In] COMRECT rect, [In] IOleInPlaceUIWindow doc, [In] bool fFrameWindow);

		[PreserveSig]
		int TranslateAccelerator([In] COMMSG msg, [In] ref Guid group, [In][MarshalAs(UnmanagedType.I4)] int nCmdID);

		[PreserveSig]
		int GetOptionKeyPath([Out][MarshalAs(UnmanagedType.LPArray)] string[] pbstrKey, [In][MarshalAs(UnmanagedType.U4)] int dw);

		[PreserveSig]
		int GetDropTarget([In][MarshalAs(UnmanagedType.Interface)] IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out IOleDropTarget ppDropTarget);

		[PreserveSig]
		int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

		[PreserveSig]
		int TranslateUrl([In][MarshalAs(UnmanagedType.U4)] int dwTranslate, [In][MarshalAs(UnmanagedType.LPWStr)] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

		[PreserveSig]
		int FilterDataObject([In][MarshalAs(UnmanagedType.Interface)] IOleDataObject pDO, [MarshalAs(UnmanagedType.Interface)] out IOleDataObject ppDORet);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3050F425-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IElementBehavior
	{
		void Init([In][MarshalAs(UnmanagedType.Interface)] IElementBehaviorSite pBehaviorSite);

		void Notify([In][MarshalAs(UnmanagedType.U4)] int dwEvent, [In] IntPtr pVar);

		void Detach();
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3050F429-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IElementBehaviorFactory
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		IElementBehavior FindBehavior([In][MarshalAs(UnmanagedType.BStr)] string bstrBehavior, [In][MarshalAs(UnmanagedType.BStr)] string bstrBehaviorUrl, [In][MarshalAs(UnmanagedType.Interface)] IElementBehaviorSite pSite);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3050F427-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IElementBehaviorSite
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetElement();

		void RegisterNotification([In][MarshalAs(UnmanagedType.I4)] int lEvent);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("3050F659-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IElementBehaviorSiteOM2
	{
		[return: MarshalAs(UnmanagedType.I4)]
		int RegisterEvent([In][MarshalAs(UnmanagedType.BStr)] string pchEvent, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetEventCookie([In][MarshalAs(UnmanagedType.BStr)] string pchEvent);

		void FireEvent([In][MarshalAs(UnmanagedType.I4)] int lCookie, [In][MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEventObject);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLEventObj CreateEventObject();

		void RegisterName([In][MarshalAs(UnmanagedType.BStr)] string pchName);

		void RegisterUrn([In][MarshalAs(UnmanagedType.BStr)] string pchUrn);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementDefaults GetDefaults();
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("3050F671-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IElementNamespace
	{
		void AddTag([In][MarshalAs(UnmanagedType.BStr)] string tagName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);
	}

	[ComVisible(true)]
	[Guid("3050F672-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IElementNamespaceFactory
	{
		void Create([In][MarshalAs(UnmanagedType.Interface)] IElementNamespace pNamespace);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("3050F7FD-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IElementNamespaceFactoryCallback
	{
		void Resolve([In][MarshalAs(UnmanagedType.BStr)] string nameSpace, [In][MarshalAs(UnmanagedType.BStr)] string tagName, [In][MarshalAs(UnmanagedType.BStr)] string attributes, [In][MarshalAs(UnmanagedType.Interface)] IElementNamespace pNamespace);
	}

	[Guid("3050F670-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IElementNamespaceTable
	{
		void AddNamespace([In][MarshalAs(UnmanagedType.BStr)] string nameSpace, [In][MarshalAs(UnmanagedType.BStr)] string urn, [In][MarshalAs(UnmanagedType.I4)] int lFlags, [In] ref object factory);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("00000103-0000-0000-C000-000000000046")]
	internal interface IEnumFORMATETC
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Next([In][MarshalAs(UnmanagedType.U4)] int celt, [Out] FORMATETC rgelt, [In][Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Skip([In][MarshalAs(UnmanagedType.U4)] int celt);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Reset();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Clone([Out][MarshalAs(UnmanagedType.LPArray)] IEnumFORMATETC[] ppenum);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B3E7C340-EF97-11CE-9BC9-00AA00608E01")]
	internal interface IEnumOleUndoUnits
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Next([In][MarshalAs(UnmanagedType.U4)] int numDesired, out IntPtr unit, [MarshalAs(UnmanagedType.U4)] out int numReceived);

		void Bogus();

		[PreserveSig]
		int Skip([In][MarshalAs(UnmanagedType.I4)] int numToSkip);

		[PreserveSig]
		int Reset();

		[PreserveSig]
		int Clone([Out][MarshalAs(UnmanagedType.Interface)] IEnumOleUndoUnits enumerator);
	}

	[ComImport]
	[Guid("00000104-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IEnumOLEVERB
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Next([MarshalAs(UnmanagedType.U4)] int celt, [Out] tagOLEVERB rgelt, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Skip([In][MarshalAs(UnmanagedType.U4)] int celt);

		void Reset();

		void Clone(out IEnumOLEVERB ppenum);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("00000105-0000-0000-C000-000000000046")]
	internal interface IEnumSTATDATA
	{
		void Next([In][MarshalAs(UnmanagedType.U4)] int celt, [Out] STATDATA rgelt, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

		void Skip([In][MarshalAs(UnmanagedType.U4)] int celt);

		void Reset();

		void Clone([Out][MarshalAs(UnmanagedType.LPArray)] IEnumSTATDATA[] ppenum);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[Guid("3050f1d8-98b5-11cf-bb82-00aa00bdce0b")]
	[ComVisible(true)]
	internal interface IHtmlBodyElement
	{
		void put_background([In][MarshalAs(UnmanagedType.BStr)] string v);

		[return: MarshalAs(UnmanagedType.BStr)]
		string get_background();

		void put_bgProperties([In][MarshalAs(UnmanagedType.BStr)] string v);

		[return: MarshalAs(UnmanagedType.BStr)]
		string get_bgProperties();

		void put_leftMargin([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_leftMargin();

		void put_topMargin([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_topMargin();

		void put_rightMargin([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_rightMargin();

		void put_bottomMargin([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_bottomMargin();

		void put_noWrap([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_noWrap();

		void put_bgColor([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_bgColor();

		void put_text([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_text();

		void put_link([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_link();

		void put_vLink([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_vLink();

		void put_aLink([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_aLink();

		void put_onload([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_onload();

		void put_onunload([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_onunload();

		void put_scroll([In][MarshalAs(UnmanagedType.BStr)] string s);

		[return: MarshalAs(UnmanagedType.BStr)]
		string get_scroll();

		void put_onselect([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_onselect();

		void put_onbeforeunload([In][MarshalAs(UnmanagedType.Interface)] object o);

		[return: MarshalAs(UnmanagedType.Interface)]
		object get_onbeforeunload();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLTxtRange createTextRange();
	}

	[Guid("3050F4E9-98B5-11CF-BB82-00AA00BDCE0B")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IHtmlControlElement
	{
		void SetTabIndex([In][MarshalAs(UnmanagedType.I2)] short p);

		[return: MarshalAs(UnmanagedType.I2)]
		short GetTabIndex();

		void Focus();

		void SetAccessKey([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetAccessKey();

		void SetOnblur([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnblur();

		void SetOnfocus([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnfocus();

		void SetOnresize([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnresize();

		void Blur();

		void AddFilter([In][MarshalAs(UnmanagedType.Interface)] object pUnk);

		void RemoveFilter([In][MarshalAs(UnmanagedType.Interface)] object pUnk);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientHeight();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientWidth();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientTop();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientLeft();
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("3050F29C-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHtmlControlRange
	{
		void Select();

		void Add([In][MarshalAs(UnmanagedType.Interface)] IHtmlControlElement item);

		void Remove([In][MarshalAs(UnmanagedType.I4)] int index);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement Item([In][MarshalAs(UnmanagedType.I4)] int index);

		void ScrollIntoView([In][MarshalAs(UnmanagedType.Struct)] object varargStart);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandSupported([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandEnabled([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandState([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandIndeterm([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.BStr)]
		string QueryCommandText([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Struct)]
		object QueryCommandValue([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool ExecCommand([In][MarshalAs(UnmanagedType.BStr)] string cmdID, [In][MarshalAs(UnmanagedType.Bool)] bool showUI, [In][MarshalAs(UnmanagedType.Struct)] object value);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool ExecCommandShowHelp([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement CommonParentElement();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetLength();
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050f65e-98b5-11cf-bb82-00aa00bdce0b")]
	internal interface IHtmlControlRange2
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int addElement([In][MarshalAs(UnmanagedType.Interface)] IHTMLElement element);
	}

	[Guid("3050F3DB-98B5-11CF-BB82-00AA00BDCE0B")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IHTMLCurrentStyle
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPosition();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetStyleFloat();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetColor();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundColor();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontFamily();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontStyle();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontObject();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetFontWeight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetFontSize();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundImage();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundPositionX();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundPositionY();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundRepeat();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderLeftColor();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderTopColor();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderRightColor();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderBottomColor();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderTopStyle();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderRightStyle();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderBottomStyle();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderLeftStyle();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderTopWidth();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderRightWidth();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderBottomWidth();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderLeftWidth();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLeft();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetTop();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetWidth();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetHeight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingLeft();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingTop();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingRight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingBottom();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextAlign();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextDecoration();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDisplay();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetVisibility();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetZIndex();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLetterSpacing();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLineHeight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetTextIndent();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetVerticalAlign();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundAttachment();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginTop();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginRight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginBottom();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginLeft();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClear();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyleType();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStylePosition();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyleImage();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetClipTop();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetClipRight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetClipBottom();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetClipLeft();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOverflow();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPageBreakBefore();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPageBreakAfter();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCursor();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTableLayout();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderCollapse();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDirection();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBehavior();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetUnicodeBidi();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetRight();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBottom();
	}

	[ComImport]
	[ComVisible(true)]
	[Guid("25336920-03F9-11CF-8FD0-00AA00686F13")]
	public class HTMLDocument
	{
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
	internal interface IHTMLDocument
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetScript();
	}

	[ComVisible(true)]
	[Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IHTMLDocument2
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetScript();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetAll();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetBody();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetActiveElement();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetImages();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetApplets();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetLinks();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetForms();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetAnchors();

		void SetTitle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTitle();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetScripts();

		void SetDesignMode([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDesignMode();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLSelectionObject GetSelection();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetReadyState();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetFrames();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetEmbeds();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetPlugins();

		void SetAlinkColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetAlinkColor();

		void SetBgColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBgColor();

		void SetFgColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetFgColor();

		void SetLinkColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLinkColor();

		void SetVlinkColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetVlinkColor();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetReferrer();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetLocation();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetLastModified();

		void SetURL([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetURL();

		void SetDomain([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDomain();

		void SetCookie([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCookie();

		void SetExpando([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetExpando();

		void SetCharset([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCharset();

		void SetDefaultCharset([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDefaultCharset();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMimeType();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFileSize();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFileCreatedDate();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFileModifiedDate();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFileUpdatedDate();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetSecurity();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetProtocol();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetNameProp();

		void DummyWrite([In][MarshalAs(UnmanagedType.I4)] int psarray);

		void DummyWriteln([In][MarshalAs(UnmanagedType.I4)] int psarray);

		[return: MarshalAs(UnmanagedType.Interface)]
		object Open([In][MarshalAs(UnmanagedType.BStr)] string URL, [In][MarshalAs(UnmanagedType.Struct)] object name, [In][MarshalAs(UnmanagedType.Struct)] object features, [In][MarshalAs(UnmanagedType.Struct)] object replace);

		void Close();

		void Clear();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandSupported([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandEnabled([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandState([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandIndeterm([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.BStr)]
		string QueryCommandText([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Struct)]
		object QueryCommandValue([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool ExecCommand([In][MarshalAs(UnmanagedType.BStr)] string cmdID, [In][MarshalAs(UnmanagedType.Bool)] bool showUI, [In][MarshalAs(UnmanagedType.Struct)] object value);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool ExecCommandShowHelp([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement CreateElement([In][MarshalAs(UnmanagedType.BStr)] string eTag);

		void SetOnhelp([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnhelp();

		void SetOnclick([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnclick();

		void SetOndblclick([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndblclick();

		void SetOnkeyup([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeyup();

		void SetOnkeydown([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeydown();

		void SetOnkeypress([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeypress();

		void SetOnmouseup([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmouseup();

		void SetOnmousedown([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmousedown();

		void SetOnmousemove([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmousemove();

		void SetOnmouseout([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmouseout();

		void SetOnmouseover([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmouseover();

		void SetOnreadystatechange([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnreadystatechange();

		void SetOnafterupdate([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnafterupdate();

		void SetOnrowexit([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnrowexit();

		void SetOnrowenter([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnrowenter();

		int SetOndragstart([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndragstart();

		void SetOnselectstart([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnselectstart();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement ElementFromPoint([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLWindow2 GetParentWindow();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyleSheetsCollection GetStyleSheets();

		void SetOnbeforeupdate([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforeupdate();

		void SetOnerrorupdate([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnerrorupdate();

		[return: MarshalAs(UnmanagedType.BStr)]
		string toString();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyleSheet CreateStyleSheet([In][MarshalAs(UnmanagedType.BStr)] string bstrHref, [In][MarshalAs(UnmanagedType.I4)] int lIndex);
	}

	[Guid("3050f662-98b5-11cf-bb82-00aa00bdce0b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IHTMLEditDesigner
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int PreHandleEvent([In] int dispId, [In][MarshalAs(UnmanagedType.Interface)] IHTMLEventObj eventObj);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int PostHandleEvent([In] int dispId, [In][MarshalAs(UnmanagedType.Interface)] IHTMLEventObj eventObj);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateAccelerator([In] int dispId, [In][MarshalAs(UnmanagedType.Interface)] IHTMLEventObj eventObj);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int PostEditorEventNotify([In] int dispId, [In][MarshalAs(UnmanagedType.Interface)] IHTMLEventObj eventObj);
	}

	[Guid("3050f6a0-98b5-11cf-bb82-00aa00bdce0b")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IHTMLEditHost
	{
		void SnapRect([In][MarshalAs(UnmanagedType.Interface)] IHTMLElement pElement, [In][Out] COMRECT rcNew, [In][MarshalAs(UnmanagedType.I4)] int nHandle);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b")]
	internal interface IHTMLEditServices
	{
		[return: MarshalAs(UnmanagedType.I4)]
		int AddDesigner([In][MarshalAs(UnmanagedType.Interface)] IHTMLEditDesigner designer);

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetSelectionServices([In][MarshalAs(UnmanagedType.Interface)] object markupContainer);

		[return: MarshalAs(UnmanagedType.I4)]
		int MoveToSelectionAnchor([In][MarshalAs(UnmanagedType.Interface)] object markupPointer);

		[return: MarshalAs(UnmanagedType.I4)]
		int MoveToSelectionEnd([In][MarshalAs(UnmanagedType.Interface)] object markupPointer);

		[return: MarshalAs(UnmanagedType.I4)]
		int RemoveDesigner([In][MarshalAs(UnmanagedType.Interface)] IHTMLEditDesigner designer);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLElement
	{
		void SetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.Struct)] object AttributeValue, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		void GetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags, [Out][MarshalAs(UnmanagedType.LPArray)] object[] pvars);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool RemoveAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		void SetClassName([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClassName();

		void SetId([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetId();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTagName();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetParentElement();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyle GetStyle();

		void SetOnhelp([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnhelp();

		void SetOnclick([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnclick();

		void SetOndblclick([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndblclick();

		void SetOnkeydown([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeydown();

		void SetOnkeyup([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeyup();

		void SetOnkeypress([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeypress();

		void SetOnmouseout([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmouseout();

		void SetOnmouseover([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmouseover();

		void SetOnmousemove([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmousemove();

		void SetOnmousedown([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmousedown();

		void SetOnmouseup([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmouseup();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetDocument();

		void SetTitle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTitle();

		void SetLanguage([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetLanguage();

		void SetOnselectstart([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnselectstart();

		void ScrollIntoView([In][MarshalAs(UnmanagedType.Struct)] object varargStart);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool Contains([In][MarshalAs(UnmanagedType.Interface)] IHTMLElement pChild);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetSourceIndex();

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetRecordNumber();

		void SetLang([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetLang();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetOffsetLeft();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetOffsetTop();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetOffsetWidth();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetOffsetHeight();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetOffsetParent();

		void SetInnerHTML([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetInnerHTML();

		void SetInnerText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetInnerText();

		void SetOuterHTML([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOuterHTML();

		void SetOuterText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOuterText();

		void InsertAdjacentHTML([In][MarshalAs(UnmanagedType.BStr)] string whereText, [In][MarshalAs(UnmanagedType.BStr)] string html);

		void InsertAdjacentText([In][MarshalAs(UnmanagedType.BStr)] string whereText, [In][MarshalAs(UnmanagedType.BStr)] string text);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetParentTextEdit();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetIsTextEdit();

		void Click();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetFilters();

		void SetOndragstart([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndragstart();

		[return: MarshalAs(UnmanagedType.BStr)]
		string toString();

		void SetOnbeforeupdate([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforeupdate();

		void SetOnafterupdate([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnafterupdate();

		void SetOnerrorupdate([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnerrorupdate();

		void SetOnrowexit([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnrowexit();

		void SetOnrowenter([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnrowenter();

		void SetOndatasetchanged([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndatasetchanged();

		void SetOndataavailable([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndataavailable();

		void SetOndatasetcomplete([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndatasetcomplete();

		void SetOnfilterchange([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnfilterchange();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetChildren();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetAll();
	}

	[ComVisible(true)]
	[Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	internal interface IHTMLElement2
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetScopeName();

		void SetCapture([In][MarshalAs(UnmanagedType.Bool)] bool containerCapture);

		void ReleaseCapture();

		void SetOnlosecapture([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnlosecapture();

		[return: MarshalAs(UnmanagedType.BStr)]
		string ComponentFromPoint([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		void DoScroll([In][MarshalAs(UnmanagedType.Struct)] object component);

		void SetOnscroll([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnscroll();

		void SetOndrag([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndrag();

		void SetOndragend([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndragend();

		void SetOndragenter([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndragenter();

		void SetOndragover([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndragover();

		void SetOndragleave([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndragleave();

		void SetOndrop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOndrop();

		void SetOnbeforecut([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforecut();

		void SetOncut([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOncut();

		void SetOnbeforecopy([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforecopy();

		void SetOncopy([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOncopy();

		void SetOnbeforepaste([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforepaste();

		void SetOnpaste([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnpaste();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLCurrentStyle GetCurrentStyle();

		void SetOnpropertychange([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnpropertychange();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetClientRects();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetBoundingClientRect();

		void SetExpression([In][MarshalAs(UnmanagedType.BStr)] string propname, [In][MarshalAs(UnmanagedType.BStr)] string expression, [In][MarshalAs(UnmanagedType.BStr)] string language);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetExpression([In][MarshalAs(UnmanagedType.BStr)] object propname);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool RemoveExpression([In][MarshalAs(UnmanagedType.BStr)] string propname);

		void SetTabIndex([In][MarshalAs(UnmanagedType.I2)] short p);

		[return: MarshalAs(UnmanagedType.I2)]
		short GetTabIndex();

		void Focus();

		void SetAccessKey([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetAccessKey();

		void SetOnblur([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnblur();

		void SetOnfocus([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnfocus();

		void SetOnresize([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnresize();

		void Blur();

		void AddFilter([In][MarshalAs(UnmanagedType.Interface)] object pUnk);

		void RemoveFilter([In][MarshalAs(UnmanagedType.Interface)] object pUnk);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientHeight();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientWidth();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientTop();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientLeft();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool AttachEvent([In][MarshalAs(UnmanagedType.BStr)] string ev, [In][MarshalAs(UnmanagedType.Interface)] object pdisp);

		void DetachEvent([In][MarshalAs(UnmanagedType.BStr)] string ev, [In][MarshalAs(UnmanagedType.Interface)] object pdisp);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetReadyState();

		void SetOnreadystatechange([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnreadystatechange();

		void SetOnrowsdelete([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnrowsdelete();

		void SetOnrowsinserted([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnrowsinserted();

		void SetOncellchange([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOncellchange();

		void SetDir([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDir();

		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateControlRange();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScrollHeight();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScrollWidth();

		void SetScrollTop([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScrollTop();

		void SetScrollLeft([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScrollLeft();

		void ClearAttributes();

		void MergeAttributes([In][MarshalAs(UnmanagedType.Interface)] IHTMLElement mergeThis);

		void SetOncontextmenu([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOncontextmenu();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement InsertAdjacentElement([In][MarshalAs(UnmanagedType.BStr)] string whereText, [In][MarshalAs(UnmanagedType.Interface)] IHTMLElement insertedElement);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement ApplyElement([In][MarshalAs(UnmanagedType.Interface)] IHTMLElement apply, [In][MarshalAs(UnmanagedType.BStr)] string whereText);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetAdjacentText([In][MarshalAs(UnmanagedType.BStr)] string whereText);

		[return: MarshalAs(UnmanagedType.BStr)]
		string ReplaceAdjacentText([In][MarshalAs(UnmanagedType.BStr)] string whereText, [In][MarshalAs(UnmanagedType.BStr)] string newText);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetCanHaveChildren();

		[return: MarshalAs(UnmanagedType.I4)]
		int AddBehavior([In][MarshalAs(UnmanagedType.BStr)] string bstrUrl, [In] ref object pvarFactory);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool RemoveBehavior([In][MarshalAs(UnmanagedType.I4)] int cookie);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyle GetRuntimeStyle();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetBehaviorUrns();

		void SetTagUrn([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTagUrn();

		void SetOnbeforeeditfocus([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforeeditfocus();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetReadyStateValue();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElementCollection GetElementsByTagName([In][MarshalAs(UnmanagedType.BStr)] string v);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyle GetBaseStyle();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLCurrentStyle GetBaseCurrentStyle();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyle GetBaseRuntimeStyle();

		void SetOnmousehover([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnmousehover();

		void SetOnkeydownpreview([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnkeydownpreview();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetBehavior([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrUrn);
	}

	[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	internal interface IHTMLElementCollection
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		string toString();

		void SetLength([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetLength();

		[return: MarshalAs(UnmanagedType.Interface)]
		object Get_newEnum();

		[return: MarshalAs(UnmanagedType.Interface)]
		object Item([In][MarshalAs(UnmanagedType.Struct)] object name, [In][MarshalAs(UnmanagedType.Struct)] object index);

		[return: MarshalAs(UnmanagedType.Interface)]
		object Tags([In][MarshalAs(UnmanagedType.Struct)] object tagName);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("3050F6C9-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLElementDefaults
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyle GetStyle();

		void SetTabStop([In][MarshalAs(UnmanagedType.Bool)] bool v);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTabStop();

		void SetViewInheritStyle([In][MarshalAs(UnmanagedType.Bool)] bool v);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetViewInheritStyle();

		void SetViewMasterTab([In][MarshalAs(UnmanagedType.Bool)] bool v);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetViewMasterTab();

		void SetScrollSegmentX([In][MarshalAs(UnmanagedType.I4)] int v);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScrollSegmentX();

		void SetScrollSegmentY([In][MarshalAs(UnmanagedType.I4)] object p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScrollSegmentY();

		void SetIsMultiLine([In][MarshalAs(UnmanagedType.Bool)] bool v);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetIsMultiLine();

		void SetContentEditable([In][MarshalAs(UnmanagedType.BStr)] string v);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetContentEditable();

		void SetCanHaveHTML([In][MarshalAs(UnmanagedType.Bool)] bool v);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetCanHaveHTML();

		void SetViewLink([In][MarshalAs(UnmanagedType.Interface)] IHTMLDocument viewLink);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLDocument GetViewLink();

		void SetFrozen([In][MarshalAs(UnmanagedType.Bool)] bool v);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetFrozen();
	}

	[Guid("3050F33C-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IHTMLElementEvents
	{
		void Bogus1();

		void Bogus2();

		void Bogus3();

		void Invoke([In][MarshalAs(UnmanagedType.U4)] int id, [In] ref Guid g, [In][MarshalAs(UnmanagedType.U4)] int lcid, [In][MarshalAs(UnmanagedType.U4)] int dwFlags, [In] DISPPARAMS pdp, [Out][MarshalAs(UnmanagedType.LPArray)] object[] pvarRes, [Out] EXCEPINFO pei, [Out][MarshalAs(UnmanagedType.LPArray)] int[] nArgError);
	}

	[Guid("3050F32D-98B5-11CF-BB82-00AA00BDCE0B")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	internal interface IHTMLEventObj
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetSrcElement();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetAltKey();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetCtrlKey();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetShiftKey();

		void SetReturnValue([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetReturnValue();

		void SetCancelBubble([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetCancelBubble();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetFromElement();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetToElement();

		void SetKeyCode([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetKeyCode();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetButton();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetEventType();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetQualifier();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetReason();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetX();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetY();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientX();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientY();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetOffsetX();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetOffsetY();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScreenX();

		[return: MarshalAs(UnmanagedType.I4)]
		int GetScreenY();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetSrcFilter();
	}

	[Guid("3050F6A6-98B5-11CF-BB82-00AA00BDCE0B")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IHTMLPainter
	{
		void Draw([In][MarshalAs(UnmanagedType.I4)] int leftBounds, [In][MarshalAs(UnmanagedType.I4)] int topBounds, [In][MarshalAs(UnmanagedType.I4)] int rightBounds, [In][MarshalAs(UnmanagedType.I4)] int bottomBounds, [In][MarshalAs(UnmanagedType.I4)] int leftUpdate, [In][MarshalAs(UnmanagedType.I4)] int topUpdate, [In][MarshalAs(UnmanagedType.I4)] int rightUpdate, [In][MarshalAs(UnmanagedType.I4)] int bottomUpdate, [In][MarshalAs(UnmanagedType.U4)] int lDrawFlags, [In] IntPtr hdc, [In] IntPtr pvDrawObject);

		void OnResize([In][MarshalAs(UnmanagedType.I4)] int cx, [In][MarshalAs(UnmanagedType.I4)] int cy);

		void GetPainterInfo([Out] HTML_PAINTER_INFO htmlPainterInfo);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool HitTestPoint([In][MarshalAs(UnmanagedType.I4)] int ptx, [In][MarshalAs(UnmanagedType.I4)] int pty, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pbHit, [Out][MarshalAs(UnmanagedType.LPArray)] int[] plPartID);
	}

	[Guid("3050f6a7-98b5-11cf-bb82-00aa00bdce0b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IHTMLPaintSite
	{
		void InvalidatePainterInfo();

		void InvalidateRect([In] IntPtr pRect);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050F3CF-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLRuleStyle
	{
		void SetFontFamily([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontFamily();

		void SetFontStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontStyle();

		void SetFontObject([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontObject();

		void SetFontWeight([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontWeight();

		void SetFontSize([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetFontSize();

		void SetFont([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFont();

		void SetColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetColor();

		void SetBackground([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackground();

		void SetBackgroundColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundColor();

		void SetBackgroundImage([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundImage();

		void SetBackgroundRepeat([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundRepeat();

		void SetBackgroundAttachment([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundAttachment();

		void SetBackgroundPosition([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundPosition();

		void SetBackgroundPositionX([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundPositionX();

		void SetBackgroundPositionY([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundPositionY();

		void SetWordSpacing([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetWordSpacing();

		void SetLetterSpacing([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLetterSpacing();

		void SetTextDecoration([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextDecoration();

		void SetTextDecorationNone([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationNone();

		void SetTextDecorationUnderline([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationUnderline();

		void SetTextDecorationOverline([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationOverline();

		void SetTextDecorationLineThrough([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationLineThrough();

		void SetTextDecorationBlink([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationBlink();

		void SetVerticalAlign([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetVerticalAlign();

		void SetTextTransform([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextTransform();

		void SetTextAlign([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextAlign();

		void SetTextIndent([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetTextIndent();

		void SetLineHeight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLineHeight();

		void SetMarginTop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginTop();

		void SetMarginRight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginRight();

		void SetMarginBottom([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginBottom();

		void SetMarginLeft([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginLeft();

		void SetMargin([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMargin();

		void SetPaddingTop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingTop();

		void SetPaddingRight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingRight();

		void SetPaddingBottom([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingBottom();

		void SetPaddingLeft([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingLeft();

		void SetPadding([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPadding();

		void SetBorder([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorder();

		void SetBorderTop([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderTop();

		void SetBorderRight([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderRight();

		void SetBorderBottom([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderBottom();

		void SetBorderLeft([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderLeft();

		void SetBorderColor([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderColor();

		void SetBorderTopColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderTopColor();

		void SetBorderRightColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderRightColor();

		void SetBorderBottomColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderBottomColor();

		void SetBorderLeftColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderLeftColor();

		void SetBorderWidth([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderWidth();

		void SetBorderTopWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderTopWidth();

		void SetBorderRightWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderRightWidth();

		void SetBorderBottomWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderBottomWidth();

		void SetBorderLeftWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderLeftWidth();

		void SetBorderStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderStyle();

		void SetBorderTopStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderTopStyle();

		void SetBorderRightStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderRightStyle();

		void SetBorderBottomStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderBottomStyle();

		void SetBorderLeftStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderLeftStyle();

		void SetWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetWidth();

		void SetHeight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetHeight();

		void SetStyleFloat([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetStyleFloat();

		void SetClear([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClear();

		void SetDisplay([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDisplay();

		void SetVisibility([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetVisibility();

		void SetListStyleType([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyleType();

		void SetListStylePosition([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStylePosition();

		void SetListStyleImage([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyleImage();

		void SetListStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyle();

		void SetWhiteSpace([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWhiteSpace();

		void SetTop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetTop();

		void SetLeft([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLeft();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPosition();

		void SetZIndex([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetZIndex();

		void SetOverflow([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOverflow();

		void SetPageBreakBefore([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPageBreakBefore();

		void SetPageBreakAfter([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPageBreakAfter();

		void SetCssText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCssText();

		void SetCursor([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCursor();

		void SetClip([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClip();

		void SetFilter([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFilter();

		void SetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.Struct)] object AttributeValue, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool RemoveAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("3050F25A-98B5-11CF-BB82-00AA00BDCE0B")]
	[ComVisible(true)]
	public interface IHTMLSelectionObject
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateRange();

		void Empty();

		void Clear();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetSelectionType();
	}

	[ComVisible(true)]
	[Guid("3050f230-98b5-11cf-bb82-00aa00bdce0b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IHTMLTextContainer
	{
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object createControlRange();

		int get_ScrollHeight();

		int get_ScrollWidth();

		int get_ScrollTop();

		int get_ScrollLeft();

		void put_ScrollHeight(int i);

		void put_ScrollWidth(int i);

		void put_ScrollTop(int i);

		void put_ScrollLeft(int i);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050F220-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLTxtRange
	{
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetHtmlText();

		void SetText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetText();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement ParentElement();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLTxtRange Duplicate();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool InRange([In][MarshalAs(UnmanagedType.Interface)] IHTMLTxtRange range);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsEqual([In][MarshalAs(UnmanagedType.Interface)] IHTMLTxtRange range);

		void ScrollIntoView([In][MarshalAs(UnmanagedType.Bool)] bool fStart);

		void Collapse([In][MarshalAs(UnmanagedType.Bool)] bool Start);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool Expand([In][MarshalAs(UnmanagedType.BStr)] string Unit);

		[return: MarshalAs(UnmanagedType.I4)]
		int Move([In][MarshalAs(UnmanagedType.BStr)] string Unit, [In][MarshalAs(UnmanagedType.I4)] int Count);

		[return: MarshalAs(UnmanagedType.I4)]
		int MoveStart([In][MarshalAs(UnmanagedType.BStr)] string Unit, [In][MarshalAs(UnmanagedType.I4)] int Count);

		[return: MarshalAs(UnmanagedType.I4)]
		int MoveEnd([In][MarshalAs(UnmanagedType.BStr)] string Unit, [In][MarshalAs(UnmanagedType.I4)] int Count);

		void Select();

		void PasteHTML([In][MarshalAs(UnmanagedType.BStr)] string html);

		void MoveToElementText([In][MarshalAs(UnmanagedType.Interface)] IHTMLElement element);

		void SetEndPoint([In][MarshalAs(UnmanagedType.BStr)] string how, [In][MarshalAs(UnmanagedType.Interface)] IHTMLTxtRange SourceRange);

		[return: MarshalAs(UnmanagedType.I4)]
		int CompareEndPoints([In][MarshalAs(UnmanagedType.BStr)] string how, [In][MarshalAs(UnmanagedType.Interface)] IHTMLTxtRange SourceRange);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool FindText([In][MarshalAs(UnmanagedType.BStr)] string String, [In][MarshalAs(UnmanagedType.I4)] int Count, [In][MarshalAs(UnmanagedType.I4)] int Flags);

		void MoveToPoint([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBookmark();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool MoveToBookmark([In][MarshalAs(UnmanagedType.BStr)] string Bookmark);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandSupported([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandEnabled([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandState([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryCommandIndeterm([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.BStr)]
		string QueryCommandText([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Struct)]
		object QueryCommandValue([In][MarshalAs(UnmanagedType.BStr)] string cmdID);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool ExecCommand([In][MarshalAs(UnmanagedType.BStr)] string cmdID, [In][MarshalAs(UnmanagedType.Bool)] bool showUI, [In][MarshalAs(UnmanagedType.Struct)] object value);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool ExecCommandShowHelp([In][MarshalAs(UnmanagedType.BStr)] string cmdID);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLStyle
	{
		void SetFontFamily([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontFamily();

		void SetFontStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontStyle();

		void SetFontObject([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontObject();

		void SetFontWeight([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFontWeight();

		void SetFontSize([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetFontSize();

		void SetFont([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFont();

		void SetColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetColor();

		void SetBackground([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackground();

		void SetBackgroundColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundColor();

		void SetBackgroundImage([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundImage();

		void SetBackgroundRepeat([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundRepeat();

		void SetBackgroundAttachment([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundAttachment();

		void SetBackgroundPosition([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBackgroundPosition();

		void SetBackgroundPositionX([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundPositionX();

		void SetBackgroundPositionY([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBackgroundPositionY();

		void SetWordSpacing([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetWordSpacing();

		void SetLetterSpacing([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLetterSpacing();

		void SetTextDecoration([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextDecoration();

		void SetTextDecorationNone([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationNone();

		void SetTextDecorationUnderline([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationUnderline();

		void SetTextDecorationOverline([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationOverline();

		void SetTextDecorationLineThrough([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationLineThrough();

		void SetTextDecorationBlink([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetTextDecorationBlink();

		void SetVerticalAlign([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetVerticalAlign();

		void SetTextTransform([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextTransform();

		void SetTextAlign([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTextAlign();

		void SetTextIndent([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetTextIndent();

		void SetLineHeight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLineHeight();

		void SetMarginTop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginTop();

		void SetMarginRight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginRight();

		void SetMarginBottom([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginBottom();

		void SetMarginLeft([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetMarginLeft();

		void SetMargin([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMargin();

		void SetPaddingTop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingTop();

		void SetPaddingRight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingRight();

		void SetPaddingBottom([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingBottom();

		void SetPaddingLeft([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetPaddingLeft();

		void SetPadding([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPadding();

		void SetBorder([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorder();

		void SetBorderTop([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderTop();

		void SetBorderRight([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderRight();

		void SetBorderBottom([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderBottom();

		void SetBorderLeft([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderLeft();

		void SetBorderColor([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderColor();

		void SetBorderTopColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderTopColor();

		void SetBorderRightColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderRightColor();

		void SetBorderBottomColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderBottomColor();

		void SetBorderLeftColor([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderLeftColor();

		void SetBorderWidth([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderWidth();

		void SetBorderTopWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderTopWidth();

		void SetBorderRightWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderRightWidth();

		void SetBorderBottomWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderBottomWidth();

		void SetBorderLeftWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetBorderLeftWidth();

		void SetBorderStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderStyle();

		void SetBorderTopStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderTopStyle();

		void SetBorderRightStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderRightStyle();

		void SetBorderBottomStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderBottomStyle();

		void SetBorderLeftStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetBorderLeftStyle();

		void SetWidth([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetWidth();

		void SetHeight([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetHeight();

		void SetStyleFloat([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetStyleFloat();

		void SetClear([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClear();

		void SetDisplay([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDisplay();

		void SetVisibility([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetVisibility();

		void SetListStyleType([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyleType();

		void SetListStylePosition([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStylePosition();

		void SetListStyleImage([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyleImage();

		void SetListStyle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetListStyle();

		void SetWhiteSpace([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetWhiteSpace();

		void SetTop([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetTop();

		void SetLeft([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetLeft();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPosition();

		void SetZIndex([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetZIndex();

		void SetOverflow([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOverflow();

		void SetPageBreakBefore([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPageBreakBefore();

		void SetPageBreakAfter([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPageBreakAfter();

		void SetCssText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCssText();

		void SetPixelTop([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetPixelTop();

		void SetPixelLeft([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetPixelLeft();

		void SetPixelWidth([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetPixelWidth();

		void SetPixelHeight([In][MarshalAs(UnmanagedType.I4)] int p);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetPixelHeight();

		void SetPosTop([In][MarshalAs(UnmanagedType.R4)] float p);

		[return: MarshalAs(UnmanagedType.R4)]
		float GetPosTop();

		void SetPosLeft([In][MarshalAs(UnmanagedType.R4)] float p);

		[return: MarshalAs(UnmanagedType.R4)]
		float GetPosLeft();

		void SetPosWidth([In][MarshalAs(UnmanagedType.R4)] float p);

		[return: MarshalAs(UnmanagedType.R4)]
		float GetPosWidth();

		void SetPosHeight([In][MarshalAs(UnmanagedType.R4)] float p);

		[return: MarshalAs(UnmanagedType.R4)]
		float GetPosHeight();

		void SetCursor([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCursor();

		void SetClip([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetClip();

		void SetFilter([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetFilter();

		void SetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.Struct)] object AttributeValue, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool RemoveAttribute([In][MarshalAs(UnmanagedType.BStr)] string strAttributeName, [In][MarshalAs(UnmanagedType.I4)] int lFlags);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLStyleSheet
	{
		void SetTitle([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTitle();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyleSheet GetParentStyleSheet();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLElement GetOwningElement();

		void SetDisabled([In][MarshalAs(UnmanagedType.Bool)] bool p);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetDisabled();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetReadOnly();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyleSheetsCollection GetImports();

		void SetHref([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetHref();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetStyleSheetType();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetId();

		[return: MarshalAs(UnmanagedType.I4)]
		int AddImport([In][MarshalAs(UnmanagedType.BStr)] string bstrURL, [In][MarshalAs(UnmanagedType.I4)] int lIndex);

		[return: MarshalAs(UnmanagedType.I4)]
		int AddRule([In][MarshalAs(UnmanagedType.BStr)] string bstrSelector, [In][MarshalAs(UnmanagedType.BStr)] string bstrStyle, [In][MarshalAs(UnmanagedType.I4)] int lIndex);

		void RemoveImport([In][MarshalAs(UnmanagedType.I4)] int lIndex);

		void RemoveRule([In][MarshalAs(UnmanagedType.I4)] int lIndex);

		void SetMedia([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetMedia();

		void SetCssText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetCssText();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyleSheetRulesCollection GetRules();
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050F37E-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLStyleSheetsCollection
	{
		[return: MarshalAs(UnmanagedType.I4)]
		int GetLength();

		[return: MarshalAs(UnmanagedType.Interface)]
		object Get_newEnum();

		[return: MarshalAs(UnmanagedType.Struct)]
		object Item([In] ref object pvarIndex);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("3050F357-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLStyleSheetRule
	{
		void SetSelectorText([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetSelectorText();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLRuleStyle GetStyle();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetReadOnly();
	}

	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	[Guid("3050F2E5-98B5-11CF-BB82-00AA00BDCE0B")]
	internal interface IHTMLStyleSheetRulesCollection
	{
		[return: MarshalAs(UnmanagedType.I4)]
		int GetLength();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLStyleSheetRule Item([In][MarshalAs(UnmanagedType.I4)] int index);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("332C4427-26CB-11D0-B483-00C04FD90119")]
	internal interface IHTMLWindow2
	{
		[return: MarshalAs(UnmanagedType.Struct)]
		object Item([In] ref object pvarIndex);

		[return: MarshalAs(UnmanagedType.I4)]
		int GetLength();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetFrames();

		void SetDefaultStatus([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDefaultStatus();

		void SetStatus([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetStatus();

		[return: MarshalAs(UnmanagedType.I4)]
		int SetTimeout([In][MarshalAs(UnmanagedType.BStr)] string expression, [In][MarshalAs(UnmanagedType.I4)] int msec, [In] ref object language);

		void ClearTimeout([In][MarshalAs(UnmanagedType.I4)] int timerID);

		void Alert([In][MarshalAs(UnmanagedType.BStr)] string message);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool Confirm([In][MarshalAs(UnmanagedType.BStr)] string message);

		[return: MarshalAs(UnmanagedType.Struct)]
		object Prompt([In][MarshalAs(UnmanagedType.BStr)] string message, [In][MarshalAs(UnmanagedType.BStr)] string defstr);

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetImage();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetLocation();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetHistory();

		void Close();

		void SetOpener([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOpener();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetNavigator();

		void SetName([In][MarshalAs(UnmanagedType.BStr)] string p);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetName();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLWindow2 GetParent();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLWindow2 Open([In][MarshalAs(UnmanagedType.BStr)] string URL, [In][MarshalAs(UnmanagedType.BStr)] string name, [In][MarshalAs(UnmanagedType.BStr)] string features, [In][MarshalAs(UnmanagedType.Bool)] bool replace);

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLWindow2 GetSelf();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLWindow2 GetTop();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLWindow2 GetWindow();

		void Navigate([In][MarshalAs(UnmanagedType.BStr)] string URL);

		void SetOnfocus([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnfocus();

		void SetOnblur([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnblur();

		void SetOnload([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnload();

		void SetOnbeforeunload([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnbeforeunload();

		void SetOnunload([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnunload();

		void SetOnhelp([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnhelp();

		void SetOnerror([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnerror();

		void SetOnresize([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnresize();

		void SetOnscroll([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOnscroll();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLDocument2 GetDocument();

		[return: MarshalAs(UnmanagedType.Interface)]
		IHTMLEventObj GetEvent();

		[return: MarshalAs(UnmanagedType.Interface)]
		object Get_newEnum();

		[return: MarshalAs(UnmanagedType.Struct)]
		object ShowModalDialog([In][MarshalAs(UnmanagedType.BStr)] string dialog, [In] ref object varArgIn, [In] ref object varOptions);

		void ShowHelp([In][MarshalAs(UnmanagedType.BStr)] string helpURL, [In][MarshalAs(UnmanagedType.Struct)] object helpArg, [In][MarshalAs(UnmanagedType.BStr)] string features);

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetScreen();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetOption();

		void Focus();

		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetClosed();

		void Blur();

		void Scroll([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetClientInformation();

		[return: MarshalAs(UnmanagedType.I4)]
		int SetInterval([In][MarshalAs(UnmanagedType.BStr)] string expression, [In][MarshalAs(UnmanagedType.I4)] int msec, [In] ref object language);

		void ClearInterval([In][MarshalAs(UnmanagedType.I4)] int timerID);

		void SetOffscreenBuffering([In][MarshalAs(UnmanagedType.Struct)] object p);

		[return: MarshalAs(UnmanagedType.Struct)]
		object GetOffscreenBuffering();

		[return: MarshalAs(UnmanagedType.Struct)]
		object ExecScript([In][MarshalAs(UnmanagedType.BStr)] string code, [In][MarshalAs(UnmanagedType.BStr)] string language);

		void ScrollBy([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		void ScrollTo([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		void MoveTo([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		void MoveBy([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		void ResizeTo([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		void ResizeBy([In][MarshalAs(UnmanagedType.I4)] int x, [In][MarshalAs(UnmanagedType.I4)] int y);

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetExternal();
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000F-0000-0000-C000-000000000046")]
	internal interface IMoniker
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int IsDirty();

		void Load([In][MarshalAs(UnmanagedType.Interface)] IStream pstm);

		void Save([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In][MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		[return: MarshalAs(UnmanagedType.I8)]
		long GetSizeMax();

		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToObject([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkToLeft, [In] ref Guid riidResult);

		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToStorage([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkToLeft, [In] ref Guid riidResult);

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker Reduce([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.I4)] int dwReduceHowFar, [In][Out][MarshalAs(UnmanagedType.Interface)] IMoniker pMkToLeft);

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker Reduce([In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkRight, [In][MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric);

		[return: MarshalAs(UnmanagedType.Interface)]
		object Reduce([In][MarshalAs(UnmanagedType.Bool)] bool fForward);

		void IsEqual([In][MarshalAs(UnmanagedType.Interface)] IMoniker pOtherMoniker);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Hash();

		void IsRunning([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkToLeft, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkNewlyRunning);

		[return: MarshalAs(UnmanagedType.LPStruct)]
		object GetTimeOfLastChange([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkToLeft);

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker Inverse();

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker CommonPrefixWith([In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkOther);

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker RelativePathTo([In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkOther);

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDisplayName([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkOther);

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker ParseDisplayName([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pMkToLeft, [In][MarshalAs(UnmanagedType.BStr)] string pszDisplayName, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pchEaten);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int IsSystemMoniker();
	}

	[Guid("00000118-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IOleClientSite
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int SaveObject();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwAssign, [In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object ppmk);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetContainer([MarshalAs(UnmanagedType.Interface)] out IOleContainer container);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ShowObject();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnShowWindow([In][MarshalAs(UnmanagedType.I4)] int fShow);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int RequestNewObjectLayout();
	}

	[ComImport]
	[ComVisible(true)]
	[Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IOleCommandTarget
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In][Out] tagOLECMD prgCmds, [In][Out] int pCmdText);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [In][MarshalAs(UnmanagedType.LPArray)] object[] pvaIn, [Out][MarshalAs(UnmanagedType.LPArray)] object[] pvaOut);
	}

	[Guid("0000011B-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IOleContainer
	{
		void ParseDisplayName([In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.BStr)] string pszDisplayName, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pchEaten, [Out][MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);

		void EnumObjects([In][MarshalAs(UnmanagedType.U4)] int grfFlags, [Out][MarshalAs(UnmanagedType.LPArray)] object[] ppenum);

		void LockContainer([In][MarshalAs(UnmanagedType.I4)] int fLock);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("0000010E-0000-0000-C000-000000000046")]
	internal interface IOleDataObject
	{
		int OleGetData(FORMATETC pFormatetc, [Out] STGMEDIUM pMedium);

		int OleGetDataHere(FORMATETC pFormatetc, [In][Out] STGMEDIUM pMedium);

		int OleQueryGetData(FORMATETC pFormatetc);

		int OleGetCanonicalFormatEtc(FORMATETC pformatectIn, [Out] FORMATETC pformatetcOut);

		int OleSetData(FORMATETC pFormatectIn, STGMEDIUM pmedium, int fRelease);

		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumFORMATETC OleEnumFormatEtc([In][MarshalAs(UnmanagedType.U4)] int dwDirection);

		int OleDAdvise(FORMATETC pFormatetc, [In][MarshalAs(UnmanagedType.U4)] int advf, [In][MarshalAs(UnmanagedType.Interface)] object pAdvSink, [Out][MarshalAs(UnmanagedType.LPArray)] int[] pdwConnection);

		int OleDUnadvise([In][MarshalAs(UnmanagedType.U4)] int dwConnection);

		int OleEnumDAdvise([Out][MarshalAs(UnmanagedType.LPArray)] object[] ppenumAdvise);
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B722BCC7-4E68-101B-A2BC-00AA00404770")]
	internal interface IOleDocumentSite
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int ActivateMe([In][MarshalAs(UnmanagedType.Interface)] IOleDocumentView pViewToActivate);
	}

	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B722BCC6-4E68-101B-A2BC-00AA00404770")]
	internal interface IOleDocumentView
	{
		void SetInPlaceSite([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceSite pIPSite);

		[return: MarshalAs(UnmanagedType.Interface)]
		IOleInPlaceSite GetInPlaceSite();

		[return: MarshalAs(UnmanagedType.Interface)]
		object GetDocument();

		void SetRect([In] COMRECT prcView);

		void GetRect([Out] COMRECT prcView);

		void SetRectComplex([In] COMRECT prcView, [In] COMRECT prcHScroll, [In] COMRECT prcVScroll, [In] COMRECT prcSizeBox);

		void Show([In][MarshalAs(UnmanagedType.I4)] int fShow);

		void UIActivate([In][MarshalAs(UnmanagedType.I4)] int fUIActivate);

		void Open();

		void CloseView([In][MarshalAs(UnmanagedType.U4)] int dwReserved);

		void SaveViewState([In][MarshalAs(UnmanagedType.Interface)] IStream pstm);

		void ApplyViewState([In][MarshalAs(UnmanagedType.Interface)] IStream pstm);

		void Clone([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceSite pIPSiteNew, [Out][MarshalAs(UnmanagedType.LPArray)] IOleDocumentView[] ppViewNew);
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000121-0000-0000-C000-000000000046")]
	internal interface IOleDropSource
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int QueryContinueDrag([In][MarshalAs(UnmanagedType.I4)] int fEscapePressed, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GiveFeedback([In][MarshalAs(UnmanagedType.U4)] int dwEffect);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("00000122-0000-0000-C000-000000000046")]
	internal interface IOleDropTarget
	{
		[PreserveSig]
		int OleDragEnter(IntPtr pDataObj, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In][MarshalAs(UnmanagedType.U8)] long pt, [In][Out] ref int pdwEffect);

		[PreserveSig]
		int OleDragOver([In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In][MarshalAs(UnmanagedType.U8)] long pt, [In][Out] ref int pdwEffect);

		[PreserveSig]
		int OleDragLeave();

		[PreserveSig]
		int OleDrop(IntPtr pDataObj, [In][MarshalAs(UnmanagedType.U4)] int grfKeyState, [In][MarshalAs(UnmanagedType.U8)] long pt, [In][Out] ref int pdwEffect);
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000117-0000-0000-C000-000000000046")]
	internal interface IOleInPlaceActiveObject
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetWindow(out IntPtr hwnd);

		void ContextSensitiveHelp([In][MarshalAs(UnmanagedType.I4)] int fEnterMode);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateAccelerator([In][MarshalAs(UnmanagedType.LPStruct)] COMMSG lpmsg);

		void OnFrameWindowActivate([In][MarshalAs(UnmanagedType.I4)] int fActivate);

		void OnDocWindowActivate([In][MarshalAs(UnmanagedType.I4)] int fActivate);

		void ResizeBorder([In] COMRECT prcBorder, [In] IOleInPlaceUIWindow pUIWindow, [In][MarshalAs(UnmanagedType.I4)] int fFrameWindow);

		void EnableModeless([In][MarshalAs(UnmanagedType.I4)] int fEnable);
	}

	[ComImport]
	[Guid("00000116-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IOleInPlaceFrame
	{
		IntPtr GetWindow();

		void ContextSensitiveHelp([In][MarshalAs(UnmanagedType.I4)] int fEnterMode);

		void GetBorder([Out] COMRECT lprectBorder);

		void RequestBorderSpace([In] COMRECT pborderwidths);

		void SetBorderSpace([In] COMRECT pborderwidths);

		void SetActiveObject([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In][MarshalAs(UnmanagedType.LPWStr)] string pszObjName);

		void InsertMenus([In] IntPtr hmenuShared, [In][Out] tagOleMenuGroupWidths lpMenuWidths);

		void SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

		void RemoveMenus([In] IntPtr hmenuShared);

		void SetStatusText([In][MarshalAs(UnmanagedType.BStr)] string pszStatusText);

		void EnableModeless([In][MarshalAs(UnmanagedType.I4)] int fEnable);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int TranslateAccelerator([In][MarshalAs(UnmanagedType.LPStruct)] COMMSG lpmsg, [In][MarshalAs(UnmanagedType.U2)] short wID);
	}

	[ComImport]
	[ComVisible(true)]
	[Guid("00000113-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IOleInPlaceObject
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetWindow(out IntPtr hwnd);

		void ContextSensitiveHelp([In][MarshalAs(UnmanagedType.I4)] int fEnterMode);

		void InPlaceDeactivate();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int UIDeactivate();

		void SetObjectRects([In] COMRECT lprcPosRect, [In] COMRECT lprcClipRect);

		void ReactivateAndUndo();
	}

	[ComImport]
	[ComVisible(true)]
	[Guid("00000119-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IOleInPlaceSite
	{
		IntPtr GetWindow();

		void ContextSensitiveHelp([In][MarshalAs(UnmanagedType.I4)] int fEnterMode);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int CanInPlaceActivate();

		void OnInPlaceActivate();

		void OnUIActivate();

		void GetWindowContext(out IOleInPlaceFrame ppFrame, out IOleInPlaceUIWindow ppDoc, [Out] COMRECT lprcPosRect, [Out] COMRECT lprcClipRect, [In][Out] tagOIFI lpFrameInfo);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Scroll([In][MarshalAs(UnmanagedType.U4)] tagSIZE scrollExtent);

		void OnUIDeactivate([In][MarshalAs(UnmanagedType.I4)] int fUndoable);

		void OnInPlaceDeactivate();

		void DiscardUndoState();

		void DeactivateAndUndo();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnPosRectChange([In] COMRECT lprcPosRect);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("00000115-0000-0000-C000-000000000046")]
	internal interface IOleInPlaceUIWindow
	{
		IntPtr GetWindow();

		void ContextSensitiveHelp([In][MarshalAs(UnmanagedType.I4)] int fEnterMode);

		void GetBorder([Out] COMRECT lprectBorder);

		void RequestBorderSpace([In] COMRECT pborderwidths);

		void SetBorderSpace([In] COMRECT pborderwidths);

		void SetActiveObject([In][MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In][MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
	}

	[ComImport]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000112-0000-0000-C000-000000000046")]
	internal interface IOleObject
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int SetClientSite([In][MarshalAs(UnmanagedType.Interface)] IOleClientSite pClientSite);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetClientSite(out IOleClientSite site);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int SetHostNames([In][MarshalAs(UnmanagedType.LPWStr)] string szContainerApp, [In][MarshalAs(UnmanagedType.LPWStr)] string szContainerObj);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Close([In][MarshalAs(UnmanagedType.I4)] int dwSaveOption);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int SetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, [In][MarshalAs(UnmanagedType.Interface)] object pmk);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetMoniker([In][MarshalAs(UnmanagedType.U4)] int dwAssign, [In][MarshalAs(UnmanagedType.U4)] int dwWhichMoniker, out object moniker);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int InitFromData([In][MarshalAs(UnmanagedType.Interface)] IOleDataObject pDataObject, [In][MarshalAs(UnmanagedType.I4)] int fCreation, [In][MarshalAs(UnmanagedType.U4)] int dwReserved);

		int GetClipboardData([In][MarshalAs(UnmanagedType.U4)] int dwReserved, out IOleDataObject data);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int DoVerb([In][MarshalAs(UnmanagedType.I4)] int iVerb, [In] IntPtr lpmsg, [In][MarshalAs(UnmanagedType.Interface)] IOleClientSite pActiveSite, [In][MarshalAs(UnmanagedType.I4)] int lindex, [In] IntPtr hwndParent, [In] COMRECT lprcPosRect);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int EnumVerbs(out IEnumOLEVERB e);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OleUpdate();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int IsUpToDate();

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetUserClassID([In][Out] ref Guid pClsid);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetUserType([In][MarshalAs(UnmanagedType.U4)] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int SetExtent([In][MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [In] tagSIZEL pSizel);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetExtent([In][MarshalAs(UnmanagedType.U4)] int dwDrawAspect, [Out] tagSIZEL pSizel);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Advise([In][MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink, out int cookie);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Unadvise([In][MarshalAs(UnmanagedType.U4)] int dwConnection);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int EnumAdvise(out IEnumSTATDATA e);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetMiscStatus([In][MarshalAs(UnmanagedType.U4)] int dwAspect, out int misc);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int SetColorScheme([In] tagLOGPALETTE pLogpal);
	}

	[ComVisible(true)]
	[Guid("894AD3B0-EF97-11CE-9BC9-00AA00608E01")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IOleUndoUnit
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Do([In][MarshalAs(UnmanagedType.Interface)] IOleUndoManager undoManager);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetDescription([MarshalAs(UnmanagedType.BStr)] out string bStr);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetUnitType([MarshalAs(UnmanagedType.I4)] out int clsid, [MarshalAs(UnmanagedType.I4)] out int plID);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int OnNextAdd();
	}

	[Guid("A1FAF330-EF97-11CE-9BC9-00AA00608E01")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IOleParentUndoUnit : IOleUndoUnit
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Open([In][MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUnit);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Close([In][MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUnit, [In][MarshalAs(UnmanagedType.Bool)] bool fCommit);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Add([In][MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int FindUnit([In][MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetParentState([MarshalAs(UnmanagedType.I8)] out long state);
	}

	[ComImport]
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	internal interface IOleServiceProvider
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int QueryService([In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("D001F200-EF97-11CE-9BC9-00AA00608E01")]
	internal interface IOleUndoManager
	{
		void Open([In][MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUndo);

		void Close([In][MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUndo, [In][MarshalAs(UnmanagedType.Bool)] bool fCommit);

		void Add([In][MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

		[return: MarshalAs(UnmanagedType.I8)]
		long GetOpenParentState();

		void DiscardFrom([In][MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

		void UndoTo([In][MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

		void RedoTo([In][MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumOleUndoUnits EnumUndoable();

		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumOleUndoUnits EnumRedoable();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetLastUndoDescription();

		[return: MarshalAs(UnmanagedType.BStr)]
		string GetLastRedoDescription();

		void Enable([In][MarshalAs(UnmanagedType.Bool)] bool fEnable);
	}

	[ComVisible(true)]
	[Guid("79eac9c9-baf9-11ce-8c82-00aa004ba90b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPersistMoniker
	{
		void GetClassID([In][Out] ref Guid pClassID);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int IsDirty();

		void Load([In] int fFullyAvailable, [In][MarshalAs(UnmanagedType.Interface)] IMoniker pmk, [In][MarshalAs(UnmanagedType.Interface)] object pbc, [In] int grfMode);

		void Save([In][MarshalAs(UnmanagedType.Interface)] IMoniker pimkName, [In][MarshalAs(UnmanagedType.Interface)] object pbc, [In][MarshalAs(UnmanagedType.Bool)] bool fRemember);

		void SaveCompleted([In][MarshalAs(UnmanagedType.Interface)] IMoniker pmk, [In][MarshalAs(UnmanagedType.Interface)] object pbc);

		[return: MarshalAs(UnmanagedType.Interface)]
		IMoniker GetCurMoniker();
	}

	[ComImport]
	[Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPersistStreamInit
	{
		void GetClassID([In][Out] ref Guid pClassID);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int IsDirty();

		void Load([In][MarshalAs(UnmanagedType.Interface)] IStream pstm);

		void Save([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In][MarshalAs(UnmanagedType.I4)] int fClearDirty);

		void GetSizeMax([Out][MarshalAs(UnmanagedType.LPArray)] long pcbSize);

		void InitNew();
	}

	[Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IPropertyNotifySink
	{
		void OnChanged(int dispID);

		void OnRequestEdit(int dispID);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	[ComVisible(true)]
	public interface IServiceProvider
	{
		[return: MarshalAs(UnmanagedType.I4)]
		int QueryService([In] ref Guid sid, [In] ref Guid iid, out IntPtr service);
	}

	[Guid("0000000C-0000-0000-C000-000000000046")]
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStream
	{
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Read([In] IntPtr buf, [In][MarshalAs(UnmanagedType.I4)] int len);

		[PreserveSig]
		[return: MarshalAs(UnmanagedType.I4)]
		int Write([In] IntPtr buf, [In][MarshalAs(UnmanagedType.I4)] int len);

		[return: MarshalAs(UnmanagedType.I8)]
		long Seek([In][MarshalAs(UnmanagedType.I8)] long dlibMove, [In][MarshalAs(UnmanagedType.I4)] int dwOrigin);

		void SetSize([In][MarshalAs(UnmanagedType.I8)] long libNewSize);

		[return: MarshalAs(UnmanagedType.I8)]
		long CopyTo([In][MarshalAs(UnmanagedType.Interface)] IStream pstm, [In][MarshalAs(UnmanagedType.I8)] long cb, [Out][MarshalAs(UnmanagedType.LPArray)] long[] pcbRead);

		void Commit([In][MarshalAs(UnmanagedType.I4)] int grfCommitFlags);

		void Revert();

		void LockRegion([In][MarshalAs(UnmanagedType.I8)] long libOffset, [In][MarshalAs(UnmanagedType.I8)] long cb, [In][MarshalAs(UnmanagedType.I4)] int dwLockType);

		void UnlockRegion([In][MarshalAs(UnmanagedType.I8)] long libOffset, [In][MarshalAs(UnmanagedType.I8)] long cb, [In][MarshalAs(UnmanagedType.I4)] int dwLockType);

		void Stat([Out] STATSTG pstatstg, [In][MarshalAs(UnmanagedType.I4)] int grfStatFlag);

		[return: MarshalAs(UnmanagedType.Interface)]
		IStream Clone();
	}

	[ComImport]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IConnectionPoint
	{
		void GetConnectionInterface(out Guid interfaceIdentifier);

		void GetConnectionPointContainer(out IConnectionPointContainer container);

		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int cookie);

		void Unadvise(int cookie);

		void EnumConnections(out object enumerator);
	}

	[ComImport]
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IConnectionPointContainer
	{
		void EnumConnectionPoints(out object enumerator);

		void FindConnectionPoint([In] ref Guid riid, out IConnectionPoint connectionPoint);
	}

	public const int WM_MOUSEMOVE = 512;

	public const int WM_LBUTTONDOWN = 513;

	public const int BEHAVIOR_EVENT_APPLYSTYLE = 2;

	public const int BEHAVIOR_EVENT_CONTENTREADY = 0;

	public const int BEHAVIOR_EVENT_CONTENTSAVE = 4;

	public const int BEHAVIOR_EVENT_DOCUMENTCONTEXTCHANGE = 3;

	public const int BEHAVIOR_EVENT_DOCUMENTREADY = 1;

	public const int DISPID_HTMLELEMENTEVENTS_ONDBLCLICK = -601;

	public const int DISPID_HTMLELEMENTEVENTS_ONDRAGSTART = -2147418101;

	public const int DISPID_HTMLELEMENTEVENTS_ONDRAG = -2147418092;

	public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEDOWN = -605;

	public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEUP = -607;

	public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEMOVE = -606;

	public const int DISPID_HTMLELEMENTEVENTS_ONMOVE = 1035;

	public const int DISPID_HTMLELEMENTEVENTS_ONMOVESTART = 1038;

	public const int DISPID_HTMLELEMENTEVENTS_ONMOVEEND = 1039;

	public const int DISPID_HTMLELEMENTEVENTS_ONRESIZESTART = 1040;

	public const int DISPID_HTMLELEMENTEVENTS_ONRESIZEEND = 1041;

	public const int DISPID_READYSTATE = -525;

	public const int DISPID_XOBJ_MIN = -2147418112;

	public const int DISPID_XOBJ_MAX = -2147352577;

	public const int DISPID_XOBJ_BASE = -2147418112;

	public const int DOCHOSTUIDBLCLICK_DEFAULT = 0;

	public const int DOCHOSTUIDBLCLICK_SHOWCODE = 2;

	public const int DOCHOSTUIDBLCLICK_SHOWPROPERTIES = 1;

	public const int DOCHOSTUIFLAG_ACTIVATE_CLIENTHIT_ONLY = 512;

	public const int DOCHOSTUIFLAG_DIALOG = 1;

	public const int DOCHOSTUIFLAG_DISABLE_COOKIE = 1024;

	public const int DOCHOSTUIFLAG_DISABLE_HELP_MENU = 2;

	public const int DOCHOSTUIFLAG_DISABLE_OFFSCREEN = 64;

	public const int DOCHOSTUIFLAG_DISABLE_SCRIPT_INACTIVE = 16;

	public const int DOCHOSTUIFLAG_DIV_BLOCKDEFAULT = 256;

	public const int DOCHOSTUIFLAG_FLAT_SCROLLBAR = 128;

	public const int DOCHOSTUIFLAG_NO3DBORDER = 4;

	public const int DOCHOSTUIFLAG_OPENNEWWIN = 32;

	public const int DOCHOSTUIFLAG_SCROLL_NO = 8;

	public const int DOCHOSTUIFLAG_ENABLE_INPLACE_NAVIGATION = 65536;

	public const int DROPEFFECT_NONE = 0;

	public const int DROPEFFECT_COPY = 1;

	public const int DROPEFFECT_MOVE = 2;

	public const int DROPEFFECT_LINK = 4;

	public const int E_ABORT = -2147467260;

	public const int E_ACCESSDENIED = -2147024891;

	public const int E_FAIL = -2147467259;

	public const int E_HANDLE = -2147024890;

	public const int E_INVALIDARG = -2147024809;

	public const int E_POINTER = -2147467261;

	public const int E_NOTIMPL = -2147467263;

	public const int E_NOINTERFACE = -2147467262;

	public const int E_OUTOFMEMORY = -2147024882;

	public const int E_UNEXPECTED = -2147418113;

	public const int ELEMENTDESCRIPTOR_FLAGS_LITERAL = 1;

	public const int ELEMENTDESCRIPTOR_FLAGS_NESTED_LITERAL = 2;

	public const int ELEMENTNAMESPACE_FLAGS_ALLOWANYTAG = 1;

	public const int ELEMENTNAMESPACE_FLAGS_QUERYFORUNKNOWNTAGS = 2;

	public const int ELEMENT_CORNER_BOTTOM = 3;

	public const int ELEMENT_CORNER_BOTTOMLEFT = 7;

	public const int ELEMENT_CORNER_BOTTOMRIGHT = 8;

	public const int ELEMENT_CORNER_LEFT = 2;

	public const int ELEMENT_CORNER_NONE = 0;

	public const int ELEMENT_CORNER_RIGHT = 4;

	public const int ELEMENT_CORNER_TOP = 1;

	public const int ELEMENT_CORNER_TOPLEFT = 5;

	public const int ELEMENT_CORNER_TOPRIGHT = 6;

	public const int HTMLPAINTER_3DSURFACE = 512;

	public const int HTMLPAINTER_ALPHA = 4;

	public const int HTMLPAINTER_COMPLEX = 8;

	public const int HTMLPAINTER_OPAQUE = 1;

	public const int HTMLPAINTER_OVERLAY = 16;

	public const int HTMLPAINTER_HITTEST = 32;

	public const int HTMLPAINTER_NOBAND = 1024;

	public const int HTMLPAINTER_NODC = 4096;

	public const int HTMLPAINTER_NOPHYSICALCLIP = 8192;

	public const int HTMLPAINTER_NOSAVEDC = 16384;

	public const int HTMLPAINTER_SURFACE = 256;

	public const int HTMLPAINTER_TRANSPARENT = 2;

	public const int HTMLPAINT_ZORDER_ABOVE_CONTENT = 7;

	public const int HTMLPAINT_ZORDER_ABOVE_FLOW = 6;

	public const int HTMLPAINT_ZORDER_BELOW_CONTENT = 4;

	public const int HTMLPAINT_ZORDER_BELOW_FLOW = 5;

	public const int HTMLPAINT_ZORDER_NONE = 0;

	public const int HTMLPAINT_ZORDER_REPLACE_ALL = 1;

	public const int HTMLPAINT_ZORDER_REPLACE_CONTENT = 2;

	public const int HTMLPAINT_ZORDER_REPLACE_BACKGROUND = 3;

	public const int HTMLPAINT_ZORDER_WINDOW_TOP = 8;

	public const int IDM_COPY = 15;

	public const int IDM_CUT = 16;

	public const int IDM_DELETE = 17;

	public const int IDM_FONTNAME = 18;

	public const int IDM_FONTSIZE = 19;

	public const int IDM_PASTE = 26;

	public const int IDM_PRINT = 27;

	public const int IDM_REDO = 29;

	public const int IDM_SELECTALL = 31;

	public const int IDM_UNDO = 43;

	public const int IDM_BACKCOLOR = 51;

	public const int IDM_BOLD = 52;

	public const int IDM_ITALIC = 56;

	public const int IDM_JUSTIFYCENTER = 57;

	public const int IDM_JUSTIFYLEFT = 59;

	public const int IDM_JUSTIFYRIGHT = 60;

	public const int IDM_UNDERLINE = 63;

	public const int IDM_STRIKETHROUGH = 91;

	public const int IDM_PRINTPREVIEW = 2003;

	public const int IDM_1D_ELEMENT = 2396;

	public const int IDM_2D_ELEMENT = 2395;

	public const int IDM_2D_POSITION = 2394;

	public const int IDM_ABSOLUTE_POSITION = 2397;

	public const int IDM_ADDTOGLYPHTABLE = 2337;

	public const int IDM_ATOMICSELECTION = 2399;

	public const int IDM_BLOCKFMT = 2234;

	public const int IDM_CHECKBOX = 2163;

	public const int IDM_BUTTON = 2167;

	public const int IDM_CLEARSELECTION = 2007;

	public const int IDM_CSSEDITING_LEVEL = 2406;

	public const int IDM_DROPDOWNBOX = 2165;

	public const int IDM_EMPTYGLYPHTABLE = 2336;

	public const int IDM_FORECOLOR = 55;

	public const int IDM_HYPERLINK = 2124;

	public const int IDM_IMAGE = 2168;

	public const int IDM_INDENT = 2186;

	public const int IDM_LISTBOX = 2166;

	public const int IDM_LIVERESIZE = 2398;

	public const int IDM_MULTIPLESELECTION = 2393;

	public const int IDM_NOACTIVATEDESIGNTIMECONTROLS = 2333;

	public const int IDM_NOACTIVATEJAVAAPPLETS = 2334;

	public const int IDM_NOACTIVATENORMALOLECONTROLS = 2332;

	public const int IDM_NOFIXUPURLSONPASTE = 2335;

	public const int IDM_ORDERLIST = 2184;

	public const int IDM_OUTDENT = 2187;

	public const int IDM_PERSISTDEFAULTVALUES = 7100;

	public const int IDM_PRESERVEUNDOALWAYS = 6049;

	public const int IDM_PROTECTMETATAGS = 7101;

	public const int IDM_RADIOBUTTON = 2164;

	public const int IDM_REMOVEFROMGLYPHTABLE = 2338;

	public const int IDM_REPLACEGLYPHCONTENTS = 2339;

	public const int IDM_RESPECTVISIBILITY_INDESIGN = 2405;

	public const int IDM_SETDIRTY = 2342;

	public const int IDM_SHOWZEROBORDERATDESIGNTIME = 2328;

	public const int IDM_SUBSCRIPT = 2247;

	public const int IDM_SUPERSCRIPT = 2248;

	public const int IDM_TEXTBOX = 2161;

	public const int IDM_TEXTAREA = 2162;

	public const int IDM_UNLINK = 2125;

	public const int IDM_UNORDERLIST = 2185;

	public const int INET_E_DEFAULT_ACTION = -2146697199;

	public const int INET_E_USE_DEFAULT_PROTOCOLHANDLER = -2146697199;

	public const int OLEIVERB_DISCARDUNDOSTATE = -6;

	public const int OLEIVERB_HIDE = -3;

	public const int OLEIVERB_INPLACEACTIVATE = -5;

	public const int OLECLOSE_NOSAVE = 1;

	public const int OLEIVERB_OPEN = -2;

	public const int OLEIVERB_PRIMARY = 0;

	public const int OLEIVERB_PROPERTIES = -7;

	public const int OLEIVERB_SHOW = -1;

	public const int OLEIVERB_UIACTIVATE = -4;

	public const int S_FALSE = 1;

	public const int S_OK = 0;

	public const int BITMAPINFO_MAX_COLORSIZE = 256;

	public const int WM_KEYFIRST = 256;

	public const int WM_KEYLAST = 264;

	public const int WM_KEYDOWN = 256;

	public const int WM_KEYUP = 257;

	public static Guid ElementBehaviorFactory = new Guid("3050f429-98b5-11cf-bb82-00aa00bdce0b");

	public static Guid Guid_MSHTML = new Guid("DE4BA900-59CA-11CF-9592-444553540000");

	public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

	public static IntPtr NullIntPtr = (IntPtr)0;

	private NativeMethods()
	{
	}

	[DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	internal static extern int CreateURLMoniker(IMoniker pmkContext, string szURL, out IMoniker ppmk);

	[DllImport("ole32.dll", PreserveSig = false)]
	internal static extern void CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, out IStream pStream);

	[DllImport("ole32.dll", PreserveSig = false)]
	internal static extern void GetHGlobalFromStream(IStream pStream, out IntPtr pHGlobal);

	[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern int CreateBindCtx(int dwReserved, out IBindCtx ppbc);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern bool GetClientRect(IntPtr hWnd, [In][Out] COMRECT rect);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern IntPtr GlobalLock(IntPtr handle);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	internal static extern bool GlobalUnlock(IntPtr handle);
}
