﻿namespace Jakar.SettingsView.iOS.Cells.Sources
{
	[Foundation.Preserve(AllMembers = true)]
	internal class TextPickerSource : UIPickerViewModel
	{
		internal IList<string> Items { get; private set; } = new List<string>();

		internal event EventHandler? UpdatePickerFromModel;

		internal int SelectedIndex { get; set; }

		internal string? SelectedItem { get; set; }

		internal string? PreSelectedItem { get; set; }

		public override nint GetComponentCount( UIPickerView picker ) => 1;

		public override nint GetRowsInComponent( UIPickerView pickerView, nint component ) => Items.Count;
		public override string GetTitle( UIPickerView picker, nint row, nint component ) => Items[(int) row];

		public override void Selected( UIPickerView picker, nint row, nint component )
		{
			if ( Items.Count == 0 )
			{
				SelectedItem = null;
				SelectedIndex = -1;
			}
			else
			{
				SelectedItem = Items[(int) row];
				SelectedIndex = (int) row;
			}
		}

		public void SetItems( IList<string> items ) { Items = items; }

		public void OnUpdatePickerFormModel()
		{
			PreSelectedItem = SelectedItem;
			UpdatePickerFromModel?.Invoke(this, EventArgs.Empty);
		}
	}
}