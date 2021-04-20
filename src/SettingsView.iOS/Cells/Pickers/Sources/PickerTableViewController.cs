using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Cells.Sources
{
	[Preserve(AllMembers = true)]
	internal class PickerTableViewController : UITableViewController
	{
		protected PickerCell _PickerCell { get; }
		protected PickerCellView _PickerCellNative { get; set; }
		protected Dictionary<int, object> _SelectedCache { get; } = new();
		protected Shared.sv.SettingsView _Parent => _PickerCell.Parent;
		protected IList _Source => _PickerCell.ItemsSource ?? throw new NullReferenceException(nameof(_PickerCell.ItemsSource));
		protected UIColor _AccentColor => _PickerCell.Prompt.Properties.AccentColor.ToUIColor();
		protected UIColor _SeparatorColor => _PickerCell.Prompt.Properties.SeparatorColor.ToUIColor();
		protected UIColor _BackgroundColor => _PickerCell.Prompt.Properties.BackgroundColor.ToUIColor();

		protected string _Title => _PickerCell.Prompt.Properties.Title;
		protected UIColor _TitleColor => _PickerCell.Prompt.Properties.TitleColor.ToUIColor();
		protected nfloat _TitleFontSize => _PickerCell.Prompt.Properties.TitleFontSize.ToNFloat();

		protected UIColor _DetailColor => _PickerCell.Prompt.Properties.ItemDescriptionColor.ToUIColor();
		protected nfloat _DetailFontSize => _PickerCell.Prompt.Properties.ItemDescriptionFontSize.ToNFloat();
		private readonly UITableView _tableView;
		protected INavigation? _ShellNavigation { get; set; }

		internal PickerTableViewController( PickerCellView pickerCellView, UITableView tableView, INavigation? shellNavigation = null ) : base(UITableViewStyle.Grouped)
		{
			_PickerCell = pickerCellView.Cell as PickerCell ?? throw new NullReferenceException(nameof(pickerCellView.Cell));
			_PickerCellNative = pickerCellView;
			_tableView = tableView;
			_ShellNavigation = shellNavigation;

			_PickerCell.SelectedItems ??= new List<object>();
		}


		public override UITableViewCell GetCell( UITableView tableView, NSIndexPath indexPath )
		{
			UITableViewCell? reusableCell = tableView.DequeueReusableCell(nameof(PickerCell));
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if ( reusableCell is null )
			{
				reusableCell = new UITableViewCell(UITableViewCellStyle.Subtitle, nameof(PickerCell));

				reusableCell.TextLabel.TextColor = _TitleColor;
				reusableCell.TextLabel.Font = reusableCell.TextLabel.Font.WithSize(_TitleFontSize);
				reusableCell.DetailTextLabel.TextColor = _DetailColor;
				reusableCell.DetailTextLabel.Font = reusableCell.DetailTextLabel.Font.WithSize(_DetailFontSize);
				reusableCell.BackgroundColor = _BackgroundColor;
				reusableCell.TintColor = _AccentColor;
			}

			object? text = _PickerCell.DisplayValue(_Source[indexPath.Row]);
			reusableCell.TextLabel.Text = $"{text}";
			object? detail = _PickerCell.SubDisplayValue(_Source[indexPath.Row]);
			reusableCell.DetailTextLabel.Text = $"{detail}";

			reusableCell.Accessory = _SelectedCache.ContainsKey(indexPath.Row)
										 ? UITableViewCellAccessory.Checkmark
										 : UITableViewCellAccessory.None;


			return reusableCell;
		}


		public override nint NumberOfSections( UITableView tableView ) => 1;

		public override nint RowsInSection( UITableView tableView, nint section ) => _Source.Count;

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			UITableViewCell cell = tableView.CellAt(indexPath);

			if ( _PickerCell.MaxSelectedNumber == 1 )
			{
				RowSelectedSingle(cell, indexPath.Row);
				DoPickToClose();
			}
			else { RowSelectedMulti(cell, indexPath.Row); }

			tableView.DeselectRow(indexPath, true);
		}

		protected void RowSelectedSingle( UITableViewCell cell, int index )
		{
			if ( _SelectedCache.ContainsKey(index) ) { return; }

			foreach ( var vCell in TableView.VisibleCells ) { vCell.Accessory = UITableViewCellAccessory.None; }

			_SelectedCache.Clear();
			cell.Accessory = UITableViewCellAccessory.Checkmark;
			_SelectedCache[index] = _Source[index];
		}

		protected void RowSelectedMulti( UITableViewCell cell, int index )
		{
			if ( _SelectedCache.ContainsKey(index) )
			{
				cell.Accessory = UITableViewCellAccessory.None;
				_SelectedCache.Remove(index);
				return;
			}

			if ( _PickerCell.MaxSelectedNumber != 0 &&
				 _SelectedCache.Count() >= _PickerCell.MaxSelectedNumber ) { return; }

			cell.Accessory = UITableViewCellAccessory.Checkmark;
			_SelectedCache[index] = _Source[index];

			DoPickToClose();
		}

		protected void DoPickToClose()
		{
			if ( _PickerCell.UsePickToClose &&
				 _SelectedCache.Count == _PickerCell.MaxSelectedNumber )
			{
				if ( _ShellNavigation is not null ) { _ShellNavigation.PopAsync(true); }
				else { NavigationController.PopViewController(true); }
			}
		}

		public override void ViewWillAppear( bool animated )
		{
			base.ViewWillAppear(animated);

			InitializeView();
			InitializeScroll();
		}

		public void InitializeView()
		{
			Title = _Title;

			TableView.SeparatorColor = _SeparatorColor;
			TableView.BackgroundColor = _BackgroundColor;
		}

		public void InitializeScroll()
		{
			IList? selectedList = _PickerCell.MergedSelectedList;

			if ( selectedList is null ) return;
			foreach ( var item in selectedList )
			{
				int idx = _Source.IndexOf(item);
				if ( idx < 0 ) { continue; }

				_SelectedCache[idx] = _Source[idx];
				if ( _PickerCell.MaxSelectedNumber >= 1 &&
					 _SelectedCache.Count >= _PickerCell.MaxSelectedNumber ) { break; }
			}

			if ( selectedList.Count <= 0 ) return;
			int index = _Source.IndexOf(selectedList[0]);
			if ( index < 0 ) { return; }

			BeginInvokeOnMainThread(() =>
									{
										TableView.ScrollToRow(NSIndexPath.Create(new nint[]
																				 {
																					 0,
																					 index
																				 }
																				),
															  UITableViewScrollPosition.Middle,
															  false
															 );
									}
								   );
		}

		public override void ViewWillDisappear( bool animated )
		{
			_PickerCell.SelectedItems ??= new List<object>();
			_PickerCell.SelectedItems.Clear();

			foreach ( KeyValuePair<int, object> kv in _SelectedCache ) { _PickerCell.SelectedItems.Add(kv.Value); }

			_PickerCell.SelectedItem = _SelectedCache.Values.FirstOrDefault();

			//_pickerCellNative.UpdateSelectedItems(true);

			if ( _PickerCell.KeepSelectedUntilBack ) { _tableView.DeselectRow(_tableView.IndexPathForSelectedRow, true); }
			
			_PickerCell.SendValueChanged();
			_PickerCell.InvokeSelectedCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _SelectedCache.Clear(); }

			base.Dispose(disposing);
		}
	}
}