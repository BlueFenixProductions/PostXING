namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlParseError
{
	private HtmlParseErrorCode _code;

	private int _line;

	private int _linePosition;

	private int _streamPosition;

	private string _sourceText;

	private string _reason;

	public HtmlParseErrorCode Code => _code;

	public int Line => _line;

	public int LinePosition => _linePosition;

	public int StreamPosition => _streamPosition;

	public string SourceText => _sourceText;

	public string Reason => _reason;

	internal HtmlParseError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText, string reason)
	{
		_code = code;
		_line = line;
		_linePosition = linePosition;
		_streamPosition = streamPosition;
		_sourceText = sourceText;
		_reason = reason;
	}
}
