using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace PostXING.Controls.MozBar;

public class ThemeManager
{
	[DllImport("uxTheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern void GetThemeColor(IntPtr hTheme, int partID, int stateID, int propID, out int color);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
	private static extern IntPtr OpenThemeData(IntPtr hwnd, string classes);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern int CloseThemeData(IntPtr hwnd);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern int GetWindowTheme(IntPtr hWnd);

	[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
	private static extern bool IsThemeActive();

	[DllImport("comctl32.dll", CallingConvention = CallingConvention.StdCall)]
	private static extern int DllGetVersion(ref DLLVERSIONINFO s);

	public bool _IsAppThemed()
	{
		try
		{
			DLLVERSIONINFO s = new DLLVERSIONINFO
			{
				cbSize = Marshal.SizeOf(typeof(DLLVERSIONINFO))
			};
			DllGetVersion(ref s);
			if (s.dwMajorVersion >= 6)
			{
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public void _CloseThemeData(IntPtr hwnd)
	{
		try
		{
			CloseThemeData(hwnd);
		}
		catch (Exception)
		{
		}
	}

	public IntPtr _OpenThemeData(IntPtr hwnd, string classes)
	{
		try
		{
			return OpenThemeData(hwnd, classes);
		}
		catch (Exception)
		{
			return IntPtr.Zero;
		}
	}

	public int _GetWindowTheme(IntPtr hwnd)
	{
		try
		{
			return GetWindowTheme(hwnd);
		}
		catch (Exception)
		{
			return -1;
		}
	}

	public bool _IsThemeActive()
	{
		try
		{
			return IsThemeActive();
		}
		catch (Exception)
		{
			return false;
		}
	}

	public Color _GetThemeColor(IntPtr hTheme, int partID, int stateID, int propID)
	{
		try
		{
			GetThemeColor(hTheme, partID, stateID, propID, out var color);
			return ColorTranslator.FromWin32(color);
		}
		catch (Exception)
		{
			return Color.Empty;
		}
	}
}
