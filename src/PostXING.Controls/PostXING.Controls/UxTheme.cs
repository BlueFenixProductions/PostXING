using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PostXING.Controls;

public class UxTheme
{
	public class WindowClasses
	{
		public static readonly string Edit = "EDIT";

		public static readonly string ListView = "LISTVIEW";

		public static readonly string TreeView = "TREEVIEW";
	}

	public class Parts
	{
		public enum Edit
		{
			EditText = 1
		}

		public enum ListView
		{
			ListItem = 1
		}

		public enum TreeView
		{
			TreeItem = 1
		}
	}

	public class PartStates
	{
		public enum EditText
		{
			Normal = 1,
			Hot,
			Selected,
			Disabled,
			Focused,
			Readonly
		}

		public enum ListItem
		{
			Normal = 1,
			Hot,
			Selected,
			Disabled,
			SelectedNotFocused
		}

		public enum TreeItem
		{
			Normal = 1,
			Hot,
			Selected,
			Disabled,
			SelectedNotFocused
		}
	}

	public static bool AppThemed
	{
		get
		{
			bool result = false;
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform == PlatformID.Win32NT && ((oSVersion.Version.Major == 5 && oSVersion.Version.Minor >= 1) || oSVersion.Version.Major > 5))
			{
				result = IsAppThemed();
			}
			return result;
		}
	}

	public static string ThemeName
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			GetCurrentThemeName(stringBuilder, 256, null, 0, null, 0);
			return stringBuilder.ToString();
		}
	}

	public static string ColorName
	{
		get
		{
			StringBuilder pszThemeFileName = new StringBuilder(256);
			StringBuilder stringBuilder = new StringBuilder(256);
			GetCurrentThemeName(pszThemeFileName, 256, stringBuilder, 256, null, 0);
			return stringBuilder.ToString();
		}
	}

	private UxTheme()
	{
	}

	[DllImport("UxTheme.dll")]
	public static extern IntPtr OpenThemeData(IntPtr hwnd, [MarshalAs(UnmanagedType.LPTStr)] string pszClassList);

	[DllImport("UxTheme.dll")]
	public static extern int CloseThemeData(IntPtr hTheme);

	[DllImport("UxTheme.dll")]
	public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);

	[DllImport("UxTheme.dll")]
	public static extern bool IsThemeActive();

	[DllImport("UxTheme.dll")]
	public static extern bool IsAppThemed();

	[DllImport("UxTheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
	protected static extern int GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

	[DllImport("UxTheme.dll")]
	public static extern int DrawThemeParentBackground(IntPtr hwnd, IntPtr hdc, ref RECT prc);
}
