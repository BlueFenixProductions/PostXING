using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace PostXING.Controls.Navigation;

public class StaticExplorer : Form
{
	private const int MAX_TOOLBAR_DROPDOWN_MENU_ITEMS = 9;

	private IContainer components;

	private ToolBarButton homeButton;

	private ToolBarButton forwardButton;

	private ToolBarButton backButton;

	private ToolBar navigationToolBar;

	private MenuItem fileExit;

	private MenuItem fileMenu;

	protected MainMenu menuBar;

	private EventHandler backButtonMenuItemHandler;

	private EventHandler forwardButtonMenuItemHandler;

	private MenuItem goMenu;

	private MenuItem goHome;

	private MenuItem goBack;

	private MenuItem goForward;

	private MenuItem goMenuSeparator;

	private ToolBarButton toolBarSeparator;

	private string applicationName;

	private StaticFrame frame;

	private ImageList toolBarImages1;

	private ImageList toolBarImages;

	public string ApplicationName
	{
		get
		{
			return applicationName;
		}
		set
		{
			applicationName = value;
			UpdateWindowTitle();
		}
	}

	public Page HomePage
	{
		get
		{
			return frame.HomePage;
		}
		set
		{
			frame.HomePage = value;
			bool enabled = frame.HomePage != null;
			homeButton.Enabled = enabled;
			goHome.Enabled = enabled;
		}
	}

	public StaticExplorer()
	{
		InitializeComponent();
		backButtonMenuItemHandler = BackButtonMenuItem_click;
		forwardButtonMenuItemHandler = ForwardButtonMenuItem_click;
	}

	public StaticExplorer(Page homePage)
		: this()
	{
		HomePage = homePage;
		frame.GoHome();
	}

	private void BackButtonMenuItem_click(object source, EventArgs e)
	{
		int index = ((MenuItem)source).Index;
		frame.GoBack(index + 1);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void fileExit_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ForwardButtonMenuItem_click(object source, EventArgs e)
	{
		int index = ((MenuItem)source).Index;
		frame.GoForward(index + 1);
	}

	private void frame_Navigate(object sender, NavigateEventArgs e)
	{
		UpdateWindowTitle();
		string[] backStackPageTitles = frame.GetBackStackPageTitles(9);
		UpdateToolBarButtonMenu(backButton, backStackPageTitles, backButtonMenuItemHandler);
		string[] forwardStackPageTitles = frame.GetForwardStackPageTitles(9);
		UpdateToolBarButtonMenu(forwardButton, forwardStackPageTitles, forwardButtonMenuItemHandler);
		goBack.Enabled = frame.CanGoBack;
		goForward.Enabled = frame.CanGoForward;
	}

	private void goBack_Click(object sender, EventArgs e)
	{
		frame.GoBack();
	}

	private void goForward_Click(object sender, EventArgs e)
	{
		frame.GoForward();
	}

	private void goHome_Click(object sender, EventArgs e)
	{
		frame.GoHome();
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(PostXING.Controls.Navigation.StaticExplorer));
		this.fileExit = new System.Windows.Forms.MenuItem();
		this.backButton = new System.Windows.Forms.ToolBarButton();
		this.fileMenu = new System.Windows.Forms.MenuItem();
		this.homeButton = new System.Windows.Forms.ToolBarButton();
		this.forwardButton = new System.Windows.Forms.ToolBarButton();
		this.toolBarImages1 = new System.Windows.Forms.ImageList(this.components);
		this.navigationToolBar = new System.Windows.Forms.ToolBar();
		this.toolBarSeparator = new System.Windows.Forms.ToolBarButton();
		this.menuBar = new System.Windows.Forms.MainMenu();
		this.goMenu = new System.Windows.Forms.MenuItem();
		this.goBack = new System.Windows.Forms.MenuItem();
		this.goForward = new System.Windows.Forms.MenuItem();
		this.goMenuSeparator = new System.Windows.Forms.MenuItem();
		this.goHome = new System.Windows.Forms.MenuItem();
		this.frame = new PostXING.Controls.Navigation.StaticFrame();
		this.toolBarImages = new System.Windows.Forms.ImageList(this.components);
		base.SuspendLayout();
		this.fileExit.Index = 0;
		this.fileExit.Shortcut = System.Windows.Forms.Shortcut.AltF4;
		this.fileExit.ShowShortcut = false;
		this.fileExit.Text = "E&xit";
		this.fileExit.Click += new System.EventHandler(fileExit_Click);
		this.backButton.Enabled = false;
		this.backButton.ImageIndex = 0;
		this.backButton.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
		this.fileMenu.Index = 0;
		this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[1] { this.fileExit });
		this.fileMenu.Text = "&File";
		this.homeButton.Enabled = false;
		this.homeButton.ImageIndex = 2;
		this.forwardButton.Enabled = false;
		this.forwardButton.ImageIndex = 1;
		this.forwardButton.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
		this.toolBarImages1.ImageSize = new System.Drawing.Size(16, 16);
		this.toolBarImages1.TransparentColor = System.Drawing.Color.Transparent;
		this.navigationToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
		this.navigationToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[4] { this.backButton, this.forwardButton, this.toolBarSeparator, this.homeButton });
		this.navigationToolBar.DropDownArrows = true;
		this.navigationToolBar.ImageList = this.toolBarImages;
		this.navigationToolBar.Location = new System.Drawing.Point(0, 0);
		this.navigationToolBar.Name = "navigationToolBar";
		this.navigationToolBar.ShowToolTips = true;
		this.navigationToolBar.Size = new System.Drawing.Size(256, 28);
		this.navigationToolBar.TabIndex = 0;
		this.navigationToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
		this.navigationToolBar.Wrappable = false;
		this.navigationToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(navigationToolBar_ButtonClick);
		this.toolBarSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
		this.menuBar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[2] { this.fileMenu, this.goMenu });
		this.goMenu.Index = 1;
		this.goMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[4] { this.goBack, this.goForward, this.goMenuSeparator, this.goHome });
		this.goMenu.Text = "&Go";
		this.goBack.Enabled = false;
		this.goBack.Index = 0;
		this.goBack.Text = "&Back";
		this.goBack.Click += new System.EventHandler(goBack_Click);
		this.goForward.Enabled = false;
		this.goForward.Index = 1;
		this.goForward.Text = "&Forward";
		this.goForward.Click += new System.EventHandler(goForward_Click);
		this.goMenuSeparator.Index = 2;
		this.goMenuSeparator.Text = "-";
		this.goHome.Enabled = false;
		this.goHome.Index = 3;
		this.goHome.Text = "&Home";
		this.goHome.Click += new System.EventHandler(goHome_Click);
		this.frame.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.frame.Location = new System.Drawing.Point(0, 32);
		this.frame.Name = "frame";
		this.frame.Size = new System.Drawing.Size(256, 80);
		this.frame.TabIndex = 1;
		this.toolBarImages.ImageSize = new System.Drawing.Size(16, 16);
		this.toolBarImages.ImageStream = (System.Windows.Forms.ImageListStreamer)resourceManager.GetObject("toolBarImages.ImageStream");
		this.toolBarImages.TransparentColor = System.Drawing.Color.Transparent;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		base.ClientSize = new System.Drawing.Size(256, 113);
		base.Controls.Add(this.frame);
		base.Controls.Add(this.navigationToolBar);
		this.ForeColor = System.Drawing.SystemColors.WindowText;
		base.KeyPreview = true;
		base.Menu = this.menuBar;
		base.Name = "StaticExplorer";
		base.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
		base.ResumeLayout(false);
	}

	protected void navigationToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
	{
		if (e.Button == backButton)
		{
			frame.GoBack();
		}
		else if (e.Button == forwardButton)
		{
			frame.GoForward();
		}
		else if (e.Button == homeButton)
		{
			frame.GoHome();
		}
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		e.Cancel = !frame.CanLeavePage;
		base.OnClosing(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Alt)
		{
			switch (e.KeyCode)
			{
			case Keys.Home:
				frame.GoHome();
				e.Handled = true;
				break;
			case Keys.Left:
				frame.GoBack();
				e.Handled = true;
				break;
			case Keys.Right:
				frame.GoForward();
				e.Handled = true;
				break;
			}
		}
		else
		{
			switch (e.KeyCode)
			{
			case Keys.End:
				if (e.Control)
				{
					frame.PageEnd();
					e.Handled = true;
				}
				break;
			case Keys.Home:
				if (e.Control)
				{
					frame.PageHome();
					e.Handled = true;
				}
				break;
			case Keys.Next:
				frame.PageDown();
				e.Handled = true;
				break;
			case Keys.Prior:
				frame.PageUp();
				e.Handled = true;
				break;
			}
		}
		base.OnKeyDown(e);
	}

	private void UpdateToolBarButtonMenu(ToolBarButton button, string[] pageTitles, EventHandler clickEventHandler)
	{
		if (pageTitles == null)
		{
			button.Enabled = false;
			return;
		}
		button.Enabled = true;
		if (button.DropDownMenu == null)
		{
			button.DropDownMenu = new ContextMenu();
		}
		else
		{
			button.DropDownMenu.MenuItems.Clear();
		}
		foreach (string caption in pageTitles)
		{
			button.DropDownMenu.MenuItems.Add(caption, clickEventHandler);
		}
	}

	private void UpdateWindowTitle()
	{
		bool flag = applicationName != null && applicationName.Length > 0;
		bool flag2 = frame.CurrentPage != null && frame.CurrentPage.Text.Length > 0;
		string text = ((flag && flag2) ? (frame.CurrentPage.Text + " - " + applicationName) : ((flag && !flag2) ? applicationName : ((flag || !flag2) ? string.Empty : frame.CurrentPage.Text)));
		Text = text;
	}

	protected override void WndProc(ref Message m)
	{
		bool flag = false;
		if (m.Msg == 793)
		{
			uint num = (uint)m.LParam.ToInt32();
			switch ((short)((num >> 16) & -61441))
			{
			case 1:
				frame.GoBack();
				flag = true;
				break;
			case 2:
				frame.GoForward();
				flag = true;
				break;
			case 7:
				frame.GoHome();
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			base.WndProc(ref m);
		}
	}
}
