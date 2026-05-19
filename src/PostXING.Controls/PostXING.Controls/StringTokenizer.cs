using System.Collections;
using System.Text;

namespace PostXING.Controls;

internal class StringTokenizer
{
	private int currentIndex;

	private int numberOfTokens;

	private ArrayList tokens;

	private string source;

	private string delimiter;

	public string Source => source;

	public string Delimiter => delimiter;

	public StringTokenizer(string source, string delimiter)
	{
		tokens = new ArrayList(10);
		this.source = source;
		this.delimiter = delimiter;
		if (delimiter.Length == 0)
		{
			this.delimiter = " ";
		}
		Tokenize();
	}

	public StringTokenizer(string source, char[] delimiter)
		: this(source, new string(delimiter))
	{
	}

	public StringTokenizer(string source)
		: this(source, "")
	{
	}

	private void Tokenize()
	{
		StringBuilder stringBuilder = new StringBuilder();
		numberOfTokens = 0;
		tokens.Clear();
		currentIndex = 0;
		for (int i = 0; i < source.Length; i++)
		{
			if (delimiter.IndexOf(source[i]) != -1)
			{
				if (stringBuilder.Length > 0)
				{
					tokens.Add(stringBuilder.ToString());
					stringBuilder.Remove(0, stringBuilder.Length);
				}
			}
			else
			{
				stringBuilder.Append(source[i]);
			}
		}
		numberOfTokens = tokens.Count;
	}

	public int CountTokens()
	{
		return tokens.Count;
	}

	public bool HasMoreTokens()
	{
		if (currentIndex <= tokens.Count - 1)
		{
			return true;
		}
		return false;
	}

	public string NextToken()
	{
		string text = "";
		if (currentIndex <= tokens.Count - 1)
		{
			text = (string)tokens[currentIndex];
			currentIndex++;
			return text;
		}
		return null;
	}

	public void SkipToken()
	{
		if (currentIndex <= tokens.Count - 1)
		{
			currentIndex++;
		}
	}

	public string PeekToken()
	{
		string text = "";
		if (currentIndex <= tokens.Count - 1)
		{
			return (string)tokens[currentIndex];
		}
		return null;
	}
}
