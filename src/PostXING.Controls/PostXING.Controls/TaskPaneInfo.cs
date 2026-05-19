using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PostXING.Controls;

public class TaskPaneInfo : IDisposable
{
	private Color gradientStartColor;

	private Color gradientEndColor;

	private LinearGradientMode direction;

	private Padding padding;

	private Border border;

	private Color borderColor;

	private Image backImage;

	private ImageStretchMode stretchMode;

	private Image watermark;

	private ContentAlignment watermarkAlignment;

	public Color GradientStartColor
	{
		get
		{
			return gradientStartColor;
		}
		set
		{
			gradientStartColor = value;
		}
	}

	public Color GradientEndColor
	{
		get
		{
			return gradientEndColor;
		}
		set
		{
			gradientEndColor = value;
		}
	}

	public LinearGradientMode GradientDirection
	{
		get
		{
			return direction;
		}
		set
		{
			direction = value;
		}
	}

	public Border Border
	{
		get
		{
			return border;
		}
		set
		{
			border = value;
		}
	}

	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			borderColor = value;
		}
	}

	public Image BackImage
	{
		get
		{
			return backImage;
		}
		set
		{
			backImage = value;
		}
	}

	public ImageStretchMode StretchMode
	{
		get
		{
			return stretchMode;
		}
		set
		{
			stretchMode = value;
		}
	}

	public Image Watermark
	{
		get
		{
			return watermark;
		}
		set
		{
			watermark = value;
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

	public Padding Padding
	{
		get
		{
			return padding;
		}
		set
		{
			padding = value;
		}
	}

	public TaskPaneInfo()
	{
		gradientStartColor = Color.Transparent;
		gradientEndColor = Color.Transparent;
		direction = LinearGradientMode.Vertical;
		padding = new Padding(12, 12, 12, 12);
		border = new Border();
		borderColor = Color.Transparent;
		backImage = null;
		stretchMode = ImageStretchMode.Tile;
		watermark = null;
		watermarkAlignment = ContentAlignment.BottomCenter;
	}

	public void SetDefaultValues()
	{
		gradientStartColor = SystemColors.Window;
		gradientEndColor = SystemColors.Window;
		direction = LinearGradientMode.Vertical;
		padding.Left = 12;
		padding.Top = 12;
		padding.Right = 12;
		padding.Bottom = 12;
		border = new Border();
		borderColor = SystemColors.Window;
		backImage = null;
		watermark = null;
	}

	public void Dispose()
	{
		if (backImage != null)
		{
			backImage.Dispose();
			backImage = null;
		}
		if (watermark != null)
		{
			watermark.Dispose();
			watermark = null;
		}
	}
}
