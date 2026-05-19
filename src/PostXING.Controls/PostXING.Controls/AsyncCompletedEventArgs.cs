using System;

namespace PostXING.Controls;

public class AsyncCompletedEventArgs : EventArgs
{
	public readonly Exception Error;

	public readonly bool Cancelled;

	public AsyncCompletedEventArgs(Exception ex, bool cancel)
	{
		Error = ex;
		Cancelled = cancel;
	}
}
