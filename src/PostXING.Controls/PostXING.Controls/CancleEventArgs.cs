using System;

namespace PostXING.Controls;

public class CancleEventArgs : EventArgs
{
	protected bool _cancel;

	public bool Cancel
	{
		get
		{
			return _cancel;
		}
		set
		{
			_cancel = value;
		}
	}
}
