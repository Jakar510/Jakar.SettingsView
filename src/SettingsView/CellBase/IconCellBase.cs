// unset

using Jakar.Api.Converters;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class IconCellBase : TitleCellBase
	{
		public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(IconCellBase), default(ImageSource?));
		public static readonly BindableProperty IconSizeProperty = BindableProperty.Create(nameof(IconSize), typeof(Size?), typeof(IconCellBase), default(Size?));
		public static readonly BindableProperty IconRadiusProperty = BindableProperty.Create(nameof(IconRadius), typeof(double?), typeof(IconCellBase), SvConstants.Cell.iconRadius);


		[TypeConverter(typeof(NullableImageSourceConverter))]
		public ImageSource? IconSource
		{
			get => (ImageSource?) GetValue(IconSourceProperty);
			set => SetValue(IconSourceProperty, value);
		}

		[TypeConverter(typeof(NullableSizeConverter))]
		public Size? IconSize
		{
			get => (Size?) GetValue(IconSizeProperty);
			set => SetValue(IconSizeProperty, value);
		}

		[TypeConverter(typeof(NullableDoubleTypeConverter))]
		public double? IconRadius
		{
			get => (double?) GetValue(IconRadiusProperty);
			set => SetValue(IconRadiusProperty, value);
		}

		internal double GetIconRadius() => IconRadius ?? Parent.CellIconRadius;
		internal Size GetIconSize() => IconSize ?? Parent.CellIconSize;
	}
}