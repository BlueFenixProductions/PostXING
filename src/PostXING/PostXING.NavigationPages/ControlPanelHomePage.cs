using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class ControlPanelHomePage : ControlPanelPage
{
	private PostXING.Controls.Navigation.LinkLabel linkLabel1;

	private PostXING.Controls.Navigation.LinkLabel linkLabel2;

	private PostXING.Controls.Navigation.LinkLabel linkLabel3;

	private PostXING.Controls.Navigation.LinkLabel linkLabel4;

	private ToolStripLabel toolStripLabel1;

	private HeaderStrip headerStrip1;

	private IContainer components;

	public ControlPanelHomePage()
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.ControlPanelHomePage));
		this.linkLabel1 = new PostXING.Controls.Navigation.LinkLabel();
		this.linkLabel2 = new PostXING.Controls.Navigation.LinkLabel();
		this.linkLabel3 = new PostXING.Controls.Navigation.LinkLabel();
		this.linkLabel4 = new PostXING.Controls.Navigation.LinkLabel();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.headerStrip1 = new PostXING.Controls.HeaderStrip();
		this.headerStrip1.SuspendLayout();
		base.SuspendLayout();
		this.linkLabel1.Image = (System.Drawing.Image)resources.GetObject("linkLabel1.Image");
		this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.Location = new System.Drawing.Point(32, 40);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(89, 23);
		this.linkLabel1.TabIndex = 0;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Recent Posts";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.linkLabel2.Image = (System.Drawing.Image)resources.GetObject("linkLabel2.Image");
		this.linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel2.Location = new System.Drawing.Point(32, 64);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(89, 23);
		this.linkLabel2.TabIndex = 1;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "Categories";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.linkLabel3.Image = (System.Drawing.Image)resources.GetObject("linkLabel3.Image");
		this.linkLabel3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel3.Location = new System.Drawing.Point(32, 88);
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.Size = new System.Drawing.Size(89, 23);
		this.linkLabel3.TabIndex = 2;
		this.linkLabel3.TabStop = true;
		this.linkLabel3.Text = "Cross Post";
		this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		this.linkLabel4.Image = (System.Drawing.Image)resources.GetObject("linkLabel4.Image");
		this.linkLabel4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel4.Location = new System.Drawing.Point(32, 111);
		this.linkLabel4.Name = "linkLabel4";
		this.linkLabel4.Size = new System.Drawing.Size(89, 23);
		this.linkLabel4.TabIndex = 3;
		this.linkLabel4.TabStop = true;
		this.linkLabel4.Text = "Upload a file";
		this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel4_LinkClicked);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(78, 16);
		this.toolStripLabel1.Text = "Common Tasks";
		this.headerStrip1.AutoSize = false;
		this.headerStrip1.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.headerStrip1.ForeColor = System.Drawing.Color.Black;
		this.headerStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.headerStrip1.HeaderStyle = PostXING.Controls.AreaHeaderStyle.Small;
		this.headerStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripLabel1 });
		this.headerStrip1.Location = new System.Drawing.Point(0, 0);
		this.headerStrip1.Name = "headerStrip1";
		this.headerStrip1.Size = new System.Drawing.Size(196, 19);
		this.headerStrip1.TabIndex = 4;
		this.headerStrip1.Text = "headerStrip1";
		base.Controls.Add(this.headerStrip1);
		base.Controls.Add(this.linkLabel4);
		base.Controls.Add(this.linkLabel3);
		base.Controls.Add(this.linkLabel2);
		base.Controls.Add(this.linkLabel1);
		base.Name = "ControlPanelHomePage";
		this.headerStrip1.ResumeLayout(false);
		this.headerStrip1.PerformLayout();
		base.ResumeLayout(false);
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		AppManager.ConcreteEditor.ControlPanelNavigate("history");
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		AppManager.ConcreteEditor.ControlPanelNavigate("categories");
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		AppManager.ConcreteEditor.ControlPanelNavigate("crosspost");
	}

	private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		AppManager.ConcreteEditor.ControlPanelNavigate("upload");
	}
}
