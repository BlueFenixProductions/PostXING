using System;

namespace PostXING.Controls;

public class TransferFailedEventArgs : EventArgs
{
	private Exception m_error;

	private string m_caption;

	public Exception Error => m_error;

	public string Caption => m_caption;

	public TransferFailedEventArgs(string caption, Exception e)
	{
		m_caption = caption;
		m_error = e;
	}
}
