using System;

namespace PostXING.Controls.Navigation;

public interface IFrame
{
	void Go(Page page);

	void Go(Type pageType);

	void GoBack();

	void GoBack(int n);

	void GoForward();

	void GoForward(int n);

	void GoHome();

	void PageDown();

	void PageEnd();

	void PageHome();

	void PageUp();
}
