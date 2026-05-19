using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class HistoryPage : ControlPanelPage
{
	private XPListView lvwRecentPosts;

	private ColumnHeader Title;

	private Button btnView;

	private Button btnCancel;

	private System.ComponentModel.BackgroundWorker backgroundWorker1;

	private Label lblMessage;

	private IContainer components;

	private BlogPost _currentPost;

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

	public HistoryPage()
	{
		InitializeComponent();
	}

	public void EditPost()
	{
		if (lvwRecentPosts.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvwRecentPosts.SelectedItems[0];
			CurrentPost = (BlogPost)listViewItem.Tag;
			PostSelectedSend(selectedForEditing: true, CurrentPost);
		}
		else
		{
			MessageBox.Show("You must select a post to edit first.");
		}
	}

	public void RefreshPosts()
	{
		lblMessage.BringToFront();
		backgroundWorker1.RunWorkerAsync();
	}

	private BlogPost[] GetRecentPosts()
	{
		if (AppManager.CurrentBlog == null || AppManager.CurrentProvider == null)
		{
			return null;
		}
		return AppManager.CurrentProvider.GetRecentPosts(AppManager.CurrentBlog, AppManager.CurrentBlog.ItemCount);
	}

	private void PostSelectedSend(bool selectedForEditing, BlogPost selectedPost)
	{
		PostSelectedEventArgs e = new PostSelectedEventArgs(selectedForEditing, selectedPost);
		if (AppManager.PostSelected != null)
		{
			AppManager.PostSelected(this, e);
		}
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		lblMessage.BringToFront();
		backgroundWorker1.RunWorkerAsync();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.HistoryPage));
		this.lvwRecentPosts = new PostXING.Controls.XPListView();
		this.Title = new System.Windows.Forms.ColumnHeader();
		this.btnView = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.lblMessage = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lvwRecentPosts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lvwRecentPosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.Title });
		this.lvwRecentPosts.FullRowSelect = true;
		this.lvwRecentPosts.GridLines = true;
		this.lvwRecentPosts.Location = new System.Drawing.Point(8, 8);
		this.lvwRecentPosts.MultiSelect = false;
		this.lvwRecentPosts.Name = "lvwRecentPosts";
		this.lvwRecentPosts.Size = new System.Drawing.Size(176, 376);
		this.lvwRecentPosts.TabIndex = 0;
		this.lvwRecentPosts.UseCompatibleStateImageBehavior = false;
		this.lvwRecentPosts.View = System.Windows.Forms.View.Details;
		this.lvwRecentPosts.DoubleClick += new System.EventHandler(lvwRecentPosts_DoubleClick);
		this.lvwRecentPosts.Resize += new System.EventHandler(lvwRecentPosts_Resize);
		this.lvwRecentPosts.SelectedIndexChanged += new System.EventHandler(lvwRecentPosts_SelectedIndexChanged);
		this.Title.Text = "Title";
		this.Title.Width = 172;
		this.btnView.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnView.Location = new System.Drawing.Point(16, 392);
		this.btnView.Name = "btnView";
		this.btnView.Size = new System.Drawing.Size(75, 23);
		this.btnView.TabIndex = 1;
		this.btnView.Text = "View";
		this.btnView.Click += new System.EventHandler(btnView_Click);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnCancel.Location = new System.Drawing.Point(104, 392);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 2;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		this.lblMessage.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblMessage.Image = (System.Drawing.Image)resources.GetObject("lblMessage.Image");
		this.lblMessage.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lblMessage.Location = new System.Drawing.Point(50, 179);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(100, 48);
		this.lblMessage.TabIndex = 3;
		this.lblMessage.Text = "Retrieving Posts...";
		this.lblMessage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnView);
		base.Controls.Add(this.lblMessage);
		base.Controls.Add(this.lvwRecentPosts);
		base.Name = "HistoryPage";
		this.Text = "Recent Posts";
		base.ResumeLayout(false);
	}

	private void lvwRecentPosts_Resize(object sender, EventArgs e)
	{
		Title.Width = lvwRecentPosts.Width - 4;
	}

	private void lvwRecentPosts_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lvwRecentPosts.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvwRecentPosts.SelectedItems[0];
			CurrentPost = (BlogPost)listViewItem.Tag;
			PostSelectedSend(selectedForEditing: false, CurrentPost);
		}
	}

	private void lvwRecentPosts_DoubleClick(object sender, EventArgs e)
	{
		EditPost();
	}

	private void btnView_Click(object sender, EventArgs e)
	{
		AppManager.EditorForm.ShowHistory();
		btnCancel.Focus();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		ApplyCancelButton();
	}

	private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
	{
		BlogPost[] array = (BlogPost[])(e.Result = GetRecentPosts());
		e.Cancel = array == null;
	}

	private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
	{
		BlogPost[] array = (BlogPost[])e.Result;
		if (array != null)
		{
			lblMessage.SendToBack();
			lvwRecentPosts.Items.Clear();
			BlogPost[] array2 = array;
			foreach (BlogPost blogPost in array2)
			{
				ListViewItem listViewItem = new ListViewItem(blogPost.Title ?? blogPost.Body.Substring(0, 25));
				listViewItem.Tag = blogPost;
				lvwRecentPosts.Items.Add(listViewItem);
			}
			if (lvwRecentPosts.Items.Count > 0)
			{
				lvwRecentPosts.Items[0].Selected = true;
			}
		}
	}
}
