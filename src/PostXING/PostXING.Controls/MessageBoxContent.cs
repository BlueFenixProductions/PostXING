using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls;

public class MessageBoxContent : UserControl
{
	private Panel panel1;

	private Label lblMessage;

	private PictureBox pbxIcon;

	private Panel pnl2Buttons;

	private Button button1;

	private Button button2;

	private Panel pnl3Buttons;

	private Button button3;

	private Button button4;

	private Button button5;

	private Panel pnlOk;

	private Button button6;

	private HeaderStrip headerStrip1;

	private ToolStripButton toolStripButton1;

	private ToolStripLabel lblTitle;

	private Container components;

	private MessageBoxButtons _buttons;

	public string Title
	{
		get
		{
			return lblTitle.Text;
		}
		set
		{
			lblTitle.Text = value;
		}
	}

	public string MessageText
	{
		get
		{
			return lblMessage.Text;
		}
		set
		{
			lblMessage.Text = value;
		}
	}

	public MessageBoxButtons Buttons
	{
		get
		{
			return _buttons;
		}
		set
		{
			_buttons = value;
			switch (value)
			{
			default:
			{
				Panel panel6 = pnl2Buttons;
				bool visible6 = (pnl3Buttons.Visible = false);
				panel6.Visible = visible6;
				pnlOk.Visible = true;
				break;
			}
			case MessageBoxButtons.AbortRetryIgnore:
			{
				Panel panel5 = pnl2Buttons;
				bool visible5 = (pnlOk.Visible = false);
				panel5.Visible = visible5;
				pnl3Buttons.Visible = true;
				button3.Text = "Abort";
				button3.DialogResult = DialogResult.Abort;
				button4.Text = "Retry";
				button4.DialogResult = DialogResult.Retry;
				button5.Text = "Ignore";
				button5.DialogResult = DialogResult.Ignore;
				break;
			}
			case MessageBoxButtons.OKCancel:
			{
				Panel panel4 = pnl3Buttons;
				bool visible4 = (pnlOk.Visible = false);
				panel4.Visible = visible4;
				pnl2Buttons.Visible = true;
				button1.Text = "OK";
				button1.DialogResult = DialogResult.OK;
				button2.Text = "Cancel";
				button2.DialogResult = DialogResult.Cancel;
				break;
			}
			case MessageBoxButtons.RetryCancel:
			{
				Panel panel3 = pnl3Buttons;
				bool visible3 = (pnlOk.Visible = false);
				panel3.Visible = visible3;
				pnl2Buttons.Visible = true;
				button1.Text = "Retry";
				button1.DialogResult = DialogResult.Retry;
				button2.Text = "Cancel";
				button2.DialogResult = DialogResult.Cancel;
				break;
			}
			case MessageBoxButtons.YesNo:
			{
				Panel panel2 = pnl3Buttons;
				bool visible2 = (pnlOk.Visible = false);
				panel2.Visible = visible2;
				pnl2Buttons.Visible = true;
				button1.Text = "Yes";
				button2.Text = "No";
				button1.DialogResult = DialogResult.Yes;
				button2.DialogResult = DialogResult.No;
				break;
			}
			case MessageBoxButtons.YesNoCancel:
			{
				Panel panel = pnl2Buttons;
				bool visible = (pnlOk.Visible = false);
				panel.Visible = visible;
				pnl3Buttons.Visible = true;
				button3.Text = "Yes";
				button3.DialogResult = DialogResult.Yes;
				button4.Text = "No";
				button4.DialogResult = DialogResult.No;
				button5.Text = "Cancel";
				button5.DialogResult = DialogResult.Cancel;
				break;
			}
			}
		}
	}

	public event CancelEventHandler Closing;

	public MessageBoxContent()
	{
		InitializeComponent();
		button1.Click += button1_Click;
		button2.Click += button1_Click;
		button3.Click += button1_Click;
		button4.Click += button1_Click;
		button5.Click += button1_Click;
		button6.Click += button1_Click;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Controls.MessageBoxContent));
		this.panel1 = new System.Windows.Forms.Panel();
		this.pnlOk = new System.Windows.Forms.Panel();
		this.button6 = new System.Windows.Forms.Button();
		this.pnl2Buttons = new System.Windows.Forms.Panel();
		this.button2 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.pnl3Buttons = new System.Windows.Forms.Panel();
		this.button5 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.pbxIcon = new System.Windows.Forms.PictureBox();
		this.lblMessage = new System.Windows.Forms.Label();
		this.headerStrip1 = new PostXING.Controls.HeaderStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.lblTitle = new System.Windows.Forms.ToolStripLabel();
		this.panel1.SuspendLayout();
		this.pnlOk.SuspendLayout();
		this.pnl2Buttons.SuspendLayout();
		this.pnl3Buttons.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pbxIcon).BeginInit();
		this.headerStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.panel1.Controls.Add(this.lblMessage);
		this.panel1.Controls.Add(this.headerStrip1);
		this.panel1.Controls.Add(this.pnlOk);
		this.panel1.Controls.Add(this.pnl2Buttons);
		this.panel1.Controls.Add(this.pnl3Buttons);
		this.panel1.Controls.Add(this.pbxIcon);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(424, 328);
		this.panel1.TabIndex = 0;
		this.pnlOk.Controls.Add(this.button6);
		this.pnlOk.Location = new System.Drawing.Point(40, 248);
		this.pnlOk.Name = "pnlOk";
		this.pnlOk.Size = new System.Drawing.Size(344, 24);
		this.pnlOk.TabIndex = 4;
		this.pnlOk.Visible = false;
		this.button6.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button6.Location = new System.Drawing.Point(128, 0);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(75, 23);
		this.button6.TabIndex = 2;
		this.button6.Text = "OK";
		this.pnl2Buttons.Controls.Add(this.button2);
		this.pnl2Buttons.Controls.Add(this.button1);
		this.pnl2Buttons.Location = new System.Drawing.Point(40, 248);
		this.pnl2Buttons.Name = "pnl2Buttons";
		this.pnl2Buttons.Size = new System.Drawing.Size(344, 24);
		this.pnl2Buttons.TabIndex = 2;
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button2.Location = new System.Drawing.Point(184, 0);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 1;
		this.button2.Text = "button2";
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button1.Location = new System.Drawing.Point(72, 0);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 0;
		this.button1.Text = "button1";
		this.pnl3Buttons.Controls.Add(this.button5);
		this.pnl3Buttons.Controls.Add(this.button3);
		this.pnl3Buttons.Controls.Add(this.button4);
		this.pnl3Buttons.Location = new System.Drawing.Point(40, 248);
		this.pnl3Buttons.Name = "pnl3Buttons";
		this.pnl3Buttons.Size = new System.Drawing.Size(344, 24);
		this.pnl3Buttons.TabIndex = 3;
		this.pnl3Buttons.Visible = false;
		this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button5.Location = new System.Drawing.Point(128, 0);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(75, 23);
		this.button5.TabIndex = 2;
		this.button5.Text = "button1";
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button3.Location = new System.Drawing.Point(232, 0);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(75, 23);
		this.button3.TabIndex = 1;
		this.button3.Text = "button2";
		this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.button4.Location = new System.Drawing.Point(24, 0);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(75, 23);
		this.button4.TabIndex = 0;
		this.button4.Text = "button1";
		this.pbxIcon.Image = (System.Drawing.Image)resources.GetObject("pbxIcon.Image");
		this.pbxIcon.Location = new System.Drawing.Point(0, 240);
		this.pbxIcon.Name = "pbxIcon";
		this.pbxIcon.Size = new System.Drawing.Size(40, 32);
		this.pbxIcon.TabIndex = 1;
		this.pbxIcon.TabStop = false;
		this.lblMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblMessage.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMessage.Location = new System.Drawing.Point(30, 44);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(354, 193);
		this.lblMessage.TabIndex = 0;
		this.lblMessage.Text = "label1";
		this.headerStrip1.AutoSize = false;
		this.headerStrip1.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Bold);
		this.headerStrip1.ForeColor = System.Drawing.Color.White;
		this.headerStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.headerStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton1, this.lblTitle });
		this.headerStrip1.Location = new System.Drawing.Point(0, 0);
		this.headerStrip1.Name = "headerStrip1";
		this.headerStrip1.Size = new System.Drawing.Size(424, 25);
		this.headerStrip1.TabIndex = 1;
		this.headerStrip1.Text = "headerStrip1";
		this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
		this.toolStripButton1.Text = "toolStripButton1";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(41, 22);
		this.lblTitle.Text = "Title";
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "MessageBoxContent";
		base.Size = new System.Drawing.Size(424, 328);
		this.panel1.ResumeLayout(false);
		this.pnlOk.ResumeLayout(false);
		this.pnl2Buttons.ResumeLayout(false);
		this.pnl3Buttons.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pbxIcon).EndInit();
		this.headerStrip1.ResumeLayout(false);
		this.headerStrip1.PerformLayout();
		base.ResumeLayout(false);
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (this.Closing != null)
		{
			this.Closing(this, new CancelEventArgs());
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		button1_Click(sender, e);
	}
}
