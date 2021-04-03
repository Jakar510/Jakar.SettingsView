using System;
using System.Collections.Generic;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;

#nullable enable
[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	[Preserve(AllMembers = true)]
	public class TextPickerCellView : BaseLabelCellView<TextPickerCell>
	{
		public UITextField DummyField { get; set; }

		private TextPickerSource? _Model { get; set; }
		private UILabel? _TitleLabel { get; set; }
		private UIPickerView? _Picker { get; set; }
		private ICommand? _Command { get; set; }


		public TextPickerCellView( TextPickerCell formsCell ) : base(formsCell)
		{
			DummyField = new NoCaretField();
			DummyField.BorderStyle = UITextBorderStyle.None;
			DummyField.BackgroundColor = UIColor.Clear;
			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpPicker();
		}


		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName ) { UpdateSelectedItem(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdateTitle(); }
			else if ( e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
			else if ( e.PropertyName == TextPickerCell.ItemsProperty.PropertyName ) { UpdateItems(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdateItems();
			UpdateSelectedItem();
			UpdateTitle();
			UpdateCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Model != null ) { _Model.UpdatePickerFromModel -= Model_UpdatePickerFromModel; }

				DummyField.RemoveFromSuperview();
				DummyField.Dispose();

				_TitleLabel?.Dispose();
				_TitleLabel = null;

				_Model?.Dispose();

				_Model = null;
				_Picker?.Dispose();
				_Picker = null;
				_Command = null;
			}

			base.Dispose(disposing);
		}

		private void SetUpPicker()
		{
			_Picker = new UIPickerView();

			var width = UIScreen.MainScreen.Bounds.Width;

			_TitleLabel = new UILabel();
			_TitleLabel.TextAlignment = UITextAlignment.Center;

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelHandler);

			var labelButton = new UIBarButtonItem(_TitleLabel);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneHandler);

			toolbar.SetItems(new[]
							 {
								 cancelButton,
								 spacer,
								 labelButton,
								 spacer,
								 doneButton
							 },
							 false
							);

			DummyField.InputView = _Picker;
			DummyField.InputAccessoryView = toolbar;

			_Model = new TextPickerSource();
			_Picker.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}
		private void CancelHandler( object o, EventArgs e )
		{
			DummyField.ResignFirstResponder();
			Select(_Model?.PreSelectedItem);
		}
		private void DoneHandler( object o, EventArgs a )
		{
			_Model?.OnUpdatePickerFormModel();
			DummyField.ResignFirstResponder();
			_Command?.Execute(_Model?.SelectedItem);
		}

		private void UpdateSelectedItem()
		{
			Select(Cell.SelectedItem);
			string? text = Cell.SelectedItem;
			_Value.UpdateText(text);
		}

		private void UpdateItems()
		{
			if ( _Model is null ) { throw new NullReferenceException(nameof(_Model)); }

			IList<string> items = Cell.Items ?? new List<string>();
			_Model.SetItems(items);
			// Force picker view to reload data from model after change
			// Otherwise it might access the model based on old view data
			// causing "Index was out of range" errors and the like.
			_Picker?.ReloadAllComponents();
			Select(Cell.SelectedItem);
		}

		private void UpdateTitle()
		{
			if ( _TitleLabel is null ) { return; }

			_TitleLabel.Text = Cell.Prompt.Properties.Title;
			_TitleLabel.SizeToFit();
			_TitleLabel.Frame = new CGRect(0, 0, 160, 44);
		}

		private void UpdateCommand() { _Command = Cell.SelectedCommand; }

		private void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			Cell.SelectedItem = _Model?.SelectedItem;
			string? text = _Model?.SelectedItem;
			_Value.UpdateText(text);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void Select( string? item )
		{
			if ( item is null ) { return; }

			if ( _Model is null ) { return; }

			int idx = _Model.Items.IndexOf(item);
			if ( idx == -1 )
			{
				item = _Model.Items.Count == 0
						   ? null
						   : _Model.Items[0];
				idx = 0;
			}

			_Picker?.Select(idx, 0, false);
			_Model.SelectedItem = item;
			_Model.SelectedIndex = idx;
			_Model.PreSelectedItem = item;
		}
	}
}