using System;
using System.ComponentModel;
using System.Globalization;

namespace PostXING.Controls.Controls;

public class ItemCollectionTypeConverter : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		return "";
	}
}
