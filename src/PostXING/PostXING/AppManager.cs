using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using AppInteropServices;
using PostXING.Components;
using PostXING.Controls.HtmlEditor.Html;
using PostXING.Dialogs;
using PostXING.Extensibility;
using PostXING.Forms;
using PostXING.MetaBlogProvider;

namespace PostXING;

public class AppManager
{
	public static PostSelectedEventHandler PostSelected;

	private static PostXINGPreferences _preferences = new PostXINGPreferences();

	private static IWebProxy _httpProxy = null;

	private static IBlogProvider _currentProvider;

	private static BlogProviderCollection _availableProviders;

	private static IEditorForm _editorForm;

	private static IConfigurationDialog _optionsDialog;

	private static Blog _currentBlog;

	private static BlogCollection _blogs = new BlogCollection();

	private static string[] _categories;

	private static string _tempFileLocation = string.Empty;

	private static string _tempXmlFragment = string.Empty;

	public static PostXINGPreferences Preferences
	{
		get
		{
			return _preferences;
		}
		set
		{
			_preferences = value;
			Save(value);
		}
	}

	public static IWebProxy HttpProxy
	{
		get
		{
			return _httpProxy;
		}
		set
		{
			_httpProxy = value;
		}
	}

	public static IBlogProvider CurrentProvider
	{
		get
		{
			return _currentProvider;
		}
		set
		{
			_currentProvider = value;
		}
	}

	public static BlogProviderCollection AvailableProviders
	{
		get
		{
			if (_availableProviders == null)
			{
				_availableProviders = _loadIBlogProviders();
			}
			return _availableProviders;
		}
		set
		{
			_availableProviders = value;
		}
	}

	public static IEditorForm EditorForm
	{
		get
		{
			return _editorForm;
		}
		set
		{
			_editorForm = value;
		}
	}

	public static EditorForm ConcreteEditor => _editorForm as EditorForm;

	public static IConfigurationDialog OptionsDialog
	{
		get
		{
			return _optionsDialog;
		}
		set
		{
			_optionsDialog = value;
		}
	}

	public static Blog CurrentBlog
	{
		get
		{
			return _currentBlog;
		}
		set
		{
			_currentBlog = value;
		}
	}

	public static BlogCollection Blogs
	{
		get
		{
			return _blogs;
		}
		set
		{
			_blogs = value;
		}
	}

	public static string[] Categories
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

	public static string PreferencePath => Path.Combine(GetUserPath(), PreferenceFileName);

	public static string PreferenceFileName => "Preferences.xml";

	public static string ConfigFilePath => Path.Combine(GetUserPath(), ConfigFileName);

	public static string ConfigFileName => "Blogs.v2.xml";

	public static string TempFileLocation
	{
		get
		{
			return _tempFileLocation;
		}
		set
		{
			_tempFileLocation = value;
		}
	}

	public static string TempXmlFragment
	{
		get
		{
			return _tempXmlFragment;
		}
		set
		{
			_tempXmlFragment = value;
		}
	}

	public static event EventHandler OnApplyPreferences;

	private AppManager()
	{
	}

	public static void Start(PostXINGPreferences preloadedPreferences)
	{
		_saveLocation();
		AppManager.OnApplyPreferences = (EventHandler)Delegate.Combine(AppManager.OnApplyPreferences, new EventHandler(AppManager_OnApplyPreferences));
		BlogCollection blogCollection = LoadConfig();
		if (blogCollection == null || blogCollection.Count == 0)
		{
			DialogResult dialogResult = MessageBox.Show("This looks like the first time you've run PostXING v2. Would you like to try to upgrade existing PostXING v1 blogs?", "Upgrade?", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				using UpgradeDialog upgradeDialog = new UpgradeDialog();
				upgradeDialog.ShowDialog();
			}
			else
			{
				using NewUserDialog newUserDialog = new NewUserDialog();
				newUserDialog.ShowDialog();
			}
		}
		if (blogCollection == null)
		{
			return;
		}
		Blogs = blogCollection;
		IBlogEnumerator enumerator = blogCollection.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Blog current = enumerator.Current;
				if (current.Selected)
				{
					CurrentBlog = current;
					CurrentProvider = AvailableProviders[current.ProviderName];
					CurrentProvider.InitializeIBlogProvider(OptionsDialog, EditorForm);
					EditorForm.LoadToolbarButtons();
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		if (CurrentBlog == null)
		{
			CurrentBlog = blogCollection[0];
		}
		ApplyPreferences(preloadedPreferences);
	}

	public static void ApplyPreferences(PostXINGPreferences preferences)
	{
		if (AppManager.OnApplyPreferences != null)
		{
			AppManager.OnApplyPreferences(preferences, EventArgs.Empty);
		}
	}

	public static void ApplyPreferences()
	{
		ApplyPreferences(Preferences);
	}

	public static PostXINGPreferences LoadPreferences()
	{
		PostXINGPreferences postXINGPreferences = (PostXINGPreferences)_load(PreferencePath, typeof(PostXINGPreferences));
		if (postXINGPreferences == null)
		{
			postXINGPreferences = new PostXINGPreferences();
		}
		return postXINGPreferences;
	}

	public static void Save(PostXINGPreferences prefs)
	{
		_save(PreferencePath, prefs);
	}

	public static BlogCollection LoadConfig()
	{
		return (BlogCollection)_load(ConfigFilePath, typeof(BlogCollection));
	}

	public static void Save(BlogCollection bc)
	{
		_save(ConfigFilePath, bc);
	}

	internal static object _load(string filePath, Type type)
	{
		FileStream fileStream = null;
		try
		{
			if (File.Exists(filePath))
			{
				fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				return xmlSerializer.Deserialize(fileStream);
			}
			return null;
		}
		catch (InvalidOperationException)
		{
			return null;
		}
		finally
		{
			fileStream?.Close();
		}
	}

	internal static void _save(string filePath, object data)
	{
		FileStream fileStream = null;
		if (data == null)
		{
			return;
		}
		try
		{
			fileStream = new FileStream(filePath, FileMode.Create);
			XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
			xmlSerializer.Serialize(fileStream, data);
		}
		finally
		{
			fileStream?.Close();
		}
	}

	public static string GetUserPath()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PostXING");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public static void SetSelected(Blog b)
	{
		IBlogEnumerator enumerator = Blogs.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Blog current = enumerator.Current;
				current.Selected = false;
				if (current.Key == b.Key)
				{
					current.Selected = true;
					CurrentBlog = current;
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

	public static string GetPluginPath()
	{
		string text = Path.Combine(Application.StartupPath, "plugins");
		if (!Directory.Exists(text))
		{
			return null;
		}
		return text;
	}

	private static void _saveLocation()
	{
		string path = "Location.txt";
		string path2 = Path.Combine(GetUserPath(), path);
		string empty = string.Empty;
		empty = Assembly.GetEntryAssembly().Location;
		using StreamWriter streamWriter = new StreamWriter(path2);
		streamWriter.WriteLine(empty);
	}

	private static BlogProviderCollection _loadIBlogProviders()
	{
		return new BlogProviderCollection(new IBlogProvider[1]
		{
			new BlogServiceProvider()
		});
	}

	public static ArrayList LoadIBlogExtensions()
	{
		return ServiceManager.SearchForIBlogExtensions(GetPluginPath());
	}

	public static BlogPost LoadExistingPost(string fileName)
	{
		HPathDocument hPathDocument = new HPathDocument();
		BlogPost blogPost = new BlogPost();
		string text;
		using (StreamReader streamReader = new StreamReader(fileName))
		{
			text = streamReader.ReadToEnd();
		}
		hPathDocument.LoadHtml(text);
		HtmlNode htmlNode = hPathDocument.DocumentNode.SelectSingleNode("//meta[@name='categories']");
		if (htmlNode != null)
		{
			string attributeValue = htmlNode.GetAttributeValue("content", "");
			blogPost.Categories = attributeValue.Split(';');
		}
		HtmlNode htmlNode2 = hPathDocument.DocumentNode.SelectSingleNode("//title");
		blogPost.Title = htmlNode2.InnerText;
		blogPost.Link = htmlNode2.GetAttributeValue("source", "");
		blogPost.Body = text;
		return blogPost;
	}

	private static void AppManager_OnApplyPreferences(object sender, EventArgs e)
	{
		if (CurrentBlog != null && sender is PostXINGPreferences postXINGPreferences)
		{
			Preferences = postXINGPreferences;
			if (postXINGPreferences.HttpProxyInfo.UseProxy)
			{
				HttpProxy = ProxyFactory.Create(postXINGPreferences.HttpProxyInfo, CurrentBlog.ServiceUrl);
				CurrentProvider.Proxy = HttpProxy;
			}
			else
			{
				HttpProxy = null;
			}
		}
	}
}
