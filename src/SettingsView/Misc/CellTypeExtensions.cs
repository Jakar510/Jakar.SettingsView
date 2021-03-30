// unset

using Jakar.Api.Extensions;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Enumerations;

#nullable enable
namespace Jakar.SettingsView.Shared.Misc
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public static class CellTypeExtensions
	{
		public static CellType ToCellType( this Xamarin.Forms.Cell cell )
		{
			return cell switch
				   {
					   TimePickerCell _ => CellType.TimePickerCell,
					   DatePickerCell _ => CellType.DatePickerCell,
					   NumberPickerCell _ => CellType.NumberPickerCell,
					   TextPickerCell _ => CellType.TextPickerCell,
					   PickerCell _ => CellType.PickerCell,
					   LabelCell _ => CellType.LabelCell,
					   CheckboxCell _ => CellType.CheckboxCell,
					   RadioCell _ => CellType.RadioCell,
					   SwitchCell _ => CellType.SwitchCell,
					   ButtonCell _ => CellType.ButtonCell,
					   CustomCell _ => CellType.CustomCell,
					   CommandCell _ => CellType.CommandCell,
					   EntryCell _ => CellType.EntryCell,

					   // SpacerCell _ => CellType.SpacerCell,
					   // EditorCell _ => CellType.EditorCell,

					   Xamarin.Forms.EntryCell _ => CellType.EntryCell_Forms,
					   Xamarin.Forms.ViewCell _ => CellType.ViewCell_Forms,
					   Xamarin.Forms.ImageCell _ => CellType.ImageCell_Forms,
					   Xamarin.Forms.SwitchCell _ => CellType.SwitchCell_Forms,
					   Xamarin.Forms.TextCell _ => CellType.TextCell_Forms,

					   _ => CellType.Unknown,
				   };
		}
		

		internal static bool IsTitleOnlyCell( this Xamarin.Forms.Cell cell ) => cell.ToCellType().IsEqual(CellType.ButtonCell);
		internal static bool IsFormsCell( this Xamarin.Forms.Cell cell ) =>
			cell.ToCellType()
				.IsOneOf(CellType.EntryCell_Forms,
						 CellType.ImageCell_Forms,
						 CellType.ViewCell_Forms,
						 CellType.SwitchCell_Forms,
						 CellType.TextCell_Forms
						);
		internal static bool IsCommandCell( this Xamarin.Forms.Cell cell ) => cell.ToCellType().IsOneOf(CellType.CommandCell, CellType.CustomCell, CellType.ButtonCell);
		internal static bool IsDescriptiveTitleCell( this Xamarin.Forms.Cell cell ) =>
			cell.ToCellType()
				.IsOneOf(CellType.CommandCell,
						 CellType.CustomCell,
						 CellType.SwitchCell,
						 CellType.RadioCell,
						 CellType.CheckboxCell
						);
		internal static bool IsAccessoryCell( this Xamarin.Forms.Cell cell ) =>
			cell.ToCellType()
				.IsOneOf(CellType.SwitchCell,
						 CellType.RadioCell,
						 CellType.CheckboxCell,
						 CellType.CustomCell,
						 CellType.CommandCell
						);
		internal static bool IsPickerCell( this Xamarin.Forms.Cell cell ) =>
			cell.ToCellType()
				.IsOneOf(CellType.PickerCell,
						 CellType.TextPickerCell,
						 CellType.NumberPickerCell,
						 CellType.TimePickerCell,
						 CellType.DatePickerCell
						);
		internal static bool IsValueCell( this Xamarin.Forms.Cell cell ) =>
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
}