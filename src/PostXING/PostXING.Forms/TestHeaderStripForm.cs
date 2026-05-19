using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Forms;

public class TestHeaderStripForm : Form
{
	private IContainer components;

	private TestHeaderStrip testHeaderStrip1;

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
		this.testHeaderStrip1 = new PostXING.Forms.TestHeaderStrip();
		base.SuspendLayout();
		this.testHeaderStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.testHeaderStrip1.Location = new System.Drawing.Point(0, 0);
		this.testHeaderStrip1.Name = "testHeaderStrip1";
		this.testHeaderStrip1.Size = new System.Drawing.Size(292, 265);
		this.testHeaderStrip1.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(292, 265);
		base.Controls.Add(this.testHeaderStrip1);
		base.Name = "TestHeaderStripForm";
		this.Text = "TestHeaderStripForm";
		base.ResumeLayout(false);
	}

	public TestHeaderStripForm()
	{
		InitializeComponent();
	}
}
