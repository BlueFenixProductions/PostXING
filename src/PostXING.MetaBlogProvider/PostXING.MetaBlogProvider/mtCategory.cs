using CookComputing.XmlRpc;

namespace PostXING.MetaBlogProvider;

public struct mtCategory
{
	public string categoryId;

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public string categoryName;

	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public bool isPrimary;
}
