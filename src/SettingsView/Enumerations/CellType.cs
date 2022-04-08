namespace Jakar.SettingsView.Shared.Enumerations;

[Xamarin.Forms.Internals.Preserve(true, false)]
public enum CellType
{
    Unknown,

    // SettingsView
    LabelCell,
    ButtonCell,
    CommandCell,
    EntryCell,
    CustomCell,
    RadioCell,
    CheckboxCell,
    SwitchCell,
    DatePickerCell,
    NumberPickerCell,
    PickerCell,
    TextPickerCell,
    TimePickerCell,

    // Xamarin.Forms
    EntryCellForms,
    ViewCellForms,
    ImageCellForms,
    SwitchCellForms,
    TextCellForms,


    SpacerCell, // not implemented yet
    EditorCell, // not implemented yet
    IPCell,     // not implemented yet

    // URLCell, // possible?
    // ImageCell, // possible?
}