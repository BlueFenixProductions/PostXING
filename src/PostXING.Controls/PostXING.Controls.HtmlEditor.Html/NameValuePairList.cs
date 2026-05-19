using System;
using System.Collections;

namespace PostXING.Controls.HtmlEditor.Html;

internal class NameValuePairList
{
	internal readonly string Text;

	private ArrayList _allPairs;

	private Hashtable _pairsWithName;

	internal NameValuePairList()
		: this(null)
	{
	}

	internal NameValuePairList(string text)
	{
		Text = text;
		_allPairs = new ArrayList();
		_pairsWithName = new Hashtable();
		Parse(text);
	}

	internal string GetNameValuePairValue(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException();
		}
		ArrayList nameValuePairs = GetNameValuePairs(name);
		if (nameValuePairs == null)
		{
			return null;
		}
		NameValuePair nameValuePair = nameValuePairs[0] as NameValuePair;
		return nameValuePair.Value;
	}

	internal ArrayList GetNameValuePairs(string name)
	{
		if (name == null)
		{
			return _allPairs;
		}
		return _pairsWithName[name] as ArrayList;
	}

	private void Parse(string text)
	{
		_allPairs.Clear();
		_pairsWithName.Clear();
		if (text == null)
		{
			return;
		}
		string[] array = text.Split(';');
		if (array == null)
		{
			return;
		}
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (text2.Length == 0)
			{
				continue;
			}
			string[] array3 = text2.Split(new char[1] { '=' }, 2);
			if (array3 != null)
			{
				NameValuePair nameValuePair = new NameValuePair(array3[0].Trim().ToLower());
				if (array3.Length < 2)
				{
					nameValuePair.Value = "";
				}
				else
				{
					nameValuePair.Value = array3[1];
				}
				_allPairs.Add(nameValuePair);
				ArrayList arrayList = _pairsWithName[nameValuePair.Name] as ArrayList;
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					_pairsWithName[nameValuePair.Name] = arrayList;
				}
				arrayList.Add(nameValuePair);
			}
		}
	}

	internal static string GetNameValuePairsValue(string text, string name)
	{
		NameValuePairList nameValuePairList = new NameValuePairList(text);
		return nameValuePairList.GetNameValuePairValue(name);
	}
}
