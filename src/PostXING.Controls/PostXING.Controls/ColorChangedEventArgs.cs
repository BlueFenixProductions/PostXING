using System;

namespace PostXING.Controls;

public class ColorChangedEventArgs : EventArgs
{
	private ColorHandler.RGB mRGB;

	private ColorHandler.HSV mHSV;

	public ColorHandler.RGB RGB => mRGB;

	public ColorHandler.HSV HSV => mHSV;

	public ColorChangedEventArgs(ColorHandler.RGB RGB, ColorHandler.HSV HSV)
	{
		mRGB = RGB;
		mHSV = HSV;
	}
}
