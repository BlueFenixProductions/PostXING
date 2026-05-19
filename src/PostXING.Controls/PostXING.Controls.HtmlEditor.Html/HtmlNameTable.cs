using System.Xml;

namespace PostXING.Controls.HtmlEditor.Html;

internal class HtmlNameTable : XmlNameTable
{
	private NameTable _nametable = new NameTable();

	internal HtmlNameTable()
	{
	}

	internal string GetOrAdd(string array)
	{
		string text = Get(array);
		if (text == null)
		{
			return Add(array);
		}
		return text;
	}

	public override string Add(string array)
	{
		return _nametable.Add(array);
	}

	public override string Get(string array)
	{
		return _nametable.Get(array);
	}

	public override string Get(char[] array, int offset, int length)
	{
		return _nametable.Get(array, offset, length);
	}

	public override string Add(char[] array, int offset, int length)
	{
		return _nametable.Add(array, offset, length);
	}
}
