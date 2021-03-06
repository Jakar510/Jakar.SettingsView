﻿// unset

using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public abstract class DescriptionCellBase : IconCellBase
	{
		public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(DescriptionCellBase), default(string));
		public static readonly BindableProperty DescriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color?), typeof(DescriptionCellBase), SVConstants.Cell.COLOR);
		public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(double?), typeof(DescriptionCellBase), SVConstants.Cell.FONT_SIZE);
		public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(DescriptionCellBase), default(string));
		public static readonly BindableProperty DescriptionFontAttributesProperty = BindableProperty.Create(nameof(DescriptionFontAttributes), typeof(FontAttributes?), typeof(DescriptionCellBase));
		public static readonly BindableProperty DescriptionAlignmentProperty = BindableProperty.Create(nameof(DescriptionAlignment), typeof(TextAlignment?), typeof(DescriptionCellBase));

		public string? Description
		{
			get => (string?) GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		// [TypeConverter(typeof(NullableColorTypeConverter))]
		public Color DescriptionColor
		{
			get => (Color) GetValue(DescriptionColorProperty);
			set => SetValue(DescriptionColorProperty, value);
		}


		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? DescriptionFontSize
		{
			get => (double?) GetValue(DescriptionFontSizeProperty);
			set => SetValue(DescriptionFontSizeProperty, value);
		}

		public string? DescriptionFontFamily
		{
			get => (string?) GetValue(DescriptionFontFamilyProperty);
			set => SetValue(DescriptionFontFamilyProperty, value);
		}

		[TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes? DescriptionFontAttributes
		{
			get => (FontAttributes?) GetValue(DescriptionFontAttributesProperty);
			set => SetValue(DescriptionFontAttributesProperty, value);
		}

		public TextAlignment? DescriptionAlignment
		{
			get => (TextAlignment?) GetValue(DescriptionAlignmentProperty);
			set => SetValue(DescriptionAlignmentProperty, value);
		}

		// internal string? GetDescriptionFontFamily() => DescriptionFontFamily ?? Parent.CellDescriptionFontFamily;
		// internal FontAttributes GetDescriptionFontAttributes() => DescriptionFontAttributes ?? Parent.CellTitleFontAttributes;
		// internal TextAlignment GetDescriptionTextAlignment() => DescriptionAlignment ?? Parent.CellDescriptionAlignment;
		// internal Color GetDescriptionColor() => DescriptionColor ?? Parent.CellDescriptionColor;
		// internal double GetDescriptionFontSize() => DescriptionFontSize ?? Parent.CellDescriptionFontSize;


		private DescriptionConfiguration? _config;

		internal DescriptionConfiguration DescriptionConfig
		{
			get
			{
				_config ??= new DescriptionConfiguration(this);
				return _config;
			}
		}

		internal class DescriptionConfiguration
		{
			private readonly DescriptionCellBase _cell;
			public DescriptionConfiguration( DescriptionCellBase cell ) => _cell = cell;

			internal string? FontFamily => _cell.DescriptionFontFamily ?? _cell.Parent.CellDescriptionFontFamily;
			internal FontAttributes FontAttributes => _cell.DescriptionFontAttributes ?? _cell.Parent.CellDescriptionFontAttributes;
			internal TextAlignment TextAlignment => _cell.DescriptionAlignment ?? _cell.Parent.CellDescriptionAlignment;

			internal Color Color =>
				_cell.DescriptionColor == SVConstants.Cell.COLOR
					? _cell.Parent.CellDescriptionColor
					: _cell.DescriptionColor;

			internal double FontSize => _cell.DescriptionFontSize ?? _cell.Parent.CellDescriptionFontSize;
		}
	}
}