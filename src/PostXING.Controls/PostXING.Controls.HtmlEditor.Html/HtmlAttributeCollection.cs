using System;
using System.Collections;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlAttributeCollection : IEnumerable
{
	public class HtmlAttributeEnumerator : IEnumerator
	{
		private int _index;

		private ArrayList _items;

		public HtmlAttribute Current => (HtmlAttribute)_items[_index];

		object IEnumerator.Current => Current;

		internal HtmlAttributeEnumerator(ArrayList items)
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

	internal Hashtable _hashitems = new Hashtable();

	private ArrayList _items = new ArrayList();

	private HtmlNode _ownernode;

	public int Count => _items.Count;

	public HtmlAttribute this[string name]
	{
		get
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return _hashitems[name.ToLower()] as HtmlAttribute;
		}
	}

	public HtmlAttribute this[int index] => _items[index] as HtmlAttribute;

	internal HtmlAttributeCollection(HtmlNode ownernode)
	{
		_ownernode = ownernode;
	}

	public HtmlAttribute Append(HtmlAttribute newAttribute)
	{
		if (newAttribute == null)
		{
			throw new ArgumentNullException("newAttribute");
		}
		_hashitems[newAttribute.Name] = newAttribute;
		newAttribute._ownernode = _ownernode;
		_items.Add(newAttribute);
		_ownernode._innerchanged = true;
		_ownernode._outerchanged = true;
		return newAttribute;
	}

	public HtmlAttribute Append(string name)
	{
		HtmlAttribute newAttribute = _ownernode._ownerdocument.CreateAttribute(name);
		return Append(newAttribute);
	}

	public HtmlAttribute Append(string name, string value)
	{
		HtmlAttribute newAttribute = _ownernode._ownerdocument.CreateAttribute(name, value);
		return Append(newAttribute);
	}

	public HtmlAttribute Prepend(HtmlAttribute newAttribute)
	{
		if (newAttribute == null)
		{
			throw new ArgumentNullException("newAttribute");
		}
		_hashitems[newAttribute.Name] = newAttribute;
		newAttribute._ownernode = _ownernode;
		_items.Insert(0, newAttribute);
		_ownernode._innerchanged = true;
		_ownernode._outerchanged = true;
		return newAttribute;
	}

	public void RemoveAt(int index)
	{
		HtmlAttribute htmlAttribute = (HtmlAttribute)_items[index];
		_hashitems.Remove(htmlAttribute.Name);
		_items.RemoveAt(index);
		_ownernode._innerchanged = true;
		_ownernode._outerchanged = true;
	}

	public void Remove(HtmlAttribute attribute)
	{
		if (attribute == null)
		{
			throw new ArgumentNullException("attribute");
		}
		int attributeIndex = GetAttributeIndex(attribute);
		if (attributeIndex == -1)
		{
			throw new IndexOutOfRangeException();
		}
		RemoveAt(attributeIndex);
	}

	public void Remove(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		string text = name.ToLower();
		for (int i = 0; i < _items.Count; i++)
		{
			HtmlAttribute htmlAttribute = (HtmlAttribute)_items[i];
			if (htmlAttribute.Name == text)
			{
				RemoveAt(i);
			}
		}
	}

	public void RemoveAll()
	{
		_hashitems.Clear();
		_items.Clear();
		_ownernode._innerchanged = true;
		_ownernode._outerchanged = true;
	}

	internal int GetAttributeIndex(HtmlAttribute attribute)
	{
		if (attribute == null)
		{
			throw new ArgumentNullException("attribute");
		}
		for (int i = 0; i < _items.Count; i++)
		{
			if ((HtmlAttribute)_items[i] == attribute)
			{
				return i;
			}
		}
		return -1;
	}

	internal int GetAttributeIndex(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		string text = name.ToLower();
		for (int i = 0; i < _items.Count; i++)
		{
			if (((HtmlAttribute)_items[i]).Name == text)
			{
				return i;
			}
		}
		return -1;
	}

	internal void Clear()
	{
		_hashitems.Clear();
		_items.Clear();
	}

	public HtmlAttributeEnumerator GetEnumerator()
	{
		return new HtmlAttributeEnumerator(_items);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
