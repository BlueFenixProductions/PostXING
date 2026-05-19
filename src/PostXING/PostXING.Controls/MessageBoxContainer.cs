using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls;

public class MessageBoxContainer : UserControl
{
	private GradientPanel gradientPanel1;

	private MessageBoxContent messageBoxContent1;

	private Container components;

	public event CancelEventHandler Closing;

	public MessageBoxContainer()
	{
		InitializeComponent();
		messageBoxContent1.Closing += messageBoxContent1_Closing;
	}

	public void ShowMessage(string text)
	{
		ShowMessage(text, string.Empty);
	}

	public void ShowMessage(string text, string title)
	{
		ShowMessage(text, title, MessageBoxButtons.OK);
	}

	public void ShowMessage(string text, string title, MessageBoxButtons buttons)
	{
		messageBoxContent1.Title = title;
		messageBoxContent1.MessageText = text;
		messageBoxContent1.Buttons = buttons;
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
		this.gradientPanel1 = new PostXING.Controls.GradientPanel();
		this.messageBoxContent1 = new PostXING.Controls.MessageBoxContent();
		this.gradientPanel1.SuspendLayout();
		base.SuspendLayout();
		this.gradientPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.gradientPanel1.Controls.Add(this.messageBoxContent1);
		this.gradientPanel1.GradientColor = System.Drawing.SystemColors.ControlLightLight;
		this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
		this.gradientPanel1.Name = "gradientPanel1";
		this.gradientPanel1.Rotation = 45f;
		this.gradientPanel1.Size = new System.Drawing.Size(800, 535);
		this.gradientPanel1.TabIndex = 0;
		this.messageBoxContent1.Buttons = System.Windows.Forms.MessageBoxButtons.OK;
		this.messageBoxContent1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.messageBoxContent1.Location = new System.Drawing.Point(176, 96);
		this.messageBoxContent1.MessageText = "label1";
		this.messageBoxContent1.Name = "messageBoxContent1";
		this.messageBoxContent1.Size = new System.Drawing.Size(424, 328);
		this.messageBoxContent1.TabIndex = 0;
		this.messageBoxContent1.Title = "tabbedDocument1";
		base.Controls.Add(this.gradientPanel1);
		base.Name = "MessageBoxContainer";
		base.Size = new System.Drawing.Size(800, 515);
		this.gradientPanel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	private void messageBoxContent1_Closing(object sender, CancelEventArgs e)
	{
		if (this.Closing != null)
		{
			this.Closing(this, new CancelEventArgs());
		}
	}
}
