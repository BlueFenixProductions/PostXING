using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;
using PostXING.NavigationPages;

namespace PostXING.Dialogs;

public class NewUserDialog : Form, IConfigurationDialog
{
	private IContainer components;

	private GradientPanel gradientPanel1;

	private GradientPanel gradientPanel2;

	private ScrollingFrame scrollingFrame1;

	private Button btnFinish;

	private Button btnCancel;

	private Button btnBack;

	private Button btnNext;

	private ProxyOptionsPage proxyOptionsPage1;

	private FTPOptionsPage ftpOptionsPage1;

	private MediaOptionsPage mediaOptionsPage1;

	private PreviewOptionsPage previewOptionsPage1;

	private WelcomeOptionsPage welcomeOptionsPage1;

	private GeneralOptionsPage generalOptionsPage1;

	private PictureBox pictureBox1;

	private Label lblCurrentOperation;

	private bool _isEditing;

	private Blog _currentBlog = new Blog();

	private object _currentProvider;

	private Page[] _pages;

	private int _pageIndex;

	public bool IsEditing
	{
		get
		{
			return _isEditing;
		}
		set
		{
			_isEditing = value;
		}
	}

	public Blog CurrentBlog
	{
		get
		{
			return _currentBlog;
		}
		set
		{
			_currentBlog = value;
		}
	}

	public Blog OriginalBlog
	{
		get
		{
			return AppManager.CurrentBlog;
		}
		set
		{
			AppManager.CurrentBlog = value;
		}
	}

	public object CurrentProvider
	{
		get
		{
			return _currentProvider;
		}
		set
		{
			_currentProvider = value;
			btnNext.Enabled = true;
			ConfigurationPages = ((IBlogProvider)value).ConfigurationPages;
		}
	}

	public Page[] ConfigurationPages
	{
		get
		{
			return _pages;
		}
		set
		{
			_pages = value;
			foreach (Page page in value)
			{
				page.Tag = "config";
			}
		}
	}

	public int ConfigurationPageIndex
	{
		get
		{
			return _pageIndex;
		}
		set
		{
			_pageIndex = value;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Dialogs.NewUserDialog));
		PostXING.Components.PostXINGPreferences postXINGPreferences = new PostXING.Components.PostXINGPreferences();
		PostXING.Components.ProxyInfo proxyInfo = new PostXING.Components.ProxyInfo();
		this.scrollingFrame1 = new PostXING.Controls.Navigation.ScrollingFrame();
		this.gradientPanel2 = new PostXING.Controls.GradientPanel();
		this.btnBack = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.btnFinish = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.gradientPanel1 = new PostXING.Controls.GradientPanel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.welcomeOptionsPage1 = new PostXING.NavigationPages.WelcomeOptionsPage();
		this.ftpOptionsPage1 = new PostXING.NavigationPages.FTPOptionsPage();
		this.mediaOptionsPage1 = new PostXING.NavigationPages.MediaOptionsPage();
		this.previewOptionsPage1 = new PostXING.NavigationPages.PreviewOptionsPage();
		this.proxyOptionsPage1 = new PostXING.NavigationPages.ProxyOptionsPage();
		this.generalOptionsPage1 = new PostXING.NavigationPages.GeneralOptionsPage();
		this.lblCurrentOperation = new System.Windows.Forms.Label();
		this.gradientPanel2.SuspendLayout();
		this.gradientPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.scrollingFrame1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.scrollingFrame1.BackColor = System.Drawing.SystemColors.Control;
		this.scrollingFrame1.Location = new System.Drawing.Point(0, 98);
		this.scrollingFrame1.Name = "scrollingFrame1";
		this.scrollingFrame1.Size = new System.Drawing.Size(481, 344);
		this.scrollingFrame1.TabIndex = 2;
		this.gradientPanel2.BackColor = System.Drawing.Color.FromArgb(215, 215, 229);
		this.gradientPanel2.Controls.Add(this.btnBack);
		this.gradientPanel2.Controls.Add(this.btnNext);
		this.gradientPanel2.Controls.Add(this.btnFinish);
		this.gradientPanel2.Controls.Add(this.btnCancel);
		this.gradientPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.gradientPanel2.GradientColor = System.Drawing.Color.FromArgb(243, 243, 247);
		this.gradientPanel2.Location = new System.Drawing.Point(0, 442);
		this.gradientPanel2.Name = "gradientPanel2";
		this.gradientPanel2.Rotation = 0f;
		this.gradientPanel2.Size = new System.Drawing.Size(481, 49);
		this.gradientPanel2.TabIndex = 1;
		this.btnBack.Enabled = false;
		this.btnBack.Image = (System.Drawing.Image)resources.GetObject("btnBack.Image");
		this.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnBack.Location = new System.Drawing.Point(113, 14);
		this.btnBack.Name = "btnBack";
		this.btnBack.Size = new System.Drawing.Size(75, 24);
		this.btnBack.TabIndex = 3;
		this.btnBack.Text = "Back";
		this.btnBack.UseVisualStyleBackColor = true;
		this.btnBack.Click += new System.EventHandler(btnBack_Click);
		this.btnNext.Enabled = false;
		this.btnNext.Image = (System.Drawing.Image)resources.GetObject("btnNext.Image");
		this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnNext.Location = new System.Drawing.Point(194, 13);
		this.btnNext.Name = "btnNext";
		this.btnNext.Size = new System.Drawing.Size(75, 24);
		this.btnNext.TabIndex = 2;
		this.btnNext.Text = "Next";
		this.btnNext.UseVisualStyleBackColor = true;
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.btnFinish.Enabled = false;
		this.btnFinish.Location = new System.Drawing.Point(286, 14);
		this.btnFinish.Name = "btnFinish";
		this.btnFinish.Size = new System.Drawing.Size(75, 23);
		this.btnFinish.TabIndex = 1;
		this.btnFinish.Text = "Finish";
		this.btnFinish.UseVisualStyleBackColor = true;
		this.btnFinish.Click += new System.EventHandler(button2_Click);
		this.btnCancel.Location = new System.Drawing.Point(394, 14);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 0;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.gradientPanel1.BackColor = System.Drawing.Color.FromArgb(215, 215, 229);
		this.gradientPanel1.Controls.Add(this.lblCurrentOperation);
		this.gradientPanel1.Controls.Add(this.pictureBox1);
		this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.gradientPanel1.GradientColor = System.Drawing.Color.FromArgb(243, 243, 247);
		this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
		this.gradientPanel1.Name = "gradientPanel1";
		this.gradientPanel1.Rotation = 0f;
		this.gradientPanel1.Size = new System.Drawing.Size(481, 99);
		this.gradientPanel1.TabIndex = 0;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(12, 24);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(50, 42);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.welcomeOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.welcomeOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.welcomeOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.welcomeOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.welcomeOptionsPage1.Location = new System.Drawing.Point(0, 0);
		this.welcomeOptionsPage1.Name = "welcomeOptionsPage1";
		this.welcomeOptionsPage1.ParentDialog = null;
		this.welcomeOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.welcomeOptionsPage1.Size = new System.Drawing.Size(481, 491);
		this.welcomeOptionsPage1.TabIndex = 7;
		this.welcomeOptionsPage1.Tag = "welcome";
		this.welcomeOptionsPage1.Text = "welcomeOptionsPage1";
		this.ftpOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.ftpOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ftpOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ftpOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.ftpOptionsPage1.Location = new System.Drawing.Point(0, 0);
		this.ftpOptionsPage1.Name = "ftpOptionsPage1";
		this.ftpOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.ftpOptionsPage1.Size = new System.Drawing.Size(481, 491);
		this.ftpOptionsPage1.TabIndex = 4;
		this.ftpOptionsPage1.Tag = "ftp";
		this.ftpOptionsPage1.Text = "ftpOptionsPage1";
		this.mediaOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.mediaOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.mediaOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mediaOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.mediaOptionsPage1.Location = new System.Drawing.Point(0, 0);
		this.mediaOptionsPage1.Name = "mediaOptionsPage1";
		this.mediaOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.mediaOptionsPage1.Size = new System.Drawing.Size(481, 491);
		this.mediaOptionsPage1.TabIndex = 5;
		this.mediaOptionsPage1.Tag = "media";
		this.mediaOptionsPage1.Text = "mediaOptionsPage1";
		this.previewOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.previewOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.previewOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.previewOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.previewOptionsPage1.Location = new System.Drawing.Point(0, 0);
		this.previewOptionsPage1.Name = "previewOptionsPage1";
		this.previewOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.previewOptionsPage1.Size = new System.Drawing.Size(481, 491);
		this.previewOptionsPage1.TabIndex = 6;
		this.previewOptionsPage1.Tag = "preview";
		this.previewOptionsPage1.Text = "previewOptionsPage1";
		this.proxyOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.proxyOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.proxyOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.proxyOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.proxyOptionsPage1.Location = new System.Drawing.Point(0, 0);
		this.proxyOptionsPage1.Name = "proxyOptionsPage1";
		postXINGPreferences.AutoCreateNewPost = false;
		proxyInfo.BypassLocal = false;
		proxyInfo.OverrideDefaultProxy = true;
		proxyInfo.ProxyDomain = "";
		proxyInfo.ProxyName = "";
		proxyInfo.ProxyPassword = "";
		proxyInfo.ProxyPort = 8080;
		proxyInfo.ProxyUserName = "";
		proxyInfo.RequiresLogin = false;
		proxyInfo.UseProxy = false;
		postXINGPreferences.HttpProxyInfo = proxyInfo;
		postXINGPreferences.MinimizeToNotificationArea = false;
		this.proxyOptionsPage1.Preferences = postXINGPreferences;
		this.proxyOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.proxyOptionsPage1.Size = new System.Drawing.Size(481, 491);
		this.proxyOptionsPage1.TabIndex = 3;
		this.proxyOptionsPage1.Tag = "proxy";
		this.proxyOptionsPage1.Text = "proxyOptionsPage1";
		this.generalOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.generalOptionsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.generalOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.generalOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.generalOptionsPage1.Location = new System.Drawing.Point(0, 0);
		this.generalOptionsPage1.Name = "generalOptionsPage1";
		this.generalOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.generalOptionsPage1.Size = new System.Drawing.Size(481, 491);
		this.generalOptionsPage1.TabIndex = 8;
		this.generalOptionsPage1.Tag = "general";
		this.generalOptionsPage1.Text = "generalOptionsPage1";
		this.lblCurrentOperation.BackColor = System.Drawing.Color.Transparent;
		this.lblCurrentOperation.Font = new System.Drawing.Font("Tahoma", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCurrentOperation.Location = new System.Drawing.Point(143, 27);
		this.lblCurrentOperation.Name = "lblCurrentOperation";
		this.lblCurrentOperation.Size = new System.Drawing.Size(325, 38);
		this.lblCurrentOperation.TabIndex = 1;
		this.lblCurrentOperation.Text = "Create a New Account";
		this.lblCurrentOperation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(481, 491);
		base.Controls.Add(this.gradientPanel1);
		base.Controls.Add(this.scrollingFrame1);
		base.Controls.Add(this.gradientPanel2);
		base.Controls.Add(this.welcomeOptionsPage1);
		base.Controls.Add(this.ftpOptionsPage1);
		base.Controls.Add(this.mediaOptionsPage1);
		base.Controls.Add(this.previewOptionsPage1);
		base.Controls.Add(this.proxyOptionsPage1);
		base.Controls.Add(this.generalOptionsPage1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "NewUserDialog";
		this.Text = "Create a New Account";
		this.gradientPanel2.ResumeLayout(false);
		this.gradientPanel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}

	public NewUserDialog()
	{
		InitializeComponent();
		scrollingFrame1.Navigate += scrollingFrame1_Navigate;
	}

	private void scrollingFrame1_Navigate(object sender, PostXING.Controls.Navigation.NavigateEventArgs e)
	{
		if (e.Page != null)
		{
			if (e.Page.Tag.ToString() == "welcome")
			{
				btnBack.Enabled = false;
			}
			SetCurrentOperation(e.Page.Tag.ToString());
		}
		if (CurrentProvider is IBlogProvider blogProvider)
		{
			btnFinish.Enabled = blogProvider.IsConfigurationComplete;
		}
	}

	private void SetCurrentOperation(string currentPageTag)
	{
		switch (currentPageTag)
		{
		case "welcome":
			lblCurrentOperation.Text = "Create a New Blog";
			break;
		case "config":
			lblCurrentOperation.Text = "Configure Provider Settings";
			break;
		case "proxy":
			lblCurrentOperation.Text = "Configure the Proxy";
			break;
		case "ftp":
			lblCurrentOperation.Text = "Configure FTP Settings";
			break;
		case "preview":
			lblCurrentOperation.Text = "Configure Preview Template";
			break;
		case "media":
			lblCurrentOperation.Text = "Set optional media information";
			break;
		case "general":
			lblCurrentOperation.Text = "Configure General Options";
			break;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		welcomeOptionsPage1.ParentDialog = this;
		scrollingFrame1.Go(welcomeOptionsPage1);
		base.OnLoad(e);
	}

	public void NavigateToProxyPage(object sender, EventArgs e)
	{
		scrollingFrame1.Go(proxyOptionsPage1);
	}

	public void NavigateToFtpPage(object sender, EventArgs e)
	{
		scrollingFrame1.Go(ftpOptionsPage1);
	}

	public void NavigateToMediaPage(object sender, EventArgs e)
	{
		scrollingFrame1.Go(mediaOptionsPage1);
	}

	public void NavigateToPreviewPage(object sender, EventArgs e)
	{
		scrollingFrame1.Go(previewOptionsPage1);
	}

	public void NavigateToGeneralPage(object sender, EventArgs e)
	{
		scrollingFrame1.Go(generalOptionsPage1);
	}

	public void NavigateToConfigurationPage(object sender, EventArgs e)
	{
		if (ConfigurationPageIndex < ConfigurationPages.Length)
		{
			scrollingFrame1.Go(ConfigurationPages[ConfigurationPageIndex++]);
			return;
		}
		scrollingFrame1.Go(proxyOptionsPage1);
		lblCurrentOperation.Text = "Configure the Proxy";
	}

	public void ApplyBlogSettings()
	{
		OriginalBlog = (Blog)CurrentBlog.Clone();
		if (scrollingFrame1.CurrentPage is OptionsPageBase optionsPageBase)
		{
			optionsPageBase.ApplySettings();
		}
		else if (scrollingFrame1.CurrentPage is IOptionsPage optionsPage)
		{
			optionsPage.ApplySettings(CurrentBlog);
		}
		AppManager.Blogs.Add(CurrentBlog);
		AppManager.SetSelected(CurrentBlog);
		AppManager.ConcreteEditor.LoadBlogs();
	}

	public void ShowOptions()
	{
		throw new Exception("The method or operation is not implemented.");
	}

	private void btnBack_Click(object sender, EventArgs e)
	{
		scrollingFrame1.GoBack();
		if (scrollingFrame1.CurrentPage.Tag.ToString() == "config")
		{
			ConfigurationPageIndex--;
		}
		btnNext.Enabled = true;
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		if (!btnBack.Enabled)
		{
			btnBack.Enabled = true;
		}
		switch (scrollingFrame1.CurrentPage.Tag.ToString())
		{
		case "welcome":
			NavigateToConfigurationPage(sender, e);
			break;
		case "proxy":
			if (CurrentProvider != null && ((IBlogProvider)CurrentProvider).IsConfigurationComplete)
			{
				NavigateToFtpPage(sender, e);
			}
			else
			{
				NavigateToConfigurationPage(sender, e);
			}
			break;
		case "ftp":
			NavigateToPreviewPage(sender, e);
			break;
		case "preview":
			NavigateToMediaPage(sender, e);
			break;
		case "media":
			NavigateToGeneralPage(sender, e);
			btnNext.Enabled = false;
			break;
		case "config":
			NavigateToConfigurationPage(sender, e);
			break;
		default:
			NavigateToProxyPage(sender, e);
			break;
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		ApplyBlogSettings();
		Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}
}
