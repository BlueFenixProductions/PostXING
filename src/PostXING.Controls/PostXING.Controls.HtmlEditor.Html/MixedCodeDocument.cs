using System;
using System.IO;
using System.Text;

namespace PostXING.Controls.HtmlEditor.Html;

public class MixedCodeDocument
{
	private enum ParseState
	{
		Text,
		Code
	}

	private Encoding _streamencoding;

	internal string _text;

	internal MixedCodeDocumentFragmentList _fragments;

	internal MixedCodeDocumentFragmentList _codefragments;

	internal MixedCodeDocumentFragmentList _textfragments;

	private ParseState _state;

	private int _index;

	private int _c;

	private int _line;

	private int _lineposition;

	private MixedCodeDocumentFragment _currentfragment;

	public string TokenCodeStart = "<%";

	public string TokenCodeEnd = "%>";

	public string TokenDirective = "@";

	public string TokenResponseWrite = "Response.Write ";

	private string TokenTextBlock = "TextBlock({0})";

	public Encoding StreamEncoding => _streamencoding;

	public MixedCodeDocumentFragmentList CodeFragments => _codefragments;

	public MixedCodeDocumentFragmentList TextFragments => _textfragments;

	public MixedCodeDocumentFragmentList Fragments => _fragments;

	public string Code
	{
		get
		{
			string text = "";
			int num = 0;
			foreach (MixedCodeDocumentFragment fragment in _fragments)
			{
				switch (fragment._type)
				{
				case MixedCodeDocumentFragmentType.Text:
					text = text + TokenResponseWrite + string.Format(TokenTextBlock, num) + "\n";
					num++;
					break;
				case MixedCodeDocumentFragmentType.Code:
					text = text + ((MixedCodeDocumentCodeFragment)fragment).Code + "\n";
					break;
				}
			}
			return text;
		}
	}

	public MixedCodeDocument()
	{
		_codefragments = new MixedCodeDocumentFragmentList(this);
		_textfragments = new MixedCodeDocumentFragmentList(this);
		_fragments = new MixedCodeDocumentFragmentList(this);
	}

	public void Load(Stream stream)
	{
		Load(new StreamReader(stream));
	}

	public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
	{
		Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
	}

	public void Load(Stream stream, Encoding encoding)
	{
		Load(new StreamReader(stream, encoding));
	}

	public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
		Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
	}

	public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
	{
		Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
	}

	public void Load(string path)
	{
		Load(new StreamReader(path));
	}

	public void Load(string path, bool detectEncodingFromByteOrderMarks)
	{
		Load(new StreamReader(path, detectEncodingFromByteOrderMarks));
	}

	public void Load(string path, Encoding encoding)
	{
		Load(new StreamReader(path, encoding));
	}

	public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
	{
		Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks));
	}

	public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
	{
		Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize));
	}

	public void LoadHtml(string html)
	{
		Load(new StringReader(html));
	}

	public void Load(TextReader reader)
	{
		_codefragments.Clear();
		_textfragments.Clear();
		if (reader is StreamReader streamReader)
		{
			_streamencoding = streamReader.CurrentEncoding;
		}
		_text = reader.ReadToEnd();
		reader.Close();
		Parse();
	}

	internal Encoding GetOutEncoding()
	{
		if (_streamencoding != null)
		{
			return _streamencoding;
		}
		return Encoding.Default;
	}

	public void Save(Stream outStream)
	{
		StreamWriter writer = new StreamWriter(outStream, GetOutEncoding());
		Save(writer);
	}

	public void Save(Stream outStream, Encoding encoding)
	{
		StreamWriter writer = new StreamWriter(outStream, encoding);
		Save(writer);
	}

	public void Save(string filename)
	{
		StreamWriter writer = new StreamWriter(filename, append: false, GetOutEncoding());
		Save(writer);
	}

	public void Save(string filename, Encoding encoding)
	{
		StreamWriter writer = new StreamWriter(filename, append: false, encoding);
		Save(writer);
	}

	public void Save(StreamWriter writer)
	{
		Save((TextWriter)writer);
	}

	public void Save(TextWriter writer)
	{
		writer.Flush();
	}

	public MixedCodeDocumentTextFragment CreateTextFragment()
	{
		return (MixedCodeDocumentTextFragment)CreateFragment(MixedCodeDocumentFragmentType.Text);
	}

	public MixedCodeDocumentCodeFragment CreateCodeFragment()
	{
		return (MixedCodeDocumentCodeFragment)CreateFragment(MixedCodeDocumentFragmentType.Code);
	}

	internal MixedCodeDocumentFragment CreateFragment(MixedCodeDocumentFragmentType type)
	{
		return type switch
		{
			MixedCodeDocumentFragmentType.Text => new MixedCodeDocumentTextFragment(this), 
			MixedCodeDocumentFragmentType.Code => new MixedCodeDocumentCodeFragment(this), 
			_ => throw new NotSupportedException(), 
		};
	}

	private void SetPosition()
	{
		_currentfragment._line = _line;
		_currentfragment._lineposition = _lineposition;
		_currentfragment._index = _index - 1;
		_currentfragment._length = 0;
	}

	private void IncrementPosition()
	{
		_index++;
		if (_c == 10)
		{
			_lineposition = 1;
			_line++;
		}
		else
		{
			_lineposition++;
		}
	}

	private void Parse()
	{
		_state = ParseState.Text;
		_index = 0;
		_currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Text);
		while (_index < _text.Length)
		{
			_c = _text[_index];
			IncrementPosition();
			switch (_state)
			{
			case ParseState.Text:
				if (_index + TokenCodeStart.Length < _text.Length && _text.Substring(_index - 1, TokenCodeStart.Length) == TokenCodeStart)
				{
					_state = ParseState.Code;
					_currentfragment._length = _index - 1 - _currentfragment._index;
					_currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Code);
					SetPosition();
				}
				break;
			case ParseState.Code:
				if (_index + TokenCodeEnd.Length < _text.Length && _text.Substring(_index - 1, TokenCodeEnd.Length) == TokenCodeEnd)
				{
					_state = ParseState.Text;
					_currentfragment._length = _index + TokenCodeEnd.Length - _currentfragment._index;
					_index += TokenCodeEnd.Length;
					_lineposition += TokenCodeEnd.Length;
					_currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Text);
					SetPosition();
				}
				break;
			}
		}
		_currentfragment._length = _index - _currentfragment._index;
	}
}
