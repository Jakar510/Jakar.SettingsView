

namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IParent<TParent>
{
    public TParent? Parent { get; set; }
}