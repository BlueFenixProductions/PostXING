using System;
using CookComputing.XmlRpc;

namespace PostXING.MetaBlogProvider;

public class BloggerAPIClientProxy : XmlRpcClientProtocol
{
	[XmlRpcMethod("blogger.deletePost", Description = "Deletes a post.")]
	[return: XmlRpcReturnValue(Description = "Always returns true.")]
	public bool blogger_deletePost(string appKey, string postid, string username, string password, [XmlRpcParameter(Description = "Where applicable, this specifies whether the blog should be republished after the post has been deleted.")] bool publish)
	{
		return (bool)Invoke("blogger_deletePost", new object[5] { appKey, postid, username, password, publish });
	}

	public IAsyncResult Beginblogger_deletePost(string appKey, string postid, string username, string password, bool publish, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_deletePost", new object[5] { appKey, postid, username, password, publish }, callback, asyncState);
	}

	public bool Endblogger_deletePost(IAsyncResult asr)
	{
		return (bool)EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.editPost", Description = "Edits a given post. Optionally, will publish the blog after making the edit.")]
	[return: XmlRpcReturnValue(Description = "Always returns true.")]
	public bool blogger_editPost(string appKey, string postid, string username, string password, string content, bool publish)
	{
		return (bool)Invoke("blogger_editPost", new object[6] { appKey, postid, username, password, content, publish });
	}

	public IAsyncResult Beginblogger_editPost(string appKey, string postid, string username, string password, string content, bool publish, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_editPost", new object[6] { appKey, postid, username, password, content, publish }, callback, asyncState);
	}

	public bool Endblogger_editPost(IAsyncResult asr)
	{
		return (bool)EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.getCategories", Description = "Returns a list of the categories that you can use to log against a post.")]
	public bgCategory[] blogger_getCategories(string blogid, string username, string password)
	{
		return (bgCategory[])Invoke("blogger_getCategories", new object[3] { blogid, username, password });
	}

	public IAsyncResult Beginblogger_getCategories(string blogid, string username, string password, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_getCategories", new object[3] { blogid, username, password }, callback, asyncState);
	}

	public bgCategory[] Endblogger_getCategories(IAsyncResult asr)
	{
		return (bgCategory[])EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.getPost", Description = "Returns a single post.")]
	public bgPost blogger_getPost(string appKey, string postid, string username, string password)
	{
		return (bgPost)Invoke("blogger_getPost", new object[4] { appKey, postid, username, password });
	}

	public IAsyncResult Beginblogger_getPost(string appKey, string postid, string username, string password, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_getPost", new object[4] { appKey, postid, username, password }, callback, asyncState);
	}

	public bgPost Endblogger_getPost(IAsyncResult asr)
	{
		return (bgPost)EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.getRecentPosts", Description = "Returns a list of the most recent posts in the system.")]
	public bgPost[] blogger_getRecentPosts(string appKey, string blogid, string username, string password, int numberOfPosts)
	{
		return (bgPost[])Invoke("blogger_getRecentPosts", new object[5] { appKey, blogid, username, password, numberOfPosts });
	}

	public IAsyncResult Beginblogger_getRecentPosts(string appKey, string blogid, string username, string password, int numberOfPosts, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_getRecentPosts", new object[5] { appKey, blogid, username, password, numberOfPosts }, callback, asyncState);
	}

	public bgPost[] Endblogger_getRecentPosts(IAsyncResult asr)
	{
		return (bgPost[])EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.getTemplate", Description = "Returns the main or archive index template of a given blog.")]
	public string blogger_getTemplate(string appKey, string blogid, string username, string password, string templateType)
	{
		return (string)Invoke("blogger_getTemplate", new object[5] { appKey, blogid, username, password, templateType });
	}

	public IAsyncResult Beginblogger_getTemplate(string appKey, string blogid, string username, string password, string templateType, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_getTemplate", new object[5] { appKey, blogid, username, password, templateType }, callback, asyncState);
	}

	public string Endblogger_getTemplate(IAsyncResult asr)
	{
		return (string)EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.getUserInfo", Description = "Authenticates a user and returns basic user info (name, email, userid, etc.).")]
	public bgUserInfo blogger_getUserInfo(string appKey, string username, string password)
	{
		return (bgUserInfo)Invoke("blogger_getUserInfo", new object[3] { appKey, username, password });
	}

	public IAsyncResult Beginblogger_getUserInfo(string appKey, string username, string password, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_getUserInfo", new object[3] { appKey, username, password }, callback, asyncState);
	}

	public bgUserInfo Endblogger_getUserInfo(IAsyncResult asr)
	{
		return (bgUserInfo)EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.getUsersBlogs", Description = "Returns information on all the blogs a given user is a member.")]
	public bgBlogInfo[] getUsersBlogs(string appKey, string username, string password)
	{
		return (bgBlogInfo[])Invoke("getUsersBlogs", new object[3] { appKey, username, password });
	}

	public IAsyncResult Beginblogger_getUsersBlogs(string appKey, string username, string password, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_getUsersBlogs", new object[3] { appKey, username, password }, callback, asyncState);
	}

	public bgBlogInfo[] Endblogger_getUsersBlogs(IAsyncResult asr)
	{
		return (bgBlogInfo[])EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.newPost", Description = "Makes a new post to a designated blog. Optionally, will publish the blog after making the post.")]
	[return: XmlRpcReturnValue(Description = "Id of new post")]
	public string blogger_newPost(string appKey, string blogid, string username, string password, string content, bool publish)
	{
		return (string)Invoke("blogger_newPost", new object[6] { appKey, blogid, username, password, content, publish });
	}

	public IAsyncResult Beginblogger_newPost(string appKey, string blogid, string username, string password, string content, bool publish, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_newPost", new object[6] { appKey, blogid, username, password, content, publish }, callback, asyncState);
	}

	public string Endblogger_newPost(IAsyncResult asr)
	{
		return (string)EndInvoke(asr);
	}

	[XmlRpcMethod("blogger.setTemplate", Description = "Edits the main or archive index template of a given blog.")]
	public bool blogger_setTemplate(string appKey, string blogid, string username, string password, string template, string templateType)
	{
		return (bool)Invoke("blogger_setTemplate", new object[6] { appKey, blogid, username, password, template, templateType });
	}

	public IAsyncResult Beginblogger_setTemplate(string appKey, string blogid, string username, string password, string template, string templateType, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("blogger_setTemplate", new object[6] { appKey, blogid, username, password, template, templateType }, callback, asyncState);
	}

	public bool Endblogger_setTemplate(IAsyncResult asr)
	{
		return (bool)EndInvoke(asr);
	}

	[XmlRpcMethod("metaWeblog.editPost", Description = "Updates an existing post to a designated blog using the metaWeblog API. Returns true if completed.")]
	public bool metaweblog_editPost(string postid, string username, string password, mwPost post, bool publish)
	{
		return (bool)Invoke("metaweblog_editPost", new object[5] { postid, username, password, post, publish });
	}

	public IAsyncResult Beginmetaweblog_editPost(string postid, string username, string password, mwPost post, bool publish, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("metaweblog_editPost", new object[5] { postid, username, password, post, publish }, callback, asyncState);
	}

	public bool Endmetaweblog_editPost(IAsyncResult asr)
	{
		return (bool)EndInvoke(asr);
	}

	[XmlRpcMethod("metaWeblog.getCategories", Description = "Retrieves a list of valid categories for a post using the metaWeblog API. Returns the metaWeblog categories struct collection.")]
	public mwCategoryInfo[] metaweblog_getCategories(string blogid, string username, string password)
	{
		return (mwCategoryInfo[])Invoke("metaweblog_getCategories", new object[3] { blogid, username, password });
	}

	[XmlRpcMethod("metaWeblog.newMediaObject", Description = "Uploads an image, movie, song, or other media using the metaWeblog API. Returns the metaObject struct.")]
	public mediaObjectInfo metaweblog_newMediaObject(string blogid, string username, string password, mediaObject mediaobject)
	{
		return (mediaObjectInfo)Invoke("metaweblog_newMediaObject", new object[4] { blogid, username, password, mediaobject });
	}

	public IAsyncResult Beginmetaweblog_getCategories(string blogid, string username, string password, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("metaweblog_getCategories", new object[3] { blogid, username, password }, callback, asyncState);
	}

	public mwCategoryInfo[] Endmetaweblog_getCategories(IAsyncResult asr)
	{
		return (mwCategoryInfo[])EndInvoke(asr);
	}

	[XmlRpcMethod("metaWeblog.getPost", Description = "Retrieves an existing post using the metaWeblog API. Returns the metaWeblog struct.")]
	public mwPost metaweblog_getPost(string postid, string username, string password)
	{
		return (mwPost)Invoke("metaweblog_getPost", new object[3] { postid, username, password });
	}

	public IAsyncResult Beginmetaweblog_getPost(string postid, string username, string password, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("metaweblog_getPost", new object[3] { postid, username, password }, callback, asyncState);
	}

	public mwPost Endmetaweblog_getPost(IAsyncResult asr)
	{
		return (mwPost)EndInvoke(asr);
	}

	[XmlRpcMethod("metaWeblog.getRecentPosts", Description = "Retrieves a list of the most recent existing post using the metaWeblog API. Returns the metaWeblog struct collection.")]
	public mwPost[] metaweblog_getRecentPosts(string blogid, string username, string password, int numberOfPosts)
	{
		return (mwPost[])Invoke("metaweblog_getRecentPosts", new object[4] { blogid, username, password, numberOfPosts });
	}

	public IAsyncResult Beginmetaweblog_getRecentPosts(string blogid, string username, string password, int numberOfPosts, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("metaweblog_getRecentPosts", new object[4] { blogid, username, password, numberOfPosts }, callback, asyncState);
	}

	public mwPost[] Endmetaweblog_getRecentPosts(IAsyncResult asr)
	{
		return (mwPost[])EndInvoke(asr);
	}

	[XmlRpcMethod("metaWeblog.newPost", Description = "Makes a new post to a designated blog using the metaWeblog API. Returns postid as a string.")]
	public string metaweblog_newPost(string blogid, string username, string password, mwPost post, bool publish)
	{
		return (string)Invoke("metaweblog_newPost", new object[5] { blogid, username, password, post, publish });
	}

	public IAsyncResult Beginmetaweblog_newPost(string blogid, string username, string password, mwPost post, bool publish, AsyncCallback callback, object asyncState)
	{
		return BeginInvoke("metaweblog_newPost", new object[5] { blogid, username, password, post, publish }, callback, asyncState);
	}

	public string Endmetaweblog_newPost(IAsyncResult asr)
	{
		return (string)EndInvoke(asr);
	}

	[XmlRpcMethod("mt.getCategoryList", Description = "Returns a list of all categories defined in the weblog.")]
	[return: XmlRpcReturnValue(Description = "The isPrimary member of each mtCategory structs is not used.")]
	public mtCategory[] mt_getCategoryList(string blogid, string username, string password)
	{
		return (mtCategory[])Invoke("mt_getCategoryList", new object[3] { blogid, username, password });
	}

	[XmlRpcMethod("mt.getPostCategories", Description = "Returns a list of all categories to which the post is assigned.")]
	public mtCategory[] mt_getPostCategories(string postid, string username, string password)
	{
		return (mtCategory[])Invoke("mt_getPostCategories", new object[3] { postid, username, password });
	}

	[XmlRpcMethod("mt.getRecentPostTitles", Description = "Returns a bandwidth-friendly list of the most recent posts in the system.")]
	public mtPostTitle[] mt_getRecentPostTitles(string blogid, string username, string password, int numberOfPosts)
	{
		return (mtPostTitle[])Invoke("mt_getRecentPostTitles", new object[3] { blogid, username, password });
	}

	[XmlRpcMethod("mt.getTrackbackPings", Description = "Retrieve the list of TrackBack pings posted to a particular entry. This could be used to programmatically retrieve the list of pings for a particular entry, then iterate through each of those pings doing the same, until one has built up a graph of the web of entries referencing one another on a particular topic.")]
	public mtTrackbackPing[] mt_getTrackbackPings(string postid)
	{
		return (mtTrackbackPing[])Invoke("mt_getTrackbackPings", new object[1] { postid });
	}

	[XmlRpcMethod("mt.publishPost", Description = "Publish (rebuild) all of the static files related to an entry from your weblog. Equivalent to saving an entry in the system (but without the ping).")]
	[return: XmlRpcReturnValue(Description = "Always returns true.")]
	public bool mt_publishPost(string postid, string username, string password)
	{
		return (bool)Invoke("mt_publishPost", new object[3] { postid, username, password });
	}

	[XmlRpcMethod("mt.setPostCategories", Description = "Sets the categories for a post.")]
	[return: XmlRpcReturnValue(Description = "Always returns true.")]
	public bool mt_setPostCategories(string postid, string username, string password, [XmlRpcParameter(Description = "categoryName not required in mtCategory struct.")] mtCategory[] categories)
	{
		return (bool)Invoke("mt_setPostCategories", new object[4] { postid, username, password, categories });
	}

	[XmlRpcMethod("mt.supportedMethods", Description = "The method names supported by the server.")]
	[return: XmlRpcReturnValue(Description = "The method names supported by the server.")]
	public string[] mt_supportedMethods()
	{
		return (string[])Invoke("mt_supportedMethods");
	}

	[XmlRpcMethod("mt.supportedTextFilters", Description = "The text filters names supported by the server.")]
	[return: XmlRpcReturnValue(Description = "The text filters names supported by the server.")]
	public mtTextFilter[] mt_supportedTextFilters()
	{
		return (mtTextFilter[])Invoke("mt_supportedTextFilters");
	}
}
