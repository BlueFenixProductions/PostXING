using System;
using System.Xml.Serialization;

namespace PostXING.Components;

public class FTPSite
{
	private Uri _target;

	private bool _passive;

	private string _userName;

	private string _password;

	private string _url;

	[XmlIgnore]
	public Uri Target
	{
		get
		{
			return _target;
		}
		set
		{
			_target = value;
		}
	}

	[XmlElement(ElementName = "Target")]
	public string TargetPath
	{
		get
		{
			if (!(_target == null))
			{
				return _target.AbsoluteUri;
			}
			return null;
		}
		set
		{
			_target = ((value == null) ? null : new Uri(value));
		}
	}

	public bool Passive
	{
		get
		{
			return _passive;
		}
		set
		{
			_passive = value;
		}
	}

	public string UserName
	{
		get
		{
			return _userName;
		}
		set
		{
			_userName = value;
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

	public string BaseUrl
	{
		get
		{
			return _url;
		}
		set
		{
			_url = value;
		}
	}
}
