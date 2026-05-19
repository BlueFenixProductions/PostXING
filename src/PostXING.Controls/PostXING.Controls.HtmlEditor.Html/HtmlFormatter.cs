using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace PostXING.Controls.HtmlEditor.Html;

public class HtmlFormatter
{
	private delegate bool CanContainTag(TagInfo info);

	private class FormatInfo
	{
		public TagInfo tagInfo;

		public bool isEndTag;

		public int indent;

		public bool isBeginTag => !isEndTag;

		public FormatInfo(TagInfo info, bool isEnd)
		{
			tagInfo = info;
			isEndTag = isEnd;
		}
	}

	private class Token
	{
		public const int Whitespace = 0;

		public const int TagName = 1;

		public const int AttrName = 2;

		public const int AttrVal = 3;

		public const int TextToken = 4;

		public const int SelfTerminating = 5;

		public const int Empty = 6;

		public const int Comment = 7;

		public const int Error = 8;

		public const int OpenBracket = 10;

		public const int CloseBracket = 11;

		public const int ForwardSlash = 12;

		public const int DoubleQuote = 13;

		public const int SingleQuote = 14;

		public const int EqualsChar = 15;

		public const int ClientScriptBlock = 20;

		public const int Style = 21;

		public const int InlineServerScript = 22;

		public const int ServerScriptBlock = 23;

		public const int XmlDirective = 24;

		private int _type;

		private char[] _chars;

		private int _charsLength;

		private string _text;

		private int _startIndex;

		private int _endIndex;

		private int _endState;

		internal char[] Chars => _chars;

		internal int CharsLength => _charsLength;

		public int EndIndex => _endIndex;

		public int EndState => _endState;

		public int Length => _endIndex - _startIndex;

		public int StartIndex => _startIndex;

		public string Text
		{
			get
			{
				if (_text == null)
				{
					_text = new string(_chars, StartIndex, EndIndex - StartIndex);
				}
				return _text;
			}
		}

		public int Type => _type;

		public Token(int type, int endState, int startIndex, int endIndex, char[] chars, int charsLength)
		{
			_type = type;
			_chars = chars;
			_charsLength = charsLength;
			_startIndex = startIndex;
			_endIndex = endIndex;
			_endState = endState;
		}
	}

	private class HtmlTokenizer
	{
		public static Token GetFirstToken(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			return GetNextToken(chars, chars.Length, 0, 0);
		}

		public static Token GetFirstToken(char[] chars, int length, int initialState)
		{
			return GetNextToken(chars, length, 0, initialState);
		}

		public static Token GetNextToken(Token token)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			return GetNextToken(token.Chars, token.CharsLength, token.EndIndex, token.EndState);
		}

		public static Token GetNextToken(char[] chars, int length, int startIndex, int startState)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (startIndex >= length)
			{
				return null;
			}
			int num = startState;
			bool flag = (startState & 0x100) != 0;
			int num2 = (flag ? 256 : 0);
			bool flag2 = (startState & 0x200) != 0;
			int num3 = (flag2 ? 512 : 0);
			bool flag3 = (startState & 0x400) != 0;
			int num4 = (flag3 ? 1024 : 0);
			bool flag4 = (startState & 0x800) != 0;
			int num5 = (flag4 ? 2048 : 0);
			int num6 = startIndex;
			int num7 = startIndex;
			int num8 = startIndex;
			Token token = null;
			while (token == null && num6 < length)
			{
				char c = chars[num6];
				switch (num & 0xFF)
				{
				case 0:
					if (c == '<')
					{
						num = 1;
						num8 = num6;
						token = new Token(4, num, num7, num8, chars, length);
					}
					break;
				case 1:
					if (c == '<')
					{
						if (num6 + 1 < length && chars[num6 + 1] == '%')
						{
							num = 0x1E | num2 | num3;
							num7 = num6;
						}
						else
						{
							num = 2 | num2 | num3;
							num8 = num6 + 1;
							token = new Token(10, num, num7, num8, chars, length);
						}
					}
					else
					{
						num = 16;
					}
					break;
				case 2:
					switch (c)
					{
					case '/':
						num = 3 | num2 | num3;
						num8 = num6;
						token = new Token(6, num, num7, num8, chars, length);
						break;
					case '!':
						num = 0x64 | num2 | num3;
						num7 = num6;
						break;
					case '%':
						num = 30;
						num7 = num6;
						break;
					default:
						if (IsWordChar(c))
						{
							num = 5 | num2 | num3;
							num7 = num6;
						}
						else
						{
							num = 16;
						}
						break;
					}
					break;
				case 30:
				{
					int num9 = IndexOf(chars, num6, length, "%>");
					if (num9 > -1)
					{
						num = 0;
						num8 = num9 + 2;
						token = new Token(22, num, num7, num8, chars, length);
					}
					else
					{
						num6 = length;
						num8 = num6;
					}
					break;
				}
				case 3:
					if (c == '/')
					{
						num = 4 | num2 | num3;
						num8 = num6 + 1;
						token = new Token(12, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 4:
					if (IsWordChar(c))
					{
						num = 5 | num2 | num3;
						num7 = num6;
					}
					else
					{
						num = 16;
					}
					break;
				case 5:
					if (IsWhitespace(c))
					{
						num = 6;
						num8 = num6;
						string text = new string(chars, num7, num8 - num7);
						if (text.ToLower().Equals("script"))
						{
							if (!flag)
							{
								num |= 0x100;
							}
						}
						else if (text.ToLower().Equals("style") && !flag2)
						{
							num |= 0x200;
						}
						token = new Token(1, num, num7, num8, chars, length);
					}
					else if (c == '>')
					{
						num = 17;
						num8 = num6;
						string text2 = new string(chars, num7, num8 - num7);
						if (text2.ToLower().Equals("script"))
						{
							if (!flag)
							{
								num |= 0x100;
							}
						}
						else if (text2.ToLower().Equals("style") && !flag2)
						{
							num |= 0x200;
						}
						token = new Token(1, num, num7, num8, chars, length);
					}
					else
					{
						if (IsWordChar(c))
						{
							break;
						}
						if (c == '/')
						{
							num = 15;
							num8 = num6;
							string text3 = new string(chars, num7, num8 - num7);
							if (text3.ToLower().Equals("script"))
							{
								if (!flag)
								{
									num |= 0x100;
								}
							}
							else if (text3.ToLower().Equals("style") && !flag2)
							{
								num |= 0x200;
							}
							token = new Token(1, num, num7, num8, chars, length);
						}
						else
						{
							num = 16;
						}
					}
					break;
				case 100:
					num = ((c != '-') ? ((!IsWordChar(c)) ? 16 : 60) : 101);
					break;
				case 101:
					num = ((c != '-') ? 16 : 102);
					break;
				case 102:
					if (c == '-')
					{
						num = 103;
					}
					break;
				case 103:
					num = ((c != '-') ? 102 : 104);
					break;
				case 104:
					if (!char.IsWhiteSpace(c))
					{
						if (c == '>')
						{
							num = 17;
							num8 = num6;
							token = new Token(7, num, num7, num8, chars, length);
						}
						else
						{
							num = 102;
						}
					}
					break;
				case 60:
					if (c == '>')
					{
						num = 17;
						num8 = num6;
						token = new Token(24, num, num7, num8, chars, length);
					}
					break;
				case 6:
					if (IsWordChar(c))
					{
						num = 7 | num2 | num3 | num5;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					}
					switch (c)
					{
					case '>':
						num = 0x11 | num2 | num3 | num5;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					case '/':
						num = 0xF | num2 | num3;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					default:
						if (!IsWhitespace(c))
						{
							num = 16;
						}
						break;
					}
					break;
				case 7:
					if (IsWhitespace(c))
					{
						num = 8 | num2 | num3 | num5;
						num8 = num6;
						if (flag && new string(chars, num7, num8 - num7).ToLower() == "runat")
						{
							num |= 0x400;
						}
						token = new Token(2, num, num7, num8, chars, length);
						break;
					}
					switch (c)
					{
					case '=':
						num = 8 | num2 | num3 | num5;
						num8 = num6;
						if (flag && new string(chars, num7, num8 - num7).ToLower() == "runat")
						{
							num |= 0x400;
						}
						token = new Token(2, num, num7, num8, chars, length);
						break;
					case '>':
						num = 0x11 | num2 | num3 | num5;
						num8 = num6;
						token = new Token(2, num, num7, num8, chars, length);
						break;
					case '/':
						num = 0xF | num2 | num3;
						num8 = num6;
						token = new Token(2, num, num7, num8, chars, length);
						break;
					default:
						if (!IsWordChar(c))
						{
							num = 16;
						}
						break;
					}
					break;
				case 8:
					switch (c)
					{
					case '=':
						num = 9 | num2 | num3 | num4 | num5;
						num7 = num6;
						num8 = num6 + 1;
						token = new Token(15, num, num7, num8, chars, length);
						break;
					case '>':
						num = 0x11 | num2 | num3 | num5;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					case '/':
						num = 15;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					default:
						if (IsWordChar(c))
						{
							num = 7 | num2 | num3 | num5;
							num8 = num6;
							token = new Token(0, num, num7, num8, chars, length);
						}
						else if (!IsWhitespace(c))
						{
							num = 16;
						}
						break;
					}
					break;
				case 18:
					if (c == '=')
					{
						num = 9 | num2 | num3 | num4 | num5;
						num8 = num6 + 1;
						token = new Token(15, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 9:
					switch (c)
					{
					case '\'':
						num = 0x14 | num2 | num3 | num4 | num5;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					case '"':
						num = 0x13 | num2 | num3 | num4 | num5;
						num8 = num6;
						token = new Token(0, num, num7, num8, chars, length);
						break;
					default:
						if (IsWordChar(c))
						{
							num = 0xE | num2 | num3 | num4 | num5;
							num8 = num6;
							token = new Token(0, num, num7, num8, chars, length);
						}
						else if (!IsWhitespace(c))
						{
							num = 16;
						}
						break;
					}
					break;
				case 19:
					if (c == '"')
					{
						num = 0xA | num2 | num3 | num4 | num5;
						num8 = num6 + 1;
						token = new Token(13, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 10:
					if (c == '"')
					{
						num = 0xB | num2 | num3 | num5;
						num8 = num6;
						if (flag3 && new string(chars, num7, num8 - num7).ToLower() == "server")
						{
							num |= 0x800;
						}
						token = new Token(3, num, num7, num8, chars, length);
					}
					break;
				case 11:
					if (c == '"')
					{
						num = 6 | num2 | num3 | num5;
						num8 = num6 + 1;
						token = new Token(13, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 20:
					if (c == '\'')
					{
						num = 0xC | num2 | num3 | num4 | num5;
						num8 = num6 + 1;
						token = new Token(14, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 12:
					if (c == '\'')
					{
						num = 0xD | num2 | num3 | num5;
						num8 = num6;
						if (flag3 && new string(chars, num7, num8 - num7).ToLower() == "server")
						{
							num |= 0x800;
						}
						token = new Token(3, num, num7, num8, chars, length);
					}
					break;
				case 13:
					if (c == '\'')
					{
						num = 6 | num2 | num3 | num5;
						num8 = num6 + 1;
						token = new Token(14, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 14:
					if (IsWhitespace(c))
					{
						num = 6 | num2 | num3 | num5;
						num8 = num6;
						if (flag3 && new string(chars, num7, num8 - num7).ToLower() == "server")
						{
							num |= 0x800;
						}
						token = new Token(3, num, num7, num8, chars, length);
						break;
					}
					switch (c)
					{
					case '>':
						num = 0x11 | num2 | num3 | num5;
						num8 = num6;
						if (flag3 && new string(chars, num7, num8 - num7).ToLower() == "server")
						{
							num |= 0x800;
						}
						token = new Token(3, num, num7, num8, chars, length);
						break;
					case '/':
						if (num6 + 1 < length && chars[num6 + 1] == '>')
						{
							num = 0xF | num2 | num3 | num5;
							num8 = num6;
							if (flag3 && new string(chars, num7, num8 - num7).ToLower() == "server")
							{
								num |= 0x800;
							}
							token = new Token(3, num, num7, num8, chars, length);
						}
						break;
					}
					break;
				case 15:
					if (c == '/' && num6 + 1 < length && chars[num6 + 1] == '>')
					{
						num = 0;
						num8 = num6 + 2;
						token = new Token(5, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 17:
					if (c == '>')
					{
						num = (flag ? (0x28 | num2 | num3 | num5) : (flag2 ? (0x32 | num2 | num3) : 0));
						num8 = num6 + 1;
						token = new Token(11, num, num7, num8, chars, length);
					}
					else
					{
						num = 16;
					}
					break;
				case 40:
				{
					int num11 = IndexOf(chars, num6, length, "</script>");
					if (num11 > -1)
					{
						num = 1 | num2 | num3 | num5;
						num8 = num11;
						token = ((!flag4) ? new Token(20, num, num7, num8, chars, length) : new Token(23, num, num7, num8, chars, length));
					}
					else
					{
						num6 = length - 1;
						num8 = num6;
					}
					break;
				}
				case 50:
				{
					int num10 = IndexOf(chars, num6, length, "</style>");
					if (num10 > -1)
					{
						num = 1 | num2 | num3;
						num8 = num10;
						token = new Token(21, num, num7, num8, chars, length);
					}
					else
					{
						num6 = length - 1;
						num8 = num6;
					}
					break;
				}
				case 16:
					if (c == '>')
					{
						num = 17;
						num8 = num6;
						token = new Token(8, num, num7, num8, chars, length);
					}
					break;
				}
				num6++;
			}
			if (num6 >= length && token == null)
			{
				int type;
				switch (num & 0xFF)
				{
				case 0:
					type = 4;
					break;
				case 40:
					type = ((!flag4) ? 20 : 23);
					break;
				case 50:
					type = 21;
					break;
				case 30:
					type = 22;
					break;
				case 100:
				case 101:
				case 102:
				case 103:
				case 104:
					type = 7;
					break;
				default:
					type = 8;
					num = 16;
					break;
				}
				num8 = num6;
				token = new Token(type, num, num7, num8, chars, length);
			}
			return token;
		}

		private static bool IsWhitespace(char c)
		{
			return char.IsWhiteSpace(c);
		}

		private static bool IsWordChar(char c)
		{
			if (!char.IsLetterOrDigit(c) && c != '_' && c != ':' && c != '#' && c != '-')
			{
				return c == '.';
			}
			return true;
		}

		private static int IndexOf(char[] chars, int startIndex, int endColumnNumber, string s)
		{
			int length = s.Length;
			int num = endColumnNumber - length + 1;
			for (int i = startIndex; i < num; i++)
			{
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (char.ToUpper(chars[i + j]) != char.ToUpper(s[j]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}
	}

	private class HtmlTokenizerStates
	{
		public const int Text = 0;

		public const int StartTag = 1;

		public const int ExpTag = 2;

		public const int ForwardSlash = 3;

		public const int ExpTagAfterSlash = 4;

		public const int InTagName = 5;

		public const int ExpAttr = 6;

		public const int InAttr = 7;

		public const int ExpEquals = 8;

		public const int ExpAttrVal = 9;

		public const int InDoubleQuoteAttrVal = 10;

		public const int EndDoubleQuote = 11;

		public const int InSingleQuoteAttrVal = 12;

		public const int EndSingleQuote = 13;

		public const int InAttrVal = 14;

		public const int SelfTerminating = 15;

		public const int Error = 16;

		public const int EndTag = 17;

		public const int EqualsChar = 18;

		public const int BeginDoubleQuote = 19;

		public const int BeginSingleQuote = 20;

		public const int ServerSideScript = 30;

		public const int Script = 40;

		public const int Style = 50;

		public const int XmlDirective = 60;

		public const int BeginCommentTag1 = 100;

		public const int BeginCommentTag2 = 101;

		public const int InCommentTag = 102;

		public const int EndCommentTag1 = 103;

		public const int EndCommentTag2 = 104;

		public const int ScriptState = 256;

		public const int StyleState = 512;

		public const int RunAtState = 1024;

		public const int RunAtServerState = 2048;
	}

	internal enum ElementType
	{
		Other,
		Block,
		Inline,
		Any
	}

	internal class FormattedTextWriter : TextWriter
	{
		private TextWriter baseWriter;

		private string indentString;

		private int currentColumn;

		private int indentLevel;

		private bool indentPending;

		private bool onNewLine;

		public override Encoding Encoding => baseWriter.Encoding;

		public override string NewLine
		{
			get
			{
				return baseWriter.NewLine;
			}
			set
			{
				baseWriter.NewLine = value;
			}
		}

		public int Indent
		{
			get
			{
				return indentLevel;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				indentLevel = value;
			}
		}

		public FormattedTextWriter(TextWriter writer, string indentString)
		{
			baseWriter = writer;
			this.indentString = indentString;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void Close()
		{
			baseWriter.Close();
		}

		public override void Flush()
		{
			baseWriter.Flush();
		}

		public static bool HasBackWhiteSpace(string s)
		{
			if (s == null || s.Length == 0)
			{
				return false;
			}
			return char.IsWhiteSpace(s[s.Length - 1]);
		}

		public static bool HasFrontWhiteSpace(string s)
		{
			if (s == null || s.Length == 0)
			{
				return false;
			}
			return char.IsWhiteSpace(s[0]);
		}

		public static bool IsWhiteSpace(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		private string MakeSingleLine(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < s.Length)
			{
				char c = s[i];
				if (char.IsWhiteSpace(c))
				{
					stringBuilder.Append(' ');
					for (; i < s.Length && char.IsWhiteSpace(s[i]); i++)
					{
					}
				}
				else
				{
					stringBuilder.Append(c);
					i++;
				}
			}
			return stringBuilder.ToString();
		}

		public static string Trim(string text, bool frontWhiteSpace)
		{
			if (text.Length == 0)
			{
				return string.Empty;
			}
			if (IsWhiteSpace(text))
			{
				if (frontWhiteSpace)
				{
					return " ";
				}
				return string.Empty;
			}
			string text2 = text.Trim();
			if (frontWhiteSpace && HasFrontWhiteSpace(text))
			{
				text2 = ' ' + text2;
			}
			if (HasBackWhiteSpace(text))
			{
				text2 += ' ';
			}
			return text2;
		}

		private void OutputIndent()
		{
			if (indentPending)
			{
				for (int i = 0; i < indentLevel; i++)
				{
					baseWriter.Write(indentString);
				}
				indentPending = false;
			}
		}

		public void WriteLiteral(string s)
		{
			if (s.Length == 0)
			{
				return;
			}
			StringReader stringReader = new StringReader(s);
			string text = stringReader.ReadLine();
			string text2 = stringReader.ReadLine();
			while (text != null)
			{
				Write(text);
				text = text2;
				text2 = stringReader.ReadLine();
				if (text != null)
				{
					WriteLine();
				}
				if (text2 != null)
				{
					text = text.Trim();
				}
				else if (text != null)
				{
					text = Trim(text, frontWhiteSpace: false);
				}
			}
		}

		public void WriteLiteralWrapped(string s, int maxLength)
		{
			if (s.Length == 0)
			{
				return;
			}
			string[] array = MakeSingleLine(s).Split(null);
			if (HasFrontWhiteSpace(s))
			{
				Write(' ');
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length <= 0)
				{
					continue;
				}
				Write(array[i]);
				if (i < array.Length - 1 && array[i + 1].Length > 0)
				{
					if (currentColumn > maxLength)
					{
						WriteLine();
					}
					else
					{
						Write(' ');
					}
				}
			}
			if (HasBackWhiteSpace(s) && !IsWhiteSpace(s))
			{
				Write(' ');
			}
		}

		public void WriteLineIfNotOnNewLine()
		{
			if (!onNewLine)
			{
				baseWriter.WriteLine();
				onNewLine = true;
				currentColumn = 0;
				indentPending = true;
			}
		}

		public override void Write(string s)
		{
			OutputIndent();
			baseWriter.Write(s);
			onNewLine = false;
			currentColumn += s.Length;
		}

		public override void Write(bool value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn += value.ToString().Length;
		}

		public override void Write(char value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn++;
		}

		public override void Write(char[] buffer)
		{
			OutputIndent();
			baseWriter.Write(buffer);
			onNewLine = false;
			currentColumn += buffer.Length;
		}

		public override void Write(char[] buffer, int index, int count)
		{
			OutputIndent();
			baseWriter.Write(buffer, index, count);
			onNewLine = false;
			currentColumn += count;
		}

		public override void Write(double value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn += value.ToString().Length;
		}

		public override void Write(float value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn += value.ToString().Length;
		}

		public override void Write(int value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn += value.ToString().Length;
		}

		public override void Write(long value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn += value.ToString().Length;
		}

		public override void Write(object value)
		{
			OutputIndent();
			baseWriter.Write(value);
			onNewLine = false;
			currentColumn += value.ToString().Length;
		}

		public override void Write(string format, object arg0)
		{
			OutputIndent();
			string text = string.Format(format, arg0);
			baseWriter.Write(text);
			onNewLine = false;
			currentColumn += text.Length;
		}

		public override void Write(string format, object arg0, object arg1)
		{
			OutputIndent();
			string text = string.Format(format, arg0, arg1);
			baseWriter.Write(text);
			onNewLine = false;
			currentColumn += text.Length;
		}

		public override void Write(string format, params object[] arg)
		{
			OutputIndent();
			string text = string.Format(format, arg);
			baseWriter.Write(text);
			onNewLine = false;
			currentColumn += text.Length;
		}

		public override void WriteLine(string s)
		{
			OutputIndent();
			baseWriter.WriteLine(s);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine()
		{
			OutputIndent();
			baseWriter.WriteLine();
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(bool value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(char value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(char[] buffer)
		{
			OutputIndent();
			baseWriter.WriteLine(buffer);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(char[] buffer, int index, int count)
		{
			OutputIndent();
			baseWriter.WriteLine(buffer, index, count);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(double value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(float value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(int value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(long value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(object value)
		{
			OutputIndent();
			baseWriter.WriteLine(value);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(string format, object arg0)
		{
			OutputIndent();
			baseWriter.WriteLine(format, arg0);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(string format, object arg0, object arg1)
		{
			OutputIndent();
			baseWriter.WriteLine(format, arg0, arg1);
			indentPending = true;
			onNewLine = true;
			currentColumn = 0;
		}

		public override void WriteLine(string format, params object[] arg)
		{
			OutputIndent();
			baseWriter.WriteLine(format, arg);
			indentPending = true;
			currentColumn = 0;
			onNewLine = true;
		}
	}

	[Flags]
	internal enum FormattingFlags
	{
		None = 0,
		Inline = 1,
		NoIndent = 2,
		NoEndTag = 4,
		PreserveContent = 8,
		Xml = 0x10,
		Comment = 0x20,
		AllowPartialTags = 0x40
	}

	internal class HtmlWriter
	{
		private FormattedTextWriter _writer;

		private TextWriter _baseWriter;

		private int _maxLineLength;

		protected TextWriter BaseWriter => _baseWriter;

		public virtual string Content
		{
			get
			{
				_writer.Flush();
				return _baseWriter.ToString();
			}
		}

		public int Indent
		{
			get
			{
				return _writer.Indent;
			}
			set
			{
				_writer.Indent = value;
			}
		}

		public FormattedTextWriter Writer => _writer;

		public HtmlWriter(TextWriter writer, string indentString, int maxLineLength)
		{
			_baseWriter = writer;
			_maxLineLength = maxLineLength;
			_writer = new FormattedTextWriter(_baseWriter, indentString);
		}

		public void Flush()
		{
			_writer.Flush();
		}

		public virtual void Write(char c)
		{
			_writer.Write(c);
		}

		public virtual void Write(string s)
		{
			_writer.Write(s);
		}

		public virtual void WriteLiteral(string s, bool frontWhiteSpace)
		{
			_writer.WriteLiteralWrapped(FormattedTextWriter.Trim(s, frontWhiteSpace), _maxLineLength);
		}

		public virtual void WriteLineIfNotOnNewLine()
		{
			_writer.WriteLineIfNotOnNewLine();
		}
	}

	internal class TagInfo
	{
		private string _tagName;

		private WhiteSpaceType _inner;

		private WhiteSpaceType _following;

		private FormattingFlags _flags;

		private ElementType _type;

		public ElementType Type => _type;

		public FormattingFlags Flags => _flags;

		public WhiteSpaceType FollowingWhiteSpaceType => _following;

		public WhiteSpaceType InnerWhiteSpaceType => _inner;

		public bool IsComment => (_flags & FormattingFlags.Comment) != 0;

		public bool IsInline => (_flags & FormattingFlags.Inline) != 0;

		public bool IsXml => (_flags & FormattingFlags.Xml) != 0;

		public bool NoEndTag => (_flags & FormattingFlags.NoEndTag) != 0;

		public bool NoIndent
		{
			get
			{
				if ((_flags & FormattingFlags.NoIndent) == 0)
				{
					return NoEndTag;
				}
				return true;
			}
		}

		public bool PreserveContent => (_flags & FormattingFlags.PreserveContent) != 0;

		public string TagName => _tagName;

		public TagInfo(string tagName, FormattingFlags flags)
			: this(tagName, flags, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Other)
		{
		}

		public TagInfo(string tagName, FormattingFlags flags, ElementType type)
			: this(tagName, flags, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, type)
		{
		}

		public TagInfo(string tagName, FormattingFlags flags, WhiteSpaceType innerWhiteSpace, WhiteSpaceType followingWhiteSpace)
			: this(tagName, flags, innerWhiteSpace, followingWhiteSpace, ElementType.Other)
		{
		}

		public TagInfo(string tagName, FormattingFlags flags, WhiteSpaceType innerWhiteSpace, WhiteSpaceType followingWhiteSpace, ElementType type)
		{
			_tagName = tagName;
			_inner = innerWhiteSpace;
			_following = followingWhiteSpace;
			_flags = flags;
			_type = type;
		}

		public TagInfo(string newTagName, TagInfo info)
		{
			_tagName = newTagName;
			_inner = info.InnerWhiteSpaceType;
			_following = info.FollowingWhiteSpaceType;
			_flags = info.Flags;
			_type = info.Type;
		}

		public virtual bool CanContainTag(TagInfo info)
		{
			return true;
		}
	}

	internal class LITagInfo : TagInfo
	{
		public LITagInfo()
			: base("li", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.CarryThrough)
		{
		}

		public override bool CanContainTag(TagInfo info)
		{
			if (info.Type == ElementType.Any)
			{
				return true;
			}
			if ((info.Type == ElementType.Inline) | (info.Type == ElementType.Block))
			{
				return true;
			}
			return false;
		}
	}

	internal class OLTagInfo : TagInfo
	{
		public OLTagInfo()
			: base("ol", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block)
		{
		}

		public override bool CanContainTag(TagInfo info)
		{
			if (info.Type == ElementType.Any)
			{
				return true;
			}
			if (info.TagName.ToLower().Equals("li"))
			{
				return true;
			}
			return false;
		}
	}

	internal class PTagInfo : TagInfo
	{
		public PTagInfo()
			: base("p", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block)
		{
		}

		public override bool CanContainTag(TagInfo info)
		{
			if (info.Type == ElementType.Any)
			{
				return true;
			}
			if ((info.Type == ElementType.Inline) | info.TagName.ToLower().Equals("table") | info.TagName.ToLower().Equals("hr"))
			{
				return true;
			}
			return false;
		}
	}

	internal class TDTagInfo : TagInfo
	{
		public TDTagInfo()
			: base("td", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Other)
		{
		}

		public override bool CanContainTag(TagInfo info)
		{
			if (info.Type == ElementType.Any)
			{
				return true;
			}
			if ((info.Type == ElementType.Inline) | (info.Type == ElementType.Block))
			{
				return true;
			}
			return false;
		}
	}

	internal class TRTagInfo : TagInfo
	{
		public TRTagInfo()
			: base("tr", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Other)
		{
		}

		public override bool CanContainTag(TagInfo info)
		{
			if (info.Type == ElementType.Any)
			{
				return true;
			}
			if (info.TagName.ToLower().Equals("th") | info.TagName.ToLower().Equals("td"))
			{
				return true;
			}
			return false;
		}
	}

	internal enum WhiteSpaceType
	{
		Significant,
		NotSignificant,
		CarryThrough
	}

	internal class XmlWriter : HtmlWriter
	{
		private bool _containsText;

		private StringBuilder _unformatted;

		private string _tagName;

		private bool _isUnknownXml;

		public bool ContainsText
		{
			get
			{
				return _containsText;
			}
			set
			{
				_containsText = value;
			}
		}

		public override string Content
		{
			get
			{
				if (ContainsText)
				{
					return _unformatted.ToString();
				}
				return base.Content;
			}
		}

		public string TagName => _tagName;

		public bool IsUnknownXml => _isUnknownXml;

		public XmlWriter(int initialIndent, string tagName, string indentString, int maxLineLength)
			: base(new StringWriter(), indentString, maxLineLength)
		{
			base.Writer.Indent = initialIndent;
			_unformatted = new StringBuilder();
			_tagName = tagName;
			_isUnknownXml = _tagName.IndexOf(':') > -1;
		}

		public override void Write(char c)
		{
			base.Write(c);
			_unformatted.Append(c);
		}

		public override void Write(string s)
		{
			base.Write(s);
			_unformatted.Append(s);
		}

		public override void WriteLiteral(string s, bool frontWhiteSpace)
		{
			base.WriteLiteral(s, frontWhiteSpace);
			_unformatted.Append(s);
		}
	}

	private enum HtmlFormatterCase
	{
		PreserveCase,
		UpperCase,
		LowerCase
	}

	private static IDictionary tagTable;

	private static TagInfo commentTag;

	private static TagInfo directiveTag;

	private static TagInfo otherServerSideScriptTag;

	private static TagInfo nestedXmlTag;

	private static TagInfo unknownXmlTag;

	private static TagInfo unknownHtmlTag;

	private HtmlFormatterCase _elementCasing = HtmlFormatterCase.LowerCase;

	private HtmlFormatterCase _attributeCasing = HtmlFormatterCase.LowerCase;

	private char _indentChar = '\t';

	private int _indentSize = 1;

	private int _maxLineLength = 80;

	public char IndentChar
	{
		get
		{
			return _indentChar;
		}
		set
		{
			_indentChar = value;
		}
	}

	public int IndentSize
	{
		get
		{
			return _indentSize;
		}
		set
		{
			_indentSize = value;
		}
	}

	public int MaxLineLength
	{
		get
		{
			return _maxLineLength;
		}
		set
		{
			_maxLineLength = value;
		}
	}

	static HtmlFormatter()
	{
		commentTag = new TagInfo("", FormattingFlags.NoEndTag | FormattingFlags.Comment, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Any);
		directiveTag = new TagInfo("", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
		otherServerSideScriptTag = new TagInfo("", FormattingFlags.Inline | FormattingFlags.NoEndTag, ElementType.Any);
		unknownXmlTag = new TagInfo("", FormattingFlags.Xml, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
		nestedXmlTag = new TagInfo("", FormattingFlags.AllowPartialTags, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
		unknownHtmlTag = new TagInfo("", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
		tagTable = new HybridDictionary(caseInsensitive: true);
		tagTable["a"] = new TagInfo("a", FormattingFlags.Inline, ElementType.Inline);
		tagTable["acronym"] = new TagInfo("acronym", FormattingFlags.Inline, ElementType.Inline);
		tagTable["address"] = new TagInfo("address", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["applet"] = new TagInfo("applet", FormattingFlags.Inline, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Inline);
		tagTable["area"] = new TagInfo("area", FormattingFlags.NoEndTag);
		tagTable["b"] = new TagInfo("b", FormattingFlags.Inline, ElementType.Inline);
		tagTable["base"] = new TagInfo("base", FormattingFlags.NoEndTag);
		tagTable["basefont"] = new TagInfo("basefont", FormattingFlags.NoEndTag, ElementType.Block);
		tagTable["bdo"] = new TagInfo("bdo", FormattingFlags.Inline, ElementType.Inline);
		tagTable["bgsound"] = new TagInfo("bgsound", FormattingFlags.NoEndTag);
		tagTable["big"] = new TagInfo("big", FormattingFlags.Inline, ElementType.Inline);
		tagTable["blink"] = new TagInfo("blink", FormattingFlags.Inline);
		tagTable["blockquote"] = new TagInfo("blockquote", FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["body"] = new TagInfo("body", FormattingFlags.None);
		tagTable["br"] = new TagInfo("br", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Inline);
		tagTable["button"] = new TagInfo("button", FormattingFlags.Inline, ElementType.Inline);
		tagTable["caption"] = new TagInfo("caption", FormattingFlags.None);
		tagTable["cite"] = new TagInfo("cite", FormattingFlags.Inline, ElementType.Inline);
		tagTable["center"] = new TagInfo("center", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["code"] = new TagInfo("code", FormattingFlags.Inline, ElementType.Inline);
		tagTable["col"] = new TagInfo("col", FormattingFlags.NoEndTag);
		tagTable["colgroup"] = new TagInfo("colgroup", FormattingFlags.None);
		tagTable["dd"] = new TagInfo("dd", FormattingFlags.None);
		tagTable["del"] = new TagInfo("del", FormattingFlags.None);
		tagTable["dfn"] = new TagInfo("dfn", FormattingFlags.Inline, ElementType.Inline);
		tagTable["dir"] = new TagInfo("dir", FormattingFlags.None, ElementType.Block);
		tagTable["div"] = new TagInfo("div", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["dl"] = new TagInfo("dl", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["dt"] = new TagInfo("dt", FormattingFlags.Inline);
		tagTable["em"] = new TagInfo("em", FormattingFlags.Inline, ElementType.Inline);
		tagTable["embed"] = new TagInfo("embed", FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough, ElementType.Inline);
		tagTable["fieldset"] = new TagInfo("fieldset", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["font"] = new TagInfo("font", FormattingFlags.Inline, ElementType.Inline);
		tagTable["form"] = new TagInfo("form", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["frame"] = new TagInfo("frame", FormattingFlags.NoEndTag);
		tagTable["frameset"] = new TagInfo("frameset", FormattingFlags.None);
		tagTable["head"] = new TagInfo("head", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant);
		tagTable["h1"] = new TagInfo("h1", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["h2"] = new TagInfo("h2", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["h3"] = new TagInfo("h3", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["h4"] = new TagInfo("h4", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["h5"] = new TagInfo("h5", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["h6"] = new TagInfo("h6", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["hr"] = new TagInfo("hr", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["html"] = new TagInfo("html", FormattingFlags.NoIndent, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant);
		tagTable["i"] = new TagInfo("i", FormattingFlags.Inline, ElementType.Inline);
		tagTable["iframe"] = new TagInfo("iframe", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Inline);
		tagTable["img"] = new TagInfo("img", FormattingFlags.Inline | FormattingFlags.NoEndTag, WhiteSpaceType.Significant, WhiteSpaceType.Significant, ElementType.Inline);
		tagTable["input"] = new TagInfo("input", FormattingFlags.NoEndTag, WhiteSpaceType.Significant, WhiteSpaceType.Significant, ElementType.Inline);
		tagTable["ins"] = new TagInfo("ins", FormattingFlags.None);
		tagTable["isindex"] = new TagInfo("isindex", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.CarryThrough, ElementType.Block);
		tagTable["kbd"] = new TagInfo("kbd", FormattingFlags.Inline, ElementType.Inline);
		tagTable["label"] = new TagInfo("label", FormattingFlags.Inline, ElementType.Inline);
		tagTable["legend"] = new TagInfo("legend", FormattingFlags.None);
		tagTable["li"] = new LITagInfo();
		tagTable["link"] = new TagInfo("link", FormattingFlags.NoEndTag);
		tagTable["listing"] = new TagInfo("listing", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["map"] = new TagInfo("map", FormattingFlags.Inline, ElementType.Inline);
		tagTable["marquee"] = new TagInfo("marquee", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["menu"] = new TagInfo("menu", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["meta"] = new TagInfo("meta", FormattingFlags.NoEndTag);
		tagTable["nobr"] = new TagInfo("nobr", FormattingFlags.Inline | FormattingFlags.NoEndTag, ElementType.Inline);
		tagTable["noembed"] = new TagInfo("noembed", FormattingFlags.None, ElementType.Block);
		tagTable["noframes"] = new TagInfo("noframes", FormattingFlags.None, ElementType.Block);
		tagTable["noscript"] = new TagInfo("noscript", FormattingFlags.None, ElementType.Block);
		tagTable["object"] = new TagInfo("object", FormattingFlags.None, ElementType.Inline);
		tagTable["ol"] = new OLTagInfo();
		tagTable["option"] = new TagInfo("option", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough);
		tagTable["p"] = new PTagInfo();
		tagTable["param"] = new TagInfo("param", FormattingFlags.NoEndTag);
		tagTable["pre"] = new TagInfo("pre", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["q"] = new TagInfo("q", FormattingFlags.Inline, ElementType.Inline);
		tagTable["rt"] = new TagInfo("rt", FormattingFlags.None);
		tagTable["ruby"] = new TagInfo("ruby", FormattingFlags.None, ElementType.Inline);
		tagTable["s"] = new TagInfo("s", FormattingFlags.Inline, ElementType.Inline);
		tagTable["samp"] = new TagInfo("samp", FormattingFlags.None, ElementType.Inline);
		tagTable["script"] = new TagInfo("script", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Inline);
		tagTable["select"] = new TagInfo("select", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Block);
		tagTable["small"] = new TagInfo("small", FormattingFlags.Inline, ElementType.Inline);
		tagTable["span"] = new TagInfo("span", FormattingFlags.Inline, ElementType.Inline);
		tagTable["strike"] = new TagInfo("strike", FormattingFlags.Inline, ElementType.Inline);
		tagTable["strong"] = new TagInfo("strong", FormattingFlags.Inline, ElementType.Inline);
		tagTable["style"] = new TagInfo("style", FormattingFlags.PreserveContent, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
		tagTable["sub"] = new TagInfo("sub", FormattingFlags.Inline, ElementType.Inline);
		tagTable["sup"] = new TagInfo("sup", FormattingFlags.Inline, ElementType.Inline);
		tagTable["table"] = new TagInfo("table", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["tbody"] = new TagInfo("tbody", FormattingFlags.None);
		tagTable["td"] = new TDTagInfo();
		tagTable["textarea"] = new TagInfo("textarea", FormattingFlags.Inline, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Inline);
		tagTable["tfoot"] = new TagInfo("tfoot", FormattingFlags.None);
		tagTable["th"] = new TagInfo("th", FormattingFlags.None);
		tagTable["thead"] = new TagInfo("thead", FormattingFlags.None);
		tagTable["title"] = new TagInfo("title", FormattingFlags.Inline);
		tagTable["tr"] = new TRTagInfo();
		tagTable["tt"] = new TagInfo("tt", FormattingFlags.Inline, ElementType.Inline);
		tagTable["u"] = new TagInfo("u", FormattingFlags.Inline, ElementType.Inline);
		tagTable["ul"] = new TagInfo("ul", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["xml"] = new TagInfo("xml", FormattingFlags.Xml, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["xmp"] = new TagInfo("xmp", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
		tagTable["var"] = new TagInfo("var", FormattingFlags.Inline, ElementType.Inline);
		tagTable["wbr"] = new TagInfo("wbr", FormattingFlags.Inline | FormattingFlags.NoEndTag, ElementType.Inline);
	}

	public void Format(string input, TextWriter output)
	{
		bool flag = true;
		int maxLineLength = _maxLineLength;
		string indentString = new string(_indentChar, _indentSize);
		char[] chars = input.ToCharArray();
		Stack stack = new Stack();
		Stack stack2 = new Stack();
		FormatInfo formatInfo = null;
		FormatInfo formatInfo2 = null;
		string text = string.Empty;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		HtmlWriter htmlWriter = new HtmlWriter(output, indentString, maxLineLength);
		stack2.Push(htmlWriter);
		Token token = HtmlTokenizer.GetFirstToken(chars);
		Token token2 = token;
		while (token != null)
		{
			htmlWriter = (HtmlWriter)stack2.Peek();
			switch (token.Type)
			{
			case 2:
				if (flag)
				{
					string empty = string.Empty;
					empty = (formatInfo.tagInfo.IsXml ? token.Text : token.Text.ToLower());
					htmlWriter.Write(empty);
					Token nextToken = HtmlTokenizer.GetNextToken(token);
					if (nextToken.Type != 15)
					{
						htmlWriter.Write("=\"" + empty + "\"");
					}
				}
				else if (!formatInfo.tagInfo.IsXml)
				{
					if (_attributeCasing == HtmlFormatterCase.UpperCase)
					{
						htmlWriter.Write(token.Text.ToUpper());
					}
					else if (_attributeCasing == HtmlFormatterCase.LowerCase)
					{
						htmlWriter.Write(token.Text.ToLower());
					}
					else
					{
						htmlWriter.Write(token.Text);
					}
				}
				else
				{
					htmlWriter.Write(token.Text);
				}
				break;
			case 3:
				if (flag && token2.Type != 13 && token2.Type != 14)
				{
					htmlWriter.Write('"');
					htmlWriter.Write(token.Text.Replace("\"", "&quot;"));
					htmlWriter.Write('"');
				}
				else
				{
					htmlWriter.Write(token.Text);
				}
				break;
			case 11:
				if (flag)
				{
					if (flag4)
					{
						flag4 = false;
					}
					else if (flag3 && !formatInfo.tagInfo.IsComment)
					{
						htmlWriter.Write(" />");
					}
					else
					{
						htmlWriter.Write('>');
					}
				}
				else
				{
					htmlWriter.Write('>');
				}
				break;
			case 13:
				htmlWriter.Write('"');
				break;
			case 15:
				htmlWriter.Write('=');
				break;
			case 8:
				if (token2.Type == 10)
				{
					htmlWriter.Write('<');
				}
				htmlWriter.Write(token.Text);
				break;
			case 5:
				formatInfo.isEndTag = true;
				if (!formatInfo.tagInfo.NoEndTag)
				{
					stack.Pop();
					if (formatInfo.tagInfo.IsXml)
					{
						HtmlWriter htmlWriter6 = (HtmlWriter)stack2.Pop();
						htmlWriter = (HtmlWriter)stack2.Peek();
						htmlWriter.Write(htmlWriter6.Content);
					}
				}
				if (token2.Type == 0 && token2.Text.Length > 0)
				{
					htmlWriter.Write("/>");
				}
				else
				{
					htmlWriter.Write(" />");
				}
				break;
			case 14:
				htmlWriter.Write('\'');
				break;
			case 24:
				htmlWriter.WriteLineIfNotOnNewLine();
				htmlWriter.Write('<');
				htmlWriter.Write(token.Text);
				htmlWriter.Write('>');
				htmlWriter.WriteLineIfNotOnNewLine();
				flag4 = true;
				break;
			case 1:
			case 7:
			case 22:
			{
				flag3 = false;
				string text2;
				TagInfo tagInfo;
				if (token.Type == 7)
				{
					text2 = token.Text;
					tagInfo = new TagInfo(token.Text, commentTag);
				}
				else if (token.Type == 22)
				{
					string text3 = token.Text.Trim();
					text3 = text3.Substring(1);
					text2 = text3;
					tagInfo = ((!text3.StartsWith("%@")) ? new TagInfo(text3, otherServerSideScriptTag) : new TagInfo(text3, directiveTag));
				}
				else
				{
					text2 = token.Text;
					tagInfo = tagTable[text2] as TagInfo;
					if (tagInfo == null)
					{
						tagInfo = ((text2.IndexOf(':') > -1) ? new TagInfo(text2, unknownXmlTag) : ((!(htmlWriter is XmlWriter)) ? new TagInfo(text2, unknownHtmlTag) : new TagInfo(text2, nestedXmlTag)));
					}
					else if (_elementCasing == HtmlFormatterCase.LowerCase || flag)
					{
						text2 = tagInfo.TagName;
					}
					else if (_elementCasing == HtmlFormatterCase.UpperCase)
					{
						text2 = tagInfo.TagName.ToUpper();
					}
				}
				if (formatInfo == null)
				{
					formatInfo = new FormatInfo(tagInfo, isEnd: false);
					formatInfo.indent = 0;
					stack.Push(formatInfo);
					htmlWriter.Write(text);
					if (tagInfo.IsXml)
					{
						HtmlWriter htmlWriter2 = new XmlWriter(htmlWriter.Indent, tagInfo.TagName, indentString, maxLineLength);
						stack2.Push(htmlWriter2);
						htmlWriter = htmlWriter2;
					}
					if (token2.Type == 12)
					{
						htmlWriter.Write("</");
					}
					else
					{
						htmlWriter.Write('<');
					}
					htmlWriter.Write(text2);
					text = string.Empty;
					break;
				}
				formatInfo2 = new FormatInfo(tagInfo, token2.Type == 12);
				WhiteSpaceType whiteSpaceType = ((!formatInfo.isEndTag) ? formatInfo.tagInfo.InnerWhiteSpaceType : formatInfo.tagInfo.FollowingWhiteSpaceType);
				bool flag5 = formatInfo.tagInfo.IsInline;
				bool flag6 = false;
				bool flag7 = false;
				if (htmlWriter is XmlWriter)
				{
					XmlWriter xmlWriter = (XmlWriter)htmlWriter;
					if (xmlWriter.IsUnknownXml)
					{
						flag7 = ((formatInfo.isBeginTag && formatInfo.tagInfo.TagName.ToLower() == xmlWriter.TagName.ToLower()) || (formatInfo2.isEndTag && formatInfo2.tagInfo.TagName.ToLower() == xmlWriter.TagName.ToLower())) && !FormattedTextWriter.IsWhiteSpace(text);
					}
					if (formatInfo.isBeginTag)
					{
						if (FormattedTextWriter.IsWhiteSpace(text))
						{
							if (xmlWriter.IsUnknownXml && formatInfo2.isEndTag && formatInfo.tagInfo.TagName.ToLower() == formatInfo2.tagInfo.TagName.ToLower())
							{
								flag5 = true;
								flag6 = true;
								text = "";
							}
						}
						else if (!xmlWriter.IsUnknownXml)
						{
							xmlWriter.ContainsText = true;
						}
					}
				}
				bool frontWhiteSpace = true;
				if (formatInfo.isBeginTag && formatInfo.tagInfo.PreserveContent)
				{
					htmlWriter.Write(text);
				}
				else
				{
					switch (whiteSpaceType)
					{
					case WhiteSpaceType.NotSignificant:
						if (!flag5 && !flag7)
						{
							htmlWriter.WriteLineIfNotOnNewLine();
							frontWhiteSpace = false;
						}
						break;
					case WhiteSpaceType.Significant:
						if (FormattedTextWriter.HasFrontWhiteSpace(text) && !flag5 && !flag7)
						{
							htmlWriter.WriteLineIfNotOnNewLine();
							frontWhiteSpace = false;
						}
						break;
					case WhiteSpaceType.CarryThrough:
						if ((flag2 || FormattedTextWriter.HasFrontWhiteSpace(text)) && !flag5 && !flag7)
						{
							htmlWriter.WriteLineIfNotOnNewLine();
							frontWhiteSpace = false;
						}
						break;
					}
					if (formatInfo.isBeginTag && !formatInfo.tagInfo.NoIndent && !flag5)
					{
						htmlWriter.Indent++;
					}
					if (flag7)
					{
						htmlWriter.Write(text);
					}
					else
					{
						htmlWriter.WriteLiteral(text, frontWhiteSpace);
					}
				}
				if (formatInfo2.isEndTag)
				{
					if (!formatInfo2.tagInfo.NoEndTag)
					{
						ArrayList arrayList = new ArrayList();
						FormatInfo formatInfo3 = null;
						bool flag8 = false;
						bool flag9 = false;
						if ((formatInfo2.tagInfo.Flags & FormattingFlags.AllowPartialTags) != FormattingFlags.None)
						{
							flag9 = true;
						}
						if (stack.Count > 0)
						{
							formatInfo3 = (FormatInfo)stack.Pop();
							arrayList.Add(formatInfo3);
							while (stack.Count > 0 && formatInfo3.tagInfo.TagName.ToLower() != formatInfo2.tagInfo.TagName.ToLower())
							{
								if ((formatInfo3.tagInfo.Flags & FormattingFlags.AllowPartialTags) != FormattingFlags.None)
								{
									flag9 = true;
									break;
								}
								formatInfo3 = (FormatInfo)stack.Pop();
								arrayList.Add(formatInfo3);
							}
							if (formatInfo3.tagInfo.TagName.ToLower() != formatInfo2.tagInfo.TagName.ToLower())
							{
								for (int num = arrayList.Count - 1; num >= 0; num--)
								{
									stack.Push(arrayList[num]);
								}
							}
							else
							{
								flag8 = true;
								for (int i = 0; i < arrayList.Count - 1; i++)
								{
									FormatInfo formatInfo4 = (FormatInfo)arrayList[i];
									if (formatInfo4.tagInfo.IsXml && stack2.Count > 1)
									{
										HtmlWriter htmlWriter3 = (HtmlWriter)stack2.Pop();
										htmlWriter = (HtmlWriter)stack2.Peek();
										htmlWriter.Write(htmlWriter3.Content);
									}
									if (!formatInfo4.tagInfo.NoEndTag)
									{
										htmlWriter.WriteLineIfNotOnNewLine();
										htmlWriter.Indent = formatInfo4.indent;
										if (flag && !flag9)
										{
											htmlWriter.Write("</" + formatInfo4.tagInfo.TagName + ">");
										}
									}
								}
								htmlWriter.Indent = formatInfo3.indent;
							}
						}
						if (flag8 || flag9)
						{
							if (!flag6 && !flag7 && !formatInfo2.tagInfo.IsInline && !formatInfo2.tagInfo.PreserveContent && (FormattedTextWriter.IsWhiteSpace(text) || FormattedTextWriter.HasBackWhiteSpace(text) || formatInfo2.tagInfo.FollowingWhiteSpaceType == WhiteSpaceType.NotSignificant) && (!(formatInfo2.tagInfo is TDTagInfo) || FormattedTextWriter.HasBackWhiteSpace(text)))
							{
								htmlWriter.WriteLineIfNotOnNewLine();
							}
							htmlWriter.Write("</");
							htmlWriter.Write(text2);
						}
						else
						{
							flag4 = true;
						}
						if (formatInfo2.tagInfo.IsXml && stack2.Count > 1)
						{
							HtmlWriter htmlWriter4 = (HtmlWriter)stack2.Pop();
							htmlWriter = (HtmlWriter)stack2.Peek();
							htmlWriter.Write(htmlWriter4.Content);
						}
					}
					else
					{
						flag4 = true;
					}
				}
				else
				{
					bool flag10 = false;
					while (!flag10 && stack.Count > 0)
					{
						FormatInfo formatInfo5 = (FormatInfo)stack.Peek();
						flag10 = formatInfo5.tagInfo.CanContainTag(formatInfo2.tagInfo);
						if (flag10)
						{
							continue;
						}
						stack.Pop();
						htmlWriter.Indent = formatInfo5.indent;
						if (flag)
						{
							if (!formatInfo5.tagInfo.IsInline)
							{
								htmlWriter.WriteLineIfNotOnNewLine();
							}
							htmlWriter.Write("</" + formatInfo5.tagInfo.TagName + ">");
						}
					}
					formatInfo2.indent = htmlWriter.Indent;
					if (!flag7 && !formatInfo2.tagInfo.IsInline && !formatInfo2.tagInfo.PreserveContent && (FormattedTextWriter.IsWhiteSpace(text) || FormattedTextWriter.HasBackWhiteSpace(text) || (text.Length == 0 && ((formatInfo.isBeginTag && formatInfo.tagInfo.InnerWhiteSpaceType == WhiteSpaceType.NotSignificant) || (formatInfo.isEndTag && formatInfo.tagInfo.FollowingWhiteSpaceType == WhiteSpaceType.NotSignificant)))))
					{
						htmlWriter.WriteLineIfNotOnNewLine();
					}
					if (!formatInfo2.tagInfo.NoEndTag)
					{
						stack.Push(formatInfo2);
					}
					else
					{
						flag3 = true;
					}
					if (formatInfo2.tagInfo.IsXml)
					{
						HtmlWriter htmlWriter5 = new XmlWriter(htmlWriter.Indent, formatInfo2.tagInfo.TagName, indentString, maxLineLength);
						stack2.Push(htmlWriter5);
						htmlWriter = htmlWriter5;
					}
					htmlWriter.Write('<');
					htmlWriter.Write(text2);
				}
				flag2 = FormattedTextWriter.HasBackWhiteSpace(text);
				text = string.Empty;
				formatInfo = formatInfo2;
				break;
			}
			case 4:
			case 20:
			case 21:
			case 23:
				text = ((!flag) ? (text + token.Text) : (text + token.Text.Replace("&nbsp;", "&#160;")));
				break;
			case 0:
				if (token.Text.Length > 0)
				{
					htmlWriter.Write(' ');
				}
				break;
			}
			token2 = token;
			token = HtmlTokenizer.GetNextToken(token);
		}
		if (text.Length > 0)
		{
			htmlWriter.Write(text);
		}
		while (stack2.Count > 1)
		{
			HtmlWriter htmlWriter7 = (HtmlWriter)stack2.Pop();
			htmlWriter = (HtmlWriter)stack2.Peek();
			htmlWriter.Write(htmlWriter7.Content);
		}
		htmlWriter.Flush();
	}
}
