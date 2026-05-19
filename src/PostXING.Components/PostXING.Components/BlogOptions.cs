using System;
using System.Web;
using System.Xml.Serialization;

namespace PostXING.Components;

public class BlogOptions
{
	private string _htmlTag = "<p class='media'>";

	private string _currentlyPlayingTemplate = "[ Currently Playing : {title} - {artist} - {album} ({duration}) ]";

	private string _nothingPlayingTemplate = "[ Nothing Playing. ]";

	private bool _includeMediaFormatter;

	private string _previewTemplate = "<HTML><HEAD>\r\n<link type='text/css' rel='stylesheet' href='style.css'></link>\r\n</HEAD><BODY>\r\n<div id='main'><div class='post'>\r\n<h2><a href='[Post Url]'>[Post Title]</a></h2>\r\n[Post Body]\r\n</div></div>\r\n</BODY></HTML>";

	[XmlAttribute]
	public string HtmlTag
	{
		get
		{
			return HttpUtility.HtmlDecode(_htmlTag);
		}
		set
		{
			_htmlTag = HttpUtility.HtmlEncode(value);
		}
	}

	[XmlAttribute]
	public string CurrentlyPlayingTemplate
	{
		get
		{
			return HttpUtility.HtmlDecode(_currentlyPlayingTemplate);
		}
		set
		{
			_currentlyPlayingTemplate = HttpUtility.HtmlEncode(value);
		}
	}

	[XmlAttribute]
	public string NothingPlayingTemplate
	{
		get
		{
			return HttpUtility.HtmlDecode(_nothingPlayingTemplate);
		}
		set
		{
			_nothingPlayingTemplate = HttpUtility.HtmlEncode(value);
		}
	}

	[XmlAttribute]
	public bool IncludeMediaFormatter
	{
		get
		{
			return _includeMediaFormatter;
		}
		set
		{
			_includeMediaFormatter = value;
		}
	}

	[XmlAttribute]
	public string PreviewTemplate
	{
		get
		{
			return HttpUtility.HtmlDecode(_previewTemplate);
		}
		set
		{
			_previewTemplate = HttpUtility.HtmlEncode(value.Replace(Environment.NewLine, ""));
		}
	}
}
