using System;

namespace PostXING.Controls;

public class ProgressChangedEventArgs : EventArgs
{
	public readonly int ProgressPercentage;

	public ProgressChangedEventArgs(int percentage)
	{
		ProgressPercentage = percentage;
	}
}
