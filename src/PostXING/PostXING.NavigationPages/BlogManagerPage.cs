using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class BlogManagerPage : ControlPanelPage
{
	private IContainer components;

	private XPListBox xpListBox1;

	private Button button1;

	private Button button2;

	private PostXING.Controls.Navigation.LinkLabel linkLabel1;

	private PostXING.Controls.Navigation.LinkLabel linkLabel2;

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
		this.xpListBox1 = new PostXING.Controls.XPListBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.linkLabel1 = new PostXING.Controls.Navigation.LinkLabel();
		this.linkLabel2 = new PostXING.Controls.Navigation.LinkLabel();
		base.SuspendLayout();
		this.xpListBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.xpListBox1.FormattingEnabled = true;
		this.xpListBox1.Location = new System.Drawing.Point(14, 7);
		this.xpListBox1.Name = "xpListBox1";
		this.xpListBox1.Size = new System.Drawing.Size(168, 108);
		this.xpListBox1.TabIndex = 0;
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.button1.Location = new System.Drawing.Point(14, 212);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 1;
		this.button1.Text = "Edit";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button2.Location = new System.Drawing.Point(95, 212);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 2;
		this.button2.Text = "Cancel";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
		this.linkLabel1.AutoSize = true;
		this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel1.Location = new System.Drawing.Point(11, 129);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(65, 13);
		this.linkLabel1.TabIndex = 3;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Edit Options";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.Click += new System.EventHandler(button1_Click);
		this.linkLabel2.Anchor = System.Windows.Forms.AnchorStyles.Left;
		this.linkLabel2.AutoSize = true;
		this.linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel2.Location = new System.Drawing.Point(11, 163);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(105, 13);
		this.linkLabel2.TabIndex = 4;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "Delete Selected Blog";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		base.Controls.Add(this.linkLabel2);
		base.Controls.Add(this.linkLabel1);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.xpListBox1);
		base.Name = "BlogManagerPage";
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public BlogManagerPage()
	{
		InitializeComponent();
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		LoadBlogList();
		base.OnPageEnter(e);
	}

	private void LoadBlogList()
	{
		xpListBox1.Items.Clear();
		for (int i = 0; i < AppManager.Blogs.Count; i++)
		{
			xpListBox1.Items.Add(AppManager.Blogs[i]);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		ApplyCancelButton();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		MessageBox.Show("Not implemented yet. Sorry :/");
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (xpListBox1.SelectedIndex < 0)
		{
			MessageBox.Show(this, "Erm, which blog were you trying to delete? If you don't select one first, I can't delete it.", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Question);
			return;
		}
		DialogResult dialogResult = MessageBox.Show(this, "Are you sure you want to remove all information about this blog from PostXING?", "Confirm Blog Deletion", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
		if (dialogResult == DialogResult.Yes)
		{
			Blog value = (Blog)xpListBox1.SelectedItem;
			AppManager.Blogs.Remove(value);
			AppManager.Save(AppManager.Blogs);
			LoadBlogList();
		}
	}
}
