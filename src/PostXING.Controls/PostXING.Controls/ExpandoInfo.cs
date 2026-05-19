using System.Drawing;

namespace PostXING.Controls;

public class ExpandoInfo
{
	private Color specialBackColor;

	private Color normalBackColor;

	private Border specialBorder;

	private Border normalBorder;

	private Color specialBorderColor;

	private Color normalBorderColor;

	private Padding specialPadding;

	private Padding normalPadding;

	private ContentAlignment watermarkAlignment;

	public Color SpecialBackColor
	{
		get
		{
			return specialBackColor;
		}
		set
		{
			specialBackColor = value;
		}
	}

	public Color NormalBackColor
	{
		get
		{
			return normalBackColor;
		}
		set
		{
			normalBackColor = value;
		}
	}

	public ContentAlignment WatermarkAlignment
	{
		get
		{
			return watermarkAlignment;
		}
		set
		{
			watermarkAlignment = value;
		}
	}

	public Border SpecialBorder
	{
		get
		{
			return specialBorder;
		}
		set
		{
			specialBorder = value;
		}
	}

	public Border NormalBorder
	{
		get
		{
			return normalBorder;
		}
		set
		{
			normalBorder = value;
		}
	}

	public Color SpecialBorderColor
	{
		get
		{
			return specialBorderColor;
		}
		set
		{
			specialBorderColor = value;
		}
	}

	public Color NormalBorderColor
	{
		get
		{
			return normalBorderColor;
		}
		set
		{
			normalBorderColor = value;
		}
	}

	public Padding SpecialPadding
	{
		get
		{
			return specialPadding;
		}
		set
		{
			specialPadding = value;
		}
	}

	public Padding NormalPadding
	{
		get
		{
			return normalPadding;
		}
		set
		{
			normalPadding = value;
		}
	}

	public ExpandoInfo()
	{
		specialBackColor = Color.Transparent;
		normalBackColor = Color.Transparent;
		specialBorder = new Border(1, 0, 1, 1);
		specialBorderColor = Color.Transparent;
		normalBorder = new Border(1, 0, 1, 1);
		normalBorderColor = Color.Transparent;
		specialPadding = new Padding(12, 10, 12, 10);
		normalPadding = new Padding(12, 10, 12, 10);
		watermarkAlignment = ContentAlignment.BottomRight;
	}

	public void SetDefaultValues()
	{
		specialBackColor = SystemColors.Window;
		normalBackColor = SystemColors.Window;
		specialBorder.Left = 1;
		specialBorder.Top = 0;
		specialBorder.Right = 1;
		specialBorder.Bottom = 1;
		specialBorderColor = SystemColors.Highlight;
		normalBorder.Left = 1;
		normalBorder.Top = 0;
		normalBorder.Right = 1;
		normalBorder.Bottom = 1;
		normalBorderColor = SystemColors.Control;
		specialPadding.Left = 12;
		specialPadding.Top = 10;
		specialPadding.Right = 12;
		specialPadding.Bottom = 10;
		normalPadding.Left = 12;
		normalPadding.Top = 10;
		normalPadding.Right = 12;
		normalPadding.Bottom = 10;
		watermarkAlignment = ContentAlignment.BottomRight;
	}
}
