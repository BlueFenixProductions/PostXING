namespace PostXING.Components.Legacy.v1;

public interface IBlogList : IBlogCollection
{
	bool IsFixedSize { get; }

	bool IsReadOnly { get; }

	Blog this[int index] { get; set; }

	int Add(Blog value);

	void Clear();

	bool Contains(Blog value);

	int IndexOf(Blog value);

	void Insert(int index, Blog value);

	void Remove(Blog value);

	void RemoveAt(int index);
}
