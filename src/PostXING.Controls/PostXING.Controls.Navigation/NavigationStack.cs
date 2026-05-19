using System;
using System.Collections;

namespace PostXING.Controls.Navigation;

internal class NavigationStack : Stack
{
	public string[] GetPageTitles(int maximumPageTitleCount)
	{
		if (Count == 0)
		{
			return null;
		}
		int num = ((maximumPageTitleCount <= 0 || Count <= maximumPageTitleCount) ? Count : maximumPageTitleCount);
		ArrayList arrayList = new ArrayList(num);
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				NavigationContext navigationContext = (NavigationContext)enumerator.Current;
				arrayList.Add(navigationContext.PageText);
				if (arrayList.Count >= num)
				{
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		string[] array = null;
		if (arrayList.Count > 0)
		{
			array = new string[arrayList.Count];
			arrayList.CopyTo(array);
		}
		return array;
	}
}
