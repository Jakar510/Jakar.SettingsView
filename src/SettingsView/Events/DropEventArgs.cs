namespace Jakar.SettingsView.Shared.Events;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class DropEventArgs : EventArgs
{
    public Section Section       { get; }
    public Cell    Cell          { get; }
    public object  SectionSource { get; }
    public object  CellSource    { get; }

    public DropEventArgs( Section section, Cell cell )
    {
        Section       = section;
        Cell          = cell;
        SectionSource = section.BindingContext;
        CellSource    = cell.BindingContext;
    }
}