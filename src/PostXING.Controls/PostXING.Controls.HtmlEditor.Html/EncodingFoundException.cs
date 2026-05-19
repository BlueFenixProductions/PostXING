using System;
using System.Text;

namespace PostXING.Controls.HtmlEditor.Html;

internal class EncodingFoundException : Exception
{
	private Encoding _encoding;

	internal Encoding Encoding => _encoding;

	internal EncodingFoundException(Encoding encoding)
	{
		_encoding = encoding;
	}
}
