using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using PostXING.Controls;

namespace PostXING.Dialogs;

public class MessageBoxDialog : Form
{
	private GradientPanel gradientPanel1;

	private MessageBoxContent messageBoxContent2;

	private Container components;

	public MessageBoxDialog()
	{
		InitializeComponent();
		SetTopLevel(value: false);
		messageBoxContent2.Closing += messageBoxContent1_Closing;
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
		messageBoxContent2.Title = title;
		messageBoxContent2.MessageText = text;
		messageBoxContent2.Buttons = buttons;
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
		System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(PostXING.Dialogs.MessageBoxDialog));
		this.gradientPanel1 = new PostXING.Controls.GradientPanel();
		this.messageBoxContent2 = new PostXING.Controls.MessageBoxContent();
		this.gradientPanel1.SuspendLayout();
		base.SuspendLayout();
		this.gradientPanel1.Controls.Add(this.messageBoxContent2);
		this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.gradientPanel1.GradientColor = System.Drawing.SystemColors.ControlLightLight;
		this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
		this.gradientPanel1.Name = "gradientPanel1";
		this.gradientPanel1.Rotation = 45f;
		this.gradientPanel1.Size = new System.Drawing.Size(800, 515);
		this.gradientPanel1.TabIndex = 0;
		this.messageBoxContent2.Buttons = System.Windows.Forms.MessageBoxButtons.OK;
		this.messageBoxContent2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.messageBoxContent2.Location = new System.Drawing.Point(168, 96);
		this.messageBoxContent2.MessageText = "label1";
		this.messageBoxContent2.Name = "messageBoxContent2";
		this.messageBoxContent2.Size = new System.Drawing.Size(424, 328);
		this.messageBoxContent2.TabIndex = 0;
		this.messageBoxContent2.Title = "tabbedDocument1";
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		base.ClientSize = new System.Drawing.Size(800, 515);
		base.Controls.Add(this.gradientPanel1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resourceManager.GetObject("$this.Icon");
		base.Name = "MessageBoxDialog";
		base.Opacity = 0.75;
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "MessageBoxDialog";
		this.gradientPanel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	private void messageBoxContent1_Closing(object sender, CancelEventArgs e)
	{
		Close();
	}
}
