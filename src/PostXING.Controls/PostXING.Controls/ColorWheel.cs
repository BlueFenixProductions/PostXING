using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PostXING.Controls;

public class ColorWheel : IDisposable
{
	public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

	public enum MouseState
	{
		MouseUp,
		ClickOnColor,
		DragInColor,
		ClickOnBrightness,
		DragInBrightness,
		ClickOutsideRegion,
		DragOutsideRegion
	}

	private const double DEGREES_PER_RADIAN = 180.0 / Math.PI;

	private const int COLOR_COUNT = 1536;

	private Graphics g;

	private Region colorRegion;

	private Region brightnessRegion;

	private Bitmap colorImage;

	public ColorChangedEventHandler ColorChanged;

	private MouseState currentState;

	private Point centerPoint;

	private int radius;

	private Rectangle colorRectangle;

	private Rectangle brightnessRectangle;

	private Rectangle selectedColorRectangle;

	private int brightnessX;

	private double brightnessScaling;

	private Color selectedColor = Color.White;

	private Color fullColor;

	private ColorHandler.RGB RGB;

	private ColorHandler.HSV HSV;

	private Point colorPoint;

	private Point brightnessPoint;

	private int brightness;

	private int brightnessMin;

	private int brightnessMax;

	public Color Color => selectedColor;

	public ColorWheel(Rectangle colorRectangle, Rectangle brightnessRectangle, Rectangle selectedColorRectangle)
	{
		using GraphicsPath graphicsPath = new GraphicsPath();
		this.colorRectangle = colorRectangle;
		this.brightnessRectangle = brightnessRectangle;
		this.selectedColorRectangle = selectedColorRectangle;
		radius = Math.Min(colorRectangle.Width, colorRectangle.Height) / 2;
		centerPoint = colorRectangle.Location;
		centerPoint.Offset(radius, radius);
		colorPoint = centerPoint;
		graphicsPath.AddEllipse(colorRectangle);
		colorRegion = new Region(graphicsPath);
		brightnessMin = this.brightnessRectangle.Top;
		brightnessMax = this.brightnessRectangle.Bottom;
		graphicsPath.AddRectangle(new Rectangle(brightnessRectangle.Left, brightnessRectangle.Top - 10, brightnessRectangle.Width + 10, brightnessRectangle.Height + 20));
		brightnessRegion = new Region(graphicsPath);
		brightnessX = brightnessRectangle.Left + brightnessRectangle.Width;
		brightnessScaling = 255.0 / (double)(brightnessMax - brightnessMin);
		brightnessPoint = new Point(brightnessX, brightnessMax);
		CreateGradient();
	}

	protected void OnColorChanged(ColorHandler.RGB RGB, ColorHandler.HSV HSV)
	{
		ColorChangedEventArgs e = new ColorChangedEventArgs(RGB, HSV);
		ColorChanged(this, e);
	}

	void IDisposable.Dispose()
	{
		if (colorImage != null)
		{
			colorImage.Dispose();
		}
		if (colorRegion != null)
		{
			colorRegion.Dispose();
		}
		if (brightnessRegion != null)
		{
			brightnessRegion.Dispose();
		}
		if (g != null)
		{
			g.Dispose();
		}
	}

	public void SetMouseUp()
	{
		currentState = MouseState.MouseUp;
	}

	public void Draw(Graphics g, ColorHandler.HSV HSV)
	{
		this.g = g;
		this.HSV = HSV;
		CalcCoordsAndUpdate(this.HSV);
		UpdateDisplay();
	}

	public void Draw(Graphics g, ColorHandler.RGB RGB)
	{
		this.g = g;
		HSV = ColorHandler.RGBtoHSV(RGB);
		CalcCoordsAndUpdate(HSV);
		UpdateDisplay();
	}

	public void Draw(Graphics g, Point mousePoint)
	{
		Point point = colorPoint;
		Point point2 = brightnessPoint;
		this.g = g;
		if (currentState == MouseState.MouseUp && !mousePoint.IsEmpty)
		{
			if (colorRegion.IsVisible(mousePoint))
			{
				currentState = MouseState.ClickOnColor;
			}
			else if (brightnessRegion.IsVisible(mousePoint))
			{
				currentState = MouseState.ClickOnBrightness;
			}
			else
			{
				currentState = MouseState.ClickOutsideRegion;
			}
		}
		switch (currentState)
		{
		case MouseState.ClickOnBrightness:
		case MouseState.DragInBrightness:
		{
			Point point3 = mousePoint;
			if (point3.Y < brightnessMin)
			{
				point3.Y = brightnessMin;
			}
			else if (point3.Y > brightnessMax)
			{
				point3.Y = brightnessMax;
			}
			point2 = new Point(brightnessX, point3.Y);
			brightness = (int)((double)(brightnessMax - point3.Y) * brightnessScaling);
			HSV.value = brightness;
			RGB = ColorHandler.HSVtoRGB(HSV);
			break;
		}
		case MouseState.ClickOnColor:
		case MouseState.DragInColor:
		{
			point = mousePoint;
			Point pt = new Point(mousePoint.X - centerPoint.X, mousePoint.Y - centerPoint.Y);
			int num = CalcDegrees(pt);
			double num2 = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y) / (double)radius;
			if (currentState == MouseState.DragInColor && num2 > 1.0)
			{
				num2 = 1.0;
				point = GetPoint(num, radius, centerPoint);
			}
			HSV.Hue = num * 255 / 360;
			HSV.Saturation = (int)(num2 * 255.0);
			HSV.value = brightness;
			RGB = ColorHandler.HSVtoRGB(HSV);
			fullColor = ColorHandler.HSVtoColor(HSV.Hue, HSV.Saturation, 255);
			break;
		}
		}
		selectedColor = ColorHandler.HSVtoColor(HSV);
		OnColorChanged(RGB, HSV);
		switch (currentState)
		{
		case MouseState.ClickOnBrightness:
			currentState = MouseState.DragInBrightness;
			break;
		case MouseState.ClickOnColor:
			currentState = MouseState.DragInColor;
			break;
		case MouseState.ClickOutsideRegion:
			currentState = MouseState.DragOutsideRegion;
			break;
		}
		colorPoint = point;
		brightnessPoint = point2;
		UpdateDisplay();
	}

	private Point CalcBrightnessPoint(int brightness)
	{
		return new Point(brightnessX, (int)((double)brightnessMax - (double)brightness / brightnessScaling));
	}

	private void UpdateDisplay()
	{
		using Brush brush = new SolidBrush(selectedColor);
		g.DrawImage(colorImage, colorRectangle);
		g.FillRectangle(brush, selectedColorRectangle);
		DrawLinearGradient(fullColor);
		DrawColorPointer(colorPoint);
		DrawBrightnessPointer(brightnessPoint);
	}

	private void CalcCoordsAndUpdate(ColorHandler.HSV HSV)
	{
		colorPoint = GetPoint((double)HSV.Hue / 255.0 * 360.0, (double)HSV.Saturation / 255.0 * (double)radius, centerPoint);
		brightnessPoint = CalcBrightnessPoint(HSV.value);
		brightness = HSV.value;
		selectedColor = ColorHandler.HSVtoColor(HSV);
		RGB = ColorHandler.HSVtoRGB(HSV);
		fullColor = ColorHandler.HSVtoColor(HSV.Hue, HSV.Saturation, 255);
	}

	private void DrawLinearGradient(Color TopColor)
	{
		using LinearGradientBrush brush = new LinearGradientBrush(brightnessRectangle, TopColor, Color.Black, LinearGradientMode.Vertical);
		g.FillRectangle(brush, brightnessRectangle);
	}

	private int CalcDegrees(Point pt)
	{
		if (pt.X == 0)
		{
			if (pt.Y > 0)
			{
				return 270;
			}
			return 90;
		}
		int num = (int)((0.0 - Math.Atan((double)pt.Y / (double)pt.X)) * (180.0 / Math.PI));
		if (pt.X < 0)
		{
			num += 180;
		}
		return (num + 360) % 360;
	}

	private void CreateGradient()
	{
		using PathGradientBrush pathGradientBrush = new PathGradientBrush(GetPoints(radius, new Point(radius, radius)));
		pathGradientBrush.CenterColor = Color.White;
		pathGradientBrush.CenterPoint = new PointF(radius, radius);
		pathGradientBrush.SurroundColors = GetColors();
		colorImage = new Bitmap(colorRectangle.Width, colorRectangle.Height, PixelFormat.Format32bppArgb);
		using Graphics graphics = Graphics.FromImage(colorImage);
		graphics.FillEllipse(pathGradientBrush, 0, 0, colorRectangle.Width, colorRectangle.Height);
	}

	private Color[] GetColors()
	{
		Color[] array = new Color[1536];
		for (int i = 0; i <= 1535; i++)
		{
			ref Color reference = ref array[i];
			reference = ColorHandler.HSVtoColor((int)((double)(i * 255) / 1536.0), 255, 255);
		}
		return array;
	}

	private Point[] GetPoints(double radius, Point centerPoint)
	{
		Point[] array = new Point[1536];
		for (int i = 0; i <= 1535; i++)
		{
			ref Point reference = ref array[i];
			reference = GetPoint((double)(i * 360) / 1536.0, radius, centerPoint);
		}
		return array;
	}

	private Point GetPoint(double degrees, double radius, Point centerPoint)
	{
		double num = degrees / (180.0 / Math.PI);
		return new Point((int)((double)centerPoint.X + Math.Floor(radius * Math.Cos(num))), (int)((double)centerPoint.Y - Math.Floor(radius * Math.Sin(num))));
	}

	private void DrawColorPointer(Point pt)
	{
		g.DrawRectangle(Pens.Black, pt.X - 3, pt.Y - 3, 6, 6);
	}

	private void DrawBrightnessPointer(Point pt)
	{
		Point[] points = new Point[3]
		{
			pt,
			new Point(pt.X + 7, pt.Y + 5),
			new Point(pt.X + 7, pt.Y - 5)
		};
		g.FillPolygon(Brushes.Black, points);
	}
}
