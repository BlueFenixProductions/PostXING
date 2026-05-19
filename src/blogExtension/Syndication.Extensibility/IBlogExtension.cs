using System.Windows.Forms;
using System.Xml.XPath;

namespace Syndication.Extensibility;

public interface IBlogExtension
{
	string DisplayName { get; }

	bool HasConfiguration { get; }

	bool HasEditingGUI { get; }

	void Configure(IWin32Window parent);

	void BlogItem(IXPathNavigable rssFragment, bool edited);
}
