namespace PostXING.Components;

public class ProxyInfo
{
	private bool _useProxy;

	private bool _bypassLocal;

	private bool _requiresLogin;

	private bool _overrideDefaultProxy;

	private int _proxyPort = 8080;

	private string _proxyName;

	private string _proxyPassword;

	private string _proxyDomain;

	private string _proxyUserName;

	public bool UseProxy
	{
		get
		{
			return _useProxy;
		}
		set
		{
			_useProxy = value;
		}
	}

	public bool BypassLocal
	{
		get
		{
			return _bypassLocal;
		}
		set
		{
			_bypassLocal = value;
		}
	}

	public bool RequiresLogin
	{
		get
		{
			return _requiresLogin;
		}
		set
		{
			_requiresLogin = value;
		}
	}

	public bool OverrideDefaultProxy
	{
		get
		{
			return _overrideDefaultProxy;
		}
		set
		{
			_overrideDefaultProxy = value;
		}
	}

	public int ProxyPort
	{
		get
		{
			return _proxyPort;
		}
		set
		{
			_proxyPort = value;
		}
	}

	public string ProxyName
	{
		get
		{
			return _proxyName;
		}
		set
		{
			_proxyName = value;
		}
	}

	public string ProxyPassword
	{
		get
		{
			return _proxyPassword;
		}
		set
		{
			_proxyPassword = value;
		}
	}

	public string ProxyDomain
	{
		get
		{
			return _proxyDomain;
		}
		set
		{
			_proxyDomain = value;
		}
	}

	public string ProxyUserName
	{
		get
		{
			return _proxyUserName;
		}
		set
		{
			_proxyUserName = value;
		}
	}

	public ProxyInfo()
		: this(useProxy: false, bypassLocal: false, requiresLogin: false, overrideDefaultProxy: true, string.Empty, 8080, string.Empty, string.Empty, string.Empty)
	{
	}

	public ProxyInfo(bool useProxy)
		: this(useProxy, bypassLocal: false, requiresLogin: false, overrideDefaultProxy: true, string.Empty, 8080, string.Empty, string.Empty, string.Empty)
	{
	}

	public ProxyInfo(bool useProxy, bool bypassLocal, bool requiresLogin, bool overrideDefaultProxy, string proxyName, int proxyPort, string proxyDomain, string proxyUserName, string proxyPassword)
	{
		UseProxy = useProxy;
		BypassLocal = bypassLocal;
		RequiresLogin = requiresLogin;
		OverrideDefaultProxy = overrideDefaultProxy;
		ProxyName = proxyName;
		ProxyPort = proxyPort;
		ProxyDomain = proxyDomain;
		ProxyUserName = proxyUserName;
		ProxyPassword = proxyPassword;
	}
}
