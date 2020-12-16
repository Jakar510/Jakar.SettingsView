using System;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class CellBase : Cell
	{
		public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(CellBase), -1.0, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(nameof(TitleFontFamily), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty TitleFontAttributesProperty = BindableProperty.Create(nameof(TitleFontAttributes), typeof(FontAttributes?), typeof(CellBase), null, defaultBindingMode: BindingMode.OneWay);

		public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionColorProperty = BindableProperty.Create(nameof(DescriptionColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(double), typeof(CellBase), -1.0d, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty DescriptionFontAttributesProperty = BindableProperty.Create(nameof(DescriptionFontAttributes), typeof(FontAttributes?), typeof(CellBase), null, defaultBindingMode: BindingMode.OneWay);

		public static readonly BindableProperty HintTextProperty = BindableProperty.Create(nameof(HintText), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintTextColorProperty = BindableProperty.Create(nameof(HintTextColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintFontSizeProperty = BindableProperty.Create(nameof(HintFontSize), typeof(double), typeof(CellBase), -1.0d, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintFontFamilyProperty = BindableProperty.Create(nameof(HintFontFamily), typeof(string), typeof(CellBase), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintFontAttributesProperty = BindableProperty.Create(nameof(HintFontAttributes), typeof(FontAttributes?), typeof(CellBase), null, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty HintColorProperty = BindableProperty.Create(nameof(HintColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);

		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(CellBase), default(ImageSource), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(Size), typeof(CellBase), default(Size), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty IconRadiusProperty = BindableProperty.Create(nameof(IconRadius), typeof(double), typeof(CellBase), -1.0d, defaultBindingMode: BindingMode.OneWay);



		public new event EventHandler Tapped;

		internal new void OnTapped() { Tapped?.Invoke(this, EventArgs.Empty); }


		public string Title
		{
			get => (string) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}


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


		public string TitleFontFamily
		{
			get => (string) GetValue(TitleFontFamilyProperty);
			set => SetValue(TitleFontFamilyProperty, value);
		}


		public FontAttributes? TitleFontAttributes
		{
			get => (FontAttributes?) GetValue(TitleFontAttributesProperty);
			set => SetValue(TitleFontAttributesProperty, value);
		}


		public string Description
		{
			get => (string) GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}


		public Color DescriptionColor
		{
			get => (Color) GetValue(DescriptionColorProperty);
			set => SetValue(DescriptionColorProperty, value);
		}


		[TypeConverter(typeof(FontSizeConverter))]
		public double DescriptionFontSize
		{
			get => (double) GetValue(DescriptionFontSizeProperty);
			set => SetValue(DescriptionFontSizeProperty, value);
		}


		public string DescriptionFontFamily
		{
			get => (string) GetValue(DescriptionFontFamilyProperty);
			set => SetValue(DescriptionFontFamilyProperty, value);
		}


		public FontAttributes? DescriptionFontAttributes
		{
			get => (FontAttributes?) GetValue(DescriptionFontAttributesProperty);
			set => SetValue(DescriptionFontAttributesProperty, value);
		}


		public string HintText
		{
			get => (string) GetValue(HintTextProperty);
			set => SetValue(HintTextProperty, value);
		}


		public Color HintTextColor
		{
			get => (Color) GetValue(HintTextColorProperty);
			set => SetValue(HintTextColorProperty, value);
		}


		[TypeConverter(typeof(FontSizeConverter))]
		public double HintFontSize
		{
			get => (double) GetValue(HintFontSizeProperty);
			set => SetValue(HintFontSizeProperty, value);
		}


		public string HintFontFamily
		{
			get => (string) GetValue(HintFontFamilyProperty);
			set => SetValue(HintFontFamilyProperty, value);
		}


		public FontAttributes? HintFontAttributes
		{
			get => (FontAttributes?) GetValue(HintFontAttributesProperty);
			set => SetValue(HintFontAttributesProperty, value);
		}


		public Color HintColor
		{
			get => (Color) GetValue(HintColorProperty);
			set => SetValue(HintColorProperty, value);
		}

		public Color BackgroundColor
		{
			get => (Color) GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}


		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource IconSource
		{
			get => (ImageSource) GetValue(IconSourceProperty);
			set => SetValue(IconSourceProperty, value);
		}


		[TypeConverter(typeof(SizeConverter))]
		public Size IconSize
		{
			get => (Size) GetValue(IconSizeProperty);
			set => SetValue(IconSizeProperty, value);
		}


		public double IconRadius
		{
			get => (double) GetValue(IconRadiusProperty);
			set => SetValue(IconRadiusProperty, value);
		}

		public Section Section { get; set; }

		public new SettingsView Parent
		{
			get => (SettingsView) base.Parent;
			set => base.Parent = value;
		}

		public virtual void Reload()
		{
			if ( Section == null ) { return; }

			int index = Section.IndexOf(this);
			if ( index < 0 ) { return; }

			// raise replace event manually.
			Cell temp = Section[index];
			Section[index] = temp;
		}
	}
}