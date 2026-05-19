using System;
using System.Net;

namespace PostXING.Components;

public class ProxyFactory
{
	private ProxyFactory()
	{
	}

	private static void SetCredentials(IWebProxy proxy, string userName, string password, string domain, string uri)
	{
		CredentialCache credentialCache = new CredentialCache();
		NetworkCredential networkCredential = null;
		networkCredential = ((domain != null && domain.Trim().Length != 0) ? new NetworkCredential(userName, password, domain) : new NetworkCredential(userName, password));
		credentialCache.Add(new Uri(uri), "Negotiate", networkCredential);
		proxy.Credentials = networkCredential;
	}

	public static IWebProxy Create(string proxyName, int proxyPort, bool bypassLocal)
	{
		WebProxy webProxy = new WebProxy(proxyName, proxyPort);
		webProxy.BypassProxyOnLocal = bypassLocal;
		return webProxy;
	}

	public static IWebProxy Create(string domain, string userName, string password, string proxyName, int proxyPort, bool bypassLocal, string uri)
	{
		WebProxy webProxy = (WebProxy)Create(proxyName, proxyPort, bypassLocal);
		SetCredentials(webProxy, userName, password, domain, uri);
		return webProxy;
	}

	public static IWebProxy Create(ProxyInfo settings, string uri)
	{
		if (!settings.OverrideDefaultProxy)
		{
			return WebRequest.DefaultWebProxy;
		}
		if (!settings.RequiresLogin)
		{
			return Create(settings.ProxyName, settings.ProxyPort, settings.BypassLocal);
		}
		if (settings.ProxyDomain.Trim().Length == 0)
		{
			return Create(string.Empty, settings.ProxyUserName, settings.ProxyPassword, settings.ProxyName, settings.ProxyPort, settings.BypassLocal, uri);
		}
		return Create(settings.ProxyDomain, settings.ProxyUserName, settings.ProxyPassword, settings.ProxyName, settings.ProxyPort, settings.BypassLocal, uri);
	}
}
