using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PostXING.Controls.Controls;

public class ImageMapEditor : UITypeEditor
{
	private IWindowsFormsEditorService wfes;

	private int m_selectedIndex = -1;

	private ImageListPanel m_imagePanel;

	protected virtual ImageList GetImageList(object component)
	{
		if (component is MozItem.ImageCollection)
		{
			return ((MozItem.ImageCollection)component).GetImageList();
		}
		return null;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		wfes = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (wfes == null || context == null)
		{
			return null;
		}
		ImageList imageList = GetImageList(context.Instance);
		if (imageList == null || imageList.Images.Count == 0)
		{
			return -1;
		}
		m_imagePanel = new ImageListPanel();
		m_imagePanel.BackgroundColor = Color.FromArgb(241, 241, 241);
		m_imagePanel.BackgroundOverColor = Color.FromArgb(102, 154, 204);
		m_imagePanel.HLinesColor = Color.FromArgb(182, 189, 210);
		m_imagePanel.VLinesColor = Color.FromArgb(182, 189, 210);
		m_imagePanel.BorderColor = Color.FromArgb(0, 0, 0);
		m_imagePanel.EnableDragDrop = true;
		m_imagePanel.Init(imageList, 12, 12, 6, (int)value);
		m_imagePanel.ItemClick += OnItemClicked;
		m_selectedIndex = -1;
		wfes.DropDownControl(m_imagePanel);
		return (m_selectedIndex != -1) ? m_selectedIndex : ((int)value);
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		if (context != null && context.Instance != null)
		{
			return UITypeEditorEditStyle.DropDown;
		}
		return base.GetEditStyle(context);
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override void PaintValue(PaintValueEventArgs pe)
	{
		int num = -1;
		if (pe.Value != null)
		{
			try
			{
				num = Convert.ToUInt16(pe.Value.ToString());
			}
			catch
			{
			}
		}
		if (pe.Context.Instance != null && num >= 0)
		{
			ImageList imageList = GetImageList(pe.Context.Instance);
			if (imageList != null && imageList.Images.Count != 0 && num < imageList.Images.Count)
			{
				pe.Graphics.DrawImage(imageList.Images[num], pe.Bounds);
			}
		}
	}

	public void OnItemClicked(object sender, ImageListPanelEventArgs e)
	{
		m_selectedIndex = e.SelectedItem;
		m_imagePanel.ItemClick += OnItemClicked;
		wfes.CloseDropDown();
		m_imagePanel.Dispose();
		m_imagePanel = null;
	}
}
