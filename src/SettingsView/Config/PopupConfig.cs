// unset

using Jakar.Api.Converters;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class PopupConfig : CellConfig, IConfigPopup
	{
		public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(PopupConfig), default(string));
		public static BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(PopupConfig), Color.Black);
		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Title.FONT_SIZE);


		public static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(PopupConfig), Color.White);
		public static BindableProperty AcceptProperty = BindableProperty.Create(nameof(Accept), typeof(string), typeof(PopupConfig), SvConstants.Prompt.ACCEPT_TEXT);
		public static BindableProperty CancelProperty = BindableProperty.Create(nameof(Cancel), typeof(string), typeof(PopupConfig), SvConstants.Prompt.CANCEL_TEXT);


		public static readonly BindableProperty SelectedFontSizeProperty = BindableProperty.Create(nameof(SelectedFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Selected.FONT_SIZE);
		public static BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(PopupConfig), SvConstants.Prompt.Selected.text_Color);
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(PopupConfig), SvConstants.Defaults.accent);
		public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(PopupConfig), SvConstants.Prompt.separator_Color);


		public static readonly BindableProperty ItemFontSizeProperty = BindableProperty.Create(nameof(ItemFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Item.FONT_SIZE);
		public static BindableProperty ItemColorProperty = BindableProperty.Create(nameof(ItemColor), typeof(Color), typeof(PopupConfig), SvConstants.Prompt.Item.color);


		public static BindableProperty ItemDescriptionColorProperty = BindableProperty.Create(nameof(ItemDescriptionColor), typeof(Color), typeof(PopupConfig), Color.SlateGray);
		public static readonly BindableProperty ItemDescriptionFontSizeProperty = BindableProperty.Create(nameof(ItemDescriptionFontSize), typeof(double?), typeof(PopupConfig), SvConstants.Prompt.Item.Description.FONT_SIZE);


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


		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? TitleFontSize
		{
			get => (double?) GetValue(TitleFontSizeProperty);
			set => SetValue(TitleFontSizeProperty, value);
		}

		// -----------------------------------------------------------------------------------

		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? ItemFontSize
		{
			get => (double?) GetValue(ItemFontSizeProperty);
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
		public double? ItemDescriptionFontSize
		{
			get => (double?) GetValue(ItemDescriptionFontSizeProperty);
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
		public double? SelectedFontSize
		{
			get => (double?) GetValue(SelectedFontSizeProperty);
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


		// -----------------------------------------------------------------------------------

		private PromptConfiguration? _config;

		internal PromptConfiguration Properties
		{
			get
			{
				_config ??= new PromptConfiguration(this);
				return _config;
			}
		}

		internal class PromptConfiguration
		{
			private readonly PopupConfig _config;
			private CellPopupConfig? _Sv => _config.Parent?.Parent.Popup;
			public PromptConfiguration( PopupConfig cell ) => _config = cell;

			internal Color BackgroundColor =>
				_config.BackgroundColor == SvConstants.Cell.color
					? _Sv?.BackgroundColor ?? SvConstants.Prompt.background_Color
					: _config.BackgroundColor;


			internal string Title => _config.Title;
			internal Color TitleColor =>
				_config.TitleColor == SvConstants.Prompt.Title.color
					? _Sv?.TitleColor ?? SvConstants.Prompt.Title.color
					: _config.TitleColor;
			internal double TitleFontSize => _config.TitleFontSize ?? _Sv?.TitleFontSize ?? SvConstants.Prompt.Title.FONT_SIZE;


			internal Color ItemColor =>
				_config.ItemColor == SvConstants.Prompt.Item.color
					? _Sv?.ItemColor ?? SvConstants.Prompt.Item.color
					: _config.ItemColor;
			internal double ItemFontSize => _config.ItemFontSize ?? _Sv?.ItemFontSize ?? SvConstants.Prompt.Item.FONT_SIZE;


			internal Color ItemDescriptionColor =>
				_config.ItemDescriptionColor == SvConstants.Prompt.Item.Description.color
					? _Sv?.ItemDescriptionColor ?? SvConstants.Prompt.Item.Description.color
					: _config.ItemDescriptionColor;
			internal double ItemDescriptionFontSize => _config.ItemDescriptionFontSize ?? _Sv?.ItemDescriptionFontSize ?? SvConstants.Prompt.Item.Description.FONT_SIZE;
			
			
			internal double SelectedFontSize => _config.SelectedFontSize ?? _Sv?.SelectedFontSize ?? SvConstants.Prompt.Selected.FONT_SIZE;

			internal Color AccentColor =>
				_config.AccentColor == SvConstants.Defaults.accent
					? _Sv?.AccentColor ?? SvConstants.Defaults.accent
					: _config.AccentColor;

			internal Color SelectedColor =>
				_config.SelectedColor == SvConstants.Prompt.Selected.text_Color
					? _Sv?.SelectedColor ?? SvConstants.Prompt.Selected.text_Color
					: _config.SelectedColor;

			internal Color SeparatorColor =>
				_config.SeparatorColor == SvConstants.Prompt.separator_Color
					? _Sv?.SeparatorColor ?? SvConstants.Prompt.separator_Color
					: _config.SeparatorColor;
		}
	}


	// public class Config
	// {
	// 	public Color BackgroundColor { get; init; }
	// 	public Color AccentColor { get; init; }
	//
	//
	// 	public ItemConfig Title { get; init; }
	// 	public ItemConfig Description { get; init; }
	// 	public ItemConfig Selected { get; init; }
	//
	//
	// 	public string Accept { get; init; }
	// 	public string Cancel { get; init; }
	//
	//
	// 	public PopupConfig( PopupCellBase cell )
	// 	{
	// 		Accept = cell.PopupAccept;
	// 		Cancel = cell.PopupCancel;
	// 		BackgroundColor = cell.PopupBackgroundColor;
	// 		AccentColor = cell.PopupAccentColor;
	//
	// 		Title = new ItemConfig(cell.PopupTitle, cell.PopupTitleColor, new FontConfig(cell.PopupTitleFontSize));
	// 		Description = new ItemConfig(cell.PopupItemDescriptionColor, new FontConfig(cell.PopupItemDescriptionFontSize));
	// 		Selected = new ItemConfig(cell.PopupSelectedColor, new FontConfig(cell.PopupSelectedFontSize));
	// 	}
	// }
}