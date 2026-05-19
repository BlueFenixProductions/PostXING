using System;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlWebException : Exception
{
	public HtmlWebException(string message)
		: base(message)
	{
	}
}
