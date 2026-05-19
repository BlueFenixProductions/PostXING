using System;
using System.Drawing;
using System.Reflection;
using System.Resources;

namespace PostXING.Controls;

public class ExplorerBarInfo : IDisposable
{
	private TaskPaneInfo taskPane;

	private TaskLinkInfo taskLink;

	private ExpandoInfo expando;

	private HeaderInfo header;

	public TaskPaneInfo TaskPane => taskPane;

	public TaskLinkInfo TaskLink => taskLink;

	public ExpandoInfo Expando => expando;

	public HeaderInfo Header => header;

	public ExplorerBarInfo()
	{
		taskPane = new TaskPaneInfo();
		taskLink = new TaskLinkInfo();
		expando = new ExpandoInfo();
		header = new HeaderInfo();
	}

	public void SetUnthemedArrowImages()
	{
		Assembly assembly = GetType().Assembly;
		ResourceManager resourceManager = new ResourceManager("PostXING.Controls.Controls.XPExplorerBar.ExpandoArrows", assembly);
		Header.SpecialArrowDown = new Bitmap((Image)resourceManager.GetObject("SPECIALGROUPEXPAND"));
		Header.SpecialArrowDownHot = new Bitmap((Image)resourceManager.GetObject("SPECIALGROUPEXPANDHOT"));
		Header.SpecialArrowUp = new Bitmap((Image)resourceManager.GetObject("SPECIALGROUPCOLLAPSE"));
		Header.SpecialArrowUpHot = new Bitmap((Image)resourceManager.GetObject("SPECIALGROUPCOLLAPSEHOT"));
		Header.NormalArrowDown = new Bitmap((Image)resourceManager.GetObject("NORMALGROUPEXPAND"));
		Header.NormalArrowDownHot = new Bitmap((Image)resourceManager.GetObject("NORMALGROUPEXPANDHOT"));
		Header.NormalArrowUp = new Bitmap((Image)resourceManager.GetObject("NORMALGROUPCOLLAPSE"));
		Header.NormalArrowUpHot = new Bitmap((Image)resourceManager.GetObject("NORMALGROUPCOLLAPSEHOT"));
	}

	public void UseClassicTheme()
	{
		TaskPane.SetDefaultValues();
		Expando.SetDefaultValues();
		Header.SetDefaultValues();
		TaskLink.SetDefaultValues();
		SetUnthemedArrowImages();
	}

	public void Dispose()
	{
		taskPane.Dispose();
		header.Dispose();
	}
}
