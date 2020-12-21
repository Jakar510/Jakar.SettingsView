// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBaseIcon : CellBaseTitle
	{
		public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(CellBase), default(ImageSource?));
		public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(Size), typeof(CellBase), default(Size?));
		public static readonly BindableProperty IconRadiusProperty = BindableProperty.Create(nameof(IconRadius), typeof(double), typeof(CellBase), -1.0d);

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource? IconSource
		{
			get => (ImageSource?) GetValue(IconSourceProperty);
			set => SetValue(IconSourceProperty, value);
		}

		[TypeConverter(typeof(SizeConverter))]
		public Size? IconSize
		{
			get => (Size?) GetValue(IconSizeProperty);
			set => SetValue(IconSizeProperty, value);
		}

		public double IconRadius
		{
			get => (double) GetValue(IconRadiusProperty);
			set => SetValue(IconRadiusProperty, value);
		}
	}
}