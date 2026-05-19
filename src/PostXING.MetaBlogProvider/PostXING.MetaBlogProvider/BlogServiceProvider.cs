using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls.Navigation;
using PostXING.Extensibility;

namespace PostXING.MetaBlogProvider;

public class BlogServiceProvider : IBlogProvider
{
	private IEditorForm _editor;

	private IConfigurationDialog _container;

	private BloggerAPIClientProxy _proxy;

	private ToolStripContainer _buttons;

	private BlogSelectionPage _selectionPage;

	private CredentialsPage _credentialsPage;

	private Page[] _configPages;

	private bool _isConfigurationComplete;

	public Page[] ConfigurationPages => _configPages;

	public string ProviderName => ToString();

	public IWebProxy Proxy
	{
		get
		{
			return _proxy.Proxy;
		}
		set
		{
			_proxy.Proxy = value;
		}
	}

	public bool SupportsCategories => true;

	public bool IsConfigurationComplete
	{
		get
		{
			return _isConfigurationComplete;
		}
		set
		{
			_isConfigurationComplete = value;
		}
	}

	public ToolStripButton[] ButtonBarItems => _buttons.ButtonBarItems;

	public ToolStripButton[] PreviewBarItems => _buttons.PreviewBarItems;

	public ToolStripMenuItem[] MenuItems => _buttons.MenuItems;

	public ToolStripButton[] DeleteButtons => _buttons.DeleteButtons;

	public void InitializeIBlogProvider(IConfigurationDialog container, IEditorForm editor)
	{
		_container = container;
		_editor = editor;
		_proxy = new BloggerAPIClientProxy();
		_proxy.UserAgent = editor.UserAgent;
		_buttons = new ToolStripContainer(this, editor);
		_selectionPage = new BlogSelectionPage(_container, this);
		_credentialsPage = new CredentialsPage(_container, this);
		_configPages = new Page[2] { _credentialsPage, _selectionPage };
	}

	public Blog[] GetBlogs(string url, string userName, string password, string blogIdentifier)
	{
		BlogRequest blogRequest = new BlogRequest();
		blogRequest.Url = url;
		bgBlogInfo[] usersBlogs = blogRequest.getUsersBlogs(blogIdentifier, userName, password);
		Blog[] array = new Blog[usersBlogs.Length];
		for (int i = 0; i < usersBlogs.Length; i++)
		{
			Blog blog = new Blog(url, userName, password);
			blog.BlogID = usersBlogs[i].blogid;
			blog.BlogName = usersBlogs[i].blogName;
			blog.Username = userName;
			blog.Password = password;
			blog.WebAddress = usersBlogs[i].url;
			array[i] = blog;
		}
		return array;
	}

	public string[] GetCategories(Blog currentBlog)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwCategoryInfo[] array = _proxy.metaweblog_getCategories(currentBlog.BlogID, currentBlog.Username, currentBlog.Password);
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].title != null && array[i].title.Trim().Length > 0)
			{
				array2[i] = array[i].title;
			}
			else if (array[i].description.Trim().Length > 0)
			{
				array2[i] = array[i].description;
			}
		}
		return array2;
	}

	public void DeletePost(Blog currentBlog, string postID)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		_proxy.blogger_deletePost(currentBlog.BlogID, postID, currentBlog.Username, currentBlog.Password, publish: true);
	}

	public BlogPost[] GetRecentPosts(Blog currentBlog, int numberOfPosts)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwPost[] array = _proxy.metaweblog_getRecentPosts(currentBlog.BlogID, currentBlog.Username, currentBlog.Password, numberOfPosts);
		BlogPostCollection blogPostCollection = new BlogPostCollection(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			BlogPost blogPost = new BlogPost(array[i].postid, array[i].title, array[i].description, array[i].dateCreated, array[i].categories, publish: false);
			blogPost.Link = array[i].link;
			blogPost.PermaLink = array[i].permalink;
			blogPostCollection.Add(blogPost);
		}
		return blogPostCollection.ToArray();
	}

	public BlogPost GetPost(Blog currentBlog, string postID)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwPost mwPost2 = _proxy.metaweblog_getPost(postID, currentBlog.Username, currentBlog.Password);
		BlogPost blogPost = new BlogPost(mwPost2.postid, mwPost2.title, mwPost2.description, mwPost2.dateCreated, mwPost2.categories, publish: true);
		blogPost.Link = mwPost2.link;
		blogPost.PermaLink = mwPost2.permalink;
		return blogPost;
	}

	public string CreatePost(Blog currentBlog, BlogPost post)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwPost post2 = new mwPost
		{
			categories = post.Categories,
			dateCreated = post.DateCreated,
			description = post.Body,
			title = post.Title,
			link = post.Link
		};
		return _proxy.metaweblog_newPost(currentBlog.BlogID, currentBlog.Username, currentBlog.Password, post2, post.Publish);
	}

	public void UpdatePost(Blog currentBlog, BlogPost post)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwPost post2 = new mwPost
		{
			categories = post.Categories,
			dateCreated = post.DateCreated,
			description = post.Body,
			link = post.Link,
			permalink = post.PermaLink,
			postid = post.PostID,
			title = post.Title,
			userid = currentBlog.Username
		};
		_proxy.metaweblog_editPost(post.PostID, currentBlog.Username, currentBlog.Password, post2, post.Publish);
	}

	public override string ToString()
	{
		return "MetaWeblog API";
	}

	public IAsyncResult BeginGetRecentPosts(Blog currentBlog, int numberOfPosts, AsyncCallback callback, object asyncState)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		return _proxy.Beginmetaweblog_getRecentPosts(currentBlog.BlogID, currentBlog.Username, currentBlog.Password, numberOfPosts, callback, asyncState);
	}

	public BlogPost[] EndGetRecentPosts(IAsyncResult asr)
	{
		mwPost[] array = _proxy.Endmetaweblog_getRecentPosts(asr);
		BlogPost[] array2 = new BlogPost[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i].PostID = array[i].postid;
			array2[i].Categories = array[i].categories;
			array2[i].DateCreated = array[i].dateCreated;
			array2[i].Title = array[i].title;
			array2[i].Body = array[i].description;
			array2[i].Link = array[i].link;
			array2[i].PermaLink = array[i].permalink;
		}
		return array2;
	}

	public IAsyncResult BeginCreatePost(Blog currentBlog, BlogPost post, AsyncCallback callback, object asyncState)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwPost post2 = new mwPost
		{
			categories = post.Categories,
			dateCreated = post.DateCreated,
			description = post.Body,
			title = post.Title,
			link = post.Link
		};
		return _proxy.Beginmetaweblog_newPost(currentBlog.BlogID, currentBlog.Username, currentBlog.Password, post2, post.Publish, callback, asyncState);
	}

	public string EndCreatePost(IAsyncResult asr)
	{
		return _proxy.Endmetaweblog_newPost(asr);
	}

	public IAsyncResult BeginUpdatePost(Blog currentBlog, BlogPost post, AsyncCallback callback, object asyncState)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		mwPost post2 = new mwPost
		{
			categories = post.Categories,
			dateCreated = post.DateCreated,
			description = post.Body,
			link = post.Link,
			permalink = post.PermaLink,
			postid = post.PostID,
			title = post.Title,
			userid = currentBlog.Username
		};
		return _proxy.Beginmetaweblog_editPost(post.PostID, currentBlog.Username, currentBlog.Password, post2, post.Publish, callback, asyncState);
	}

	public bool EndUpdatePost(IAsyncResult asr)
	{
		return _proxy.Endmetaweblog_editPost(asr);
	}

	public IAsyncResult BeginDeletePost(Blog currentBlog, string postID, AsyncCallback callback, object asyncState)
	{
		_proxy.Url = currentBlog.ServiceUrl;
		return _proxy.Beginblogger_deletePost(currentBlog.BlogID, postID, currentBlog.Username, currentBlog.Password, publish: true, callback, asyncState);
	}

	public bool EndDeletePost(IAsyncResult asr)
	{
		return _proxy.Endblogger_deletePost(asr);
	}

	public string UploadFile(Blog currentBlog, string mimeType, string clientFilePath)
	{
		mediaObject mediaobject = new mediaObject
		{
			type = mimeType
		};
		FileInfo fileInfo = new FileInfo(clientFilePath);
		mediaobject.name = fileInfo.Name;
		using (FileStream fileStream = File.Open(clientFilePath, FileMode.Open))
		{
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, (int)fileStream.Length);
			mediaobject.bits = array;
		}
		return _proxy.metaweblog_newMediaObject(currentBlog.BlogID, currentBlog.Username, currentBlog.Password, mediaobject).url;
	}
}
