using System;
using System.Collections.Generic;
using System.Text;
using Jakar.SettingsView.Shared.Misc;

namespace Jakar.SettingsView.Shared.Enumerations
{
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
		EntryCell_Forms,
		ViewCell_Forms,
		ImageCell_Forms,
		SwitchCell_Forms,
		TextCell_Forms,


		SpacerCell, // not implemented yet
		EditorCell, // not implemented yet
		IPCell,     // not implemented yet

		// URLCell, // possible?
		// ImageCell, // possible?
	}
}