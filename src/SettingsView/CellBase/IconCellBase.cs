// unset


namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
public abstract class IconCellBase : TitleCellBase
{
    public static readonly BindableProperty iconSourceProperty = BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(IconCellBase));
    public static readonly BindableProperty iconSizeProperty   = BindableProperty.Create(nameof(IconSize),   typeof(Size?),       typeof(IconCellBase), default(Size?));
    public static readonly BindableProperty iconRadiusProperty = BindableProperty.Create(nameof(IconRadius), typeof(double?),     typeof(IconCellBase), SvConstants.Cell.iconRadius);


    [TypeConverter(typeof(NullableImageSourceConverter))]
    public ImageSource? IconSource
    {
        get => (ImageSource?) GetValue(iconSourceProperty);
        set => SetValue(iconSourceProperty, value);
    }

    [TypeConverter(typeof(NullableSizeConverter))]
    public Size? IconSize
    {
        get => (Size?) GetValue(iconSizeProperty);
        set => SetValue(iconSizeProperty, value);
    }

    [TypeConverter(typeof(NullableDoubleTypeConverter))]
    public double? IconRadius
    {
        get => (double?) GetValue(iconRadiusProperty);
        set => SetValue(iconRadiusProperty, value);
    }


    private IUseIconConfiguration? _config;

    protected internal IUseIconConfiguration IconConfig
    {
        get
        {
            _config ??= new IconConfiguration(this);
            return _config;
        }
    }



    public sealed class IconConfiguration : IUseIconConfiguration
    {
        private readonly IconCellBase _cell;
        public IconConfiguration( IconCellBase cell ) => _cell = cell;

        public ImageSource? Source     => _cell.IconSource;
        public double       IconRadius => _cell.IconRadius ?? _cell.Parent.CellIconRadius;
        public Size         IconSize   => _cell.IconSize ?? _cell.Parent.CellIconSize;
    }
}