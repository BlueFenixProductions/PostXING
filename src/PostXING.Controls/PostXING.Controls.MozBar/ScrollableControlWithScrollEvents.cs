using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PostXING.Controls.MozBar;

[ToolboxItem(false)]
public class ScrollableControlWithScrollEvents : ScrollableControl
{
	private const int WM_HSCROLL = 276;

	private const int WM_VSCROLL = 277;

	private const int SB_HORZ = 0;

	private const int SB_VERT = 1;

	private const int SIF_RANGE = 1;

	private const int SIF_PAGE = 2;

	private const int SIF_POS = 4;

	private const int SIF_DISABLENOSCROLL = 8;

	private const int SIF_TRACKPOS = 16;

	private const int SIF_ALL = 31;

	private static ScrollEventType[] _events = new ScrollEventType[9]
	{
		ScrollEventType.SmallDecrement,
		ScrollEventType.SmallIncrement,
		ScrollEventType.LargeDecrement,
		ScrollEventType.LargeIncrement,
		ScrollEventType.ThumbPosition,
		ScrollEventType.ThumbTrack,
		ScrollEventType.First,
		ScrollEventType.Last,
		ScrollEventType.EndScroll
	};

	[Category("Panel")]
	[Description("Indicates that the control has been scrolled horizontally.")]
	[Browsable(true)]
	public new event MozScrollEventHandler HorizontalScroll;

	[Category("Panel")]
	[Browsable(true)]
	[Description("Indicates that the control has been scrolled vertically.")]
	public new event MozScrollEventHandler VerticalScroll;

	[DllImport("User32")]
	private static extern bool GetScrollInfo(IntPtr hWnd, int fnBar, ref SCROLLINFO info);

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		if (m.Msg == 276)
		{
			if (this.HorizontalScroll != null)
			{
				uint num = (uint)m.WParam.ToInt32();
				SCROLLINFO info = default(SCROLLINFO);
				info.cbSize = Marshal.SizeOf(info);
				info.fMask = 31;
				GetScrollInfo(base.Handle, 0, ref info);
				this.HorizontalScroll(this, new MozScrollEventArgs(GetEventType(num & 0xFFFF), (int)(num >> 16), info));
			}
		}
		else if (m.Msg == 277 && this.VerticalScroll != null)
		{
			uint num2 = (uint)m.WParam.ToInt32();
			SCROLLINFO info2 = default(SCROLLINFO);
			info2.cbSize = Marshal.SizeOf(info2);
			info2.fMask = 31;
			GetScrollInfo(base.Handle, 1, ref info2);
			this.VerticalScroll(this, new MozScrollEventArgs(GetEventType(num2 & 0xFFFF), (int)(num2 >> 16), info2));
		}
	}

	private ScrollEventType GetEventType(uint wParam)
	{
		if (wParam < _events.Length)
		{
			return _events[wParam];
		}
		return ScrollEventType.EndScroll;
	}
}
