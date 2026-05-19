using System;

namespace PostXING.Components.Legacy.v1;

[Serializable]
public class Blog
{
	private int _itemCount = 15;

	private string _serviceUrl;

	private string _username;

	private string _password;

	private bool _useCategories;

	private bool _pingWeblogsCom;

	private bool _pingBloDotGs;

	private bool _selected;

	private bgBlogInfo _blogInfo;

	private BlogOptions _options = new BlogOptions();

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

	public string ServiceUrl
	{
		get
		{
			return _serviceUrl;
		}
		set
		{
			_serviceUrl = value;
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

	public bool UseCategories
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

	public bool PingWeblogsCom
	{
		get
		{
			return _pingWeblogsCom;
		}
		set
		{
			_pingWeblogsCom = value;
		}
	}

	public bool PingBloDotGs
	{
		get
		{
			return _pingBloDotGs;
		}
		set
		{
			_pingBloDotGs = value;
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

	public string Key => ServiceUrl + BlogInfo.blogid.ToString();

	public bgBlogInfo BlogInfo
	{
		get
		{
			return _blogInfo;
		}
		set
		{
			_blogInfo = value;
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

	public Blog()
	{
	}

	public override string ToString()
	{
		try
		{
			return BlogInfo.blogName;
		}
		catch
		{
			return base.ToString();
		}
	}

	public Blog(string serviceUrl, string username, string password)
	{
		ServiceUrl = serviceUrl;
		Username = username;
		Password = password;
	}
}
