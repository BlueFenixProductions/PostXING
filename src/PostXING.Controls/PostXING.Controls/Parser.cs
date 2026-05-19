using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;

namespace PostXING.Controls;

internal class Parser
{
	private const int UNKNOWN = 0;

	private const int MAINSECTIONSS = 1;

	private const int MAINSECTIONTASKSS = 2;

	private const int SECTIONSS = 3;

	private const int SECTIONTASKSS = 4;

	private const int TASKPANE = 5;

	private const int BUTTON = 1;

	private const int DESTINATIONTASK = 2;

	private const int ACTIONTASK = 4;

	private const int TITLE = 8;

	private const int ARROW = 16;

	private const int WATERMARK = 32;

	private const int TASKLIST = 64;

	private const int SECTIONLIST = 128;

	private const int SELECTED = 256;

	private const int MOUSEFOCUSED = 512;

	private const int KEYFOCUSED = 1024;

	private const int EXPANDO = 2048;

	private const int BACKDROP = 4096;

	private const int HEADER = 8192;

	private const int CONTENT = 1;

	private const int CONTENTALIGN = 2;

	private const int FONTFACE = 3;

	private const int FONTSIZE = 4;

	private const int FONTWEIGHT = 5;

	private const int FONTSTYLE = 6;

	private const int BACKGROUND = 7;

	private const int FOREGROUND = 8;

	private const int BORDERTHICKNESS = 9;

	private const int BORDERDOLOR = 10;

	private const int PADDING = 11;

	private const int MARGIN = 12;

	private int style;

	private int section;

	private int property;

	private ExplorerBarInfo info;

	private StringTokenizer tokenizer;

	public Parser(string uifile)
	{
		uifile = uifile.Replace("rp", " ");
		uifile = uifile.Replace("rcstr", " ");
		uifile = uifile.Replace("rcint", " ");
		uifile = uifile.Replace("pt", " ");
		uifile = uifile.Replace("rect", " ");
		tokenizer = new StringTokenizer(uifile, " \t\n\r\f<>=()[]{}:;,\\");
		style = 0;
		section = 0;
		property = 0;
	}

	public ExplorerBarInfo Parse()
	{
		info = new ExplorerBarInfo();
		string text = null;
		while (tokenizer.HasMoreTokens())
		{
			text = tokenizer.NextToken();
			if (text.Equals("style"))
			{
				style = GetStyle(text);
			}
			else if (text.Equals("/style"))
			{
				style = 0;
			}
			else if (style != 0 && IsSection(text))
			{
				section = GetSection(text);
			}
			else if (style != 0 && section != 0 && IsProperty(text))
			{
				property = GetPropertyType(text);
				ExtractProperty();
			}
		}
		return info;
	}

	private int GetStyle(string s)
	{
		if (!tokenizer.PeekToken().Equals("resid"))
		{
			return 0;
		}
		tokenizer.SkipToken();
		return tokenizer.NextToken() switch
		{
			"mainsectionss" => 1, 
			"mainsectiontaskss" => 2, 
			"sectionss" => 3, 
			"sectiontaskss" => 4, 
			"taskpane" => 5, 
			_ => 0, 
		};
	}

	private bool IsSection(string s)
	{
		if (!s.Equals("button") && !s.Equals("destinationtask") && !s.Equals("actiontask") && !s.Equals("title") && !s.Equals("arrow") && !s.Equals("watermark") && !s.Equals("tasklist") && !s.Equals("sectionlist") && !s.Equals("backdrop") && !s.Equals("expando"))
		{
			return s.Equals("header");
		}
		return true;
	}

	private int GetSection(string s)
	{
		switch (s)
		{
		case "button":
			if (tokenizer.PeekToken().Equals("keyfocused"))
			{
				tokenizer.SkipToken();
				return 1025;
			}
			return 1;
		case "destinationtask":
			return 2;
		case "actiontask":
			return 4;
		case "title":
			if (tokenizer.PeekToken().Equals("mousefocused"))
			{
				tokenizer.SkipToken();
				return 520;
			}
			return 8;
		case "arrow":
			if (tokenizer.PeekToken().Equals("selected"))
			{
				tokenizer.SkipToken();
				if (tokenizer.PeekToken().Equals("mousefocused"))
				{
					tokenizer.SkipToken();
					return 784;
				}
				return 272;
			}
			if (tokenizer.PeekToken().Equals("mousefocused"))
			{
				tokenizer.SkipToken();
				return 528;
			}
			return 16;
		case "watermark":
			return 32;
		case "tasklist":
			return 64;
		case "sectionlist":
			return 128;
		case "expando":
			return 2048;
		case "backdrop":
			return 4096;
		case "header":
			return 8192;
		default:
			return 0;
		}
	}

	private bool IsProperty(string s)
	{
		if (!s.Equals("content") && !s.Equals("contentalign") && !s.Equals("fontface") && !s.Equals("fontsize") && !s.Equals("fontweight") && !s.Equals("fontstyle") && !s.Equals("background") && !s.Equals("foreground") && !s.Equals("borderthickness") && !s.Equals("bordercolor") && !s.Equals("padding") && !s.Equals("margin"))
		{
			return s.Equals("cursor");
		}
		return true;
	}

	private int GetPropertyType(string s)
	{
		return s switch
		{
			"content" => 1, 
			"contentalign" => 2, 
			"fontface" => 3, 
			"fontsize" => 4, 
			"fontweight" => 5, 
			"fontstyle" => 6, 
			"background" => 7, 
			"foreground" => 8, 
			"borderthickness" => 9, 
			"bordercolor" => 10, 
			"padding" => 11, 
			"margin" => 12, 
			_ => 0, 
		};
	}

	private void ExtractProperty()
	{
		switch (property)
		{
		case 1:
			ExtractContent();
			break;
		case 2:
			ExtractContentAlignment();
			break;
		case 3:
			ExtractFontFace();
			break;
		case 4:
			ExtractFontSize();
			break;
		case 5:
			ExtractFontWeight();
			break;
		case 6:
			ExtractFontStyle();
			break;
		case 7:
			ExtractBackground();
			break;
		case 8:
			ExtractForeground();
			break;
		case 9:
			ExtractBorder();
			break;
		case 10:
			ExtractBorderColor();
			break;
		case 11:
			ExtractPadding();
			break;
		case 12:
			ExtractMargin();
			break;
		}
	}

	private void ExtractContent()
	{
		string text = tokenizer.PeekToken();
		if (text.Equals("rcbmp"))
		{
			tokenizer.SkipToken();
			ExtractBitmap();
		}
	}

	private void ExtractContentAlignment()
	{
		string text = tokenizer.NextToken();
		ContentAlignment contentAlignment = GetContentAlignment(text);
		text.IndexOf("wrap");
		if (style == 1)
		{
			if (section == 8)
			{
				info.Header.SpecialAlignment = contentAlignment;
			}
			else if (section == 32)
			{
				info.Expando.WatermarkAlignment = contentAlignment;
			}
		}
		else if (style == 3)
		{
			if (section == 8)
			{
				info.Header.NormalAlignment = contentAlignment;
			}
			else if (section == 32)
			{
				info.Expando.WatermarkAlignment = contentAlignment;
			}
		}
		else if (style == 5 && section == 4096)
		{
			info.TaskPane.WatermarkAlignment = contentAlignment;
		}
	}

	private ContentAlignment GetContentAlignment(string s)
	{
		if (s.IndexOf("top") != -1)
		{
			if (s.IndexOf("left") != -1)
			{
				return ContentAlignment.TopLeft;
			}
			if (s.IndexOf("center") != -1)
			{
				return ContentAlignment.TopCenter;
			}
			if (s.IndexOf("right") != -1)
			{
				return ContentAlignment.TopRight;
			}
			return ContentAlignment.TopLeft;
		}
		if (s.IndexOf("middle") != -1)
		{
			if (s.IndexOf("left") != -1)
			{
				return ContentAlignment.MiddleLeft;
			}
			if (s.IndexOf("center") != -1)
			{
				return ContentAlignment.MiddleCenter;
			}
			if (s.IndexOf("right") != -1)
			{
				return ContentAlignment.MiddleRight;
			}
			return ContentAlignment.MiddleLeft;
		}
		if (s.IndexOf("bottom") != -1)
		{
			if (s.IndexOf("left") != -1)
			{
				return ContentAlignment.BottomLeft;
			}
			if (s.IndexOf("center") != -1)
			{
				return ContentAlignment.BottomCenter;
			}
			if (s.IndexOf("right") != -1)
			{
				return ContentAlignment.BottomRight;
			}
			return ContentAlignment.BottomLeft;
		}
		if (s.Equals("wrapleft"))
		{
			ContentAlignment contentAlignment = ContentAlignment.MiddleLeft;
		}
		if (s.Equals("wrapcenter"))
		{
			ContentAlignment contentAlignment = ContentAlignment.MiddleRight;
		}
		if (s.Equals("wrapright"))
		{
			return ContentAlignment.MiddleRight;
		}
		return ContentAlignment.MiddleLeft;
	}

	private void ExtractFontFace()
	{
		int id = int.Parse(tokenizer.NextToken());
		string resourceString = ThemeManager.GetResourceString(id);
		if ((style == 1 || style == 3) && section == 2048 && resourceString != null && resourceString.Length > 0)
		{
			info.Header.FontName = resourceString;
		}
	}

	private void ExtractFontSize()
	{
		int id = int.Parse(tokenizer.NextToken());
		string resourceString = ThemeManager.GetResourceString(id);
		if ((style == 1 || style == 3) && section == 2048 && resourceString != null && resourceString.Length > 0)
		{
			info.Header.FontSize = float.Parse(resourceString);
		}
	}

	private void ExtractFontWeight()
	{
		int num = 400;
		int id = int.Parse(tokenizer.NextToken());
		string resourceString = ThemeManager.GetResourceString(id);
		if (resourceString != null && resourceString.Length > 0)
		{
			num = int.Parse(resourceString);
		}
		FontStyle fontWeight = ((num == 700) ? FontStyle.Bold : FontStyle.Regular);
		if (style == 1)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.FontWeight = fontWeight;
			}
		}
		else if (style == 3 && (section == 1 || section == 8192))
		{
			info.Header.FontWeight = fontWeight;
		}
	}

	private void ExtractFontStyle()
	{
		string text = tokenizer.NextToken();
		FontStyle fontStyle = (text.Equals("underline") ? FontStyle.Underline : (text.Equals("italic") ? FontStyle.Italic : (text.Equals("strikeout") ? FontStyle.Strikeout : FontStyle.Regular)));
		if (style == 1 || style == 3)
		{
			if (section == 8)
			{
				info.Header.FontStyle = fontStyle;
			}
		}
		else if (style == 2)
		{
			if (section - 512 == 1)
			{
				info.TaskLink.FontDecoration = fontStyle;
			}
		}
		else if (style == 4 && section - 512 == 1)
		{
			info.TaskLink.FontDecoration = fontStyle;
		}
	}

	private void ExtractBackground()
	{
		string text = tokenizer.PeekToken();
		if (text.Equals("rcbmp"))
		{
			tokenizer.SkipToken();
			ExtractBitmap();
			return;
		}
		if (text.Equals("gradient"))
		{
			tokenizer.SkipToken();
			info.TaskPane.GradientStartColor = ExtractColor();
			info.TaskPane.GradientEndColor = ExtractColor();
			info.TaskPane.GradientDirection = (LinearGradientMode)int.Parse(tokenizer.NextToken());
			return;
		}
		Color color = ExtractColor();
		if (color.A == 0 && color.R == 0 && color.G == 0 && color.B == 0)
		{
			return;
		}
		if (style == 1)
		{
			if (section == 32 || section == 64)
			{
				info.Expando.SpecialBackColor = color;
			}
			else if (section == 2048)
			{
				info.Expando.SpecialBackColor = color;
				info.Header.SpecialBackColor = color;
			}
			else if (section == 8192)
			{
				info.Header.SpecialBackColor = color;
			}
		}
		else if (style == 3)
		{
			if (section == 64)
			{
				info.Expando.NormalBackColor = color;
			}
			else if (section == 2048)
			{
				info.Expando.NormalBackColor = color;
				info.Header.NormalBackColor = color;
			}
			else if (section == 8192)
			{
				info.Header.NormalBackColor = color;
			}
		}
		else if (style == 5 && (section == 4096 || section == 128))
		{
			info.TaskPane.GradientStartColor = color;
			info.TaskPane.GradientEndColor = color;
			info.TaskPane.GradientDirection = LinearGradientMode.Vertical;
		}
	}

	private void ExtractBitmap()
	{
		string resourceName = tokenizer.NextToken();
		ImageStretchMode imageStretchMode = (ImageStretchMode)int.Parse(tokenizer.NextToken());
		string text = tokenizer.NextToken();
		Bitmap bitmap = null;
		if (imageStretchMode == ImageStretchMode.Transparent || imageStretchMode == ImageStretchMode.ARGBImage)
		{
			bitmap = ThemeManager.GetResourcePNG(resourceName);
		}
		else
		{
			bitmap = ThemeManager.GetResourceBMP(resourceName);
			if (text.StartsWith("#"))
			{
				byte[] bytes = GetBytes(text);
				bitmap.MakeTransparent(Color.FromArgb(bytes[0], bytes[1], bytes[2]));
			}
		}
		if (style == 1)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.SpecialBackImage = bitmap;
			}
			else if (section == 16)
			{
				info.Header.SpecialArrowDown = bitmap;
			}
			else if (section - 256 - 512 == 16)
			{
				info.Header.SpecialArrowUpHot = bitmap;
			}
			else if (section - 256 == 16)
			{
				info.Header.SpecialArrowUp = bitmap;
			}
			else if (section - 512 == 16)
			{
				info.Header.SpecialArrowDownHot = bitmap;
			}
		}
		else if (style == 3)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.NormalBackImage = bitmap;
			}
			else if (section == 16)
			{
				info.Header.NormalArrowDown = bitmap;
			}
			else if (section - 256 - 512 == 16)
			{
				info.Header.NormalArrowUpHot = bitmap;
			}
			else if (section - 256 == 16)
			{
				info.Header.NormalArrowUp = bitmap;
			}
			else if (section - 512 == 16)
			{
				info.Header.NormalArrowDownHot = bitmap;
			}
		}
		else if (style == 5)
		{
			if (section == 128)
			{
				info.TaskPane.BackImage = bitmap;
				info.TaskPane.StretchMode = imageStretchMode;
			}
			else if (section == 4096)
			{
				info.TaskPane.Watermark = bitmap;
			}
		}
	}

	private void ExtractForeground()
	{
		Color color = ExtractColor();
		if (style == 1)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.SpecialTitleColor = color;
			}
			else if (section == 8)
			{
				info.Header.SpecialTitleColor = color;
			}
			else if (section - 512 == 8)
			{
				info.Header.SpecialTitleHotColor = color;
			}
			else if (section - 1024 == 8)
			{
				info.Header.SpecialTitleHotColor = color;
			}
		}
		else if (style == 2)
		{
			if (section == 1)
			{
				info.TaskLink.LinkColor = color;
			}
			else if (section == 8)
			{
				info.TaskLink.LinkColor = color;
			}
			else if (section - 512 == 8)
			{
				info.TaskLink.HotLinkColor = color;
			}
		}
		else if (style == 3)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.NormalTitleColor = color;
			}
			else if (section == 8)
			{
				info.Header.NormalTitleColor = color;
			}
			else if (section - 512 == 8)
			{
				info.Header.NormalTitleHotColor = color;
			}
			else if (section - 1024 == 8)
			{
				info.Header.NormalTitleHotColor = color;
			}
		}
		else if (style == 4)
		{
			if (section == 1)
			{
				info.TaskLink.LinkColor = color;
			}
			else if (section == 8)
			{
				info.TaskLink.LinkColor = color;
			}
			else if (section - 512 == 8)
			{
				info.TaskLink.HotLinkColor = color;
			}
		}
	}

	private void ExtractPadding()
	{
		Padding padding = new Padding();
		padding.Left = int.Parse(tokenizer.NextToken());
		padding.Top = int.Parse(tokenizer.NextToken());
		padding.Right = int.Parse(tokenizer.NextToken());
		padding.Bottom = int.Parse(tokenizer.NextToken());
		if (style == 1)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.SpecialPadding = padding;
			}
			else if (section == 64)
			{
				info.Expando.SpecialPadding = padding;
			}
		}
		else if (style == 2)
		{
			if (section == 8)
			{
				info.TaskLink.Padding = padding;
			}
		}
		else if (style == 3)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.NormalPadding = padding;
			}
			else if (section == 64)
			{
				info.Expando.NormalPadding = padding;
			}
		}
		else if (style == 4)
		{
			if (section == 8)
			{
				info.TaskLink.Padding = padding;
			}
		}
		else if (style == 5 && section == 128)
		{
			info.TaskPane.Padding = padding;
		}
	}

	private void ExtractMargin()
	{
		Margin margin = new Margin();
		margin.Left = int.Parse(tokenizer.NextToken());
		margin.Top = int.Parse(tokenizer.NextToken());
		margin.Bottom = int.Parse(tokenizer.NextToken());
		margin.Right = int.Parse(tokenizer.NextToken());
		if (style == 2)
		{
			if (section == 2)
			{
				info.TaskLink.Margin = margin;
			}
			else if (section == 4)
			{
				info.TaskLink.Margin = margin;
			}
		}
		else if (style == 4)
		{
			if (section == 2)
			{
				info.TaskLink.Margin = margin;
			}
			else if (section == 4)
			{
				info.TaskLink.Margin = margin;
			}
		}
	}

	private void ExtractBorder()
	{
		Border border = new Border();
		border.Left = int.Parse(tokenizer.NextToken());
		border.Top = int.Parse(tokenizer.NextToken());
		border.Right = int.Parse(tokenizer.NextToken());
		border.Bottom = int.Parse(tokenizer.NextToken());
		if (style == 1)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.SpecialBorder = border;
			}
			else if (section == 64)
			{
				info.Expando.SpecialBorder = border;
			}
		}
		else if (style == 3)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.NormalBorder = border;
			}
			else if (section == 64)
			{
				info.Expando.NormalBorder = border;
			}
		}
		else if (style == 5 && section == 128)
		{
			info.TaskPane.Border = border;
		}
	}

	private void ExtractBorderColor()
	{
		Color color = ExtractColor();
		if (style == 1)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.SpecialBorderColor = color;
			}
			else if (section == 64)
			{
				info.Expando.SpecialBorderColor = color;
			}
		}
		else if (style == 3)
		{
			if (section == 1 || section == 8192)
			{
				info.Header.NormalBorderColor = color;
			}
			else if (section == 64)
			{
				info.Expando.NormalBorderColor = color;
			}
		}
		else if (style == 5 && section == 128)
		{
			info.TaskPane.BorderColor = color;
		}
	}

	private Color ExtractColor()
	{
		string text = tokenizer.PeekToken();
		Color transparent = Color.Transparent;
		if (text.Equals("rgb"))
		{
			return ExtractRGBColor();
		}
		if (text.Equals("argb"))
		{
			return ExtractARGBColor();
		}
		if (text.StartsWith("#"))
		{
			return ExtractHexColor(text);
		}
		return Color.FromName(text);
	}

	private Color ExtractRGBColor()
	{
		if (tokenizer.PeekToken().Equals("rgb"))
		{
			tokenizer.SkipToken();
		}
		return Color.FromArgb(int.Parse(tokenizer.NextToken()), int.Parse(tokenizer.NextToken()), int.Parse(tokenizer.NextToken()));
	}

	private Color ExtractARGBColor()
	{
		if (tokenizer.PeekToken().Equals("argb"))
		{
			tokenizer.SkipToken();
		}
		Color result = Color.FromArgb(int.Parse(tokenizer.NextToken()), int.Parse(tokenizer.NextToken()), int.Parse(tokenizer.NextToken()), int.Parse(tokenizer.NextToken()));
		if (result.A == 0 && result.R == 0 && result.G == 0 && result.B == 0)
		{
			return result;
		}
		return Color.FromArgb(255 - result.A, result.R, result.G, result.B);
	}

	private Color ExtractHexColor(string s)
	{
		byte[] bytes = GetBytes(s.Substring(1));
		return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
	}

	public byte[] GetBytes(string hexString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in hexString)
		{
			if (IsHexDigit(c))
			{
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length % 2 != 0)
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		int num = stringBuilder.Length / 2;
		byte[] array = new byte[num];
		int num2 = 0;
		for (int j = 0; j < array.Length; j++)
		{
			string hex = new string(new char[2]
			{
				stringBuilder[num2],
				stringBuilder[num2 + 1]
			});
			array[j] = HexToByte(hex);
			num2 += 2;
		}
		return array;
	}

	private bool IsHexDigit(char c)
	{
		int num = Convert.ToInt32('A');
		int num2 = Convert.ToInt32('0');
		c = char.ToUpper(c);
		int num3 = Convert.ToInt32(c);
		if (num3 >= num && num3 < num + 6)
		{
			return true;
		}
		if (num3 >= num2 && num3 < num2 + 10)
		{
			return true;
		}
		return false;
	}

	private byte HexToByte(string hex)
	{
		if (hex.Length > 2 || hex.Length <= 0)
		{
			throw new ArgumentException("hex must be 1 or 2 characters in length");
		}
		return byte.Parse(hex, NumberStyles.HexNumber);
	}
}
