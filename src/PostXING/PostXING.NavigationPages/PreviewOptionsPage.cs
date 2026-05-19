using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class PreviewOptionsPage : OptionsPageBase
{
	private XPTextBox txtPreviewTemplate;

	private Label label2;

	private Label label1;

	private Container components;

	public PreviewOptionsPage()
	{
		InitializeComponent();
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		base.OnPageEnter(e);
		if (_dialog.CurrentBlog != null)
		{
			txtPreviewTemplate.Text = _dialog.CurrentBlog.Options.PreviewTemplate;
		}
	}

	public override void ApplySettings()
	{
		_dialog.CurrentBlog.Options.PreviewTemplate = txtPreviewTemplate.Text;
	}

	public override void OnPageLeave(PageEventArgs e)
	{
		ApplySettings();
		base.OnPageLeave(e);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.PreviewOptionsPage));
		this.txtPreviewTemplate = new PostXING.Controls.XPTextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.txtPreviewTemplate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtPreviewTemplate.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPreviewTemplate.Location = new System.Drawing.Point(8, 48);
		this.txtPreviewTemplate.Multiline = true;
		this.txtPreviewTemplate.Name = "txtPreviewTemplate";
		this.txtPreviewTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
		this.txtPreviewTemplate.Size = new System.Drawing.Size(280, 176);
		this.txtPreviewTemplate.TabIndex = 6;
		this.txtPreviewTemplate.Text = resources.GetString("txtPreviewTemplate.Text");
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.label2.Location = new System.Drawing.Point(24, 232);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(256, 32);
		this.label2.TabIndex = 8;
		this.label2.Text = "Place [Post Title] where you want the title and [Post Body] where you want the post body";
		this.label1.Location = new System.Drawing.Point(8, 33);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(128, 23);
		this.label1.TabIndex = 7;
		this.label1.Text = "Preview Template:";
		base.Controls.Add(this.txtPreviewTemplate);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Name = "PreviewOptionsPage";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
