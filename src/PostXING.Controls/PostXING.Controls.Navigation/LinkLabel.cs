using System;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls.Navigation;

[ToolboxBitmap(typeof(System.Windows.Forms.LinkLabel))]
public class LinkLabel : System.Windows.Forms.LinkLabel
{
	private Color saveLinkColor = Color.Empty;

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		if (!base.DesignMode)
		{
			saveLinkColor = base.LinkColor;
			base.LinkColor = base.ActiveLinkColor;
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		MouseButtons button = e.Button;
		if (button != MouseButtons.XButton1 && button != MouseButtons.XButton2)
		{
			base.OnMouseUp(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		if (!base.DesignMode)
		{
			base.LinkColor = saveLinkColor;
		}
	}
}
