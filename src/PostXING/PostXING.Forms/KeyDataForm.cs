using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Forms;

public class KeyDataForm : Form
{
	private IContainer components;

	private Label label1;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel toolStripStatusLabel1;

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
		this.label1 = new System.Windows.Forms.Label();
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.statusStrip1.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(35, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "label1";
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripStatusLabel1 });
		this.statusStrip1.Location = new System.Drawing.Point(0, 23);
		this.statusStrip1.Name = "statusStrip1";
		this.statusStrip1.Size = new System.Drawing.Size(226, 22);
		this.statusStrip1.TabIndex = 1;
		this.statusStrip1.Text = "statusStrip1";
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
		this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(226, 45);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.label1);
		base.Name = "KeyDataForm";
		this.Text = "KeyDataForm";
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	public KeyDataForm()
	{
		InitializeComponent();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		int num = (int)keyData;
		label1.Text = num.ToString();
		if ((keyData & Keys.Control) == 0 && (keyData & Keys.Return) == 0)
		{
			if ((keyData & Keys.Shift) > Keys.None)
			{
				statusStrip1.Items[0].Text = 65536 + "+" + 131072 + "+" + 13;
			}
			else
			{
				statusStrip1.Items[0].Text = 131072 + "+" + 13;
			}
		}
		else
		{
			statusStrip1.Items[0].Text = "none.";
		}
		return base.ProcessCmdKey(ref msg, keyData);
	}
}
