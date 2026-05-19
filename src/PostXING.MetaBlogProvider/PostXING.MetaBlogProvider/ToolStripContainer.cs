using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Extensibility;

namespace PostXING.MetaBlogProvider;

public class ToolStripContainer : UserControl
{
	private IBlogProvider _provider;

	private IEditorForm _editor;

	private ToolStripMenuItem[] _menuItems;

	private ToolStripButton[] _buttonBarItems;

	private ToolStripButton[] _previewBarItems;

	private ToolStripButton[] _deleteButtons;

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btniPost;

	private ToolStripButton btniPublish;

	private ToolStripButton btniDelete;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem fileToolStripMenuItem;

	private ToolStripMenuItem mbiPublish;

	private ToolStripMenuItem mbiPost;

	private ToolStripMenuItem mbiDelete;

	private ToolStrip toolStrip2;

	private ToolStripButton pbtnPost;

	private ToolStripButton pbtnPublish;

	private ToolStripButton pbtnDelete;

	public ToolStripMenuItem[] MenuItems
	{
		get
		{
			if (_menuItems == null)
			{
				_menuItems = new ToolStripMenuItem[3] { mbiPost, mbiPublish, mbiDelete };
			}
			return _menuItems;
		}
	}

	public ToolStripButton[] ButtonBarItems
	{
		get
		{
			if (_buttonBarItems == null)
			{
				_buttonBarItems = new ToolStripButton[3] { btniPost, btniPublish, btniDelete };
			}
			return _buttonBarItems;
		}
	}

	public ToolStripButton[] PreviewBarItems
	{
		get
		{
			if (_previewBarItems == null)
			{
				_previewBarItems = new ToolStripButton[3] { pbtnPost, pbtnPublish, pbtnDelete };
			}
			return _previewBarItems;
		}
	}

	public ToolStripButton[] DeleteButtons
	{
		get
		{
			if (_deleteButtons == null)
			{
				_deleteButtons = new ToolStripButton[2] { btniDelete, pbtnDelete };
			}
			return _deleteButtons;
		}
	}

	public ToolStripContainer()
	{
		InitializeComponent();
	}

	public ToolStripContainer(IBlogProvider provider, IEditorForm editor)
		: this()
	{
		_provider = provider;
		_editor = editor;
	}

	private void AddPost()
	{
		AsyncCallback callback = AddPostCallback;
		_provider.BeginCreatePost(_editor.CurrentBlog, _editor.CurrentPost, callback, null);
	}

	private void AddPostCallback(IAsyncResult asr)
	{
		string postID;
		try
		{
			postID = _provider.EndCreatePost(asr);
		}
		catch
		{
			postID = "-1";
		}
		FinishAddNewPost(postID);
	}

	private void FinishAddNewPost(string postID)
	{
		_editor.StopProgress();
		if (postID != null && postID != "-1")
		{
			_editor.ResetMessage("Post added with new ID: " + postID, preserveCurrentPost: true);
			_editor.CurrentPost.PostID = postID;
			_editor.IsEditing = true;
		}
		else
		{
			MessageBox.Show("The post could not be added.", "Error adding post.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void EditPost()
	{
		AsyncCallback callback = EditPostCallback;
		_provider.BeginUpdatePost(_editor.CurrentBlog, _editor.CurrentPost, callback, null);
	}

	private void EditPostCallback(IAsyncResult asr)
	{
		object successful;
		try
		{
			successful = _provider.EndUpdatePost(asr);
		}
		catch
		{
			successful = false;
		}
		FinishEditPost(successful);
	}

	private void FinishEditPost(object successful)
	{
		_editor.StopProgress();
		if ((bool)successful)
		{
			_editor.ResetMessage("Message '" + _editor.CurrentPost.Title + "' successfully updated.", preserveCurrentPost: true);
		}
		else
		{
			MessageBox.Show("Message '" + _editor.CurrentPost.Title + "' could not be updated.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeletePostCallback(IAsyncResult asr)
	{
		object obj;
		try
		{
			obj = _provider.EndDeletePost(asr);
		}
		catch
		{
			obj = false;
		}
		FinishDeletePost(obj);
	}

	private void FinishDeletePost(object obj)
	{
		_editor.StopProgress();
		if ((bool)obj)
		{
			_editor.ResetMessage("Message '" + _editor.CurrentPost.Title + "' successfully deleted.");
		}
		else
		{
			MessageBox.Show("Message '" + _editor.CurrentPost.Title + "' could not be deleted.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PostOrPublish()
	{
		if (_editor.IsEditing)
		{
			EditPost();
		}
		else
		{
			AddPost();
		}
	}

	private void Post_Click(object sender, EventArgs e)
	{
		_editor.CurrentPost.Publish = false;
		_editor.StartProgress("Posting message '{0}'", _editor.CurrentPost.Title);
		PostOrPublish();
	}

	private void Publish_Click(object sender, EventArgs e)
	{
		_editor.CurrentPost.Publish = true;
		_editor.StartProgress("Publishing message '{0}'", _editor.CurrentPost.Title);
		PostOrPublish();
	}

	private void Delete_Click(object sender, EventArgs e)
	{
		DialogResult dialogResult = MessageBox.Show(this, "This will permanantly delete this message. Are you sure?", "Delete Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
		if (dialogResult == DialogResult.Yes)
		{
			_editor.StartProgress("Deleting message '{0}'", _editor.CurrentPost.Title);
			AsyncCallback callback = DeletePostCallback;
			_provider.BeginDeletePost(_editor.CurrentBlog, _editor.CurrentPost.PostID, callback, null);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.MetaBlogProvider.ToolStripContainer));
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btniPost = new System.Windows.Forms.ToolStripButton();
		this.btniPublish = new System.Windows.Forms.ToolStripButton();
		this.btniDelete = new System.Windows.Forms.ToolStripButton();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.pbtnPost = new System.Windows.Forms.ToolStripButton();
		this.pbtnPublish = new System.Windows.Forms.ToolStripButton();
		this.pbtnDelete = new System.Windows.Forms.ToolStripButton();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.mbiPublish = new System.Windows.Forms.ToolStripMenuItem();
		this.mbiPost = new System.Windows.Forms.ToolStripMenuItem();
		this.mbiDelete = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStrip1.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btniPost, this.btniPublish, this.btniDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 24);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(281, 25);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btniPost.Image = (System.Drawing.Image)resources.GetObject("btniPost.Image");
		this.btniPost.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btniPost.Name = "btniPost";
		this.btniPost.Size = new System.Drawing.Size(50, 22);
		this.btniPost.Text = "Post";
		this.btniPost.Click += new System.EventHandler(Post_Click);
		this.btniPublish.Image = (System.Drawing.Image)resources.GetObject("btniPublish.Image");
		this.btniPublish.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btniPublish.Name = "btniPublish";
		this.btniPublish.Size = new System.Drawing.Size(98, 22);
		this.btniPublish.Text = "Post && Publish";
		this.btniPublish.Click += new System.EventHandler(Publish_Click);
		this.btniDelete.Image = (System.Drawing.Image)resources.GetObject("btniDelete.Image");
		this.btniDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btniDelete.Name = "btniDelete";
		this.btniDelete.Size = new System.Drawing.Size(60, 22);
		this.btniDelete.Text = "Delete";
		this.btniDelete.Click += new System.EventHandler(Delete_Click);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.fileToolStripMenuItem });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(281, 24);
		this.menuStrip1.TabIndex = 1;
		this.menuStrip1.Text = "menuStrip1";
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.pbtnPost, this.pbtnPublish, this.pbtnDelete });
		this.toolStrip2.Location = new System.Drawing.Point(0, 49);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(281, 25);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.pbtnPost.Image = (System.Drawing.Image)resources.GetObject("pbtnPost.Image");
		this.pbtnPost.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.pbtnPost.Name = "pbtnPost";
		this.pbtnPost.Size = new System.Drawing.Size(50, 22);
		this.pbtnPost.Text = "Post";
		this.pbtnPost.Click += new System.EventHandler(Post_Click);
		this.pbtnPublish.Image = (System.Drawing.Image)resources.GetObject("pbtnPublish.Image");
		this.pbtnPublish.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.pbtnPublish.Name = "pbtnPublish";
		this.pbtnPublish.Size = new System.Drawing.Size(98, 22);
		this.pbtnPublish.Text = "Post && Publish";
		this.pbtnPublish.Click += new System.EventHandler(Publish_Click);
		this.pbtnDelete.Image = (System.Drawing.Image)resources.GetObject("pbtnDelete.Image");
		this.pbtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.pbtnDelete.Name = "pbtnDelete";
		this.pbtnDelete.Size = new System.Drawing.Size(60, 22);
		this.pbtnDelete.Text = "Delete";
		this.pbtnDelete.Click += new System.EventHandler(Delete_Click);
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.mbiPost, this.mbiPublish, this.mbiDelete });
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
		this.fileToolStripMenuItem.Text = "File";
		this.mbiPublish.Image = (System.Drawing.Image)resources.GetObject("mbiPublish.Image");
		this.mbiPublish.Name = "mbiPublish";
		this.mbiPublish.Size = new System.Drawing.Size(152, 22);
		this.mbiPublish.Text = "Post && Publish";
		this.mbiPublish.Click += new System.EventHandler(Publish_Click);
		this.mbiPost.Image = (System.Drawing.Image)resources.GetObject("mbiPost.Image");
		this.mbiPost.Name = "mbiPost";
		this.mbiPost.Size = new System.Drawing.Size(152, 22);
		this.mbiPost.Text = "Post";
		this.mbiPost.Click += new System.EventHandler(Post_Click);
		this.mbiDelete.Image = (System.Drawing.Image)resources.GetObject("mbiDelete.Image");
		this.mbiDelete.Name = "mbiDelete";
		this.mbiDelete.Size = new System.Drawing.Size(152, 22);
		this.mbiDelete.Text = "Delete";
		this.mbiDelete.Click += new System.EventHandler(Delete_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.toolStrip2);
		base.Controls.Add(this.toolStrip1);
		base.Controls.Add(this.menuStrip1);
		base.Name = "ToolStripContainer";
		base.Size = new System.Drawing.Size(281, 92);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
