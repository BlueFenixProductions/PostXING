namespace PostXING.Extensibility;

public interface IIBlogProviderList : IIBlogProviderCollection
{
	bool IsFixedSize { get; }

	bool IsReadOnly { get; }

	IBlogProvider this[int index] { get; set; }

	int Add(IBlogProvider value);

	void Clear();

	bool Contains(IBlogProvider value);

	int IndexOf(IBlogProvider value);

	void Insert(int index, IBlogProvider value);

	void Remove(IBlogProvider value);

	void RemoveAt(int index);
}
