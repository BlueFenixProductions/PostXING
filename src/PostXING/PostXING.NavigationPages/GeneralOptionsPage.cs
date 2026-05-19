using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class GeneralOptionsPage : OptionsPageBase
{
	private CheckBox chkAutoCreateNewPost;

	private CheckBox chkAutoSelectForCrossPosting;

	private CheckBox chkMinimizeToNotificationArea;

	private CheckBox chkSuppressSplashPage;

	private CheckBox chkAutoSaveEnabled;

	private Label label1;

	private MaskedTextBox txtAutoSaveIncrementInMinutes;

	private GroupBox groupBox1;

	private CheckBox chkRemoveDraftsOnPost;

	private Container components;

	public GeneralOptionsPage()
	{
		InitializeComponent();
	}

	public override void ApplySettings()
	{
		AppManager.Preferences.AutoCreateNewPost = chkAutoCreateNewPost.Checked;
		AppManager.Preferences.MinimizeToNotificationArea = chkMinimizeToNotificationArea.Checked;
		AppManager.Preferences.SuppressSplashPage = chkSuppressSplashPage.Checked;
		AppManager.Preferences.AutoSaveEnabled = chkAutoSaveEnabled.Checked;
		int result = 2;
		if (int.TryParse(txtAutoSaveIncrementInMinutes.Text, out result))
		{
			if (result < 1)
			{
				result = 1;
			}
			if (result > 9)
			{
				result = 9;
			}
		}
		AppManager.Preferences.AutoSaveIntervalInMinutes = result;
		AppManager.ApplyPreferences();
		_dialog.CurrentBlog.AutoSelectForCrossPosting = chkAutoSelectForCrossPosting.Checked;
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		ApplySettings();
		base.OnPageLeave(e);
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		base.OnPageEnter(e);
		chkAutoCreateNewPost.Checked = AppManager.Preferences.AutoCreateNewPost;
		chkMinimizeToNotificationArea.Checked = AppManager.Preferences.MinimizeToNotificationArea;
		chkSuppressSplashPage.Checked = AppManager.Preferences.SuppressSplashPage;
		chkAutoSelectForCrossPosting.Checked = _dialog.CurrentBlog.AutoSelectForCrossPosting;
		chkAutoSaveEnabled.Checked = AppManager.Preferences.AutoSaveEnabled;
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
		this.chkAutoCreateNewPost = new System.Windows.Forms.CheckBox();
		this.chkAutoSelectForCrossPosting = new System.Windows.Forms.CheckBox();
		this.chkMinimizeToNotificationArea = new System.Windows.Forms.CheckBox();
		this.chkSuppressSplashPage = new System.Windows.Forms.CheckBox();
		this.chkAutoSaveEnabled = new System.Windows.Forms.CheckBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAutoSaveIncrementInMinutes = new System.Windows.Forms.MaskedTextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkRemoveDraftsOnPost = new System.Windows.Forms.CheckBox();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.chkAutoCreateNewPost.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkAutoCreateNewPost.Location = new System.Drawing.Point(16, 3);
		this.chkAutoCreateNewPost.Name = "chkAutoCreateNewPost";
		this.chkAutoCreateNewPost.Size = new System.Drawing.Size(128, 24);
		this.chkAutoCreateNewPost.TabIndex = 6;
		this.chkAutoCreateNewPost.Text = "Auto Create New Post";
		this.chkAutoSelectForCrossPosting.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkAutoSelectForCrossPosting.Location = new System.Drawing.Point(16, 33);
		this.chkAutoSelectForCrossPosting.Name = "chkAutoSelectForCrossPosting";
		this.chkAutoSelectForCrossPosting.Size = new System.Drawing.Size(168, 24);
		this.chkAutoSelectForCrossPosting.TabIndex = 7;
		this.chkAutoSelectForCrossPosting.Text = "Auto Select for Cross Posting";
		this.chkMinimizeToNotificationArea.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkMinimizeToNotificationArea.Location = new System.Drawing.Point(16, 63);
		this.chkMinimizeToNotificationArea.Name = "chkMinimizeToNotificationArea";
		this.chkMinimizeToNotificationArea.Size = new System.Drawing.Size(168, 24);
		this.chkMinimizeToNotificationArea.TabIndex = 8;
		this.chkMinimizeToNotificationArea.Text = "Minimize to Notification Area";
		this.chkSuppressSplashPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkSuppressSplashPage.Location = new System.Drawing.Point(16, 93);
		this.chkSuppressSplashPage.Name = "chkSuppressSplashPage";
		this.chkSuppressSplashPage.Size = new System.Drawing.Size(168, 24);
		this.chkSuppressSplashPage.TabIndex = 9;
		this.chkSuppressSplashPage.Text = "Suppress Splash Page";
		this.chkAutoSaveEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.chkAutoSaveEnabled.Location = new System.Drawing.Point(16, 123);
		this.chkAutoSaveEnabled.Name = "chkAutoSaveEnabled";
		this.chkAutoSaveEnabled.Size = new System.Drawing.Size(191, 24);
		this.chkAutoSaveEnabled.TabIndex = 10;
		this.chkAutoSaveEnabled.Text = "Auto Save drafts of my posts every";
		this.chkAutoSaveEnabled.CheckedChanged += new System.EventHandler(chkAutoSaveEnabled_CheckedChanged);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(249, 128);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(44, 13);
		this.label1.TabIndex = 12;
		this.label1.Text = "minutes";
		this.txtAutoSaveIncrementInMinutes.Enabled = false;
		this.txtAutoSaveIncrementInMinutes.Location = new System.Drawing.Point(211, 125);
		this.txtAutoSaveIncrementInMinutes.Mask = "0";
		this.txtAutoSaveIncrementInMinutes.Name = "txtAutoSaveIncrementInMinutes";
		this.txtAutoSaveIncrementInMinutes.Size = new System.Drawing.Size(32, 21);
		this.txtAutoSaveIncrementInMinutes.TabIndex = 13;
		this.txtAutoSaveIncrementInMinutes.Text = "2";
		this.groupBox1.Controls.Add(this.chkRemoveDraftsOnPost);
		this.groupBox1.Enabled = false;
		this.groupBox1.Location = new System.Drawing.Point(16, 153);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(271, 130);
		this.groupBox1.TabIndex = 14;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Draft Handling";
		this.chkRemoveDraftsOnPost.AutoSize = true;
		this.chkRemoveDraftsOnPost.Location = new System.Drawing.Point(6, 20);
		this.chkRemoveDraftsOnPost.Name = "chkRemoveDraftsOnPost";
		this.chkRemoveDraftsOnPost.Size = new System.Drawing.Size(157, 17);
		this.chkRemoveDraftsOnPost.TabIndex = 0;
		this.chkRemoveDraftsOnPost.Text = "Remove drafts when I post";
		this.chkRemoveDraftsOnPost.UseVisualStyleBackColor = true;
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.txtAutoSaveIncrementInMinutes);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.chkAutoSaveEnabled);
		base.Controls.Add(this.chkSuppressSplashPage);
		base.Controls.Add(this.chkMinimizeToNotificationArea);
		base.Controls.Add(this.chkAutoSelectForCrossPosting);
		base.Controls.Add(this.chkAutoCreateNewPost);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "GeneralOptionsPage";
		base.Size = new System.Drawing.Size(315, 307);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void chkAutoSaveEnabled_CheckedChanged(object sender, EventArgs e)
	{
		GroupBox groupBox = groupBox1;
		bool enabled = (txtAutoSaveIncrementInMinutes.Enabled = chkAutoSaveEnabled.Checked);
		groupBox.Enabled = enabled;
	}
}
