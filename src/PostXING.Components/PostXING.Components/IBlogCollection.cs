namespace PostXING.Components;

public interface IBlogCollection
{
	int Count { get; }

	bool IsSynchronized { get; }

	object SyncRoot { get; }

	void CopyTo(Blog[] array, int arrayIndex);

	IBlogEnumerator GetEnumerator();
}
