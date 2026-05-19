using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;

namespace PostXING;

public class MediaConfiguration : UserControl
{
	private XPTextBox txtHtmlTag;

	private XPTextBox txtCurrentlyPlaying;

	private XPTextBox txtNothingPlaying;

	private Label label1;

	private Label label2;

	private Label label3;

	private Container components;

	public string HtmlTagText
	{
		get
		{
			return txtHtmlTag.Text;
		}
		set
		{
			txtHtmlTag.Text = value;
		}
	}

	public string CurrentlyPlayingText
	{
		get
		{
			return txtCurrentlyPlaying.Text;
		}
		set
		{
			txtCurrentlyPlaying.Text = value;
		}
	}

	public string NothingPlayingText
	{
		get
		{
			return txtNothingPlaying.Text;
		}
		set
		{
			txtNothingPlaying.Text = value;
		}
	}

	public MediaConfiguration()
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
		this.txtHtmlTag = new PostXING.Controls.XPTextBox();
		this.txtCurrentlyPlaying = new PostXING.Controls.XPTextBox();
		this.txtNothingPlaying = new PostXING.Controls.XPTextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.txtHtmlTag.Location = new System.Drawing.Point(72, 8);
		this.txtHtmlTag.Name = "txtHtmlTag";
		this.txtHtmlTag.Size = new System.Drawing.Size(216, 21);
		this.txtHtmlTag.TabIndex = 0;
		this.txtHtmlTag.Text = "<p class='media'>";
		this.txtCurrentlyPlaying.Location = new System.Drawing.Point(72, 48);
		this.txtCurrentlyPlaying.Multiline = true;
		this.txtCurrentlyPlaying.Name = "txtCurrentlyPlaying";
		this.txtCurrentlyPlaying.Size = new System.Drawing.Size(216, 32);
		this.txtCurrentlyPlaying.TabIndex = 1;
		this.txtCurrentlyPlaying.Text = "[ Currently Playing : {title} - {artist} - {album} ({duration}) ]";
		this.txtNothingPlaying.Location = new System.Drawing.Point(72, 96);
		this.txtNothingPlaying.Name = "txtNothingPlaying";
		this.txtNothingPlaying.Size = new System.Drawing.Size(216, 21);
		this.txtNothingPlaying.TabIndex = 2;
		this.txtNothingPlaying.Text = "[ Nothing Playing. ]";
		this.label1.Location = new System.Drawing.Point(7, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 23);
		this.label1.TabIndex = 3;
		this.label1.Text = "Html tag:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label2.Location = new System.Drawing.Point(8, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(56, 40);
		this.label2.TabIndex = 4;
		this.label2.Text = "Currently Playing Template:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label3.Location = new System.Drawing.Point(8, 88);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 32);
		this.label3.TabIndex = 5;
		this.label3.Text = "Nothing Playing:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.txtNothingPlaying);
		base.Controls.Add(this.txtCurrentlyPlaying);
		base.Controls.Add(this.txtHtmlTag);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "MediaConfiguration";
		base.Size = new System.Drawing.Size(296, 205);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
