namespace PostXING.Extensibility;

public interface IIBlogProviderEnumerator
{
	IBlogProvider Current { get; }

	bool MoveNext();

	void Reset();
}
