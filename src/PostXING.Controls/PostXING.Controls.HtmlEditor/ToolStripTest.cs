using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls.HtmlEditor;

public class ToolStripTest : UserControl
{
	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripColorButton toolStripColorButton1;

	public ToolStripTest()
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Controls.HtmlEditor.ToolStripTest));
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripColorButton1 = new PostXING.Controls.ToolStripColorButton();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripColorButton1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(599, 25);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripColorButton1.AutomaticText = "Automatic";
		this.toolStripColorButton1.Color = System.Drawing.Color.Empty;
		this.toolStripColorButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripColorButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripColorButton1.Image");
		this.toolStripColorButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripColorButton1.MoreColorsText = "More Colors...";
		this.toolStripColorButton1.Name = "toolStripColorButton1";
		this.toolStripColorButton1.PanelVisible = false;
		this.toolStripColorButton1.Size = new System.Drawing.Size(23, 22);
		this.toolStripColorButton1.Text = "toolStripColorButton1";
		this.toolStripColorButton1.UseCustomColorDialog = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.toolStrip1);
		base.Name = "ToolStripTest";
		base.Size = new System.Drawing.Size(599, 455);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
