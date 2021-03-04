﻿using System;
using System.Collections;
using System.Collections.Generic;
using UIKit;

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	internal class TextPickerSource : UIPickerViewModel
	{
		internal IList<string> Items { get; private set; }

		internal event EventHandler UpdatePickerFromModel;

		internal int SelectedIndex { get; set; }

		internal string SelectedItem { get; set; }

		internal string PreSelectedItem { get; set; }

		/// <summary>
		/// Gets the component count.
		/// </summary>
		/// <returns>The component count.</returns>
		/// <param name="picker">Picker.</param>
		public override nint GetComponentCount( UIPickerView picker ) => 1;

		/// <summary>
		/// Gets the rows in component.
		/// </summary>
		/// <returns>The rows in component.</returns>
		/// <param name="pickerView">Picker view.</param>
		/// <param name="component">Component.</param>
		public override nint GetRowsInComponent( UIPickerView pickerView, nint component ) => Items != null ? Items.Count : 0;

		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <returns>The title.</returns>
		/// <param name="picker">Picker.</param>
		/// <param name="row">Row.</param>
		/// <param name="component">Component.</param>
		public override string GetTitle( UIPickerView picker, nint row, nint component ) => Items[(int) row].ToString();

		/// <summary>
		/// Selected the specified picker, row and component.
		/// </summary>
		/// <returns>The selected.</returns>
		/// <param name="picker">Picker.</param>
		/// <param name="row">Row.</param>
		/// <param name="component">Component.</param>
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

		/// <summary>
		/// Sets the items.
		/// </summary>
		/// <param name="items">Items.</param>
		public void SetItems( IList<string> items ) { Items = items; }

		/// <summary>
		/// Ons the update picker form model.
		/// </summary>
		public void OnUpdatePickerFormModel()
		{
			PreSelectedItem = SelectedItem;
			UpdatePickerFromModel?.Invoke(this, EventArgs.Empty);
		}
	}
}