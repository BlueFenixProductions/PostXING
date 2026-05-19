using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class AutoSavePage : ControlPanelPage
{
	private BlogPost _currentPost;

	private IContainer components;

	private MozPane mozPane1;

	private ImageList imageList1;

	private GradientPanel gradientPanel1;

	private Label label1;

	private FlowLayoutPanel flowLayoutPanel1;

	private PostXING.Controls.Navigation.LinkLabel linkLabel1;

	public BlogPost CurrentPost
	{
		get
		{
			return _currentPost;
		}
		set
		{
			_currentPost = value;
		}
	}

	public AutoSavePage()
	{
		InitializeComponent();
		AppManager.PostSelected = (PostSelectedEventHandler)Delegate.Combine(AppManager.PostSelected, new PostSelectedEventHandler(PostSelected));
	}

	private void PostSelected(object sender, PostSelectedEventArgs e)
	{
		RefreshDisplay();
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		base.OnPageEnter(e);
		RefreshDisplay();
	}

	public void RefreshDisplay()
	{
		mozPane1.SuspendLayout();
		mozPane1.Controls.Clear();
		string[] files = Directory.GetFiles(Path.Combine(AppManager.GetUserPath(), "drafts"));
		if (files.Length > 0)
		{
			for (int i = 0; i < files.Length; i++)
			{
				BlogPost blogPost = AppManager.LoadExistingPost(files[i]);
				blogPost.AutoSaveFileName = files[i];
				MozItem mozItem = new MozItem();
				mozItem.Images.Normal = 0;
				mozItem.Images.Focus = 0;
				mozItem.Images.Selected = 0;
				mozItem.ItemStyle = MozItemStyle.TextAndPicture;
				mozItem.Text = (string.IsNullOrEmpty(blogPost.Title) ? "Untitled Post" : blogPost.Title);
				mozItem.Tag = blogPost;
				mozItem.Click += draftItem_Click;
				mozPane1.Controls.Add(mozItem);
			}
			mozPane1.BringToFront();
			mozPane1.ResumeLayout();
		}
		else
		{
			mozPane1.SendToBack();
		}
	}

	private void draftItem_Click(object sender, EventArgs e)
	{
		if (sender is MozItem mozItem)
		{
			CurrentPost = (BlogPost)mozItem.Tag;
			PostSelectedEventArgs e2 = new PostSelectedEventArgs(selectedForEditing: true, CurrentPost);
			AppManager.PostSelected(this, e2);
		}
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (CurrentPost == null)
		{
			AppManager.EditorForm.ResetMessage("You've got to select a draft to delete first.", preserveCurrentPost: true);
		}
		else if (File.Exists(CurrentPost.AutoSaveFileName))
		{
			File.Delete(CurrentPost.AutoSaveFileName);
			RefreshDisplay();
			AppManager.EditorForm.ResetMessage("Post '" + CurrentPost.Title + "' deleted from drafts folder.", preserveCurrentPost: false);
			CurrentPost = null;
		}
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.AutoSavePage));
		this.mozPane1 = new PostXING.Controls.Controls.MozPane();
		this.imageList1 = new System.Windows.Forms.ImageList(this.components);
		this.gradientPanel1 = new PostXING.Controls.GradientPanel();
		this.label1 = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.linkLabel1 = new PostXING.Controls.Navigation.LinkLabel();
		((System.ComponentModel.ISupportInitialize)this.mozPane1).BeginInit();
		this.gradientPanel1.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.mozPane1.BackColor = System.Drawing.Color.White;
		this.mozPane1.BorderColor = System.Drawing.Color.FromArgb(176, 176, 176);
		this.mozPane1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.ImageList = this.imageList1;
		this.mozPane1.ItemBorderStyles.Focus = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.ItemBorderStyles.Normal = System.Windows.Forms.ButtonBorderStyle.None;
		this.mozPane1.ItemBorderStyles.Selected = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.ItemColors.Background = System.Drawing.Color.White;
		this.mozPane1.ItemColors.Border = System.Drawing.Color.Black;
		this.mozPane1.ItemColors.Divider = System.Drawing.Color.FromArgb(176, 176, 176);
		this.mozPane1.ItemColors.FocusBackground = System.Drawing.Color.FromArgb(228, 228, 228);
		this.mozPane1.ItemColors.FocusBorder = System.Drawing.Color.FromArgb(202, 202, 202);
		this.mozPane1.ItemColors.FocusText = System.Drawing.Color.Black;
		this.mozPane1.ItemColors.SelectedBackground = System.Drawing.Color.FromArgb(202, 202, 202);
		this.mozPane1.ItemColors.SelectedBorder = System.Drawing.Color.FromArgb(202, 202, 202);
		this.mozPane1.ItemColors.SelectedText = System.Drawing.Color.Black;
		this.mozPane1.ItemColors.Text = System.Drawing.Color.Black;
		this.mozPane1.Location = new System.Drawing.Point(7, 7);
		this.mozPane1.MaxSelectedItems = 1;
		this.mozPane1.Name = "mozPane1";
		this.mozPane1.SelectButton = PostXING.Controls.Controls.MozSelectButton.Left;
		this.mozPane1.Size = new System.Drawing.Size(181, 395);
		this.mozPane1.Style = PostXING.Controls.Controls.MozPaneStyle.Vertical;
		this.mozPane1.TabIndex = 0;
		this.mozPane1.Theme = true;
		this.mozPane1.Toggle = false;
		this.imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
		this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList1.Images.SetKeyName(0, "Editor.ico");
		this.imageList1.Images.SetKeyName(1, "Delete.ico");
		this.gradientPanel1.BackColor = System.Drawing.SystemColors.Control;
		this.gradientPanel1.Controls.Add(this.label1);
		this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gradientPanel1.GradientColor = System.Drawing.SystemColors.ControlDark;
		this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
		this.gradientPanel1.Name = "gradientPanel1";
		this.gradientPanel1.Rotation = 45f;
		this.gradientPanel1.Size = new System.Drawing.Size(196, 439);
		this.gradientPanel1.TabIndex = 4;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.SystemColors.HighlightText;
		this.label1.Image = (System.Drawing.Image)resources.GetObject("label1.Image");
		this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label1.Location = new System.Drawing.Point(16, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(162, 122);
		this.label1.TabIndex = 0;
		this.label1.Text = "Couldn't find any Auto Saved drafts.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.flowLayoutPanel1.Controls.Add(this.mozPane1);
		this.flowLayoutPanel1.Controls.Add(this.linkLabel1);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
		this.flowLayoutPanel1.Size = new System.Drawing.Size(196, 439);
		this.flowLayoutPanel1.TabIndex = 5;
		this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.ImageIndex = 1;
		this.linkLabel1.ImageList = this.imageList1;
		this.linkLabel1.Location = new System.Drawing.Point(7, 405);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
		this.linkLabel1.Size = new System.Drawing.Size(126, 13);
		this.linkLabel1.TabIndex = 1;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Delete selected draft";
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		base.Controls.Add(this.flowLayoutPanel1);
		base.Controls.Add(this.gradientPanel1);
		base.Name = "AutoSavePage";
		((System.ComponentModel.ISupportInitialize)this.mozPane1).EndInit();
		this.gradientPanel1.ResumeLayout(false);
		this.flowLayoutPanel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
