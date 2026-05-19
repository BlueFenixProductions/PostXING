using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls;

public class SmoothProgressBar : UserControl
{
	private Container components;

	private int min;

	private int max = 100;

	private int val;

	private Color BarColor = Color.Blue;

	public int Minimum
	{
		get
		{
			return min;
		}
		set
		{
			if (value < 0)
			{
				min = 0;
			}
			if (value > max)
			{
				min = value;
				min = value;
			}
			if (val < min)
			{
				val = min;
			}
			Invalidate();
		}
	}

	public int Maximum
	{
		get
		{
			return max;
		}
		set
		{
			if (value < min)
			{
				min = value;
			}
			max = value;
			if (val > max)
			{
				val = max;
			}
			Invalidate();
		}
	}

	public int Value
	{
		get
		{
			return val;
		}
		set
		{
			int num = val;
			if (value < min)
			{
				val = min;
			}
			else if (value > max)
			{
				val = max;
			}
			else
			{
				val = value;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle clientRectangle2 = base.ClientRectangle;
			float num2 = (float)(val - min) / (float)(max - min);
			clientRectangle.Width = (int)((float)clientRectangle.Width * num2);
			num2 = (float)(num - min) / (float)(max - min);
			clientRectangle2.Width = (int)((float)clientRectangle2.Width * num2);
			Rectangle rc = default(Rectangle);
			if (clientRectangle.Width > clientRectangle2.Width)
			{
				rc.X = clientRectangle2.Size.Width;
				rc.Width = clientRectangle.Width - clientRectangle2.Width;
			}
			else
			{
				rc.X = clientRectangle.Size.Width;
				rc.Width = clientRectangle2.Width - clientRectangle.Width;
			}
			rc.Height = base.Height;
			Invalidate(rc);
		}
	}

	public Color ProgressBarColor
	{
		get
		{
			return BarColor;
		}
		set
		{
			BarColor = value;
			Invalidate();
		}
	}

	public SmoothProgressBar()
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
		base.Name = "SmoothProgressBar";
		base.Size = new System.Drawing.Size(150, 24);
	}

	protected override void OnResize(EventArgs e)
	{
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		SolidBrush solidBrush = new SolidBrush(BarColor);
		float num = (float)(val - min) / (float)(max - min);
		Rectangle clientRectangle = base.ClientRectangle;
		clientRectangle.Width = (int)((float)clientRectangle.Width * num);
		graphics.FillRectangle(solidBrush, clientRectangle);
		Draw3DBorder(graphics);
		solidBrush.Dispose();
		graphics.Dispose();
	}

	private void Draw3DBorder(Graphics g)
	{
		int num = (int)Pens.White.Width;
		g.DrawLine(Pens.DarkGray, new Point(base.ClientRectangle.Left, base.ClientRectangle.Top), new Point(base.ClientRectangle.Width - num, base.ClientRectangle.Top));
		g.DrawLine(Pens.DarkGray, new Point(base.ClientRectangle.Left, base.ClientRectangle.Top), new Point(base.ClientRectangle.Left, base.ClientRectangle.Height - num));
		g.DrawLine(Pens.White, new Point(base.ClientRectangle.Left, base.ClientRectangle.Height - num), new Point(base.ClientRectangle.Width - num, base.ClientRectangle.Height - num));
		g.DrawLine(Pens.White, new Point(base.ClientRectangle.Width - num, base.ClientRectangle.Top), new Point(base.ClientRectangle.Width - num, base.ClientRectangle.Height - num));
	}
}
