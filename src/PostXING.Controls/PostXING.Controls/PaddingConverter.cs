using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace PostXING.Controls;

public class PaddingConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = ((string)value).Trim();
			if (text.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char[] separator = culture.TextInfo.ListSeparator.ToCharArray();
			string[] array = text.Split(separator);
			if (array.Length < 4)
			{
				return null;
			}
			return new Padding(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]), int.Parse(array[3]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (destinationType == typeof(string) && value is Padding)
		{
			Padding padding = (Padding)value;
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			string separator = culture.TextInfo.ListSeparator + " ";
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			return string.Join(separator, new string[4]
			{
				converter.ConvertToString(context, culture, padding.Left),
				converter.ConvertToString(context, culture, padding.Top),
				converter.ConvertToString(context, culture, padding.Right),
				converter.ConvertToString(context, culture, padding.Bottom)
			});
		}
		if (destinationType == typeof(InstanceDescriptor) && value is Padding)
		{
			Padding padding2 = (Padding)value;
			Type[] array = new Type[4];
			array[0] = (array[1] = (array[2] = (array[3] = typeof(int))));
			ConstructorInfo constructor = typeof(Padding).GetConstructor(array);
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, new object[4] { padding2.Left, padding2.Top, padding2.Right, padding2.Bottom });
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
	{
		return new Padding((int)propertyValues["Left"], (int)propertyValues["Top"], (int)propertyValues["Right"], (int)propertyValues["Bottom"]);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Padding), attributes);
		return properties.Sort(new string[4] { "Left", "Top", "Right", "Bottom" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
