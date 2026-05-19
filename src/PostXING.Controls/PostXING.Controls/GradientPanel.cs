using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PostXING.Controls;

[ToolboxBitmap(typeof(Panel))]
public class GradientPanel : Panel
{
	private LinearGradientBrush _lgb;

	private Color _gradientColor;

	private float _rotation;

	public Color GradientColor
	{
		get
		{
			return _gradientColor;
		}
		set
		{
			_gradientColor = value;
		}
	}

	public float Rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			_rotation = value;
		}
	}

	private void createBrush()
	{
		if (base.ClientRectangle.IsEmpty)
		{
			return;
		}
		Color gradientColor = SystemColors.ControlDark;
		Color backColor = SystemColors.ControlLight;
		if (UxTheme.AppThemed && UxTheme.ThemeName.ToLower().EndsWith("luna.msstyles"))
		{
			switch (UxTheme.ColorName.ToLower())
			{
			case "metallic":
				gradientColor = Color.FromArgb(243, 243, 247);
				backColor = Color.FromArgb(215, 215, 229);
				break;
			case "normalcolor":
				gradientColor = Color.FromArgb(195, 218, 249);
				backColor = Color.FromArgb(158, 190, 245);
				break;
			case "homestead":
				gradientColor = Color.FromArgb(242, 240, 227);
				backColor = Color.FromArgb(217, 217, 167);
				break;
			}
			BackColor = backColor;
			GradientColor = gradientColor;
			_lgb = new LinearGradientBrush(base.ClientRectangle, BackColor, GradientColor, LinearGradientMode.Horizontal);
		}
		else
		{
			_lgb = new LinearGradientBrush(base.ClientRectangle, BackColor, GradientColor, Rotation);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!e.ClipRectangle.IsEmpty)
		{
			createBrush();
			using (_lgb)
			{
				e.Graphics.FillRectangle(_lgb, base.ClientRectangle);
			}
			base.OnPaint(e);
		}
	}
}
