using System;
using System.Runtime.Serialization;

namespace PostXING.Extensibility;

[Serializable]
public class ExtensibilityException : ApplicationException, ISerializable
{
	public ExtensibilityException()
	{
	}

	public ExtensibilityException(string message)
		: base(message)
	{
	}

	public ExtensibilityException(string message, Exception e)
		: base(message, e)
	{
	}
}
