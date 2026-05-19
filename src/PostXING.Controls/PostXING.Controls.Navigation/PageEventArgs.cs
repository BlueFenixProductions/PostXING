namespace PostXING.Controls.Navigation;

public class PageEventArgs
{
	private ViewState viewState;

	public ViewState ViewState => viewState;

	public PageEventArgs(ViewState viewState)
	{
		this.viewState = viewState;
	}
}
