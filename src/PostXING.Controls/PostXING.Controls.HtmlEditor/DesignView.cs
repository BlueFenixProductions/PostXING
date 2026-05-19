using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls.HtmlEditor.Html;

namespace PostXING.Controls.HtmlEditor;

public class DesignView : UserControl
{
	private IContainer components;

	private ToolStripContainer toolStripContainer1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnBold;

	private ToolStripButton btnItalics;

	private ToolStripButton btnUnderline;

	private ToolStripButton btnStrike;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnLeft;

	private ToolStripButton btnCenter;

	private ToolStripButton btnRight;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnOrderedList;

	private ToolStripButton btnBullets;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton btnHorRule;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnIndent;

	private ToolStripButton btnUnindent;

	private ToolStripButton btnUndo;

	private ToolStripButton btnRedo;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton cutToolStripButton;

	private ToolStripButton copyToolStripButton;

	private ToolStripButton pasteToolStripButton;

	private ToolStripButton btnHyperlink;

	private ToolStripSeparator toolStripSeparator8;

	private ToolStripSeparator toolStripSeparator9;

	private ToolStripButton btnImage;

	private ToolStripButton btnUnlink;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripColorButton btnFontColor;

	private HtmlControl htmlControl1;

	private ToolStripColorButton btnFontBackground;

	private ToolStrip toolStrip2;

	private ToolStripComboBox toolStripComboBox1;

	private ToolStripComboBox toolStripComboBox2;

	private ToolStripComboBox toolStripComboBox3;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton btnSyntaxHighlighting;

	private ToolStrip toolStrip3;

	private string _applicationName;

	public HtmlControl DesignEditor => htmlControl1;

	public ToolStrip ReBar => toolStrip3;

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

	public HtmlControl HtmlControl => htmlControl1;

	public event EventHandler SyntaxHighlightingInvoked;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Controls.HtmlEditor.DesignView));
		this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
		this.htmlControl1 = new PostXING.Controls.HtmlEditor.Html.HtmlControl();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnBold = new System.Windows.Forms.ToolStripButton();
		this.btnItalics = new System.Windows.Forms.ToolStripButton();
		this.btnUnderline = new System.Windows.Forms.ToolStripButton();
		this.btnStrike = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLeft = new System.Windows.Forms.ToolStripButton();
		this.btnCenter = new System.Windows.Forms.ToolStripButton();
		this.btnRight = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnOrderedList = new System.Windows.Forms.ToolStripButton();
		this.btnBullets = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnHorRule = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnIndent = new System.Windows.Forms.ToolStripButton();
		this.btnUnindent = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUndo = new System.Windows.Forms.ToolStripButton();
		this.btnRedo = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnImage = new System.Windows.Forms.ToolStripButton();
		this.btnHyperlink = new System.Windows.Forms.ToolStripButton();
		this.btnUnlink = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnFontColor = new PostXING.Controls.ToolStripColorButton();
		this.btnFontBackground = new PostXING.Controls.ToolStripColorButton();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
		this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
		this.toolStripComboBox3 = new System.Windows.Forms.ToolStripComboBox();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSyntaxHighlighting = new System.Windows.Forms.ToolStripButton();
		this.toolStripContainer1.ContentPanel.SuspendLayout();
		this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
		this.toolStripContainer1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.toolStripContainer1.ContentPanel.Controls.Add(this.htmlControl1);
		this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(746, 451);
		this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
		this.toolStripContainer1.Name = "toolStripContainer1";
		this.toolStripContainer1.Size = new System.Drawing.Size(746, 526);
		this.toolStripContainer1.TabIndex = 0;
		this.toolStripContainer1.Text = "toolStripContainer1";
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip3);
		this.htmlControl1.AbsolutePositioningEnabled = false;
		this.htmlControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.htmlControl1.IsDesignMode = true;
		this.htmlControl1.IsDirty = false;
		this.htmlControl1.Location = new System.Drawing.Point(0, 0);
		this.htmlControl1.MultipleSelectionEnabled = false;
		this.htmlControl1.Name = "htmlControl1";
		this.htmlControl1.ScriptEnabled = false;
		this.htmlControl1.ScriptObject = null;
		this.htmlControl1.Size = new System.Drawing.Size(746, 451);
		this.htmlControl1.TabIndex = 0;
		this.htmlControl1.Text = "htmlControl1";
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[30]
		{
			this.btnBold, this.btnItalics, this.btnUnderline, this.btnStrike, this.toolStripSeparator1, this.btnLeft, this.btnCenter, this.btnRight, this.toolStripSeparator2, this.btnOrderedList,
			this.btnBullets, this.toolStripSeparator3, this.btnHorRule, this.toolStripSeparator4, this.btnIndent, this.btnUnindent, this.toolStripSeparator8, this.cutToolStripButton, this.copyToolStripButton, this.pasteToolStripButton,
			this.toolStripSeparator9, this.btnUndo, this.btnRedo, this.toolStripSeparator, this.btnImage, this.btnHyperlink, this.btnUnlink, this.toolStripSeparator6, this.btnFontColor, this.btnFontBackground
		});
		this.toolStrip1.Location = new System.Drawing.Point(3, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(566, 25);
		this.toolStrip1.TabIndex = 0;
		this.btnBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnBold.Image = (System.Drawing.Image)resources.GetObject("btnBold.Image");
		this.btnBold.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBold.Name = "btnBold";
		this.btnBold.Size = new System.Drawing.Size(23, 22);
		this.btnBold.Text = "Bold";
		this.btnBold.Click += new System.EventHandler(btnBold_Click);
		this.btnItalics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnItalics.Image = (System.Drawing.Image)resources.GetObject("btnItalics.Image");
		this.btnItalics.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnItalics.Name = "btnItalics";
		this.btnItalics.Size = new System.Drawing.Size(23, 22);
		this.btnItalics.Text = "Italics";
		this.btnItalics.Click += new System.EventHandler(btnItalics_Click);
		this.btnUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnUnderline.Image = (System.Drawing.Image)resources.GetObject("btnUnderline.Image");
		this.btnUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUnderline.Name = "btnUnderline";
		this.btnUnderline.Size = new System.Drawing.Size(23, 22);
		this.btnUnderline.Text = "Underline";
		this.btnUnderline.Click += new System.EventHandler(btnUnderline_Click);
		this.btnStrike.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnStrike.Image = (System.Drawing.Image)resources.GetObject("btnStrike.Image");
		this.btnStrike.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnStrike.Name = "btnStrike";
		this.btnStrike.Size = new System.Drawing.Size(23, 22);
		this.btnStrike.Text = "Strikethru";
		this.btnStrike.Click += new System.EventHandler(btnStrike_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.btnLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnLeft.Image = (System.Drawing.Image)resources.GetObject("btnLeft.Image");
		this.btnLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLeft.Name = "btnLeft";
		this.btnLeft.Size = new System.Drawing.Size(23, 22);
		this.btnLeft.Text = "Left Justify";
		this.btnLeft.Click += new System.EventHandler(btnLeft_Click);
		this.btnCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnCenter.Image = (System.Drawing.Image)resources.GetObject("btnCenter.Image");
		this.btnCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCenter.Name = "btnCenter";
		this.btnCenter.Size = new System.Drawing.Size(23, 22);
		this.btnCenter.Text = "Center Justify";
		this.btnCenter.Click += new System.EventHandler(btnCenter_Click);
		this.btnRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnRight.Image = (System.Drawing.Image)resources.GetObject("btnRight.Image");
		this.btnRight.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRight.Name = "btnRight";
		this.btnRight.Size = new System.Drawing.Size(23, 22);
		this.btnRight.Text = "Right Justify";
		this.btnRight.Click += new System.EventHandler(btnRight_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnOrderedList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnOrderedList.Image = (System.Drawing.Image)resources.GetObject("btnOrderedList.Image");
		this.btnOrderedList.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOrderedList.Name = "btnOrderedList";
		this.btnOrderedList.Size = new System.Drawing.Size(23, 22);
		this.btnOrderedList.Text = "Ordered List";
		this.btnOrderedList.Click += new System.EventHandler(btnOrderedList_Click);
		this.btnBullets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnBullets.Image = (System.Drawing.Image)resources.GetObject("btnBullets.Image");
		this.btnBullets.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBullets.Name = "btnBullets";
		this.btnBullets.Size = new System.Drawing.Size(23, 22);
		this.btnBullets.Text = "Bullets";
		this.btnBullets.Click += new System.EventHandler(btnBullets_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnHorRule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnHorRule.Image = (System.Drawing.Image)resources.GetObject("btnHorRule.Image");
		this.btnHorRule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHorRule.Name = "btnHorRule";
		this.btnHorRule.Size = new System.Drawing.Size(23, 22);
		this.btnHorRule.Text = "Horizontal Rule";
		this.btnHorRule.Click += new System.EventHandler(btnHorRule_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnIndent.Image = (System.Drawing.Image)resources.GetObject("btnIndent.Image");
		this.btnIndent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnIndent.Name = "btnIndent";
		this.btnIndent.Size = new System.Drawing.Size(23, 22);
		this.btnIndent.Text = "Indent";
		this.btnIndent.Click += new System.EventHandler(btnIndent_Click);
		this.btnUnindent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnUnindent.Image = (System.Drawing.Image)resources.GetObject("btnUnindent.Image");
		this.btnUnindent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUnindent.Name = "btnUnindent";
		this.btnUnindent.Size = new System.Drawing.Size(23, 22);
		this.btnUnindent.Text = "Unindent";
		this.btnUnindent.Click += new System.EventHandler(btnUnindent_Click);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
		this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.cutToolStripButton.Image = (System.Drawing.Image)resources.GetObject("cutToolStripButton.Image");
		this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.cutToolStripButton.Name = "cutToolStripButton";
		this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.cutToolStripButton.Text = "C&ut";
		this.cutToolStripButton.Click += new System.EventHandler(cutToolStripButton_Click);
		this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.copyToolStripButton.Image = (System.Drawing.Image)resources.GetObject("copyToolStripButton.Image");
		this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.copyToolStripButton.Name = "copyToolStripButton";
		this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.copyToolStripButton.Text = "&Copy";
		this.copyToolStripButton.Click += new System.EventHandler(copyToolStripButton_Click);
		this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.pasteToolStripButton.Image = (System.Drawing.Image)resources.GetObject("pasteToolStripButton.Image");
		this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.pasteToolStripButton.Name = "pasteToolStripButton";
		this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.pasteToolStripButton.Text = "&Paste";
		this.pasteToolStripButton.Click += new System.EventHandler(pasteToolStripButton_Click);
		this.toolStripSeparator9.Name = "toolStripSeparator9";
		this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
		this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnUndo.Image = (System.Drawing.Image)resources.GetObject("btnUndo.Image");
		this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUndo.Name = "btnUndo";
		this.btnUndo.Size = new System.Drawing.Size(23, 22);
		this.btnUndo.Text = "Undo";
		this.btnUndo.Click += new System.EventHandler(btnUndo_Click);
		this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnRedo.Image = (System.Drawing.Image)resources.GetObject("btnRedo.Image");
		this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRedo.Name = "btnRedo";
		this.btnRedo.Size = new System.Drawing.Size(23, 22);
		this.btnRedo.Text = "Redo";
		this.btnRedo.Click += new System.EventHandler(btnRedo_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
		this.btnImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnImage.Image = (System.Drawing.Image)resources.GetObject("btnImage.Image");
		this.btnImage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnImage.Name = "btnImage";
		this.btnImage.Size = new System.Drawing.Size(23, 22);
		this.btnImage.Text = "Insert Image";
		this.btnImage.Click += new System.EventHandler(btnImage_Click);
		this.btnHyperlink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnHyperlink.Image = (System.Drawing.Image)resources.GetObject("btnHyperlink.Image");
		this.btnHyperlink.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHyperlink.Name = "btnHyperlink";
		this.btnHyperlink.Size = new System.Drawing.Size(23, 22);
		this.btnHyperlink.Text = "Insert Hyperlink";
		this.btnHyperlink.Click += new System.EventHandler(btnHyperlink_Click);
		this.btnUnlink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnUnlink.Image = (System.Drawing.Image)resources.GetObject("btnUnlink.Image");
		this.btnUnlink.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUnlink.Name = "btnUnlink";
		this.btnUnlink.Size = new System.Drawing.Size(23, 22);
		this.btnUnlink.Text = "Unlink";
		this.btnUnlink.Click += new System.EventHandler(btnUnlink_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnFontColor.AutomaticText = "Automatic";
		this.btnFontColor.Color = System.Drawing.Color.Empty;
		this.btnFontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnFontColor.Image = (System.Drawing.Image)resources.GetObject("btnFontColor.Image");
		this.btnFontColor.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFontColor.MoreColorsText = "More Colors...";
		this.btnFontColor.Name = "btnFontColor";
		this.btnFontColor.PanelVisible = false;
		this.btnFontColor.Size = new System.Drawing.Size(23, 22);
		this.btnFontColor.Text = "Font Color";
		this.btnFontColor.UseCustomColorDialog = false;
		this.btnFontColor.Changed += new System.EventHandler(btnFontColor_Changed);
		this.btnFontBackground.AutomaticText = "Automatic";
		this.btnFontBackground.Color = System.Drawing.Color.Empty;
		this.btnFontBackground.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnFontBackground.Image = (System.Drawing.Image)resources.GetObject("btnFontBackground.Image");
		this.btnFontBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFontBackground.MoreColorsText = "More Colors...";
		this.btnFontBackground.Name = "btnFontBackground";
		this.btnFontBackground.PanelVisible = false;
		this.btnFontBackground.Size = new System.Drawing.Size(23, 22);
		this.btnFontBackground.Text = "Font Background";
		this.btnFontBackground.UseCustomColorDialog = false;
		this.btnFontBackground.Changed += new System.EventHandler(btnFontBackground_Changed);
		this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip3.Location = new System.Drawing.Point(3, 50);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(111, 25);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripComboBox1, this.toolStripComboBox2, this.toolStripComboBox3, this.toolStripSeparator7, this.btnSyntaxHighlighting });
		this.toolStrip2.Location = new System.Drawing.Point(3, 25);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(542, 25);
		this.toolStrip2.TabIndex = 1;
		this.toolStripComboBox1.Items.AddRange(new object[11]
		{
			"Normal", "Formatted", "Heading1", "Heading2", "Heading3", "Heading4", "Heading5", "Heading6", "Paragraph", "OrderedList",
			"UnorderedList"
		});
		this.toolStripComboBox1.Name = "toolStripComboBox1";
		this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
		this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(toolStripComboBox1_SelectedIndexChanged);
		this.toolStripComboBox2.Items.AddRange(new object[7] { "Arial", "Courier New", "Garamond", "Georgia", "Tahoma", "Times New Roman", "Verdana" });
		this.toolStripComboBox2.Name = "toolStripComboBox2";
		this.toolStripComboBox2.Size = new System.Drawing.Size(121, 25);
		this.toolStripComboBox2.SelectedIndexChanged += new System.EventHandler(toolStripComboBox2_SelectedIndexChanged);
		this.toolStripComboBox3.Items.AddRange(new object[7] { "Smallest", "Smaller", "Small", "Medium", "Large", "Larger", "Largest" });
		this.toolStripComboBox3.Name = "toolStripComboBox3";
		this.toolStripComboBox3.Size = new System.Drawing.Size(121, 25);
		this.toolStripComboBox3.SelectedIndexChanged += new System.EventHandler(toolStripComboBox3_SelectedIndexChanged);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
		this.btnSyntaxHighlighting.Image = (System.Drawing.Image)resources.GetObject("btnSyntaxHighlighting.Image");
		this.btnSyntaxHighlighting.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSyntaxHighlighting.Name = "btnSyntaxHighlighting";
		this.btnSyntaxHighlighting.Size = new System.Drawing.Size(124, 22);
		this.btnSyntaxHighlighting.Text = "Syntax Highlighting";
		this.btnSyntaxHighlighting.Click += new System.EventHandler(btnSyntaxHighlighting_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.toolStripContainer1);
		base.Name = "DesignView";
		base.Size = new System.Drawing.Size(746, 526);
		this.toolStripContainer1.ContentPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.PerformLayout();
		this.toolStripContainer1.ResumeLayout(false);
		this.toolStripContainer1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}

	public DesignView()
	{
		InitializeComponent();
	}

	public string StripHTMLBody()
	{
		string html = DesignEditor.SaveHtml();
		HPathDocument hPathDocument = new HPathDocument();
		hPathDocument.LoadHtml(html);
		HtmlNode htmlNode = hPathDocument.DocumentNode.SelectSingleNode("//body");
		return htmlNode.InnerHtml;
	}

	private void btnBold_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ToggleBold();
	}

	private void btnItalics_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ToggleItalics();
	}

	private void btnUnderline_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ToggleUnderline();
	}

	private void btnStrike_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ToggleStrikethrough();
	}

	private void btnLeft_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.Alignment = HtmlAlignment.Left;
	}

	private void btnCenter_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.Alignment = HtmlAlignment.Center;
	}

	private void btnRight_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.Alignment = HtmlAlignment.Right;
	}

	private void btnOrderedList_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ToggleOrderedList();
	}

	private void btnBullets_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ToggleUnorderedList();
	}

	private void btnHorRule_Click(object sender, EventArgs e)
	{
		DesignEditor.InsertHtml("<HR>");
	}

	private void btnIndent_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.Indent();
	}

	private void btnUnindent_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.Unindent();
	}

	private void cutToolStripButton_Click(object sender, EventArgs e)
	{
		DesignEditor.Cut();
	}

	private void copyToolStripButton_Click(object sender, EventArgs e)
	{
		DesignEditor.Copy();
	}

	private void pasteToolStripButton_Click(object sender, EventArgs e)
	{
		DesignEditor.Paste();
	}

	private void btnUndo_Click(object sender, EventArgs e)
	{
		DesignEditor.Undo();
	}

	private void btnRedo_Click(object sender, EventArgs e)
	{
		DesignEditor.Redo();
	}

	private void btnImage_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.Image();
	}

	private void btnHyperlink_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.HyperLink();
	}

	private void btnUnlink_Click(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.RemoveLink();
	}

	private void btnFontBackground_Changed(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.BackColor = btnFontBackground.Color;
	}

	private void btnFontColor_Changed(object sender, EventArgs e)
	{
		DesignEditor.TextFormatting.ForeColor = btnFontColor.Color;
	}

	private void btnSyntaxHighlighting_Click(object sender, EventArgs e)
	{
		if (this.SyntaxHighlightingInvoked != null)
		{
			this.SyntaxHighlightingInvoked(sender, e);
		}
	}

	private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is ToolStripComboBox { SelectedIndex: >-1 } toolStripComboBox)
		{
			DesignEditor.TextFormatting.SetHtmlFormat((HtmlFormat)toolStripComboBox.SelectedIndex);
		}
	}

	private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is ToolStripComboBox { SelectedIndex: >-1 } toolStripComboBox)
		{
			DesignEditor.TextFormatting.FontName = toolStripComboBox.SelectedItem.ToString();
		}
	}

	private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is ToolStripComboBox { SelectedIndex: >-1 } toolStripComboBox)
		{
			DesignEditor.TextFormatting.FontSize = (HtmlFontSize)Enum.Parse(typeof(HtmlFontSize), toolStripComboBox.SelectedItem.ToString());
		}
	}
}
