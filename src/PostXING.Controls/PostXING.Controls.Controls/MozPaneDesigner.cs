using System.Collections;
using System.Windows.Forms.Design;

namespace PostXING.Controls.Controls;

public class MozPaneDesigner : ScrollableControlDesigner
{
	public override SelectionRules SelectionRules
	{
		get
		{
			SelectionRules selectionRules = base.SelectionRules;
			return SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
		properties.Remove("BackgroundImage");
		properties.Remove("ForeColor");
		properties.Remove("Text");
		properties.Remove("RightToLeft");
		properties.Remove("ImeMode");
		properties.Remove("AutoScroll");
	}
}
