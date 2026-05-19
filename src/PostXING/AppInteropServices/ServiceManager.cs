#define TRACE
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using PostXING.Extensibility;
using Syndication.Extensibility;

namespace AppInteropServices;

[Serializable]
[ReflectionPermission(SecurityAction.Demand, MemberAccess = true, Unrestricted = true)]
public class ServiceManager
{
	public static ArrayList SearchForIPlugins(string path)
	{
		return SearchForPlugins(path, PluginType.IPlugin);
	}

	public static ArrayList SearchForIBlogExtensions(string path)
	{
		return SearchForPlugins(path, PluginType.IBlogExtension);
	}

	public static ArrayList SearchForIBlogProviders(string path)
	{
		return SearchForPlugins(path, PluginType.IBlogProvider);
	}

	private static ArrayList SearchForPlugins(string path, PluginType whichPlugin)
	{
		AppDomain appDomain = AppDomain.CreateDomain("loaderDomain");
		ServiceManager serviceManager = (ServiceManager)appDomain.CreateInstanceAndUnwrap(Assembly.GetAssembly(typeof(ServiceManager)).FullName, "AppInteropServices.ServiceManager");
		ArrayList arrayList = new ArrayList();
		switch (whichPlugin)
		{
		case PluginType.IPlugin:
			arrayList = serviceManager.SearchForIPluginTypes(path);
			break;
		case PluginType.IBlogExtension:
			arrayList = serviceManager.SearchForIBlogExtensionTypes(path);
			break;
		case PluginType.IBlogProvider:
			arrayList = serviceManager.SearchForIBlogProviderTypes(path);
			break;
		}
		AppDomain.Unload(appDomain);
		ArrayList arrayList2 = new ArrayList();
		foreach (Type item in arrayList)
		{
			try
			{
				switch (whichPlugin)
				{
				case PluginType.IPlugin:
				{
					IPlugin value3 = (IPlugin)Activator.CreateInstance(item);
					arrayList2.Add(value3);
					break;
				}
				case PluginType.IBlogExtension:
				{
					IBlogExtension value2 = (IBlogExtension)Activator.CreateInstance(item);
					arrayList2.Add(value2);
					break;
				}
				case PluginType.IBlogProvider:
				{
					IBlogProvider value = (IBlogProvider)Activator.CreateInstance(item);
					arrayList2.Add(value);
					break;
				}
				}
			}
			catch (Exception)
			{
			}
		}
		return arrayList2;
	}

	public ArrayList SearchForIPluginTypes(string path)
	{
		return SearchForPluginTypes(path, PluginType.IPlugin);
	}

	public ArrayList SearchForIBlogExtensionTypes(string path)
	{
		return SearchForPluginTypes(path, PluginType.IBlogExtension);
	}

	public ArrayList SearchForIBlogProviderTypes(string path)
	{
		return SearchForPluginTypes(path, PluginType.IBlogProvider);
	}

	private ArrayList SearchForPluginTypes(string path, PluginType whichPlugin)
	{
		Type typeFromHandle = typeof(object);
		switch (whichPlugin)
		{
		case PluginType.IPlugin:
			typeFromHandle = typeof(IPlugin);
			break;
		case PluginType.IBlogExtension:
			typeFromHandle = typeof(IBlogExtension);
			break;
		case PluginType.IBlogProvider:
			typeFromHandle = typeof(IBlogProvider);
			break;
		}
		ArrayList arrayList = new ArrayList();
		IPermission permission = new ReflectionPermission(ReflectionPermissionFlag.MemberAccess);
		try
		{
			permission.Demand();
		}
		catch (SecurityException ex)
		{
			Trace.WriteLine(ex.Message);
			return arrayList;
		}
		if (path == null || !Directory.Exists(path))
		{
			return arrayList;
		}
		string[] files = Directory.GetFiles(path, "*.exe");
		string[] files2 = Directory.GetFiles(path, "*.dll");
		string[] array = new string[files.Length + files2.Length];
		files.CopyTo(array, 0);
		files2.CopyTo(array, files.Length);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text == null || text.Length == 0)
			{
				continue;
			}
			try
			{
				Assembly assembly = Assembly.LoadFrom(text);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (typeFromHandle.IsAssignableFrom(type))
					{
						arrayList.Add(type);
					}
				}
			}
			catch (ReflectionTypeLoadException ex2)
			{
				Trace.WriteLine(ex2.Message);
			}
			catch (Exception ex3)
			{
				Trace.WriteLine(ex3.Message);
			}
		}
		return arrayList;
	}
}
