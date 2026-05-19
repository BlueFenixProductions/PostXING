using System;

namespace PostXING.Controls.Controls;

public class ItemBorderStyleChangedEventArgs : EventArgs
{
	private MozItemState m_state;

	public MozItemState State => m_state;

	public ItemBorderStyleChangedEventArgs()
	{
		m_state = MozItemState.Normal;
	}

	public ItemBorderStyleChangedEventArgs(MozItemState state)
	{
		m_state = state;
	}
}
