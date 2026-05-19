namespace PostXING.Controls.Navigation;

public class NavigationContext
{
	private Page page;

	public ViewState ViewState;

	public int HorizontalScrollPosition;

	public int VerticalScrollPosition;

	public Page Page => page;

	public string PageText
	{
		get
		{
			if (page == null)
			{
				return string.Empty;
			}
			if (ViewState == null)
			{
				return page.Text;
			}
			return (string)ViewState.ReadProperty("Text");
		}
	}

	public NavigationContext(Page page)
	{
		this.page = page;
	}
}
