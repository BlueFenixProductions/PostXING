using System;

namespace PostXING.Controls.Controls;

public class MozItemEventArgs : EventArgs
{
	private MozItem m_mozItem;

	public MozItem MozItem => m_mozItem;

	public MozItemEventArgs()
	{
		m_mozItem = null;
	}

	public MozItemEventArgs(MozItem mozItem)
	{
		m_mozItem = mozItem;
	}
}
