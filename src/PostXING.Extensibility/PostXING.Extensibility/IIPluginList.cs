namespace PostXING.Extensibility;

public interface IIPluginList : IIPluginCollection
{
	bool IsFixedSize { get; }

	bool IsReadOnly { get; }

	IPlugin this[int index] { get; set; }

	int Add(IPlugin value);

	void Clear();

	bool Contains(IPlugin value);

	int IndexOf(IPlugin value);

	void Insert(int index, IPlugin value);

	void Remove(IPlugin value);

	void RemoveAt(int index);
}
