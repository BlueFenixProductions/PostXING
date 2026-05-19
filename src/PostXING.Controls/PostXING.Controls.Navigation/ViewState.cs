using System.Collections;

namespace PostXING.Controls.Navigation;

public class ViewState
{
	private Hashtable state = new Hashtable();

	public object ReadProperty(string propertyName)
	{
		return state[propertyName];
	}

	public void WriteProperty(string propertyName, object propertyValue)
	{
		state[propertyName] = propertyValue;
	}
}
