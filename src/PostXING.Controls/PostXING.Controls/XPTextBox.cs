using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PostXING.Controls;

[ToolboxBitmap(typeof(TextBox))]
[ToolboxItem(true)]
public class XPTextBox : TextBox
{
	private bool visualStylesEnabled;

	protected bool VisualStylesEnabled
	{
		get
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform == PlatformID.Win32NT && ((oSVersion.Version.Major == 5 && oSVersion.Version.Minor >= 1) || oSVersion.Version.Major > 5) && UxTheme.IsThemeActive() && UxTheme.IsAppThemed())
			{
				DLLVERSIONINFO pdvi = new DLLVERSIONINFO
				{
					cbSize = Marshal.SizeOf(typeof(DLLVERSIONINFO))
				};
				if (NativeMethods.DllGetVersion(ref pdvi) == 0)
				{
					return pdvi.dwMajorVersion > 5;
				}
			}
			return false;
		}
	}

	public XPTextBox()
	{
		visualStylesEnabled = VisualStylesEnabled;
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		base.OnSystemColorsChanged(e);
		visualStylesEnabled = VisualStylesEnabled;
	}

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		if (!visualStylesEnabled || m.Msg != 791 || (m.LParam.ToInt32() & 2) != 2)
		{
			return;
		}
		IntPtr intPtr = UxTheme.OpenThemeData(base.Handle, UxTheme.WindowClasses.Edit);
		if (intPtr != IntPtr.Zero)
		{
			int iPartId = 1;
			int iStateId = 1;
			if (base.ReadOnly)
			{
				iStateId = 6;
			}
			else if (!base.Enabled)
			{
				iStateId = 4;
			}
			RECT pRect = new RECT
			{
				right = base.Width,
				bottom = base.Height
			};
			RECT pClipRect = new RECT
			{
				left = pRect.left,
				top = pRect.top,
				right = pRect.left + 2,
				bottom = pRect.bottom
			};
			UxTheme.DrawThemeBackground(intPtr, m.WParam, iPartId, iStateId, ref pRect, ref pClipRect);
			pClipRect.left = pRect.left;
			pClipRect.top = pRect.top;
			pClipRect.right = pRect.right;
			pClipRect.bottom = pRect.top + 2;
			UxTheme.DrawThemeBackground(intPtr, m.WParam, iPartId, iStateId, ref pRect, ref pClipRect);
			pClipRect.left = pRect.right - 2;
			pClipRect.top = pRect.top;
			pClipRect.right = pRect.right;
			pClipRect.bottom = pRect.bottom;
			UxTheme.DrawThemeBackground(intPtr, m.WParam, iPartId, iStateId, ref pRect, ref pClipRect);
			pClipRect.left = pRect.left;
			pClipRect.top = pRect.bottom - 2;
			pClipRect.right = pRect.right;
			pClipRect.bottom = pRect.bottom;
			UxTheme.DrawThemeBackground(intPtr, m.WParam, iPartId, iStateId, ref pRect, ref pClipRect);
		}
		UxTheme.CloseThemeData(intPtr);
	}
}
