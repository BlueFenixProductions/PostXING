using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls.HtmlEditor.Html;

namespace PostXING.Controls.HtmlEditor;

public class PreviewView : UserControl
{
	private IContainer components;

	private ToolStripContainer toolStripContainer1;

	private HtmlControl htmlControl1;

	private ToolStrip toolStrip1;

	public ToolStrip PreBar => toolStrip1;

	public HtmlControl PreviewEditor => htmlControl1;

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
		this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.htmlControl1 = new PostXING.Controls.HtmlEditor.Html.HtmlControl();
		this.toolStripContainer1.ContentPanel.SuspendLayout();
		this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
		this.toolStripContainer1.SuspendLayout();
		base.SuspendLayout();
		this.toolStripContainer1.ContentPanel.Controls.Add(this.htmlControl1);
		this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(631, 464);
		this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
		this.toolStripContainer1.Name = "toolStripContainer1";
		this.toolStripContainer1.Size = new System.Drawing.Size(631, 489);
		this.toolStripContainer1.TabIndex = 0;
		this.toolStripContainer1.Text = "toolStripContainer1";
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.Location = new System.Drawing.Point(3, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(111, 25);
		this.toolStrip1.TabIndex = 0;
		this.htmlControl1.AbsolutePositioningEnabled = false;
		this.htmlControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.htmlControl1.IsDesignMode = false;
		this.htmlControl1.IsDirty = false;
		this.htmlControl1.Location = new System.Drawing.Point(0, 0);
		this.htmlControl1.MultipleSelectionEnabled = false;
		this.htmlControl1.Name = "htmlControl1";
		this.htmlControl1.ScriptEnabled = false;
		this.htmlControl1.ScriptObject = null;
		this.htmlControl1.Size = new System.Drawing.Size(631, 464);
		this.htmlControl1.TabIndex = 0;
		this.htmlControl1.Text = "htmlControl1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.toolStripContainer1);
		base.Name = "PreviewView";
		base.Size = new System.Drawing.Size(631, 489);
		this.toolStripContainer1.ContentPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.PerformLayout();
		this.toolStripContainer1.ResumeLayout(false);
		this.toolStripContainer1.PerformLayout();
		base.ResumeLayout(false);
	}

	public PreviewView()
	{
		InitializeComponent();
	}
}
