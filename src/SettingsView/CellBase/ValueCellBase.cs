﻿// unset

using System;
using System.Runtime.CompilerServices;
using Jakar.Api.Converters;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class ValueCellBase : HintTextCellBase
	{
		public static readonly BindableProperty ValueTextColorProperty = BindableProperty.Create(nameof(ValueTextColor), typeof(Color?), typeof(ValueTextCellBase), SvConstants.Cell.color);
		public static readonly BindableProperty ValueTextFontSizeProperty = BindableProperty.Create(nameof(ValueTextFontSize), typeof(double?), typeof(ValueTextCellBase), SvConstants.Cell.font_Size);
		public static readonly BindableProperty ValueTextFontFamilyProperty = BindableProperty.Create(nameof(ValueTextFontFamily), typeof(string), typeof(ValueTextCellBase), default(string));
		public static readonly BindableProperty ValueTextFontAttributesProperty = BindableProperty.Create(nameof(ValueTextFontAttributes), typeof(FontAttributes?), typeof(ValueTextCellBase));
		public static readonly BindableProperty ValueTextAlignmentProperty = BindableProperty.Create(nameof(ValueTextAlignment), typeof(TextAlignment?), typeof(ValueTextCellBase));


		[TypeConverter(typeof(ColorTypeConverter))]
		public Color ValueTextColor
		{
			get => (Color) GetValue(ValueTextColorProperty);
			set => SetValue(ValueTextColorProperty, value);
		}

		[TypeConverter(typeof(NullableFontSizeConverter))]
		public double? ValueTextFontSize
		{
			get => (double?) GetValue(ValueTextFontSizeProperty);
			set => SetValue(ValueTextFontSizeProperty, value);
		}

		public string? ValueTextFontFamily
		{
			get => (string?) GetValue(ValueTextFontFamilyProperty);
			set => SetValue(ValueTextFontFamilyProperty, value);
		}

		[TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes? ValueTextFontAttributes
		{
			get => (FontAttributes?) GetValue(ValueTextFontAttributesProperty);
			set => SetValue(ValueTextFontAttributesProperty, value);
		}
		
		[TypeConverter(typeof(NullableTextAlignmentConverter))]
		public TextAlignment? ValueTextAlignment
		{
			get => (TextAlignment?) GetValue(ValueTextAlignmentProperty);
			set => SetValue(ValueTextAlignmentProperty, value);
		}


		private IUseConfiguration? _config;

		protected internal IUseConfiguration ValueTextConfig
		{
			get
			{
				_config ??= new ValueTextConfiguration(this);
				return _config;
			}
		}



		public sealed class ValueTextConfiguration : IUseConfiguration
		{
			private readonly ValueCellBase _cell;
			public ValueTextConfiguration( ValueCellBase cell ) => _cell = cell;

			public string?        FontFamily     => _cell.ValueTextFontFamily ?? _cell.Parent.CellValueTextFontFamily;
			public FontAttributes FontAttributes => _cell.ValueTextFontAttributes ?? _cell.Parent.CellValueTextFontAttributes;
			public TextAlignment  TextAlignment  => _cell.ValueTextAlignment ?? _cell.Parent.CellValueTextAlignment;

			public Color Color =>
				_cell.ValueTextColor == SvConstants.Cell.color
					? _cell.Parent.CellValueTextColor
					: _cell.ValueTextColor;

			public double FontSize => _cell.ValueTextFontSize ?? _cell.Parent.CellValueTextFontSize;
		}
	}



	public abstract class ValueCellBase<TValue> : ValueCellBase, IValueChanged<TValue>
	{
		public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;

		void IValueChanged<TValue>.SendValueChanged( TValue value ) { ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value)); }
		internal IValueChanged<TValue> ValueChangedHandler => this;
	}
}
