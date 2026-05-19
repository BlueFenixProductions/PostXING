namespace PostXING.Controls;

public class DoWorkEventArgs : CancleEventArgs
{
	private object _result;

	public readonly object Argument;

	public object Result
	{
		get
		{
			return _result;
		}
		set
		{
			_result = value;
		}
	}

	public DoWorkEventArgs(object argument)
	{
		Argument = argument;
	}
}
