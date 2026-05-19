namespace PostXING.Controls.HtmlEditor.Html;

public abstract class MixedCodeDocumentFragment
{
	internal MixedCodeDocumentFragmentType _type;

	internal MixedCodeDocument _doc;

	internal int _index;

	internal int _length;

	internal int _line;

	internal int _lineposition;

	internal string _fragmenttext;

	public MixedCodeDocumentFragmentType FragmentType => _type;

	public int StreamPosition => _index;

	public int Line => _line;

	public int LinePosition => _lineposition;

	public string FragmentText
	{
		get
		{
			if (_fragmenttext == null)
			{
				_fragmenttext = _doc._text.Substring(_index, _length);
			}
			return _fragmenttext;
		}
	}

	internal MixedCodeDocumentFragment(MixedCodeDocument doc, MixedCodeDocumentFragmentType type)
	{
		_doc = doc;
		_type = type;
		switch (type)
		{
		case MixedCodeDocumentFragmentType.Text:
			_doc._textfragments.Append(this);
			break;
		case MixedCodeDocumentFragmentType.Code:
			_doc._codefragments.Append(this);
			break;
		}
		_doc._fragments.Append(this);
	}
}
