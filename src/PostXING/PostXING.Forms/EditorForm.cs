using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using PostXING.Components;
using PostXING.Controls;
using PostXING.Controls.HtmlEditor;
using PostXING.Controls.HtmlEditor.Html;
using PostXING.Controls.Navigation;
using PostXING.Dialogs;
using PostXING.Extensibility;
using PostXING.NavigationPages;
using Syndication.Extensibility;

namespace PostXING.Forms;

public class EditorForm : Form, IEditorForm, IMessageFilter
{
	private delegate void SetIsEditingDelegate(bool value);

	private delegate bool BlogItemDelegate(object sender, out string message);

	private delegate void BlogItemComplete(object successful, string message);

	private delegate void ResetMessageDelegate(string message);

	private PostXINGAppContext context = new PostXINGAppContext();

	private MessageBoxDialog dlg;

	private ControlPanelHomePage controlPanelHomePage1;

	private MessageBoxContainer ctnr;

	private int _controlPanelWidth = 197;

	private int _placeHolderIndex;

	private static bool formInitialized = false;

	private bool _ensureNotMinimized;

	private DarkRoom dr = new DarkRoom();

	private bool _isDarkRoomEnabled;

	private FormWindowState _preDarkRoomState;

	private BlogPost _currentPost = new BlogPost();

	private object _postID;

	private bool _isEditing;

	private static int WM_QUERYENDSESSION = 17;

	private Control _lastActiveControl = new Control();

	private IContainer components;

	private ToolStripButton rebar_Categories;

	private ToolStripButton rebar_CrossPost;

	private ToolStripButton prebar_Categories;

	private ToolStripButton prebar_CrossPost;

	private ImageList imgButtonBar;

	private StatusBarProgress statusBarProgress1;

	private XPTextBox txtTitle;

	private Label label1;

	private Label label2;

	private XPTextBox txtUrl;

	private Panel pnlEditorContainer;

	private SaveFileDialog sfdSavePost;

	private OpenFileDialog ofdOpenPost;

	private StaticFrame frameMainView;

	private StaticFrame frameControlPanel;

	private EditorPage editorPage1;

	private AboutPage aboutPage1;

	private UploadFilePage uploadFilePage1;

	private HistoryPage historyPage1;

	private HistoryMainView historyMainView1;

	private GradientPanel gradientPanel1;

	private GradientPanel HistoryControlPanel;

	private GradientPanel gradientPanel2;

	private Label lblHistoryDate;

	private Label lblHistoryEntryID;

	private Label label3;

	private Label label5;

	private Label label6;

	private PictureBox btnHistoryRefresh;

	private System.Windows.Forms.LinkLabel lnkPostUrl;

	private Label label4;

	private PictureBox btnDeletePost;

	private PictureBox btnEditPost;

	private Label label7;

	private CategoriesPage categoriesPage1;

	private CrossPostPage crossPostPage1;

	private BlogManagerPage blogManagerPage1;

	private NotifyIcon notifyIcon;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem fileToolStripMenuItem1;

	private ToolStripMenuItem newToolStripMenuItem;

	private ToolStripMenuItem openToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripMenuItem saveToolStripMenuItem;

	private ToolStripMenuItem saveAsToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem placeHolderToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripMenuItem exitToolStripMenuItem;

	private ToolStripMenuItem viewToolStripMenuItem;

	private ToolStripMenuItem viewBlogToolStripMenuItem;

	private ToolStripMenuItem historyToolStripMenuItem;

	private ToolStripMenuItem categoriesToolStripMenuItem;

	private ToolStripMenuItem toolsToolStripMenuItem;

	private ToolStripMenuItem optionsToolStripMenuItem;

	private ToolStripMenuItem helpToolStripMenuItem;

	private ToolStripMenuItem aboutToolStripMenuItem;

	private ToolStripMenuItem testToolStripMenuItem;

	private ToolStripMenuItem crossPostToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem uploadFileToolStripMenuItem;

	private ToolStripMenuItem newBlogToolStripMenuItem;

	private ToolStripMenuItem manageBlogsToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripMenuItem pluginsToolStripMenuItem;

	private ToolStripMenuItem noPluginsLoadedToolStripMenuItem;

	private ToolStripMenuItem iBlogExtensionsToolStripMenuItem;

	private ToolStripMenuItem noIBlogExtensionsLoadedToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStrip toolStrip1;

	private ToolStripButton newToolStripButton;

	private ToolStripButton openToolStripButton;

	private ToolStripButton saveToolStripButton;

	private ToolStripButton uploadToolStripButton;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton newBlogToolStripButton;

	private ToolStripButton viewBlogToolStripButton;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton optionsToolStripButton;

	private ToolStripMenuItem testProgressToolStripMenuItem;

	private ToolStripMenuItem testMessageBoxContainerToolStripMenuItem;

	private ToolStripMenuItem testHeaderStripToolStripMenuItem;

	private ToolStripComboBox cbxiUsersBlogs;

	private ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator8;

	private ToolStripContainer toolStripContainer1;

	private SplitContainer splitContainer1;

	private HeaderStrip headerStrip1;

	private ToolStripButton backToolStripButton;

	private ToolStripButton nextToolStripButton;

	private ToolStripSeparator toolStripSeparator9;

	private ToolStripButton toolStripButton4;

	private CompletionCombo fcbxNumberOfPosts;

	private ToolStripMenuItem editToolStripMenuItem;

	private ToolStripMenuItem undoToolStripMenuItem;

	private ToolStripMenuItem redoToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator10;

	private ToolStripMenuItem cutToolStripMenuItem;

	private ToolStripMenuItem copyToolStripMenuItem;

	private ToolStripMenuItem pasteToolStripMenuItem;

	private ToolStripMenuItem pasteSpecialToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator11;

	private ToolStripMenuItem findToolStripMenuItem;

	private ToolStripMenuItem findNextToolStripMenuItem;

	private ToolStripMenuItem replaceToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator12;

	private ToolStripMenuItem selectAllToolStripMenuItem;

	private HeaderStrip headerStrip2;

	private ToolStripLabel controlPanelToolStripLabel;

	private ToolStripButton closeControlPanelToolStripButton;

	private Timer AutoSaveTimer;

	private ToolStripMenuItem draftsToolStripMenuItem;

	private AutoSavePage autoSavePage1;

	private ToolStripMenuItem testDarkRoomConfigToolStripMenuItem;

	private ToolStripMenuItem testKeyDataToolStripMenuItem;

	public BlogPost CurrentPost
	{
		get
		{
			if (formInitialized)
			{
				_currentPost.Body = editorPage1.Html;
			}
			_currentPost.DateCreated = DateTime.Now;
			_currentPost.Link = txtUrl.Text;
			_currentPost.Title = txtTitle.Text;
			return _currentPost;
		}
		set
		{
			_currentPost = value;
		}
	}

	public object PostID
	{
		get
		{
			return _postID;
		}
		set
		{
			_postID = value;
		}
	}

	public bool IsEditing
	{
		get
		{
			return _isEditing;
		}
		set
		{
			if (base.InvokeRequired)
			{
				Invoke(new SetIsEditingDelegate(SetIsEditingTrue), value);
			}
			else
			{
				SetIsEditingTrue(value);
			}
		}
	}

	BlogPost IEditorForm.CurrentPost => CurrentPost;

	public string UserAgent => Application.ProductName + " v" + Application.ProductVersion;

	public Blog CurrentBlog => AppManager.CurrentBlog;

	public Control LastActiveControl
	{
		get
		{
			return _lastActiveControl;
		}
		set
		{
			_lastActiveControl = value;
		}
	}

	public event EventHandler NewPostCreatedEventHandler;

	public EditorForm()
	{
		InitializeComponent();
		_placeHolderIndex = fileToolStripMenuItem1.DropDownItems.IndexOf(placeHolderToolStripMenuItem);
		InitializeForm();
		InitializeRebar();
		context.Editor = AppManager.EditorForm;
		_ = base.DesignMode;
		cbxiUsersBlogs.ComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
		AppManager.PostSelected = (PostSelectedEventHandler)Delegate.Combine(AppManager.PostSelected, new PostSelectedEventHandler(historyPage1_PostSelected));
	}

	public void SetIsEditingTrue(bool value)
	{
		_isEditing = value;
		if (AppManager.CurrentProvider != null)
		{
			ToolStripButton[] deleteButtons = AppManager.CurrentProvider.DeleteButtons;
			foreach (ToolStripButton toolStripButton in deleteButtons)
			{
				toolStripButton.Visible = value;
			}
		}
	}

	private void InitializeForm()
	{
		frameMainView.HomePage = editorPage1;
		editorPage1.Dock = DockStyle.Fill;
		frameMainView.Go(editorPage1);
		frameControlPanel.HomePage = controlPanelHomePage1;
		frameControlPanel.GoHome();
		frameControlPanel.Navigate += frameControlPanel_Navigate;
		uploadFilePage1.FileUploaded += uploadFilePage1_FileUploaded;
		historyMainView1.PageEnter += historyMainView1_PageEnter;
		historyMainView1.PageLeave += historyMainView1_PageLeave;
		AppManager.OnApplyPreferences += AppManager_OnApplyPreferences;
	}

	private void InitializeRebar()
	{
		rebar_Categories = new ToolStripButton();
		rebar_CrossPost = new ToolStripButton();
		prebar_Categories = new ToolStripButton();
		prebar_CrossPost = new ToolStripButton();
		editorPage1.SuspendLayout();
		editorPage1.ButtonBar.Items.AddRange(new ToolStripButton[2] { rebar_Categories, rebar_CrossPost });
		editorPage1.PreviewBar.Items.AddRange(new ToolStripButton[2] { prebar_Categories, prebar_CrossPost });
		ToolStripButton toolStripButton = rebar_CrossPost;
		Image image = (prebar_CrossPost.Image = crossPostToolStripMenuItem.Image);
		toolStripButton.Image = image;
		ToolStripButton toolStripButton2 = prebar_CrossPost;
		object tag = (rebar_CrossPost.Tag = "CrossPost");
		toolStripButton2.Tag = tag;
		ToolStripButton toolStripButton3 = prebar_CrossPost;
		string text = (rebar_CrossPost.Text = "Cross Post");
		toolStripButton3.Text = text;
		ToolStripButton toolStripButton4 = prebar_CrossPost;
		string toolTipText = (rebar_CrossPost.ToolTipText = "Post a new message to more than one Blog.");
		toolStripButton4.ToolTipText = toolTipText;
		ToolStripButton toolStripButton5 = prebar_CrossPost;
		ToolStripItemDisplayStyle displayStyle = (rebar_CrossPost.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText);
		toolStripButton5.DisplayStyle = displayStyle;
		prebar_CrossPost.Click += tools_CrossPost_Activate;
		rebar_CrossPost.Click += tools_CrossPost_Activate;
		ToolStripButton toolStripButton6 = rebar_Categories;
		Image image3 = (prebar_Categories.Image = categoriesToolStripMenuItem.Image);
		toolStripButton6.Image = image3;
		ToolStripButton toolStripButton7 = rebar_Categories;
		object tag2 = (prebar_Categories.Tag = "Categories");
		toolStripButton7.Tag = tag2;
		ToolStripButton toolStripButton8 = rebar_Categories;
		string text4 = (prebar_Categories.Text = "Categories");
		toolStripButton8.Text = text4;
		ToolStripButton toolStripButton9 = rebar_Categories;
		string toolTipText2 = (rebar_Categories.ToolTipText = categoriesToolStripMenuItem.ToolTipText);
		toolStripButton9.ToolTipText = toolTipText2;
		ToolStripButton toolStripButton10 = prebar_Categories;
		ToolStripItemDisplayStyle displayStyle2 = (rebar_Categories.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText);
		toolStripButton10.DisplayStyle = displayStyle2;
		prebar_Categories.Click += view_Categories_Activate;
		rebar_Categories.Click += view_Categories_Activate;
		editorPage1.ResumeLayout(performLayout: false);
	}

	private void InitializeIPlugins()
	{
		context.InitPlugins();
		if (context.Plugins.Count > 0)
		{
			pluginsToolStripMenuItem.DropDownItems.Clear();
		}
		editorPage1.SuspendLayout();
		IIPluginEnumerator enumerator = context.Plugins.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IPlugin current = enumerator.Current;
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(current.MenuCaption);
				toolStripMenuItem.ShortcutKeys = (Keys)current.Shortcut;
				toolStripMenuItem.Tag = current;
				ToolStripButton toolStripButton = new ToolStripButton();
				ToolStripButton toolStripButton2 = new ToolStripButton();
				toolStripButton.Tag = current;
				toolStripButton2.Tag = current;
				if (current.Icon != null)
				{
					Bitmap image = (Bitmap)(toolStripButton2.Image = (toolStripButton.Image = current.Icon.ToBitmap()));
					toolStripMenuItem.Image = image;
				}
				toolStripButton.Click += HandlePlugin;
				toolStripButton2.Click += HandlePlugin;
				toolStripMenuItem.Click += HandlePlugin;
				if (current.BeginGroup)
				{
					pluginsToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
					editorPage1.ButtonBar.Items.Add(new ToolStripSeparator());
					editorPage1.PreviewBar.Items.Add(new ToolStripSeparator());
				}
				pluginsToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
				if (current.HasConfiguration)
				{
					ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(current.MenuCaption + " - Configure");
					toolStripMenuItem2.Click += HandlePluginConfiguration;
					toolStripMenuItem2.Tag = current;
					pluginsToolStripMenuItem.DropDownItems.Add(toolStripMenuItem2);
				}
				editorPage1.ButtonBar.Items.Add(toolStripButton);
				editorPage1.PreviewBar.Items.Add(toolStripButton2);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		editorPage1.ResumeLayout(performLayout: false);
		context.CurrentEditorTextChanged += context_CurrentEditorTextChanged;
	}

	private void InitializeIBlogExtensions()
	{
		ArrayList arrayList = AppManager.LoadIBlogExtensions();
		if (arrayList.Count > 0)
		{
			iBlogExtensionsToolStripMenuItem.DropDownItems.Clear();
		}
		foreach (IBlogExtension item in arrayList)
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(item.DisplayName);
			toolStripMenuItem.Click += HandleIBlogExtension;
			toolStripMenuItem.Tag = item;
			iBlogExtensionsToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
			if (item.HasConfiguration)
			{
				ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(item.DisplayName);
				toolStripMenuItem2.Click += HandleIBlogExtensionConfiguration;
				toolStripMenuItem2.Tag = item;
				iBlogExtensionsToolStripMenuItem.DropDownItems.Add(toolStripMenuItem2);
			}
		}
	}

	private void InitializeAutoSave()
	{
		AutoSaveTimer.Enabled = AppManager.Preferences.AutoSaveEnabled;
		AutoSaveTimer.Interval = AppManager.Preferences.AutoSaveIntervalInMinutes * 60 * 1000;
	}

	private bool OkToClose()
	{
		if (editorPage1.IsDirty && MessageBox.Show("You have un-posted work. Are you sure you want to exit?", "Un-Posted Work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
		{
			return false;
		}
		return true;
	}

	private bool LoadExistingPost()
	{
		if (AppManager.TempFileLocation == string.Empty)
		{
			return false;
		}
		CurrentPost = AppManager.LoadExistingPost(AppManager.TempFileLocation);
		txtTitle.Text = CurrentPost.Title;
		txtUrl.Text = CurrentPost.Link;
		editorPage1.DesignEditor.LoadHtml(CurrentPost.Body);
		formInitialized = true;
		return true;
	}

	internal void LoadBlogs()
	{
		cbxiUsersBlogs.ComboBox.Items.Clear();
		for (int i = 0; i < AppManager.Blogs.Count; i++)
		{
			cbxiUsersBlogs.ComboBox.Items.Add(AppManager.Blogs[i]);
		}
		if (AppManager.CurrentBlog != null)
		{
			int num = AppManager.Blogs.IndexOf(AppManager.CurrentBlog);
			cbxiUsersBlogs.ComboBox.SelectedIndex = ((num > -1) ? num : 0);
		}
	}

	private void SetPreviewTemplate()
	{
		if (AppManager.CurrentBlog != null)
		{
			string previewTemplate = AppManager.CurrentBlog.Options.PreviewTemplate;
			previewTemplate = previewTemplate.Replace("[Post Title]", txtTitle.Text);
			previewTemplate = previewTemplate.Replace("{", "{{").Replace("}", "}}");
			previewTemplate = previewTemplate.Replace("[Post Url]", "");
			previewTemplate = previewTemplate.Replace("[Post Body]", "{0}");
			editorPage1.PreviewTemplate = previewTemplate;
			editorPage1.RefreshDisplay();
		}
	}

	private bool BlogItem(object sender, out string message)
	{
		ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
		bool result = false;
		message = "Posting";
		if (toolStripMenuItem != null)
		{
			string html = editorPage1.Html;
			XmlDocument xmlDocument = new XmlDocument();
			string xml = $"<rss>\r\n\t\t\t\t\t<channel>\r\n\t\t\t\t\t<title/>\r\n\t\t\t\t\t<link/>\r\n\t\t\t\t\t<description/>\r\n\t\t\t\t\t\t<item>\r\n\t\t\t\t\t\t\t<title>{txtTitle.Text}</title>\r\n\t\t\t\t\t\t\t<link>{txtUrl.Text}</link>\r\n\t\t\t\t\t\t\t<description><![CDATA[{html}]]></description>\r\n\t\t\t\t\t\t</item>\r\n\t\t\t\t\t</channel>\r\n\t\t\t\t</rss>";
			xmlDocument.LoadXml(xml);
			string text = "Unknown";
			try
			{
				IBlogExtension blogExtension = (IBlogExtension)toolStripMenuItem.Tag;
				text = blogExtension.DisplayName;
				statusBarProgress1.StartProgress(message + " " + text);
				blogExtension.BlogItem(xmlDocument, edited: true);
				result = true;
				message = text + ": Posted Successfully";
			}
			catch (Exception ex)
			{
				result = false;
				message = text + ": " + ex.Message;
			}
		}
		return result;
	}

	private void BlogItemCallback(IAsyncResult asr)
	{
		try
		{
			BlogItemDelegate blogItemDelegate = (BlogItemDelegate)asr.AsyncState;
			string message;
			object obj = blogItemDelegate.EndInvoke(out message, asr);
			BlogItemComplete blogItemComplete = BlogItemCompleted;
			if (base.InvokeRequired)
			{
				Invoke(blogItemComplete, obj, message);
			}
			else
			{
				blogItemComplete(obj, message);
			}
		}
		catch (Exception ex)
		{
			statusBarProgress1.StopProgress();
			statusBarProgress1.ResetProgress(ex.Message);
		}
	}

	private void BlogItemCompleted(object successful, string message)
	{
		statusBarProgress1.StopProgress();
		if ((bool)successful)
		{
			statusBarProgress1.ResetProgress(message);
		}
		else
		{
			statusBarProgress1.ResetProgress("Error : " + message);
		}
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		switch (keyData)
		{
		case Keys.Return | Keys.Control:
		case Keys.Return | Keys.Shift | Keys.Control:
		{
			CurrentPost.Publish = (keyData & Keys.Shift) == 0;
			string message = (CurrentPost.Publish ? "Publishing message '{0}'" : "Posting a draft of '{0}'");
			StartProgress(message, CurrentPost.Title);
			PostOrPublish();
			return true;
		}
		case Keys.Escape:
			if (_isDarkRoomEnabled)
			{
				SuspendLayout();
				base.FormBorderStyle = FormBorderStyle.Sizable;
				base.WindowState = _preDarkRoomState;
				BackColor = Color.FromKnownColor(KnownColor.Control);
				base.Padding = new System.Windows.Forms.Padding(0);
				BlogPost currentPost = CurrentPost;
				string title = (txtTitle.Text = dr.Title);
				currentPost.Title = title;
				editorPage1.SetHtml(dr.Html);
				base.Controls.Remove(dr);
				ResumeLayout();
				_isDarkRoomEnabled = false;
			}
			else
			{
				testDarkRoomConfigToolStripMenuItem_Click(null, EventArgs.Empty);
			}
			return true;
		default:
			return base.ProcessCmdKey(ref msg, keyData);
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

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == WM_QUERYENDSESSION)
		{
			Application.Exit();
		}
		base.WndProc(ref m);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (!OkToClose())
		{
			e.Cancel = true;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!base.DesignMode)
		{
			CloseControlPanel();
			LoadBlogs();
			SetPreviewTemplate();
			InitializeAutoSave();
			if (!LoadExistingPost())
			{
				ResetNewPost();
			}
		}
	}

	private void HandlePlugin(object sender, EventArgs e)
	{
		if (sender is ToolStripMenuItem toolStripMenuItem)
		{
			PluginClick(sender, e, (IPlugin)toolStripMenuItem.Tag);
		}
		else if (sender is ToolStripButton toolStripButton)
		{
			PluginClick(sender, e, (IPlugin)toolStripButton.Tag);
		}
	}

	private void PluginClick(object sender, EventArgs e, IPlugin plug)
	{
		AssignCurrentEditorText();
		plug.Init(context);
		plug.OnClick(sender, e);
	}

	private void AssignCurrentEditorText()
	{
		context.CurrentEditorText = string.Format("<title source='{1}'>{0}</title>", txtTitle.Text, txtUrl.Text);
		context.CurrentEditorText += editorPage1.Html;
	}

	private void HandlePluginConfiguration(object sender, EventArgs e)
	{
		if (sender is ToolStripMenuItem toolStripMenuItem)
		{
			context.IsCallingFromHostApplication = true;
			AssignCurrentEditorText();
			IPlugin plugin = (IPlugin)toolStripMenuItem.Tag;
			plugin.Init(context);
			plugin.Configure(this);
		}
	}

	private void HandleIBlogExtension(object sender, EventArgs e)
	{
		BlogItemDelegate blogItemDelegate = BlogItem;
		string message = "Posting";
		blogItemDelegate.BeginInvoke(sender, out message, BlogItemCallback, blogItemDelegate);
	}

	private void HandleIBlogExtensionConfiguration(object sender, EventArgs e)
	{
		if (sender is ToolStripMenuItem toolStripMenuItem)
		{
			IBlogExtension blogExtension = (IBlogExtension)toolStripMenuItem.Tag;
			blogExtension.Configure(this);
		}
	}

	private void help_About_Activate(object sender, EventArgs e)
	{
		aboutPage1.Visible = true;
		frameMainView.Go(aboutPage1);
	}

	private void tools_UploadFile_Activate(object sender, EventArgs e)
	{
		uploadFilePage1.Visible = true;
		ControlPanelNavigate(uploadFilePage1);
	}

	private void txtTitle_Leave(object sender, EventArgs e)
	{
		SetTitleText();
	}

	private void SetTitleText()
	{
		if (txtTitle.Text.Trim().Length > 0)
		{
			Text = txtTitle.Text;
			SetPreviewTemplate();
		}
		else
		{
			Text = "PostXING";
		}
	}

	private void file_New_Activate(object sender, EventArgs e)
	{
		ResetNewPost();
	}

	private void view_History_Activate(object sender, EventArgs e)
	{
		historyPage1.Visible = true;
		ControlPanelNavigate(historyPage1);
		ShowHistory();
	}

	private void tools_NewBlog_Activate(object sender, EventArgs e)
	{
		using NewUserDialog newUserDialog = new NewUserDialog();
		newUserDialog.ShowDialog();
	}

	private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbxiUsersBlogs.ComboBox.SelectedIndex > -1)
		{
			Blog blog = (Blog)cbxiUsersBlogs.ComboBox.SelectedItem;
			AppManager.SetSelected(blog);
			AppManager.Save(AppManager.Blogs);
			if (AppManager.CurrentProvider == null || AppManager.CurrentProvider.ProviderName != blog.ProviderName)
			{
				AppManager.CurrentProvider = AppManager.AvailableProviders[blog.ProviderName];
				AppManager.CurrentProvider.InitializeIBlogProvider(AppManager.OptionsDialog, AppManager.EditorForm);
				LoadToolbarButtons();
			}
			fcbxNumberOfPosts.Text = blog.ItemCount.ToString();
			ToolStripMenuItem toolStripMenuItem = categoriesToolStripMenuItem;
			ToolStripButton toolStripButton = rebar_Categories;
			bool flag = (prebar_Categories.Enabled = AppManager.CurrentProvider.SupportsCategories);
			bool enabled = (toolStripButton.Enabled = flag);
			toolStripMenuItem.Enabled = enabled;
			SetPreviewTemplate();
		}
	}

	private void view_Blog_Activate(object sender, EventArgs e)
	{
		try
		{
			Help.ShowHelp(this, AppManager.CurrentBlog.WebAddress);
		}
		catch
		{
			try
			{
				Uri uri = new Uri(AppManager.CurrentBlog.ServiceUrl);
				Help.ShowHelp(this, uri.Scheme + "://" + AppManager.CurrentBlog.Host + AppManager.CurrentBlog.WebAddress);
			}
			catch
			{
				MessageBox.Show(this, "Sorry, your blog engine has returned a web address that is not recognized. There's no way to browse to your site with the url: " + AppManager.CurrentBlog.WebAddress, "URL Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}

	private void txtUrl_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.Text))
		{
			e.Effect = DragDropEffects.All;
		}
	}

	private void txtUrl_DragDrop(object sender, DragEventArgs e)
	{
		txtUrl.Text = e.Data.GetData(DataFormats.Text).ToString();
	}

	private void file_Exit_Activate(object sender, EventArgs e)
	{
		Close();
	}

	private void view_Categories_Activate(object sender, EventArgs e)
	{
		ControlPanelNavigate(categoriesPage1);
		if (IsEditing)
		{
			categoriesPage1.BindCategories(CurrentPost.Categories);
		}
	}

	internal void ControlPanelNavigate(string pageName)
	{
		switch (pageName)
		{
		case "categories":
			ControlPanelNavigate(categoriesPage1);
			break;
		case "crosspost":
			ControlPanelNavigate(crossPostPage1);
			break;
		case "history":
			ControlPanelNavigate(historyPage1);
			ShowHistory();
			break;
		case "upload":
			ControlPanelNavigate(uploadFilePage1);
			break;
		case "autosave":
			ControlPanelNavigate(autoSavePage1);
			break;
		}
	}

	internal void ControlPanelNavigate(Page currentPage)
	{
		SuspendLayout();
		splitContainer1.SplitterDistance = base.Width - _controlPanelWidth;
		splitContainer1.SplitterWidth = 4;
		splitContainer1.Panel2.Show();
		controlPanelToolStripLabel.Text = currentPage.Text;
		ResumeLayout(performLayout: false);
		frameControlPanel.Go(currentPage);
	}

	private void tools_CrossPost_Activate(object sender, EventArgs e)
	{
		ControlPanelNavigate(crossPostPage1);
	}

	private void context_CurrentEditorTextChanged(object sender, EventArgs e)
	{
		HPathDocument hPathDocument = new HPathDocument();
		hPathDocument.LoadHtml(context.CurrentEditorText);
		HtmlNode htmlNode = hPathDocument.DocumentNode.SelectSingleNode("//meta[@name='categories']");
		if (htmlNode != null)
		{
			string attributeValue = htmlNode.GetAttributeValue("content", "");
			CurrentPost.Categories = attributeValue.Split(';');
		}
		HtmlNode htmlNode2 = hPathDocument.DocumentNode.SelectSingleNode("//title[1]");
		if (htmlNode2 != null)
		{
			txtTitle.Text = htmlNode2.InnerText;
			SetTitleText();
			txtUrl.Text = htmlNode2.GetAttributeValue("source", "");
		}
		editorPage1.DesignEditor.LoadHtml(context.CurrentEditorText);
	}

	private void savePost(BlogPost post)
	{
		using StreamWriter streamWriter = File.CreateText(post.FileName);
		if (post.Categories != null)
		{
			streamWriter.Write(string.Format("<meta name='categories' content={0}>", string.Join(";", post.Categories)));
		}
		streamWriter.Write(string.Format("<title source='{1}'>{0}</title>", post.Title, post.PermaLink));
		streamWriter.Write(post.Body);
		streamWriter.Close();
		ResetMessage("Post saved as " + post.FileName);
	}

	private void mbiSaveAs_Activate(object sender, EventArgs e)
	{
		string text = "PostXING";
		if (txtTitle.Text.Trim().Length > 0)
		{
			text = txtTitle.Text;
		}
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		foreach (char oldChar in invalidFileNameChars)
		{
			text = text.Replace(oldChar, '_');
		}
		text = text.Replace(':', '_');
		text += ".htm";
		sfdSavePost.FileName = text;
		sfdSavePost.Title += text;
		if (sfdSavePost.ShowDialog(this) == DialogResult.OK && sfdSavePost.FileName != "")
		{
			CurrentPost.FileName = sfdSavePost.FileName;
			savePost(CurrentPost);
		}
	}

	private void mbiSavePost_Activate(object sender, EventArgs e)
	{
		if (CurrentPost.FileName == null)
		{
			mbiSaveAs_Activate(sender, e);
		}
		else
		{
			savePost(CurrentPost);
		}
	}

	private void file_Open_Activate(object sender, EventArgs e)
	{
		if (ofdOpenPost.ShowDialog(this) == DialogResult.OK && ofdOpenPost.FileName != "")
		{
			CurrentPost.FileName = ofdOpenPost.FileName;
			using StreamReader streamReader = File.OpenText(ofdOpenPost.FileName);
			context.CurrentEditorText = streamReader.ReadToEnd();
			streamReader.Close();
			context_CurrentEditorTextChanged(sender, e);
			frameMainView.GoHome();
		}
	}

	private void tools_Options_Activate(object sender, EventArgs e)
	{
		if (AppManager.CurrentBlog == null)
		{
			tools_NewBlog_Activate(sender, e);
		}
		AppManager.OptionsDialog.CurrentBlog = AppManager.CurrentBlog;
		AppManager.OptionsDialog.CurrentProvider = AppManager.CurrentProvider;
		AppManager.OptionsDialog.IsEditing = true;
		AppManager.OptionsDialog.ShowOptions();
	}

	private void btniHome_Activate(object sender, EventArgs e)
	{
		frameControlPanel.GoHome();
	}

	private void btniNext_Activate(object sender, EventArgs e)
	{
		frameControlPanel.GoForward();
	}

	private void btniPrevious_Activate(object sender, EventArgs e)
	{
		frameControlPanel.GoBack();
	}

	private void uploadFilePage1_FileUploaded(object sender, FileUploadedEventArgs e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new FileUploadedEventHandler(uploadFilePage1_FileUploaded), sender, e);
			return;
		}
		editorPage1.DesignEditor.InsertHtml(e.Html);
		if (editorPage1.CurrentEditorMode != EditorMode.Design)
		{
			editorPage1.SetHtml(editorPage1.StripHTMLBody());
			editorPage1.RefreshDisplay();
		}
	}

	private void frameControlPanel_Navigate(object sender, PostXING.Controls.Navigation.NavigateEventArgs e)
	{
		controlPanelToolStripLabel.Text = frameControlPanel.CurrentPage.Text;
	}

	private void mbiTestProgress_Activate(object sender, EventArgs e)
	{
		statusBarProgress1.StartProgress("Testing...");
		for (int i = 0; i < 10; i++)
		{
			Console.WriteLine();
		}
		statusBarProgress1.StopProgress();
	}

	private void historyMainView1_PageEnter(object sender, PageEventArgs e)
	{
		gradientPanel1.BringToFront();
		HistoryControlPanel.BringToFront();
	}

	private void historyMainView1_PageLeave(object sender, PageEventArgs e)
	{
		gradientPanel1.SendToBack();
		HistoryControlPanel.SendToBack();
	}

	private void AutoSaveTimer_Tick(object sender, EventArgs e)
	{
		if (!AppManager.Preferences.AutoSaveEnabled)
		{
			AutoSaveTimer.Enabled = false;
		}
		else if ((!string.IsNullOrEmpty(CurrentPost.Title) || !(CurrentPost.Body == "\r\n<p>&nbsp;</p>")) && !string.IsNullOrEmpty(CurrentPost.Body))
		{
			string fileName = CurrentPost.FileName;
			if (AutoSaveFileNameIsPathed())
			{
				string autoSaveFileName = Path.Combine(Path.Combine(AppManager.GetUserPath(), "drafts"), CurrentPost.AutoSaveFileName + ".htm");
				CurrentPost.AutoSaveFileName = autoSaveFileName;
			}
			CurrentPost.FileName = CurrentPost.AutoSaveFileName;
			savePost(CurrentPost);
			autoSavePage1.RefreshDisplay();
			ResetMessage("Post '" + CurrentPost.Title + "' was autosaved at " + DateTime.Now.ToString(), preserveCurrentPost: true);
			CurrentPost.FileName = fileName;
		}
	}

	private void AppManager_OnApplyPreferences(object sender, EventArgs e)
	{
		InitializeAutoSave();
	}

	private bool AutoSaveFileNameIsPathed()
	{
		string path = Path.Combine(AppManager.GetUserPath(), "drafts");
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		return !CurrentPost.AutoSaveFileName.Contains(":");
	}

	public void ResetMessage(string message, bool preserveCurrentPost)
	{
		if (preserveCurrentPost)
		{
			ResetMessage(message);
		}
		else if (base.InvokeRequired)
		{
			Invoke(new ResetMessageDelegate(ResetNewPost), message);
		}
		else
		{
			ResetNewPost(message);
		}
	}

	public void ResetMessage(string message)
	{
		if (base.InvokeRequired)
		{
			Invoke(new ResetMessageDelegate(ResetMessage), message);
		}
		else
		{
			statusBarProgress1.ResetProgress(message);
			dr.StatusBarMessage = message;
		}
	}

	private void ResetNewPost()
	{
		ResetNewPost("New Post");
	}

	private void ResetNewPost(string StatusBarMessage)
	{
		if (AppManager.CurrentBlog != null && OkToClose())
		{
			IsEditing = false;
			txtTitle.Clear();
			txtUrl.Clear();
			Text = "PostXING";
			frameMainView.GoHome();
			editorPage1.CurrentEditorMode = EditorMode.Design;
			string text = "<p>&nbsp;</p>";
			if (AppManager.CurrentBlog.Options.IncludeMediaFormatter)
			{
				text += MediaPlayerInfo.GetCurrentlyPlayingMedia(AppManager.CurrentBlog.Options);
			}
			editorPage1.SetHtml(text);
			if (AutoSaveFileNameIsPathed() && File.Exists(CurrentPost.AutoSaveFileName))
			{
				File.Delete(CurrentPost.AutoSaveFileName);
			}
			CurrentPost = new BlogPost();
			CurrentPost.Body = text;
			if (this.NewPostCreatedEventHandler != null)
			{
				Invoke(this.NewPostCreatedEventHandler, this, EventArgs.Empty);
			}
			formInitialized = true;
			statusBarProgress1.ResetProgress(StatusBarMessage);
			dr.StatusBarMessage = StatusBarMessage;
		}
	}

	public void ShowHistory()
	{
		historyMainView1.Visible = true;
		frameMainView.Go(historyMainView1);
	}

	public void StopProgress()
	{
		if (base.InvokeRequired)
		{
			Invoke(new MethodInvoker(statusBarProgress1.StopProgress));
		}
		else
		{
			statusBarProgress1.StopProgress();
		}
	}

	public void StartProgress(string message, params object[] args)
	{
		statusBarProgress1.StartProgress(string.Format(message, args));
		dr.StatusBarMessage = string.Format(message, args);
	}

	public void LoadToolbarButtons()
	{
		editorPage1.SuspendLayout();
		ToolStripButton[] buttonBarItems = AppManager.CurrentProvider.ButtonBarItems;
		foreach (ToolStripButton toolStripButton in buttonBarItems)
		{
			editorPage1.ButtonBar.Items.Insert(toolStripButton.Owner.Items.IndexOf(toolStripButton), toolStripButton);
		}
		ToolStripButton[] previewBarItems = AppManager.CurrentProvider.PreviewBarItems;
		foreach (ToolStripButton toolStripButton2 in previewBarItems)
		{
			editorPage1.PreviewBar.Items.Insert(toolStripButton2.Owner.Items.IndexOf(toolStripButton2), toolStripButton2);
		}
		editorPage1.ResumeLayout(performLayout: false);
	}

	private void menuButtonItem3_Activate(object sender, EventArgs e)
	{
		dlg = new MessageBoxDialog();
		base.Controls.Add(dlg);
		dlg.BringToFront();
		dlg.Location = new Point(0, 0);
		dlg.Visible = true;
		dlg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		dlg.ShowMessage("This is just a test message. Don't panic.", "Test Message", MessageBoxButtons.OK);
		dlg.Closing += dlg_Closing;
	}

	private void dlg_Closing(object sender, CancelEventArgs e)
	{
		base.Controls.Remove(dlg);
	}

	private void menuButtonItem4_Activate(object sender, EventArgs e)
	{
		ctnr = new MessageBoxContainer();
		base.Controls.Add(ctnr);
		ctnr.BringToFront();
		ctnr.Location = new Point(0, 0);
		ctnr.Size = base.Size;
		ctnr.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		ctnr.Visible = true;
		ctnr.ShowMessage("This is a control-based test message. Don't panic.", "Test Message 2");
		ctnr.Closing += ctnr_Closing;
	}

	private void ctnr_Closing(object sender, CancelEventArgs e)
	{
		base.Controls.Remove(ctnr);
	}

	private void lnkPostUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (lnkPostUrl.Text.ToLower().StartsWith("http"))
		{
			Help.ShowHelp(this, lnkPostUrl.Text);
		}
	}

	private void historyPage1_PostSelected(object sender, PostSelectedEventArgs e)
	{
		if (e.SelectedForEditing)
		{
			PrepareEditPost(e.SelectedPost);
			return;
		}
		lblHistoryDate.Text = "Date: " + e.SelectedPost.DateCreated;
		lblHistoryEntryID.Text = "Entry ID: " + e.SelectedPost.PostID;
		lnkPostUrl.Text = e.SelectedPost.PermaLink;
		SetHistoryPreviewTemplate(e.SelectedPost);
		historyMainView1.LoadHtml(e.SelectedPost.Body);
		CurrentPost = e.SelectedPost;
	}

	private void SetHistoryPreviewTemplate(BlogPost blogPost)
	{
		if (AppManager.CurrentBlog != null)
		{
			string previewTemplate = AppManager.CurrentBlog.Options.PreviewTemplate;
			previewTemplate = previewTemplate.Replace("[Post Title]", blogPost.Title);
			previewTemplate = previewTemplate.Replace("{", "{{").Replace("}", "}}");
			previewTemplate = previewTemplate.Replace("[Post Url]", "");
			previewTemplate = previewTemplate.Replace("[Post Body]", "{0}");
			historyMainView1.PreviewTemplate = previewTemplate;
		}
	}

	private void PrepareEditPost(BlogPost post)
	{
		frameMainView.GoHome();
		if (post != null)
		{
			CurrentPost = post;
			txtTitle.Text = post.Title;
			txtUrl.Text = post.Link;
			editorPage1.CurrentEditorMode = EditorMode.Design;
			editorPage1.DesignEditor.LoadHtml(post.Body);
			IsEditing = true;
		}
	}

	private void btnEditPost_Click(object sender, EventArgs e)
	{
		PrepareEditPost(historyPage1.CurrentPost);
	}

	private void btnDeletePost_Click(object sender, EventArgs e)
	{
		DialogResult dialogResult = MessageBox.Show(this, "This will permanantly delete this message. Are you sure?", "Delete Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
		if (dialogResult == DialogResult.Yes)
		{
			StartProgress("Deleting message '{0}'", CurrentPost.Title);
			AsyncCallback callback = DeletePostCallback;
			AppManager.CurrentProvider.BeginDeletePost(CurrentBlog, CurrentPost.PostID, callback, null);
		}
	}

	private void PostOrPublish()
	{
		if (_isDarkRoomEnabled)
		{
			editorPage1.SetHtml(dr.Html);
		}
		if (IsEditing)
		{
			EditPost();
		}
		else
		{
			AddPost();
		}
	}

	private void AddPost()
	{
		AsyncCallback callback = AddPostCallback;
		AppManager.CurrentProvider.BeginCreatePost(CurrentBlog, CurrentPost, callback, null);
	}

	private void AddPostCallback(IAsyncResult asr)
	{
		string postID;
		try
		{
			postID = AppManager.CurrentProvider.EndCreatePost(asr);
		}
		catch
		{
			postID = "-1";
		}
		FinishAddNewPost(postID);
	}

	private void FinishAddNewPost(string postID)
	{
		StopProgress();
		if (postID != null && postID != "-1")
		{
			string text = (CurrentPost.Publish ? ("'" + CurrentPost.Title + "' published") : "Draft posted");
			ResetMessage(text + " with new ID: " + postID, preserveCurrentPost: true);
			CurrentPost.PostID = postID;
			IsEditing = true;
		}
		else
		{
			MessageBox.Show("The post could not be added.", "Error adding post.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void EditPost()
	{
		AsyncCallback callback = EditPostCallback;
		AppManager.CurrentProvider.BeginUpdatePost(CurrentBlog, CurrentPost, callback, null);
	}

	private void EditPostCallback(IAsyncResult asr)
	{
		object successful;
		try
		{
			successful = AppManager.CurrentProvider.EndUpdatePost(asr);
		}
		catch
		{
			successful = false;
		}
		FinishEditPost(successful);
	}

	private void FinishEditPost(object successful)
	{
		StopProgress();
		if ((bool)successful)
		{
			ResetMessage("'" + CurrentPost.Title + "' successfully updated.", preserveCurrentPost: true);
		}
		else
		{
			MessageBox.Show("'" + CurrentPost.Title + "' could not be updated.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeletePostCallback(IAsyncResult asr)
	{
		object obj;
		try
		{
			obj = AppManager.CurrentProvider.EndDeletePost(asr);
		}
		catch
		{
			obj = false;
		}
		FinishDeletePost(obj);
	}

	private void FinishDeletePost(object obj)
	{
		StopProgress();
		if ((bool)obj)
		{
			ResetMessage("Message '" + CurrentPost.Title + "' successfully deleted.");
			if (base.InvokeRequired)
			{
				Invoke(new MethodInvoker(historyPage1.RefreshPosts));
			}
			else
			{
				historyPage1.RefreshPosts();
			}
		}
		else
		{
			MessageBox.Show("Message '" + CurrentPost.Title + "' could not be deleted.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void fcbxNumberOfPosts_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (AppManager.CurrentBlog != null)
		{
			AppManager.CurrentBlog.ItemCount = int.Parse(fcbxNumberOfPosts.Text);
			AppManager.Blogs.Remove(AppManager.CurrentBlog);
			AppManager.Blogs.Add(AppManager.CurrentBlog);
			AppManager.Save(AppManager.Blogs);
			if (historyPage1.Visible)
			{
				historyPage1.RefreshPosts();
			}
		}
	}

	private void mbiManageBlogs_Activate(object sender, EventArgs e)
	{
		ControlPanelNavigate(blogManagerPage1);
	}

	private void txtTitle_TextChanged(object sender, EventArgs e)
	{
		if (editorPage1.CurrentEditorMode == EditorMode.Design || editorPage1.CurrentEditorMode == EditorMode.Html)
		{
			SetTitleText();
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		Application.AddMessageFilter(this);
		if (LastActiveControl.Name == frameMainView.Name)
		{
			KeyboardNavigationMainView();
		}
		else if (LastActiveControl.Name == frameControlPanel.Name)
		{
			if (!splitContainer1.Panel2Collapsed)
			{
				splitContainer1.Panel2.Focus();
			}
		}
		else
		{
			LastActiveControl.Focus();
		}
		base.OnActivated(e);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		Application.RemoveMessageFilter(this);
		LastActiveControl = base.ActiveControl;
		base.OnDeactivate(e);
	}

	public bool PreFilterMessage(ref Message m)
	{
		bool result = false;
		try
		{
			if (m.Msg == 256 || m.Msg == 260)
			{
				Keys keys = (Keys)((int)m.WParam & 0xFFFF);
				if (keys == Keys.Tab)
				{
					if (Control.ModifierKeys == Keys.None)
					{
						if (txtTitle.ContainsFocus)
						{
							txtUrl.Focus();
							return true;
						}
						if (txtUrl.ContainsFocus)
						{
							KeyboardNavigationMainView();
							return true;
						}
						if (frameMainView.ContainsFocus)
						{
							if (!splitContainer1.Panel2Collapsed)
							{
								splitContainer1.Panel2.Focus();
							}
							else
							{
								txtTitle.Focus();
							}
							return true;
						}
						if (frameControlPanel.ContainsFocus)
						{
							txtTitle.Focus();
							return true;
						}
					}
					else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift && (Control.ModifierKeys & Keys.Control) == 0)
					{
						if (txtTitle.ContainsFocus)
						{
							if (!splitContainer1.Panel2Collapsed)
							{
								splitContainer1.Panel2.Focus();
							}
							else
							{
								KeyboardNavigationMainView();
							}
							return true;
						}
						if (frameMainView.ContainsFocus)
						{
							txtUrl.Focus();
							return true;
						}
						if (txtUrl.ContainsFocus)
						{
							txtTitle.Focus();
							return true;
						}
						if (frameControlPanel.ContainsFocus)
						{
							KeyboardNavigationMainView();
							return true;
						}
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	private void KeyboardNavigationMainView()
	{
		switch (editorPage1.CurrentEditorMode)
		{
		case EditorMode.Design:
			editorPage1.FocusDesignView();
			break;
		case EditorMode.Html:
			editorPage1.FocusHtmlView();
			break;
		case EditorMode.Preview:
			editorPage1.FocusPreviewView();
			break;
		case EditorMode.Xml:
			editorPage1.FocusXmlView();
			break;
		}
	}

	private void EditorForm_Resize(object sender, EventArgs e)
	{
		if (AppManager.Preferences.MinimizeToNotificationArea)
		{
			if (base.WindowState == FormWindowState.Normal || base.WindowState == FormWindowState.Maximized)
			{
				notifyIcon.Visible = false;
				base.ShowInTaskbar = true;
				frameMainView.GoHome();
			}
			else if (base.WindowState == FormWindowState.Minimized && !_ensureNotMinimized)
			{
				notifyIcon.Visible = true;
				base.ShowInTaskbar = false;
			}
			else
			{
				_ensureNotMinimized = false;
				base.WindowState = FormWindowState.Normal;
			}
		}
	}

	private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (AppManager.Preferences.MinimizeToNotificationArea)
		{
			_ensureNotMinimized = true;
			historyMainView1.PageEnter -= historyMainView1_PageEnter;
			historyMainView1.PageLeave -= historyMainView1_PageLeave;
			historyMainView1 = new HistoryMainView();
			historyMainView1.PageEnter += historyMainView1_PageEnter;
			historyMainView1.PageLeave += historyMainView1_PageLeave;
		}
		base.WindowState = FormWindowState.Normal;
		Refresh();
	}

	private void mbiTestHeaderStrip_Activate(object sender, EventArgs e)
	{
		using TestHeaderStripForm testHeaderStripForm = new TestHeaderStripForm();
		testHeaderStripForm.ShowDialog();
	}

	private void closeControlPanelToolStripButton_Click(object sender, EventArgs e)
	{
		CloseControlPanel();
	}

	private void CloseControlPanel()
	{
		SuspendLayout();
		_controlPanelWidth = base.Width - splitContainer1.SplitterDistance;
		splitContainer1.SplitterWidth = 1;
		splitContainer1.Panel2.Hide();
		splitContainer1.SplitterDistance = splitContainer1.Width + 3;
		ResumeLayout(performLayout: false);
	}

	private void fileToolStripMenuItem1_DropDownOpening(object sender, EventArgs e)
	{
		if (AppManager.CurrentProvider == null)
		{
			fileToolStripMenuItem1.DropDownItems.Insert(_placeHolderIndex, placeHolderToolStripMenuItem);
		}
		else if (fileToolStripMenuItem1.DropDownItems.Contains(placeHolderToolStripMenuItem))
		{
			fileToolStripMenuItem1.DropDownItems.Remove(placeHolderToolStripMenuItem);
			for (int i = 0; i < AppManager.CurrentProvider.MenuItems.Length; i++)
			{
				fileToolStripMenuItem1.DropDownItems.Insert(_placeHolderIndex + i, AppManager.CurrentProvider.MenuItems[i]);
			}
		}
	}

	private void undoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.Undo();
	}

	private void cutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.Cut();
	}

	private void copyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.Copy();
	}

	private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.Paste();
	}

	private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.SelectAll();
	}

	private void redoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.Redo();
	}

	private void findToolStripMenuItem_Click(object sender, EventArgs e)
	{
		editorPage1.EditorSurface.ShowFinder();
	}

	private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}

	private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}

	private void pasteSpecialToolStripMenuItem_Click(object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}

	private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
	{
		pasteToolStripMenuItem.Enabled = editorPage1.EditorSurface.CanPaste;
		undoToolStripMenuItem.Enabled = editorPage1.EditorSurface.CanUndo;
		redoToolStripMenuItem.Enabled = editorPage1.EditorSurface.CanRedo;
		copyToolStripMenuItem.Enabled = editorPage1.EditorSurface.CanCopy;
		selectAllToolStripMenuItem.Enabled = editorPage1.EditorSurface.CanSelectAll;
	}

	private void draftsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ControlPanelNavigate("autosave");
	}

	private void testDarkRoomConfigToolStripMenuItem_Click(object sender, EventArgs e)
	{
		SuspendLayout();
		base.Controls.Add(dr);
		dr.BringToFront();
		dr.Location = new Point(0, 0);
		dr.Size = base.Size;
		dr.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		base.FormBorderStyle = FormBorderStyle.None;
		base.Padding = new System.Windows.Forms.Padding(10, 10, 15, 35);
		BackColor = Color.Black;
		_preDarkRoomState = base.WindowState;
		base.WindowState = FormWindowState.Maximized;
		ResumeLayout(performLayout: true);
		dr.Title = CurrentPost.Title;
		dr.Html = editorPage1.Html;
		dr.Focus();
		dr.SetCaretPositionToEnd();
		_isDarkRoomEnabled = true;
	}

	private void testKeyDataToolStripMenuItem_Click(object sender, EventArgs e)
	{
		KeyDataForm keyDataForm = new KeyDataForm();
		keyDataForm.ShowDialog();
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostXING.Forms.EditorForm));
		this.imgButtonBar = new System.Windows.Forms.ImageList(this.components);
		this.pnlEditorContainer = new System.Windows.Forms.Panel();
		this.gradientPanel2 = new PostXING.Controls.GradientPanel();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtTitle = new PostXING.Controls.XPTextBox();
		this.txtUrl = new PostXING.Controls.XPTextBox();
		this.editorPage1 = new PostXING.NavigationPages.EditorPage();
		this.frameMainView = new PostXING.Controls.Navigation.StaticFrame();
		this.aboutPage1 = new PostXING.NavigationPages.AboutPage();
		this.historyMainView1 = new PostXING.NavigationPages.HistoryMainView();
		this.gradientPanel1 = new PostXING.Controls.GradientPanel();
		this.HistoryControlPanel = new PostXING.Controls.GradientPanel();
		this.label7 = new System.Windows.Forms.Label();
		this.btnEditPost = new System.Windows.Forms.PictureBox();
		this.btnDeletePost = new System.Windows.Forms.PictureBox();
		this.label4 = new System.Windows.Forms.Label();
		this.lnkPostUrl = new System.Windows.Forms.LinkLabel();
		this.btnHistoryRefresh = new System.Windows.Forms.PictureBox();
		this.lblHistoryDate = new System.Windows.Forms.Label();
		this.lblHistoryEntryID = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.fcbxNumberOfPosts = new Genghis.Windows.Forms.CompletionCombo();
		this.sfdSavePost = new System.Windows.Forms.SaveFileDialog();
		this.ofdOpenPost = new System.Windows.Forms.OpenFileDialog();
		this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.placeHolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
		this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pasteSpecialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
		this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.findNextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
		this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.viewBlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.categoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.draftsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.crossPostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.newBlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.manageBlogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.noPluginsLoadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.iBlogExtensionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.noIBlogExtensionsLoadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.testProgressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.testMessageBoxContainerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.testHeaderStripToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.testDarkRoomConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.uploadToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.newBlogToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.cbxiUsersBlogs = new System.Windows.Forms.ToolStripComboBox();
		this.viewBlogToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.optionsToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.historyPage1 = new PostXING.NavigationPages.HistoryPage();
		this.headerStrip1 = new PostXING.Controls.HeaderStrip();
		this.backToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.nextToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.headerStrip2 = new PostXING.Controls.HeaderStrip();
		this.controlPanelToolStripLabel = new System.Windows.Forms.ToolStripLabel();
		this.closeControlPanelToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.controlPanelHomePage1 = new PostXING.NavigationPages.ControlPanelHomePage();
		this.frameControlPanel = new PostXING.Controls.Navigation.StaticFrame();
		this.autoSavePage1 = new PostXING.NavigationPages.AutoSavePage();
		this.crossPostPage1 = new PostXING.NavigationPages.CrossPostPage();
		this.uploadFilePage1 = new PostXING.NavigationPages.UploadFilePage();
		this.blogManagerPage1 = new PostXING.NavigationPages.BlogManagerPage();
		this.categoriesPage1 = new PostXING.NavigationPages.CategoriesPage();
		this.AutoSaveTimer = new System.Windows.Forms.Timer(this.components);
		this.statusBarProgress1 = new PostXING.StatusBarProgress();
		this.testKeyDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.pnlEditorContainer.SuspendLayout();
		this.gradientPanel2.SuspendLayout();
		this.HistoryControlPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnEditPost).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnDeletePost).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnHistoryRefresh).BeginInit();
		this.menuStrip1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.toolStripContainer1.ContentPanel.SuspendLayout();
		this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
		this.toolStripContainer1.SuspendLayout();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.headerStrip1.SuspendLayout();
		this.headerStrip2.SuspendLayout();
		base.SuspendLayout();
		this.imgButtonBar.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgButtonBar.ImageStream");
		this.imgButtonBar.TransparentColor = System.Drawing.Color.Transparent;
		this.imgButtonBar.Images.SetKeyName(0, "");
		this.imgButtonBar.Images.SetKeyName(1, "");
		this.imgButtonBar.Images.SetKeyName(2, "");
		this.imgButtonBar.Images.SetKeyName(3, "");
		this.imgButtonBar.Images.SetKeyName(4, "");
		this.pnlEditorContainer.Controls.Add(this.gradientPanel2);
		this.pnlEditorContainer.Controls.Add(this.editorPage1);
		this.pnlEditorContainer.Controls.Add(this.frameMainView);
		this.pnlEditorContainer.Controls.Add(this.aboutPage1);
		this.pnlEditorContainer.Controls.Add(this.historyMainView1);
		this.pnlEditorContainer.Controls.Add(this.gradientPanel1);
		this.pnlEditorContainer.Controls.Add(this.HistoryControlPanel);
		this.pnlEditorContainer.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlEditorContainer.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.pnlEditorContainer.Location = new System.Drawing.Point(0, 0);
		this.pnlEditorContainer.Name = "pnlEditorContainer";
		this.pnlEditorContainer.Size = new System.Drawing.Size(599, 470);
		this.pnlEditorContainer.TabIndex = 10;
		this.gradientPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.gradientPanel2.BackColor = System.Drawing.SystemColors.Control;
		this.gradientPanel2.Controls.Add(this.label1);
		this.gradientPanel2.Controls.Add(this.label2);
		this.gradientPanel2.Controls.Add(this.txtTitle);
		this.gradientPanel2.Controls.Add(this.txtUrl);
		this.gradientPanel2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gradientPanel2.GradientColor = System.Drawing.SystemColors.ControlLightLight;
		this.gradientPanel2.Location = new System.Drawing.Point(0, 0);
		this.gradientPanel2.Name = "gradientPanel2";
		this.gradientPanel2.Rotation = 45f;
		this.gradientPanel2.Size = new System.Drawing.Size(599, 64);
		this.gradientPanel2.TabIndex = 0;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(16, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(32, 23);
		this.label1.TabIndex = 1;
		this.label1.Text = "Title:";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(0, 32);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 23);
		this.label2.TabIndex = 8;
		this.label2.Text = "Source:";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtTitle.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTitle.Location = new System.Drawing.Point(56, 8);
		this.txtTitle.Name = "txtTitle";
		this.txtTitle.Size = new System.Drawing.Size(535, 21);
		this.txtTitle.TabIndex = 0;
		this.txtTitle.Leave += new System.EventHandler(txtTitle_Leave);
		this.txtTitle.TextChanged += new System.EventHandler(txtTitle_TextChanged);
		this.txtUrl.AllowDrop = true;
		this.txtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtUrl.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUrl.Location = new System.Drawing.Point(56, 34);
		this.txtUrl.Name = "txtUrl";
		this.txtUrl.Size = new System.Drawing.Size(535, 21);
		this.txtUrl.TabIndex = 1;
		this.txtUrl.DragDrop += new System.Windows.Forms.DragEventHandler(txtUrl_DragDrop);
		this.txtUrl.DragEnter += new System.Windows.Forms.DragEventHandler(txtUrl_DragEnter);
		this.editorPage1.BackColor = System.Drawing.SystemColors.Window;
		this.editorPage1.CurrentEditorMode = PostXING.Controls.HtmlEditor.EditorMode.Design;
		this.editorPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.editorPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.editorPage1.Location = new System.Drawing.Point(0, 64);
		this.editorPage1.Name = "editorPage1";
		this.editorPage1.PreviewTemplate = null;
		this.editorPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.editorPage1.Size = new System.Drawing.Size(600, 404);
		this.editorPage1.TabIndex = 4;
		this.editorPage1.Text = "editorPage1";
		this.frameMainView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.frameMainView.Location = new System.Drawing.Point(0, 64);
		this.frameMainView.Name = "frameMainView";
		this.frameMainView.Size = new System.Drawing.Size(599, 406);
		this.frameMainView.TabIndex = 10;
		this.aboutPage1.BackColor = System.Drawing.SystemColors.Window;
		this.aboutPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.aboutPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.aboutPage1.Location = new System.Drawing.Point(0, 64);
		this.aboutPage1.Name = "aboutPage1";
		this.aboutPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.aboutPage1.Size = new System.Drawing.Size(600, 408);
		this.aboutPage1.TabIndex = 12;
		this.aboutPage1.Text = "aboutPage1";
		this.aboutPage1.Visible = false;
		this.historyMainView1.BackColor = System.Drawing.SystemColors.Window;
		this.historyMainView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.historyMainView1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.historyMainView1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.historyMainView1.Location = new System.Drawing.Point(0, 0);
		this.historyMainView1.Name = "historyMainView1";
		this.historyMainView1.PreviewTemplate = null;
		this.historyMainView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.historyMainView1.Size = new System.Drawing.Size(599, 470);
		this.historyMainView1.TabIndex = 13;
		this.historyMainView1.Text = "historyMainView1";
		this.historyMainView1.Visible = false;
		this.gradientPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.gradientPanel1.GradientColor = System.Drawing.SystemColors.ControlLightLight;
		this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
		this.gradientPanel1.Name = "gradientPanel1";
		this.gradientPanel1.Rotation = 45f;
		this.gradientPanel1.Size = new System.Drawing.Size(599, 64);
		this.gradientPanel1.TabIndex = 14;
		this.HistoryControlPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.HistoryControlPanel.Controls.Add(this.label7);
		this.HistoryControlPanel.Controls.Add(this.btnEditPost);
		this.HistoryControlPanel.Controls.Add(this.btnDeletePost);
		this.HistoryControlPanel.Controls.Add(this.label4);
		this.HistoryControlPanel.Controls.Add(this.lnkPostUrl);
		this.HistoryControlPanel.Controls.Add(this.btnHistoryRefresh);
		this.HistoryControlPanel.Controls.Add(this.lblHistoryDate);
		this.HistoryControlPanel.Controls.Add(this.lblHistoryEntryID);
		this.HistoryControlPanel.Controls.Add(this.label3);
		this.HistoryControlPanel.Controls.Add(this.label5);
		this.HistoryControlPanel.Controls.Add(this.label6);
		this.HistoryControlPanel.Controls.Add(this.fcbxNumberOfPosts);
		this.HistoryControlPanel.GradientColor = System.Drawing.SystemColors.ControlLightLight;
		this.HistoryControlPanel.Location = new System.Drawing.Point(0, 0);
		this.HistoryControlPanel.Name = "HistoryControlPanel";
		this.HistoryControlPanel.Rotation = 45f;
		this.HistoryControlPanel.Size = new System.Drawing.Size(599, 64);
		this.HistoryControlPanel.TabIndex = 15;
		this.label7.BackColor = System.Drawing.Color.Transparent;
		this.label7.Location = new System.Drawing.Point(56, 40);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(8, 23);
		this.label7.TabIndex = 25;
		this.label7.Text = "|";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEditPost.Image = (System.Drawing.Image)resources.GetObject("btnEditPost.Image");
		this.btnEditPost.Location = new System.Drawing.Point(32, 40);
		this.btnEditPost.Name = "btnEditPost";
		this.btnEditPost.Size = new System.Drawing.Size(16, 16);
		this.btnEditPost.TabIndex = 24;
		this.btnEditPost.TabStop = false;
		this.btnEditPost.Click += new System.EventHandler(btnEditPost_Click);
		this.btnDeletePost.Image = (System.Drawing.Image)resources.GetObject("btnDeletePost.Image");
		this.btnDeletePost.Location = new System.Drawing.Point(72, 40);
		this.btnDeletePost.Name = "btnDeletePost";
		this.btnDeletePost.Size = new System.Drawing.Size(16, 16);
		this.btnDeletePost.TabIndex = 23;
		this.btnDeletePost.TabStop = false;
		this.btnDeletePost.Click += new System.EventHandler(btnDeletePost_Click);
		this.label4.BackColor = System.Drawing.Color.Transparent;
		this.label4.Location = new System.Drawing.Point(96, 40);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(8, 23);
		this.label4.TabIndex = 22;
		this.label4.Text = "|";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkPostUrl.BackColor = System.Drawing.Color.Transparent;
		this.lnkPostUrl.Location = new System.Drawing.Point(112, 36);
		this.lnkPostUrl.Name = "lnkPostUrl";
		this.lnkPostUrl.Size = new System.Drawing.Size(472, 23);
		this.lnkPostUrl.TabIndex = 21;
		this.lnkPostUrl.TabStop = true;
		this.lnkPostUrl.Text = "LinkToPost";
		this.lnkPostUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lnkPostUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lnkPostUrl_LinkClicked);
		this.btnHistoryRefresh.Image = (System.Drawing.Image)resources.GetObject("btnHistoryRefresh.Image");
		this.btnHistoryRefresh.Location = new System.Drawing.Point(144, 10);
		this.btnHistoryRefresh.Name = "btnHistoryRefresh";
		this.btnHistoryRefresh.Size = new System.Drawing.Size(16, 16);
		this.btnHistoryRefresh.TabIndex = 20;
		this.btnHistoryRefresh.TabStop = false;
		this.btnHistoryRefresh.Click += new System.EventHandler(fcbxNumberOfPosts_SelectedIndexChanged);
		this.lblHistoryDate.BackColor = System.Drawing.Color.Transparent;
		this.lblHistoryDate.Location = new System.Drawing.Point(184, 8);
		this.lblHistoryDate.Name = "lblHistoryDate";
		this.lblHistoryDate.Size = new System.Drawing.Size(168, 23);
		this.lblHistoryDate.TabIndex = 16;
		this.lblHistoryDate.Text = "Date";
		this.lblHistoryDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblHistoryEntryID.BackColor = System.Drawing.Color.Transparent;
		this.lblHistoryEntryID.Location = new System.Drawing.Point(376, 8);
		this.lblHistoryEntryID.Name = "lblHistoryEntryID";
		this.lblHistoryEntryID.Size = new System.Drawing.Size(208, 23);
		this.lblHistoryEntryID.TabIndex = 17;
		this.lblHistoryEntryID.Text = "Entry ID";
		this.lblHistoryEntryID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label3.BackColor = System.Drawing.Color.Transparent;
		this.label3.Location = new System.Drawing.Point(8, 8);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 24);
		this.label3.TabIndex = 15;
		this.label3.Text = "Number of Posts:";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Location = new System.Drawing.Point(168, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(8, 23);
		this.label5.TabIndex = 18;
		this.label5.Text = "|";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label6.BackColor = System.Drawing.Color.Transparent;
		this.label6.Location = new System.Drawing.Point(360, 8);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(8, 23);
		this.label6.TabIndex = 19;
		this.label6.Text = "|";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.fcbxNumberOfPosts.Items.AddRange(new object[5] { "5", "10", "15", "20", "25" });
		this.fcbxNumberOfPosts.LimitToList = true;
		this.fcbxNumberOfPosts.Location = new System.Drawing.Point(80, 8);
		this.fcbxNumberOfPosts.Name = "fcbxNumberOfPosts";
		this.fcbxNumberOfPosts.Size = new System.Drawing.Size(56, 21);
		this.fcbxNumberOfPosts.TabIndex = 14;
		this.fcbxNumberOfPosts.SelectedIndexChanged += new System.EventHandler(fcbxNumberOfPosts_SelectedIndexChanged);
		this.sfdSavePost.Filter = "Html Files|*.htm|All Files|*.*";
		this.sfdSavePost.Title = "Save Post: ";
		this.ofdOpenPost.Filter = "Html Files|*.htm|All Files|*.*";
		this.notifyIcon.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon.Icon");
		this.notifyIcon.Text = "PostXING";
		this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseDoubleClick);
		this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.fileToolStripMenuItem1, this.editToolStripMenuItem, this.viewToolStripMenuItem, this.toolsToolStripMenuItem, this.helpToolStripMenuItem, this.testToolStripMenuItem });
		this.menuStrip1.Location = new System.Drawing.Point(0, 0);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(800, 24);
		this.menuStrip1.TabIndex = 11;
		this.menuStrip1.Text = "menuStrip1";
		this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.newToolStripMenuItem, this.openToolStripMenuItem, this.toolStripSeparator, this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.toolStripSeparator1, this.placeHolderToolStripMenuItem, this.toolStripSeparator2, this.exitToolStripMenuItem });
		this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
		this.fileToolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
		this.fileToolStripMenuItem1.Text = "&File";
		this.fileToolStripMenuItem1.DropDownOpening += new System.EventHandler(fileToolStripMenuItem1_DropDownOpening);
		this.newToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("newToolStripMenuItem.Image");
		this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.newToolStripMenuItem.Name = "newToolStripMenuItem";
		this.newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Control;
		this.newToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
		this.newToolStripMenuItem.Text = "&New";
		this.newToolStripMenuItem.Click += new System.EventHandler(file_New_Activate);
		this.openToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("openToolStripMenuItem.Image");
		this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.openToolStripMenuItem.Name = "openToolStripMenuItem";
		this.openToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control;
		this.openToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
		this.openToolStripMenuItem.Text = "&Open";
		this.openToolStripMenuItem.Click += new System.EventHandler(file_Open_Activate);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(180, 6);
		this.saveToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("saveToolStripMenuItem.Image");
		this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
		this.saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control;
		this.saveToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
		this.saveToolStripMenuItem.Text = "&Save";
		this.saveToolStripMenuItem.Click += new System.EventHandler(mbiSavePost_Activate);
		this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
		this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
		this.saveAsToolStripMenuItem.Text = "Save &As...";
		this.saveAsToolStripMenuItem.Click += new System.EventHandler(mbiSaveAs_Activate);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
		this.placeHolderToolStripMenuItem.Name = "placeHolderToolStripMenuItem";
		this.placeHolderToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
		this.placeHolderToolStripMenuItem.Text = "No Provider Loaded.";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
		this.exitToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("exitToolStripMenuItem.Image");
		this.exitToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
		this.exitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
		this.exitToolStripMenuItem.Text = "E&xit";
		this.exitToolStripMenuItem.Click += new System.EventHandler(file_Exit_Activate);
		this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.undoToolStripMenuItem, this.redoToolStripMenuItem, this.toolStripSeparator10, this.cutToolStripMenuItem, this.copyToolStripMenuItem, this.pasteToolStripMenuItem, this.pasteSpecialToolStripMenuItem, this.toolStripSeparator11, this.findToolStripMenuItem, this.findNextToolStripMenuItem,
			this.replaceToolStripMenuItem, this.toolStripSeparator12, this.selectAllToolStripMenuItem
		});
		this.editToolStripMenuItem.Name = "editToolStripMenuItem";
		this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
		this.editToolStripMenuItem.Text = "&Edit";
		this.editToolStripMenuItem.DropDownOpened += new System.EventHandler(editToolStripMenuItem_DropDownOpened);
		this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
		this.undoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control;
		this.undoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.undoToolStripMenuItem.Text = "Undo";
		this.undoToolStripMenuItem.Click += new System.EventHandler(undoToolStripMenuItem_Click);
		this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
		this.redoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Y | System.Windows.Forms.Keys.Control;
		this.redoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.redoToolStripMenuItem.Text = "Redo";
		this.redoToolStripMenuItem.Click += new System.EventHandler(redoToolStripMenuItem_Click);
		this.toolStripSeparator10.Name = "toolStripSeparator10";
		this.toolStripSeparator10.Size = new System.Drawing.Size(225, 6);
		this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
		this.cutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
		this.cutToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.cutToolStripMenuItem.Text = "Cut";
		this.cutToolStripMenuItem.Click += new System.EventHandler(cutToolStripMenuItem_Click);
		this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
		this.copyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
		this.copyToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.copyToolStripMenuItem.Text = "Copy";
		this.copyToolStripMenuItem.Click += new System.EventHandler(copyToolStripMenuItem_Click);
		this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
		this.pasteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Control;
		this.pasteToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.pasteToolStripMenuItem.Text = "Paste";
		this.pasteToolStripMenuItem.Click += new System.EventHandler(pasteToolStripMenuItem_Click);
		this.pasteSpecialToolStripMenuItem.Enabled = false;
		this.pasteSpecialToolStripMenuItem.Name = "pasteSpecialToolStripMenuItem";
		this.pasteSpecialToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
		this.pasteSpecialToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.pasteSpecialToolStripMenuItem.Text = "Paste Special...";
		this.pasteSpecialToolStripMenuItem.Click += new System.EventHandler(pasteSpecialToolStripMenuItem_Click);
		this.toolStripSeparator11.Name = "toolStripSeparator11";
		this.toolStripSeparator11.Size = new System.Drawing.Size(225, 6);
		this.findToolStripMenuItem.Name = "findToolStripMenuItem";
		this.findToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control;
		this.findToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.findToolStripMenuItem.Text = "Find...";
		this.findToolStripMenuItem.Click += new System.EventHandler(findToolStripMenuItem_Click);
		this.findNextToolStripMenuItem.Enabled = false;
		this.findNextToolStripMenuItem.Name = "findNextToolStripMenuItem";
		this.findNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
		this.findNextToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.findNextToolStripMenuItem.Text = "Find Next";
		this.findNextToolStripMenuItem.Click += new System.EventHandler(findNextToolStripMenuItem_Click);
		this.replaceToolStripMenuItem.Enabled = false;
		this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
		this.replaceToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.H | System.Windows.Forms.Keys.Control;
		this.replaceToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.replaceToolStripMenuItem.Text = "Replace...";
		this.replaceToolStripMenuItem.Click += new System.EventHandler(replaceToolStripMenuItem_Click);
		this.toolStripSeparator12.Name = "toolStripSeparator12";
		this.toolStripSeparator12.Size = new System.Drawing.Size(225, 6);
		this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
		this.selectAllToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control;
		this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
		this.selectAllToolStripMenuItem.Text = "Select All";
		this.selectAllToolStripMenuItem.Click += new System.EventHandler(selectAllToolStripMenuItem_Click);
		this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.viewBlogToolStripMenuItem, this.historyToolStripMenuItem, this.categoriesToolStripMenuItem, this.draftsToolStripMenuItem });
		this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
		this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
		this.viewToolStripMenuItem.Text = "&View";
		this.viewBlogToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("viewBlogToolStripMenuItem.Image");
		this.viewBlogToolStripMenuItem.Name = "viewBlogToolStripMenuItem";
		this.viewBlogToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
		this.viewBlogToolStripMenuItem.Text = "View Blog";
		this.viewBlogToolStripMenuItem.Click += new System.EventHandler(view_Blog_Activate);
		this.historyToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("historyToolStripMenuItem.Image");
		this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
		this.historyToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
		this.historyToolStripMenuItem.Text = "History";
		this.historyToolStripMenuItem.Click += new System.EventHandler(view_History_Activate);
		this.categoriesToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("categoriesToolStripMenuItem.Image");
		this.categoriesToolStripMenuItem.Name = "categoriesToolStripMenuItem";
		this.categoriesToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
		this.categoriesToolStripMenuItem.Text = "Ca&tegories";
		this.categoriesToolStripMenuItem.Click += new System.EventHandler(view_Categories_Activate);
		this.draftsToolStripMenuItem.Name = "draftsToolStripMenuItem";
		this.draftsToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
		this.draftsToolStripMenuItem.Text = "Drafts";
		this.draftsToolStripMenuItem.Click += new System.EventHandler(draftsToolStripMenuItem_Click);
		this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.crossPostToolStripMenuItem, this.toolStripSeparator6, this.uploadFileToolStripMenuItem, this.newBlogToolStripMenuItem, this.manageBlogsToolStripMenuItem, this.toolStripSeparator4, this.pluginsToolStripMenuItem, this.iBlogExtensionsToolStripMenuItem, this.toolStripSeparator3, this.optionsToolStripMenuItem });
		this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
		this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
		this.toolsToolStripMenuItem.Text = "&Tools";
		this.crossPostToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("crossPostToolStripMenuItem.Image");
		this.crossPostToolStripMenuItem.Name = "crossPostToolStripMenuItem";
		this.crossPostToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.crossPostToolStripMenuItem.Text = "Cross &Post";
		this.crossPostToolStripMenuItem.Click += new System.EventHandler(tools_CrossPost_Activate);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(158, 6);
		this.uploadFileToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("uploadFileToolStripMenuItem.Image");
		this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
		this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.uploadFileToolStripMenuItem.Text = "Upload File";
		this.uploadFileToolStripMenuItem.Click += new System.EventHandler(tools_UploadFile_Activate);
		this.newBlogToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("newBlogToolStripMenuItem.Image");
		this.newBlogToolStripMenuItem.Name = "newBlogToolStripMenuItem";
		this.newBlogToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.newBlogToolStripMenuItem.Text = "New Blog";
		this.newBlogToolStripMenuItem.Click += new System.EventHandler(tools_NewBlog_Activate);
		this.manageBlogsToolStripMenuItem.Name = "manageBlogsToolStripMenuItem";
		this.manageBlogsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.manageBlogsToolStripMenuItem.Text = "Manage Blogs";
		this.manageBlogsToolStripMenuItem.Click += new System.EventHandler(mbiManageBlogs_Activate);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(158, 6);
		this.pluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.noPluginsLoadedToolStripMenuItem });
		this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
		this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.pluginsToolStripMenuItem.Text = "Plugins";
		this.noPluginsLoadedToolStripMenuItem.Name = "noPluginsLoadedToolStripMenuItem";
		this.noPluginsLoadedToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
		this.noPluginsLoadedToolStripMenuItem.Text = "No Plugins Loaded";
		this.iBlogExtensionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.noIBlogExtensionsLoadedToolStripMenuItem });
		this.iBlogExtensionsToolStripMenuItem.Name = "iBlogExtensionsToolStripMenuItem";
		this.iBlogExtensionsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.iBlogExtensionsToolStripMenuItem.Text = "IBlogExtensions";
		this.noIBlogExtensionsLoadedToolStripMenuItem.Name = "noIBlogExtensionsLoadedToolStripMenuItem";
		this.noIBlogExtensionsLoadedToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
		this.noIBlogExtensionsLoadedToolStripMenuItem.Text = "No IBlogExtensions Loaded";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(158, 6);
		this.optionsToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("optionsToolStripMenuItem.Image");
		this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
		this.optionsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
		this.optionsToolStripMenuItem.Text = "&Options...";
		this.optionsToolStripMenuItem.Click += new System.EventHandler(tools_Options_Activate);
		this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.aboutToolStripMenuItem });
		this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
		this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
		this.helpToolStripMenuItem.Text = "&Help";
		this.aboutToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("aboutToolStripMenuItem.Image");
		this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
		this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
		this.aboutToolStripMenuItem.Text = "&About";
		this.aboutToolStripMenuItem.Click += new System.EventHandler(help_About_Activate);
		this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.testProgressToolStripMenuItem, this.testMessageBoxContainerToolStripMenuItem, this.testHeaderStripToolStripMenuItem, this.testDarkRoomConfigToolStripMenuItem, this.testKeyDataToolStripMenuItem });
		this.testToolStripMenuItem.Name = "testToolStripMenuItem";
		this.testToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
		this.testToolStripMenuItem.Text = "Test";
		this.testProgressToolStripMenuItem.Name = "testProgressToolStripMenuItem";
		this.testProgressToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
		this.testProgressToolStripMenuItem.Text = "Test Progress";
		this.testProgressToolStripMenuItem.Click += new System.EventHandler(mbiTestProgress_Activate);
		this.testMessageBoxContainerToolStripMenuItem.Name = "testMessageBoxContainerToolStripMenuItem";
		this.testMessageBoxContainerToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
		this.testMessageBoxContainerToolStripMenuItem.Text = "Test MessageBoxContainer";
		this.testMessageBoxContainerToolStripMenuItem.Click += new System.EventHandler(menuButtonItem4_Activate);
		this.testHeaderStripToolStripMenuItem.Name = "testHeaderStripToolStripMenuItem";
		this.testHeaderStripToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
		this.testHeaderStripToolStripMenuItem.Text = "Test HeaderStrip";
		this.testHeaderStripToolStripMenuItem.Click += new System.EventHandler(mbiTestHeaderStrip_Activate);
		this.testDarkRoomConfigToolStripMenuItem.Name = "testDarkRoomConfigToolStripMenuItem";
		this.testDarkRoomConfigToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
		this.testDarkRoomConfigToolStripMenuItem.Text = "Test DarkRoom config";
		this.testDarkRoomConfigToolStripMenuItem.Click += new System.EventHandler(testDarkRoomConfigToolStripMenuItem_Click);
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.newToolStripButton, this.openToolStripButton, this.saveToolStripButton, this.toolStripButton1, this.toolStripSeparator8, this.uploadToolStripButton, this.toolStripSeparator5, this.newBlogToolStripButton, this.cbxiUsersBlogs, this.viewBlogToolStripButton,
			this.toolStripSeparator7, this.optionsToolStripButton
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 24);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(800, 25);
		this.toolStrip1.Stretch = true;
		this.toolStrip1.TabIndex = 12;
		this.toolStrip1.Text = "toolStrip1";
		this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.newToolStripButton.Image = (System.Drawing.Image)resources.GetObject("newToolStripButton.Image");
		this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.newToolStripButton.Name = "newToolStripButton";
		this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.newToolStripButton.Text = "&New";
		this.newToolStripButton.Click += new System.EventHandler(file_New_Activate);
		this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.openToolStripButton.Image = (System.Drawing.Image)resources.GetObject("openToolStripButton.Image");
		this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.openToolStripButton.Name = "openToolStripButton";
		this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.openToolStripButton.Text = "&Open";
		this.openToolStripButton.Click += new System.EventHandler(file_Open_Activate);
		this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.saveToolStripButton.Image = (System.Drawing.Image)resources.GetObject("saveToolStripButton.Image");
		this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.saveToolStripButton.Name = "saveToolStripButton";
		this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.saveToolStripButton.Text = "&Save";
		this.saveToolStripButton.Click += new System.EventHandler(mbiSavePost_Activate);
		this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
		this.toolStripButton1.Text = "Post History";
		this.toolStripButton1.Click += new System.EventHandler(view_History_Activate);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
		this.uploadToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.uploadToolStripButton.Image = (System.Drawing.Image)resources.GetObject("uploadToolStripButton.Image");
		this.uploadToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.uploadToolStripButton.Name = "uploadToolStripButton";
		this.uploadToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.uploadToolStripButton.Text = "Upload a File";
		this.uploadToolStripButton.Click += new System.EventHandler(tools_UploadFile_Activate);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.newBlogToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.newBlogToolStripButton.Image = (System.Drawing.Image)resources.GetObject("newBlogToolStripButton.Image");
		this.newBlogToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.newBlogToolStripButton.Name = "newBlogToolStripButton";
		this.newBlogToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.newBlogToolStripButton.Text = "New Blog";
		this.newBlogToolStripButton.Click += new System.EventHandler(tools_NewBlog_Activate);
		this.cbxiUsersBlogs.Name = "cbxiUsersBlogs";
		this.cbxiUsersBlogs.Size = new System.Drawing.Size(125, 25);
		this.cbxiUsersBlogs.SelectedIndexChanged += new System.EventHandler(ComboBox_SelectedIndexChanged);
		this.viewBlogToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.viewBlogToolStripButton.Image = (System.Drawing.Image)resources.GetObject("viewBlogToolStripButton.Image");
		this.viewBlogToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.viewBlogToolStripButton.Name = "viewBlogToolStripButton";
		this.viewBlogToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.viewBlogToolStripButton.Text = "View Blog";
		this.viewBlogToolStripButton.Click += new System.EventHandler(view_Blog_Activate);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
		this.optionsToolStripButton.Image = (System.Drawing.Image)resources.GetObject("optionsToolStripButton.Image");
		this.optionsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.optionsToolStripButton.Name = "optionsToolStripButton";
		this.optionsToolStripButton.Size = new System.Drawing.Size(76, 22);
		this.optionsToolStripButton.Text = "Options...";
		this.optionsToolStripButton.Click += new System.EventHandler(tools_Options_Activate);
		this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
		this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 470);
		this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
		this.toolStripContainer1.Name = "toolStripContainer1";
		this.toolStripContainer1.Size = new System.Drawing.Size(800, 519);
		this.toolStripContainer1.TabIndex = 13;
		this.toolStripContainer1.Text = "toolStripContainer1";
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
		this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add(this.pnlEditorContainer);
		this.splitContainer1.Panel1.Controls.Add(this.historyPage1);
		this.splitContainer1.Panel2.Controls.Add(this.headerStrip1);
		this.splitContainer1.Panel2.Controls.Add(this.headerStrip2);
		this.splitContainer1.Panel2.Controls.Add(this.controlPanelHomePage1);
		this.splitContainer1.Panel2.Controls.Add(this.frameControlPanel);
		this.splitContainer1.Panel2.Controls.Add(this.autoSavePage1);
		this.splitContainer1.Panel2.Controls.Add(this.crossPostPage1);
		this.splitContainer1.Panel2.Controls.Add(this.uploadFilePage1);
		this.splitContainer1.Panel2.Controls.Add(this.blogManagerPage1);
		this.splitContainer1.Panel2.Controls.Add(this.categoriesPage1);
		this.splitContainer1.Panel2MinSize = 0;
		this.splitContainer1.Size = new System.Drawing.Size(800, 470);
		this.splitContainer1.SplitterDistance = 599;
		this.splitContainer1.TabIndex = 0;
		this.historyPage1.BackColor = System.Drawing.SystemColors.Window;
		this.historyPage1.CurrentPost = null;
		this.historyPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.historyPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.historyPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.historyPage1.Location = new System.Drawing.Point(0, 0);
		this.historyPage1.Name = "historyPage1";
		this.historyPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.historyPage1.Size = new System.Drawing.Size(599, 470);
		this.historyPage1.TabIndex = 2;
		this.historyPage1.Text = "Recent Posts";
		this.historyPage1.Visible = false;
		this.headerStrip1.AutoSize = false;
		this.headerStrip1.Font = new System.Drawing.Font("Tahoma", 10.25f);
		this.headerStrip1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
		this.headerStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.headerStrip1.HeaderStyle = PostXING.Controls.AreaHeaderStyle.ControlPanel;
		this.headerStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.backToolStripButton, this.nextToolStripButton, this.toolStripSeparator9, this.toolStripButton4 });
		this.headerStrip1.Location = new System.Drawing.Point(0, 25);
		this.headerStrip1.Name = "headerStrip1";
		this.headerStrip1.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
		this.headerStrip1.Size = new System.Drawing.Size(197, 23);
		this.headerStrip1.TabIndex = 1;
		this.headerStrip1.Text = "headerStrip1";
		this.backToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.backToolStripButton.Image = (System.Drawing.Image)resources.GetObject("backToolStripButton.Image");
		this.backToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.backToolStripButton.Name = "backToolStripButton";
		this.backToolStripButton.Size = new System.Drawing.Size(23, 20);
		this.backToolStripButton.Text = "Back";
		this.backToolStripButton.Click += new System.EventHandler(btniPrevious_Activate);
		this.nextToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.nextToolStripButton.Image = (System.Drawing.Image)resources.GetObject("nextToolStripButton.Image");
		this.nextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.nextToolStripButton.Name = "nextToolStripButton";
		this.nextToolStripButton.Size = new System.Drawing.Size(23, 20);
		this.nextToolStripButton.Text = "Next";
		this.nextToolStripButton.Click += new System.EventHandler(btniNext_Activate);
		this.toolStripSeparator9.Name = "toolStripSeparator9";
		this.toolStripSeparator9.Size = new System.Drawing.Size(6, 23);
		this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.toolStripButton4.Image = (System.Drawing.Image)resources.GetObject("toolStripButton4.Image");
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(23, 20);
		this.toolStripButton4.Text = "Home";
		this.toolStripButton4.Click += new System.EventHandler(btniHome_Activate);
		this.headerStrip2.AutoSize = false;
		this.headerStrip2.Font = new System.Drawing.Font("Tahoma", 8.25f);
		this.headerStrip2.ForeColor = System.Drawing.Color.Black;
		this.headerStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.headerStrip2.HeaderStyle = PostXING.Controls.AreaHeaderStyle.ControlPanelHeader;
		this.headerStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.controlPanelToolStripLabel, this.closeControlPanelToolStripButton });
		this.headerStrip2.Location = new System.Drawing.Point(0, 0);
		this.headerStrip2.Name = "headerStrip2";
		this.headerStrip2.Size = new System.Drawing.Size(197, 25);
		this.headerStrip2.TabIndex = 12;
		this.headerStrip2.Text = "headerStrip2";
		this.controlPanelToolStripLabel.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold);
		this.controlPanelToolStripLabel.Name = "controlPanelToolStripLabel";
		this.controlPanelToolStripLabel.Size = new System.Drawing.Size(82, 22);
		this.controlPanelToolStripLabel.Text = "Control Panel";
		this.closeControlPanelToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		this.closeControlPanelToolStripButton.AutoToolTip = false;
		this.closeControlPanelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.closeControlPanelToolStripButton.Font = new System.Drawing.Font("Verdana", 8.25f);
		this.closeControlPanelToolStripButton.Image = (System.Drawing.Image)resources.GetObject("closeControlPanelToolStripButton.Image");
		this.closeControlPanelToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
		this.closeControlPanelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.closeControlPanelToolStripButton.Name = "closeControlPanelToolStripButton";
		this.closeControlPanelToolStripButton.Size = new System.Drawing.Size(23, 22);
		this.closeControlPanelToolStripButton.Text = "X";
		this.closeControlPanelToolStripButton.Click += new System.EventHandler(closeControlPanelToolStripButton_Click);
		this.controlPanelHomePage1.BackColor = System.Drawing.SystemColors.Window;
		this.controlPanelHomePage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.controlPanelHomePage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.controlPanelHomePage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.controlPanelHomePage1.Location = new System.Drawing.Point(0, 0);
		this.controlPanelHomePage1.Name = "controlPanelHomePage1";
		this.controlPanelHomePage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.controlPanelHomePage1.Size = new System.Drawing.Size(197, 470);
		this.controlPanelHomePage1.TabIndex = 5;
		this.controlPanelHomePage1.Text = "Control Panel";
		this.frameControlPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.frameControlPanel.Location = new System.Drawing.Point(0, 46);
		this.frameControlPanel.Name = "frameControlPanel";
		this.frameControlPanel.Size = new System.Drawing.Size(197, 424);
		this.frameControlPanel.TabIndex = 0;
		this.autoSavePage1.BackColor = System.Drawing.SystemColors.Window;
		this.autoSavePage1.CurrentPost = null;
		this.autoSavePage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.autoSavePage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.autoSavePage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.autoSavePage1.Location = new System.Drawing.Point(0, 0);
		this.autoSavePage1.Name = "autoSavePage1";
		this.autoSavePage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.autoSavePage1.Size = new System.Drawing.Size(197, 470);
		this.autoSavePage1.TabIndex = 14;
		this.autoSavePage1.Text = "Drafts";
		this.crossPostPage1.BackColor = System.Drawing.SystemColors.Window;
		this.crossPostPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.crossPostPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.crossPostPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.crossPostPage1.Location = new System.Drawing.Point(0, 0);
		this.crossPostPage1.Name = "crossPostPage1";
		this.crossPostPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.crossPostPage1.Size = new System.Drawing.Size(197, 470);
		this.crossPostPage1.TabIndex = 4;
		this.crossPostPage1.Text = "Cross Post";
		this.crossPostPage1.Visible = false;
		this.uploadFilePage1.BackColor = System.Drawing.SystemColors.Window;
		this.uploadFilePage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.uploadFilePage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.uploadFilePage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.uploadFilePage1.Location = new System.Drawing.Point(0, 0);
		this.uploadFilePage1.Name = "uploadFilePage1";
		this.uploadFilePage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.uploadFilePage1.Size = new System.Drawing.Size(197, 470);
		this.uploadFilePage1.TabIndex = 1;
		this.uploadFilePage1.Text = "Upload a File";
		this.uploadFilePage1.Visible = false;
		this.blogManagerPage1.BackColor = System.Drawing.SystemColors.Window;
		this.blogManagerPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.blogManagerPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.blogManagerPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.blogManagerPage1.Location = new System.Drawing.Point(0, 0);
		this.blogManagerPage1.Name = "blogManagerPage1";
		this.blogManagerPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.blogManagerPage1.Size = new System.Drawing.Size(197, 470);
		this.blogManagerPage1.TabIndex = 11;
		this.blogManagerPage1.Text = "Manage Blog Options";
		this.categoriesPage1.BackColor = System.Drawing.SystemColors.Window;
		this.categoriesPage1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.categoriesPage1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.categoriesPage1.ForeColor = System.Drawing.SystemColors.WindowText;
		this.categoriesPage1.Location = new System.Drawing.Point(0, 0);
		this.categoriesPage1.Name = "categoriesPage1";
		this.categoriesPage1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.categoriesPage1.Size = new System.Drawing.Size(197, 470);
		this.categoriesPage1.TabIndex = 3;
		this.categoriesPage1.Text = "Categories";
		this.AutoSaveTimer.Interval = 12000;
		this.AutoSaveTimer.Tick += new System.EventHandler(AutoSaveTimer_Tick);
		this.statusBarProgress1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.statusBarProgress1.Location = new System.Drawing.Point(0, 519);
		this.statusBarProgress1.Name = "statusBarProgress1";
		this.statusBarProgress1.Size = new System.Drawing.Size(800, 21);
		this.statusBarProgress1.TabIndex = 7;
		this.testKeyDataToolStripMenuItem.Name = "testKeyDataToolStripMenuItem";
		this.testKeyDataToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
		this.testKeyDataToolStripMenuItem.Text = "Test KeyData";
		this.testKeyDataToolStripMenuItem.Click += new System.EventHandler(testKeyDataToolStripMenuItem_Click);
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
		base.ClientSize = new System.Drawing.Size(800, 540);
		base.Controls.Add(this.toolStripContainer1);
		base.Controls.Add(this.statusBarProgress1);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MainMenuStrip = this.menuStrip1;
		this.MinimumSize = new System.Drawing.Size(760, 568);
		base.Name = "EditorForm";
		this.Text = "PostXING";
		base.Resize += new System.EventHandler(EditorForm_Resize);
		this.pnlEditorContainer.ResumeLayout(false);
		this.gradientPanel2.ResumeLayout(false);
		this.gradientPanel2.PerformLayout();
		this.HistoryControlPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.btnEditPost).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnDeletePost).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnHistoryRefresh).EndInit();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.toolStripContainer1.ContentPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
		this.toolStripContainer1.TopToolStripPanel.PerformLayout();
		this.toolStripContainer1.ResumeLayout(false);
		this.toolStripContainer1.PerformLayout();
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		this.splitContainer1.ResumeLayout(false);
		this.headerStrip1.ResumeLayout(false);
		this.headerStrip1.PerformLayout();
		this.headerStrip2.ResumeLayout(false);
		this.headerStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
