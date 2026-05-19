using System;
using System.Net;
using System.Windows.Forms;
using PostXING.Components;
using PostXING.Controls.Navigation;

namespace PostXING.Extensibility;

public interface IBlogProvider
{
	string ProviderName { get; }

	bool IsConfigurationComplete { get; set; }

	IWebProxy Proxy { get; set; }

	bool SupportsCategories { get; }

	Page[] ConfigurationPages { get; }

	ToolStripButton[] ButtonBarItems { get; }

	ToolStripButton[] PreviewBarItems { get; }

	ToolStripMenuItem[] MenuItems { get; }

	ToolStripButton[] DeleteButtons { get; }

	string CreatePost(Blog currentBlog, BlogPost post);

	IAsyncResult BeginCreatePost(Blog currentBlog, BlogPost post, AsyncCallback callback, object asyncState);

	string EndCreatePost(IAsyncResult asr);

	Blog[] GetBlogs(string url, string userName, string password, string blogIdentifier);

	BlogPost GetPost(Blog currentBlog, string postID);

	void UpdatePost(Blog currentBlog, BlogPost post);

	IAsyncResult BeginUpdatePost(Blog currentBlog, BlogPost post, AsyncCallback callback, object asyncState);

	bool EndUpdatePost(IAsyncResult asr);

	void DeletePost(Blog currentBlog, string postID);

	IAsyncResult BeginDeletePost(Blog currentBlog, string postID, AsyncCallback callback, object asyncState);

	bool EndDeletePost(IAsyncResult asr);

	BlogPost[] GetRecentPosts(Blog currentBlog, int numberOfPosts);

	IAsyncResult BeginGetRecentPosts(Blog currentBlog, int numberOfPosts, AsyncCallback callback, object asyncState);

	BlogPost[] EndGetRecentPosts(IAsyncResult asr);

	string[] GetCategories(Blog currentBlog);

	string UploadFile(Blog currentBlog, string mimeType, string clientFilePath);

	new string ToString();

	void InitializeIBlogProvider(IConfigurationDialog container, IEditorForm editor);
}
