using System;
using System.Windows.Forms;

namespace PostXING.Controls.Controls;

public class MozItemClickEventArgs : EventArgs
{
	private MozItem m_mozItem;

	private MouseButtons m_button;

	public MozItem MozItem => m_mozItem;

	public MouseButtons Button => m_button;

	public MozItemClickEventArgs()
	{
		m_mozItem = null;
		m_button = MouseButtons.Left;
	}

	public MozItemClickEventArgs(MozItem mozItem, MouseButtons button)
	{
		m_mozItem = mozItem;
		m_button = button;
	}
}
