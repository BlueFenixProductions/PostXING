using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING;

public class SiteConfiguration : UserControl
{
	private CheckBox chkWeblogsCom;

	private TextBox txtPort;

	private Label label6;

	private Label label4;

	private TextBox tbCount;

	private TextBox tbPassword;

	private TextBox tbUserName;

	private Label label3;

	private Label label2;

	private Label label1;

	private TextBox txtPage;

	private Label label5;

	private TextBox tbUrl;

	private CheckBox chkPingBloDotGs;

	private Label label7;

	private Label label8;

	private Container components;

	public string UserName
	{
		get
		{
			return tbUserName.Text;
		}
		set
		{
			tbUserName.Text = value;
		}
	}

	public string Password
	{
		get
		{
			return tbPassword.Text;
		}
		set
		{
			tbPassword.Text = value;
		}
	}

	public string Host
	{
		get
		{
			return tbUrl.Text;
		}
		set
		{
			tbUrl.Text = value;
		}
	}

	public bool PingWeblogsCom
	{
		get
		{
			return chkWeblogsCom.Checked;
		}
		set
		{
			chkWeblogsCom.Checked = value;
		}
	}

	public bool PingBloDotGs
	{
		get
		{
			return chkPingBloDotGs.Checked;
		}
		set
		{
			chkPingBloDotGs.Checked = value;
		}
	}

	public string Page
	{
		get
		{
			return txtPage.Text;
		}
		set
		{
			txtPage.Text = value;
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

	public string ItemCount
	{
		get
		{
			return tbCount.Text;
		}
		set
		{
			tbCount.Text = value;
		}
	}

	public SiteConfiguration()
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
		this.chkWeblogsCom = new System.Windows.Forms.CheckBox();
		this.txtPort = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.tbCount = new System.Windows.Forms.TextBox();
		this.tbUrl = new System.Windows.Forms.TextBox();
		this.tbPassword = new System.Windows.Forms.TextBox();
		this.tbUserName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtPage = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.chkPingBloDotGs = new System.Windows.Forms.CheckBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.chkWeblogsCom.Enabled = false;
		this.chkWeblogsCom.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkWeblogsCom.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkWeblogsCom.ForeColor = System.Drawing.SystemColors.ControlText;
		this.chkWeblogsCom.Location = new System.Drawing.Point(240, 8);
		this.chkWeblogsCom.Name = "chkWeblogsCom";
		this.chkWeblogsCom.Size = new System.Drawing.Size(120, 24);
		this.chkWeblogsCom.TabIndex = 25;
		this.chkWeblogsCom.Text = "Ping Weblogs.com";
		this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPort.Location = new System.Drawing.Point(88, 152);
		this.txtPort.Name = "txtPort";
		this.txtPort.Size = new System.Drawing.Size(56, 21);
		this.txtPort.TabIndex = 23;
		this.txtPort.Text = "80";
		this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.Enabled = false;
		this.label6.Location = new System.Drawing.Point(34, 152);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(40, 23);
		this.label6.TabIndex = 24;
		this.label6.Text = "Por&t:";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label4.Location = new System.Drawing.Point(4, 184);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 23);
		this.label4.TabIndex = 22;
		this.label4.Text = "Item Count:";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.tbCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbCount.Location = new System.Drawing.Point(88, 184);
		this.tbCount.Name = "tbCount";
		this.tbCount.Size = new System.Drawing.Size(56, 21);
		this.tbCount.TabIndex = 21;
		this.tbCount.Text = "15";
		this.tbCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.tbUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbUrl.Location = new System.Drawing.Point(88, 72);
		this.tbUrl.Name = "tbUrl";
		this.tbUrl.Size = new System.Drawing.Size(272, 21);
		this.tbUrl.TabIndex = 19;
		this.tbUrl.Text = "";
		this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbPassword.Location = new System.Drawing.Point(88, 40);
		this.tbPassword.Name = "tbPassword";
		this.tbPassword.Size = new System.Drawing.Size(128, 21);
		this.tbPassword.TabIndex = 17;
		this.tbPassword.Text = "";
		this.tbUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.tbUserName.Location = new System.Drawing.Point(88, 8);
		this.tbUserName.Name = "tbUserName";
		this.tbUserName.Size = new System.Drawing.Size(128, 21);
		this.tbUserName.TabIndex = 15;
		this.tbUserName.Text = "";
		this.label3.Location = new System.Drawing.Point(10, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 23);
		this.label3.TabIndex = 20;
		this.label3.Text = "Host:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label2.Location = new System.Drawing.Point(10, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(64, 23);
		this.label2.TabIndex = 18;
		this.label2.Text = "Password:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label1.Location = new System.Drawing.Point(10, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 23);
		this.label1.TabIndex = 16;
		this.label1.Text = "UserName:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPage.Location = new System.Drawing.Point(88, 120);
		this.txtPage.Name = "txtPage";
		this.txtPage.Size = new System.Drawing.Size(272, 21);
		this.txtPage.TabIndex = 26;
		this.txtPage.Text = "";
		this.label5.Enabled = false;
		this.label5.Location = new System.Drawing.Point(26, 120);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 23);
		this.label5.TabIndex = 27;
		this.label5.Text = "Page:";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.chkPingBloDotGs.Enabled = false;
		this.chkPingBloDotGs.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkPingBloDotGs.Location = new System.Drawing.Point(240, 32);
		this.chkPingBloDotGs.Name = "chkPingBloDotGs";
		this.chkPingBloDotGs.TabIndex = 28;
		this.chkPingBloDotGs.Text = "Ping Blo.gs";
		this.label7.Location = new System.Drawing.Point(8, 96);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(352, 16);
		this.label7.TabIndex = 29;
		this.label7.Text = "[e.g.: http://weblogs.asp.net]";
		this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label8.Location = new System.Drawing.Point(152, 144);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(208, 16);
		this.label8.TabIndex = 30;
		this.label8.Text = "[e.g. /BLOG/services/metablogapi.aspx]";
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.chkPingBloDotGs);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.txtPage);
		base.Controls.Add(this.chkWeblogsCom);
		base.Controls.Add(this.txtPort);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.tbCount);
		base.Controls.Add(this.tbUrl);
		base.Controls.Add(this.tbPassword);
		base.Controls.Add(this.tbUserName);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "SiteConfiguration";
		base.Size = new System.Drawing.Size(368, 216);
		base.ResumeLayout(false);
	}
}
