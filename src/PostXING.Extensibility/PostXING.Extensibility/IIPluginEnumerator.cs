namespace PostXING.Extensibility;

public interface IIPluginEnumerator
{
	IPlugin Current { get; }

	bool MoveNext();

	void Reset();
}
