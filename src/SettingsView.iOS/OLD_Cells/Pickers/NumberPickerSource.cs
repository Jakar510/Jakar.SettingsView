using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	[Foundation.Preserve(AllMembers = true)]
	internal class NumberPickerSource : UIPickerViewModel
	{
		internal IList<int> Items { get; private set; }

		internal event EventHandler UpdatePickerFromModel;

		internal int SelectedIndex { get; set; }

		internal int SelectedItem { get; set; }

		internal int PreSelectedItem { get; set; }

		public override nint GetComponentCount( UIPickerView picker ) => 1;

		public override nint GetRowsInComponent( UIPickerView pickerView, nint component ) =>
			Items != null
				? Items.Count
				: 0;

		public override string GetTitle( UIPickerView picker, nint row, nint component ) => Items[(int) row].ToString();

		public override void Selected( UIPickerView picker, nint row, nint component )
		{
			if ( Items.Count == 0 )
			{
				SelectedItem = 0;
				SelectedIndex = -1;
			}
			else
			{
				SelectedItem = Items[(int) row];
				SelectedIndex = (int) row;
			}
		}

		public void SetNumbers( int min, int max )
		{
			if ( min < 0 ) min = 0;
			if ( max < 0 ) max = 0;
			if ( min > max )
			{
				//Set min value to zero temporally, because it is sometimes min greater than max depending on the order which min and max value is bound.
				min = 0;
			}

			Items = Enumerable.Range(min, max - min + 1).ToList();
		}

		public void OnUpdatePickerFormModel()
		{
			PreSelectedItem = SelectedItem;
			UpdatePickerFromModel?.Invoke(this, EventArgs.Empty);
		}
	}
}