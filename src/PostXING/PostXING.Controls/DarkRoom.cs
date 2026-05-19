using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls.HtmlEditor.Html;

namespace PostXING.Controls;

public class DarkRoom : UserControl
{
	private const string htmlBegin = "\r\n\t\t\t<html>\r\n\t\t\t<head>\r\n\t\t\t<style type='text/css'>\r\n\t\t\tbody{\r\n\t\t\t\tbackground-color:#000;\r\n\t\t\t\tcolor:#0f0;\r\n\t\t\t\tfont-family:Consolas,monospace;\r\n\t\t\t\tfont-size:12pt;\r\n\t\t\t\ttext-align:justify;\r\n\t\t\t\tpadding:50px auto;\r\n\t\t\t\tmargin:0px 18%;\r\n\t\t\t\tscrollbar-face-color:#0c0;\r\n\t\t\t\tscrollbar-arrow-color:#000;\r\n\t\t\t\tscrollbar-track-color:#000;\r\n\t\t\t\tscrollbar-shadow-color:#000;\r\n\t\t\t\tscrollbar-highlight-color:#000;\r\n\t\t\t\tscrollbar-3dlight-color:#eee;\r\n\t\t\t\tscrollbar-darkshadow-Color:#0e0;\r\n\t\t\t}\r\n\r\n\t\t\ta{\r\n\t\t\t\tcolor:#0f0;\r\n\t\t\t}\r\n\r\n\t\t\tp{\r\n\t\t\t\tfont-size:12pt;\r\n\t\t\t}\r\n\t\r\n\t\t\th3{\r\n\t\t\t\ttext-decoration:underline;\r\n\t\t\t\ttext-align:center;\r\n\t\t\t}\r\n\t\t\t</style>\r\n\t\t\t</head>\r\n\t\t\t<body>";

	private const string htmlEnd = "\r\n\t\t\t</body>\r\n\t\t\t</html>";

	private bool _listChordPressed;

	private string _title;

	private IContainer components;

	private HtmlControl htmlControl1;

	private Label label1;

	private TableLayoutPanel tableLayoutPanel1;

	private Label label2;

	public string StatusBarMessage
	{
		get
		{
			return label2.Text;
		}
		set
		{
			label2.Text = value;
		}
	}

	public string Title
	{
		get
		{
			syncronizeTitle();
			return _title.Replace("<H3>", "").Replace("</H3>", "");
		}
		set
		{
			_title = "<H3>" + value + "</H3>";
		}
	}

	public string Html
	{
		get
		{
			syncronizeTitle();
			return htmlControl1.GetBodyHtml().Replace(_title, "");
		}
		set
		{
			htmlControl1.LoadHtml("\r\n\t\t\t<html>\r\n\t\t\t<head>\r\n\t\t\t<style type='text/css'>\r\n\t\t\tbody{\r\n\t\t\t\tbackground-color:#000;\r\n\t\t\t\tcolor:#0f0;\r\n\t\t\t\tfont-family:Consolas,monospace;\r\n\t\t\t\tfont-size:12pt;\r\n\t\t\t\ttext-align:justify;\r\n\t\t\t\tpadding:50px auto;\r\n\t\t\t\tmargin:0px 18%;\r\n\t\t\t\tscrollbar-face-color:#0c0;\r\n\t\t\t\tscrollbar-arrow-color:#000;\r\n\t\t\t\tscrollbar-track-color:#000;\r\n\t\t\t\tscrollbar-shadow-color:#000;\r\n\t\t\t\tscrollbar-highlight-color:#000;\r\n\t\t\t\tscrollbar-3dlight-color:#eee;\r\n\t\t\t\tscrollbar-darkshadow-Color:#0e0;\r\n\t\t\t}\r\n\r\n\t\t\ta{\r\n\t\t\t\tcolor:#0f0;\r\n\t\t\t}\r\n\r\n\t\t\tp{\r\n\t\t\t\tfont-size:12pt;\r\n\t\t\t}\r\n\t\r\n\t\t\th3{\r\n\t\t\t\ttext-decoration:underline;\r\n\t\t\t\ttext-align:center;\r\n\t\t\t}\r\n\t\t\t</style>\r\n\t\t\t</head>\r\n\t\t\t<body>" + ((_title == "<H3></H3>") ? "<H3>Give it a Title</H3>" : _title) + value + "\r\n\t\t\t</body>\r\n\t\t\t</html>");
		}
	}

	public DarkRoom()
	{
		InitializeComponent();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch (keyData)
		{
		case Keys.X | Keys.Control:
			htmlControl1.Cut();
			return true;
		case Keys.C | Keys.Control:
			htmlControl1.Copy();
			return true;
		case Keys.V | Keys.Control:
			htmlControl1.Paste();
			return true;
		case Keys.Z | Keys.Control:
			htmlControl1.Undo();
			return true;
		case Keys.Y | Keys.Control:
			htmlControl1.Redo();
			return true;
		case Keys.A | Keys.Control:
			htmlControl1.SelectAll();
			return true;
		case Keys.M | Keys.Control:
			htmlControl1.TextFormatting.Indent();
			return true;
		case Keys.M | Keys.Shift | Keys.Control:
			htmlControl1.TextFormatting.Unindent();
			return true;
		case Keys.L | Keys.Control:
			_listChordPressed = true;
			return true;
		case Keys.O | Keys.Control:
			if (_listChordPressed)
			{
				htmlControl1.TextFormatting.ToggleOrderedList();
				_listChordPressed = false;
				return true;
			}
			break;
		}
		if (keyData == (Keys.U | Keys.Control) && _listChordPressed)
		{
			htmlControl1.TextFormatting.ToggleUnorderedList();
			_listChordPressed = false;
			return true;
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}

	public void SetCaretPositionToEnd()
	{
		htmlControl1.SetCaretPositionToEnd();
	}

	public new void Focus()
	{
		htmlControl1.Focus();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		htmlControl1.Focus();
	}

	private void syncronizeTitle()
	{
		string bodyHtml = htmlControl1.GetBodyHtml();
		if (!string.IsNullOrEmpty(bodyHtml) && bodyHtml.IndexOf(Environment.NewLine) > -1)
		{
			_title = bodyHtml.Remove(bodyHtml.IndexOf(Environment.NewLine));
		}
		if (!_title.Contains("<H3"))
		{
			_title = "<H3>Give it a title</H3>";
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
		this.htmlControl1 = new PostXING.Controls.HtmlEditor.Html.HtmlControl();
		this.label1 = new System.Windows.Forms.Label();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.label2 = new System.Windows.Forms.Label();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		this.htmlControl1.AbsolutePositioningEnabled = false;
		this.htmlControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.htmlControl1.IsDesignMode = true;
		this.htmlControl1.IsDirty = false;
		this.htmlControl1.Location = new System.Drawing.Point(0, 0);
		this.htmlControl1.MultipleSelectionEnabled = false;
		this.htmlControl1.Name = "htmlControl1";
		this.htmlControl1.ScriptEnabled = false;
		this.htmlControl1.ScriptObject = null;
		this.htmlControl1.Size = new System.Drawing.Size(554, 380);
		this.htmlControl1.TabIndex = 0;
		this.htmlControl1.Text = "htmlControl1";
		this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label1.Font = new System.Drawing.Font("Lucida Console", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Lime;
		this.label1.Location = new System.Drawing.Point(280, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(271, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "esc = back to normal.";
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 378);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(554, 18);
		this.tableLayoutPanel1.TabIndex = 2;
		this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label2.Font = new System.Drawing.Font("Lucida Console", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Lime;
		this.label2.Location = new System.Drawing.Point(3, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(271, 13);
		this.label2.TabIndex = 2;
		this.label2.Text = "New Post.";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.Black;
		base.Controls.Add(this.htmlControl1);
		base.Controls.Add(this.tableLayoutPanel1);
		base.Name = "DarkRoom";
		base.Size = new System.Drawing.Size(554, 396);
		this.tableLayoutPanel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
