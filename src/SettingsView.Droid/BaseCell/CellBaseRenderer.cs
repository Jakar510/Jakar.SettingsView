namespace Jakar.SettingsView.Droid.BaseCell;

[Preserve(AllMembers = true)]
public class CellBaseRenderer<TNativeCell> : CellRenderer where TNativeCell : BaseCellView
{
    protected override Android.Views.View GetCellCore( Cell                item,
                                                       Android.Views.View? convertView,
                                                       ViewGroup           parent,
                                                       Context             context
    )
    {
        if ( convertView is not TNativeCell nativeCell )
        {
            nativeCell = InstanceCreator<TNativeCell>.Create(context, item);

            // nativeCell = InstanceCreator<Context, Xamarin.Forms.Cell, TNativeCell>.Create(context, item);
        }

        ClearPropertyChanged(nativeCell);

        nativeCell.Cell = item;

        SetUpPropertyChanged(nativeCell);

        nativeCell.UpdateCell();

        // base.GetCellCore()
        return nativeCell;
    }

    // protected override void OnCellPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.OnCellPropertyChanged(sender, e); }

    protected void SetUpPropertyChanged( BaseCellView nativeCell )
    {
        if ( nativeCell.Cell is not CellBase formsCell ) return;
        Shared.sv.SettingsView? parentElement = formsCell.Parent;

        formsCell.PropertyChanged += nativeCell.CellPropertyChanged;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if ( parentElement is null ) return;
        parentElement.PropertyChanged += nativeCell.ParentPropertyChanged;
        Section? section = parentElement.Model.GetSectionFromCell(formsCell);
        if ( section is null ) return;
        formsCell.Section                 =  section;
        formsCell.Section.PropertyChanged += nativeCell.SectionPropertyChanged;
    }

    protected void ClearPropertyChanged( BaseCellView nativeCell )
    {
        if ( nativeCell.Cell is not CellBase formsCell ) return;
        Shared.sv.SettingsView? parentElement = formsCell.Parent;

        formsCell.PropertyChanged -= nativeCell.CellPropertyChanged;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if ( parentElement is null ) return;
        parentElement.PropertyChanged -= nativeCell.ParentPropertyChanged;
        if ( formsCell.Section != null ) { formsCell.Section.PropertyChanged -= nativeCell.SectionPropertyChanged; }
    }
}
