namespace PostXING.Components.Legacy.v1;

public interface IBlogCollection
{
	int Count { get; }

	bool IsSynchronized { get; }

	object SyncRoot { get; }

	void CopyTo(Blog[] array, int arrayIndex);

	IBlogEnumerator GetEnumerator();
}
