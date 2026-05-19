using System;
using System.Drawing;

namespace PostXING.Controls;

public class HeaderInfo : IDisposable
{
	private Font titleFont;

	private int margin;

	private Image specialBackImage;

	private Image normalBackImage;

	private int backImageWidth;

	private int backImageHeight;

	private Color specialTitle;

	private Color normalTitle;

	private Color specialTitleHot;

	private Color normalTitleHot;

	private ContentAlignment specialAlignment;

	private ContentAlignment normalAlignment;

	private Padding specialPadding;

	private Padding normalPadding;

	private Border specialBorder;

	private Border normalBorder;

	private Color specialBorderColor;

	private Color normalBorderColor;

	private Color specialBackColor;

	private Color normalBackColor;

	private Image specialArrowUp;

	private Image specialArrowUpHot;

	private Image specialArrowDown;

	private Image specialArrowDownHot;

	private Image normalArrowUp;

	private Image normalArrowUpHot;

	private Image normalArrowDown;

	private Image normalArrowDownHot;

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

	public Font TitleFont => titleFont;

	public string FontName
	{
		get
		{
			return TitleFont.Name;
		}
		set
		{
			titleFont = new Font(value, TitleFont.SizeInPoints, TitleFont.Style);
		}
	}

	public float FontSize
	{
		get
		{
			return TitleFont.SizeInPoints;
		}
		set
		{
			titleFont = new Font(TitleFont.Name, value, TitleFont.Style);
		}
	}

	public FontStyle FontWeight
	{
		get
		{
			return TitleFont.Style;
		}
		set
		{
			value |= TitleFont.Style;
			titleFont = new Font(TitleFont.Name, TitleFont.SizeInPoints, value);
		}
	}

	public FontStyle FontStyle
	{
		get
		{
			return TitleFont.Style;
		}
		set
		{
			value |= TitleFont.Style;
			titleFont = new Font(TitleFont.Name, TitleFont.SizeInPoints, value);
		}
	}

	public Image SpecialBackImage
	{
		get
		{
			return specialBackImage;
		}
		set
		{
			specialBackImage = value;
			if (value != null)
			{
				backImageWidth = value.Width;
				backImageHeight = value.Height;
			}
		}
	}

	public Image NormalBackImage
	{
		get
		{
			return normalBackImage;
		}
		set
		{
			normalBackImage = value;
			if (value != null)
			{
				backImageWidth = value.Width;
				backImageHeight = value.Height;
			}
		}
	}

	public int BackImageWidth
	{
		get
		{
			if (backImageWidth == -1)
			{
				return 186;
			}
			return backImageWidth;
		}
		set
		{
			backImageWidth = value;
		}
	}

	public int BackImageHeight
	{
		get
		{
			if (backImageHeight < 23)
			{
				return 23;
			}
			return backImageHeight;
		}
		set
		{
			backImageHeight = value;
		}
	}

	public Image SpecialArrowUp
	{
		get
		{
			return specialArrowUp;
		}
		set
		{
			specialArrowUp = value;
		}
	}

	public Image SpecialArrowUpHot
	{
		get
		{
			return specialArrowUpHot;
		}
		set
		{
			specialArrowUpHot = value;
		}
	}

	public Image SpecialArrowDown
	{
		get
		{
			return specialArrowDown;
		}
		set
		{
			specialArrowDown = value;
		}
	}

	public Image SpecialArrowDownHot
	{
		get
		{
			return specialArrowDownHot;
		}
		set
		{
			specialArrowDownHot = value;
		}
	}

	public Image NormalArrowUp
	{
		get
		{
			return normalArrowUp;
		}
		set
		{
			normalArrowUp = value;
		}
	}

	public Image NormalArrowUpHot
	{
		get
		{
			return normalArrowUpHot;
		}
		set
		{
			normalArrowUpHot = value;
		}
	}

	public Image NormalArrowDown
	{
		get
		{
			return normalArrowDown;
		}
		set
		{
			normalArrowDown = value;
		}
	}

	public Image NormalArrowDownHot
	{
		get
		{
			return normalArrowDownHot;
		}
		set
		{
			normalArrowDownHot = value;
		}
	}

	public int Margin
	{
		get
		{
			return margin;
		}
		set
		{
			margin = value;
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

	public Color SpecialTitleColor
	{
		get
		{
			return specialTitle;
		}
		set
		{
			specialTitle = value;
			if (SpecialTitleHotColor == Color.Transparent)
			{
				SpecialTitleHotColor = value;
			}
		}
	}

	public Color SpecialTitleHotColor
	{
		get
		{
			return specialTitleHot;
		}
		set
		{
			specialTitleHot = value;
		}
	}

	public Color NormalTitleColor
	{
		get
		{
			return normalTitle;
		}
		set
		{
			normalTitle = value;
			if (NormalTitleHotColor == Color.Transparent)
			{
				NormalTitleHotColor = value;
			}
		}
	}

	public Color NormalTitleHotColor
	{
		get
		{
			return normalTitleHot;
		}
		set
		{
			normalTitleHot = value;
		}
	}

	public ContentAlignment SpecialAlignment
	{
		get
		{
			return specialAlignment;
		}
		set
		{
			specialAlignment = value;
		}
	}

	public ContentAlignment NormalAlignment
	{
		get
		{
			return normalAlignment;
		}
		set
		{
			normalAlignment = value;
		}
	}

	public HeaderInfo()
	{
		if (Environment.OSVersion.Version.Major >= 5)
		{
			titleFont = new Font("Tahoma", 8.25f, FontStyle.Bold);
		}
		else
		{
			titleFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
		}
		margin = 15;
		specialTitle = Color.Transparent;
		specialTitleHot = Color.Transparent;
		normalTitle = Color.Transparent;
		normalTitleHot = Color.Transparent;
		specialAlignment = ContentAlignment.MiddleLeft;
		normalAlignment = ContentAlignment.MiddleLeft;
		specialPadding = new Padding(10, 0, 1, 0);
		normalPadding = new Padding(10, 0, 1, 0);
		specialBorder = new Border(2, 2, 2, 0);
		specialBorderColor = Color.Transparent;
		normalBorder = new Border(2, 2, 2, 0);
		normalBorderColor = Color.Transparent;
		specialBackColor = Color.Transparent;
		normalBackColor = Color.Transparent;
		specialBackImage = null;
		normalBackImage = null;
		backImageWidth = -1;
		backImageHeight = -1;
		specialArrowUp = null;
		specialArrowUpHot = null;
		specialArrowDown = null;
		specialArrowDownHot = null;
		normalArrowUp = null;
		normalArrowUpHot = null;
		normalArrowDown = null;
		normalArrowDownHot = null;
	}

	public void SetDefaultValues()
	{
		if (Environment.OSVersion.Version.Major >= 5)
		{
			titleFont = new Font("Tahoma", 8.25f, FontStyle.Bold);
		}
		else
		{
			titleFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
		}
		margin = 15;
		specialTitle = SystemColors.HighlightText;
		specialTitleHot = SystemColors.HighlightText;
		normalTitle = SystemColors.ControlText;
		normalTitleHot = SystemColors.ControlText;
		specialAlignment = ContentAlignment.MiddleLeft;
		normalAlignment = ContentAlignment.MiddleLeft;
		specialPadding.Left = 10;
		specialPadding.Top = 0;
		specialPadding.Right = 1;
		specialPadding.Bottom = 0;
		normalPadding.Left = 10;
		normalPadding.Top = 0;
		normalPadding.Right = 1;
		normalPadding.Bottom = 0;
		specialBorder.Left = 2;
		specialBorder.Top = 2;
		specialBorder.Right = 2;
		specialBorder.Bottom = 0;
		specialBorderColor = SystemColors.Highlight;
		specialBackColor = SystemColors.Highlight;
		normalBorder.Left = 2;
		normalBorder.Top = 2;
		normalBorder.Right = 2;
		normalBorder.Bottom = 0;
		normalBorderColor = SystemColors.Control;
		normalBackColor = SystemColors.Control;
		specialBackImage = null;
		normalBackImage = null;
		backImageWidth = 186;
		backImageHeight = 25;
		specialArrowUp = null;
		specialArrowUpHot = null;
		specialArrowDown = null;
		specialArrowDownHot = null;
		normalArrowUp = null;
		normalArrowUpHot = null;
		normalArrowDown = null;
		normalArrowDownHot = null;
	}

	public void Dispose()
	{
		if (specialBackImage != null)
		{
			specialBackImage.Dispose();
			specialBackImage = null;
		}
		if (normalBackImage != null)
		{
			normalBackImage.Dispose();
			normalBackImage = null;
		}
		if (specialArrowUp != null)
		{
			specialArrowUp.Dispose();
			specialArrowUp = null;
		}
		if (specialArrowUpHot != null)
		{
			specialArrowUpHot.Dispose();
			specialArrowUpHot = null;
		}
		if (specialArrowDown != null)
		{
			specialArrowDown.Dispose();
			specialArrowDown = null;
		}
		if (specialArrowDownHot != null)
		{
			specialArrowDownHot.Dispose();
			specialArrowDownHot = null;
		}
		if (normalArrowUp != null)
		{
			normalArrowUp.Dispose();
			normalArrowUp = null;
		}
		if (normalArrowUpHot != null)
		{
			normalArrowUpHot.Dispose();
			normalArrowUpHot = null;
		}
		if (normalArrowDown != null)
		{
			normalArrowDown.Dispose();
			normalArrowDown = null;
		}
		if (normalArrowDownHot != null)
		{
			normalArrowDownHot.Dispose();
			normalArrowDownHot = null;
		}
	}
}
