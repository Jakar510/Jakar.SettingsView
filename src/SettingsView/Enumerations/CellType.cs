using System;
using System.Collections.Generic;
using System.Text;

namespace Jakar.SettingsView.Shared.Enumerations
{
	public enum CellType
	{
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

		EditorCell, // not implemented yet
	}
}
