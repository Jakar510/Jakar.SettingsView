// unset

namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface IUpdateCell<in TCell>
{
    public void SetCell( TCell cell );
}