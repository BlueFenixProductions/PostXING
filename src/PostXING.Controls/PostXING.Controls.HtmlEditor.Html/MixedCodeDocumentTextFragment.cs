namespace PostXING.Controls.HtmlEditor.Html;

public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
{
	public string Text
	{
		get
		{
			return base.FragmentText;
		}
		set
		{
			_fragmenttext = value;
		}
	}

	internal MixedCodeDocumentTextFragment(MixedCodeDocument doc)
		: base(doc, MixedCodeDocumentFragmentType.Text)
	{
	}
}
