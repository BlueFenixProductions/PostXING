namespace PostXING.Components;

public class PostXINGPreferences
{
	private ProxyInfo _httpProxyInfo = new ProxyInfo();

	private bool _autoCreateNewPost;

	private bool _suppressSplashPage;

	private bool _autoSaveEnabled;

	private int _autoSaveIntervalInMinutes = 2;

	private bool _minimizeToNotificationArea;

	private string _uploadUsing = "FTP";

	public ProxyInfo HttpProxyInfo
	{
		get
		{
			return _httpProxyInfo;
		}
		set
		{
			_httpProxyInfo = value;
		}
	}

	public bool AutoCreateNewPost
	{
		get
		{
			return _autoCreateNewPost;
		}
		set
		{
			_autoCreateNewPost = value;
		}
	}

	public bool SuppressSplashPage
	{
		get
		{
			return _suppressSplashPage;
		}
		set
		{
			_suppressSplashPage = value;
		}
	}

	public bool AutoSaveEnabled
	{
		get
		{
			return _autoSaveEnabled;
		}
		set
		{
			_autoSaveEnabled = value;
		}
	}

	public int AutoSaveIntervalInMinutes
	{
		get
		{
			return _autoSaveIntervalInMinutes;
		}
		set
		{
			_autoSaveIntervalInMinutes = value;
		}
	}

	public bool MinimizeToNotificationArea
	{
		get
		{
			return _minimizeToNotificationArea;
		}
		set
		{
			_minimizeToNotificationArea = value;
		}
	}

	public string UploadUsing
	{
		get
		{
			return _uploadUsing;
		}
		set
		{
			_uploadUsing = value;
		}
	}
}
