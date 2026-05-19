using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(ColorWheelDialog))]
public class ColorWheelDialog : Form
{
	private enum ChangeStyle
	{
		MouseMove,
		RGB,
		HSV,
		None
	}

	internal Button btnCancel;

	internal Button btnOK;

	internal Label Label3;

	internal NumericUpDown nudSaturation;

	internal Label Label7;

	internal NumericUpDown nudBrightness;

	internal NumericUpDown nudRed;

	internal Panel pnlColor;

	internal Label Label6;

	internal Label Label1;

	internal Label Label5;

	internal Panel pnlSelectedColor;

	internal Panel pnlBrightness;

	internal NumericUpDown nudBlue;

	internal Label Label4;

	internal NumericUpDown nudGreen;

	internal Label Label2;

	internal NumericUpDown nudHue;

	private Container components;

	private ChangeStyle changeType = ChangeStyle.None;

	private Point selectedPoint;

	private ColorWheel myColorWheel;

	private ColorHandler.RGB RGB;

	private ColorHandler.HSV HSV;

	private bool isInUpdate;

	public Color Color
	{
		get
		{
			return myColorWheel.Color;
		}
		set
		{
			changeType = ChangeStyle.RGB;
			RGB = new ColorHandler.RGB(value.R, value.G, value.B);
			HSV = ColorHandler.RGBtoHSV(RGB);
		}
	}

	public ColorWheelDialog()
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
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnOK = new System.Windows.Forms.Button();
		this.Label3 = new System.Windows.Forms.Label();
		this.nudSaturation = new System.Windows.Forms.NumericUpDown();
		this.Label7 = new System.Windows.Forms.Label();
		this.nudBrightness = new System.Windows.Forms.NumericUpDown();
		this.nudRed = new System.Windows.Forms.NumericUpDown();
		this.pnlColor = new System.Windows.Forms.Panel();
		this.Label6 = new System.Windows.Forms.Label();
		this.Label1 = new System.Windows.Forms.Label();
		this.Label5 = new System.Windows.Forms.Label();
		this.pnlSelectedColor = new System.Windows.Forms.Panel();
		this.pnlBrightness = new System.Windows.Forms.Panel();
		this.nudBlue = new System.Windows.Forms.NumericUpDown();
		this.Label4 = new System.Windows.Forms.Label();
		this.nudGreen = new System.Windows.Forms.NumericUpDown();
		this.Label2 = new System.Windows.Forms.Label();
		this.nudHue = new System.Windows.Forms.NumericUpDown();
		((System.ComponentModel.ISupportInitialize)this.nudSaturation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudBrightness).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudRed).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudBlue).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudGreen).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nudHue).BeginInit();
		base.SuspendLayout();
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(192, 320);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(64, 24);
		this.btnCancel.TabIndex = 55;
		this.btnCancel.Text = "Cancel";
		this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btnOK.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOK.Location = new System.Drawing.Point(120, 320);
		this.btnOK.Name = "btnOK";
		this.btnOK.Size = new System.Drawing.Size(64, 24);
		this.btnOK.TabIndex = 54;
		this.btnOK.Text = "OK";
		this.Label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label3.Location = new System.Drawing.Point(152, 280);
		this.Label3.Name = "Label3";
		this.Label3.Size = new System.Drawing.Size(48, 23);
		this.Label3.TabIndex = 45;
		this.Label3.Text = "Blue:";
		this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.nudSaturation.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.nudSaturation.Location = new System.Drawing.Point(96, 256);
		this.nudSaturation.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.nudSaturation.Name = "nudSaturation";
		this.nudSaturation.Size = new System.Drawing.Size(48, 23);
		this.nudSaturation.TabIndex = 42;
		this.nudSaturation.TextChanged += new System.EventHandler(HandleTextChanged);
		this.nudSaturation.ValueChanged += new System.EventHandler(HandleHSVChange);
		this.Label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label7.Location = new System.Drawing.Point(16, 280);
		this.Label7.Name = "Label7";
		this.Label7.Size = new System.Drawing.Size(72, 23);
		this.Label7.TabIndex = 50;
		this.Label7.Text = "Brightness:";
		this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.nudBrightness.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.nudBrightness.Location = new System.Drawing.Point(96, 280);
		this.nudBrightness.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.nudBrightness.Name = "nudBrightness";
		this.nudBrightness.Size = new System.Drawing.Size(48, 23);
		this.nudBrightness.TabIndex = 47;
		this.nudBrightness.TextChanged += new System.EventHandler(HandleTextChanged);
		this.nudBrightness.ValueChanged += new System.EventHandler(HandleHSVChange);
		this.nudRed.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.nudRed.Location = new System.Drawing.Point(208, 232);
		this.nudRed.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.nudRed.Name = "nudRed";
		this.nudRed.Size = new System.Drawing.Size(48, 23);
		this.nudRed.TabIndex = 38;
		this.nudRed.TextChanged += new System.EventHandler(HandleTextChanged);
		this.nudRed.ValueChanged += new System.EventHandler(HandleRGBChange);
		this.pnlColor.Location = new System.Drawing.Point(8, 8);
		this.pnlColor.Name = "pnlColor";
		this.pnlColor.Size = new System.Drawing.Size(176, 176);
		this.pnlColor.TabIndex = 51;
		this.pnlColor.Visible = false;
		this.pnlColor.MouseUp += new System.Windows.Forms.MouseEventHandler(frmMain_MouseUp);
		this.Label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label6.Location = new System.Drawing.Point(16, 256);
		this.Label6.Name = "Label6";
		this.Label6.Size = new System.Drawing.Size(72, 23);
		this.Label6.TabIndex = 49;
		this.Label6.Text = "Saturation:";
		this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label1.Location = new System.Drawing.Point(152, 232);
		this.Label1.Name = "Label1";
		this.Label1.Size = new System.Drawing.Size(48, 23);
		this.Label1.TabIndex = 43;
		this.Label1.Text = "Red:";
		this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.Label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label5.Location = new System.Drawing.Point(16, 232);
		this.Label5.Name = "Label5";
		this.Label5.Size = new System.Drawing.Size(72, 23);
		this.Label5.TabIndex = 48;
		this.Label5.Text = "Hue:";
		this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.pnlSelectedColor.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.pnlSelectedColor.Location = new System.Drawing.Point(208, 200);
		this.pnlSelectedColor.Name = "pnlSelectedColor";
		this.pnlSelectedColor.Size = new System.Drawing.Size(48, 24);
		this.pnlSelectedColor.TabIndex = 53;
		this.pnlSelectedColor.Visible = false;
		this.pnlBrightness.Location = new System.Drawing.Point(208, 8);
		this.pnlBrightness.Name = "pnlBrightness";
		this.pnlBrightness.Size = new System.Drawing.Size(16, 176);
		this.pnlBrightness.TabIndex = 52;
		this.pnlBrightness.Visible = false;
		this.nudBlue.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.nudBlue.Location = new System.Drawing.Point(208, 280);
		this.nudBlue.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.nudBlue.Name = "nudBlue";
		this.nudBlue.Size = new System.Drawing.Size(48, 23);
		this.nudBlue.TabIndex = 40;
		this.nudBlue.TextChanged += new System.EventHandler(HandleTextChanged);
		this.nudBlue.ValueChanged += new System.EventHandler(HandleRGBChange);
		this.Label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label4.Location = new System.Drawing.Point(152, 200);
		this.Label4.Name = "Label4";
		this.Label4.Size = new System.Drawing.Size(48, 24);
		this.Label4.TabIndex = 46;
		this.Label4.Text = "Color:";
		this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.nudGreen.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.nudGreen.Location = new System.Drawing.Point(208, 256);
		this.nudGreen.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.nudGreen.Name = "nudGreen";
		this.nudGreen.Size = new System.Drawing.Size(48, 23);
		this.nudGreen.TabIndex = 39;
		this.nudGreen.TextChanged += new System.EventHandler(HandleTextChanged);
		this.nudGreen.ValueChanged += new System.EventHandler(HandleRGBChange);
		this.Label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Label2.Location = new System.Drawing.Point(152, 256);
		this.Label2.Name = "Label2";
		this.Label2.Size = new System.Drawing.Size(48, 23);
		this.Label2.TabIndex = 44;
		this.Label2.Text = "Green:";
		this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.nudHue.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.nudHue.Location = new System.Drawing.Point(96, 232);
		this.nudHue.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.nudHue.Name = "nudHue";
		this.nudHue.Size = new System.Drawing.Size(48, 23);
		this.nudHue.TabIndex = 41;
		this.nudHue.TextChanged += new System.EventHandler(HandleTextChanged);
		this.nudHue.ValueChanged += new System.EventHandler(HandleHSVChange);
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
		base.ClientSize = new System.Drawing.Size(264, 349);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.Label3);
		base.Controls.Add(this.nudSaturation);
		base.Controls.Add(this.Label7);
		base.Controls.Add(this.nudBrightness);
		base.Controls.Add(this.nudRed);
		base.Controls.Add(this.pnlColor);
		base.Controls.Add(this.Label6);
		base.Controls.Add(this.Label1);
		base.Controls.Add(this.Label5);
		base.Controls.Add(this.pnlSelectedColor);
		base.Controls.Add(this.pnlBrightness);
		base.Controls.Add(this.nudBlue);
		base.Controls.Add(this.Label4);
		base.Controls.Add(this.nudGreen);
		base.Controls.Add(this.Label2);
		base.Controls.Add(this.nudHue);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "ColorWheelDialog";
		this.Text = "Select Color";
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(HandleMouse);
		base.Load += new System.EventHandler(ColorChooser1_Load);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(frmMain_MouseUp);
		base.Paint += new System.Windows.Forms.PaintEventHandler(ColorChooser1_Paint);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(HandleMouse);
		((System.ComponentModel.ISupportInitialize)this.nudSaturation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudBrightness).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudRed).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudBlue).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudGreen).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nudHue).EndInit();
		base.ResumeLayout(false);
	}

	private void ColorChooser1_Load(object sender, EventArgs e)
	{
		SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
		SetStyle(ControlStyles.UserPaint, value: true);
		SetStyle(ControlStyles.DoubleBuffer, value: true);
		pnlSelectedColor.Visible = false;
		pnlBrightness.Visible = false;
		pnlColor.Visible = false;
		Rectangle selectedColorRectangle = new Rectangle(pnlSelectedColor.Location, pnlSelectedColor.Size);
		Rectangle brightnessRectangle = new Rectangle(pnlBrightness.Location, pnlBrightness.Size);
		Rectangle colorRectangle = new Rectangle(pnlColor.Location, pnlColor.Size);
		myColorWheel = new ColorWheel(colorRectangle, brightnessRectangle, selectedColorRectangle);
		ColorWheel colorWheel = myColorWheel;
		colorWheel.ColorChanged = (ColorWheel.ColorChangedEventHandler)Delegate.Combine(colorWheel.ColorChanged, new ColorWheel.ColorChangedEventHandler(myColorWheel_ColorChanged));
		SetRGB(RGB);
		SetHSV(HSV);
	}

	private void HandleMouse(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			changeType = ChangeStyle.MouseMove;
			selectedPoint = new Point(e.X, e.Y);
			Invalidate();
		}
	}

	private void frmMain_MouseUp(object sender, MouseEventArgs e)
	{
		myColorWheel.SetMouseUp();
		changeType = ChangeStyle.None;
	}

	private void HandleRGBChange(object sender, EventArgs e)
	{
		if (!isInUpdate)
		{
			changeType = ChangeStyle.RGB;
			RGB = new ColorHandler.RGB((int)nudRed.Value, (int)nudGreen.Value, (int)nudBlue.Value);
			SetHSV(ColorHandler.RGBtoHSV(RGB));
			Invalidate();
		}
	}

	private void HandleHSVChange(object sender, EventArgs e)
	{
		if (!isInUpdate)
		{
			changeType = ChangeStyle.HSV;
			HSV = new ColorHandler.HSV((int)nudHue.Value, (int)nudSaturation.Value, (int)nudBrightness.Value);
			SetRGB(ColorHandler.HSVtoRGB(HSV));
			Invalidate();
		}
	}

	private void SetRGB(ColorHandler.RGB RGB)
	{
		isInUpdate = true;
		RefreshValue(nudRed, RGB.Red);
		RefreshValue(nudBlue, RGB.Blue);
		RefreshValue(nudGreen, RGB.Green);
		isInUpdate = false;
	}

	private void SetHSV(ColorHandler.HSV HSV)
	{
		isInUpdate = true;
		RefreshValue(nudHue, HSV.Hue);
		RefreshValue(nudSaturation, HSV.Saturation);
		RefreshValue(nudBrightness, HSV.value);
		isInUpdate = false;
	}

	private void HandleTextChanged(object sender, EventArgs e)
	{
		_ = ((NumericUpDown)sender).Value;
	}

	private void RefreshValue(NumericUpDown nud, int value)
	{
		if (nud.Value != (decimal)value)
		{
			nud.Value = value;
			nud.Refresh();
		}
	}

	private void myColorWheel_ColorChanged(object sender, ColorChangedEventArgs e)
	{
		SetRGB(e.RGB);
		SetHSV(e.HSV);
	}

	private void ColorChooser1_Paint(object sender, PaintEventArgs e)
	{
		switch (changeType)
		{
		case ChangeStyle.HSV:
			myColorWheel.Draw(e.Graphics, HSV);
			break;
		case ChangeStyle.MouseMove:
		case ChangeStyle.None:
			myColorWheel.Draw(e.Graphics, selectedPoint);
			break;
		case ChangeStyle.RGB:
			myColorWheel.Draw(e.Graphics, RGB);
			break;
		}
	}
}
