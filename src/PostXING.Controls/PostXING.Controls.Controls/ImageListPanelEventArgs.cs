using System;

namespace PostXING.Controls.Controls;

public class ImageListPanelEventArgs : EventArgs
{
	public int SelectedItem;

	public ImageListPanelEventArgs(int selectedItem)
	{
		SelectedItem = selectedItem;
	}
}
