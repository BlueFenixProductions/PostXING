using System;

namespace PostXING.Components;

public class BlogPost
{
	private string _title;

	private string _postID;

	private string _body;

	private DateTime _dateCreated = DateTime.Now;

	private string[] _categories;

	private bool _publish;

	private string _link;

	private string _permaLink;

	private string _fileName;

	private string _autoSaveID;

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
		}
	}

	public string PostID
	{
		get
		{
			return _postID;
		}
		set
		{
			_postID = value;
		}
	}

	public string Body
	{
		get
		{
			return _body;
		}
		set
		{
			_body = value;
		}
	}

	public DateTime DateCreated
	{
		get
		{
			return _dateCreated;
		}
		set
		{
			_dateCreated = value;
		}
	}

	public string[] Categories
	{
		get
		{
			return _categories;
		}
		set
		{
			_categories = value;
		}
	}

	public bool Publish
	{
		get
		{
			return _publish;
		}
		set
		{
			_publish = value;
		}
	}

	public string Link
	{
		get
		{
			return _link;
		}
		set
		{
			_link = value;
		}
	}

	public string PermaLink
	{
		get
		{
			return _permaLink;
		}
		set
		{
			_permaLink = value;
		}
	}

	public string FileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			_fileName = value;
		}
	}

	public string AutoSaveFileName
	{
		get
		{
			if (string.IsNullOrEmpty(_autoSaveID))
			{
				_autoSaveID = Guid.NewGuid().ToString();
			}
			return _autoSaveID;
		}
		set
		{
			_autoSaveID = value;
		}
	}

	public BlogPost()
	{
	}

	public BlogPost(string postID, string title, string body, DateTime dateCreated, string[] categories, bool publish)
	{
		PostID = postID;
		Title = title;
		Body = body;
		DateCreated = dateCreated;
		Categories = categories;
		Publish = publish;
	}
}
