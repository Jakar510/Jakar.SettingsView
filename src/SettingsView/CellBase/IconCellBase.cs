// unset

using Jakar.SettingsView.Shared.Converters;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public class IconCellBase : TitleCellBase
	{
		public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(IconCellBase), default(ImageSource?));
		public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(Size), typeof(IconCellBase), default(Size?));
		public static readonly BindableProperty IconRadiusProperty = BindableProperty.Create(nameof(IconRadius), typeof(double), typeof(IconCellBase), -1.0d);


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