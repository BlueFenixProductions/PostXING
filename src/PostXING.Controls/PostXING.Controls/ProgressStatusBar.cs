using System;
using System.Drawing;
using System.Windows.Forms;

namespace PostXING.Controls;

public class ProgressStatusBar : StatusBar
{
	private StatusBarPanel _message = new StatusBarPanel();

	private StatusBarPanel _progressPanel = new StatusBarPanel();

	private SmoothProgressBar _progress = new SmoothProgressBar();

	public StatusBarPanel Message => _message;

	public SmoothProgressBar Progress => _progress;

	public Color ProgressBarColor
	{
		get
		{
			return _progress.ProgressBarColor;
		}
		set
		{
			_progress.ProgressBarColor = value;
		}
	}

	public ProgressStatusBar()
	{
		SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, value: true);
		base.ShowPanels = true;
		_message.AutoSize = StatusBarPanelAutoSize.Spring;
		_message.BorderStyle = StatusBarPanelBorderStyle.Sunken;
		base.Panels.Add(_message);
		_progressPanel.BorderStyle = StatusBarPanelBorderStyle.None;
		_progressPanel.Width = 100;
		base.Panels.Add(_progressPanel);
		_progress.Parent = this;
		_postitionProgressBar();
	}

	private void _postitionProgressBar()
	{
		Size border3DSize = SystemInformation.Border3DSize;
		int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
		Rectangle bounds = new Rectangle(base.ClientSize.Width - _progressPanel.Width + border3DSize.Width, border3DSize.Height, _progressPanel.Width - 2 * border3DSize.Width, base.ClientSize.Height - border3DSize.Height);
		if (base.SizingGrip)
		{
			bounds.X -= verticalScrollBarWidth;
		}
		_progress.Bounds = bounds;
	}

	protected override void OnResize(EventArgs e)
	{
		_postitionProgressBar();
		base.OnResize(e);
	}
}
