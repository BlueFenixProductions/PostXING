using System;

namespace PostXING.Components;

public class PostSelectedEventArgs : EventArgs
{
	private bool _selectedForEditing;

	public readonly BlogPost SelectedPost;

	public bool SelectedForEditing
	{
		get
		{
			return _selectedForEditing;
		}
		set
		{
			_selectedForEditing = value;
		}
	}

	public PostSelectedEventArgs(bool selectedForEditing, BlogPost selectedPost)
	{
		_selectedForEditing = selectedForEditing;
		SelectedPost = selectedPost;
	}
}
