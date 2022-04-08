// unset

namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IBorder
{
    public Thickness BorderThickness { get; set; }
    public Color     BorderColor     { get; set; }
}