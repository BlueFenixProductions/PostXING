using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls.HtmlEditor;
using PostXING.Controls.HtmlEditor.Html;

namespace PostXING.NavigationPages;

public class EditorPage : MainViewPage
{
	private HtmlEditorSurface editor1;

	private IContainer components;

	public ToolStrip ButtonBar => editor1.ButtonBar;

	public ToolStrip PreviewBar => editor1.PreviewBar;

	public bool IsDirty => editor1.IsDirty;

	public EditorMode CurrentEditorMode
	{
		get
		{
			return editor1.CurrentEditorMode;
		}
		set
		{
			editor1.CurrentEditorMode = value;
		}
	}

	public string Html => editor1.Html;

	public HtmlControl DesignEditor => editor1.DesignEditor;

	public string PreviewTemplate
	{
		get
		{
			return editor1.PreviewTemplate;
		}
		set
		{
			editor1.PreviewTemplate = value;
		}
	}

	public HtmlEditorSurface EditorSurface => editor1;

	public EditorPage()
	{
		InitializeComponent();
		editor1.ApplicationName += Application.ProductVersion;
	}

	public void SetHtml(string value)
	{
		editor1.SetHtml(value);
	}

	internal string StripHTMLBody()
	{
		return editor1.StripHTMLBody();
	}

	public void ReverseRefreshTabSelection(EditorMode previousMode)
	{
		editor1.ReverseRefreshTabSelection(previousMode);
	}

	public void RefreshTabSelection(EditorMode previousMode)
	{
		editor1.RefreshTabSelection(previousMode);
	}

	public void RefreshDisplay()
	{
		editor1.RefreshDisplay();
	}

	public void RefreshDisplay(EditorMode previousMode)
	{
		editor1.RefreshDisplay(previousMode);
	}

	public new void SuspendLayout()
	{
		editor1.SuspendLayout();
	}

	public new void ResumeLayout(bool performLayout)
	{
		editor1.ResumeLayout(performLayout);
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
		this.editor1 = new PostXING.Controls.HtmlEditor.HtmlEditorSurface();
		this.SuspendLayout();
		this.editor1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.editor1.ApplicationName = "PostXING v";
		this.editor1.CurrentEditorMode = PostXING.Controls.HtmlEditor.EditorMode.Design;
		this.editor1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.editor1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.editor1.Location = new System.Drawing.Point(0, 0);
		this.editor1.Name = "editor1";
		this.editor1.PreviewTemplate = null;
		this.editor1.Size = new System.Drawing.Size(600, 408);
		this.editor1.TabIndex = 0;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.editor1);
		base.Name = "EditorPage";
		this.ResumeLayout(false);
	}

	internal void FocusHtmlView()
	{
		RefreshTabSelection(EditorMode.Design);
	}

	internal void FocusPreviewView()
	{
		RefreshTabSelection(EditorMode.Html);
	}

	internal void FocusXmlView()
	{
		RefreshTabSelection(EditorMode.Preview);
	}

	internal void FocusDesignView()
	{
		RefreshTabSelection(EditorMode.Xml);
	}
}
