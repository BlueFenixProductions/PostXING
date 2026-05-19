using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace PostXING;

public class StatusBarProgress : UserControl
{
	private System.Timers.Timer timer1;

	private ProgressStatusBar progressStatusBar1;

	private Container components;

	public StatusBarProgress()
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
		this.timer1 = new System.Timers.Timer();
		this.progressStatusBar1 = new PostXING.ProgressStatusBar();
		((System.ComponentModel.ISupportInitialize)this.timer1).BeginInit();
		base.SuspendLayout();
		this.timer1.Interval = 150.0;
		this.timer1.SynchronizingObject = this;
		this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
		this.progressStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.progressStatusBar1.Icon = null;
		this.progressStatusBar1.Location = new System.Drawing.Point(0, 0);
		this.progressStatusBar1.Name = "progressStatusBar1";
		this.progressStatusBar1.ProgressBarColor = System.Drawing.SystemColors.Highlight;
		this.progressStatusBar1.Size = new System.Drawing.Size(544, 24);
		this.progressStatusBar1.TabIndex = 0;
		base.Controls.Add(this.progressStatusBar1);
		base.Name = "StatusBarProgress";
		base.Size = new System.Drawing.Size(544, 24);
		((System.ComponentModel.ISupportInitialize)this.timer1).EndInit();
		base.ResumeLayout(false);
	}

	private void timer1_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (progressStatusBar1.Progress.Value == progressStatusBar1.Progress.Maximum)
		{
			progressStatusBar1.Progress.Value = 0;
		}
		progressStatusBar1.Progress.Value++;
	}

	public void StartProgress(string StatusBarMessage)
	{
		ResetProgress(StatusBarMessage);
		timer1.Enabled = true;
		timer1.Start();
	}

	public void StopProgress()
	{
		timer1.Stop();
		timer1.Enabled = false;
		for (int i = progressStatusBar1.Progress.Value; i < progressStatusBar1.Progress.Maximum; i++)
		{
			progressStatusBar1.Progress.Value = i;
			Thread.Sleep(5);
		}
		progressStatusBar1.Message.Text = "";
	}

	public void ResetProgress(string StatusBarMessage)
	{
		progressStatusBar1.Message.Text = StatusBarMessage;
		progressStatusBar1.Progress.Value = 0;
	}
}
