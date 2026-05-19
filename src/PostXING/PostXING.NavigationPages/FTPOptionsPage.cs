using System;
using System.ComponentModel;
using System.Drawing;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class FTPOptionsPage : OptionsPageBase
{
	private FTPConfiguration ftpConfiguration1;

	private Container components;

	public FTPOptionsPage()
	{
		InitializeComponent();
	}

	public override void ApplySettings()
	{
		if (!string.IsNullOrEmpty(ftpConfiguration1.Host))
		{
			_dialog.CurrentBlog.FTPInfo.Target = new Uri(new Uri($"{Uri.UriSchemeFtp}://{ftpConfiguration1.Host}:{ftpConfiguration1.Port}"), ftpConfiguration1.RemotePath);
			_dialog.CurrentBlog.FTPInfo.Password = ftpConfiguration1.Password;
			_dialog.CurrentBlog.FTPInfo.UserName = ftpConfiguration1.UserName;
			_dialog.CurrentBlog.FTPInfo.BaseUrl = ftpConfiguration1.BaseUrl;
			_dialog.CurrentBlog.FTPInfo.Passive = ftpConfiguration1.Passive;
		}
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		if (!string.IsNullOrEmpty(ftpConfiguration1.Host) && !string.IsNullOrEmpty(ftpConfiguration1.Port) && !string.IsNullOrEmpty(ftpConfiguration1.RemotePath))
		{
			ApplySettings();
		}
		base.OnPageLeave(e);
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		base.OnPageEnter(e);
		if (_dialog.CurrentBlog.FTPInfo.Target != null)
		{
			ftpConfiguration1.Host = _dialog.CurrentBlog.FTPInfo.Target.Host;
			ftpConfiguration1.Port = _dialog.CurrentBlog.FTPInfo.Target.Port.ToString();
			ftpConfiguration1.RemotePath = _dialog.CurrentBlog.FTPInfo.Target.AbsolutePath;
			ftpConfiguration1.BaseUrl = _dialog.CurrentBlog.FTPInfo.BaseUrl;
			ftpConfiguration1.UserName = _dialog.CurrentBlog.FTPInfo.UserName;
			ftpConfiguration1.Password = _dialog.CurrentBlog.FTPInfo.Password;
			ftpConfiguration1.Passive = _dialog.CurrentBlog.FTPInfo.Passive;
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
		this.ftpConfiguration1 = new PostXING.FTPConfiguration();
		base.SuspendLayout();
		this.ftpConfiguration1.BaseUrl = "";
		this.ftpConfiguration1.Host = "";
		this.ftpConfiguration1.Location = new System.Drawing.Point(4, 41);
		this.ftpConfiguration1.Name = "ftpConfiguration1";
		this.ftpConfiguration1.Passive = false;
		this.ftpConfiguration1.Password = "";
		this.ftpConfiguration1.Port = "21";
		this.ftpConfiguration1.RemotePath = "";
		this.ftpConfiguration1.Size = new System.Drawing.Size(286, 224);
		this.ftpConfiguration1.TabIndex = 0;
		this.ftpConfiguration1.UserName = "";
		base.Controls.Add(this.ftpConfiguration1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "FTPOptionsPage";
		base.Size = new System.Drawing.Size(304, 300);
		base.ResumeLayout(false);
	}
}
