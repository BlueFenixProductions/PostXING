namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlCommentNode : HtmlNode
{
	private string _comment;

	public override string InnerHtml
	{
		get
		{
			if (_comment == null)
			{
				return base.InnerHtml;
			}
			return _comment;
		}
		set
		{
			_comment = value;
		}
	}

	public override string OuterHtml
	{
		get
		{
			if (_comment == null)
			{
				return base.OuterHtml;
			}
			return "<!--" + _comment + "-->";
		}
	}

	public string Comment
	{
		get
		{
			if (_comment == null)
			{
				return base.InnerHtml;
			}
			return _comment;
		}
		set
		{
			_comment = value;
		}
	}

	internal HtmlCommentNode(HPathDocument ownerdocument, int index)
		: base(HtmlNodeType.Comment, ownerdocument, index)
	{
	}
}
