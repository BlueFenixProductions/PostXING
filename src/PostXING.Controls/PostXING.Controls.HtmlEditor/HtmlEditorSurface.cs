using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CodeHtmler;
using Dotnetrix.Controls;
using PostXING.Controls.HtmlEditor.Html;

namespace PostXING.Controls.HtmlEditor;

public class HtmlEditorSurface : UserControl
{
	private IContainer components;

	private XPTextBox txtHtml;

	private HtmlControl hcXml;

	private DesignView designView1;

	private PreviewView previewView1;

	private ImageList imageList1;

	private TabControlExEx tabControlExEx1;

	private TabPageEX tabPageEX1;

	private TabPageEX tabPageEX2;

	private TabPageEX tabPageEX3;

	private TabPageEX tabPageEX4;

	private ToolStrip finder1;

	private ToolStripButton closeFinderToolStripButton;

	private ToolStripTextBox txtFindThis;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton findNextToolStripButton;

	private ToolStripButton findPreviousToolStripButton;

	private EditorMode _currentEditorMode;

	private string _applicationName;

	private string previewTemplate;

	public EditorMode CurrentEditorMode
	{
		get
		{
			return _currentEditorMode;
		}
		set
		{
			_currentEditorMode = value;
		}
	}

	public string ApplicationName
	{
		get
		{
			return _applicationName;
		}
		set
		{
			_applicationName = value;
		}
	}

	public string Html
	{
		get
		{
			if (CurrentEditorMode == EditorMode.Design)
			{
				return designView1.StripHTMLBody();
			}
			return txtHtml.Text;
		}
	}

	public string PreviewTemplate
	{
		get
		{
			return previewTemplate;
		}
		set
		{
			previewTemplate = value;
		}
	}

	public bool IsDirty => DesignEditor.IsDirty;

	public TextBox HtmlEditor => txtHtml;

	public HtmlControl DesignEditor => designView1.DesignEditor;

	public HtmlControl PreviewEditor => previewView1.PreviewEditor;

	public ToolStrip ButtonBar => designView1.ReBar;

	public ToolStrip PreviewBar => previewView1.PreBar;

	public bool CanPaste => CurrentEditorMode switch
	{
		EditorMode.Design => DesignEditor.CanPaste, 
		EditorMode.Html => Clipboard.ContainsText(), 
		EditorMode.Preview => false, 
		EditorMode.Xml => throw new NotImplementedException(), 
		_ => throw new InvalidOperationException("Unexpected editor mode"), 
	};

	public bool CanCopy => CurrentEditorMode switch
	{
		EditorMode.Design => DesignEditor.CanCopy, 
		EditorMode.Html => HtmlEditor.SelectionLength > 0, 
		EditorMode.Preview => PreviewEditor.CanCopy, 
		EditorMode.Xml => throw new NotImplementedException(), 
		_ => throw new InvalidOperationException("Unexpected editor mode"), 
	};

	public bool CanCut => CurrentEditorMode switch
	{
		EditorMode.Design => DesignEditor.CanCut, 
		EditorMode.Html => HtmlEditor.SelectionLength > 0, 
		EditorMode.Preview => false, 
		EditorMode.Xml => throw new NotImplementedException(), 
		_ => throw new InvalidOperationException("Unexpected editor mode"), 
	};

	public bool CanSelectAll => CurrentEditorMode switch
	{
		EditorMode.Design => DesignEditor.CanSelectAll, 
		EditorMode.Html => HtmlEditor.TextLength > 0, 
		EditorMode.Preview => PreviewEditor.CanSelectAll, 
		EditorMode.Xml => throw new NotImplementedException(), 
		_ => throw new InvalidOperationException("Unexpected editor mode"), 
	};

	public bool CanUndo => CurrentEditorMode switch
	{
		EditorMode.Design => DesignEditor.CanUndo, 
		EditorMode.Html => HtmlEditor.CanUndo, 
		EditorMode.Preview => false, 
		EditorMode.Xml => throw new NotImplementedException(), 
		_ => throw new InvalidOperationException("Unexpected editor mode"), 
	};

	public bool CanRedo => CurrentEditorMode switch
	{
		EditorMode.Design => DesignEditor.CanUndo, 
		EditorMode.Html => HtmlEditor.CanUndo, 
		EditorMode.Preview => false, 
		EditorMode.Xml => throw new NotImplementedException(), 
		_ => throw new InvalidOperationException("Unexpected editor mode"), 
	};

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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Controls.HtmlEditor.HtmlEditorSurface));
		this.imageList1 = new System.Windows.Forms.ImageList(this.components);
		this.finder1 = new System.Windows.Forms.ToolStrip();
		this.closeFinderToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.txtFindThis = new System.Windows.Forms.ToolStripTextBox();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.findNextToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.findPreviousToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.tabControlExEx1 = new PostXING.Controls.TabControlExEx();
		this.tabPageEX1 = new Dotnetrix.Controls.TabPageEX();
		this.designView1 = new PostXING.Controls.HtmlEditor.DesignView();
		this.tabPageEX2 = new Dotnetrix.Controls.TabPageEX();
		this.txtHtml = new PostXING.Controls.XPTextBox();
		this.tabPageEX3 = new Dotnetrix.Controls.TabPageEX();
		this.previewView1 = new PostXING.Controls.HtmlEditor.PreviewView();
		this.tabPageEX4 = new Dotnetrix.Controls.TabPageEX();
		this.hcXml = new PostXING.Controls.HtmlEditor.Html.HtmlControl();
		this.finder1.SuspendLayout();
		this.tabControlExEx1.SuspendLayout();
		this.tabPageEX1.SuspendLayout();
		this.tabPageEX2.SuspendLayout();
		this.tabPageEX3.SuspendLayout();
		this.tabPageEX4.SuspendLayout();
		base.SuspendLayout();
		this.imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
		this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList1.Images.SetKeyName(0, "Design16.ico");
		this.imageList1.Images.SetKeyName(1, "Html16.ico");
		this.imageList1.Images.SetKeyName(2, "Preview16.ico");
		this.imageList1.Images.SetKeyName(3, "xml16.ico");
		this.finder1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.finder1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.finder1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.closeFinderToolStripButton, this.txtFindThis, this.toolStripSeparator1, this.findNextToolStripButton, this.findPreviousToolStripButton });
		this.finder1.Location = new System.Drawing.Point(0, 343);
		this.finder1.Name = "finder1";
		this.finder1.Size = new System.Drawing.Size(610, 25);
		this.finder1.TabIndex = 3;
		this.finder1.Text = "toolStrip1";
		this.finder1.Visible = false;
		this.closeFinderToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.closeFinderToolStripButton.Image = (System.Drawing.Image)resources.GetObject("closeFinderToolStripButton.Image");
		this.closeFinderToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.closeFinderToolStripButton.Name = "closeFinderToolStripButton";
		this.closeFinderToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.closeFinderToolStripButton.Text = "toolStripButton1";
		this.closeFinderToolStripButton.Click += new System.EventHandler(closeFinderToolStripButton_Click);
		this.txtFindThis.Name = "txtFindThis";
		this.txtFindThis.Size = new System.Drawing.Size(100, 25);
		this.txtFindThis.TextChanged += new System.EventHandler(txtFindThis_TextChanged);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.findNextToolStripButton.Image = (System.Drawing.Image)resources.GetObject("findNextToolStripButton.Image");
		this.findNextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.findNextToolStripButton.Name = "findNextToolStripButton";
		this.findNextToolStripButton.Size = new System.Drawing.Size(73, 22);
		this.findNextToolStripButton.Text = "Find &Next";
		this.findNextToolStripButton.Click += new System.EventHandler(findNextToolStripButton_Click);
		this.findPreviousToolStripButton.Image = (System.Drawing.Image)resources.GetObject("findPreviousToolStripButton.Image");
		this.findPreviousToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.findPreviousToolStripButton.Name = "findPreviousToolStripButton";
		this.findPreviousToolStripButton.Size = new System.Drawing.Size(91, 22);
		this.findPreviousToolStripButton.Text = "Find &Previous";
		this.findPreviousToolStripButton.Click += new System.EventHandler(findPreviousToolStripButton_Click);
		this.tabControlExEx1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlExEx1.Controls.Add(this.tabPageEX1);
		this.tabControlExEx1.Controls.Add(this.tabPageEX2);
		this.tabControlExEx1.Controls.Add(this.tabPageEX3);
		this.tabControlExEx1.Controls.Add(this.tabPageEX4);
		this.tabControlExEx1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlExEx1.HideTabs = false;
		this.tabControlExEx1.HotTrack = true;
		this.tabControlExEx1.ImageList = this.imageList1;
		this.tabControlExEx1.Location = new System.Drawing.Point(0, 0);
		this.tabControlExEx1.Margin = new System.Windows.Forms.Padding(0);
		this.tabControlExEx1.Name = "tabControlExEx1";
		this.tabControlExEx1.Padding = new System.Drawing.Point(0, 0);
		this.tabControlExEx1.SelectedIndex = 0;
		this.tabControlExEx1.Size = new System.Drawing.Size(610, 368);
		this.tabControlExEx1.TabIndex = 2;
		this.tabControlExEx1.SelectedIndexChanging += new Dotnetrix.Controls.TabPageChangeEventHandler(tabControlEXEx1_SelectedIndexChanging);
		this.tabPageEX1.Controls.Add(this.designView1);
		this.tabPageEX1.ImageIndex = 0;
		this.tabPageEX1.Location = new System.Drawing.Point(0, 0);
		this.tabPageEX1.Name = "tabPageEX1";
		this.tabPageEX1.Size = new System.Drawing.Size(610, 343);
		this.tabPageEX1.TabIndex = 0;
		this.tabPageEX1.Text = "Design";
		this.designView1.ApplicationName = null;
		this.designView1.AutoSize = true;
		this.designView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.designView1.Location = new System.Drawing.Point(0, 0);
		this.designView1.Margin = new System.Windows.Forms.Padding(0);
		this.designView1.Name = "designView1";
		this.designView1.Size = new System.Drawing.Size(610, 343);
		this.designView1.TabIndex = 0;
		this.designView1.SyntaxHighlightingInvoked += new System.EventHandler(designSurface1_SyntaxHighlightingInvoked);
		this.tabPageEX2.Controls.Add(this.txtHtml);
		this.tabPageEX2.ImageIndex = 1;
		this.tabPageEX2.Location = new System.Drawing.Point(0, 0);
		this.tabPageEX2.Name = "tabPageEX2";
		this.tabPageEX2.Size = new System.Drawing.Size(610, 318);
		this.tabPageEX2.TabIndex = 1;
		this.tabPageEX2.Text = "Html";
		this.txtHtml.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtHtml.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtHtml.Location = new System.Drawing.Point(0, 0);
		this.txtHtml.Multiline = true;
		this.txtHtml.Name = "txtHtml";
		this.txtHtml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.txtHtml.Size = new System.Drawing.Size(610, 318);
		this.txtHtml.TabIndex = 0;
		this.tabPageEX3.Controls.Add(this.previewView1);
		this.tabPageEX3.ImageIndex = 2;
		this.tabPageEX3.Location = new System.Drawing.Point(0, 0);
		this.tabPageEX3.Name = "tabPageEX3";
		this.tabPageEX3.Size = new System.Drawing.Size(610, 318);
		this.tabPageEX3.TabIndex = 2;
		this.tabPageEX3.Text = "Preview";
		this.previewView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.previewView1.Location = new System.Drawing.Point(0, 0);
		this.previewView1.Name = "previewView1";
		this.previewView1.Size = new System.Drawing.Size(610, 318);
		this.previewView1.TabIndex = 0;
		this.tabPageEX4.Controls.Add(this.hcXml);
		this.tabPageEX4.ImageIndex = 3;
		this.tabPageEX4.Location = new System.Drawing.Point(0, 0);
		this.tabPageEX4.Name = "tabPageEX4";
		this.tabPageEX4.Size = new System.Drawing.Size(610, 318);
		this.tabPageEX4.TabIndex = 3;
		this.tabPageEX4.Text = "<xml/>";
		this.hcXml.AbsolutePositioningEnabled = false;
		this.hcXml.Dock = System.Windows.Forms.DockStyle.Fill;
		this.hcXml.IsDesignMode = false;
		this.hcXml.IsDirty = false;
		this.hcXml.Location = new System.Drawing.Point(0, 0);
		this.hcXml.MultipleSelectionEnabled = false;
		this.hcXml.Name = "hcXml";
		this.hcXml.ScriptEnabled = false;
		this.hcXml.ScriptObject = null;
		this.hcXml.Size = new System.Drawing.Size(610, 318);
		this.hcXml.TabIndex = 0;
		this.hcXml.Text = "htmlControl1";
		base.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tabControlExEx1);
		base.Controls.Add(this.finder1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Margin = new System.Windows.Forms.Padding(0);
		base.Name = "HtmlEditorSurface";
		base.Size = new System.Drawing.Size(610, 368);
		this.finder1.ResumeLayout(false);
		this.finder1.PerformLayout();
		this.tabControlExEx1.ResumeLayout(false);
		this.tabPageEX1.ResumeLayout(false);
		this.tabPageEX1.PerformLayout();
		this.tabPageEX2.ResumeLayout(false);
		this.tabPageEX2.PerformLayout();
		this.tabPageEX3.ResumeLayout(false);
		this.tabPageEX4.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public HtmlEditorSurface()
	{
		InitializeComponent();
	}

	public void SetHtml(string value)
	{
		if (CurrentEditorMode == EditorMode.Design)
		{
			DesignEditor.LoadHtml(value);
			DesignEditor.LoadHtml(value);
		}
		else
		{
			txtHtml.Text = value;
		}
	}

	public void Cut()
	{
		if (CanCut)
		{
			switch (CurrentEditorMode)
			{
			case EditorMode.Design:
				DesignEditor.Cut();
				break;
			case EditorMode.Html:
				HtmlEditor.Cut();
				break;
			case EditorMode.Xml:
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException("Unexpected editor mode");
			}
		}
	}

	public void Copy()
	{
		if (CanCopy)
		{
			switch (CurrentEditorMode)
			{
			case EditorMode.Design:
				DesignEditor.Copy();
				break;
			case EditorMode.Html:
				HtmlEditor.Copy();
				break;
			case EditorMode.Preview:
				PreviewEditor.Copy();
				break;
			case EditorMode.Xml:
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException("Unexpected editor mode");
			}
		}
	}

	public void Paste()
	{
		if (CanPaste)
		{
			switch (CurrentEditorMode)
			{
			case EditorMode.Design:
				DesignEditor.Paste();
				break;
			case EditorMode.Html:
				HtmlEditor.Paste();
				break;
			case EditorMode.Xml:
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException("Unexpected editor mode");
			}
		}
	}

	public void Undo()
	{
		if (CanUndo)
		{
			switch (CurrentEditorMode)
			{
			case EditorMode.Design:
				DesignEditor.Undo();
				break;
			case EditorMode.Html:
				HtmlEditor.Undo();
				break;
			case EditorMode.Xml:
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException("Unexpected editor mode");
			}
		}
	}

	public void Redo()
	{
		if (CanRedo)
		{
			switch (CurrentEditorMode)
			{
			case EditorMode.Design:
				DesignEditor.Redo();
				break;
			case EditorMode.Xml:
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException("Unexpected editor mode");
			}
		}
	}

	public void SelectAll()
	{
		if (CanSelectAll)
		{
			switch (CurrentEditorMode)
			{
			case EditorMode.Design:
				DesignEditor.SelectAll();
				break;
			case EditorMode.Html:
				HtmlEditor.SelectAll();
				break;
			case EditorMode.Preview:
				PreviewEditor.SelectAll();
				break;
			case EditorMode.Xml:
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException("Unexpected editor mode");
			}
		}
	}

	public void ShowFinder()
	{
		finder1.Visible = true;
		txtFindThis.Focus();
	}

	public void HideFinder()
	{
		finder1.Visible = false;
	}

	private void closeFinderToolStripButton_Click(object sender, EventArgs e)
	{
		HideFinder();
	}

	private void txtFindThis_TextChanged(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(txtFindThis.Text))
		{
			Find(txtFindThis.Text, matchCase: false, wholeWord: false, searchUp: false, useNextOrPrevious: false);
		}
	}

	private void findNextToolStripButton_Click(object sender, EventArgs e)
	{
		Find(txtFindThis.Text, matchCase: false, wholeWord: false, searchUp: false, useNextOrPrevious: true);
	}

	private void findPreviousToolStripButton_Click(object sender, EventArgs e)
	{
		Find(txtFindThis.Text, matchCase: false, wholeWord: false, searchUp: true, useNextOrPrevious: true);
	}

	public bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp, bool useNextOrPrevious)
	{
		if (!useNextOrPrevious)
		{
			SelectAll();
		}
		switch (CurrentEditorMode)
		{
		case EditorMode.Design:
			return DesignEditor.Find(searchString, matchCase, wholeWord, searchUp);
		case EditorMode.Html:
		{
			string text = (matchCase ? txtHtml.Text : txtHtml.Text.ToLower());
			string value = (matchCase ? searchString : searchString.ToLower());
			int num = (searchUp ? text.LastIndexOf(value) : text.IndexOf(value));
			if (num > -1)
			{
				txtHtml.Select(num, searchString.Length);
				return true;
			}
			return false;
		}
		case EditorMode.Preview:
			return PreviewEditor.Find(searchString, matchCase, wholeWord, searchUp);
		case EditorMode.Xml:
			return hcXml.Find(searchString, matchCase, wholeWord, searchUp);
		default:
			throw new InvalidOperationException("Unexpected editor mode");
		}
	}

	public string StripHTMLBody()
	{
		return designView1.StripHTMLBody();
	}

	public void ReverseRefreshTabSelection(EditorMode previousMode)
	{
		switch (previousMode)
		{
		case EditorMode.Design:
			tabControlExEx1.SelectTab(tabPageEX4);
			break;
		case EditorMode.Html:
			tabControlExEx1.SelectTab(tabPageEX1);
			designView1.DesignEditor.Focus();
			break;
		case EditorMode.Preview:
			tabControlExEx1.SelectTab(tabPageEX2);
			txtHtml.Focus();
			break;
		case EditorMode.Xml:
			tabControlExEx1.SelectTab(tabPageEX3);
			break;
		}
	}

	public void RefreshTabSelection(EditorMode previousMode)
	{
		switch (previousMode)
		{
		case EditorMode.Design:
			tabControlExEx1.SelectTab(tabPageEX2);
			txtHtml.Focus();
			break;
		case EditorMode.Html:
			tabControlExEx1.SelectTab(tabPageEX3);
			previewView1.PreviewEditor.Focus();
			break;
		case EditorMode.Preview:
			tabControlExEx1.SelectTab(tabPageEX4);
			hcXml.Focus();
			break;
		case EditorMode.Xml:
			tabControlExEx1.SelectTab(tabPageEX1);
			designView1.DesignEditor.Focus();
			break;
		}
	}

	public void RefreshDisplay()
	{
		RefreshDisplay(CurrentEditorMode);
	}

	public void RefreshDisplay(EditorMode previousMode)
	{
		switch (CurrentEditorMode)
		{
		case EditorMode.Design:
			if (previousMode == EditorMode.Html)
			{
				DesignEditor.LoadHtml(txtHtml.Text);
			}
			break;
		case EditorMode.Html:
			if (previousMode == EditorMode.Design)
			{
				txtHtml.Text = designView1.StripHTMLBody();
			}
			break;
		case EditorMode.Preview:
			SyncronizeContent(previousMode);
			SetHtml(txtHtml.Text);
			if (PreviewTemplate.Length == 0)
			{
				PreviewEditor.LoadHtml(txtHtml.Text);
			}
			else
			{
				PreviewEditor.LoadHtml(string.Format(PreviewTemplate, txtHtml.Text));
			}
			break;
		case EditorMode.Xml:
		{
			SyncronizeContent(previousMode);
			Language language = Languages.GetLanguage("XML");
			string content = language.ApplyStyles(txtHtml.Text);
			hcXml.LoadHtml(content);
			break;
		}
		}
	}

	private void SyncronizeContent(EditorMode previousMode)
	{
		switch (previousMode)
		{
		case EditorMode.Design:
			txtHtml.Text = designView1.StripHTMLBody();
			break;
		case EditorMode.Html:
			DesignEditor.LoadHtml(txtHtml.Text);
			break;
		}
	}

	private void designSurface1_SyntaxHighlightingInvoked(object sender, EventArgs e)
	{
		using CodeHtmlerForm codeHtmlerForm = new CodeHtmlerForm();
		if (codeHtmlerForm.ShowDialog() == DialogResult.OK)
		{
			designView1.DesignEditor.InsertHtml(codeHtmlerForm.CodeHtml);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch (keyData)
		{
		case Keys.M | Keys.Control:
			DesignEditor.TextFormatting.Indent();
			return true;
		case Keys.M | Keys.Shift | Keys.Control:
			DesignEditor.TextFormatting.Unindent();
			return true;
		default:
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}

	private void tabControlEXEx1_SelectedIndexChanging(object sender, TabPageChangeEventArgs e)
	{
		EditorMode currentEditorMode = CurrentEditorMode;
		CurrentEditorMode = (EditorMode)tabControlExEx1.TabPages.IndexOfKey(e.NextTab.Name);
		RefreshDisplay(currentEditorMode);
	}
}
