using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Dotnetrix.Controls;

namespace PostXING.Controls;

public class TabControlExEx : TabControlEX
{
	private bool _hideTabs;

	private IContainer components;

	public bool HideTabs
	{
		get
		{
			return _hideTabs;
		}
		set
		{
			_hideTabs = value;
		}
	}

	public override Rectangle DisplayRectangle
	{
		get
		{
			if (HideTabs)
			{
				return new Rectangle(0, 0, base.Width, base.Height);
			}
			int num = ((base.Alignment > TabAlignment.Bottom) ? base.ItemSize.Width : base.ItemSize.Height);
			int num2 = ((base.Appearance != TabAppearanceEX.Normal) ? ((3 + num) * base.RowCount) : (num * base.RowCount));
			return base.Alignment switch
			{
				TabAlignment.Bottom => new Rectangle(0, 0, base.Width, base.Height - num2 - 4), 
				TabAlignment.Left => new Rectangle(num2, 4, base.Width - num2 - 4, base.Height - 8), 
				TabAlignment.Right => new Rectangle(4, 4, base.Width - num2 - 4, base.Height - 8), 
				_ => new Rectangle(4, num2, base.Width - 8, base.Height - num2 - 4), 
			};
		}
	}

	public TabControlExEx()
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
		this.components = new System.ComponentModel.Container();
	}
}
