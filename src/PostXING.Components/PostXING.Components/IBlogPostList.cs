namespace PostXING.Components;

public interface IBlogPostList : IBlogPostCollection
{
	bool IsFixedSize { get; }

	bool IsReadOnly { get; }

	BlogPost this[int index] { get; set; }

	int Add(BlogPost value);

	void Clear();

	bool Contains(BlogPost value);

	int IndexOf(BlogPost value);

	void Insert(int index, BlogPost value);

	void Remove(BlogPost value);

	void RemoveAt(int index);
}
