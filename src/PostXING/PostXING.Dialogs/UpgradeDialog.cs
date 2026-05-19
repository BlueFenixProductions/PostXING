using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Components.Legacy.v1;
using PostXING.Controls;

namespace PostXING.Dialogs;

public class UpgradeDialog : Form
{
	private IContainer components;

	private XPCheckedListBox xpCheckedListBox1;

	private Button button1;

	private Button button2;

	private Label label1;

	public UpgradeDialog()
	{
		InitializeComponent();
	}

	protected override void OnLoad(EventArgs e)
	{
		string text = "No PostXING v1 blogs found.";
		string text2 = Path.Combine(AppManager.GetUserPath(), "Blogs.xml");
		if (File.Exists(text2))
		{
			PostXING.Components.Legacy.v1.BlogCollection blogCollection = (PostXING.Components.Legacy.v1.BlogCollection)AppManager._load(text2, typeof(PostXING.Components.Legacy.v1.BlogCollection));
			if (blogCollection != null && blogCollection.Count > 0)
			{
				PostXING.Components.Legacy.v1.IBlogEnumerator enumerator = blogCollection.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						PostXING.Components.Legacy.v1.Blog current = enumerator.Current;
						xpCheckedListBox1.Items.Add(current);
					}
				}
				finally
				{
					if (enumerator is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}
			}
			else
			{
				MessageBox.Show(text);
			}
		}
		else
		{
			MessageBox.Show(text);
		}
		base.OnLoad(e);
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (xpCheckedListBox1.CheckedItems.Count > 0)
		{
			foreach (PostXING.Components.Legacy.v1.Blog checkedItem in xpCheckedListBox1.CheckedItems)
			{
				PostXING.Components.Blog blog2 = new PostXING.Components.Blog(checkedItem.BlogInfo.blogid, checkedItem.BlogInfo.blogName, checkedItem.ServiceUrl, checkedItem.Username, checkedItem.Password);
				blog2.ProviderName = "MetaWeblog API";
				blog2.WebAddress = checkedItem.BlogInfo.url;
				blog2.ItemCount = checkedItem.ItemCount;
				blog2.Options = checkedItem.Options;
				blog2.SupportsCategories = true;
				AppManager.Blogs.Add(blog2);
			}
			AppManager.Save(AppManager.Blogs);
			AppManager.ConcreteEditor.LoadBlogs();
			button1.Enabled = false;
			button2.Text = "Close";
			label1.Text = "Blogs converted successfully.";
		}
		else
		{
			MessageBox.Show("Please select at least one blog to convert.");
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Close();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Dialogs.UpgradeDialog));
		this.xpCheckedListBox1 = new PostXING.Controls.XPCheckedListBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.xpCheckedListBox1.CheckOnClick = true;
		this.xpCheckedListBox1.FormattingEnabled = true;
		this.xpCheckedListBox1.Location = new System.Drawing.Point(13, 28);
		this.xpCheckedListBox1.Name = "xpCheckedListBox1";
		this.xpCheckedListBox1.Size = new System.Drawing.Size(154, 214);
		this.xpCheckedListBox1.TabIndex = 0;
		this.button1.Location = new System.Drawing.Point(165, 248);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 1;
		this.button1.Text = "Import";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Location = new System.Drawing.Point(247, 247);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 2;
		this.button2.Text = "Cancel";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.label1.Location = new System.Drawing.Point(183, 28);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(139, 47);
		this.label1.TabIndex = 3;
		this.label1.Text = "Select the Blogs you would like to import.";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(343, 278);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.xpCheckedListBox1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "UpgradeDialog";
		this.Text = "UpgradeDialog";
		base.ResumeLayout(false);
	}
}
