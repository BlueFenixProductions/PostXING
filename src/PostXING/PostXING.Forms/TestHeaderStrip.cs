using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;

namespace PostXING.Forms;

public class TestHeaderStrip : UserControl
{
	private IContainer components;

	private HeaderStrip headerStrip1;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton3;

	public TestHeaderStrip()
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Forms.TestHeaderStrip));
		this.headerStrip1 = new PostXING.Controls.HeaderStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.headerStrip1.SuspendLayout();
		base.SuspendLayout();
		this.headerStrip1.AutoSize = false;
		this.headerStrip1.Font = new System.Drawing.Font("Tahoma", 10.25f);
		this.headerStrip1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.headerStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.headerStrip1.HeaderStyle = PostXING.Controls.AreaHeaderStyle.ControlPanel;
		this.headerStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton1, this.toolStripButton2, this.toolStripSeparator1, this.toolStripButton3 });
		this.headerStrip1.Location = new System.Drawing.Point(0, 0);
		this.headerStrip1.Name = "headerStrip1";
		this.headerStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
		this.headerStrip1.Size = new System.Drawing.Size(336, 23);
		this.headerStrip1.TabIndex = 0;
		this.headerStrip1.Text = "headerStrip1";
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(23, 20);
		this.toolStripButton1.Text = "toolStripButton1";
		this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(23, 20);
		this.toolStripButton2.Text = "toolStripButton2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
		this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(23, 20);
		this.toolStripButton3.Text = "toolStripButton3";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.headerStrip1);
		base.Name = "TestHeaderStrip";
		base.Size = new System.Drawing.Size(336, 150);
		this.headerStrip1.ResumeLayout(false);
		this.headerStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
