using System.Collections;

namespace PostXING.Controls.Navigation;

internal class ReferenceCountTable
{
	private Hashtable countTable = new Hashtable();

	public int this[object o]
	{
		get
		{
			if (!countTable.ContainsKey(o))
			{
				return 0;
			}
			return (int)countTable[o];
		}
	}

	public void AddReference(object o)
	{
		countTable[o] = this[o] + 1;
	}

	public int RemoveReference(object o)
	{
		int num = this[o] - 1;
		if (num > 0)
		{
			countTable[o] = num;
		}
		else
		{
			countTable.Remove(o);
		}
		return num;
	}
}
