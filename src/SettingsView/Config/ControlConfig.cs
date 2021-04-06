// unset

using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Config
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class ControlConfig : SvConfig, IConfigControl
	{
		public static BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ControlConfig), SvConstants.Defaults.color);

		public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize),
																				  typeof(double),
																				  typeof(ControlConfig),
																				  SvConstants.Defaults.FONT_SIZE,
																				  BindingMode.OneWay,
																				  defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, typeof(ControlConfig))
																				 );

		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ControlConfig), default(string?));
		public static BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ControlConfig), default(string?));

		public static BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(ControlConfig), FontAttributes.None);
		public static BindableProperty AlignmentProperty = BindableProperty.Create(nameof(Alignment), typeof(TextAlignment), typeof(ControlConfig), TextAlignment.Start);


		public string? Text
		{
			get => (string?) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public Color Color
		{
			get => (Color) GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get => (double) GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}


		public string? FontFamily
		{
			get => (string?) GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		public FontAttributes FontAttributes
		{
			get => (FontAttributes) GetValue(FontAttributesProperty);
			set => SetValue(FontAttributesProperty, value);
		}

		public TextAlignment Alignment
		{
			get => (TextAlignment) GetValue(AlignmentProperty);
			set => SetValue(AlignmentProperty, value);
		}
	}
}