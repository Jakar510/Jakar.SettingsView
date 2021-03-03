﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	internal class PickerTableViewController : UITableViewController
	{
		private PickerCell _pickerCell;
		private PickerCellView _pickerCellNative;
		private Shared.SettingsView _parent;
		private IList _source;
		private Dictionary<int, object> _selectedCache = new Dictionary<int, object>();
		private UIColor _accentColor;
		private UIColor _titleColor;
		private UIColor _detailColor;
		private nfloat _fontSize;
		private nfloat _detailFontSize;
		private UIColor _background;
		private UITableView _tableView;
		private INavigation _shellNavigation;

		internal PickerTableViewController( PickerCellView pickerCellView, UITableView tableView, INavigation shellNavigation = null ) : base(UITableViewStyle.Grouped)
		{
			_pickerCell = pickerCellView.Cell as PickerCell;
			_pickerCellNative = pickerCellView;
			_parent = pickerCellView.CellParent;
			_source = _pickerCell.ItemsSource as IList;
			_tableView = tableView;
			_shellNavigation = shellNavigation;

			if ( _pickerCell.SelectedItems == null ) { _pickerCell.SelectedItems = new List<object>(); }

			SetUpProperties();
		}


		private void SetUpProperties()
		{
			if ( _pickerCell.PopupAccentColor != Color.Default ) { _accentColor = _pickerCell.PopupAccentColor.ToUIColor(); }
			else if ( _parent.CellAccentColor != Color.Default ) { _accentColor = _parent.CellAccentColor.ToUIColor(); }

			if ( _pickerCell.TitleColor != Color.Default ) { _titleColor = _pickerCell.TitleColor.ToUIColor(); }
			else if ( _parent != null &&
					  _parent.CellTitleColor != Color.Default ) { _titleColor = _parent.CellTitleColor.ToUIColor(); }

			if ( _pickerCell.TitleFontSize > 0 ) { _fontSize = (nfloat) _pickerCell.TitleFontSize; }
			else if ( _parent != null ) { _fontSize = (nfloat) _parent.CellTitleFontSize; }

			if ( _pickerCell.DescriptionColor != Color.Default ) { _detailColor = _pickerCell.DescriptionColor.ToUIColor(); }
			else if ( _parent != null &&
					  _parent.CellDescriptionColor != Color.Default ) { _detailColor = _parent.CellDescriptionColor.ToUIColor(); }

			if ( _pickerCell.DescriptionFontSize > 0 ) { _detailFontSize = (nfloat) _pickerCell.DescriptionFontSize; }
			else if ( _parent != null ) { _detailFontSize = (nfloat) _parent.CellDescriptionFontSize; }

			if ( _pickerCell.BackgroundColor != Color.Default ) { _background = _pickerCell.BackgroundColor.ToUIColor(); }
			else if ( _parent != null &&
					  _parent.CellBackgroundColor != Color.Default ) { _background = _parent.CellBackgroundColor.ToUIColor(); }
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell( UITableView tableView, NSIndexPath indexPath )
		{
			UITableViewCell reusableCell = tableView.DequeueReusableCell("pikcercell");
			if ( reusableCell == null )
			{
				reusableCell = new UITableViewCell(UITableViewCellStyle.Subtitle, "pickercell");

				reusableCell.TextLabel.TextColor = _titleColor;
				reusableCell.TextLabel.Font = reusableCell.TextLabel.Font.WithSize(_fontSize);
				reusableCell.DetailTextLabel.TextColor = _detailColor;
				reusableCell.DetailTextLabel.Font = reusableCell.DetailTextLabel.Font.WithSize(_detailFontSize);
				reusableCell.BackgroundColor = _background;
				reusableCell.TintColor = _accentColor;
			}

			object text = _pickerCell.DisplayValue(_source[indexPath.Row]);
			reusableCell.TextLabel.Text = $"{text}";
			object detail = _pickerCell.SubDisplayValue(_source[indexPath.Row]);
			reusableCell.DetailTextLabel.Text = $"{detail}";

			reusableCell.Accessory = _selectedCache.ContainsKey(indexPath.Row) ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;


			return reusableCell;
		}


		/// <summary>
		/// Numbers the of sections.
		/// </summary>
		/// <returns>The of sections.</returns>
		/// <param name="tableView">Table view.</param>
		public override nint NumberOfSections( UITableView tableView ) => 1;

		/// <summary>
		/// Rowses the in section.
		/// </summary>
		/// <returns>The in section.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="section">Section.</param>
		public override nint RowsInSection( UITableView tableView, nint section ) => _source.Count;

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			UITableViewCell cell = tableView.CellAt(indexPath);

			if ( _pickerCell.MaxSelectedNumber == 1 )
			{
				RowSelectedSingle(cell, indexPath.Row);
				DoPickToClose();
			}
			else { RowSelectedMulti(cell, indexPath.Row); }

			tableView.DeselectRow(indexPath, true);
		}

		private void RowSelectedSingle( UITableViewCell cell, int index )
		{
			if ( _selectedCache.ContainsKey(index) ) { return; }

			foreach ( UITableViewCell vCell in TableView.VisibleCells ) { vCell.Accessory = UITableViewCellAccessory.None; }

			_selectedCache.Clear();
			cell.Accessory = UITableViewCellAccessory.Checkmark;
			_selectedCache[index] = _source[index];
		}

		private void RowSelectedMulti( UITableViewCell cell, int index )
		{
			if ( _selectedCache.ContainsKey(index) )
			{
				cell.Accessory = UITableViewCellAccessory.None;
				_selectedCache.Remove(index);
				return;
			}

			if ( _pickerCell.MaxSelectedNumber != 0 &&
				 _selectedCache.Count() >= _pickerCell.MaxSelectedNumber ) { return; }

			cell.Accessory = UITableViewCellAccessory.Checkmark;
			_selectedCache[index] = _source[index];

			DoPickToClose();
		}

		private void DoPickToClose()
		{
			if ( _pickerCell.UsePickToClose &&
				 _selectedCache.Count == _pickerCell.MaxSelectedNumber )
			{
				if ( _shellNavigation != null ) { _shellNavigation.PopAsync(true); }
				else { NavigationController.PopViewController(true); }
			}
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear( bool animated )
		{
			base.ViewWillAppear(animated);

			InitializeView();
			InitializeScroll();
		}

		public void InitializeView()
		{
			Title = _pickerCell.PopupPageTitle;

			var parent = _pickerCell.Parent;
			if ( parent != null )
			{
				TableView.SeparatorColor = parent.SeparatorColor.ToUIColor();
				TableView.BackgroundColor = parent.BackgroundColor.ToUIColor();
			}
		}

		public void InitializeScroll()
		{
			IList selectedList = _pickerCell.MergedSelectedList;

			foreach ( object item in selectedList )
			{
				int idx = _source.IndexOf(item);
				if ( idx < 0 ) { continue; }

				_selectedCache[idx] = _source[idx];
				if ( _pickerCell.MaxSelectedNumber >= 1 &&
					 _selectedCache.Count >= _pickerCell.MaxSelectedNumber ) { break; }
			}

			if ( selectedList.Count > 0 )
			{
				int idx = _source.IndexOf(selectedList[0]);
				if ( idx < 0 ) { return; }

				BeginInvokeOnMainThread(() =>
										{
											TableView.ScrollToRow(NSIndexPath.Create(new nint[]
																					 {
																						 0,
																						 idx
																					 }), UITableViewScrollPosition.Middle, false);
										});
			}
		}

		/// <summary>
		/// Views the will disappear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillDisappear( bool animated )
		{
			_pickerCell.SelectedItems.Clear();

			foreach ( KeyValuePair<int, object> kv in _selectedCache ) { _pickerCell.SelectedItems.Add(kv.Value); }

			_pickerCell.SelectedItem = _selectedCache.Values.FirstOrDefault();

			//_pickerCellNative.UpdateSelectedItems(true);

			if ( _pickerCell.KeepSelectedUntilBack ) { _tableView.DeselectRow(_tableView.IndexPathForSelectedRow, true); }

			_pickerCell.InvokeCommand();
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_pickerCell = null;
				_selectedCache = null;
				_source = null;
				_parent = null;
				_accentColor.Dispose();
				_accentColor = null;
				_titleColor?.Dispose();
				_titleColor = null;
				_background?.Dispose();
				_background = null;
				_tableView = null;
			}

			base.Dispose(disposing);
		}
	}
}