using System;
using System.Collections;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlNodeCollection : IEnumerable
{
	public class HtmlNodeEnumerator : IEnumerator
	{
		private int _index;

		private ArrayList _items;

		public HtmlNode Current => (HtmlNode)_items[_index];

		object IEnumerator.Current => Current;

		internal HtmlNodeEnumerator(ArrayList items)
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

	private ArrayList _items = new ArrayList();

	private HtmlNode _parentnode;

	public int Count => _items.Count;

	public HtmlNode this[int index] => _items[index] as HtmlNode;

	public int this[HtmlNode node]
	{
		get
		{
			int nodeIndex = GetNodeIndex(node);
			if (nodeIndex == -1)
			{
				throw new ArgumentOutOfRangeException("node", "Node \"" + node.CloneNode(deep: false).OuterHtml + "\" was not found in the collection");
			}
			return nodeIndex;
		}
	}

	internal HtmlNodeCollection(HtmlNode parentnode)
	{
		_parentnode = parentnode;
	}

	internal void Clear()
	{
		foreach (HtmlNode item in _items)
		{
			item._parentnode = null;
			item._nextnode = null;
			item._prevnode = null;
		}
		_items.Clear();
	}

	internal void Remove(int index)
	{
		HtmlNode htmlNode = null;
		HtmlNode htmlNode2 = null;
		HtmlNode htmlNode3 = (HtmlNode)_items[index];
		if (index > 0)
		{
			htmlNode2 = (HtmlNode)_items[index - 1];
		}
		if (index < _items.Count - 1)
		{
			htmlNode = (HtmlNode)_items[index + 1];
		}
		_items.RemoveAt(index);
		if (htmlNode2 != null)
		{
			if (htmlNode == htmlNode2)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			htmlNode2._nextnode = htmlNode;
		}
		if (htmlNode != null)
		{
			htmlNode._prevnode = htmlNode2;
		}
		htmlNode3._prevnode = null;
		htmlNode3._nextnode = null;
		htmlNode3._parentnode = null;
	}

	internal void Replace(int index, HtmlNode node)
	{
		HtmlNode htmlNode = null;
		HtmlNode htmlNode2 = null;
		HtmlNode htmlNode3 = (HtmlNode)_items[index];
		if (index > 0)
		{
			htmlNode2 = (HtmlNode)_items[index - 1];
		}
		if (index < _items.Count - 1)
		{
			htmlNode = (HtmlNode)_items[index + 1];
		}
		_items[index] = node;
		if (htmlNode2 != null)
		{
			if (node == htmlNode2)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			htmlNode2._nextnode = node;
		}
		if (htmlNode != null)
		{
			htmlNode._prevnode = node;
		}
		node._prevnode = htmlNode2;
		if (htmlNode == node)
		{
			throw new InvalidProgramException("Unexpected error.");
		}
		node._nextnode = htmlNode;
		node._parentnode = _parentnode;
		htmlNode3._prevnode = null;
		htmlNode3._nextnode = null;
		htmlNode3._parentnode = null;
	}

	internal void Insert(int index, HtmlNode node)
	{
		HtmlNode htmlNode = null;
		HtmlNode htmlNode2 = null;
		if (index > 0)
		{
			htmlNode2 = (HtmlNode)_items[index - 1];
		}
		if (index < _items.Count)
		{
			htmlNode = (HtmlNode)_items[index];
		}
		_items.Insert(index, node);
		if (htmlNode2 != null)
		{
			if (node == htmlNode2)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			htmlNode2._nextnode = node;
		}
		if (htmlNode != null)
		{
			htmlNode._prevnode = node;
		}
		node._prevnode = htmlNode2;
		if (htmlNode == node)
		{
			throw new InvalidProgramException("Unexpected error.");
		}
		node._nextnode = htmlNode;
		node._parentnode = _parentnode;
	}

	internal void Append(HtmlNode node)
	{
		HtmlNode htmlNode = null;
		if (_items.Count > 0)
		{
			htmlNode = (HtmlNode)_items[_items.Count - 1];
		}
		_items.Add(node);
		node._prevnode = htmlNode;
		node._nextnode = null;
		node._parentnode = _parentnode;
		if (htmlNode != null)
		{
			if (htmlNode == node)
			{
				throw new InvalidProgramException("Unexpected error.");
			}
			htmlNode._nextnode = node;
		}
	}

	internal void Prepend(HtmlNode node)
	{
		HtmlNode htmlNode = null;
		if (_items.Count > 0)
		{
			htmlNode = (HtmlNode)_items[0];
		}
		_items.Insert(0, node);
		if (node == htmlNode)
		{
			throw new InvalidProgramException("Unexpected error.");
		}
		node._nextnode = htmlNode;
		node._prevnode = null;
		node._parentnode = _parentnode;
		if (htmlNode != null)
		{
			htmlNode._prevnode = node;
		}
	}

	internal void Add(HtmlNode node)
	{
		_items.Add(node);
	}

	internal int GetNodeIndex(HtmlNode node)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (node == (HtmlNode)_items[i])
			{
				return i;
			}
		}
		return -1;
	}

	public HtmlNodeEnumerator GetEnumerator()
	{
		return new HtmlNodeEnumerator(_items);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
