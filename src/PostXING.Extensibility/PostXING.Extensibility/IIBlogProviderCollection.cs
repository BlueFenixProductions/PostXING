namespace PostXING.Extensibility;

public interface IIBlogProviderCollection
{
	int Count { get; }

	bool IsSynchronized { get; }

	object SyncRoot { get; }

	void CopyTo(IBlogProvider[] array, int arrayIndex);

	IIBlogProviderEnumerator GetEnumerator();
}
