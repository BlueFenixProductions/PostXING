using CookComputing.XmlRpc;

namespace PostXING.MetaBlogProvider;

public struct mtTextFilter
{
	[XmlRpcMember(Description = "unique string identifying a text formatting plugin")]
	public string key;

	[XmlRpcMember(Description = "readable description to be displayed to a user")]
	public string value;
}
