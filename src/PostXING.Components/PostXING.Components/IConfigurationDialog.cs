using System;

namespace PostXING.Components;

public interface IConfigurationDialog
{
	bool IsEditing { get; set; }

	Blog CurrentBlog { get; set; }

	Blog OriginalBlog { get; set; }

	object CurrentProvider { get; set; }

	void NavigateToProxyPage(object sender, EventArgs e);

	void NavigateToFtpPage(object sender, EventArgs e);

	void NavigateToMediaPage(object sender, EventArgs e);

	void NavigateToPreviewPage(object sender, EventArgs e);

	void NavigateToGeneralPage(object sender, EventArgs e);

	void NavigateToConfigurationPage(object sender, EventArgs e);

	void ApplyBlogSettings();

	void ShowOptions();
}
