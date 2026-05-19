using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;

namespace PostXING.MetaBlogProvider;

public class CredentialsPage : Page
{
	private PostXING.Controls.Navigation.LinkLabel lnkSelectBlog;

	private XPTextBox txtUserName;

	private XPTextBox txtPassword;

	private XPTextBox txtHost;

	private XPTextBox txtPort;

	private XPTextBox txtPage;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private CheckBox chkUseSSL;

	private Label label8;

	private XPTextBox txtItemCount;

	private Label label7;

	private Container components;

	private static IConfigurationDialog _dialog;

	private PostXING.Controls.Navigation.LinkLabel linkLabel1;

	private static IBlogProvider _provider;

	public CredentialsPage()
	{
		InitializeComponent();
	}

	public CredentialsPage(IConfigurationDialog dialog, IBlogProvider provider)
		: this()
	{
		_dialog = dialog;
		_provider = provider;
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		if (e.ViewState != null)
		{
			_dialog = (IConfigurationDialog)e.ViewState.ReadProperty("dialog");
			_provider = (IBlogProvider)e.ViewState.ReadProperty("provider");
		}
		txtUserName.Text = _dialog.CurrentBlog.Username;
		txtPassword.Text = _dialog.CurrentBlog.Password;
		txtHost.Text = _dialog.CurrentBlog.Host;
		txtPort.Text = _dialog.CurrentBlog.Port.ToString();
		txtPage.Text = _dialog.CurrentBlog.Page;
		txtItemCount.Text = _dialog.CurrentBlog.ItemCount.ToString();
		lnkSelectBlog.Visible = _dialog.IsEditing;
		base.OnPageEnter(e);
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		_dialog.CurrentBlog.Username = txtUserName.Text;
		_dialog.CurrentBlog.Password = txtPassword.Text;
		_dialog.CurrentBlog.Host = txtHost.Text;
		_dialog.CurrentBlog.Port = int.Parse(txtPort.Text);
		_dialog.CurrentBlog.Page = txtPage.Text;
		_dialog.CurrentBlog.ItemCount = int.Parse(txtItemCount.Text);
		_dialog.CurrentBlog.UseSSL = chkUseSSL.Checked;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.MetaBlogProvider.CredentialsPage));
		this.lnkSelectBlog = new PostXING.Controls.Navigation.LinkLabel();
		this.txtUserName = new PostXING.Controls.XPTextBox();
		this.txtPassword = new PostXING.Controls.XPTextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtHost = new PostXING.Controls.XPTextBox();
		this.txtPort = new PostXING.Controls.XPTextBox();
		this.txtPage = new PostXING.Controls.XPTextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.chkUseSSL = new System.Windows.Forms.CheckBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtItemCount = new PostXING.Controls.XPTextBox();
		this.linkLabel1 = new PostXING.Controls.Navigation.LinkLabel();
		base.SuspendLayout();
		this.lnkSelectBlog.Image = (System.Drawing.Image)resources.GetObject("lnkSelectBlog.Image");
		this.lnkSelectBlog.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lnkSelectBlog.Location = new System.Drawing.Point(200, 272);
		this.lnkSelectBlog.Name = "lnkSelectBlog";
		this.lnkSelectBlog.Size = new System.Drawing.Size(80, 23);
		this.lnkSelectBlog.TabIndex = 0;
		this.lnkSelectBlog.TabStop = true;
		this.lnkSelectBlog.Text = "Select Blog";
		this.lnkSelectBlog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkSelectBlog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkSelectBlog_LinkClicked);
		this.txtUserName.Location = new System.Drawing.Point(80, 56);
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(128, 21);
		this.txtUserName.TabIndex = 1;
		this.txtPassword.Location = new System.Drawing.Point(80, 88);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(128, 21);
		this.txtPassword.TabIndex = 2;
		this.label1.Location = new System.Drawing.Point(8, 56);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 23);
		this.label1.TabIndex = 3;
		this.label1.Text = "UserName:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label2.Location = new System.Drawing.Point(8, 88);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(64, 23);
		this.label2.TabIndex = 4;
		this.label2.Text = "Password:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtHost.Location = new System.Drawing.Point(80, 120);
		this.txtHost.Name = "txtHost";
		this.txtHost.Size = new System.Drawing.Size(200, 21);
		this.txtHost.TabIndex = 5;
		this.txtPort.Location = new System.Drawing.Point(80, 152);
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(32, 21);
		this.txtPort.TabIndex = 14;
		this.txtPort.Text = "80";
		this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPage.Location = new System.Drawing.Point(80, 184);
		this.txtPage.Name = "txtPage";
		this.txtPage.Size = new System.Drawing.Size(200, 21);
		this.txtPage.TabIndex = 13;
		this.label3.Location = new System.Drawing.Point(8, 184);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 23);
		this.label3.TabIndex = 8;
		this.label3.Text = "Page:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label4.Location = new System.Drawing.Point(8, 152);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 23);
		this.label4.TabIndex = 9;
		this.label4.Text = "Port:";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label5.Location = new System.Drawing.Point(8, 120);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 23);
		this.label5.TabIndex = 10;
		this.label5.Text = "Host:";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label6.Location = new System.Drawing.Point(160, 144);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(120, 23);
		this.label6.TabIndex = 11;
		this.label6.Text = "[e.g. weblogs.asp.net]";
		this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label7.Location = new System.Drawing.Point(56, 209);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(224, 23);
		this.label7.TabIndex = 12;
		this.label7.Text = "[e.g. /yourblog/services/metablogapi.aspx]";
		this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.chkUseSSL.Location = new System.Drawing.Point(224, 88);
		this.chkUseSSL.Name = "chkUseSSL";
		this.chkUseSSL.Size = new System.Drawing.Size(64, 24);
		this.chkUseSSL.TabIndex = 15;
		this.chkUseSSL.Text = "Use SSL";
		this.label8.Location = new System.Drawing.Point(0, 232);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 23);
		this.label8.TabIndex = 16;
		this.label8.Text = "Item Count:";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtItemCount.Location = new System.Drawing.Point(80, 232);
		this.txtItemCount.Name = "txtItemCount";
		this.txtItemCount.Size = new System.Drawing.Size(32, 21);
		this.txtItemCount.TabIndex = 17;
		this.txtItemCount.Text = "15";
		this.txtItemCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.linkLabel1.Image = (System.Drawing.Image)resources.GetObject("linkLabel1.Image");
		this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel1.Location = new System.Drawing.Point(176, 232);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(104, 23);
		this.linkLabel1.TabIndex = 18;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Configure Proxy";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		base.Controls.Add(this.linkLabel1);
		base.Controls.Add(this.txtItemCount);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.chkUseSSL);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.txtPage);
		base.Controls.Add(this.txtPort);
		base.Controls.Add(this.txtHost);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.lnkSelectBlog);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "CredentialsPage";
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void lnkSelectBlog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		base.Frame.Go(_provider.ConfigurationPages[1]);
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (txtHost.Text.Trim().Length > 0 && txtPort.Text.Trim().Length > 0 && txtPage.Text.Trim().Length > 0)
		{
			_dialog.NavigateToProxyPage(sender, e);
		}
		else
		{
			MessageBox.Show("Please enter the Host, Page, and Port before configuring your proxy.");
		}
	}
}
