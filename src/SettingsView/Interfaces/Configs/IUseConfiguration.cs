// unset

namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUseCheckableConfiguration
{
    public Color AccentColor { get; }
    public Color OffColor    { get; }
}



[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUseIconConfiguration
{
    public ImageSource? Source     { get; }
    public double       IconRadius { get; }
    public Size         IconSize   { get; }
}



[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUseConfiguration
{
    public Color Color { get; }

    public double         FontSize       { get; }
    public string?        FontFamily     { get; }
    public FontAttributes FontAttributes { get; }

    public TextAlignment TextAlignment { get; }
}