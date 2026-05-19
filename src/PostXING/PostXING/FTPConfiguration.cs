using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;

namespace PostXING;

public class FTPConfiguration : UserControl
{
	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private XPTextBox txtHost;

	private XPTextBox txtPath;

	private XPTextBox txtPort;

	private XPTextBox txtUserName;

	private XPTextBox txtPassword;

	private XPTextBox txtBaseUrl;

	private CheckBox chkPassive;

	private Container components;

	public string Host
	{
		get
		{
			return txtHost.Text;
		}
		set
		{
			txtHost.Text = value;
		}
	}

	public string RemotePath
	{
		get
		{
			return txtPath.Text;
		}
		set
		{
			txtPath.Text = value;
		}
	}

	public string Port
	{
		get
		{
			return txtPort.Text;
		}
		set
		{
			txtPort.Text = value;
		}
	}

	public string UserName
	{
		get
		{
			return txtUserName.Text;
		}
		set
		{
			txtUserName.Text = value;
		}
	}

	public string Password
	{
		get
		{
			return txtPassword.Text;
		}
		set
		{
			txtPassword.Text = value;
		}
	}

	public string BaseUrl
	{
		get
		{
			return txtBaseUrl.Text;
		}
		set
		{
			txtBaseUrl.Text = value;
		}
	}

	public bool Passive
	{
		get
		{
			return chkPassive.Checked;
		}
		set
		{
			chkPassive.Checked = value;
		}
	}

	public FTPConfiguration()
	{
		InitializeComponent();
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
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtHost = new PostXING.Controls.XPTextBox();
		this.txtPath = new PostXING.Controls.XPTextBox();
		this.txtPort = new PostXING.Controls.XPTextBox();
		this.txtUserName = new PostXING.Controls.XPTextBox();
		this.txtPassword = new PostXING.Controls.XPTextBox();
		this.txtBaseUrl = new PostXING.Controls.XPTextBox();
		this.chkPassive = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		this.label1.Location = new System.Drawing.Point(52, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(59, 23);
		this.label1.TabIndex = 0;
		this.label1.Text = "Host:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label2.Location = new System.Drawing.Point(12, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(99, 23);
		this.label2.TabIndex = 1;
		this.label2.Text = "Remote Path:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label3.Location = new System.Drawing.Point(52, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 23);
		this.label3.TabIndex = 2;
		this.label3.Text = "Port:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label4.Location = new System.Drawing.Point(28, 104);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(83, 23);
		this.label4.TabIndex = 3;
		this.label4.Text = "Username:";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label5.Location = new System.Drawing.Point(28, 136);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(83, 23);
		this.label5.TabIndex = 4;
		this.label5.Text = "Password:";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label6.Location = new System.Drawing.Point(3, 168);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(108, 23);
		this.label6.TabIndex = 5;
		this.label6.Text = "Base HTTP URL:";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtHost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtHost.Location = new System.Drawing.Point(117, 8);
		this.txtHost.Name = "txtHost";
		this.txtHost.Size = new System.Drawing.Size(201, 20);
		this.txtHost.TabIndex = 6;
		this.txtPath.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPath.Location = new System.Drawing.Point(117, 40);
		this.txtPath.Name = "txtPath";
		this.txtPath.Size = new System.Drawing.Size(201, 20);
		this.txtPath.TabIndex = 7;
		this.txtPort.Location = new System.Drawing.Point(117, 72);
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(27, 20);
		this.txtPort.TabIndex = 8;
		this.txtPort.Text = "21";
		this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtUserName.Location = new System.Drawing.Point(117, 104);
		this.txtUserName.Name = "txtUserName";
		this.txtUserName.Size = new System.Drawing.Size(101, 20);
		this.txtUserName.TabIndex = 9;
		this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPassword.Location = new System.Drawing.Point(117, 136);
		this.txtPassword.Name = "txtPassword";
		this.txtPassword.PasswordChar = '*';
		this.txtPassword.Size = new System.Drawing.Size(101, 20);
		this.txtPassword.TabIndex = 10;
		this.txtBaseUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtBaseUrl.Location = new System.Drawing.Point(117, 168);
		this.txtBaseUrl.Name = "txtBaseUrl";
		this.txtBaseUrl.Size = new System.Drawing.Size(201, 20);
		this.txtBaseUrl.TabIndex = 11;
		this.chkPassive.AutoSize = true;
		this.chkPassive.Location = new System.Drawing.Point(117, 200);
		this.chkPassive.Name = "chkPassive";
		this.chkPassive.Size = new System.Drawing.Size(93, 17);
		this.chkPassive.TabIndex = 12;
		this.chkPassive.Text = "Passive Mode";
		this.chkPassive.UseVisualStyleBackColor = true;
		base.Controls.Add(this.chkPassive);
		base.Controls.Add(this.txtBaseUrl);
		base.Controls.Add(this.txtPassword);
		base.Controls.Add(this.txtUserName);
		base.Controls.Add(this.txtPort);
		base.Controls.Add(this.txtPath);
		base.Controls.Add(this.txtHost);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Name = "FTPConfiguration";
		base.Size = new System.Drawing.Size(326, 244);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
