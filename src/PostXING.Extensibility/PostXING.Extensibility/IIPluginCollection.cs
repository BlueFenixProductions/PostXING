namespace PostXING.Extensibility;

public interface IIPluginCollection
{
	int Count { get; }

	bool IsSynchronized { get; }

	object SyncRoot { get; }

	void CopyTo(IPlugin[] array, int arrayIndex);

	IIPluginEnumerator GetEnumerator();
}
