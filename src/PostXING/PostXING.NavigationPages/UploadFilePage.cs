using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.Controls;
using PostXING.Controls.Navigation;

namespace PostXING.NavigationPages;

public class UploadFilePage : ControlPanelPage
{
	private const string m_dropFormatType = "FileDrop";

	private Label label1;

	private GroupBox groupBox1;

	private RadioButton rbNone;

	private RadioButton rbAnchor;

	private OpenFileDialog ofdUploadFile;

	private IContainer components;

	private FTPControl ftpProgress;

	private FTPSite ftpSite;

	private Button btnUpload;

	private Button btnCancel;

	private ListBox lstFiles;

	private Button btnRemoveFiles;

	private Button btnAddFiles;

	private MozPane mozPane1;

	private MozItem mozItem1;

	private MozItem mozItem2;

	private Label label4;

	private RadioButton rbImage;

	public event FileUploadedEventHandler FileUploaded
	{
		add
		{
			ftpProgress.FileUploaded += value;
		}
		remove
		{
			ftpProgress.FileUploaded -= value;
		}
	}

	public UploadFilePage()
	{
		InitializeComponent();
	}

	public override void OnPageEnter(PageEventArgs e)
	{
		switch (AppManager.Preferences.UploadUsing)
		{
		default:
			mozPane1.SelectItem(0);
			break;
		case "BlogProvider":
			mozPane1.SelectItem(1);
			break;
		}
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
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rbImage = new System.Windows.Forms.RadioButton();
		this.rbNone = new System.Windows.Forms.RadioButton();
		this.rbAnchor = new System.Windows.Forms.RadioButton();
		this.ofdUploadFile = new System.Windows.Forms.OpenFileDialog();
		this.btnUpload = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.lstFiles = new System.Windows.Forms.ListBox();
		this.btnRemoveFiles = new System.Windows.Forms.Button();
		this.btnAddFiles = new System.Windows.Forms.Button();
		this.ftpProgress = new PostXING.Controls.FTPControl();
		this.mozPane1 = new PostXING.Controls.Controls.MozPane();
		this.mozItem1 = new PostXING.Controls.Controls.MozItem();
		this.mozItem2 = new PostXING.Controls.Controls.MozItem();
		this.label4 = new System.Windows.Forms.Label();
		System.Windows.Forms.Label label = new System.Windows.Forms.Label();
		System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.mozPane1).BeginInit();
		this.mozPane1.SuspendLayout();
		base.SuspendLayout();
		label.AutoSize = true;
		label.Location = new System.Drawing.Point(74, 20);
		label.Name = "label2";
		label.Size = new System.Drawing.Size(91, 13);
		label.TabIndex = 3;
		label.Text = "<img src=\"...\" />";
		label2.AutoSize = true;
		label2.Location = new System.Drawing.Point(74, 44);
		label2.Name = "label3";
		label2.Size = new System.Drawing.Size(87, 13);
		label2.TabIndex = 4;
		label2.Text = "<a href=\"...\" />";
		this.label1.Location = new System.Drawing.Point(8, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(100, 16);
		this.label1.TabIndex = 9;
		this.label1.Text = "File(s):";
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(label2);
		this.groupBox1.Controls.Add(label);
		this.groupBox1.Controls.Add(this.rbImage);
		this.groupBox1.Controls.Add(this.rbNone);
		this.groupBox1.Controls.Add(this.rbAnchor);
		this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.groupBox1.Location = new System.Drawing.Point(10, 134);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(174, 92);
		this.groupBox1.TabIndex = 10;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Insert As:";
		this.rbImage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.rbImage.Enabled = false;
		this.rbImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rbImage.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rbImage.Location = new System.Drawing.Point(6, 20);
		this.rbImage.Name = "rbImage";
		this.rbImage.Size = new System.Drawing.Size(64, 14);
		this.rbImage.TabIndex = 3;
		this.rbImage.TabStop = true;
		this.rbImage.Text = "Image";
		this.rbImage.CheckedChanged += new System.EventHandler(modeChange);
		this.rbNone.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.rbNone.Enabled = false;
		this.rbNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rbNone.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rbNone.Location = new System.Drawing.Point(6, 68);
		this.rbNone.Name = "rbNone";
		this.rbNone.Size = new System.Drawing.Size(64, 14);
		this.rbNone.TabIndex = 5;
		this.rbNone.Text = "None";
		this.rbNone.CheckedChanged += new System.EventHandler(modeChange);
		this.rbAnchor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.rbAnchor.Enabled = false;
		this.rbAnchor.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.rbAnchor.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.rbAnchor.Location = new System.Drawing.Point(6, 44);
		this.rbAnchor.Name = "rbAnchor";
		this.rbAnchor.Size = new System.Drawing.Size(64, 14);
		this.rbAnchor.TabIndex = 4;
		this.rbAnchor.TabStop = true;
		this.rbAnchor.Text = "Link";
		this.rbAnchor.CheckedChanged += new System.EventHandler(modeChange);
		this.ofdUploadFile.Multiselect = true;
		this.ofdUploadFile.Title = "Select Files For Upload...";
		this.btnUpload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpload.Enabled = false;
		this.btnUpload.Location = new System.Drawing.Point(60, 232);
		this.btnUpload.Name = "btnUpload";
		this.btnUpload.Size = new System.Drawing.Size(59, 24);
		this.btnUpload.TabIndex = 7;
		this.btnUpload.Text = "Upload";
		this.btnUpload.UseVisualStyleBackColor = true;
		this.btnUpload.Click += new System.EventHandler(btnUpload_Clicked);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.Location = new System.Drawing.Point(125, 232);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(59, 24);
		this.btnCancel.TabIndex = 8;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Clicked);
		this.lstFiles.AllowDrop = true;
		this.lstFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lstFiles.FormattingEnabled = true;
		this.lstFiles.Location = new System.Drawing.Point(10, 27);
		this.lstFiles.Name = "lstFiles";
		this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
		this.lstFiles.Size = new System.Drawing.Size(174, 69);
		this.lstFiles.TabIndex = 0;
		this.lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(lstFiles_DragEnter);
		this.lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(lstFiles_DragDrop);
		this.lstFiles.SelectedIndexChanged += new System.EventHandler(lstFiles_SelectedIndexChanged);
		this.btnRemoveFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnRemoveFiles.Enabled = false;
		this.btnRemoveFiles.Location = new System.Drawing.Point(60, 102);
		this.btnRemoveFiles.Name = "btnRemoveFiles";
		this.btnRemoveFiles.Size = new System.Drawing.Size(59, 24);
		this.btnRemoveFiles.TabIndex = 2;
		this.btnRemoveFiles.Text = "Remove";
		this.btnRemoveFiles.UseVisualStyleBackColor = true;
		this.btnRemoveFiles.Click += new System.EventHandler(btnRemoveFiles_Click);
		this.btnAddFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnAddFiles.Location = new System.Drawing.Point(125, 102);
		this.btnAddFiles.Name = "btnAddFiles";
		this.btnAddFiles.Size = new System.Drawing.Size(59, 24);
		this.btnAddFiles.TabIndex = 1;
		this.btnAddFiles.Text = "Add...";
		this.btnAddFiles.UseVisualStyleBackColor = true;
		this.btnAddFiles.Click += new System.EventHandler(btnAddFiles_Click);
		this.ftpProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ftpProgress.Location = new System.Drawing.Point(10, 348);
		this.ftpProgress.Name = "ftpProgress";
		this.ftpProgress.Size = new System.Drawing.Size(174, 78);
		this.ftpProgress.TabIndex = 20;
		this.ftpProgress.TransferFailed += new PostXING.Controls.TransferFailedEventHandler(ftpProgress_TransferFailed);
		this.ftpProgress.FileUploaded += new PostXING.Controls.FileUploadedEventHandler(ftpProgress_FileUploaded);
		this.mozPane1.BackColor = System.Drawing.Color.White;
		this.mozPane1.BorderColor = System.Drawing.Color.FromArgb(127, 157, 185);
		this.mozPane1.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.ImageList = null;
		this.mozPane1.ItemBorderStyles.Focus = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.ItemBorderStyles.Normal = System.Windows.Forms.ButtonBorderStyle.None;
		this.mozPane1.ItemBorderStyles.Selected = System.Windows.Forms.ButtonBorderStyle.Solid;
		this.mozPane1.ItemColors.Background = System.Drawing.Color.White;
		this.mozPane1.ItemColors.Border = System.Drawing.Color.Black;
		this.mozPane1.ItemColors.Divider = System.Drawing.Color.FromArgb(127, 157, 185);
		this.mozPane1.ItemColors.FocusBackground = System.Drawing.Color.FromArgb(224, 232, 246);
		this.mozPane1.ItemColors.FocusBorder = System.Drawing.Color.FromArgb(152, 180, 226);
		this.mozPane1.ItemColors.FocusText = System.Drawing.Color.Black;
		this.mozPane1.ItemColors.SelectedBackground = System.Drawing.Color.FromArgb(193, 210, 238);
		this.mozPane1.ItemColors.SelectedBorder = System.Drawing.Color.FromArgb(49, 106, 197);
		this.mozPane1.ItemColors.SelectedText = System.Drawing.Color.Black;
		this.mozPane1.ItemColors.Text = System.Drawing.Color.Black;
		this.mozPane1.Items.AddRange(new PostXING.Controls.Controls.MozItem[2] { this.mozItem1, this.mozItem2 });
		this.mozPane1.Location = new System.Drawing.Point(60, 261);
		this.mozPane1.MaxSelectedItems = 1;
		this.mozPane1.Name = "mozPane1";
		this.mozPane1.SelectButton = PostXING.Controls.Controls.MozSelectButton.Left;
		this.mozPane1.Size = new System.Drawing.Size(124, 81);
		this.mozPane1.Style = PostXING.Controls.Controls.MozPaneStyle.Vertical;
		this.mozPane1.TabIndex = 21;
		this.mozPane1.Theme = false;
		this.mozPane1.Toggle = false;
		this.mozPane1.ItemSelected += new PostXING.Controls.Controls.MozItemEventHandler(mozPane1_ItemSelected);
		this.mozItem1.Images.Focus = -1;
		this.mozItem1.Images.Normal = -1;
		this.mozItem1.Images.Selected = -1;
		this.mozItem1.ItemStyle = PostXING.Controls.Controls.MozItemStyle.Text;
		this.mozItem1.Location = new System.Drawing.Point(2, 2);
		this.mozItem1.Name = "mozItem1";
		this.mozItem1.Size = new System.Drawing.Size(120, 22);
		this.mozItem1.TabIndex = 0;
		this.mozItem1.Tag = "FTP";
		this.mozItem1.Text = "FTP";
		this.mozItem1.TextAlign = PostXING.Controls.Controls.MozTextAlign.Bottom;
		this.mozItem2.Images.Focus = -1;
		this.mozItem2.Images.Normal = -1;
		this.mozItem2.Images.Selected = -1;
		this.mozItem2.ItemStyle = PostXING.Controls.Controls.MozItemStyle.Text;
		this.mozItem2.Location = new System.Drawing.Point(2, 26);
		this.mozItem2.Name = "mozItem2";
		this.mozItem2.Size = new System.Drawing.Size(120, 22);
		this.mozItem2.TabIndex = 1;
		this.mozItem2.Tag = "BlogProvider";
		this.mozItem2.Text = "Blog Provider";
		this.mozItem2.TextAlign = PostXING.Controls.Controls.MozTextAlign.Bottom;
		this.label4.Location = new System.Drawing.Point(16, 261);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(40, 33);
		this.label4.TabIndex = 22;
		this.label4.Text = "Upload Using:";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.Controls.Add(this.label4);
		base.Controls.Add(this.mozPane1);
		base.Controls.Add(this.btnAddFiles);
		base.Controls.Add(this.btnRemoveFiles);
		base.Controls.Add(this.lstFiles);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnUpload);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.ftpProgress);
		base.Name = "UploadFilePage";
		this.Text = "Upload a File";
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.mozPane1).EndInit();
		this.mozPane1.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	private void btnCancel_Clicked(object sender, EventArgs e)
	{
		ApplyCancelButton();
	}

	private void btnUpload_Clicked(object sender, EventArgs e)
	{
		if (AppManager.CurrentBlog == null)
		{
			return;
		}
		switch (AppManager.Preferences.UploadUsing)
		{
		default:
			if (AppManager.CurrentBlog.FTPInfo != null && AppManager.CurrentBlog.FTPInfo.Target != null)
			{
				ftpSite = AppManager.CurrentBlog.FTPInfo;
				ftpProgress.Transfer((FTPControl.UploadedFileInfo[])new ArrayList(lstFiles.Items).ToArray(typeof(FTPControl.UploadedFileInfo)), ftpSite);
				break;
			}
			MessageBox.Show("There is no FTP site configured for this blog. To set one up, open Blog Settings.", "No FTP Site", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		case "BlogProvider":
			AppManager.CurrentProvider.UploadFile(AppManager.CurrentBlog, string.Empty, string.Empty);
			break;
		}
		btnCancel.Focus();
	}

	private void btnAddFiles_Click(object sender, EventArgs e)
	{
		if (ofdUploadFile.ShowDialog() == DialogResult.OK)
		{
			addFiles(ofdUploadFile.FileNames);
		}
	}

	private void addFiles(string[] paths)
	{
		foreach (string path in paths)
		{
			lstFiles.Items.Add(new FTPControl.UploadedFileInfo(path));
		}
		Button button = btnUpload;
		bool enabled = (btnRemoveFiles.Enabled = true);
		button.Enabled = enabled;
	}

	private void btnRemoveFiles_Click(object sender, EventArgs e)
	{
		foreach (object item in new ArrayList(lstFiles.SelectedItems))
		{
			lstFiles.Items.Remove(item);
		}
		if (lstFiles.Items.Count == 0)
		{
			Button button = btnUpload;
			bool enabled = (btnRemoveFiles.Enabled = false);
			button.Enabled = enabled;
			setModeEnable(enabled: false);
		}
	}

	private void setModeEnable(bool enabled)
	{
		RadioButton radioButton = rbAnchor;
		RadioButton radioButton2 = rbImage;
		bool flag = (rbNone.Enabled = enabled);
		bool enabled2 = (radioButton2.Enabled = flag);
		radioButton.Enabled = enabled2;
	}

	private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lstFiles.SelectedItems.Count >= 1)
		{
			FTPControl.UploadedFileInfo uploadedFileInfo = (FTPControl.UploadedFileInfo)lstFiles.SelectedItem;
			SuspendLayout();
			setModeEnable(enabled: true);
			switch (uploadedFileInfo.Mode)
			{
			case FTPControl.UploadedFileInfo.UploadMode.Anchor:
				rbAnchor.Select();
				break;
			case FTPControl.UploadedFileInfo.UploadMode.None:
				rbNone.Select();
				break;
			case FTPControl.UploadedFileInfo.UploadMode.Image:
				rbImage.Select();
				break;
			default:
				throw new InvalidOperationException("Unexpected file upload mode");
			}
			ResumeLayout();
		}
		else
		{
			setModeEnable(enabled: false);
		}
	}

	private void lstFiles_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent("FileDrop"))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void lstFiles_DragDrop(object sender, DragEventArgs e)
	{
		addFiles((string[])e.Data.GetData("FileDrop"));
	}

	private void modeChange(object sender, EventArgs e)
	{
		FTPControl.UploadedFileInfo uploadedFileInfo = (FTPControl.UploadedFileInfo)lstFiles.SelectedItem;
		if (sender == rbNone)
		{
			uploadedFileInfo.Mode = FTPControl.UploadedFileInfo.UploadMode.None;
			return;
		}
		if (sender == rbAnchor)
		{
			uploadedFileInfo.Mode = FTPControl.UploadedFileInfo.UploadMode.Anchor;
			return;
		}
		if (sender == rbImage)
		{
			uploadedFileInfo.Mode = FTPControl.UploadedFileInfo.UploadMode.Image;
			return;
		}
		throw new InvalidOperationException();
	}

	private void ftpProgress_TransferFailed(object sender, TransferFailedEventArgs e)
	{
		MessageBox.Show($"{e.Caption}!\r\nError message: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}

	private void ftpProgress_FileUploaded(object sender, FileUploadedEventArgs e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new FileUploadedEventHandler(ftpProgress_FileUploaded), sender, e);
		}
		lstFiles.Items.Remove(e.FileInfo);
	}

	private void mozPane1_ItemSelected(object sender, MozItemEventArgs e)
	{
		AppManager.Preferences.UploadUsing = e.MozItem.Tag.ToString();
		AppManager.Save(AppManager.Preferences);
	}
}
