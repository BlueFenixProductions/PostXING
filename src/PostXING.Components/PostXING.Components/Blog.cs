using System;

namespace PostXING.Components;

[Serializable]
public class Blog : ICloneable
{
	private int _itemCount = 15;

	private string _webAddress;

	private string _host;

	private string _page;

	private int _port = 80;

	private string _username;

	private string _password;

	private bool _useCategories;

	private bool _useSSL;

	private bool _selected;

	private bool _autoSelectForCrossPosting;

	private string _blogID = string.Empty;

	private string _blogName = string.Empty;

	private string _providerName;

	private BlogOptions _options = new BlogOptions();

	private FTPSite _fTPInfo = new FTPSite();

	private string[] _cachedCategories;

	private string _blogKey = string.Empty;

	public int ItemCount
	{
		get
		{
			return _itemCount;
		}
		set
		{
			_itemCount = value;
		}
	}

	public string ServiceUrl => SetServiceUrl();

	public string WebAddress
	{
		get
		{
			return _webAddress;
		}
		set
		{
			_webAddress = value;
		}
	}

	public string Host
	{
		get
		{
			return _host;
		}
		set
		{
			_host = value;
		}
	}

	public string Page
	{
		get
		{
			return _page;
		}
		set
		{
			_page = value;
		}
	}

	public int Port
	{
		get
		{
			return _port;
		}
		set
		{
			_port = value;
		}
	}

	public string Username
	{
		get
		{
			return _username;
		}
		set
		{
			_username = value;
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value;
		}
	}

	public bool SupportsCategories
	{
		get
		{
			return _useCategories;
		}
		set
		{
			_useCategories = value;
		}
	}

	public bool UseSSL
	{
		get
		{
			return _useSSL;
		}
		set
		{
			_useSSL = value;
		}
	}

	public bool Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
		}
	}

	public bool AutoSelectForCrossPosting
	{
		get
		{
			return _autoSelectForCrossPosting;
		}
		set
		{
			_autoSelectForCrossPosting = value;
		}
	}

	public string BlogID
	{
		get
		{
			return _blogID;
		}
		set
		{
			_blogID = value;
		}
	}

	public string BlogName
	{
		get
		{
			return _blogName;
		}
		set
		{
			_blogName = value;
		}
	}

	public string ProviderName
	{
		get
		{
			return _providerName;
		}
		set
		{
			_providerName = value;
		}
	}

	public BlogOptions Options
	{
		get
		{
			return _options;
		}
		set
		{
			_options = value;
		}
	}

	public FTPSite FTPInfo
	{
		get
		{
			return _fTPInfo;
		}
		set
		{
			_fTPInfo = value;
		}
	}

	public string[] CachedCategories
	{
		get
		{
			return _cachedCategories;
		}
		set
		{
			_cachedCategories = value;
		}
	}

	public string Key
	{
		get
		{
			if (_blogKey == string.Empty)
			{
				_blogKey = Guid.NewGuid().ToString();
			}
			return _blogKey;
		}
		set
		{
			_blogKey = value;
		}
	}

	public Blog()
	{
	}

	public override string ToString()
	{
		try
		{
			return (BlogName.Trim().Length > 0) ? BlogName : BlogID;
		}
		catch
		{
			return base.ToString();
		}
	}

	public Blog(string serviceUrl, string username, string password)
	{
		Uri uri = new Uri(serviceUrl);
		UseSSL = uri.Scheme.IndexOf("https") > -1;
		Host = uri.Host;
		Page = uri.PathAndQuery;
		Port = uri.Port;
		Username = username;
		Password = password;
	}

	public Blog(string blogID, string blogName, string serviceUrl, string username, string password)
		: this(serviceUrl, username, password)
	{
		BlogID = blogID;
		BlogName = blogName;
	}

	public string SetServiceUrl()
	{
		return SetServiceUrl(Host, Port, Page);
	}

	public string SetServiceUrl(string host, int port, string page)
	{
		UriBuilder uriBuilder = new UriBuilder();
		uriBuilder.Scheme = (UseSSL ? "https" : "http");
		uriBuilder.Host = host;
		uriBuilder.Port = port;
		uriBuilder.Path = page;
		return uriBuilder.Uri.ToString();
	}

	public object Clone()
	{
		Blog blog = new Blog(BlogID, BlogName, ServiceUrl, Username, Password);
		blog.FTPInfo = FTPInfo;
		blog.CachedCategories = CachedCategories;
		blog.Host = Host;
		blog.ItemCount = ItemCount;
		blog.Options = Options;
		blog.Page = Page;
		blog.Port = Port;
		blog.ProviderName = ProviderName;
		blog.Selected = Selected;
		blog.SupportsCategories = SupportsCategories;
		blog.UseSSL = UseSSL;
		blog.WebAddress = WebAddress;
		blog.Key = Key;
		return blog;
	}
}
