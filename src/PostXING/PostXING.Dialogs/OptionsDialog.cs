using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls.Controls;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;
using PostXING.NavigationPages;

namespace PostXING.Dialogs;

public class OptionsDialog : Form, IConfigurationDialog
{
	private MozPane mozPane1;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel3;

	private Panel panel4;

	private ScrollingFrame scrollingFrame1;

	private ImageList imageList2;

	private MozItem mozWelcome;

	private MozItem mozConfigure;

	private MozItem mozFTP;

	private MozItem mozMedia;

	private MozItem mozPreview;

	private MozItem mozProxy;

	private MozItem mozGeneral;

	private WelcomeOptionsPage welcomeOptionsPage1;

	private Button btnApply;

	private Button btnDone;

	private Button btnCancel;

	private ProxyOptionsPage proxyOptionsPage1;

	private bool _isEditing;

	private object _currentProvider;

	public bool IsEditing
	{
		get
		{
			return _isEditing;
		}
		set
		{
			_isEditing = value;
			if (_isEditing)
			{
				CurrentProvider = AppManager.CurrentProvider;
			}
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
		}
	}

	public Blog CurrentBlog
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

	public OptionsDialog()
	{
		InitializeComponent();
	}

	public OptionsDialog(bool isEditing)
		: this()
	{
		IsEditing = isEditing;
	}

	internal void SelectConfigurationPage()
	{
		mozConfigure.SelectItem();
		Page[] configurationPages = ((IBlogProvider)CurrentProvider).ConfigurationPages;
		if (configurationPages != null && configurationPages.Length > 0)
		{
			scrollingFrame1.Go(configurationPages[0].GetType());
		}
	}

	internal void SelectGeneralPage()
	{
		mozGeneral.SelectItem();
		scrollingFrame1.Go(typeof(GeneralOptionsPage));
	}

	internal void SelectProxyPage()
	{
		mozProxy.SelectItem();
		proxyOptionsPage1.Visible = true;
		proxyOptionsPage1.Dock = DockStyle.Fill;
		scrollingFrame1.Go(proxyOptionsPage1);
	}

	internal void SelectFTPPage()
	{
		mozFTP.SelectItem();
		scrollingFrame1.Go(typeof(FTPOptionsPage));
	}

	internal void SelectMediaPage()
	{
		mozMedia.SelectItem();
		scrollingFrame1.Go(typeof(MediaOptionsPage));
	}

	internal void SelectPreviewPage()
	{
		mozPreview.SelectItem();
		scrollingFrame1.Go(typeof(PreviewOptionsPage));
	}

	protected override void OnLoad(EventArgs e)
	{
		welcomeOptionsPage1 = new WelcomeOptionsPage();
		welcomeOptionsPage1.ParentDialog = this;
		scrollingFrame1.HomePage = welcomeOptionsPage1;
		mozWelcome.SelectItem();
		scrollingFrame1.GoHome();
		base.OnLoad(e);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		e.Cancel = true;
		mozWelcome.SelectItem();
		scrollingFrame1.GoHome();
		Hide();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Dialogs.OptionsDialog));
		PostXING.Components.PostXINGPreferences postXINGPreferences = new PostXING.Components.PostXINGPreferences();
		PostXING.Components.ProxyInfo proxyInfo = new PostXING.Components.ProxyInfo();
		this.mozPane1 = new PostXING.Controls.Controls.MozPane();
		this.imageList2 = new System.Windows.Forms.ImageList(this.components);
		this.mozWelcome = new PostXING.Controls.Controls.MozItem();
		this.mozConfigure = new PostXING.Controls.Controls.MozItem();
		this.mozFTP = new PostXING.Controls.Controls.MozItem();
		this.mozMedia = new PostXING.Controls.Controls.MozItem();
		this.mozPreview = new PostXING.Controls.Controls.MozItem();
		this.mozProxy = new PostXING.Controls.Controls.MozItem();
		this.mozGeneral = new PostXING.Controls.Controls.MozItem();
		this.btnApply = new System.Windows.Forms.Button();
		this.btnDone = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.scrollingFrame1 = new PostXING.Controls.Navigation.ScrollingFrame();
		this.proxyOptionsPage1 = new PostXING.NavigationPages.ProxyOptionsPage();
		((System.ComponentModel.ISupportInitialize)this.mozPane1).BeginInit();
		this.mozPane1.SuspendLayout();
		base.SuspendLayout();
		this.mozPane1.BackColor = System.Drawing.Color.White;
		this.mozPane1.BorderColor = System.Drawing.Color.FromArgb(176, 176, 176);
		this.mozPane1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.Dock = System.Windows.Forms.DockStyle.Left;
		this.mozPane1.ImageList = this.imageList2;
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
		this.mozPane1.Items.AddRange(new PostXING.Controls.Controls.MozItem[7] { this.mozWelcome, this.mozConfigure, this.mozFTP, this.mozMedia, this.mozPreview, this.mozProxy, this.mozGeneral });
		this.mozPane1.Location = new System.Drawing.Point(8, 16);
		this.mozPane1.MaxSelectedItems = 1;
		this.mozPane1.Name = "mozPane1";
		this.mozPane1.SelectButton = PostXING.Controls.Controls.MozSelectButton.Left;
		this.mozPane1.Size = new System.Drawing.Size(128, 439);
		this.mozPane1.Style = PostXING.Controls.Controls.MozPaneStyle.Vertical;
		this.mozPane1.TabIndex = 0;
		this.mozPane1.Theme = true;
		this.mozPane1.Toggle = false;
		this.imageList2.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList2.ImageStream");
		this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList2.Images.SetKeyName(0, "");
		this.imageList2.Images.SetKeyName(1, "");
		this.imageList2.Images.SetKeyName(2, "");
		this.imageList2.Images.SetKeyName(3, "");
		this.imageList2.Images.SetKeyName(4, "");
		this.imageList2.Images.SetKeyName(5, "");
		this.imageList2.Images.SetKeyName(6, "");
		this.imageList2.Images.SetKeyName(7, "");
		this.mozWelcome.Images.Focus = 7;
		this.mozWelcome.Images.Normal = 7;
		this.mozWelcome.Images.Selected = 7;
		this.mozWelcome.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozWelcome.Location = new System.Drawing.Point(2, 2);
		this.mozWelcome.Name = "mozWelcome";
		this.mozWelcome.Size = new System.Drawing.Size(124, 40);
		this.mozWelcome.TabIndex = 0;
		this.mozWelcome.Text = "Welcome";
		this.mozWelcome.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozWelcome.Click += new System.EventHandler(mozWelcome_Click);
		this.mozConfigure.Images.Focus = 3;
		this.mozConfigure.Images.Normal = 3;
		this.mozConfigure.Images.Selected = 3;
		this.mozConfigure.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozConfigure.Location = new System.Drawing.Point(2, 44);
		this.mozConfigure.Name = "mozConfigure";
		this.mozConfigure.Size = new System.Drawing.Size(124, 40);
		this.mozConfigure.TabIndex = 1;
		this.mozConfigure.Text = "Configure";
		this.mozConfigure.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozConfigure.Click += new System.EventHandler(mozConfigure_Click);
		this.mozFTP.Images.Focus = 2;
		this.mozFTP.Images.Normal = 2;
		this.mozFTP.Images.Selected = 2;
		this.mozFTP.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozFTP.Location = new System.Drawing.Point(2, 86);
		this.mozFTP.Name = "mozFTP";
		this.mozFTP.Size = new System.Drawing.Size(124, 40);
		this.mozFTP.TabIndex = 2;
		this.mozFTP.Text = "FTP";
		this.mozFTP.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozFTP.Click += new System.EventHandler(mozFTP_Click);
		this.mozMedia.Images.Focus = 4;
		this.mozMedia.Images.Normal = 4;
		this.mozMedia.Images.Selected = 4;
		this.mozMedia.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozMedia.Location = new System.Drawing.Point(2, 128);
		this.mozMedia.Name = "mozMedia";
		this.mozMedia.Size = new System.Drawing.Size(124, 40);
		this.mozMedia.TabIndex = 3;
		this.mozMedia.Text = "Media";
		this.mozMedia.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozMedia.Click += new System.EventHandler(mozMedia_Click);
		this.mozPreview.Images.Focus = 5;
		this.mozPreview.Images.Normal = 5;
		this.mozPreview.Images.Selected = 5;
		this.mozPreview.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozPreview.Location = new System.Drawing.Point(2, 170);
		this.mozPreview.Name = "mozPreview";
		this.mozPreview.Size = new System.Drawing.Size(124, 40);
		this.mozPreview.TabIndex = 4;
		this.mozPreview.Text = "Preview";
		this.mozPreview.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozPreview.Click += new System.EventHandler(mozPreview_Click);
		this.mozProxy.Images.Focus = 0;
		this.mozProxy.Images.Normal = 0;
		this.mozProxy.Images.Selected = 0;
		this.mozProxy.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozProxy.Location = new System.Drawing.Point(2, 212);
		this.mozProxy.Name = "mozProxy";
		this.mozProxy.Size = new System.Drawing.Size(124, 40);
		this.mozProxy.TabIndex = 5;
		this.mozProxy.Text = "Proxy";
		this.mozProxy.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozProxy.Click += new System.EventHandler(mozProxy_Click);
		this.mozGeneral.Images.Focus = 1;
		this.mozGeneral.Images.Normal = 1;
		this.mozGeneral.Images.Selected = 1;
		this.mozGeneral.ItemStyle = PostXING.Controls.Controls.MozItemStyle.TextAndPicture;
		this.mozGeneral.Location = new System.Drawing.Point(2, 254);
		this.mozGeneral.Name = "mozGeneral";
		this.mozGeneral.Size = new System.Drawing.Size(124, 40);
		this.mozGeneral.TabIndex = 6;
		this.mozGeneral.Text = "General";
		this.mozGeneral.TextAlign = PostXING.Controls.Controls.MozTextAlign.Right;
		this.mozGeneral.Click += new System.EventHandler(mozGeneral_Click);
		this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnApply.Location = new System.Drawing.Point(207, 434);
		this.btnApply.Name = "btnApply";
		this.btnApply.Size = new System.Drawing.Size(75, 23);
		this.btnApply.TabIndex = 2;
		this.btnApply.Text = "Apply";
		this.btnApply.Click += new System.EventHandler(btnApply_Click);
		this.btnDone.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnDone.Location = new System.Drawing.Point(295, 434);
		this.btnDone.Name = "btnDone";
		this.btnDone.Size = new System.Drawing.Size(75, 23);
		this.btnDone.TabIndex = 3;
		this.btnDone.Text = "Done";
		this.btnDone.Click += new System.EventHandler(btnDone_Click);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnCancel.Location = new System.Drawing.Point(399, 434);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 4;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 16);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(8, 439);
		this.panel1.TabIndex = 5;
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(489, 16);
		this.panel2.TabIndex = 6;
		this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel3.Location = new System.Drawing.Point(0, 455);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(489, 16);
		this.panel3.TabIndex = 7;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(136, 16);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(8, 439);
		this.panel4.TabIndex = 8;
		this.scrollingFrame1.Dock = System.Windows.Forms.DockStyle.Top;
		this.scrollingFrame1.Location = new System.Drawing.Point(144, 16);
		this.scrollingFrame1.Name = "scrollingFrame1";
		this.scrollingFrame1.Size = new System.Drawing.Size(345, 400);
		this.scrollingFrame1.TabIndex = 9;
		this.proxyOptionsPage1.BackColor = System.Drawing.SystemColors.Window;
		this.proxyOptionsPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.proxyOptionsPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.proxyOptionsPage1.Location = new System.Drawing.Point(144, 16);
		this.proxyOptionsPage1.Name = "proxyOptionsPage1";
		postXINGPreferences.AutoCreateNewPost = false;
		postXINGPreferences.AutoSaveEnabled = false;
		postXINGPreferences.AutoSaveIntervalInMinutes = 2;
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
		postXINGPreferences.SuppressSplashPage = false;
		postXINGPreferences.UploadUsing = "FTP";
		this.proxyOptionsPage1.Preferences = postXINGPreferences;
		this.proxyOptionsPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.proxyOptionsPage1.Size = new System.Drawing.Size(336, 368);
		this.proxyOptionsPage1.TabIndex = 10;
		this.proxyOptionsPage1.Text = "proxyOptionsPage1";
		this.proxyOptionsPage1.Visible = false;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
		base.ClientSize = new System.Drawing.Size(489, 471);
		base.Controls.Add(this.proxyOptionsPage1);
		base.Controls.Add(this.scrollingFrame1);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnDone);
		base.Controls.Add(this.btnApply);
		base.Controls.Add(this.mozPane1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel3);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "OptionsDialog";
		this.Text = "Options";
		((System.ComponentModel.ISupportInitialize)this.mozPane1).EndInit();
		this.mozPane1.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	public void ShowOptions()
	{
		Show();
		Focus();
	}

	public void NavigateToProxyPage(object sender, EventArgs e)
	{
		SelectProxyPage();
	}

	public void NavigateToPreviewPage(object sender, EventArgs e)
	{
		SelectPreviewPage();
	}

	public void NavigateToFtpPage(object sender, EventArgs e)
	{
		SelectFTPPage();
	}

	public void NavigateToMediaPage(object sender, EventArgs e)
	{
		SelectMediaPage();
	}

	public void NavigateToGeneralPage(object sender, EventArgs e)
	{
		SelectGeneralPage();
	}

	public void NavigateToConfigurationPage(object sender, EventArgs e)
	{
		SelectConfigurationPage();
	}

	public void ApplyBlogSettings()
	{
		if (scrollingFrame1.CurrentPage is OptionsPageBase optionsPageBase)
		{
			optionsPageBase.ApplySettings();
		}
		else if (scrollingFrame1.CurrentPage is IOptionsPage optionsPage)
		{
			optionsPage.ApplySettings(CurrentBlog);
		}
		int num = AppManager.Blogs.IndexOfKey(CurrentBlog.Key);
		if (num > -1)
		{
			AppManager.Blogs.RemoveAt(num);
		}
		AppManager.Blogs.Add(CurrentBlog);
		AppManager.SetSelected(CurrentBlog);
		AppManager.ConcreteEditor.LoadBlogs();
	}

	private void mozWelcome_Click(object sender, EventArgs e)
	{
		scrollingFrame1.Go(welcomeOptionsPage1);
	}

	private void mozProxy_Click(object sender, EventArgs e)
	{
		SelectProxyPage();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnApply_Click(object sender, EventArgs e)
	{
		ApplyBlogSettings();
	}

	private void btnDone_Click(object sender, EventArgs e)
	{
		btnApply_Click(sender, e);
		btnCancel_Click(sender, e);
	}

	private void mozFTP_Click(object sender, EventArgs e)
	{
		SelectFTPPage();
	}

	private void mozMedia_Click(object sender, EventArgs e)
	{
		SelectMediaPage();
	}

	private void mozPreview_Click(object sender, EventArgs e)
	{
		SelectPreviewPage();
	}

	private void mozConfigure_Click(object sender, EventArgs e)
	{
		SelectConfigurationPage();
	}

	private void mozGeneral_Click(object sender, EventArgs e)
	{
		SelectGeneralPage();
	}
}
