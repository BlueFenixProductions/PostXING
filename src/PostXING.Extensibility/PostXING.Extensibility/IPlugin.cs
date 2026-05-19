using System;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Extensibility;

public interface IPlugin
{
	bool HasConfiguration { get; }

	PluginDockPositions PreferredDockState { get; set; }

	Control control { get; }

	PluginTypes PluginType { get; }

	Icon Icon { get; set; }

	Shortcut Shortcut { get; set; }

	string PluginName { get; set; }

	bool BeginGroup { get; set; }

	string MenuCaption { get; set; }

	void Configure(IWin32Window parent);

	void Init(AppContext context);

	void BeforeDispose(AppContext context);

	void Dispose();

	void OnClick();

	void OnClick(object sender, EventArgs e);
}
