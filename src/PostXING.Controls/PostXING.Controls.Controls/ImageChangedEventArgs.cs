using System;

namespace PostXING.Controls.Controls;

public class ImageChangedEventArgs : EventArgs
{
	private MozItemState m_image;

	public MozItemState Image => m_image;

	public ImageChangedEventArgs()
	{
		m_image = MozItemState.Normal;
	}

	public ImageChangedEventArgs(MozItemState image)
	{
		m_image = image;
	}
}
