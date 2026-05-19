using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;

namespace PostXING;

public class ProgressStatusBar : UserControl
{
	private StatusBar statusBar1;

	private StatusBarPanel pnlMessage;

	private StatusBarPanel pnlIcon;

	private StatusBarPanel pnlProgressBar;

	private SmoothProgressBar smoothProgressBar1;

	private Container components;

	public StatusBarPanel Message => pnlMessage;

	public SmoothProgressBar Progress => smoothProgressBar1;

	public Color ProgressBarColor
	{
		get
		{
			return smoothProgressBar1.ProgressBarColor;
		}
		set
		{
			smoothProgressBar1.ProgressBarColor = value;
		}
	}

	public Icon Icon
	{
		get
		{
			return pnlIcon.Icon;
		}
		set
		{
			pnlIcon.Icon = value;
			statusBar1.Refresh();
		}
	}

	public ProgressStatusBar()
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
		this.statusBar1 = new System.Windows.Forms.StatusBar();
		this.pnlMessage = new System.Windows.Forms.StatusBarPanel();
		this.pnlIcon = new System.Windows.Forms.StatusBarPanel();
		this.pnlProgressBar = new System.Windows.Forms.StatusBarPanel();
		this.smoothProgressBar1 = new PostXING.Controls.SmoothProgressBar();
		((System.ComponentModel.ISupportInitialize)this.pnlMessage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pnlIcon).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pnlProgressBar).BeginInit();
		base.SuspendLayout();
		this.statusBar1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.statusBar1.Location = new System.Drawing.Point(0, 2);
		this.statusBar1.Name = "statusBar1";
		this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[3] { this.pnlMessage, this.pnlIcon, this.pnlProgressBar });
		this.statusBar1.ShowPanels = true;
		this.statusBar1.Size = new System.Drawing.Size(512, 22);
		this.statusBar1.TabIndex = 0;
		this.pnlMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
		this.pnlMessage.Width = 372;
		this.pnlIcon.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
		this.pnlIcon.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
		this.pnlIcon.Width = 24;
		this.pnlProgressBar.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
		this.smoothProgressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.smoothProgressBar1.Location = new System.Drawing.Point(398, 4);
		this.smoothProgressBar1.Maximum = 100;
		this.smoothProgressBar1.Minimum = 0;
		this.smoothProgressBar1.Name = "smoothProgressBar1";
		this.smoothProgressBar1.ProgressBarColor = System.Drawing.Color.Blue;
		this.smoothProgressBar1.Size = new System.Drawing.Size(98, 20);
		this.smoothProgressBar1.TabIndex = 1;
		this.smoothProgressBar1.Value = 0;
		base.Controls.Add(this.smoothProgressBar1);
		base.Controls.Add(this.statusBar1);
		base.Name = "ProgressStatusBar";
		base.Size = new System.Drawing.Size(512, 24);
		((System.ComponentModel.ISupportInitialize)this.pnlMessage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pnlIcon).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pnlProgressBar).EndInit();
		base.ResumeLayout(false);
	}
}
