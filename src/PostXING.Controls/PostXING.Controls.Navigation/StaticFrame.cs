using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls.Navigation;

[DefaultEvent("Navigate")]
[ToolboxBitmap(typeof(StaticFrame))]
public class StaticFrame : UserControl, IFrame
{
	private Container components;

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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CanGoBack => backStack.Count > 0;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CanGoForward => forwardStack.Count > 0;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[Category("Behavior")]
	[Description("Occurs when the frame has navigated to a new page.")]
	public event NavigateEventHandler Navigate;

	public StaticFrame()
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string[] GetBackStackPageTitles(int maximumPageTitleCount)
	{
		return backStack.GetPageTitles(maximumPageTitleCount);
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		this.pageSite = new System.Windows.Forms.Panel();
		base.SuspendLayout();
		this.pageSite.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.pageSite.BackColor = System.Drawing.SystemColors.Control;
		this.pageSite.Location = new System.Drawing.Point(0, 0);
		this.pageSite.Name = "pageSite";
		this.pageSite.Size = new System.Drawing.Size(224, 208);
		this.pageSite.TabIndex = 6;
		base.Controls.Add(this.pageSite);
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
	}

	public void PageEnd()
	{
	}

	public void PageHome()
	{
	}

	public void PageUp()
	{
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
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
		case ScrollBars.Horizontal:
			height = base.ClientSize.Height;
			break;
		case ScrollBars.Vertical:
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
		case ScrollBars.Horizontal:
			page.Top = 0;
			break;
		case ScrollBars.None:
			pageSite.Size = base.ClientSize;
			if (page != null)
			{
				page.Location = new Point(0, 0);
			}
			break;
		case ScrollBars.Vertical:
			page.Left = 0;
			break;
		case ScrollBars.Both:
			break;
		}
	}

	private void SyncPageToHorizontalScrollBar()
	{
	}

	private void SyncPageToVerticalScrollBar()
	{
	}

	private void verticalScrollBar_ValueChanged(object sender, EventArgs e)
	{
		SyncPageToVerticalScrollBar();
	}
}
