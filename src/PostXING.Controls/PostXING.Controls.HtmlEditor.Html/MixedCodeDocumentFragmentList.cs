using System;
using System.Collections;

namespace PostXING.Controls.HtmlEditor.Html;

public class MixedCodeDocumentFragmentList : IEnumerable
{
	public class MixedCodeDocumentFragmentEnumerator : IEnumerator
	{
		private int _index;

		private ArrayList _items;

		public MixedCodeDocumentFragment Current => (MixedCodeDocumentFragment)_items[_index];

		object IEnumerator.Current => Current;

		internal MixedCodeDocumentFragmentEnumerator(ArrayList items)
		{
			_items = items;
			_index = -1;
		}

		public void Reset()
		{
			_index = -1;
		}

		public bool MoveNext()
		{
			_index++;
			return _index < _items.Count;
		}
	}

	private MixedCodeDocument _doc;

	private ArrayList _items = new ArrayList();

	public int Count => _items.Count;

	public MixedCodeDocumentFragment this[int index] => _items[index] as MixedCodeDocumentFragment;

	internal MixedCodeDocumentFragmentList(MixedCodeDocument doc)
	{
		_doc = doc;
	}

	public void Append(MixedCodeDocumentFragment newFragment)
	{
		if (newFragment == null)
		{
			throw new ArgumentNullException("newFragment");
		}
		_items.Add(newFragment);
	}

	public void Prepend(MixedCodeDocumentFragment newFragment)
	{
		if (newFragment == null)
		{
			throw new ArgumentNullException("newFragment");
		}
		_items.Insert(0, newFragment);
	}

	public void Remove(MixedCodeDocumentFragment fragment)
	{
		if (fragment == null)
		{
			throw new ArgumentNullException("fragment");
		}
		int fragmentIndex = GetFragmentIndex(fragment);
		if (fragmentIndex == -1)
		{
			throw new IndexOutOfRangeException();
		}
		RemoveAt(fragmentIndex);
	}

	public void RemoveAt(int index)
	{
		_ = (MixedCodeDocumentFragment)_items[index];
		_items.RemoveAt(index);
	}

	public void RemoveAll()
	{
		_items.Clear();
	}

	internal int GetFragmentIndex(MixedCodeDocumentFragment fragment)
	{
		if (fragment == null)
		{
			throw new ArgumentNullException("fragment");
		}
		for (int i = 0; i < _items.Count; i++)
		{
			if ((MixedCodeDocumentFragment)_items[i] == fragment)
			{
				return i;
			}
		}
		return -1;
	}

	internal void Clear()
	{
		_items.Clear();
	}

	public MixedCodeDocumentFragmentEnumerator GetEnumerator()
	{
		return new MixedCodeDocumentFragmentEnumerator(_items);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
