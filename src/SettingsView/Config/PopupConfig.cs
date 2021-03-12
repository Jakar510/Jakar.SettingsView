// unset

using System.Data;
using Jakar.SettingsView.Shared.Converters;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	public class PopupConfig : CellConfig, IConfigPopup
	{
		public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(PopupConfig), default(string));
		public static BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(PopupConfig), Color.Black);
		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double?), typeof(PopupConfig), SVConstants.Prompt.Title.FONT_SIZE);


		public static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(PopupConfig), Color.White);
		public static BindableProperty AcceptProperty = BindableProperty.Create(nameof(Accept), typeof(string), typeof(PopupConfig), SVConstants.Prompt.ACCEPT_TEXT);
		public static BindableProperty CancelProperty = BindableProperty.Create(nameof(Cancel), typeof(string), typeof(PopupConfig), SVConstants.Prompt.CANCEL_TEXT);


		public static readonly BindableProperty SelectedFontSizeProperty = BindableProperty.Create(nameof(SelectedFontSize), typeof(double?), typeof(PopupConfig), SVConstants.Prompt.Selected.FONT_SIZE);
		public static BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(PopupConfig), SVConstants.Prompt.Selected.TEXT_COLOR);
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(PopupConfig), SVConstants.Defaults.ACCENT);
		public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(PopupConfig), SVConstants.Prompt.SEPARATOR_COLOR);


		public static readonly BindableProperty ItemFontSizeProperty = BindableProperty.Create(nameof(ItemFontSize), typeof(double?), typeof(PopupConfig), SVConstants.Prompt.Item.FONT_SIZE);
		public static BindableProperty ItemColorProperty = BindableProperty.Create(nameof(ItemColor), typeof(Color), typeof(PopupConfig), SVConstants.Prompt.Item.COLOR);


		public static BindableProperty ItemDescriptionColorProperty = BindableProperty.Create(nameof(ItemDescriptionColor), typeof(Color), typeof(PopupConfig), Color.SlateGray);
		public static readonly BindableProperty ItemDescriptionFontSizeProperty = BindableProperty.Create(nameof(ItemDescriptionFontSize), typeof(double?), typeof(PopupConfig), SVConstants.Prompt.Item.Description.FontSize);


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
				_config.BackgroundColor == SVConstants.Cell.COLOR
					? _Sv?.BackgroundColor ?? SVConstants.Prompt.BACKGROUND_COLOR
					: _config.BackgroundColor;


			internal string Title => _config.Title;
			internal Color TitleColor =>
				_config.TitleColor == SVConstants.Prompt.Title.COLOR
					? _Sv?.TitleColor ?? SVConstants.Prompt.Title.COLOR
					: _config.TitleColor;
			internal double TitleFontSize => _config.TitleFontSize ?? _Sv?.TitleFontSize ?? SVConstants.Prompt.Title.FONT_SIZE;


			internal Color ItemColor =>
				_config.ItemColor == SVConstants.Prompt.Item.COLOR
					? _Sv?.ItemColor ?? SVConstants.Prompt.Item.COLOR
					: _config.ItemColor;
			internal double ItemFontSize => _config.ItemFontSize ?? _Sv?.ItemFontSize ?? SVConstants.Prompt.Item.FONT_SIZE;


			internal Color ItemDescriptionColor =>
				_config.ItemDescriptionColor == SVConstants.Prompt.Item.Description.Color
					? _Sv?.ItemDescriptionColor ?? SVConstants.Prompt.Item.Description.Color
					: _config.ItemDescriptionColor;
			internal double ItemDescriptionFontSize => _config.ItemDescriptionFontSize ?? _Sv?.ItemDescriptionFontSize ?? SVConstants.Prompt.Item.Description.FontSize;
			
			
			internal double SelectedFontSize => _config.SelectedFontSize ?? _Sv?.SelectedFontSize ?? SVConstants.Prompt.Selected.FONT_SIZE;

			internal Color AccentColor =>
				_config.AccentColor == SVConstants.Defaults.ACCENT
					? _Sv?.AccentColor ?? SVConstants.Defaults.ACCENT
					: _config.AccentColor;

			internal Color SelectedColor =>
				_config.SelectedColor == SVConstants.Prompt.Selected.TEXT_COLOR
					? _Sv?.SelectedColor ?? SVConstants.Prompt.Selected.TEXT_COLOR
					: _config.SelectedColor;

			internal Color SeparatorColor =>
				_config.SeparatorColor == SVConstants.Prompt.SEPARATOR_COLOR
					? _Sv?.SeparatorColor ?? SVConstants.Prompt.SEPARATOR_COLOR
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