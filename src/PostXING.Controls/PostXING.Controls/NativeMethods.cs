using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PostXING.Controls;

public abstract class NativeMethods
{
	public static IntPtr LoadLibraryEx(string lpfFileName, LoadLibraryExFlags flags)
	{
		return InternalLoadLibraryEx(lpfFileName, IntPtr.Zero, (int)flags);
	}

	[DllImport("Kernel32.dll", EntryPoint = "LoadLibraryEx")]
	private static extern IntPtr InternalLoadLibraryEx(string lpfFileName, IntPtr hFile, int dwFlags);

	[DllImport("Kernel32.dll")]
	public static extern bool FreeLibrary(IntPtr hModule);

	[DllImport("Kernel32.dll")]
	public static extern IntPtr FindResource(IntPtr hModule, string lpName, int lpType);

	[DllImport("Kernel32.dll")]
	public static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);

	[DllImport("Kernel32.dll")]
	public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

	[DllImport("Kernel32.dll")]
	public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

	[DllImport("Kernel32.dll")]
	public static extern int FreeResource(IntPtr hglbResource);

	[DllImport("Kernel32.dll")]
	public static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);

	[DllImport("User32.dll")]
	public static extern IntPtr LoadBitmap(IntPtr hInstance, long lpBitmapName);

	[DllImport("Gdi32.dll")]
	public static extern int GdiFlush();

	[DllImport("User32.dll")]
	public static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);

	public static int SendMessage(IntPtr hwnd, WindowMessageFlags msg, IntPtr wParam, IntPtr lParam)
	{
		return InternalSendMessage(hwnd, (int)msg, wParam, lParam);
	}

	[DllImport("User32.dll", EntryPoint = "SendMessage")]
	private static extern int InternalSendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

	[DllImport("Comctl32.dll")]
	public static extern int DllGetVersion(ref DLLVERSIONINFO pdvi);

	[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
	public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	public static int SetErrorMode(SetErrorModeFlags uMode)
	{
		return InternalSetErrorMode((int)uMode);
	}

	[DllImport("Kernel32.dll", EntryPoint = "SetErrorMode")]
	private static extern int InternalSetErrorMode(int uMode);

	[DllImport("User32.dll")]
	public static extern IntPtr GetWindowDC(IntPtr hwnd);

	[DllImport("User32.dll")]
	public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

	public static int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags)
	{
		return InternalSetWindowPos(hWnd, hWndInsertAfter, X, Y, cx, cy, (int)uFlags);
	}

	[DllImport("User32.dll", EntryPoint = "SetWindowPos")]
	private static extern int InternalSetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

	public static bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags)
	{
		return InternalRedrawWindow(hWnd, lprcUpdate, hrgnUpdate, (int)flags);
	}

	[DllImport("User32.dll", EntryPoint = "RedrawWindow")]
	private static extern bool InternalRedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, int flags);

	public static bool ShowScrollBar(IntPtr hWnd, ScrollBarConstants wBar, bool bShow)
	{
		return InternalShowScrollBar(hWnd, (int)wBar, bShow);
	}

	[DllImport("User32.dll", EntryPoint = "ShowScrollBar")]
	private static extern bool InternalShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

	public static bool EnableScrollBar(IntPtr hWnd, ScrollBarConstants wBar, EnableScrollBarFlags wArrows)
	{
		return InternalEnableScrollBar(hWnd, (int)wBar, (int)wArrows);
	}

	[DllImport("User32.dll", EntryPoint = "EnableScrollBar")]
	private static extern bool InternalEnableScrollBar(IntPtr hWnd, int wBar, int wArrows);

	public static bool GetScrollInfo(IntPtr hWnd, ScrollBarConstants fnBar, ref SCROLLINFO lpsi)
	{
		return InternalGetScrollInfo(hWnd, (int)fnBar, ref lpsi);
	}

	[DllImport("User32.dll", EntryPoint = "GetScrollInfo")]
	private static extern bool InternalGetScrollInfo(IntPtr hWnd, int fnBar, ref SCROLLINFO lpsi);

	[DllImport("User32.dll")]
	public static extern int GetScrollPos(IntPtr hWnd, int nBar);

	public static int SetScrollInfo(IntPtr hWnd, ScrollBarConstants fnBar, ref SCROLLINFO lpsi, bool fRedraw)
	{
		return InternalSetScrollInfo(hWnd, (int)fnBar, ref lpsi, fRedraw);
	}

	[DllImport("User32.dll", EntryPoint = "SetScrollInfo")]
	private static extern int InternalSetScrollInfo(IntPtr hWnd, int fnBar, ref SCROLLINFO lpsi, bool fRedraw);

	[DllImport("User32.dll")]
	public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

	public static int HIWORD(int n)
	{
		return (n >> 16) & 0xFFFF;
	}

	public static int LOWORD(int n)
	{
		return n & 0xFFFF;
	}

	public static IntPtr MAKELPARAM(int low, int high)
	{
		return (IntPtr)((high << 16) | (low & 0xFFFF));
	}
}
