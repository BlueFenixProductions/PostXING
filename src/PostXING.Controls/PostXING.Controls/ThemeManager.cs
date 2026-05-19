using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PostXING.Controls;

public class ThemeManager
{
	private static IntPtr hModule = IntPtr.Zero;

	private static bool LoadShellStyleDll()
	{
		string text = UxTheme.ThemeName.Substring(0, UxTheme.ThemeName.LastIndexOf('\\'));
		string text2 = text + "\\Shell\\" + UxTheme.ColorName;
		string text3 = text2 + "\\shellstyle.dll";
		if (!File.Exists(text3))
		{
			return false;
		}
		int errorMode = NativeMethods.SetErrorMode((SetErrorModeFlags)32769);
		hModule = NativeMethods.LoadLibraryEx(text3, LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE);
		NativeMethods.SetErrorMode((SetErrorModeFlags)errorMode);
		return hModule != IntPtr.Zero;
	}

	private static bool LoadShellStyleDll(string stylePath)
	{
		if (!File.Exists(stylePath))
		{
			return LoadShellStyleDll();
		}
		int errorMode = NativeMethods.SetErrorMode((SetErrorModeFlags)32769);
		hModule = NativeMethods.LoadLibraryEx(stylePath, LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE);
		NativeMethods.SetErrorMode((SetErrorModeFlags)errorMode);
		return hModule != IntPtr.Zero;
	}

	private static void FreeShellStyleDll()
	{
		NativeMethods.FreeLibrary(hModule);
		hModule = IntPtr.Zero;
	}

	internal static string GetResourceUIFile()
	{
		IntPtr hResInfo = NativeMethods.FindResource(hModule, "#1", "UIFILE");
		int num = NativeMethods.SizeofResource(hModule, hResInfo);
		IntPtr intPtr = NativeMethods.LoadResource(hModule, hResInfo);
		byte[] array = new byte[num];
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr intPtr2 = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
		NativeMethods.CopyMemory(intPtr2, intPtr, num);
		gCHandle.Free();
		NativeMethods.FreeResource(intPtr);
		return Marshal.PtrToStringAnsi(intPtr2, num);
	}

	internal static Bitmap GetResourceBMP(string resourceName)
	{
		IntPtr hbitmap = NativeMethods.LoadBitmap(hModule, int.Parse(resourceName));
		return Image.FromHbitmap(hbitmap);
	}

	internal static Bitmap GetResourcePNG(string resourceName)
	{
		Bitmap bitmap = Bitmap.FromResource(hModule, "#" + resourceName);
		IntPtr hResInfo = NativeMethods.FindResource(hModule, "#" + resourceName, 2);
		int num = NativeMethods.SizeofResource(hModule, hResInfo);
		Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
		IntPtr intPtr = NativeMethods.LoadResource(hModule, hResInfo);
		byte[] array = new byte[num];
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		IntPtr destination = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
		NativeMethods.CopyMemory(destination, intPtr, num);
		NativeMethods.FreeResource(intPtr);
		Rectangle rect = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
		BitmapData bitmapData = bitmap2.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		destination = Marshal.UnsafeAddrOfPinnedArrayElement(array, 40);
		NativeMethods.CopyMemory(bitmapData.Scan0, destination, num - 40);
		gCHandle.Free();
		bitmap2.UnlockBits(bitmapData);
		NativeMethods.GdiFlush();
		bitmap2.RotateFlip(RotateFlipType.Rotate180FlipX);
		return bitmap2;
	}

	internal static string GetResourceString(int id)
	{
		if (hModule == IntPtr.Zero)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder(1024);
		NativeMethods.LoadString(hModule, id, stringBuilder, 1024);
		return stringBuilder.ToString();
	}
}
