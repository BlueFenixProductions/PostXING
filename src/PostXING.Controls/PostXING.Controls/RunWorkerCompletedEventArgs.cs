using System;

namespace PostXING.Controls;

public class RunWorkerCompletedEventArgs : AsyncCompletedEventArgs
{
	public readonly object Result;

	public readonly object WorkArgument;

	public RunWorkerCompletedEventArgs(object workArgument, object result, Exception error, bool cancel)
		: base(error, cancel)
	{
		Result = result;
	}
}
