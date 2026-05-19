namespace PostXING.Components;

public interface IBlogPostCollection
{
	int Count { get; }

	bool IsSynchronized { get; }

	object SyncRoot { get; }

	void CopyTo(BlogPost[] array, int arrayIndex);

	IBlogPostEnumerator GetEnumerator();
}
