using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CodeHtmler;
using Genghis.Windows.Forms;

namespace PostXING.Controls.HtmlEditor;

public class CodeHtmlerForm : Form
{
	private Languages languages;

	private string codeHtml = string.Empty;

	private string codeStyles = string.Empty;

	private IContainer components;

	private XPTextBox xpTextBox1;

	private Label label1;

	private CompletionCombo cbxLanguages;

	private Button btnOptions;

	private CheckBox checkBox1;

	private Button btnInsertCode;

	private Button btnCancel;

	public string CodeHtml
	{
		get
		{
			return codeHtml;
		}
		set
		{
			codeHtml = value;
		}
	}

	public string CodeStyles
	{
		get
		{
			return codeStyles;
		}
		set
		{
			codeStyles = value;
		}
	}

	public CodeHtmlerForm()
	{
		InitializeComponent();
		RefreshLanguages();
	}

	private void RefreshLanguages()
	{
		languages = Languages.Load();
		cbxLanguages.Items.Clear();
		foreach (Language codeLanguage in languages.CodeLanguages)
		{
			cbxLanguages.Items.Add(codeLanguage.Name);
		}
		if (languages.CodeLanguages.Count > 0)
		{
			cbxLanguages.SelectedIndex = 0;
		}
	}

	private void btnOptions_Click(object sender, EventArgs e)
	{
		using PropertyGridDialog propertyGridDialog = new PropertyGridDialog(languages);
		if (propertyGridDialog.ShowDialog() == DialogResult.OK)
		{
			languages = (Languages)propertyGridDialog.PropertyGridObject;
			languages.Save();
			RefreshLanguages();
		}
	}

	private void cbxLanguages_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void label1_Click(object sender, EventArgs e)
	{
	}

	private void btnInsertCode_Click(object sender, EventArgs e)
	{
		if (cbxLanguages.SelectedIndex >= 0)
		{
			Language language = languages.CodeLanguages[cbxLanguages.SelectedIndex] as Language;
			language.ShowLineNumbers = checkBox1.Checked;
			codeHtml = language.ApplyStyles(xpTextBox1.Text);
			if (language.StyleType == StyleType.GlobalStyles)
			{
				codeStyles = $"<style>{language.CodeElementsCSSStyles()}</style";
			}
			base.DialogResult = DialogResult.OK;
		}
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Controls.HtmlEditor.CodeHtmlerForm));
		this.label1 = new System.Windows.Forms.Label();
		this.cbxLanguages = new Genghis.Windows.Forms.CompletionCombo();
		this.btnOptions = new System.Windows.Forms.Button();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.btnInsertCode = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.xpTextBox1 = new PostXING.Controls.XPTextBox();
		base.SuspendLayout();
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 477);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 14);
		this.label1.TabIndex = 1;
		this.label1.Text = "Language";
		this.label1.Click += new System.EventHandler(label1_Click);
		this.cbxLanguages.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.cbxLanguages.FormattingEnabled = true;
		this.cbxLanguages.LimitToList = true;
		this.cbxLanguages.Location = new System.Drawing.Point(79, 477);
		this.cbxLanguages.Name = "cbxLanguages";
		this.cbxLanguages.Size = new System.Drawing.Size(121, 22);
		this.cbxLanguages.TabIndex = 2;
		this.cbxLanguages.Text = " ";
		this.cbxLanguages.SelectedIndexChanged += new System.EventHandler(cbxLanguages_SelectedIndexChanged);
		this.btnOptions.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnOptions.Location = new System.Drawing.Point(207, 475);
		this.btnOptions.Name = "btnOptions";
		this.btnOptions.Size = new System.Drawing.Size(75, 23);
		this.btnOptions.TabIndex = 3;
		this.btnOptions.Text = "Options";
		this.btnOptions.UseVisualStyleBackColor = true;
		this.btnOptions.Click += new System.EventHandler(btnOptions_Click);
		this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(291, 480);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(102, 18);
		this.checkBox1.TabIndex = 4;
		this.checkBox1.Text = "Line Numbers";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.btnInsertCode.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnInsertCode.Location = new System.Drawing.Point(454, 475);
		this.btnInsertCode.Name = "btnInsertCode";
		this.btnInsertCode.Size = new System.Drawing.Size(91, 23);
		this.btnInsertCode.TabIndex = 5;
		this.btnInsertCode.Text = "Insert Code";
		this.btnInsertCode.UseVisualStyleBackColor = true;
		this.btnInsertCode.Click += new System.EventHandler(btnInsertCode_Click);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Location = new System.Drawing.Point(552, 475);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 6;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.xpTextBox1.AcceptsReturn = true;
		this.xpTextBox1.AcceptsTab = true;
		this.xpTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.xpTextBox1.Font = new System.Drawing.Font("Courier New", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.xpTextBox1.Location = new System.Drawing.Point(14, 13);
		this.xpTextBox1.Multiline = true;
		this.xpTextBox1.Name = "xpTextBox1";
		this.xpTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
		this.xpTextBox1.Size = new System.Drawing.Size(625, 449);
		this.xpTextBox1.TabIndex = 0;
		this.xpTextBox1.Text = "using System;";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 14f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(651, 509);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnInsertCode);
		base.Controls.Add(this.checkBox1);
		base.Controls.Add(this.btnOptions);
		base.Controls.Add(this.cbxLanguages);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.xpTextBox1);
		this.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "CodeHtmlerForm";
		this.Text = "Syntax Highlighting";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
