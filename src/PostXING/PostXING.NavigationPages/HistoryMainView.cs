using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;
using PostXING.Controls.HtmlEditor;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class HistoryMainView : MainViewPage
{
	private PreviewView previewView1;

	private HeaderStrip headerStrip1;

	private ToolStripButton toolStripButton1;

	private ToolStripLabel toolStripLabel1;

	private IContainer components;

	private string _previewTemplate;

	public string PreviewTemplate
	{
		get
		{
			return _previewTemplate;
		}
		set
		{
			_previewTemplate = value;
		}
	}

	public HistoryMainView()
	{
		InitializeComponent();
	}

	public void LoadHtml(string html)
	{
		if (string.IsNullOrEmpty(PreviewTemplate))
		{
			previewView1.PreviewEditor.LoadHtml(html);
		}
		else
		{
			previewView1.PreviewEditor.LoadHtml(string.Format(PreviewTemplate, html));
		}
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		previewView1.PreBar.Visible = false;
		PreviewTemplate = AppManager.CurrentBlog.Options.PreviewTemplate;
		base.OnPageEnter(e);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.HistoryMainView));
		this.previewView1 = new PostXING.Controls.HtmlEditor.PreviewView();
		this.headerStrip1 = new PostXING.Controls.HeaderStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.headerStrip1.SuspendLayout();
		base.SuspendLayout();
		this.previewView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.previewView1.Location = new System.Drawing.Point(0, 25);
		this.previewView1.Name = "previewView1";
		this.previewView1.Size = new System.Drawing.Size(600, 383);
		this.previewView1.TabIndex = 0;
		this.headerStrip1.AutoSize = false;
		this.headerStrip1.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Bold);
		this.headerStrip1.ForeColor = System.Drawing.Color.White;
		this.headerStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.headerStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton1, this.toolStripLabel1 });
		this.headerStrip1.Location = new System.Drawing.Point(0, 0);
		this.headerStrip1.Name = "headerStrip1";
		this.headerStrip1.Size = new System.Drawing.Size(600, 25);
		this.headerStrip1.TabIndex = 1;
		this.headerStrip1.Text = "headerStrip1";
		this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
		this.toolStripButton1.Text = "Return to Editing";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(111, 22);
		this.toolStripLabel1.Text = "Recent Posts";
		base.Controls.Add(this.previewView1);
		base.Controls.Add(this.headerStrip1);
		base.Name = "HistoryMainView";
		this.headerStrip1.ResumeLayout(false);
		this.headerStrip1.PerformLayout();
		base.ResumeLayout(false);
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		GoHome();
	}
}
