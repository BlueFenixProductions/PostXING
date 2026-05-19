using System.ComponentModel;
using PostXING.Components;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;

namespace PostXING.NavigationPages;

public class OptionsPageBase : Page
{
	private Container components;

	protected IConfigurationDialog _dialog;

	protected IBlogProvider _provider;

	public OptionsPageBase()
	{
		InitializeComponent();
	}

	public virtual void ApplySettings()
	{
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		if (e.ViewState != null)
		{
			_dialog = (IConfigurationDialog)e.ViewState.ReadProperty("dialog");
			_provider = (IBlogProvider)e.ViewState.ReadProperty("provider");
		}
		base.OnPageEnter(e);
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		e.ViewState.WriteProperty("dialog", _dialog);
		e.ViewState.WriteProperty("provider", _provider);
		base.OnPageLeave(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}
