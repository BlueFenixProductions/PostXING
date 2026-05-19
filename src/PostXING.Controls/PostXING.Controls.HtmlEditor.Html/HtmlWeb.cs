using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Microsoft.Win32;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlWeb
{
	public delegate bool PreRequestHandler(HttpWebRequest request);

	public delegate void PostResponseHandler(HttpWebRequest request, HttpWebResponse response);

	public delegate void PreHandleDocumentHandler(HPathDocument document);

	private int _streamBufferSize = 1024;

	private string _cachePath;

	private bool _usingCache;

	private bool _fromCache;

	private bool _cacheOnly;

	private bool _useCookies;

	private int _requestDuration;

	private bool _autoDetectEncoding = true;

	private HttpStatusCode _statusCode = HttpStatusCode.OK;

	private Uri _responseUri;

	public PreRequestHandler PreRequest;

	public PostResponseHandler PostResponse;

	public PreHandleDocumentHandler PreHandleDocument;

	public bool FromCache => _fromCache;

	public Uri ResponseUri => _responseUri;

	public bool CacheOnly
	{
		get
		{
			return _cacheOnly;
		}
		set
		{
			if (value && !UsingCache)
			{
				throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
			}
			_cacheOnly = value;
		}
	}

	public bool UseCookies
	{
		get
		{
			return _useCookies;
		}
		set
		{
			_useCookies = value;
		}
	}

	public int RequestDuration => _requestDuration;

	public bool AutoDetectEncoding
	{
		get
		{
			return _autoDetectEncoding;
		}
		set
		{
			_autoDetectEncoding = value;
		}
	}

	public HttpStatusCode StatusCode => _statusCode;

	public int StreamBufferSize
	{
		get
		{
			return _streamBufferSize;
		}
		set
		{
			if (_streamBufferSize <= 0)
			{
				throw new ArgumentException("Size must be greater than zero.");
			}
			_streamBufferSize = value;
		}
	}

	public string CachePath
	{
		get
		{
			return _cachePath;
		}
		set
		{
			_cachePath = value;
		}
	}

	public bool UsingCache
	{
		get
		{
			if (_cachePath == null)
			{
				return false;
			}
			return _usingCache;
		}
		set
		{
			if (value && _cachePath == null)
			{
				throw new HtmlWebException("You need to define a CachePath first.");
			}
			_usingCache = value;
		}
	}

	public void Get(string url, string path)
	{
		Get(url, path, "GET");
	}

	public void Get(string url, string path, string method)
	{
		Uri uri = new Uri(url);
		if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
		{
			Get(uri, method, path, null);
			return;
		}
		throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
	}

	public HPathDocument Load(string url)
	{
		return Load(url, "GET");
	}

	public HPathDocument Load(string url, string method)
	{
		Uri uri = new Uri(url);
		HPathDocument hPathDocument;
		if (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)
		{
			hPathDocument = LoadUrl(uri, method);
		}
		else
		{
			if (!(uri.Scheme == Uri.UriSchemeFile))
			{
				throw new HtmlWebException("Unsupported uri scheme: '" + uri.Scheme + "'.");
			}
			hPathDocument = new HPathDocument();
			hPathDocument.OptionAutoCloseOnEnd = false;
			hPathDocument.OptionAutoCloseOnEnd = true;
			hPathDocument.DetectEncodingAndLoad(url, _autoDetectEncoding);
		}
		if (PreHandleDocument != null)
		{
			PreHandleDocument(hPathDocument);
		}
		return hPathDocument;
	}

	private bool IsCacheHtmlContent(string path)
	{
		string contentTypeForExtension = GetContentTypeForExtension(Path.GetExtension(path), null);
		return IsHtmlContent(contentTypeForExtension);
	}

	private bool IsHtmlContent(string contentType)
	{
		return contentType.ToLower().StartsWith("text/html");
	}

	private string GetCacheHeadersPath(Uri uri)
	{
		return GetCachePath(uri) + ".h.xml";
	}

	public string GetCachePath(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (!UsingCache)
		{
			throw new HtmlWebException("Cache is not enabled. Set UsingCache to true first.");
		}
		if (uri.AbsolutePath == "/")
		{
			return Path.Combine(_cachePath, ".htm");
		}
		return Path.Combine(_cachePath, (uri.Host + uri.AbsolutePath).Replace('/', '\\'));
	}

	private HPathDocument LoadUrl(Uri uri, string method)
	{
		HPathDocument hPathDocument = new HPathDocument();
		hPathDocument.OptionAutoCloseOnEnd = false;
		hPathDocument.OptionFixNestedTags = true;
		_statusCode = Get(uri, method, null, hPathDocument);
		if (_statusCode == HttpStatusCode.NotModified)
		{
			hPathDocument.DetectEncodingAndLoad(GetCachePath(uri));
		}
		return hPathDocument;
	}

	private HttpStatusCode Get(Uri uri, string method, string path, HPathDocument doc)
	{
		string text = null;
		bool flag = false;
		HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
		httpWebRequest.Method = method;
		_fromCache = false;
		_requestDuration = 0;
		int tickCount = Environment.TickCount;
		if (UsingCache)
		{
			text = GetCachePath(httpWebRequest.RequestUri);
			if (File.Exists(text))
			{
				httpWebRequest.IfModifiedSince = File.GetLastAccessTime(text);
				flag = true;
			}
		}
		if (_cacheOnly)
		{
			if (!File.Exists(text))
			{
				throw new HtmlWebException("File was not found at cache path: '" + text + "'");
			}
			if (path != null)
			{
				IOLibrary.CopyAlways(text, path);
				File.SetLastWriteTime(path, File.GetLastWriteTime(text));
			}
			_fromCache = true;
			return HttpStatusCode.NotModified;
		}
		if (_useCookies)
		{
			httpWebRequest.CookieContainer = new CookieContainer();
		}
		if (PreRequest != null && !PreRequest(httpWebRequest))
		{
			return HttpStatusCode.ResetContent;
		}
		HttpWebResponse httpWebResponse;
		try
		{
			httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
		}
		catch (WebException ex)
		{
			_requestDuration = Environment.TickCount - tickCount;
			httpWebResponse = (HttpWebResponse)ex.Response;
			if (httpWebResponse == null)
			{
				if (flag)
				{
					if (path != null)
					{
						IOLibrary.CopyAlways(text, path);
						File.SetLastWriteTime(path, File.GetLastWriteTime(text));
					}
					return HttpStatusCode.NotModified;
				}
				throw;
			}
		}
		catch (Exception)
		{
			_requestDuration = Environment.TickCount - tickCount;
			throw;
		}
		if (PostResponse != null)
		{
			PostResponse(httpWebRequest, httpWebResponse);
		}
		_requestDuration = Environment.TickCount - tickCount;
		_responseUri = httpWebResponse.ResponseUri;
		bool flag2 = IsHtmlContent(httpWebResponse.ContentType);
		Encoding encoding = ((httpWebResponse.ContentEncoding == null || httpWebResponse.ContentEncoding.Length <= 0) ? null : Encoding.GetEncoding(httpWebResponse.ContentEncoding));
		if (httpWebResponse.StatusCode == HttpStatusCode.NotModified)
		{
			if (UsingCache)
			{
				_fromCache = true;
				if (path != null)
				{
					IOLibrary.CopyAlways(text, path);
					File.SetLastWriteTime(path, File.GetLastWriteTime(text));
				}
				return httpWebResponse.StatusCode;
			}
			throw new HtmlWebException("Server has send a NotModifed code, without cache enabled.");
		}
		Stream responseStream = httpWebResponse.GetResponseStream();
		if (responseStream != null)
		{
			if (UsingCache)
			{
				SaveStream(responseStream, text, RemoveMilliseconds(httpWebResponse.LastModified), _streamBufferSize);
				SaveCacheHeaders(httpWebRequest.RequestUri, httpWebResponse);
				if (path != null)
				{
					IOLibrary.CopyAlways(text, path);
					File.SetLastWriteTime(path, File.GetLastWriteTime(text));
				}
			}
			else if (doc != null && flag2)
			{
				if (encoding != null)
				{
					doc.Load(responseStream, encoding);
				}
				else
				{
					doc.Load(responseStream);
				}
			}
			httpWebResponse.Close();
		}
		return httpWebResponse.StatusCode;
	}

	private string GetCacheHeader(Uri requestUri, string name, string def)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(GetCacheHeadersPath(requestUri));
		XmlNode xmlNode = xmlDocument.SelectSingleNode("//h[translate(@n, 'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')='" + name.ToUpper() + "']");
		if (xmlNode == null)
		{
			return def;
		}
		return xmlNode.Attributes[name].Value;
	}

	private void SaveCacheHeaders(Uri requestUri, HttpWebResponse resp)
	{
		string cacheHeadersPath = GetCacheHeadersPath(requestUri);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<c></c>");
		XmlNode firstChild = xmlDocument.FirstChild;
		foreach (string header in resp.Headers)
		{
			XmlNode xmlNode = xmlDocument.CreateElement("h");
			XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("n");
			xmlAttribute.Value = header;
			xmlNode.Attributes.Append(xmlAttribute);
			xmlAttribute = xmlDocument.CreateAttribute("v");
			xmlAttribute.Value = resp.Headers[header];
			xmlNode.Attributes.Append(xmlAttribute);
			firstChild.AppendChild(xmlNode);
		}
		xmlDocument.Save(cacheHeadersPath);
	}

	private static long SaveStream(Stream stream, string path, DateTime touchDate, int streamBufferSize)
	{
		FilePreparePath(path);
		FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
		BinaryReader binaryReader = null;
		BinaryWriter binaryWriter = null;
		long num;
		try
		{
			binaryReader = new BinaryReader(stream);
			binaryWriter = new BinaryWriter(fileStream);
			num = 0L;
			byte[] array;
			do
			{
				array = binaryReader.ReadBytes(streamBufferSize);
				num += array.Length;
				if (array.Length > 0)
				{
					binaryWriter.Write(array);
				}
			}
			while (array.Length > 0);
		}
		finally
		{
			binaryReader?.Close();
			if (binaryWriter != null)
			{
				binaryWriter.Flush();
				binaryWriter.Close();
			}
			fileStream?.Close();
		}
		File.SetLastWriteTime(path, touchDate);
		return num;
	}

	private static void FilePreparePath(string target)
	{
		if (File.Exists(target))
		{
			FileAttributes attributes = File.GetAttributes(target);
			File.SetAttributes(target, attributes & ~FileAttributes.ReadOnly);
			return;
		}
		string directoryName = Path.GetDirectoryName(target);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
	}

	private static DateTime RemoveMilliseconds(DateTime t)
	{
		return new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, 0);
	}

	public static string GetExtensionForContentType(string contentType, string def)
	{
		if (contentType == null || contentType.Length == 0)
		{
			return def;
		}
		try
		{
			RegistryKey classesRoot = Registry.ClassesRoot;
			classesRoot = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + contentType, writable: false);
			return (string)classesRoot.GetValue("Extension", def);
		}
		catch (Exception)
		{
			return def;
		}
	}

	public static string GetContentTypeForExtension(string extension, string def)
	{
		if (extension == null || extension.Length == 0)
		{
			return def;
		}
		try
		{
			RegistryKey classesRoot = Registry.ClassesRoot;
			classesRoot = classesRoot.OpenSubKey(extension, writable: false);
			return (string)classesRoot.GetValue("", def);
		}
		catch (Exception)
		{
			return def;
		}
	}

	public void LoadHtmlAsXml(string htmlUrl, XmlTextWriter writer)
	{
		HPathDocument hPathDocument = Load(htmlUrl);
		hPathDocument.Save(writer);
	}

	public void LoadHtmlAsXml(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, XmlTextWriter writer)
	{
		LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, writer, null);
	}

	public void LoadHtmlAsXml(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, XmlTextWriter writer, string xmlPath)
	{
		if (htmlUrl == null)
		{
			throw new ArgumentNullException("htmlUrl");
		}
		HPathDocument hPathDocument = Load(htmlUrl);
		if (xmlPath != null)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(xmlPath, hPathDocument.Encoding);
			hPathDocument.Save(xmlTextWriter);
			xmlTextWriter.Close();
		}
		if (xsltArgs == null)
		{
			xsltArgs = new XsltArgumentList();
		}
		xsltArgs.AddParam("url", "", htmlUrl);
		xsltArgs.AddParam("requestDuration", "", RequestDuration);
		xsltArgs.AddParam("fromCache", "", FromCache);
		XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
		xslCompiledTransform.Load(xsltUrl);
		xslCompiledTransform.Transform(hPathDocument, xsltArgs, writer);
	}

	public object CreateInstance(string url, Type type)
	{
		return CreateInstance(url, null, null, type);
	}

	public object CreateInstance(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, Type type)
	{
		return CreateInstance(htmlUrl, xsltUrl, xsltArgs, type, null);
	}

	public object CreateInstance(string htmlUrl, string xsltUrl, XsltArgumentList xsltArgs, Type type, string xmlPath)
	{
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		if (xsltUrl == null)
		{
			LoadHtmlAsXml(htmlUrl, xmlTextWriter);
		}
		else if (xmlPath == null)
		{
			LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, xmlTextWriter);
		}
		else
		{
			LoadHtmlAsXml(htmlUrl, xsltUrl, xsltArgs, xmlTextWriter, xmlPath);
		}
		xmlTextWriter.Flush();
		StringReader input = new StringReader(stringWriter.ToString());
		XmlTextReader xmlReader = new XmlTextReader(input);
		XmlSerializer xmlSerializer = new XmlSerializer(type);
		object obj = null;
		try
		{
			return xmlSerializer.Deserialize(xmlReader);
		}
		catch (InvalidOperationException ex)
		{
			throw new Exception(ex.ToString() + ", --- xml:" + stringWriter.ToString());
		}
	}
}
