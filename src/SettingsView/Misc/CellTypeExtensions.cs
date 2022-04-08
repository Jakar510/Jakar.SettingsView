// unset

namespace Jakar.SettingsView.Shared.Misc;

[Xamarin.Forms.Internals.Preserve(true, false)]
public static class CellTypeExtensions
{
    public static CellType ToCellType( this Cell cell )
    {
        return cell switch
               {
                   // SpacerCell _ => CellType.SpacerCell,
                   // EditorCell _ => CellType.EditorCell,
                   // IPCell _ => CellType.IPCell,
                   TimePickerCell   => CellType.TimePickerCell,
                   DatePickerCell   => CellType.DatePickerCell,
                   NumberPickerCell => CellType.NumberPickerCell,
                   TextPickerCell   => CellType.TextPickerCell,
                   PickerCell       => CellType.PickerCell,
                   LabelCell        => CellType.LabelCell,
                   CheckboxCell     => CellType.CheckboxCell,
                   RadioCell        => CellType.RadioCell,
                   SwitchCell       => CellType.SwitchCell,
                   ButtonCell       => CellType.ButtonCell,
                   CustomCell       => CellType.CustomCell,
                   CommandCell      => CellType.CommandCell,
                   EntryCell        => CellType.EntryCell,
                   XfSwitchCell     => CellType.EntryCellForms,
                   XfViewCell       => CellType.ViewCellForms,
                   XfImageCell      => CellType.ImageCellForms,
                   XfEntryCell      => CellType.SwitchCellForms,
                   XfTextCell       => CellType.TextCellForms,
                   _                => CellType.Unknown,
               };
    }


    internal static bool IsOneOf( this CellType cell, params CellType[] options ) => options.Any(item => item == cell);

    internal static bool IsTitleOnlyCell( this Cell cell ) => cell.ToCellType().IsEqual(CellType.ButtonCell);

    internal static bool IsFormsCell( this Cell cell )
    {
        var type = cell.ToCellType();

        return type.IsOneOf(CellType.EntryCellForms,
                            CellType.ImageCellForms,
                            CellType.ViewCellForms,
                            CellType.SwitchCellForms,
                            CellType.TextCellForms
                           );
    }

    internal static bool IsCommandCell( this Cell cell ) => cell.ToCellType().IsOneOf(CellType.CommandCell, CellType.CustomCell, CellType.ButtonCell);

    internal static bool IsDescriptiveTitleCell( this Cell cell ) =>
        cell.ToCellType()
            .IsOneOf(CellType.CommandCell,
                     CellType.CustomCell,
                     CellType.SwitchCell,
                     CellType.RadioCell,
                     CellType.CheckboxCell
                    );

    internal static bool IsAccessoryCell( this Cell cell ) =>
        cell.ToCellType()
            .IsOneOf(CellType.SwitchCell,
                     CellType.RadioCell,
                     CellType.CheckboxCell,
                     CellType.CustomCell,
                     CellType.CommandCell
                    );

    internal static bool IsPickerCell( this Cell cell ) =>
        cell.ToCellType()
            .IsOneOf(CellType.PickerCell,
                     CellType.TextPickerCell,
                     CellType.NumberPickerCell,
                     CellType.TimePickerCell,
                     CellType.DatePickerCell
                    );

    internal static bool IsValueCell( this Cell cell ) =>
        cell.ToCellType()
            .IsOneOf(CellType.PickerCell,
                     CellType.TextPickerCell,
                     CellType.NumberPickerCell,
                     CellType.TimePickerCell,
                     CellType.DatePickerCell,
                     CellType.EntryCell,
                     CellType.LabelCell
                    );
}
