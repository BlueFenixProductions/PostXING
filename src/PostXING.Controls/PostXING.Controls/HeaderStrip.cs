using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PostXING.Controls;

public class HeaderStrip : ToolStrip
{
	private AreaHeaderStyle _headerStyle;

	private ToolStripProfessionalRenderer _pr;

	[DefaultValue(AreaHeaderStyle.Large)]
	public AreaHeaderStyle HeaderStyle
	{
		get
		{
			return _headerStyle;
		}
		set
		{
			if (_headerStyle != value)
			{
				_headerStyle = value;
				SetHeaderStyle();
			}
		}
	}

	public HeaderStrip()
	{
		Dock = DockStyle.Top;
		base.GripStyle = ToolStripGripStyle.Hidden;
		AutoSize = false;
		SetRenderer();
		SystemEvents.UserPreferenceChanged += HeaderStrip_UserPreferenceChanged;
		SetHeaderStyle();
	}

	protected override void OnRendererChanged(EventArgs e)
	{
		base.OnRendererChanged(e);
		SetRenderer();
	}

	private void SetHeaderStyle()
	{
		Font menuFont = SystemFonts.MenuFont;
		switch (_headerStyle)
		{
		case AreaHeaderStyle.Large:
			Font = new Font("Arial", menuFont.SizeInPoints + 3.75f, FontStyle.Bold);
			base.ForeColor = Color.White;
			break;
		case AreaHeaderStyle.Small:
		case AreaHeaderStyle.ControlPanelHeader:
			Font = menuFont;
			base.ForeColor = Color.Black;
			break;
		case AreaHeaderStyle.ControlPanel:
			Font = new Font(menuFont.Name, menuFont.SizeInPoints + 2f);
			base.ForeColor = SystemColors.ActiveCaptionText;
			break;
		}
		ToolStripLabel toolStripLabel = new ToolStripLabel();
		toolStripLabel.Font = Font;
		toolStripLabel.Text = "I";
		base.Height = toolStripLabel.GetPreferredSize(Size.Empty).Height + 6;
	}

	private void SetRenderer()
	{
		if (base.Renderer is ToolStripProfessionalRenderer && base.Renderer != _pr)
		{
			if (_pr == null)
			{
				_pr = new ToolStripProfessionalRenderer();
				_pr.RoundedEdges = false;
				_pr.RenderToolStripBackground += Renderer_RenderToolStripBackground;
			}
			base.Renderer = _pr;
		}
	}

	private GraphicsPath GetRoundRect(Rectangle rect, int roundness)
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		if (roundness > 1)
		{
			float num = Math.Min((float)Math.Min(rect.Width, rect.Height) / 2f, (float)roundness * 2f);
			RectangleF rect2 = new RectangleF(rect.Left, rect.Top, num * 2f, num * 2f);
			RectangleF rect3 = new RectangleF((float)rect.Right - num * 2f, (float)rect.Bottom - num * 2f - 1f, num * 2f, num * 2f);
			graphicsPath.AddArc(rect2, 180f, 90f);
			graphicsPath.AddLine((float)rect.Left + num, rect.Top, rect.Right, rect.Top);
			graphicsPath.AddArc(rect3, 0f, 90f);
			graphicsPath.AddLine((float)rect.Right - num, rect.Bottom, rect.Left, rect.Bottom);
			graphicsPath.CloseAllFigures();
		}
		else
		{
			graphicsPath.AddRectangle(rect);
		}
		return graphicsPath;
	}

	private void Renderer_RenderToolStripBackground(object sender, ToolStripRenderEventArgs e)
	{
		if (!(base.Renderer is ToolStripProfessionalRenderer))
		{
			return;
		}
		ToolStripProfessionalRenderer toolStripProfessionalRenderer = base.Renderer as ToolStripProfessionalRenderer;
		Color color;
		Color color2;
		switch (_headerStyle)
		{
		case AreaHeaderStyle.Large:
			color = toolStripProfessionalRenderer.ColorTable.OverflowButtonGradientMiddle;
			color2 = toolStripProfessionalRenderer.ColorTable.OverflowButtonGradientEnd;
			break;
		default:
			color = toolStripProfessionalRenderer.ColorTable.MenuStripGradientEnd;
			color2 = toolStripProfessionalRenderer.ColorTable.MenuStripGradientBegin;
			break;
		}
		Rectangle rect = new Rectangle(Point.Empty, e.ToolStrip.Size);
		if (rect.Width <= 0 || rect.Height <= 0)
		{
			return;
		}
		using Brush brush = new LinearGradientBrush(rect, color, color2, LinearGradientMode.Vertical);
		e.Graphics.FillRectangle(brush, rect);
		if (_headerStyle == AreaHeaderStyle.ControlPanel)
		{
			using (Brush brush2 = new SolidBrush(toolStripProfessionalRenderer.ColorTable.OverflowButtonGradientEnd))
			{
				rect.Width = 85;
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				e.Graphics.FillPath(brush2, GetRoundRect(rect, 4));
				return;
			}
		}
	}

	private void HeaderStrip_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
	{
		SetHeaderStyle();
	}
}
