// unset

using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

#nullable enable
namespace Jakar.SettingsView.iOS.Cells.Sources
{
	[Foundation.Preserve(AllMembers = true)]
	public class BasePickerSource<TValue> : UIPickerViewModel
	{
		protected internal IList<TValue> Items { get; private set; } = new List<TValue>();

		protected internal int SelectedIndex { get; set; }
		protected internal TValue? SelectedItem { get; set; }
		protected internal TValue? PreSelectedItem { get; set; }


		protected internal event EventHandler? UpdatePickerFromModel;

		public override nint GetComponentCount( UIPickerView picker ) => 1;
		public override nint GetRowsInComponent( UIPickerView pickerView, nint component ) => Items.Count;

		public override string GetTitle( UIPickerView picker, nint row, nint component ) => Items[(int) row]?.ToString() ?? string.Empty;

		public override void Selected( UIPickerView picker, nint row, nint component )
		{
			if ( !Items.Any() )
			{
				SelectedItem = default;
				SelectedIndex = -1;
			}
			else
			{
				SelectedItem = Items[(int) row];
				SelectedIndex = (int) row;
			}
		}

		public void SetItems( IList<TValue> items ) { Items = items; }

		public void OnUpdatePickerFormModel()
		{
			PreSelectedItem = SelectedItem;
			UpdatePickerFromModel?.Invoke(this, EventArgs.Empty);
		}
	}
}