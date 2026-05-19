using System.Windows.Forms;

namespace PostXING.Controls.MozBar;

public class MozScrollEventArgs
{
	private ScrollEventType m_type;

	private int m_newValue;

	private SCROLLINFO m_info;

	public SCROLLINFO ScrollInfo => m_info;

	public ScrollEventType Type => m_type;

	public int NewValue => m_newValue;

	public MozScrollEventArgs(ScrollEventType type, int newValue, SCROLLINFO info)
	{
		m_type = type;
		m_newValue = newValue;
		m_info = info;
	}
}
