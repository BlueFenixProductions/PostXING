using System.Drawing;

namespace PostXING.Controls;

public class TaskLinkInfo
{
	private Padding padding;

	private Margin margin;

	private Color linkNormal;

	private Color linkHot;

	private FontStyle fontDecoration;

	public Margin Margin
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

	public Color LinkColor
	{
		get
		{
			return linkNormal;
		}
		set
		{
			linkNormal = value;
		}
	}

	public Color HotLinkColor
	{
		get
		{
			return linkHot;
		}
		set
		{
			linkHot = value;
		}
	}

	public FontStyle FontDecoration
	{
		get
		{
			return fontDecoration;
		}
		set
		{
			fontDecoration = value;
		}
	}

	public TaskLinkInfo()
	{
		padding = new Padding(6, 0, 4, 0);
		margin = new Margin(0, 4, 0, 0);
		linkNormal = SystemColors.ControlText;
		linkHot = SystemColors.ControlText;
		fontDecoration = FontStyle.Underline;
	}

	public void SetDefaultValues()
	{
		padding.Left = 6;
		padding.Top = 0;
		padding.Right = 4;
		padding.Bottom = 0;
		margin.Left = 0;
		margin.Top = 4;
		margin.Right = 0;
		margin.Bottom = 0;
		linkNormal = SystemColors.ControlText;
		linkHot = SystemColors.HotTrack;
		fontDecoration = FontStyle.Underline;
	}
}
