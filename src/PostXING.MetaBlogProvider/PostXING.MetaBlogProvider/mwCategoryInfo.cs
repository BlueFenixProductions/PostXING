using CookComputing.XmlRpc;

namespace PostXING.MetaBlogProvider;

public struct mwCategoryInfo
{
	public string description;

	public string htmlUrl;

	public string rssUrl;

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public string title;

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public string categoryid;
}
