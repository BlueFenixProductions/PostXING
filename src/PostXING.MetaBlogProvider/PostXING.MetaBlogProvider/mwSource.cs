using CookComputing.XmlRpc;

namespace PostXING.MetaBlogProvider;

[XmlRpcMissingMapping(MappingAction.Ignore)]
public struct mwSource
{
	public string name;

	public string url;
}
