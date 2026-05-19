using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Extensibility;

public class GenericDialogPlugin : Form, IPlugin
{
	private Container components;

	protected AppContext m_appContext;

	protected string m_Name = "simple plugin";

	protected string m_menuCaption = "&My simple plugin";

	protected Shortcut m_Shortcut;

	protected bool m_BeginGroup;

	protected bool m_HasConfiguration;

	private PluginDockPositions m_pos = PluginDockPositions.Floating;

	public virtual bool HasConfiguration => m_HasConfiguration;

	public virtual Shortcut Shortcut
	{
		get
		{
			return m_Shortcut;
		}
		set
		{
			m_Shortcut = value;
		}
	}

	public new virtual Icon Icon
	{
		get
		{
			return base.Icon;
		}
		set
		{
			base.Icon = value;
		}
	}

	public virtual bool BeginGroup
	{
		get
		{
			return m_BeginGroup;
		}
		set
		{
			m_BeginGroup = value;
		}
	}

	public virtual PluginTypes PluginType => PluginTypes.Dialog;

	public virtual string MenuCaption
	{
		get
		{
			return m_menuCaption;
		}
		set
		{
			m_menuCaption = value;
		}
	}

	public virtual string PluginName
	{
		get
		{
			return m_Name;
		}
		set
		{
			m_Name = value;
		}
	}

	public virtual Control control => this;

	public virtual PluginDockPositions PreferredDockState
	{
		get
		{
			return m_pos;
		}
		set
		{
			m_pos = value;
		}
	}

	public AppContext Context => m_appContext;

	public GenericDialogPlugin()
	{
		InitializeComponent();
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
		this.components = new System.ComponentModel.Container();
		base.Size = new System.Drawing.Size(300, 300);
		this.Text = "GenericDialogPlugin";
	}

	public virtual void Configure(IWin32Window parent)
	{
	}

	public virtual void OnClick()
	{
	}

	public virtual void OnClick(object sender, EventArgs e)
	{
		OnClick();
	}

	public virtual void Init(AppContext context)
	{
		m_appContext = context;
	}

	public virtual void BeforeDispose(AppContext context)
	{
	}
}
