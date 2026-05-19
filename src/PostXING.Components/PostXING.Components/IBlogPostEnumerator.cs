namespace PostXING.Components;

public interface IBlogPostEnumerator
{
	BlogPost Current { get; }

	bool MoveNext();

	void Reset();
}
