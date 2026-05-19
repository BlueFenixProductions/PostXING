using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls.Controls;

[ToolboxItem(false)]
public class ImageListPanel : Control
{
	protected Bitmap _Bitmap;

	protected ImageList _imageList;

	protected int _nBitmapWidth;

	protected int _nBitmapHeight;

	protected int _nItemWidth;

	protected int _nItemHeight;

	protected int _nRows;

	protected int _nColumns;

	protected int _nHSpace;

	protected int _nVSpace;

	protected int _nCoordX = -1;

	protected int _nCoordY = -1;

	protected bool _bIsMouseDown;

	protected int _defaultImage;

	public Color BackgroundColor = Color.FromArgb(255, 255, 255);

	public Color BackgroundOverColor = Color.FromArgb(241, 238, 231);

	public Color HLinesColor = Color.FromArgb(222, 222, 222);

	public Color VLinesColor = Color.FromArgb(165, 182, 222);

	public Color BorderColor = Color.FromArgb(0, 16, 123);

	public bool EnableDragDrop;

	public event ImageListPanelEventHandler ItemClick;

	public bool Init(ImageList imageList, int nHSpace, int nVSpace, int nColumns, int defaultImage)
	{
		Brush brush = new SolidBrush(BackgroundColor);
		Pen pen = new Pen(VLinesColor);
		Pen pen2 = new Pen(HLinesColor);
		Pen pen3 = new Pen(BorderColor);
		_imageList = imageList;
		_nColumns = nColumns;
		_defaultImage = defaultImage;
		if (_defaultImage > _imageList.Images.Count)
		{
			_defaultImage = _imageList.Images.Count;
		}
		if (_defaultImage < 0)
		{
			_defaultImage = -1;
		}
		int num = imageList.Images.Count / _nColumns;
		if (imageList.Images.Count % _nColumns > 0)
		{
			num++;
		}
		_nRows = num;
		_nHSpace = nHSpace;
		_nVSpace = nVSpace;
		_nItemWidth = _imageList.ImageSize.Width + nHSpace;
		_nItemHeight = _imageList.ImageSize.Height + nVSpace;
		_nBitmapWidth = _nColumns * _nItemWidth + 1;
		_nBitmapHeight = _nRows * _nItemHeight + 1;
		base.Width = _nBitmapWidth;
		base.Height = _nBitmapHeight;
		_Bitmap = new Bitmap(_nBitmapWidth, _nBitmapHeight);
		Graphics graphics = Graphics.FromImage(_Bitmap);
		graphics.FillRectangle(brush, 0, 0, _nBitmapWidth, _nBitmapHeight);
		for (int i = 0; i < _nColumns; i++)
		{
			graphics.DrawLine(pen, i * _nItemWidth, 0, i * _nItemWidth, _nBitmapHeight - 1);
		}
		for (int j = 0; j < _nRows; j++)
		{
			graphics.DrawLine(pen2, 0, j * _nItemHeight, _nBitmapWidth - 1, j * _nItemHeight);
		}
		graphics.DrawRectangle(pen3, 0, 0, _nBitmapWidth - 1, _nBitmapHeight - 1);
		for (int k = 0; k < _nColumns; k++)
		{
			for (int l = 0; l < _nRows; l++)
			{
				if (l * _nColumns + k < imageList.Images.Count)
				{
					imageList.Draw(graphics, k * _nItemWidth + _nHSpace / 2, l * _nItemHeight + nVSpace / 2, imageList.ImageSize.Width, imageList.ImageSize.Height, l * _nColumns + k);
				}
			}
		}
		brush.Dispose();
		pen.Dispose();
		pen2.Dispose();
		pen3.Dispose();
		Invalidate();
		return true;
	}

	public void Show(int x, int y)
	{
		base.Left = x;
		base.Top = y;
		Show();
	}

	protected override void OnMouseLeave(EventArgs ea)
	{
		base.OnMouseLeave(ea);
		_nCoordX = -1;
		_nCoordY = -1;
		Invalidate();
	}

	protected override void OnKeyDown(KeyEventArgs kea)
	{
		if (_nCoordX == -1 || _nCoordY == -1)
		{
			_nCoordX = 0;
			_nCoordY = 0;
			Invalidate();
			return;
		}
		switch (kea.KeyCode)
		{
		case Keys.Down:
			if (_nCoordY < _nRows - 1)
			{
				_nCoordY++;
				Invalidate();
			}
			break;
		case Keys.Up:
			if (_nCoordY > 0)
			{
				_nCoordY--;
				Invalidate();
			}
			break;
		case Keys.Right:
			if (_nCoordX < _nColumns - 1)
			{
				_nCoordX++;
				Invalidate();
			}
			break;
		case Keys.Left:
			if (_nCoordX > 0)
			{
				_nCoordX--;
				Invalidate();
			}
			break;
		case Keys.Return:
		case Keys.Space:
		{
			int num = _nCoordY * _nColumns + _nCoordX;
			if (this.ItemClick != null && num >= 0 && num < _imageList.Images.Count)
			{
				this.ItemClick(this, new ImageListPanelEventArgs(num));
				_nCoordX = -1;
				_nCoordY = -1;
				Hide();
			}
			break;
		}
		case Keys.Escape:
			_nCoordX = -1;
			_nCoordY = -1;
			Hide();
			break;
		}
	}

	protected override void OnMouseMove(MouseEventArgs mea)
	{
		if (base.ClientRectangle.Contains(new Point(mea.X, mea.Y)))
		{
			if (EnableDragDrop && _bIsMouseDown)
			{
				int num = _nCoordY * _nColumns + _nCoordX;
				if (num <= _imageList.Images.Count - 1)
				{
					DataObject dataObject = new DataObject();
					dataObject.SetData(DataFormats.Text, num.ToString());
					dataObject.SetData(DataFormats.Bitmap, _imageList.Images[num]);
					try
					{
						DoDragDrop(dataObject, DragDropEffects.Copy | DragDropEffects.Move);
					}
					catch
					{
					}
					_bIsMouseDown = false;
				}
			}
			if (mea.X / _nItemWidth != _nCoordX || mea.Y / _nItemHeight != _nCoordY)
			{
				_nCoordX = mea.X / _nItemWidth;
				_nCoordY = mea.Y / _nItemHeight;
				Invalidate();
			}
		}
		else
		{
			_nCoordX = -1;
			_nCoordY = -1;
			Invalidate();
		}
		base.OnMouseMove(mea);
	}

	protected override void OnMouseDown(MouseEventArgs mea)
	{
		base.OnMouseDown(mea);
		_bIsMouseDown = true;
		Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs mea)
	{
		base.OnMouseDown(mea);
		_bIsMouseDown = false;
		int num = _nCoordY * _nColumns + _nCoordX;
		if (this.ItemClick != null && num >= 0 && num < _imageList.Images.Count)
		{
			this.ItemClick(this, new ImageListPanelEventArgs(num));
			Hide();
		}
	}

	protected override void OnPaintBackground(PaintEventArgs pea)
	{
		Graphics graphics = pea.Graphics;
		graphics.PageUnit = GraphicsUnit.Pixel;
		Bitmap image = new Bitmap(_nBitmapWidth, _nBitmapHeight);
		Graphics graphics2 = Graphics.FromImage(image);
		graphics2.DrawImage(_Bitmap, 0, 0);
		if (_nCoordX != -1 && _nCoordY != -1 && _nCoordY * _nColumns + _nCoordX < _imageList.Images.Count)
		{
			graphics2.FillRectangle(new SolidBrush(BackgroundOverColor), _nCoordX * _nItemWidth + 1, _nCoordY * _nItemHeight + 1, _nItemWidth - 1, _nItemHeight - 1);
			if (_bIsMouseDown)
			{
				_imageList.Draw(graphics2, _nCoordX * _nItemWidth + _nHSpace / 2 + 1, _nCoordY * _nItemHeight + _nVSpace / 2 + 1, _imageList.ImageSize.Width, _imageList.ImageSize.Height, _nCoordY * _nColumns + _nCoordX);
			}
			else
			{
				_imageList.Draw(graphics2, _nCoordX * _nItemWidth + _nHSpace / 2, _nCoordY * _nItemHeight + _nVSpace / 2, _imageList.ImageSize.Width, _imageList.ImageSize.Height, _nCoordY * _nColumns + _nCoordX);
			}
			graphics2.DrawRectangle(new Pen(BorderColor), _nCoordX * _nItemWidth, _nCoordY * _nItemHeight, _nItemWidth, _nItemHeight);
		}
		graphics.DrawImage(image, 0, 0);
		graphics2.Dispose();
	}
}
