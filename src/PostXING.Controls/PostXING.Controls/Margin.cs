using System;
using System.ComponentModel;

namespace PostXING.Controls;

[Serializable]
[TypeConverter(typeof(MarginConverter))]
public class Margin
{
	private int left;

	private int right;

	private int top;

	private int bottom;

	public int Left
	{
		get
		{
			return left;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Margin value cannot be negative");
			}
			left = value;
		}
	}

	public int Right
	{
		get
		{
			return right;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Margin value cannot be negative");
			}
			right = value;
		}
	}

	public int Top
	{
		get
		{
			return top;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Margin value cannot be negative");
			}
			top = value;
		}
	}

	public int Bottom
	{
		get
		{
			return bottom;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Margin value cannot be negative");
			}
			bottom = value;
		}
	}

	public Margin()
	{
		left = 0;
		right = 0;
		top = 0;
		bottom = 0;
	}

	public Margin(int left, int top, int right, int bottom)
	{
		this.left = left;
		this.right = right;
		this.top = top;
		this.bottom = bottom;
	}
}
