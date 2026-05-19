using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls.Navigation;

[ToolboxBitmap(typeof(Page))]
public class Page : UserControl
{
	private Container components;

	private IFrame frame;

	private ScrollBars scrollBars = ScrollBars.Vertical;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public IFrame Frame
	{
		get
		{
			return frame;
		}
		set
		{
			frame = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether the page wants its frame to give the page scroll bars.")]
	public ScrollBars ScrollBars
	{
		get
		{
			return scrollBars;
		}
		set
		{
			scrollBars = value;
			OnScrollBarsChanged();
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[Category("Behavior")]
	[Description("Occurs when the frame is navigating into the page.")]
	public event PageEventHandler PageEnter;

	[Category("Behavior")]
	[Description("Occurs when the frame is navigating away from the page.")]
	public event PageEventHandler PageLeave;

	[Category("Property Changed")]
	[Description("Event fired when the value of ScrollBars property is changed on Control")]
	public event EventHandler ScrollBarsChanged;

	public Page()
	{
		InitializeComponent();
	}

	public virtual bool CanEnter(ViewState viewState)
	{
		return true;
	}

	public virtual bool CanLeave()
	{
		return true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	public virtual void Go(Page page)
	{
		Frame.Go(page);
	}

	public virtual void Go(Type pageType)
	{
		Frame.Go(pageType);
	}

	public void GoBack()
	{
		Frame.GoBack();
	}

	public void GoBack(int n)
	{
		Frame.GoBack(n);
	}

	public void GoForward()
	{
		Frame.GoForward();
	}

	public void GoForward(int n)
	{
		Frame.GoForward(n);
	}

	public void GoHome()
	{
		Frame.GoHome();
	}

	private void InitializeComponent()
	{
		this.BackColor = System.Drawing.SystemColors.Window;
		this.ForeColor = System.Drawing.SystemColors.WindowText;
		base.Name = "Page";
		base.Size = new System.Drawing.Size(300, 300);
	}

	protected virtual void OnScrollBarsChanged()
	{
		if (this.ScrollBarsChanged != null)
		{
			this.ScrollBarsChanged(this, EventArgs.Empty);
		}
	}

	public virtual void OnPageEnter(PageEventArgs e)
	{
		if (e.ViewState != null)
		{
			Text = (string)e.ViewState.ReadProperty("Text");
		}
		if (this.PageEnter != null)
		{
			this.PageEnter(this, e);
		}
	}

	public virtual void OnPageLeave(PageEventArgs e)
	{
		e.ViewState.WriteProperty("Text", Text);
		if (this.PageLeave != null)
		{
			this.PageLeave(this, e);
		}
	}

	public override string ToString()
	{
		return Text;
	}
}
