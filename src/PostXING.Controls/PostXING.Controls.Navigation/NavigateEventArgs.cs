using System;

namespace PostXING.Controls.Navigation;

public class NavigateEventArgs : EventArgs
{
	private Page page;

	public Page Page => page;

	public NavigateEventArgs(Page page)
	{
		this.page = page;
	}
}
