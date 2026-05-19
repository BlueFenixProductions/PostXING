namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlTextNode : HtmlNode
{
	private string _text;

	public override string InnerHtml
	{
		get
		{
			return OuterHtml;
		}
		set
		{
			_text = value;
		}
	}

	public override string OuterHtml
	{
		get
		{
			if (_text == null)
			{
				return base.OuterHtml;
			}
			return _text;
		}
	}

	public string Text
	{
		get
		{
			if (_text == null)
			{
				return base.OuterHtml;
			}
			return _text;
		}
		set
		{
			_text = value;
		}
	}

	internal HtmlTextNode(HPathDocument ownerdocument, int index)
		: base(HtmlNodeType.Text, ownerdocument, index)
	{
	}
}
