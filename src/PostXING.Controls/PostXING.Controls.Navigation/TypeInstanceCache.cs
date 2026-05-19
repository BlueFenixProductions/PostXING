using System;
using System.Collections;

namespace PostXING.Controls.Navigation;

internal class TypeInstanceCache
{
	private Hashtable cache = new Hashtable();

	public object GetInstanceOfType(Type type)
	{
		object obj = cache[type];
		if (obj == null)
		{
			obj = Activator.CreateInstance(type);
			cache[type] = obj;
		}
		return obj;
	}
}
