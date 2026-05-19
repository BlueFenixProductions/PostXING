using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PostXING.Controls;

public class ToolStripColorButton : ToolStripButton
{
	protected class ColorPanel : Form
	{
		private ToolStripColorButton colorButton;

		private int colorIndex = -1;

		private int keyboardIndex = -50;

		private Color[] colorList = new Color[40]
		{
			Color.FromArgb(0, 0, 0),
			Color.FromArgb(153, 51, 0),
			Color.FromArgb(51, 51, 0),
			Color.FromArgb(0, 51, 0),
			Color.FromArgb(0, 51, 102),
			Color.FromArgb(0, 0, 128),
			Color.FromArgb(51, 51, 153),
			Color.FromArgb(51, 51, 51),
			Color.FromArgb(128, 0, 0),
			Color.FromArgb(255, 102, 0),
			Color.FromArgb(128, 128, 0),
			Color.FromArgb(0, 128, 0),
			Color.FromArgb(0, 128, 128),
			Color.FromArgb(0, 0, 255),
			Color.FromArgb(102, 102, 153),
			Color.FromArgb(128, 128, 128),
			Color.FromArgb(255, 0, 0),
			Color.FromArgb(255, 153, 0),
			Color.FromArgb(153, 204, 0),
			Color.FromArgb(51, 153, 102),
			Color.FromArgb(51, 204, 204),
			Color.FromArgb(51, 102, 255),
			Color.FromArgb(128, 0, 128),
			Color.FromArgb(153, 153, 153),
			Color.FromArgb(255, 0, 255),
			Color.FromArgb(255, 204, 0),
			Color.FromArgb(255, 255, 0),
			Color.FromArgb(0, 255, 0),
			Color.FromArgb(0, 255, 255),
			Color.FromArgb(0, 204, 255),
			Color.FromArgb(153, 51, 102),
			Color.FromArgb(192, 192, 192),
			Color.FromArgb(255, 153, 204),
			Color.FromArgb(255, 204, 153),
			Color.FromArgb(255, 255, 153),
			Color.FromArgb(204, 255, 204),
			Color.FromArgb(204, 255, 255),
			Color.FromArgb(153, 204, 255),
			Color.FromArgb(204, 153, 255),
			Color.FromArgb(255, 255, 255)
		};

		public ColorPanel(Point pt, ToolStripColorButton button)
		{
			colorButton = button;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.ControlBox = false;
			base.ShowInTaskbar = false;
			base.TopMost = true;
			SetStyle(ControlStyles.DoubleBuffer, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			base.Width = 156;
			base.Height = 100;
			if (colorButton.AutomaticText != "")
			{
				base.Height += 23;
			}
			if (colorButton.MoreColorsText != "")
			{
				base.Height += 23;
			}
			CenterToScreen();
			base.Location = pt;
			base.Capture = true;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			colorButton.PanelVisible = false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Color color = SystemColors.ControlLightLight;
			Color color2 = SystemColors.ControlLight;
			string text;
			if (UxTheme.AppThemed && UxTheme.ThemeName.ToLower().EndsWith("luna.msstyles") && (text = UxTheme.ColorName.ToLower()) != null && text == "metallic")
			{
				color = Color.FromArgb(243, 243, 247);
				color2 = Color.FromArgb(215, 215, 229);
			}
			using (LinearGradientBrush brush = new LinearGradientBrush(base.ClientRectangle, color, color2, LinearGradientMode.Vertical))
			{
				e.Graphics.FillRectangle(brush, base.ClientRectangle);
			}
			using (Pen pen = new Pen(SystemColors.ControlDark))
			{
				using Pen pen2 = new Pen(SystemColors.ControlLightLight);
				using SolidBrush brush2 = new SolidBrush(SystemColors.ControlLightLight);
				bool flag = false;
				int num = 6;
				int num2 = 5;
				if (colorButton.AutomaticText != "")
				{
					flag = colorButton.Color == Color.Transparent;
					DrawButton(e, num, num2, colorButton.AutomaticText, 100, flag);
					num2 += 23;
				}
				for (int i = 0; i < 40; i++)
				{
					if (colorButton.Color.ToArgb() == colorList[i].ToArgb())
					{
						flag = true;
					}
					if (colorIndex == i)
					{
						e.Graphics.DrawRectangle(pen2, num - 3, num2 - 3, 17, 17);
						e.Graphics.DrawLine(pen, num - 2, num2 + 14, num + 14, num2 + 14);
						e.Graphics.DrawLine(pen, num + 14, num2 - 2, num + 14, num2 + 14);
					}
					else if (colorButton.Color.ToArgb() == colorList[i].ToArgb())
					{
						if (keyboardIndex == -50)
						{
							keyboardIndex = i;
						}
						e.Graphics.FillRectangle(brush2, num - 3, num2 - 3, 18, 18);
						e.Graphics.DrawLine(pen, num - 3, num2 - 3, num + 13, num2 - 3);
						e.Graphics.DrawLine(pen, num - 3, num2 - 3, num - 3, num2 + 13);
					}
					e.Graphics.FillRectangle(new SolidBrush(colorList[i]), num, num2, 11, 11);
					e.Graphics.DrawRectangle(pen, num, num2, 11, 11);
					if ((i + 1) % 8 == 0)
					{
						num = 6;
						num2 += 18;
					}
					else
					{
						num += 18;
					}
				}
				if (colorButton.MoreColorsText != "")
				{
					DrawButton(e, num, num2, colorButton.MoreColorsText, 101, !flag);
				}
			}
			base.OnPaint(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.KeyCode == Keys.Left)
			{
				MoveIndex(-1);
			}
			else if (e.KeyCode == Keys.Up)
			{
				MoveIndex(-8);
			}
			else if (e.KeyCode == Keys.Down)
			{
				MoveIndex(8);
			}
			else if (e.KeyCode == Keys.Right)
			{
				MoveIndex(1);
			}
			else if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Space)
			{
				OnClick(EventArgs.Empty);
			}
			else
			{
				base.OnKeyDown(e);
			}
		}

		private void MoveIndex(int delta)
		{
			int num = ((colorButton.AutomaticText != "") ? (-8) : 0);
			int num2 = 39 + ((colorButton.MoreColorsText != "") ? 8 : 0);
			int num3 = num2 - num + 1;
			if (delta == -1 && keyboardIndex < 0)
			{
				keyboardIndex = num2;
			}
			else if (delta == 1 && keyboardIndex > 39)
			{
				keyboardIndex = num;
			}
			else if (delta == 1 && keyboardIndex < 0)
			{
				keyboardIndex = 0;
			}
			else if (delta == -1 && keyboardIndex > 39)
			{
				keyboardIndex = 39;
			}
			else
			{
				keyboardIndex += delta;
			}
			if (keyboardIndex < num)
			{
				keyboardIndex += num3;
			}
			if (keyboardIndex > num2)
			{
				keyboardIndex -= num3;
			}
			if (keyboardIndex < 0)
			{
				colorIndex = 100;
			}
			else if (keyboardIndex > 39)
			{
				colorIndex = 101;
			}
			else
			{
				colorIndex = keyboardIndex;
			}
			Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (RectangleToScreen(base.ClientRectangle).Contains(Cursor.Position))
			{
				base.OnMouseDown(e);
			}
			else
			{
				Close();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (RectangleToScreen(base.ClientRectangle).Contains(Cursor.Position))
			{
				Point pt = PointToClient(Cursor.Position);
				int num = 6;
				int num2 = 5;
				if (colorButton.AutomaticText != "")
				{
					if (SetColorIndex(new Rectangle(num - 3, num2 - 3, 143, 22), pt, 100))
					{
						return;
					}
					num2 += 23;
				}
				for (int i = 0; i < 40; i++)
				{
					if (SetColorIndex(new Rectangle(num - 3, num2 - 3, 17, 17), pt, i))
					{
						return;
					}
					if ((i + 1) % 8 == 0)
					{
						num = 6;
						num2 += 18;
					}
					else
					{
						num += 18;
					}
				}
				if (colorButton.MoreColorsText != "" && SetColorIndex(new Rectangle(num - 3, num2 - 3, 143, 22), pt, 101))
				{
					return;
				}
			}
			if (colorIndex != -1)
			{
				colorIndex = -1;
				Invalidate();
			}
		}

		protected override void OnClick(EventArgs e)
		{
			if (colorIndex < 0)
			{
				return;
			}
			if (colorIndex < 40)
			{
				colorButton.Color = colorList[colorIndex];
			}
			else if (colorIndex == 100)
			{
				colorButton.Color = Color.Transparent;
			}
			else if (colorButton.UseCustomColorDialog)
			{
				ColorWheelDialog colorWheelDialog = new ColorWheelDialog();
				colorWheelDialog.Color = colorButton.Color;
				if (colorWheelDialog.ShowDialog(this) != DialogResult.OK)
				{
					Close();
					return;
				}
				colorButton.Color = colorWheelDialog.Color;
			}
			else
			{
				ColorDialog colorDialog = new ColorDialog();
				colorDialog.Color = colorButton.Color;
				colorDialog.FullOpen = true;
				if (colorDialog.ShowDialog(this) != DialogResult.OK)
				{
					Close();
					return;
				}
				colorButton.Color = colorDialog.Color;
			}
			Close();
			colorButton.OnChanged(EventArgs.Empty);
		}

		protected void DrawButton(PaintEventArgs e, int x, int y, string text, int index, bool selected)
		{
			Pen pen = new Pen(SystemColors.ControlDark);
			Pen pen2 = new Pen(SystemColors.ControlLightLight);
			SolidBrush brush = new SolidBrush(SystemColors.ControlLightLight);
			if (colorIndex == index)
			{
				e.Graphics.DrawRectangle(pen2, x - 3, y - 3, 143, 22);
				e.Graphics.DrawLine(pen, x - 2, y + 19, x + 140, y + 19);
				e.Graphics.DrawLine(pen, x + 140, y - 2, x + 140, y + 19);
			}
			else if (selected)
			{
				e.Graphics.FillRectangle(brush, x - 3, y - 3, 144, 23);
				e.Graphics.DrawLine(pen, x - 3, y - 3, x + 139, y - 3);
				e.Graphics.DrawLine(pen, x - 3, y - 3, x - 3, y + 18);
			}
			Rectangle rectangle = new Rectangle(x, y, 137, 16);
			SolidBrush brush2 = new SolidBrush(SystemColors.ControlText);
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			e.Graphics.DrawRectangle(pen, rectangle);
			e.Graphics.DrawString(text, colorButton.Font, brush2, rectangle, stringFormat);
		}

		protected bool SetColorIndex(Rectangle rc, Point pt, int index)
		{
			if (rc.Contains(pt))
			{
				if (colorIndex != index)
				{
					colorIndex = index;
					Invalidate();
				}
				return true;
			}
			return false;
		}
	}

	private IContainer components;

	private bool _panelVisible;

	private Color _color;

	private string _automatic = "Automatic";

	private string _morecolors = "More Colors...";

	private bool _useCustomColorDialog;

	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}

	public string AutomaticText
	{
		get
		{
			return _automatic;
		}
		set
		{
			_automatic = value;
		}
	}

	public string MoreColorsText
	{
		get
		{
			return _morecolors;
		}
		set
		{
			_morecolors = value;
		}
	}

	public bool UseCustomColorDialog
	{
		get
		{
			return _useCustomColorDialog;
		}
		set
		{
			_useCustomColorDialog = value;
		}
	}

	public bool PanelVisible
	{
		get
		{
			return _panelVisible;
		}
		set
		{
			_panelVisible = value;
		}
	}

	public event EventHandler Changed;

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
		components = new Container();
	}

	public ToolStripColorButton()
	{
		InitializeComponent();
		Text = string.Empty;
	}

	protected virtual void OnChanged(EventArgs e)
	{
		if (this.Changed != null)
		{
			this.Changed(this, e);
		}
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		base.OnPaint(pe);
	}

	protected override void OnClick(EventArgs e)
	{
		_panelVisible = true;
		Invalidate();
		Point pt = base.Parent.PointToScreen(new Point(Bounds.Left, Bounds.Bottom));
		ColorPanel colorPanel = new ColorPanel(pt, this);
		colorPanel.Show();
		base.OnClick(e);
	}
}
