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

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	[Preserve(AllMembers = true)]
	public class TextPickerCellView : LabelCellView
	{
		public UITextField DummyField { get; set; }

		private TextPickerSource _model;
		private UILabel _titleLabel;
		private UIPickerView _picker;
		private ICommand _command;

		private TextPickerCell _TextPickerCell => Cell as TextPickerCell;

		public TextPickerCellView( Cell formsCell ) : base(formsCell)
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
				_model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
				DummyField.RemoveFromSuperview();
				DummyField?.Dispose();
				DummyField = null;
				_titleLabel?.Dispose();
				_titleLabel = null;
				_model?.Dispose();
				_model = null;
				_picker?.Dispose();
				_picker = null;
				_command = null;
			}

			base.Dispose(disposing);
		}

		private void SetUpPicker()
		{
			_picker = new UIPickerView();

			var width = UIScreen.MainScreen.Bounds.Width;

			_titleLabel = new UILabel();
			_titleLabel.TextAlignment = UITextAlignment.Center;

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   DummyField.ResignFirstResponder();
													   Select(_model.PreSelectedItem);
												   }
												  );

			var labelButton = new UIBarButtonItem(_titleLabel);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 _model.OnUpdatePickerFormModel();
													 DummyField.ResignFirstResponder();
													 _command?.Execute(_model.SelectedItem);
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

			DummyField.InputView = _picker;
			DummyField.InputAccessoryView = toolbar;

			_model = new TextPickerSource();
			_picker.Model = _model;

			_model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		private void UpdateSelectedItem()
		{
			Select(_TextPickerCell.SelectedItem);
			ValueLabel.Text = _TextPickerCell.SelectedItem?.ToString();
		}

		private void UpdateItems()
		{
			var items = _TextPickerCell.Items ?? new List<string>();
			_model.SetItems(items);
			// Force picker view to reload data from model after change
			// Otherwise it might access the model based on old view data
			// causing "Index was out of range" errors and the like.
			_picker.ReloadAllComponents();
			Select(_TextPickerCell.SelectedItem);
		}

		private void UpdateTitle()
		{
			_titleLabel.Text = _TextPickerCell.Prompt.Properties.Title;
			_titleLabel.SizeToFit();
			_titleLabel.Frame = new CGRect(0, 0, 160, 44);
		}

		private void UpdateCommand() { _command = _TextPickerCell.SelectedCommand; }

		private void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_TextPickerCell.SelectedItem = _model.SelectedItem;
			ValueLabel.Text = _model.SelectedItem;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void Select( string item )
		{
			var idx = _model.Items.IndexOf(item);
			if ( idx == -1 )
			{
				item = _model.Items.Count == 0
						   ? null
						   : _model.Items[0];
				idx = 0;
			}

			_picker.Select(idx, 0, false);
			_model.SelectedItem = item;
			_model.SelectedIndex = idx;
			_model.PreSelectedItem = item;
		}
	}
}