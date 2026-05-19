using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PostXING.Controls.MozBar;

namespace PostXING.Controls.Controls;

[ToolboxBitmap(typeof(MozPane), "PostXING.Controls.MozBar.MozPane.bmp")]
[Designer(typeof(MozPaneDesigner))]
[ToolboxItem(true)]
public class MozPane : ScrollableControlWithScrollEvents, ISupportInitialize
{
	public class MozItemCollection : CollectionBase
	{
		private MozPane owner;

		public virtual MozItem this[int index] => base.List[index] as MozItem;

		public MozItemCollection(MozPane owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
		}

		public MozItemCollection(MozPane owner, MozItemCollection mozItems)
			: this(owner)
		{
			Add(mozItems);
		}

		public void Add(MozItem value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			base.List.Add(value);
			if (owner != null && !owner.deserializing)
			{
				owner.Controls.Add(value);
				owner.OnMozItemAdded(new MozItemEventArgs(value));
			}
		}

		public void AddRange(MozItem[] mozItems)
		{
			if (mozItems == null)
			{
				throw new ArgumentNullException("mozItems");
			}
			for (int i = 0; i < mozItems.Length; i++)
			{
				Add(mozItems[i]);
			}
		}

		public void Add(MozItemCollection mozItems)
		{
			if (mozItems == null)
			{
				throw new ArgumentNullException("mozItems");
			}
			for (int i = 0; i < mozItems.Count; i++)
			{
				Add(mozItems[i]);
			}
		}

		public new void Clear()
		{
			while (base.Count > 0)
			{
				RemoveAt(0);
			}
		}

		public bool Contains(MozItem mozItem)
		{
			if (mozItem == null)
			{
				throw new ArgumentNullException("mozItem");
			}
			return IndexOf(mozItem) != -1;
		}

		public bool Contains(Control control)
		{
			if (!(control is MozItem))
			{
				return false;
			}
			return Contains((MozItem)control);
		}

		public int IndexOf(MozItem mozItem)
		{
			if (mozItem == null)
			{
				throw new ArgumentNullException("mozItem");
			}
			for (int i = 0; i < base.Count; i++)
			{
				if (this[i] == mozItem)
				{
					return i;
				}
			}
			return -1;
		}

		public void Remove(MozItem value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			base.List.Remove(value);
			if (owner != null && !owner.deserializing)
			{
				owner.Controls.Remove(value);
				owner.OnMozItemRemoved(new MozItemEventArgs(value));
			}
		}

		public new void RemoveAt(int index)
		{
			Remove(this[index]);
		}

		public void Move(MozItem value, int index)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (index < 0)
			{
				index = 0;
			}
			else if (index > base.Count)
			{
				index = base.Count;
			}
			if (Contains(value) && IndexOf(value) != index)
			{
				base.List.Remove(value);
				if (index > base.Count)
				{
					base.List.Add(value);
				}
				else
				{
					base.List.Insert(index, value);
				}
				if (owner != null && !owner.deserializing)
				{
					owner.MatchControlCollToMozItemsColl();
				}
			}
		}

		public void MoveToTop(MozItem value)
		{
			Move(value, 0);
		}

		public void MoveToBottom(MozItem value)
		{
			Move(value, base.Count);
		}
	}

	internal class MozItemCollectionEditor : CollectionEditor
	{
		public MozItemCollectionEditor(Type type)
			: base(type)
		{
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider isp, object value)
		{
			MozPane mozPane = (MozPane)context.Instance;
			object result = base.EditValue(context, isp, value);
			mozPane.UpdateMozItems();
			return result;
		}

		protected override object CreateInstance(Type itemType)
		{
			object obj = base.CreateInstance(itemType);
			((MozItem)obj).Name = "MozItem";
			return obj;
		}
	}

	[TypeConverter(typeof(PaddingCollectionTypeConverter))]
	public class PaddingCollection
	{
		private MozPane m_pane;

		private int m_horizontal;

		private int m_vertical;

		[Description("Horizontal padding.")]
		[RefreshProperties(RefreshProperties.All)]
		public int Horizontal
		{
			get
			{
				return m_horizontal;
			}
			set
			{
				if (m_horizontal == value)
				{
					return;
				}
				m_horizontal = value;
				if (m_pane != null)
				{
					m_pane.DoLayout();
					m_pane.Invalidate();
					if (m_pane.PaddingChanged != null)
					{
						m_pane.PaddingChanged(this, new EventArgs());
					}
				}
			}
		}

		[Description("Vertical padding.")]
		[RefreshProperties(RefreshProperties.All)]
		public int Vertical
		{
			get
			{
				return m_vertical;
			}
			set
			{
				if (m_vertical == value)
				{
					return;
				}
				m_vertical = value;
				if (m_pane != null)
				{
					m_pane.DoLayout();
					m_pane.Invalidate();
					if (m_pane.PaddingChanged != null)
					{
						m_pane.PaddingChanged(this, new EventArgs());
					}
				}
			}
		}

		public PaddingCollection(MozPane pane)
		{
			m_pane = pane;
			m_horizontal = 2;
			m_vertical = 2;
		}
	}

	public class PaddingCollectionTypeConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				string[] array = value.ToString().Split(new char[1] { ';' }, 2);
				if (array.Length == 2)
				{
					PaddingCollection paddingCollection = new PaddingCollection((MozPane)context.Instance);
					paddingCollection.Horizontal = int.Parse(array[0]);
					paddingCollection.Vertical = int.Parse(array[1]);
					return paddingCollection;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is PaddingCollection)
			{
				PaddingCollection paddingCollection = (PaddingCollection)value;
				return paddingCollection.Horizontal + "; " + paddingCollection.Vertical;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	[TypeConverter(typeof(ItemCollectionTypeConverter))]
	public class ColorCollection
	{
		private Color m_selectedColor;

		private Color m_selectedBorderColor;

		private Color m_focusColor;

		private Color m_focusBorderColor;

		private Color m_textColor;

		private Color m_backColor;

		private Color m_borderColor;

		private Color m_dividerColor;

		private Color m_selectedText;

		private Color m_focusText;

		public MozPane m_pane;

		[Description("Color used for item text.")]
		public Color Text
		{
			get
			{
				return m_textColor;
			}
			set
			{
				if (m_textColor != value)
				{
					m_textColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.Text));
					}
				}
			}
		}

		[Description("Text color when item is selected.")]
		public Color SelectedText
		{
			get
			{
				return m_selectedText;
			}
			set
			{
				if (m_selectedText != value)
				{
					m_selectedText = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.SelectedText));
					}
				}
			}
		}

		[Description("Text color when item has focus.")]
		public Color FocusText
		{
			get
			{
				return m_focusText;
			}
			set
			{
				if (m_focusText != value)
				{
					m_focusText = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.FocusText));
					}
				}
			}
		}

		[Description("Background color when item is not selected or has focus.")]
		public Color Background
		{
			get
			{
				return m_backColor;
			}
			set
			{
				if (m_backColor != value)
				{
					m_backColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.Background));
					}
				}
			}
		}

		[Description("Border color when item is not selected or has focus.")]
		public Color Border
		{
			get
			{
				return m_borderColor;
			}
			set
			{
				if (m_borderColor != value)
				{
					m_borderColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.Border));
					}
				}
			}
		}

		[Description("Color used when item style is set to divider.")]
		public Color Divider
		{
			get
			{
				return m_dividerColor;
			}
			set
			{
				if (m_dividerColor != value)
				{
					m_dividerColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.Divider));
					}
				}
			}
		}

		[Description("Background color when item is selected.")]
		public Color SelectedBackground
		{
			get
			{
				return m_selectedColor;
			}
			set
			{
				if (m_selectedColor != value)
				{
					m_selectedColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.SelectedBackground));
					}
				}
			}
		}

		[Description("Border color when item is selected.")]
		public Color SelectedBorder
		{
			get
			{
				return m_selectedBorderColor;
			}
			set
			{
				if (m_selectedBorderColor != value)
				{
					m_selectedBorderColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.SelectedBorder));
					}
				}
			}
		}

		[Description("Background color when item has focus.")]
		public Color FocusBackground
		{
			get
			{
				return m_focusColor;
			}
			set
			{
				if (m_focusColor != value)
				{
					m_focusColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.FocusBackground));
					}
				}
			}
		}

		[Description("Border color when item has focus.")]
		public Color FocusBorder
		{
			get
			{
				return m_focusBorderColor;
			}
			set
			{
				if (m_focusBorderColor != value)
				{
					m_focusBorderColor = value;
					UpdateItems();
					if (m_pane.ItemColorChanged != null)
					{
						m_pane.ItemColorChanged(this, new ItemColorChangedEventArgs(MozItemColor.FocusBorder));
					}
				}
			}
		}

		public ColorCollection(MozPane pane)
		{
			m_pane = pane;
			m_selectedColor = Color.FromArgb(193, 210, 238);
			m_selectedBorderColor = Color.FromArgb(49, 106, 197);
			m_focusColor = Color.FromArgb(224, 232, 246);
			m_focusBorderColor = Color.FromArgb(152, 180, 226);
			m_backColor = Color.White;
			m_borderColor = Color.Black;
			m_textColor = Color.Black;
			m_selectedText = Color.Black;
			m_focusText = Color.Black;
			m_dividerColor = Color.FromArgb(127, 157, 185);
		}

		public void GetSystemColors()
		{
			Color color = (m_selectedColor = SystemColors.Highlight);
			m_selectedBorderColor = ControlPaint.Dark(color);
			m_focusColor = ControlPaint.Light(color);
			m_focusBorderColor = color;
			m_borderColor = Color.Black;
			m_textColor = Color.Black;
			m_selectedText = Color.Black;
			m_focusText = Color.Black;
			m_dividerColor = Color.FromArgb(127, 157, 185);
		}

		public void Dispose()
		{
		}

		private void UpdateItems()
		{
			for (int i = 0; i < m_pane.Items.Count; i++)
			{
				m_pane.Items[i].Invalidate();
			}
		}
	}

	[TypeConverter(typeof(ItemCollectionTypeConverter))]
	public class BorderStyleCollection
	{
		public MozPane m_pane;

		private ButtonBorderStyle m_borderStyle;

		private ButtonBorderStyle m_focusBorderStyle;

		private ButtonBorderStyle m_selectedBorderStyle;

		[Description("Border style when item has no focus.")]
		public ButtonBorderStyle Normal
		{
			get
			{
				return m_borderStyle;
			}
			set
			{
				if (m_borderStyle != value)
				{
					m_borderStyle = value;
					UpdateItems();
					if (m_pane.ItemBorderStyleChanged != null)
					{
						m_pane.ItemBorderStyleChanged(this, new ItemBorderStyleChangedEventArgs(MozItemState.Normal));
					}
				}
			}
		}

		[Description("Border style when item has focus.")]
		public ButtonBorderStyle Focus
		{
			get
			{
				return m_focusBorderStyle;
			}
			set
			{
				if (m_focusBorderStyle != value)
				{
					m_focusBorderStyle = value;
					UpdateItems();
					if (m_pane.ItemBorderStyleChanged != null)
					{
						m_pane.ItemBorderStyleChanged(this, new ItemBorderStyleChangedEventArgs(MozItemState.Focus));
					}
				}
			}
		}

		[Description("Border style when item is selected.")]
		public ButtonBorderStyle Selected
		{
			get
			{
				return m_selectedBorderStyle;
			}
			set
			{
				if (m_selectedBorderStyle != value)
				{
					m_selectedBorderStyle = value;
					UpdateItems();
					if (m_pane.ItemBorderStyleChanged != null)
					{
						m_pane.ItemBorderStyleChanged(this, new ItemBorderStyleChangedEventArgs(MozItemState.Selected));
					}
				}
			}
		}

		public BorderStyleCollection(MozPane pane)
		{
			m_pane = pane;
			m_borderStyle = ButtonBorderStyle.None;
			m_focusBorderStyle = ButtonBorderStyle.Solid;
			m_selectedBorderStyle = ButtonBorderStyle.Solid;
		}

		public void Dispose()
		{
		}

		private void UpdateItems()
		{
			for (int i = 0; i < m_pane.Items.Count; i++)
			{
				m_pane.Items[i].Invalidate();
			}
		}
	}

	private const int WM_THEMECHANGED = 794;

	private const int WM_KEYDOWN = 256;

	private const int SB_HORZ = 0;

	private const int SB_VERT = 1;

	private const int SB_CTL = 2;

	private const int SB_BOTH = 3;

	private const int WM_NCCALCSIZE = 131;

	private const int WS_HSCROLL = 1048576;

	private const int WS_VSCROLL = 2097152;

	private const int WM_HSCROLL = 276;

	private const int WM_VSCROLL = 277;

	private const int GWL_STYLE = -16;

	private const int SB_LINEUP = 0;

	private const int SB_LINEDOWN = 1;

	private const int SB_PAGEUP = 2;

	private const int SB_PAGEDOWN = 3;

	private const int SB_THUMBPOSITION = 4;

	private const int SB_THUMBTRACK = 5;

	private const int SB_ENDSCROLL = 8;

	private Container components;

	private bool layout;

	private int beginUpdateCount;

	internal bool deserializing;

	internal bool initialising;

	private int m_tabIndex = -1;

	private MozItem m_mouseOverItem;

	private Color m_borderColor;

	private PaddingCollection m_padding;

	private ColorCollection m_colorCollection;

	private BorderStyleCollection m_borderStyleCollection;

	private MozSelectButton m_selectButton;

	private PostXING.Controls.MozBar.ThemeManager m_themeManager;

	private MozPaneStyle m_style;

	private IntPtr m_theme;

	private bool m_useTheme;

	private bool m_toggle;

	private int m_maxSelectedItems;

	private int m_selectedItems;

	private ButtonBorderStyle m_borderStyle;

	private ImageList m_imageList;

	private MozItemCollection m_mozItemCollection;

	[Description("Mouse button used to select items.")]
	[Category("Behavior")]
	public MozSelectButton SelectButton
	{
		get
		{
			return m_selectButton;
		}
		set
		{
			if (m_selectButton != value)
			{
				m_selectButton = value;
				if (this.SelectButtonChanged != null)
				{
					this.SelectButtonChanged(this, new EventArgs());
				}
			}
		}
	}

	[Description("Indicates wether the control should use the current theme.")]
	[Category("Behavior")]
	public bool Theme
	{
		get
		{
			return m_useTheme;
		}
		set
		{
			if (m_useTheme != value)
			{
				m_useTheme = value;
				if (this.UseThemeChanged != null)
				{
					this.UseThemeChanged(this, new EventArgs());
				}
				if (m_useTheme)
				{
					GetThemeColors();
				}
			}
		}
	}

	[Browsable(true)]
	[RefreshProperties(RefreshProperties.All)]
	[Category("Behavior")]
	[Description("Indicates the possibility to toggle items i.e. unselect selected items.")]
	public bool Toggle
	{
		get
		{
			return m_toggle;
		}
		set
		{
			if (value != m_toggle)
			{
				m_toggle = value;
				if (!value)
				{
					MaxSelectedItems = 1;
				}
				if (this.ToggleChanged != null)
				{
					this.ToggleChanged(this, new EventArgs());
				}
			}
		}
	}

	[Description("Max number of selected items.")]
	[Browsable(true)]
	[Category("Behavior")]
	public int MaxSelectedItems
	{
		get
		{
			return m_maxSelectedItems;
		}
		set
		{
			if (value != m_maxSelectedItems)
			{
				if (value < 1)
				{
					value = 1;
				}
				if (value > Items.Count)
				{
					value = Items.Count;
				}
				m_maxSelectedItems = value;
				if (this.MaxSelectedItemsChanged != null)
				{
					this.MaxSelectedItemsChanged(this, new EventArgs());
				}
			}
		}
	}

	[Browsable(false)]
	public int SelectedItems => m_selectedItems;

	[Category("Appearance")]
	[Description("Colors for various states.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public ColorCollection ItemColors
	{
		get
		{
			return m_colorCollection;
		}
		set
		{
			if (value != null)
			{
				m_colorCollection = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Various border styles.")]
	[Browsable(true)]
	[Category("Appearance")]
	public BorderStyleCollection ItemBorderStyles
	{
		get
		{
			return m_borderStyleCollection;
		}
		set
		{
			if (value != null && value != m_borderStyleCollection)
			{
				m_borderStyleCollection = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	[Description("Padding (Horizontal, Vertical)")]
	[Browsable(true)]
	public new PaddingCollection Padding
	{
		get
		{
			return m_padding;
		}
		set
		{
			if (value != m_padding)
			{
				if (value != null)
				{
					m_padding = value;
				}
				DoLayout();
				Invalidate();
				if (this.PaddingChanged != null)
				{
					this.PaddingChanged(this, new EventArgs());
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Pane style.")]
	public MozPaneStyle Style
	{
		get
		{
			return m_style;
		}
		set
		{
			if (m_style != value)
			{
				m_style = value;
				for (int i = 0; i < Items.Count; i++)
				{
					Items[i].DoLayout();
					Invalidate();
				}
				DoLayout();
				Invalidate();
				if (this.PaneStyleChanged != null)
				{
					this.PaneStyleChanged(this, new EventArgs());
				}
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("")]
	public ImageList ImageList
	{
		get
		{
			return m_imageList;
		}
		set
		{
			if (m_imageList != value)
			{
				m_imageList = value;
				for (int i = 0; i < Items.Count; i++)
				{
					Items[i].Images.Normal = -1;
					Items[i].Images.Focus = -1;
					Items[i].Images.Selected = -1;
					Items[i].DoLayout();
					Invalidate();
				}
				if (this.ImageListChanged != null)
				{
					this.ImageListChanged(this, new EventArgs());
				}
				DoLayout();
				Invalidate();
			}
		}
	}

	[Category("Appearance")]
	[Description("Border color for panel.")]
	[Browsable(true)]
	public ButtonBorderStyle BorderStyle
	{
		get
		{
			return m_borderStyle;
		}
		set
		{
			if (m_borderStyle != value)
			{
				m_borderStyle = value;
				Invalidate();
			}
		}
	}

	[Description("Border color for panel.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color BorderColor
	{
		get
		{
			return m_borderColor;
		}
		set
		{
			if (value != m_borderColor)
			{
				m_borderColor = value;
				if (this.BorderColorChanged != null)
				{
					this.BorderColorChanged(this, new EventArgs());
				}
				Invalidate();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("The Items contained in the Panel.")]
	[Editor(typeof(MozItemCollectionEditor), typeof(UITypeEditor))]
	[Category("Behavior")]
	[DefaultValue(null)]
	public MozItemCollection Items => m_mozItemCollection;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new ControlCollection Controls => base.Controls;

	[Obsolete("This property is not supported", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return base.BackgroundImage;
		}
		set
		{
			base.BackgroundImage = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[Obsolete("This property is not supported", true)]
	public override RightToLeft RightToLeft
	{
		get
		{
			return base.RightToLeft;
		}
		set
		{
			base.RightToLeft = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This property is not supported", true)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This property is not supported", true)]
	public override Color ForeColor
	{
		get
		{
			return base.ForeColor;
		}
		set
		{
			base.ForeColor = value;
		}
	}

	[Category("Property Changed")]
	[Description("Indicates that an item color has changed.")]
	[Browsable(true)]
	public event ItemColorChangedEventHandler ItemColorChanged;

	[Browsable(true)]
	[Category("Property Changed")]
	[Description("Indicates that an item borderstyle has changed.")]
	public event ItemBorderStyleChangedEventHandler ItemBorderStyleChanged;

	[Category("Property Changed")]
	[Browsable(true)]
	[Description("Indicates that the border color was changed.")]
	public event EventHandler BorderColorChanged;

	[Description("Indicates that the Theme setting was changed.")]
	[Browsable(true)]
	[Category("Property Changed")]
	public event EventHandler UseThemeChanged;

	[Category("Property Changed")]
	[Description("Indicates that the select button was changed.")]
	[Browsable(true)]
	public event EventHandler SelectButtonChanged;

	[Description("Indicates that the imagelist has has been changed.")]
	[Browsable(true)]
	[Category("Property Changed")]
	public event EventHandler ImageListChanged;

	[Description("Indicates that the style has been changed.")]
	[Browsable(true)]
	[Category("Property Changed")]
	public event EventHandler PaneStyleChanged;

	[Description("Indicates that the toggle property has been changed.")]
	[Browsable(true)]
	[Category("Property Changed")]
	public event EventHandler ToggleChanged;

	[Description("Indicates the max allowed number of selected items has changed.")]
	[Browsable(true)]
	[Category("Property Changed")]
	public event EventHandler MaxSelectedItemsChanged;

	[Category("Property Changed")]
	[Description("Indicates that the padding has has been changed.")]
	[Browsable(true)]
	public new event EventHandler PaddingChanged;

	[Description("Indicates that an item was added to the panel.")]
	[Browsable(true)]
	[Category("Panel")]
	public event MozItemEventHandler ItemAdded;

	[Browsable(true)]
	[Category("Panel")]
	[Description("Indicates that an item was removed from the panel.")]
	public event MozItemEventHandler ItemRemoved;

	[Browsable(true)]
	[Category("Action")]
	[Description("Indicates that an item was selected.")]
	public event MozItemEventHandler ItemSelected;

	[Browsable(true)]
	[Category("Action")]
	[Description("Indicates that an item was unselected.")]
	public event MozItemEventHandler ItemDeselected;

	[Browsable(true)]
	[Category("Action")]
	[Description("Indicates that an item lost focus.")]
	public event MozItemEventHandler ItemLostFocus;

	[Browsable(true)]
	[Category("Action")]
	[Description("Indicates that an item recieved focus.")]
	public event MozItemEventHandler ItemGotFocus;

	[Browsable(true)]
	[Category("Action")]
	[Description("Indicates that an item has been double clicked.")]
	public event MozItemClickEventHandler ItemDoubleClick;

	[Description("Indicates that an item has been clicked.")]
	[Browsable(true)]
	[Category("Action")]
	public event MozItemClickEventHandler ItemClick;

	[DllImport("user32.dll")]
	private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

	[DllImport("user32", CharSet = CharSet.Auto)]
	private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

	[DllImport("user32")]
	public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

	public MozPane()
	{
		InitializeComponent();
		components = new Container();
		SetStyle(ControlStyles.DoubleBuffer, value: true);
		SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
		SetStyle(ControlStyles.UserPaint, value: true);
		SetStyle(ControlStyles.ResizeRedraw, value: true);
		SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
		m_mozItemCollection = new MozItemCollection(this);
		AutoScroll = true;
		m_padding = new PaddingCollection(this);
		m_colorCollection = new ColorCollection(this);
		m_borderStyleCollection = new BorderStyleCollection(this);
		m_themeManager = new PostXING.Controls.MozBar.ThemeManager();
		m_selectButton = MozSelectButton.Left;
		m_style = MozPaneStyle.Vertical;
		m_toggle = false;
		m_maxSelectedItems = 1;
		m_selectedItems = 0;
		m_useTheme = false;
		m_theme = IntPtr.Zero;
		base.ParentChanged += OnParentChanged;
		beginUpdateCount = 0;
		deserializing = false;
		initialising = false;
		m_borderColor = Color.FromArgb(127, 157, 185);
		BackColor = Color.White;
		m_borderStyle = ButtonBorderStyle.Solid;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (components != null)
			{
				components.Dispose();
			}
			m_colorCollection.Dispose();
			m_borderStyleCollection.Dispose();
		}
		base.Dispose(disposing);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		base.OnGotFocus(e);
		if (m_tabIndex == -1)
		{
			m_tabIndex = 0;
		}
		RemoveFocus();
		if (Items.Count >= 1 && Items[m_tabIndex].state != MozItemState.Selected)
		{
			Items[m_tabIndex].state = MozItemState.Focus;
			ScrollIntoView(m_tabIndex, 1);
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		base.OnLostFocus(e);
		RemoveFocus();
		m_tabIndex = -1;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e);
		switch (e.KeyCode)
		{
		case Keys.Return:
		case Keys.Space:
			if (m_tabIndex != -1)
			{
				SelectItem(m_tabIndex);
			}
			break;
		case Keys.Tab:
		case Keys.Right:
		case Keys.Down:
			m_tabIndex++;
			if (m_tabIndex < Items.Count && m_tabIndex > 0)
			{
				RemoveFocus();
				if (Items[m_tabIndex].state != MozItemState.Selected)
				{
					Items[m_tabIndex].state = MozItemState.Focus;
				}
				ScrollIntoView(m_tabIndex, 0);
			}
			break;
		case Keys.Left:
		case Keys.Up:
			m_tabIndex--;
			if (m_tabIndex >= 0 && m_tabIndex < Items.Count)
			{
				RemoveFocus();
			}
			if (Items[m_tabIndex].state != MozItemState.Selected)
			{
				Items[m_tabIndex].state = MozItemState.Focus;
			}
			ScrollIntoView(m_tabIndex, 1);
			break;
		}
	}

	public override bool PreProcessMessage(ref Message msg)
	{
		if (msg.Msg == 256)
		{
			_ = (int)msg.WParam;
			_ = Control.ModifierKeys;
			switch ((Keys)(int)msg.WParam)
			{
			case Keys.Tab:
			case Keys.Right:
			case Keys.Down:
				if (m_tabIndex < Items.Count - 1)
				{
					return false;
				}
				m_tabIndex = -1;
				break;
			case Keys.Left:
			case Keys.Up:
				if (m_tabIndex > 0)
				{
					return false;
				}
				m_tabIndex = -1;
				break;
			}
		}
		return base.PreProcessMessage(ref msg);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		base.OnSystemColorsChanged(e);
		if (Theme)
		{
			GetThemeColors();
		}
	}

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		switch (m.Msg)
		{
		case 794:
			if (Theme)
			{
				GetThemeColors();
			}
			break;
		case 131:
			if (Style == MozPaneStyle.Vertical)
			{
				ShowScrollBar(m.HWnd, 0, 0);
			}
			else
			{
				ShowScrollBar(m.HWnd, 1, 0);
			}
			break;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		ButtonBorderStyle leftStyle = m_borderStyle;
		ButtonBorderStyle topStyle = m_borderStyle;
		ButtonBorderStyle rightStyle = m_borderStyle;
		ButtonBorderStyle bottomStyle = m_borderStyle;
		base.OnPaint(e);
		Rectangle rectangle = default(Rectangle);
		rectangle = DisplayRectangle;
		Brush brush = new SolidBrush(BackColor);
		e.Graphics.FillRectangle(brush, DisplayRectangle);
		if (Style == MozPaneStyle.Vertical)
		{
			if (IsVerticalScrollBarVisible())
			{
				topStyle = ButtonBorderStyle.None;
				bottomStyle = ButtonBorderStyle.None;
			}
		}
		else if (IsHorizontalScrollBarVisible())
		{
			leftStyle = ButtonBorderStyle.None;
			rightStyle = ButtonBorderStyle.None;
		}
		ControlPaint.DrawBorder(e.Graphics, rectangle, m_borderColor, 1, leftStyle, m_borderColor, 1, topStyle, m_borderColor, 1, rightStyle, m_borderColor, 1, bottomStyle);
		brush.Dispose();
	}

	private void OnParentChanged(object sender, EventArgs e)
	{
		if (base.Parent != null)
		{
			base.Parent.VisibleChanged += OnParentVisibleChanged;
		}
	}

	private void OnParentVisibleChanged(object sender, EventArgs e)
	{
		if (sender != base.Parent)
		{
			((Control)sender).VisibleChanged -= OnParentVisibleChanged;
		}
		else if (base.Parent.Visible)
		{
			DoLayout();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		DoLayout();
		Invalidate();
	}

	private void MozItem_GotFocus(object sender, MozItemEventArgs e)
	{
		if (e.MozItem.state != MozItemState.Selected)
		{
			e.MozItem.state = MozItemState.Focus;
			m_mouseOverItem = e.MozItem;
			if (this.ItemGotFocus != null)
			{
				this.ItemGotFocus(this, e);
			}
		}
	}

	private void MozItem_LostFocus(object sender, MozItemEventArgs e)
	{
		if (e.MozItem.state != MozItemState.Selected)
		{
			e.MozItem.state = MozItemState.Normal;
			m_mouseOverItem = null;
			if (this.ItemLostFocus != null)
			{
				this.ItemLostFocus(this, e);
			}
		}
	}

	private void MozItem_Click(object sender, MozItemClickEventArgs e)
	{
		if (this.ItemClick != null)
		{
			this.ItemClick(this, e);
		}
		if (e.Button.ToString() == SelectButton.ToString() || SelectButton == MozSelectButton.Any)
		{
			m_tabIndex = Items.IndexOf(e.MozItem);
			Focus();
			SelectItem(Items.IndexOf(e.MozItem));
		}
	}

	private void MozItem_DoubleClick(object sender, MozItemClickEventArgs e)
	{
		if (this.ItemDoubleClick != null)
		{
			this.ItemDoubleClick(this, e);
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (!(e.Control is MozItem))
		{
			Controls.Remove(e.Control);
			throw new InvalidCastException("Only MozItem's can be added to the MozPane");
		}
		base.OnControlAdded(e);
		if (!Items.Contains((MozItem)e.Control))
		{
			Items.Add((MozItem)e.Control);
		}
		Invalidate();
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		base.OnControlRemoved(e);
		if (Items.Contains(e.Control))
		{
			Items.Remove((MozItem)e.Control);
		}
		Invalidate();
	}

	protected virtual void OnMozItemAdded(MozItemEventArgs e)
	{
		if (!Controls.Contains(e.MozItem))
		{
			Controls.Add(e.MozItem);
		}
		e.MozItem.MozPane = this;
		e.MozItem.ItemGotFocus += MozItem_GotFocus;
		e.MozItem.ItemLostFocus += MozItem_LostFocus;
		e.MozItem.ItemClick += MozItem_Click;
		e.MozItem.ItemDoubleClick += MozItem_DoubleClick;
		DoLayout();
		if (this.ItemAdded != null)
		{
			this.ItemAdded(this, e);
		}
	}

	protected virtual void OnMozItemRemoved(MozItemEventArgs e)
	{
		if (Controls.Contains(e.MozItem))
		{
			Controls.Remove(e.MozItem);
		}
		e.MozItem.ItemGotFocus -= MozItem_GotFocus;
		e.MozItem.ItemLostFocus -= MozItem_LostFocus;
		e.MozItem.ItemClick -= MozItem_Click;
		e.MozItem.ItemDoubleClick -= MozItem_DoubleClick;
		DoLayout();
		if (this.ItemRemoved != null)
		{
			this.ItemRemoved(this, e);
		}
	}

	public void BeginInit()
	{
		initialising = true;
	}

	public void EndInit()
	{
		initialising = false;
	}

	public void DoLayout()
	{
		DoLayout(performRealLayout: false);
	}

	public void DoLayout(bool performRealLayout)
	{
		if (layout || beginUpdateCount > 0 || deserializing)
		{
			return;
		}
		layout = true;
		SuspendLayout();
		switch (m_style)
		{
		case MozPaneStyle.Vertical:
		{
			int num2 = DisplayRectangle.Y + m_padding.Vertical;
			_ = base.ClientRectangle.Width;
			_ = m_padding.Horizontal;
			for (int j = 0; j < Items.Count; j++)
			{
				MozItem mozItem = Items[j];
				if (mozItem.Visible || mozItem.Parent == null || !mozItem.Parent.Visible)
				{
					Point location = new Point(m_padding.Horizontal, num2);
					mozItem.Location = location;
					mozItem.Width = base.Width;
					num2 += mozItem.Height + m_padding.Vertical;
				}
			}
			break;
		}
		case MozPaneStyle.Horizontal:
		{
			int num = DisplayRectangle.X + m_padding.Horizontal;
			int height = base.ClientRectangle.Height - 2 * m_padding.Vertical;
			for (int i = 0; i < Items.Count; i++)
			{
				MozItem mozItem = Items[i];
				if (mozItem.Visible || mozItem.Parent == null || !mozItem.Parent.Visible)
				{
					Point location = new Point(num, m_padding.Vertical);
					mozItem.Location = location;
					mozItem.Height = height;
					num += mozItem.Width + m_padding.Horizontal;
				}
			}
			break;
		}
		}
		ResumeLayout(performRealLayout);
		layout = false;
	}

	internal bool IsVerticalScrollBarVisible()
	{
		return (GetWindowLong(base.Handle, -16) & 0x200000) != 0;
	}

	internal bool IsHorizontalScrollBarVisible()
	{
		return (GetWindowLong(base.Handle, -16) & 0x100000) != 0;
	}

	internal void UpdateMozItems()
	{
		if (Items.Count == Controls.Count)
		{
			MatchControlCollToMozItemsColl();
			return;
		}
		if (Items.Count > Controls.Count)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				if (!Controls.Contains(Items[i]))
				{
					OnMozItemAdded(new MozItemEventArgs(Items[i]));
				}
			}
			return;
		}
		int num = 0;
		while (num < Controls.Count)
		{
			MozItem mozItem = (MozItem)Controls[num];
			if (!Items.Contains(mozItem))
			{
				OnMozItemRemoved(new MozItemEventArgs(mozItem));
			}
			else
			{
				num++;
			}
		}
	}

	internal void MatchControlCollToMozItemsColl()
	{
		SuspendLayout();
		for (int i = 0; i < Items.Count; i++)
		{
			Controls.SetChildIndex(Items[i], i);
		}
		ResumeLayout(performLayout: false);
		DoLayout(performRealLayout: true);
		Invalidate(invalidateChildren: true);
	}

	internal static int HiWord(int number)
	{
		if ((number & 0x80000000u) == 2147483648u)
		{
			return number >> 16;
		}
		return (number >> 16) & 0xFFFF;
	}

	internal static int LoWord(int number)
	{
		return number & 0xFFFF;
	}

	internal static int MakeLong(int LoWord, int HiWord)
	{
		return (HiWord << 16) | (LoWord & 0xFFFF);
	}

	internal static IntPtr MakeLParam(int LoWord, int HiWord)
	{
		return (IntPtr)((HiWord << 16) | (LoWord & 0xFFFF));
	}

	private void ScrollIntoView(int index, int direction)
	{
		if (IsVerticalScrollBarVisible())
		{
			if (direction == 0)
			{
				int num = 1;
				while (Items[index].Bottom > base.ClientRectangle.Bottom)
				{
					SendMessage(base.Handle, 277u, (UIntPtr)(ulong)num, (IntPtr)0);
				}
			}
			else
			{
				int num = 0;
				while (Items[index].Top < base.ClientRectangle.Top)
				{
					SendMessage(base.Handle, 277u, (UIntPtr)(ulong)num, (IntPtr)0);
				}
			}
		}
		if (!IsHorizontalScrollBarVisible())
		{
			return;
		}
		if (direction == 0)
		{
			int num = 1;
			while (Items[index].Right > base.ClientRectangle.Right + 1)
			{
				SendMessage(base.Handle, 276u, (UIntPtr)(ulong)num, (IntPtr)0);
			}
		}
		else
		{
			int num = 0;
			while (Items[index].Left < base.ClientRectangle.Left + 1)
			{
				SendMessage(base.Handle, 276u, (UIntPtr)(ulong)num, (IntPtr)0);
			}
		}
	}

	private void RemoveFocus()
	{
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].state == MozItemState.Focus && Items[i] != m_mouseOverItem)
			{
				Items[i].state = MozItemState.Normal;
			}
		}
	}

	private void GetThemeColors()
	{
		int partID = 1;
		int partID2 = 5;
		int propID = 3810;
		int propID2 = 3811;
		Color color = default(Color);
		Color focusBackground = default(Color);
		Color color2 = default(Color);
		bool flag = false;
		if (m_themeManager._IsAppThemed())
		{
			if (m_theme != IntPtr.Zero)
			{
				m_themeManager._CloseThemeData(m_theme);
			}
			m_theme = m_themeManager._OpenThemeData(base.Handle, "EXPLORERBAR");
			if (m_theme != IntPtr.Zero)
			{
				color = m_themeManager._GetThemeColor(m_theme, partID, 1, propID2);
				focusBackground = m_themeManager._GetThemeColor(m_theme, partID2, 1, propID);
				color2 = ControlPaint.Light(color);
				color = ControlPaint.LightLight(color);
				focusBackground = ControlPaint.LightLight(color);
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			color = SystemColors.ActiveCaption;
			focusBackground = ControlPaint.Light(color);
			color2 = SystemColors.ActiveBorder;
		}
		ItemColors.SelectedBorder = color;
		ItemColors.Divider = color2;
		BorderColor = color2;
		ItemColors.SelectedBackground = color;
		ItemColors.FocusBackground = focusBackground;
		ItemColors.FocusBorder = color;
		Invalidate();
	}

	public void SelectItem(int index)
	{
		if (index < 0 || index > Items.Count - 1)
		{
			return;
		}
		if (Items[index].state != MozItemState.Selected)
		{
			if (Items[index].ItemStyle == MozItemStyle.Divider)
			{
				return;
			}
			if (!Toggle)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (Items[i] != m_mouseOverItem || Items[i].state == MozItemState.Selected)
					{
						Items[i].state = MozItemState.Normal;
					}
				}
				m_selectedItems = 0;
			}
			if (m_maxSelectedItems >= m_selectedItems + 1)
			{
				Items[index].state = MozItemState.Selected;
				m_selectedItems++;
				if (this.ItemSelected != null)
				{
					this.ItemSelected(this, new MozItemEventArgs(Items[index]));
				}
			}
		}
		else if (Toggle)
		{
			Items[index].state = MozItemState.Focus;
			m_selectedItems--;
			if (this.ItemDeselected != null)
			{
				this.ItemDeselected(this, new MozItemEventArgs(Items[index]));
			}
		}
	}

	public void SelectItem(string tag)
	{
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].Tag.ToString() == tag)
			{
				SelectItem(i);
			}
		}
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}
