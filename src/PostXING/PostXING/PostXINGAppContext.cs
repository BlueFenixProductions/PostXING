using System;
using System.Collections;
using AppInteropServices;
using PostXING.Extensibility;

namespace PostXING;

public class PostXINGAppContext : AppContext
{
	public override string CurrentEditorText
	{
		get
		{
			return _currentEditorText;
		}
		set
		{
			_currentEditorText = value;
			if (this.CurrentEditorTextChanged != null && !IsCallingFromHostApplication)
			{
				this.CurrentEditorTextChanged(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler CurrentEditorTextChanged;

	public PostXINGAppContext(string CurrentEditorText)
	{
		_currentEditorText = CurrentEditorText;
	}

	public PostXINGAppContext()
		: this(string.Empty)
	{
	}

	public override void InitPlugins()
	{
		ArrayList arrayList = ServiceManager.SearchForIPlugins(AppManager.GetPluginPath());
		if (arrayList != null && arrayList.Count != 0)
		{
			IPlugin[] array = (IPlugin[])arrayList.ToArray(typeof(IPlugin));
			m_pluPlugins = new PluginCollection(array);
		}
	}
}
