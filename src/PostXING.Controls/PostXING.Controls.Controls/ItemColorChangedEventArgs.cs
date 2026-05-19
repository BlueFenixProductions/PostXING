using System;

namespace PostXING.Controls.Controls;

public class ItemColorChangedEventArgs : EventArgs
{
	private MozItemColor m_color;

	public MozItemColor Color => m_color;

	public ItemColorChangedEventArgs()
	{
		m_color = MozItemColor.Background;
	}

	public ItemColorChangedEventArgs(MozItemColor color)
	{
		m_color = color;
	}
}
