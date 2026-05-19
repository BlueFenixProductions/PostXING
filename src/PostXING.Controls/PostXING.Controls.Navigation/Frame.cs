using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls.Navigation;

[DefaultEvent("Navigate")]
[ToolboxBitmap(typeof(Frame))]
public class Frame : UserControl, IFrame
{
	private Container components;

	private HScrollBar horizontalScrollBar;

	private VScrollBar verticalScrollBar;

	private Panel pageSite;

	private Page homePage;

	private NavigationContext currentContext;

	private NavigationStack backStack;

	private NavigationStack forwardStack;

	private ReferenceCountTable pageReferenceCount;

	private TypeInstanceCache pageTypeInstanceCache;

	private EventHandler pageScrollBarsChangedHandler;

	private NavigationContext CurrentContext
	{
		get
		{
			return currentContext;
		}
		set
		{
			if (value == null)
			{
				throw new NullReferenceException();
			}
			NavigationContext navigationContext = currentContext;
			Page page = null;
			if (navigationContext != null)
			{
				navigationContext.HorizontalScrollPosition = HorizontalScrollPosition;
				navigationContext.VerticalScrollPosition = VerticalScrollPosition;
				page = navigationContext.Page;
				if (page != null)
				{
					navigationContext.ViewState = new ViewState();
					PageEventArgs e = new PageEventArgs(navigationContext.ViewState);
					page.OnPageLeave(e);
					page.ScrollBarsChanged -= pageScrollBarsChangedHandler;
				}
			}
			currentContext = value;
			Page currentPage = CurrentPage;
			SetupScrollBarsForPage(currentPage);
			if (currentPage != null)
			{
				ResizePage(currentPage);
				HorizontalScrollPosition = currentContext.HorizontalScrollPosition;
				VerticalScrollPosition = currentContext.VerticalScrollPosition;
				PageEventArgs e2 = new PageEventArgs(currentContext.ViewState);
				currentPage.OnPageEnter(e2);
				pageSite.BackColor = currentPage.BackColor;
				currentPage.Show();
				pageSite.Show();
				currentPage.ScrollBarsChanged += pageScrollBarsChangedHandler;
			}
			else
			{
				pageSite.Hide();
			}
			if (page != null && page != currentPage)
			{
				page.Hide();
			}
			OnNavigate(new NavigateEventArgs(currentPage));
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CanGoBack => backStack.Count > 0;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CanGoForward => forwardStack.Count > 0;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool CanLeavePage
	{
		get
		{
			if (CurrentPage != null)
			{
				return CurrentPage.CanLeave();
			}
			return true;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Page CurrentPage
	{
		get
		{
			if (currentContext == null)
			{
				return null;
			}
			return currentContext.Page;
		}
		set
		{
			Go(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Page HomePage
	{
		get
		{
			return homePage;
		}
		set
		{
			homePage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int HorizontalScrollPosition
	{
		get
		{
			return horizontalScrollBar.Value;
		}
		set
		{
			SetScrollBarValue(horizontalScrollBar, value);
			SyncPageToHorizontalScrollBar();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int VerticalScrollPosition
	{
		get
		{
			return verticalScrollBar.Value;
		}
		set
		{
			SetScrollBarValue(verticalScrollBar, value);
			SyncPageToVerticalScrollBar();
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the frame has navigated to a new page.")]
	public event NavigateEventHandler Navigate;

	public Frame()
	{
		InitializeComponent();
		if (!base.DesignMode)
		{
			backStack = new NavigationStack();
			forwardStack = new NavigationStack();
			pageReferenceCount = new ReferenceCountTable();
			pageTypeInstanceCache = new TypeInstanceCache();
			pageScrollBarsChangedHandler = Page_ScrollBarsChanged;
		}
	}

	private void AddPageToFrame(Page page)
	{
		pageReferenceCount.AddReference(page);
		pageSite.Controls.Add(page);
		page.Location = new Point(0, 0);
		page.Frame = this;
	}

	private void AdjustScrollBar(ScrollBar scrollBar, int visiblePageLength, int actualPageLength)
	{
		if (visiblePageLength >= actualPageLength)
		{
			scrollBar.Value = 0;
			scrollBar.Enabled = false;
		}
		else if (visiblePageLength > 0)
		{
			scrollBar.Maximum = actualPageLength;
			scrollBar.LargeChange = (int)((double)visiblePageLength / (double)actualPageLength * (double)scrollBar.Maximum);
			scrollBar.SmallChange = scrollBar.LargeChange / 8;
			scrollBar.Enabled = true;
			if (scrollBar.Maximum - scrollBar.LargeChange < scrollBar.Value)
			{
				scrollBar.Value = scrollBar.Maximum - scrollBar.LargeChange;
			}
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string[] GetBackStackPageTitles(int maximumPageTitleCount)
	{
		return backStack.GetPageTitles(maximumPageTitleCount);
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string[] GetForwardStackPageTitles(int maximumPageTitleCount)
	{
		return forwardStack.GetPageTitles(maximumPageTitleCount);
	}

	public virtual void Go(Page page)
	{
		if (!CanLeavePage || (page != null && !page.CanEnter(null)))
		{
			return;
		}
		if (CurrentContext != null)
		{
			backStack.Push(CurrentContext);
		}
		while (forwardStack.Count > 0)
		{
			NavigationContext navigationContext = (NavigationContext)forwardStack.Pop();
			Page page2 = navigationContext.Page;
			if (page2 != null)
			{
				RemovePageFromFrame(page2);
			}
		}
		if (page != null)
		{
			AddPageToFrame(page);
		}
		CurrentContext = new NavigationContext(page);
	}

	public void Go(Type pageType)
	{
		Page page = (Page)pageTypeInstanceCache.GetInstanceOfType(pageType);
		Go(page);
	}

	public void GoBack()
	{
		MoveThroughHistory(-1);
	}

	public void GoBack(int n)
	{
		MoveThroughHistory(-n);
	}

	public void GoForward()
	{
		MoveThroughHistory(1);
	}

	public void GoForward(int n)
	{
		MoveThroughHistory(n);
	}

	public void GoHome()
	{
		if (HomePage != null && CurrentPage != HomePage)
		{
			Go(HomePage);
		}
	}

	private void horizontalScrollBar_ValueChanged(object sender, EventArgs e)
	{
		SyncPageToHorizontalScrollBar();
	}

	private void InitializeComponent()
	{
		this.horizontalScrollBar = new System.Windows.Forms.HScrollBar();
		this.verticalScrollBar = new System.Windows.Forms.VScrollBar();
		this.pageSite = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.horizontalScrollBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.horizontalScrollBar.Enabled = false;
		this.horizontalScrollBar.Location = new System.Drawing.Point(0, 191);
		this.horizontalScrollBar.Name = "horizontalScrollBar";
		this.horizontalScrollBar.Size = new System.Drawing.Size(207, 17);
		this.horizontalScrollBar.TabIndex = 5;
		this.horizontalScrollBar.Visible = false;
		this.horizontalScrollBar.ValueChanged += new System.EventHandler(horizontalScrollBar_ValueChanged);
		this.verticalScrollBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.verticalScrollBar.Enabled = false;
		this.verticalScrollBar.Font = new System.Drawing.Font("Tahoma", 8f);
		this.verticalScrollBar.ForeColor = System.Drawing.SystemColors.WindowText;
		this.verticalScrollBar.Location = new System.Drawing.Point(207, 0);
		this.verticalScrollBar.Name = "verticalScrollBar";
		this.verticalScrollBar.Size = new System.Drawing.Size(17, 191);
		this.verticalScrollBar.TabIndex = 4;
		this.verticalScrollBar.Visible = false;
		this.verticalScrollBar.ValueChanged += new System.EventHandler(verticalScrollBar_ValueChanged);
		this.pageSite.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pageSite.BackColor = System.Drawing.SystemColors.Control;
		this.pageSite.Location = new System.Drawing.Point(0, 0);
		this.pageSite.Name = "pageSite";
		this.pageSite.Size = new System.Drawing.Size(208, 192);
		this.pageSite.TabIndex = 6;
		base.Controls.Add(this.pageSite);
		base.Controls.Add(this.horizontalScrollBar);
		base.Controls.Add(this.verticalScrollBar);
		base.Name = "Frame";
		base.Size = new System.Drawing.Size(224, 208);
		base.ResumeLayout(false);
	}

	private void MoveThroughHistory(int n)
	{
		if (!CanLeavePage)
		{
			return;
		}
		NavigationStack navigationStack;
		NavigationStack navigationStack2;
		int num;
		if (n < 0)
		{
			navigationStack = backStack;
			navigationStack2 = forwardStack;
			num = -n;
		}
		else
		{
			if (n <= 0)
			{
				return;
			}
			navigationStack = forwardStack;
			navigationStack2 = backStack;
			num = n;
		}
		NavigationContext navigationContext = CurrentContext;
		bool flag = false;
		int num2 = 0;
		while (!flag && navigationStack.Count > 0)
		{
			NavigationContext navigationContext2 = (NavigationContext)navigationStack.Pop();
			num2++;
			Page page = navigationContext2.Page;
			if (page == null)
			{
				continue;
			}
			ViewState viewState = navigationContext2.ViewState;
			if (page.CanEnter(viewState))
			{
				if (navigationContext != null)
				{
					navigationStack2.Push(navigationContext);
				}
				navigationContext = navigationContext2;
				if (num2 >= num)
				{
					flag = true;
				}
			}
			else
			{
				RemovePageFromFrame(page);
			}
		}
		if (navigationContext != CurrentContext)
		{
			CurrentContext = navigationContext;
		}
	}

	private void Page_ScrollBarsChanged(object sender, EventArgs e)
	{
		SetupScrollBarsForPage(CurrentPage);
		ResizePage(CurrentPage);
	}

	public void PageDown()
	{
		VerticalScrollPosition += verticalScrollBar.LargeChange;
	}

	public void PageEnd()
	{
		VerticalScrollPosition = verticalScrollBar.Maximum - verticalScrollBar.LargeChange;
	}

	public void PageHome()
	{
		VerticalScrollPosition = 0;
	}

	public void PageUp()
	{
		VerticalScrollPosition -= verticalScrollBar.LargeChange;
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (verticalScrollBar.Enabled)
		{
			int num = e.Delta / 120;
			int height = CurrentPage.Font.Height;
			int mouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;
			int num2 = mouseWheelScrollLines * height * num;
			VerticalScrollPosition -= num2;
		}
		base.OnMouseWheel(e);
	}

	protected virtual void OnNavigate(NavigateEventArgs e)
	{
		if (this.Navigate != null)
		{
			this.Navigate(this, e);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		if (CurrentPage != null)
		{
			ResizePage(CurrentPage);
		}
	}

	private void RemovePageFromFrame(Page page)
	{
		if (pageReferenceCount.RemoveReference(page) == 0)
		{
			pageSite.Controls.Remove(page);
			page.Frame = null;
		}
	}

	private void ResizePage(Page page)
	{
		int width = page.Width;
		int height = page.Height;
		switch (page.ScrollBars)
		{
		case ScrollBars.Both:
			AdjustScrollBar(horizontalScrollBar, base.ClientSize.Width - verticalScrollBar.Width, page.Width);
			AdjustScrollBar(verticalScrollBar, base.ClientSize.Height - horizontalScrollBar.Height, page.Height);
			break;
		case ScrollBars.Horizontal:
			AdjustScrollBar(horizontalScrollBar, base.ClientSize.Width, page.Width);
			height = base.ClientSize.Height;
			break;
		case ScrollBars.Vertical:
			AdjustScrollBar(verticalScrollBar, base.ClientSize.Height, page.Height);
			width = base.ClientSize.Width;
			break;
		case ScrollBars.None:
			height = base.ClientSize.Height;
			width = base.ClientSize.Width;
			break;
		}
		Size size = new Size(width, height);
		if (page.Size != size)
		{
			page.Size = size;
		}
	}

	private void SetScrollBarValue(ScrollBar scrollBar, int scrollValue)
	{
		scrollValue = Math.Max(scrollValue, 0);
		scrollValue = Math.Min(scrollValue, scrollBar.Maximum - scrollBar.LargeChange);
		scrollBar.Value = scrollValue;
	}

	private void SetupScrollBarsForPage(Page page)
	{
		switch (page?.ScrollBars ?? ScrollBars.None)
		{
		case ScrollBars.Both:
			horizontalScrollBar.Width = base.ClientSize.Width - verticalScrollBar.Width;
			verticalScrollBar.Height = base.ClientSize.Height - horizontalScrollBar.Height;
			horizontalScrollBar.Show();
			verticalScrollBar.Show();
			pageSite.Size = new Size(verticalScrollBar.Left, horizontalScrollBar.Top);
			break;
		case ScrollBars.Horizontal:
			horizontalScrollBar.Width = base.ClientSize.Width;
			horizontalScrollBar.Show();
			verticalScrollBar.Hide();
			pageSite.Size = new Size(base.ClientSize.Width, horizontalScrollBar.Top);
			page.Top = 0;
			break;
		case ScrollBars.None:
			horizontalScrollBar.Hide();
			verticalScrollBar.Hide();
			pageSite.Size = base.ClientSize;
			if (page != null)
			{
				page.Location = new Point(0, 0);
			}
			break;
		case ScrollBars.Vertical:
			verticalScrollBar.Height = base.ClientSize.Height;
			horizontalScrollBar.Hide();
			verticalScrollBar.Show();
			pageSite.Size = new Size(verticalScrollBar.Left, base.ClientSize.Height);
			page.Left = 0;
			break;
		}
		if (!horizontalScrollBar.Visible)
		{
			horizontalScrollBar.Enabled = false;
		}
		if (!verticalScrollBar.Visible)
		{
			verticalScrollBar.Enabled = false;
		}
	}

	private void SyncPageToHorizontalScrollBar()
	{
		int num = -horizontalScrollBar.Value;
		if (CurrentPage.Left != num)
		{
			CurrentPage.Left = num;
		}
	}

	private void SyncPageToVerticalScrollBar()
	{
		int num = -verticalScrollBar.Value;
		if (CurrentPage.Top != num)
		{
			CurrentPage.Top = num;
		}
	}

	private void verticalScrollBar_ValueChanged(object sender, EventArgs e)
	{
		SyncPageToVerticalScrollBar();
	}
}
