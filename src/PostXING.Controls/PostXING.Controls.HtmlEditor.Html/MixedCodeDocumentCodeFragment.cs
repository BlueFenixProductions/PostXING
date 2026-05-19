namespace PostXING.Controls.HtmlEditor.Html;

public class MixedCodeDocumentCodeFragment : MixedCodeDocumentFragment
{
	internal string _code;

	public string Code
	{
		get
		{
			if (_code == null)
			{
				_code = base.FragmentText.Substring(_doc.TokenCodeStart.Length, base.FragmentText.Length - _doc.TokenCodeEnd.Length - _doc.TokenCodeStart.Length - 1).Trim();
				if (_code.StartsWith("="))
				{
					_code = _doc.TokenResponseWrite + _code.Substring(1, _code.Length - 1);
				}
			}
			return _code;
		}
		set
		{
			_code = value;
		}
	}

	internal MixedCodeDocumentCodeFragment(MixedCodeDocument doc)
		: base(doc, MixedCodeDocumentFragmentType.Code)
	{
	}
}
