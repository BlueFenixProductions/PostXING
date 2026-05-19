using System;
using System.Drawing;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlTextFormatting
{
	private static readonly string[] formats = new string[11]
	{
		"Normal", "Formatted", "Heading 1", "Heading 2", "Heading 3", "Heading 4", "Heading 5", "Heading 6", "Paragraph", "Numbered List",
		"Bulleted List"
	};

	private HtmlControl control;

	public Color BackColor
	{
		get
		{
			return ConvertColorFromHtml(control.Execute(51));
		}
		set
		{
			control.Execute(51, new object[1] { ColorTranslator.ToHtml(value) });
		}
	}

	public bool CanIndent => control.IsEnabled(2186);

	public bool CanSetBackColor => control.IsEnabled(51);

	public bool CanSetFontName => control.IsEnabled(18);

	public bool CanSetFontSize => control.IsEnabled(19);

	public bool CanSetHtmlFormat => control.IsEnabled(2234);

	public bool CanUnindent => control.IsEnabled(2187);

	public string FontName
	{
		get
		{
			return control.Execute(18) as string;
		}
		set
		{
			control.Execute(18, new object[1] { value });
		}
	}

	public HtmlFontSize FontSize
	{
		get
		{
			object obj = control.Execute(19);
			if (obj == null)
			{
				return HtmlFontSize.Medium;
			}
			return (HtmlFontSize)obj;
		}
		set
		{
			control.Execute(19, new object[1] { (int)value });
		}
	}

	public bool CanSetForeColor => control.IsEnabled(55);

	public Color ForeColor
	{
		get
		{
			return ConvertColorFromHtml(control.Execute(55));
		}
		set
		{
			string text = ColorTranslator.ToHtml(value);
			control.Execute(55, new object[1] { text });
		}
	}

	public HtmlFormat HtmlFormat
	{
		get
		{
			if (control.Execute(2234, null) is string text)
			{
				for (int i = 0; i < formats.Length; i++)
				{
					if (text.Equals(formats[i]))
					{
						return (HtmlFormat)i;
					}
				}
			}
			return HtmlFormat.Normal;
		}
		set
		{
			control.Execute(2234, new object[1] { formats[(int)value] });
		}
	}

	public bool CanToggleBold => control.IsEnabled(52);

	public bool IsBold => control.IsChecked(52);

	public bool CanToggleItalic => control.IsEnabled(56);

	public bool IsItalic => control.IsChecked(56);

	public bool CanToggleStrikethrough => control.IsEnabled(91);

	public bool IsStrikethrough => control.IsChecked(91);

	public bool CanToggleSubscript => control.IsEnabled(2247);

	public bool IsSubscript => control.IsChecked(2247);

	public bool CanToggleSuperscript => control.IsEnabled(2248);

	public bool IsSuperscript => control.IsChecked(2248);

	public bool CanToggleUnderline => control.IsEnabled(63);

	public bool IsUnderline => control.IsChecked(63);

	public HtmlAlignment Alignment
	{
		get
		{
			if (control.IsChecked(MapAlignment(HtmlAlignment.Left)))
			{
				return HtmlAlignment.Left;
			}
			if (control.IsChecked(MapAlignment(HtmlAlignment.Right)))
			{
				return HtmlAlignment.Right;
			}
			if (control.IsChecked(MapAlignment(HtmlAlignment.Center)))
			{
				return HtmlAlignment.Center;
			}
			return HtmlAlignment.Full;
		}
		set
		{
			control.Execute(MapAlignment(value));
		}
	}

	public HtmlTextFormatting(HtmlControl control)
	{
		this.control = control;
	}

	private Color ConvertColorFromHtml(object colorValue)
	{
		if (colorValue != null)
		{
			Type type = colorValue.GetType();
			if (type == typeof(int))
			{
				return ColorTranslator.FromWin32((int)colorValue);
			}
			if (type == typeof(string))
			{
				return ColorTranslator.FromHtml((string)colorValue);
			}
		}
		return Color.Empty;
	}

	public void RemoveLink()
	{
		control.Execute(2125);
	}

	public void HyperLink()
	{
		control.ExecuteWithUserInterface(2124, null);
	}

	public void Image()
	{
		control.ExecuteWithUserInterface(2168, null);
	}

	public void ToggleOrderedList()
	{
		control.Execute(2184);
	}

	public void ToggleUnorderedList()
	{
		control.Execute(2185);
	}

	public void SetHtmlFormat(HtmlFormat format)
	{
		control.Execute(2234, new object[1] { formats[(int)format] });
	}

	public void Indent()
	{
		control.Execute(2186);
	}

	public void ToggleBold()
	{
		control.Execute(52);
	}

	public void ToggleItalics()
	{
		control.Execute(56);
	}

	public void ToggleStrikethrough()
	{
		control.Execute(91);
	}

	public void ToggleSubscript()
	{
		control.Execute(2247);
	}

	public void ToggleSuperscript()
	{
		control.Execute(2248);
	}

	public void ToggleUnderline()
	{
		control.Execute(63);
	}

	public void Unindent()
	{
		control.Execute(2187);
	}

	public bool CanAlign(HtmlAlignment alignment)
	{
		return control.IsEnabled(MapAlignment(alignment));
	}

	private int MapAlignment(HtmlAlignment alignment)
	{
		return alignment switch
		{
			HtmlAlignment.Left => 59, 
			HtmlAlignment.Right => 60, 
			HtmlAlignment.Center => 57, 
			_ => -1, 
		};
	}
}
