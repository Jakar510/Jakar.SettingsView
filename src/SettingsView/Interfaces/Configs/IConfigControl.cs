// unset

namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IConfigControl
{
    public Color Color { get; set; }

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize { get; set; }

    public string? FontFamily { get; set; }
    public string? Text       { get; set; }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes FontAttributes { get; set; }

    public TextAlignment Alignment { get; set; }
}