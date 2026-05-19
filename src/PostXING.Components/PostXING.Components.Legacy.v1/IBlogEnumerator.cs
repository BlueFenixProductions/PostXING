namespace PostXING.Components.Legacy.v1;

public interface IBlogEnumerator
{
	Blog Current { get; }

	bool MoveNext();

	void Reset();
}
