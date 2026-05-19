using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PostXING.Controls.Controls;

public class MozItemDesigner : ControlDesigner
{
	public override SelectionRules SelectionRules
	{
		get
		{
			SelectionRules selectionRules = base.SelectionRules;
			return SelectionRules.Visible;
		}
	}

	protected override void OnPaintAdornments(PaintEventArgs pe)
	{
		base.OnPaintAdornments(pe);
		Control control = base.Control;
		Rectangle clientRectangle = control.ClientRectangle;
		ControlPaint.DrawBorder(pe.Graphics, clientRectangle, Color.Black, ButtonBorderStyle.Dashed);
	}

	public override bool CanBeParentedTo(IDesigner parentDesigner)
	{
		if (parentDesigner.Component is MozPane)
		{
			return true;
		}
		return false;
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		properties.Remove("BackgroundImage");
		properties.Remove("RightToLeft");
		properties.Remove("Imemode");
	}

	protected override void PreFilterEvents(IDictionary events)
	{
		base.PreFilterEvents(events);
		events.Remove("ForeColorChanged");
		events.Remove("BackColorChanged");
		events.Remove("BorderStyleChanged");
	}
}
