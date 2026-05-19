#define TRACE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genghis.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;

namespace PostXING.MetaBlogProvider;

public class BlogSelectionPage : Page, IOptionsPage
{
	private PostXING.Controls.Navigation.LinkLabel lnkFTP;

	private PostXING.Controls.Navigation.LinkLabel lnkMedia;

	private PostXING.Controls.Navigation.LinkLabel lnkPreview;

	private Label lblMessage;

	private PostXING.Controls.Navigation.LinkLabel lnkCredentials;

	private Container components;

	private static IConfigurationDialog _dialog;

	private CompletionCombo cbxAvailableBlogs;

	private Label label1;

	private XPTextBox xpTextBox1;

	private Label label2;

	private IBlogProvider _provider;

	public BlogSelectionPage()
	{
		InitializeComponent();
	}

	public BlogSelectionPage(IConfigurationDialog dialog, IBlogProvider provider)
		: this()
	{
		_dialog = dialog;
		_provider = provider;
	}

	private void _enableElements(bool enable)
	{
		CompletionCombo completionCombo = cbxAvailableBlogs;
		PostXING.Controls.Navigation.LinkLabel linkLabel = lnkFTP;
		PostXING.Controls.Navigation.LinkLabel linkLabel2 = lnkMedia;
		bool flag = (lnkPreview.Enabled = enable);
		bool flag3 = (linkLabel2.Enabled = flag);
		bool enabled = (linkLabel.Enabled = flag3);
		completionCombo.Enabled = enabled;
		lblMessage.Visible = !enable;
	}

	private void _showElements(bool enable)
	{
		PostXING.Controls.Navigation.LinkLabel linkLabel = lnkCredentials;
		PostXING.Controls.Navigation.LinkLabel linkLabel2 = lnkFTP;
		PostXING.Controls.Navigation.LinkLabel linkLabel3 = lnkMedia;
		bool flag = (lnkPreview.Visible = enable);
		bool flag3 = (linkLabel3.Visible = flag);
		bool visible = (linkLabel2.Visible = flag3);
		linkLabel.Visible = visible;
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		if (e.ViewState != null)
		{
			_dialog = (IConfigurationDialog)e.ViewState.ReadProperty("dialog");
			_provider = (IBlogProvider)e.ViewState.ReadProperty("provider");
		}
		try
		{
			_enableElements(enable: true);
			_showElements(_dialog.IsEditing);
			cbxAvailableBlogs.Items.Clear();
			Blog[] blogs = _provider.GetBlogs(_dialog.CurrentBlog.ServiceUrl, _dialog.CurrentBlog.Username, _dialog.CurrentBlog.Password, string.Empty);
			if (blogs != null && blogs.Length > 0)
			{
				cbxAvailableBlogs.Items.AddRange(blogs);
				if (_dialog.IsEditing)
				{
					xpTextBox1.Text = _dialog.CurrentBlog.BlogName;
					for (int i = 0; i < blogs.Length; i++)
					{
						if (blogs[i].BlogName == _dialog.CurrentBlog.BlogName)
						{
							cbxAvailableBlogs.SelectedIndex = i;
							break;
						}
						if (blogs[i].BlogID == _dialog.CurrentBlog.BlogID)
						{
							cbxAvailableBlogs.SelectedIndex = i;
							break;
						}
					}
				}
				else
				{
					cbxAvailableBlogs.SelectedIndex = 0;
				}
			}
			else
			{
				_enableElements(enable: false);
			}
		}
		catch (Exception ex)
		{
			string text = ex.Message + "::" + Environment.NewLine;
			text += ((ex.InnerException != null) ? ex.InnerException.Message : "");
			MessageBox.Show(text);
			Trace.WriteLine(ex.Message);
			_enableElements(enable: false);
		}
		base.OnPageEnter(e);
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		ApplySettings(_dialog.CurrentBlog);
		e.ViewState.WriteProperty("dialog", _dialog);
		e.ViewState.WriteProperty("provider", _provider);
		base.OnPageLeave(e);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.MetaBlogProvider.BlogSelectionPage));
		this.lnkFTP = new PostXING.Controls.Navigation.LinkLabel();
		this.lnkMedia = new PostXING.Controls.Navigation.LinkLabel();
		this.lnkPreview = new PostXING.Controls.Navigation.LinkLabel();
		this.lblMessage = new System.Windows.Forms.Label();
		this.lnkCredentials = new PostXING.Controls.Navigation.LinkLabel();
		this.cbxAvailableBlogs = new Genghis.Windows.Forms.CompletionCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.xpTextBox1 = new PostXING.Controls.XPTextBox();
		this.label2 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lnkFTP.Image = (System.Drawing.Image)resources.GetObject("lnkFTP.Image");
		this.lnkFTP.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkFTP.Location = new System.Drawing.Point(168, 176);
		this.lnkFTP.Name = "lnkFTP";
		this.lnkFTP.Size = new System.Drawing.Size(96, 23);
		this.lnkFTP.TabIndex = 1;
		this.lnkFTP.TabStop = true;
		this.lnkFTP.Text = "Configure FTP";
		this.lnkFTP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkFTP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkFTP_LinkClicked);
		this.lnkMedia.Image = (System.Drawing.Image)resources.GetObject("lnkMedia.Image");
		this.lnkMedia.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkMedia.Location = new System.Drawing.Point(158, 208);
		this.lnkMedia.Name = "lnkMedia";
		this.lnkMedia.Size = new System.Drawing.Size(106, 23);
		this.lnkMedia.TabIndex = 2;
		this.lnkMedia.TabStop = true;
		this.lnkMedia.Text = "Configure Media";
		this.lnkMedia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkMedia.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkMedia_LinkClicked);
		this.lnkPreview.Image = (System.Drawing.Image)resources.GetObject("lnkPreview.Image");
		this.lnkPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkPreview.Location = new System.Drawing.Point(150, 240);
		this.lnkPreview.Name = "lnkPreview";
		this.lnkPreview.Size = new System.Drawing.Size(114, 23);
		this.lnkPreview.TabIndex = 3;
		this.lnkPreview.TabStop = true;
		this.lnkPreview.Text = "Configure Preview";
		this.lnkPreview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkPreview.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkPreview_LinkClicked);
		this.lblMessage.Location = new System.Drawing.Point(16, 168);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(128, 64);
		this.lblMessage.TabIndex = 4;
		this.lblMessage.Text = "No Blogs were found at the url you specified. Please go back and try again.";
		this.lblMessage.Visible = false;
		this.lnkCredentials.Image = (System.Drawing.Image)resources.GetObject("lnkCredentials.Image");
		this.lnkCredentials.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkCredentials.Location = new System.Drawing.Point(8, 264);
		this.lnkCredentials.Name = "lnkCredentials";
		this.lnkCredentials.Size = new System.Drawing.Size(128, 23);
		this.lnkCredentials.TabIndex = 5;
		this.lnkCredentials.TabStop = true;
		this.lnkCredentials.Text = "Set Blog Credentials";
		this.lnkCredentials.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkCredentials.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkCredentials_LinkClicked);
		this.cbxAvailableBlogs.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cbxAvailableBlogs.LimitToList = true;
		this.cbxAvailableBlogs.Location = new System.Drawing.Point(48, 80);
		this.cbxAvailableBlogs.Name = "cbxAvailableBlogs";
		this.cbxAvailableBlogs.Size = new System.Drawing.Size(200, 21);
		this.cbxAvailableBlogs.TabIndex = 6;
		this.cbxAvailableBlogs.Text = "Select a Blog";
		this.cbxAvailableBlogs.SelectedIndexChanged += new System.EventHandler(cbxAvailableBlogs_SelectedIndexChanged);
		this.label1.Location = new System.Drawing.Point(16, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 23);
		this.label1.TabIndex = 7;
		this.label1.Text = "Available Blogs:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
		this.xpTextBox1.Enabled = false;
		this.xpTextBox1.Location = new System.Drawing.Point(124, 119);
		this.xpTextBox1.Name = "xpTextBox1";
		this.xpTextBox1.Size = new System.Drawing.Size(140, 21);
		this.xpTextBox1.TabIndex = 8;
		this.label2.Enabled = false;
		this.label2.Location = new System.Drawing.Point(8, 119);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(108, 21);
		this.label2.TabIndex = 9;
		this.label2.Text = "Custom Blog Name:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.Controls.Add(this.label2);
		base.Controls.Add(this.xpTextBox1);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.cbxAvailableBlogs);
		base.Controls.Add(this.lnkCredentials);
		base.Controls.Add(this.lblMessage);
		base.Controls.Add(this.lnkPreview);
		base.Controls.Add(this.lnkMedia);
		base.Controls.Add(this.lnkFTP);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "BlogSelectionPage";
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void lnkCredentials_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		base.Frame.Go(_provider.ConfigurationPages[0]);
	}

	private void lnkFTP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		_dialog.NavigateToFtpPage(sender, e);
	}

	private void lnkMedia_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		_dialog.NavigateToMediaPage(sender, e);
	}

	private void lnkPreview_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		_dialog.NavigateToPreviewPage(sender, e);
	}

	private void cbxAvailableBlogs_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbxAvailableBlogs.SelectedIndex > -1)
		{
			Blog currentBlog = (Blog)cbxAvailableBlogs.SelectedItem;
			ApplySettings(currentBlog);
			Label label = label2;
			bool enabled = (xpTextBox1.Enabled = true);
			label.Enabled = enabled;
		}
	}

	public void ApplySettings(Blog currentBlog)
	{
		_dialog.CurrentBlog.BlogID = currentBlog.BlogID;
		if (xpTextBox1.Text.Trim().Length > 0)
		{
			_dialog.CurrentBlog.BlogName = xpTextBox1.Text;
		}
		else
		{
			_dialog.CurrentBlog.BlogName = currentBlog.BlogName;
		}
		_dialog.CurrentBlog.WebAddress = currentBlog.WebAddress;
		_dialog.CurrentBlog.SupportsCategories = _provider.SupportsCategories;
		_dialog.CurrentBlog.ProviderName = _provider.ProviderName;
		_provider.IsConfigurationComplete = true;
	}
}
