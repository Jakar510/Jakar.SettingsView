namespace Jakar.SettingsView.Shared.Interfaces;

[Xamarin.Forms.Internals.Preserve(true, false)]
public interface ICellBase : IVisibleCell
{
    public Cell            Cell            { get; }
    public Section?        Section         { get; set; }
    public sv.SettingsView Parent          { get; set; }
    public Color           BackgroundColor { get; set; }
    public void Reload();
}