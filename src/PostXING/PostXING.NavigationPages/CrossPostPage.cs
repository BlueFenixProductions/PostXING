using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Extensibility;

namespace PostXING.NavigationPages;

public class CrossPostPage : ControlPanelPage
{
	private XPCheckedListBox chkConfiguredBlogs;

	private CheckBox chkPublish;

	private Button btnPost;

	private Button btnCancel;

	private Label label1;

	private PictureBox btnRefresh;

	private IContainer components;

	public CrossPostPage()
	{
		InitializeComponent();
	}

	protected override void OnLoad(EventArgs e)
	{
		LoadBlogs();
		base.OnLoad(e);
	}

	private void LoadBlogs()
	{
		BlogCollection blogCollection = new BlogCollection(AppManager.Blogs);
		chkConfiguredBlogs.Items.Clear();
		IBlogEnumerator enumerator = blogCollection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Blog current = enumerator.Current;
				chkConfiguredBlogs.Items.Add(current, current.AutoSelectForCrossPosting);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.CrossPostPage));
		this.chkConfiguredBlogs = new PostXING.Controls.XPCheckedListBox();
		this.chkPublish = new System.Windows.Forms.CheckBox();
		this.btnPost = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.btnRefresh = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).BeginInit();
		base.SuspendLayout();
		this.chkConfiguredBlogs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chkConfiguredBlogs.CheckOnClick = true;
		this.chkConfiguredBlogs.Location = new System.Drawing.Point(8, 8);
		this.chkConfiguredBlogs.Name = "chkConfiguredBlogs";
		this.chkConfiguredBlogs.Size = new System.Drawing.Size(176, 324);
		this.chkConfiguredBlogs.TabIndex = 0;
		this.chkPublish.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.chkPublish.Checked = true;
		this.chkPublish.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPublish.Location = new System.Drawing.Point(8, 362);
		this.chkPublish.Name = "chkPublish";
		this.chkPublish.Size = new System.Drawing.Size(83, 24);
		this.chkPublish.TabIndex = 3;
		this.chkPublish.Text = "Publish";
		this.btnPost.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnPost.Location = new System.Drawing.Point(16, 392);
		this.btnPost.Name = "btnPost";
		this.btnPost.Size = new System.Drawing.Size(75, 23);
		this.btnPost.TabIndex = 4;
		this.btnPost.Text = "Post";
		this.btnPost.Click += new System.EventHandler(btnPost_Click);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnCancel.Location = new System.Drawing.Point(104, 392);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 5;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label1.Image = (System.Drawing.Image)resources.GetObject("label1.Image");
		this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
		this.label1.Location = new System.Drawing.Point(85, 348);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(99, 41);
		this.label1.TabIndex = 6;
		this.label1.Text = "Hitting Post will create a new post.";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.Location = new System.Drawing.Point(8, 338);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(16, 16);
		this.btnRefresh.TabIndex = 7;
		this.btnRefresh.TabStop = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.Controls.Add(this.btnRefresh);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnPost);
		base.Controls.Add(this.chkPublish);
		base.Controls.Add(this.chkConfiguredBlogs);
		base.Name = "CrossPostPage";
		((System.ComponentModel.ISupportInitialize)this.btnRefresh).EndInit();
		base.ResumeLayout(false);
	}

	private void btnPost_Click(object sender, EventArgs e)
	{
		bool flag = chkPublish.Checked;
		string text = (flag ? "Publishing " : "Posting ");
		BlogPost currentPost = AppManager.EditorForm.CurrentPost;
		currentPost.Publish = flag;
		foreach (Blog checkedItem in chkConfiguredBlogs.CheckedItems)
		{
			AppManager.EditorForm.StartProgress(text + "message '{0}'", currentPost.Title);
			AsyncCallback callback = AddPostCallback;
			IBlogProvider blogProvider = AppManager.AvailableProviders[checkedItem.ProviderName];
			blogProvider.BeginCreatePost(checkedItem, currentPost, callback, checkedItem);
		}
		btnCancel.Focus();
	}

	private void AddPostCallback(IAsyncResult asr)
	{
		if (!(asr.AsyncState is Blog blog))
		{
			return;
		}
		IBlogProvider blogProvider = AppManager.AvailableProviders[blog.ProviderName];
		string text;
		try
		{
			text = blogProvider.EndCreatePost(asr);
		}
		catch
		{
			text = "-1";
		}
		AppManager.EditorForm.StopProgress();
		if (text != null && text != "-1")
		{
			AppManager.EditorForm.ResetMessage("Post added with new ID: " + text, preserveCurrentPost: true);
			if (AppManager.CurrentBlog == blog)
			{
				AppManager.EditorForm.CurrentPost.PostID = text;
				AppManager.EditorForm.IsEditing = true;
			}
		}
		else
		{
			MessageBox.Show("The post could not be added.", "Error adding post.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		LoadBlogs();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		ApplyCancelButton();
	}
}
