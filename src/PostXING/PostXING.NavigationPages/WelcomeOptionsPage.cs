using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genghis.Windows.Forms;
using PostXING.Components;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;

namespace PostXING.NavigationPages;

public class WelcomeOptionsPage : OptionsPageBase
{
	private Label label1;

	private PostXING.Controls.Navigation.LinkLabel lnkConfigure;

	private PostXING.Controls.Navigation.LinkLabel lnkGeneralOptions;

	private PostXING.Controls.Navigation.LinkLabel lnkProxySettings;

	private PictureBox pictureBox1;

	private Label label2;

	private Label label3;

	private CompletionCombo cbxProviders;

	private Container components;

	private IConfigurationDialog _parent;

	public IConfigurationDialog ParentDialog
	{
		get
		{
			return _parent;
		}
		set
		{
			_parent = value;
			if (_parent != null && _parent.CurrentProvider != null)
			{
				cbxProviders.Text = _parent.CurrentProvider.ToString();
			}
		}
	}

	public WelcomeOptionsPage()
	{
		InitializeComponent();
	}

	protected override void OnLoad(EventArgs e)
	{
		if (!base.DesignMode)
		{
			cbxProviders.Items.AddRange(AppManager.AvailableProviders.ToArray());
			if (ParentDialog != null && ParentDialog.IsEditing)
			{
				label1.Text += "change some settings for an existing blog.";
			}
			else
			{
				label1.Text += "set up a new blog.";
			}
		}
		base.OnLoad(e);
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		PostXING.Controls.Navigation.LinkLabel linkLabel = lnkGeneralOptions;
		PostXING.Controls.Navigation.LinkLabel linkLabel2 = lnkProxySettings;
		bool flag = (lnkConfigure.Visible = ParentDialog.IsEditing);
		bool visible = (linkLabel2.Visible = flag);
		linkLabel.Visible = visible;
		base.OnPageEnter(e);
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		e.ViewState.WriteProperty("dialog", ParentDialog);
		e.ViewState.WriteProperty("provider", ParentDialog.CurrentProvider);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.WelcomeOptionsPage));
		this.label1 = new System.Windows.Forms.Label();
		this.lnkConfigure = new PostXING.Controls.Navigation.LinkLabel();
		this.lnkGeneralOptions = new PostXING.Controls.Navigation.LinkLabel();
		this.lnkProxySettings = new PostXING.Controls.Navigation.LinkLabel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.cbxProviders = new Genghis.Windows.Forms.CompletionCombo();
		this.label3 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(24, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(256, 48);
		this.label1.TabIndex = 0;
		this.label1.Text = "Welcome to PostXING configuration. Let's ";
		this.lnkConfigure.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lnkConfigure.Enabled = false;
		this.lnkConfigure.Image = (System.Drawing.Image)resources.GetObject("lnkConfigure.Image");
		this.lnkConfigure.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkConfigure.Location = new System.Drawing.Point(176, 136);
		this.lnkConfigure.Name = "lnkConfigure";
		this.lnkConfigure.Size = new System.Drawing.Size(80, 23);
		this.lnkConfigure.TabIndex = 1;
		this.lnkConfigure.TabStop = true;
		this.lnkConfigure.Text = "Configure!";
		this.lnkConfigure.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkConfigure.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkConfigure_LinkClicked);
		this.lnkGeneralOptions.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lnkGeneralOptions.Image = (System.Drawing.Image)resources.GetObject("lnkGeneralOptions.Image");
		this.lnkGeneralOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkGeneralOptions.Location = new System.Drawing.Point(184, 264);
		this.lnkGeneralOptions.Name = "lnkGeneralOptions";
		this.lnkGeneralOptions.Size = new System.Drawing.Size(112, 23);
		this.lnkGeneralOptions.TabIndex = 2;
		this.lnkGeneralOptions.TabStop = true;
		this.lnkGeneralOptions.Text = "General Options";
		this.lnkGeneralOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkGeneralOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkGeneralOptions_LinkClicked);
		this.lnkProxySettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.lnkProxySettings.Image = (System.Drawing.Image)resources.GetObject("lnkProxySettings.Image");
		this.lnkProxySettings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkProxySettings.Location = new System.Drawing.Point(184, 232);
		this.lnkProxySettings.Name = "lnkProxySettings";
		this.lnkProxySettings.Size = new System.Drawing.Size(100, 23);
		this.lnkProxySettings.TabIndex = 3;
		this.lnkProxySettings.TabStop = true;
		this.lnkProxySettings.Text = "Proxy Settings";
		this.lnkProxySettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkProxySettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkProxySettings_LinkClicked);
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(16, 112);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(192, 184);
		this.pictureBox1.TabIndex = 4;
		this.pictureBox1.TabStop = false;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.Location = new System.Drawing.Point(8, 303);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(288, 32);
		this.label2.TabIndex = 0;
		this.label2.Text = "Note: if you need to use a proxy that may need to be done before you can configure your blog.";
		this.cbxProviders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.cbxProviders.LimitToList = true;
		this.cbxProviders.Location = new System.Drawing.Point(144, 88);
		this.cbxProviders.Name = "cbxProviders";
		this.cbxProviders.Size = new System.Drawing.Size(121, 21);
		this.cbxProviders.TabIndex = 5;
		this.cbxProviders.SelectedIndexChanged += new System.EventHandler(cbxProviders_SelectedIndexChanged);
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label3.Location = new System.Drawing.Point(40, 88);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(96, 23);
		this.label3.TabIndex = 6;
		this.label3.Text = "Select a Provider:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.Controls.Add(this.label3);
		base.Controls.Add(this.cbxProviders);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.lnkProxySettings);
		base.Controls.Add(this.lnkGeneralOptions);
		base.Controls.Add(this.lnkConfigure);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.pictureBox1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "WelcomeOptionsPage";
		base.Size = new System.Drawing.Size(328, 360);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}

	private void cbxProviders_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbxProviders.SelectedIndex > -1)
		{
			lnkConfigure.Enabled = true;
			IBlogProvider blogProvider = (IBlogProvider)cbxProviders.SelectedItem;
			if (ParentDialog.CurrentProvider == null || blogProvider.ProviderName != ParentDialog.CurrentProvider.ToString())
			{
				blogProvider.InitializeIBlogProvider(ParentDialog, AppManager.EditorForm);
				ParentDialog.CurrentProvider = blogProvider;
			}
		}
	}

	private void lnkConfigure_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		ParentDialog.NavigateToConfigurationPage(sender, e);
	}

	private void lnkProxySettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		ParentDialog.NavigateToProxyPage(sender, e);
	}

	private void lnkGeneralOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		ParentDialog.NavigateToGeneralPage(sender, e);
	}
}
