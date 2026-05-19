using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class MediaOptionsPage : OptionsPageBase
{
	private CheckBox chkIncludeMediaInfo;

	private MediaConfiguration mediaConfiguration1;

	private Container components;

	public MediaOptionsPage()
	{
		InitializeComponent();
	}

	public override void ApplySettings()
	{
		_dialog.CurrentBlog.Options.IncludeMediaFormatter = chkIncludeMediaInfo.Checked;
		_dialog.CurrentBlog.Options.NothingPlayingTemplate = mediaConfiguration1.NothingPlayingText;
		_dialog.CurrentBlog.Options.CurrentlyPlayingTemplate = mediaConfiguration1.CurrentlyPlayingText;
		_dialog.CurrentBlog.Options.HtmlTag = mediaConfiguration1.HtmlTagText;
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		ApplySettings();
		base.OnPageLeave(e);
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		base.OnPageEnter(e);
		chkIncludeMediaInfo.Checked = _dialog.CurrentBlog.Options.IncludeMediaFormatter;
		mediaConfiguration1.NothingPlayingText = _dialog.CurrentBlog.Options.NothingPlayingTemplate;
		mediaConfiguration1.CurrentlyPlayingText = _dialog.CurrentBlog.Options.CurrentlyPlayingTemplate;
		mediaConfiguration1.HtmlTagText = _dialog.CurrentBlog.Options.HtmlTag;
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
		this.chkIncludeMediaInfo = new System.Windows.Forms.CheckBox();
		this.mediaConfiguration1 = new PostXING.MediaConfiguration();
		base.SuspendLayout();
		this.chkIncludeMediaInfo.AutoSize = true;
		this.chkIncludeMediaInfo.Location = new System.Drawing.Point(76, 180);
		this.chkIncludeMediaInfo.Name = "chkIncludeMediaInfo";
		this.chkIncludeMediaInfo.Size = new System.Drawing.Size(197, 17);
		this.chkIncludeMediaInfo.TabIndex = 7;
		this.chkIncludeMediaInfo.Text = "Include Windows Media Information";
		this.chkIncludeMediaInfo.UseVisualStyleBackColor = true;
		this.chkIncludeMediaInfo.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
		this.mediaConfiguration1.CurrentlyPlayingText = "[ Currently Playing : {title} - {artist} - {album} ({duration}) ]";
		this.mediaConfiguration1.Enabled = false;
		this.mediaConfiguration1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.mediaConfiguration1.HtmlTagText = "<p class='media'>";
		this.mediaConfiguration1.Location = new System.Drawing.Point(4, 46);
		this.mediaConfiguration1.Name = "mediaConfiguration1";
		this.mediaConfiguration1.NothingPlayingText = "[ Nothing Playing. ]";
		this.mediaConfiguration1.Size = new System.Drawing.Size(296, 128);
		this.mediaConfiguration1.TabIndex = 8;
		base.Controls.Add(this.mediaConfiguration1);
		base.Controls.Add(this.chkIncludeMediaInfo);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "MediaOptionsPage";
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		mediaConfiguration1.Enabled = chkIncludeMediaInfo.Checked;
	}
}
