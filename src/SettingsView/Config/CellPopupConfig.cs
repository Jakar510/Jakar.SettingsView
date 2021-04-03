// unset

using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Config
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class CellPopupConfig : SVConfig
	{
		public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CellPopupConfig), default(string));
		public static BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.Title.color);
		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Title.FONT_SIZE);


		public static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.background_Color);
		public static BindableProperty AcceptProperty = BindableProperty.Create(nameof(Accept), typeof(string), typeof(CellPopupConfig), SvConstants.Prompt.ACCEPT_TEXT);
		public static BindableProperty CancelProperty = BindableProperty.Create(nameof(Cancel), typeof(string), typeof(CellPopupConfig), SvConstants.Prompt.CANCEL_TEXT);


		public static readonly BindableProperty SelectedFontSizeProperty = BindableProperty.Create(nameof(SelectedFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Selected.FONT_SIZE);
		public static BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.Selected.text_Color);
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Defaults.accent);
		public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.separator_Color);


		public static readonly BindableProperty ItemFontSizeProperty = BindableProperty.Create(nameof(ItemFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Item.FONT_SIZE);
		public static BindableProperty ItemColorProperty = BindableProperty.Create(nameof(ItemColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.Item.color);


		public static BindableProperty ItemDescriptionColorProperty = BindableProperty.Create(nameof(ItemDescriptionColor), typeof(Color), typeof(CellPopupConfig), SvConstants.Prompt.Item.Description.color);
		public static readonly BindableProperty ItemDescriptionFontSizeProperty = BindableProperty.Create(nameof(ItemDescriptionFontSize), typeof(double), typeof(CellPopupConfig), SvConstants.Prompt.Item.Description.FONT_SIZE);


		[TypeConverter(typeof(ColorTypeConverter))]
		public Color BackgroundColor
		{
			get => (Color) GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		// -----------------------------------------------------------------------------------

		public string Title
		{
			get => (string) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}


		[TypeConverter(typeof(ColorTypeConverter))]
		public Color TitleColor
		{
			get => (Color) GetValue(TitleColorProperty);
			set => SetValue(TitleColorProperty, value);
		}


		[TypeConverter(typeof(FontSizeConverter))]
		public double TitleFontSize
		{
			get => (double) GetValue(TitleFontSizeProperty);
			set => SetValue(TitleFontSizeProperty, value);
		}

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(FontSizeConverter))]
		public double ItemFontSize
		{
			get => (double) GetValue(ItemFontSizeProperty);
			set => SetValue(ItemFontSizeProperty, value);
		}


		[TypeConverter(typeof(ColorTypeConverter))]
		public Color ItemColor
		{
			get => (Color) GetValue(ItemColorProperty);
			set => SetValue(ItemColorProperty, value);
		}

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color ItemDescriptionColor
		{
			get => (Color) GetValue(ItemDescriptionColorProperty);
			set => SetValue(ItemDescriptionColorProperty, value);
		}


		[TypeConverter(typeof(FontSizeConverter))]
		public double ItemDescriptionFontSize
		{
			get => (double) GetValue(ItemDescriptionFontSizeProperty);
			set => SetValue(ItemDescriptionFontSizeProperty, value);
		}

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(FontSizeConverter))]
		public double SelectedFontSize
		{
			get => (double) GetValue(SelectedFontSizeProperty);
			set => SetValue(SelectedFontSizeProperty, value);
		}


		[TypeConverter(typeof(ColorTypeConverter))]
		public Color SelectedColor
		{
			get => (Color) GetValue(SelectedColorProperty);
			set => SetValue(SelectedColorProperty, value);
		}


		[TypeConverter(typeof(ColorTypeConverter))]
		public Color SeparatorColor
		{
			get => (Color) GetValue(SeparatorColorProperty);
			set => SetValue(SeparatorColorProperty, value);
		}

		// -----------------------------------------------------------------------------------

		public string Accept
		{
			get => (string) GetValue(AcceptProperty);
			set => SetValue(AcceptProperty, value);
		}

		public string Cancel
		{
			get => (string) GetValue(CancelProperty);
			set => SetValue(CancelProperty, value);
		}
	}
}