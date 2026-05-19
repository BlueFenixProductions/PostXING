using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class ProxyOptionsPage : OptionsPageBase
{
	private GroupBox groupBox1;

	private RadioButton rbtnPrivateProxy;

	private RadioButton rbtnDefaultProxy;

	private RadioButton rbtnDontUse;

	private Panel pnlPrivateProxySettings;

	private CheckBox chkBypassLocal;

	private TextBox txtHttpPort;

	private CheckBox chkNeedsAuthorization;

	private Label label2;

	private TextBox txtHttpProxy;

	private Label label3;

	private Panel pnlProxyCredentials;

	private Label label4;

	private Label label5;

	private TextBox txtProxyPassword;

	private TextBox txtProxyUserName;

	private Container components;

	private PostXINGPreferences preferences = new PostXINGPreferences();

	public PostXINGPreferences Preferences
	{
		get
		{
			return preferences;
		}
		set
		{
			preferences = value;
		}
	}

	public ProxyOptionsPage()
	{
		InitializeComponent();
	}

	private void _getPreferencesFromDialog()
	{
		preferences.HttpProxyInfo.OverrideDefaultProxy = rbtnPrivateProxy.Checked;
		preferences.HttpProxyInfo.BypassLocal = chkBypassLocal.Checked;
		string[] array = txtProxyUserName.Text.Trim().Split('\\');
		if (array.Length > 1)
		{
			preferences.HttpProxyInfo.ProxyDomain = array[0];
		}
		preferences.HttpProxyInfo.ProxyUserName = array[array.Length - 1];
		preferences.HttpProxyInfo.ProxyPassword = txtProxyPassword.Text.Trim();
		preferences.HttpProxyInfo.ProxyName = txtHttpProxy.Text.Trim();
		preferences.HttpProxyInfo.ProxyPort = int.Parse(txtHttpPort.Text.Trim());
		preferences.HttpProxyInfo.RequiresLogin = chkNeedsAuthorization.Checked;
	}

	private void _setDialogFromPreferences()
	{
		preferences = AppManager.Preferences;
		rbtnDefaultProxy.Checked = !preferences.HttpProxyInfo.OverrideDefaultProxy;
		rbtnPrivateProxy.Checked = preferences.HttpProxyInfo.OverrideDefaultProxy;
		rbtnDontUse.Checked = !preferences.HttpProxyInfo.UseProxy;
		txtHttpProxy.Text = preferences.HttpProxyInfo.ProxyName;
		txtHttpPort.Text = preferences.HttpProxyInfo.ProxyPort.ToString();
		chkBypassLocal.Checked = preferences.HttpProxyInfo.BypassLocal;
		chkNeedsAuthorization.Checked = preferences.HttpProxyInfo.RequiresLogin;
		txtProxyUserName.Text = preferences.HttpProxyInfo.ProxyUserName;
		txtProxyPassword.Text = preferences.HttpProxyInfo.ProxyPassword;
	}

	protected override void OnLoad(EventArgs e)
	{
		_setDialogFromPreferences();
		base.OnLoad(e);
	}

	public override void ApplySettings()
	{
		preferences.HttpProxyInfo = new ProxyInfo(!rbtnDontUse.Checked);
		_getPreferencesFromDialog();
		if (_dialog.IsEditing)
		{
			AppManager.ApplyPreferences(preferences);
		}
		else
		{
			_provider.Proxy = ProxyFactory.Create(preferences.HttpProxyInfo, _dialog.CurrentBlog.ServiceUrl);
		}
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		ApplySettings();
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
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rbtnPrivateProxy = new System.Windows.Forms.RadioButton();
		this.rbtnDefaultProxy = new System.Windows.Forms.RadioButton();
		this.rbtnDontUse = new System.Windows.Forms.RadioButton();
		this.pnlPrivateProxySettings = new System.Windows.Forms.Panel();
		this.chkBypassLocal = new System.Windows.Forms.CheckBox();
		this.txtHttpPort = new System.Windows.Forms.TextBox();
		this.chkNeedsAuthorization = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtHttpProxy = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.pnlProxyCredentials = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtProxyPassword = new System.Windows.Forms.TextBox();
		this.txtProxyUserName = new System.Windows.Forms.TextBox();
		this.groupBox1.SuspendLayout();
		this.pnlPrivateProxySettings.SuspendLayout();
		this.pnlProxyCredentials.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Controls.Add(this.rbtnPrivateProxy);
		this.groupBox1.Controls.Add(this.rbtnDefaultProxy);
		this.groupBox1.Controls.Add(this.rbtnDontUse);
		this.groupBox1.Controls.Add(this.pnlPrivateProxySettings);
		this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.groupBox1.Location = new System.Drawing.Point(8, 20);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(320, 299);
		this.groupBox1.TabIndex = 5;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Proxy server";
		this.rbtnPrivateProxy.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rbtnPrivateProxy.Location = new System.Drawing.Point(16, 64);
		this.rbtnPrivateProxy.Name = "rbtnPrivateProxy";
		this.rbtnPrivateProxy.Size = new System.Drawing.Size(296, 16);
		this.rbtnPrivateProxy.TabIndex = 2;
		this.rbtnPrivateProxy.Text = "Use private proxy server settings:";
		this.rbtnPrivateProxy.CheckedChanged += new System.EventHandler(proxyServer_CheckedChanged);
		this.rbtnDefaultProxy.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rbtnDefaultProxy.Location = new System.Drawing.Point(16, 48);
		this.rbtnDefaultProxy.Name = "rbtnDefaultProxy";
		this.rbtnDefaultProxy.Size = new System.Drawing.Size(296, 16);
		this.rbtnDefaultProxy.TabIndex = 1;
		this.rbtnDefaultProxy.Text = "Use proxy server settings from Internet Explorer";
		this.rbtnDefaultProxy.CheckedChanged += new System.EventHandler(proxyServer_CheckedChanged);
		this.rbtnDontUse.Checked = true;
		this.rbtnDontUse.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rbtnDontUse.Location = new System.Drawing.Point(16, 32);
		this.rbtnDontUse.Name = "rbtnDontUse";
		this.rbtnDontUse.Size = new System.Drawing.Size(296, 16);
		this.rbtnDontUse.TabIndex = 0;
		this.rbtnDontUse.TabStop = true;
		this.rbtnDontUse.Text = "Don't use a proxy server";
		this.rbtnDontUse.CheckedChanged += new System.EventHandler(proxyServer_CheckedChanged);
		this.pnlPrivateProxySettings.Controls.Add(this.chkBypassLocal);
		this.pnlPrivateProxySettings.Controls.Add(this.txtHttpPort);
		this.pnlPrivateProxySettings.Controls.Add(this.chkNeedsAuthorization);
		this.pnlPrivateProxySettings.Controls.Add(this.label2);
		this.pnlPrivateProxySettings.Controls.Add(this.txtHttpProxy);
		this.pnlPrivateProxySettings.Controls.Add(this.label3);
		this.pnlPrivateProxySettings.Controls.Add(this.pnlProxyCredentials);
		this.pnlPrivateProxySettings.Enabled = false;
		this.pnlPrivateProxySettings.Location = new System.Drawing.Point(8, 80);
		this.pnlPrivateProxySettings.Name = "pnlPrivateProxySettings";
		this.pnlPrivateProxySettings.Size = new System.Drawing.Size(304, 208);
		this.pnlPrivateProxySettings.TabIndex = 18;
		this.chkBypassLocal.Checked = true;
		this.chkBypassLocal.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkBypassLocal.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkBypassLocal.Location = new System.Drawing.Point(24, 112);
		this.chkBypassLocal.Name = "chkBypassLocal";
		this.chkBypassLocal.Size = new System.Drawing.Size(280, 16);
		this.chkBypassLocal.TabIndex = 7;
		this.chkBypassLocal.Text = "Bypass proxy addresses for local addresses";
		this.txtHttpPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHttpPort.Location = new System.Drawing.Point(240, 32);
		this.txtHttpPort.Name = "txtHttpPort";
		this.txtHttpPort.Size = new System.Drawing.Size(56, 21);
		this.txtHttpPort.TabIndex = 4;
		this.txtHttpPort.Text = "8080";
		this.chkNeedsAuthorization.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkNeedsAuthorization.Location = new System.Drawing.Point(24, 136);
		this.chkNeedsAuthorization.Name = "chkNeedsAuthorization";
		this.chkNeedsAuthorization.Size = new System.Drawing.Size(280, 16);
		this.chkNeedsAuthorization.TabIndex = 8;
		this.chkNeedsAuthorization.Text = "Proxy needs authorization";
		this.chkNeedsAuthorization.CheckedChanged += new System.EventHandler(chkNeedsAuthorization_CheckedChanged);
		this.label2.Location = new System.Drawing.Point(1, 32);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 23);
		this.label2.TabIndex = 5;
		this.label2.Text = "HTTP Proxy:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtHttpProxy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHttpProxy.Location = new System.Drawing.Point(80, 32);
		this.txtHttpProxy.Name = "txtHttpProxy";
		this.txtHttpProxy.Size = new System.Drawing.Size(120, 21);
		this.txtHttpProxy.TabIndex = 3;
		this.label3.Location = new System.Drawing.Point(192, 32);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 23);
		this.label3.TabIndex = 6;
		this.label3.Text = "Port:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.pnlProxyCredentials.Controls.Add(this.label4);
		this.pnlProxyCredentials.Controls.Add(this.label5);
		this.pnlProxyCredentials.Controls.Add(this.txtProxyPassword);
		this.pnlProxyCredentials.Controls.Add(this.txtProxyUserName);
		this.pnlProxyCredentials.Enabled = false;
		this.pnlProxyCredentials.Location = new System.Drawing.Point(0, 152);
		this.pnlProxyCredentials.Name = "pnlProxyCredentials";
		this.pnlProxyCredentials.Size = new System.Drawing.Size(304, 56);
		this.pnlProxyCredentials.TabIndex = 5;
		this.label4.Location = new System.Drawing.Point(0, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 23);
		this.label4.TabIndex = 11;
		this.label4.Text = "User Name:";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label5.Location = new System.Drawing.Point(8, 32);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(56, 23);
		this.label5.TabIndex = 12;
		this.label5.Text = "Password:";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtProxyPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProxyPassword.Location = new System.Drawing.Point(72, 32);
		this.txtProxyPassword.Name = "txtProxyPassword";
		this.txtProxyPassword.Size = new System.Drawing.Size(232, 21);
		this.txtProxyPassword.TabIndex = 10;
		this.txtProxyUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProxyUserName.Location = new System.Drawing.Point(72, 8);
		this.txtProxyUserName.Name = "txtProxyUserName";
		this.txtProxyUserName.Size = new System.Drawing.Size(232, 21);
		this.txtProxyUserName.TabIndex = 9;
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "ProxyOptionsPage";
		base.Size = new System.Drawing.Size(336, 368);
		this.groupBox1.ResumeLayout(false);
		this.pnlPrivateProxySettings.ResumeLayout(false);
		this.pnlPrivateProxySettings.PerformLayout();
		this.pnlProxyCredentials.ResumeLayout(false);
		this.pnlProxyCredentials.PerformLayout();
		base.ResumeLayout(false);
	}

	private void proxyServer_CheckedChanged(object sender, EventArgs e)
	{
		pnlPrivateProxySettings.Enabled = rbtnPrivateProxy.Checked;
	}

	private void chkNeedsAuthorization_CheckedChanged(object sender, EventArgs e)
	{
		pnlProxyCredentials.Enabled = chkNeedsAuthorization.Checked;
	}
}
