namespace PostXING.Components;

public interface IBlogEnumerator
{
	Blog Current { get; }

	bool MoveNext();

	void Reset();
}
