using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class CategoriesPage : ControlPanelPage
{
	private Label label2;

	private XPTextBox txtNewCategories;

	private Button btnOk;

	private Button btnCancel;

	private GroupBox grpOfflineCategories;

	private XPTextBox txtOfflineCategories;

	private Label label1;

	private IContainer components;

	private XPCheckedListBox chkLiveCategories;

	private Label lblMessage;

	private System.ComponentModel.BackgroundWorker backgroundWorker1;

	private bool isUsingLiveCategories = true;

	private string[] _selectedCategories;

	public CategoriesPage()
	{
		InitializeComponent();
	}

	private void AnihilateLiveCategories()
	{
		if (base.InvokeRequired)
		{
			Invoke(new MethodInvoker(AnihilateLiveCategories));
		}
		else if (AppManager.CurrentBlog != null && AppManager.CurrentBlog.CachedCategories != null && AppManager.CurrentBlog.CachedCategories.Length > 0)
		{
			_loadAndCheckCategories(AppManager.EditorForm.CurrentPost.Categories, AppManager.CurrentBlog.CachedCategories);
			chkLiveCategories.Visible = true;
			lblMessage.Visible = false;
		}
		else
		{
			Label label = lblMessage;
			bool visible = (chkLiveCategories.Visible = (isUsingLiveCategories = false));
			label.Visible = visible;
		}
	}

	private bool CategoryExistsInPost(string[] categories, string title)
	{
		if (categories == null)
		{
			return false;
		}
		foreach (string text in categories)
		{
			if (text == title)
			{
				return true;
			}
		}
		return false;
	}

	public void BindCategories()
	{
		BindCategories(AppManager.EditorForm.CurrentPost.Categories);
	}

	public void BindCategories(string[] selectedCategories)
	{
		_selectedCategories = selectedCategories;
		Label label = lblMessage;
		bool visible = (chkLiveCategories.Visible = true);
		label.Visible = visible;
		if (backgroundWorker1.IsBusy)
		{
			AnihilateLiveCategories();
		}
		else
		{
			backgroundWorker1.RunWorkerAsync();
		}
	}

	private void _loadAndCheckCategories(string[] selectedCategories, string[] categories)
	{
		chkLiveCategories.Items.Clear();
		foreach (string text in categories)
		{
			chkLiveCategories.Items.Add(text, CategoryExistsInPost(selectedCategories, text));
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		if (!base.DesignMode)
		{
			AppManager.ConcreteEditor.NewPostCreatedEventHandler += ConcreteEditor_NewPostCreatedEventHandler;
		}
		base.OnLoad(e);
	}

	private void ConcreteEditor_NewPostCreatedEventHandler(object sender, EventArgs e)
	{
		BindCategories();
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		BindCategories();
		base.OnPageEnter(e);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.NavigationPages.CategoriesPage));
		this.chkLiveCategories = new PostXING.Controls.XPCheckedListBox();
		this.txtNewCategories = new PostXING.Controls.XPTextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.btnOk = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.grpOfflineCategories = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtOfflineCategories = new PostXING.Controls.XPTextBox();
		this.lblMessage = new System.Windows.Forms.Label();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.grpOfflineCategories.SuspendLayout();
		base.SuspendLayout();
		this.chkLiveCategories.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chkLiveCategories.CheckOnClick = true;
		this.chkLiveCategories.Location = new System.Drawing.Point(8, 8);
		this.chkLiveCategories.Name = "chkLiveCategories";
		this.chkLiveCategories.Size = new System.Drawing.Size(176, 340);
		this.chkLiveCategories.TabIndex = 0;
		this.chkLiveCategories.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(chkLiveCategories_ItemCheck);
		this.txtNewCategories.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtNewCategories.Location = new System.Drawing.Point(8, 376);
		this.txtNewCategories.Name = "txtNewCategories";
		this.txtNewCategories.Size = new System.Drawing.Size(176, 21);
		this.txtNewCategories.TabIndex = 1;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label2.Location = new System.Drawing.Point(8, 352);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(192, 23);
		this.label2.TabIndex = 6;
		this.label2.Text = "New Categories ( separated by ';'  ):";
		this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnOk.Location = new System.Drawing.Point(16, 404);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(75, 23);
		this.btnOk.TabIndex = 7;
		this.btnOk.Text = "OK";
		this.btnOk.Click += new System.EventHandler(btnOk_Click);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.btnCancel.Location = new System.Drawing.Point(104, 404);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 8;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.grpOfflineCategories.Controls.Add(this.label1);
		this.grpOfflineCategories.Controls.Add(this.txtOfflineCategories);
		this.grpOfflineCategories.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.grpOfflineCategories.Location = new System.Drawing.Point(8, 8);
		this.grpOfflineCategories.Name = "grpOfflineCategories";
		this.grpOfflineCategories.Size = new System.Drawing.Size(176, 336);
		this.grpOfflineCategories.TabIndex = 9;
		this.grpOfflineCategories.TabStop = false;
		this.grpOfflineCategories.Text = "Offline Categories";
		this.label1.Location = new System.Drawing.Point(8, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(160, 80);
		this.label1.TabIndex = 1;
		this.label1.Text = "There was an error retrieving your categories. Please add any categories you would like to associate with the current post in the textbox below, one to a line.";
		this.txtOfflineCategories.Location = new System.Drawing.Point(4, 104);
		this.txtOfflineCategories.Multiline = true;
		this.txtOfflineCategories.Name = "txtOfflineCategories";
		this.txtOfflineCategories.Size = new System.Drawing.Size(168, 224);
		this.txtOfflineCategories.TabIndex = 0;
		this.lblMessage.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblMessage.Image = (System.Drawing.Image)resources.GetObject("lblMessage.Image");
		this.lblMessage.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
		this.lblMessage.Location = new System.Drawing.Point(44, 148);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(100, 67);
		this.lblMessage.TabIndex = 10;
		this.lblMessage.Text = "Retrieving Categories...";
		this.lblMessage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		base.Controls.Add(this.lblMessage);
		base.Controls.Add(this.chkLiveCategories);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOk);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.txtNewCategories);
		base.Controls.Add(this.grpOfflineCategories);
		base.Name = "CategoriesPage";
		this.Text = "Categories";
		this.grpOfflineCategories.ResumeLayout(false);
		this.grpOfflineCategories.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		ApplySelectedCategories(-1);
	}

	private void ApplySelectedCategories(int newlyCheckedIndex)
	{
		ArrayList arrayList = new ArrayList();
		if (chkLiveCategories.CheckedItems.Count > 0 && isUsingLiveCategories)
		{
			string[] array = new string[chkLiveCategories.CheckedItems.Count];
			for (int i = 0; i < chkLiveCategories.CheckedItems.Count; i++)
			{
				array[i] = chkLiveCategories.CheckedItems[i].ToString();
			}
			arrayList.AddRange(array);
		}
		else if (!isUsingLiveCategories)
		{
			string[] c = txtOfflineCategories.Text.Split(Environment.NewLine.ToCharArray());
			arrayList.AddRange(c);
		}
		if (newlyCheckedIndex > -1)
		{
			arrayList.Add(chkLiveCategories.Items[newlyCheckedIndex].ToString());
		}
		if (txtNewCategories.Text.Trim().Length > 0)
		{
			string[] c2 = txtNewCategories.Text.Trim().Split(';');
			arrayList.AddRange(c2);
		}
		AppManager.EditorForm.CurrentPost.Categories = (string[])arrayList.ToArray(typeof(string));
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		ApplyCancelButton();
	}

	private void chkLiveCategories_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		if (e.NewValue == CheckState.Checked)
		{
			ApplySelectedCategories(e.Index);
		}
	}

	private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
	{
		try
		{
			e.Result = AppManager.CurrentProvider.GetCategories(AppManager.CurrentBlog);
		}
		catch
		{
			AnihilateLiveCategories();
		}
	}

	private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
	{
		if (e.Result == null)
		{
			AnihilateLiveCategories();
			return;
		}
		AppManager.Categories = (string[])e.Result;
		AppManager.CurrentBlog.CachedCategories = AppManager.Categories;
		AppManager.Save(AppManager.Blogs);
		_loadAndCheckCategories(_selectedCategories, AppManager.Categories);
		lblMessage.Visible = false;
	}
}
