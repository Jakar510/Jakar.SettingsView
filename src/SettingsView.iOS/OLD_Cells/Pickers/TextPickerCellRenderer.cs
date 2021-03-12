using System;
using System.Collections.Generic;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;

// [assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class TextPickerCellView : LabelCellView
	{
		protected UITextField _DummyField { get; set; }

		protected TextPickerSource? _Model { get; set; }
		protected UILabel? _PopupTitle { get; set; }
		protected UIPickerView? _Picker { get; set; }
		protected ICommand? _Command { get; set; }

		protected TextPickerCell _TextPickerCell => Cell as TextPickerCell;

		public TextPickerCellView( Cell formsCell ) : base(formsCell)
		{
			_DummyField = new NoCaretField
						  {
							  BorderStyle = UITextBorderStyle.None,
							  BackgroundColor = UIColor.Clear
						  };
			ContentView.AddSubview(_DummyField);
			ContentView.SendSubviewToBack(_DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpPicker();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName ) { UpdateSelectedItem(); }
			else if ( e.PropertyName == TitleCellBase.TitleProperty.PropertyName ) { UpdateTitle(); }
			else if ( e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
			else if ( e.PropertyName == TextPickerCell.ItemsProperty.PropertyName ) { UpdateItems(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			_DummyField.BecomeFirstResponder();
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
				_Model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
				_DummyField.RemoveFromSuperview();
				_DummyField?.Dispose();
				_DummyField = null;
				_PopupTitle?.Dispose();
				_PopupTitle = null;
				_Model?.Dispose();
				_Model = null;
				_Picker?.Dispose();
				_Picker = null;
				_Command = null;
			}

			base.Dispose(disposing);
		}

		protected void SetUpPicker()
		{
			_Picker = new UIPickerView();

			nfloat width = UIScreen.MainScreen.Bounds.Width;

			_PopupTitle = new UILabel();
			_PopupTitle.TextAlignment = UITextAlignment.Center;

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   _DummyField.ResignFirstResponder();
													   Select(_Model.PreSelectedItem);
												   }
												  );

			var labelButton = new UIBarButtonItem(_PopupTitle);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 _Model.OnUpdatePickerFormModel();
													 _DummyField.ResignFirstResponder();
													 _Command?.Execute(_Model.SelectedItem);
												 }
												);

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

			_DummyField.InputView = _Picker;
			_DummyField.InputAccessoryView = toolbar;

			_Model = new TextPickerSource();
			_Picker.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		protected void UpdateSelectedItem()
		{
			Select(_TextPickerCell.SelectedItem);
			ValueLabel.Text = _TextPickerCell.SelectedItem?.ToString();
		}

		protected void UpdateItems()
		{
			IList<string> items = _TextPickerCell.Items ?? new List<string>();
			_Model.SetItems(items);
			// Force picker view to reload data from model after change
			// Otherwise it might access the model based on old view data
			// causing "Index was out of range" errors and the like.
			_Picker.ReloadAllComponents();
			Select(_TextPickerCell.SelectedItem);
		}

		protected void UpdateTitle()
		{
			_PopupTitle.Text = _TextPickerCell.Title;
			_PopupTitle.SizeToFit();
			_PopupTitle.Frame = new CGRect(0, 0, 160, 44);
		}

		protected void UpdateCommand() { _Command = _TextPickerCell.SelectedCommand; }

		protected void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_TextPickerCell.SelectedItem = _Model.SelectedItem;
			ValueLabel.Text = _Model.SelectedItem?.ToString();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			_DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		protected void Select( string? item )
		{
			int idx = _Model.Items.IndexOf(item);
			if ( idx == -1 )
			{
				item = _Model.Items.Count == 0
						   ? null
						   : _Model.Items[0];
				idx = 0;
			}

			_Picker.Select(idx, 0, false);
			_Model.SelectedItem = item;
			_Model.SelectedIndex = idx;
			_Model.PreSelectedItem = item;
		}
	}
}