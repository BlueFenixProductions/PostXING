using System;
using System.Windows.Forms;
using PostXING.Components;

namespace PostXING.Extensibility;

public abstract class AppContext : IDisposable
{
	protected PluginCollection m_pluPlugins = new PluginCollection();

	protected bool _isCallingFromHostApplication = true;

	protected string _currentEditorText = string.Empty;

	protected IEditorForm _editor;

	public virtual PluginCollection Plugins => m_pluPlugins;

	public virtual bool IsCallingFromHostApplication
	{
		get
		{
			return _isCallingFromHostApplication;
		}
		set
		{
			_isCallingFromHostApplication = value;
		}
	}

	public virtual string CurrentEditorText
	{
		get
		{
			return _currentEditorText;
		}
		set
		{
			_currentEditorText = value;
		}
	}

	public virtual IEditorForm Editor
	{
		get
		{
			return _editor;
		}
		set
		{
			_editor = value;
		}
	}

	public AppContext()
	{
	}

	public abstract void InitPlugins();

	public virtual void UnloadPlugins()
	{
		IIPluginEnumerator enumerator = Plugins.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				IPlugin current = enumerator.Current;
				current.BeforeDispose(this);
				current.Dispose();
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		Plugins.Clear();
	}

	public static DockStyle TranslatePluginDockToWinformDock(PluginDockPositions position)
	{
		return position switch
		{
			PluginDockPositions.Left => DockStyle.Left, 
			PluginDockPositions.Bottom => DockStyle.Bottom, 
			PluginDockPositions.Floating => DockStyle.None, 
			PluginDockPositions.Right => DockStyle.Right, 
			PluginDockPositions.Top => DockStyle.Top, 
			_ => DockStyle.Left, 
		};
	}

	public void Dispose()
	{
		UnloadPlugins();
	}
}
