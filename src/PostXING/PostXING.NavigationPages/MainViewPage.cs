using System.ComponentModel;
using System.Drawing;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class MainViewPage : Page
{
	private Container components;

	public MainViewPage()
	{
		InitializeComponent();
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
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "MainViewPage";
		base.Size = new System.Drawing.Size(600, 408);
	}
}
