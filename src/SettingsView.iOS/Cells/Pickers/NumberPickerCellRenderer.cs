using System;
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

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }

	[Preserve(AllMembers = true)]
	public class NumberPickerCellView : LabelCellView
	{
		public UITextField DummyField { get; set; }

		private NumberPickerSource _model;
		private UILabel _titleLabel;
		private UIPickerView _picker;
		private ICommand _command;

		private NumberPickerCell _NumberPikcerCell => Cell as NumberPickerCell;

		public NumberPickerCellView( Cell formsCell ) : base(formsCell)
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
			if ( e.PropertyName == NumberPickerCell.MinProperty.PropertyName ||
				 e.PropertyName == NumberPickerCell.MaxProperty.PropertyName ) { UpdateNumberList(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdateTitle(); }
			else if ( e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			if ( DummyField is null )
				return; // For HotReload

			UpdateNumberList();
			UpdateNumber();
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

			_model = new NumberPickerSource();
			_picker.Model = _model;

			_model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		private void UpdateNumber()
		{
			Select(_NumberPikcerCell.Number);
			ValueLabel.Text = _NumberPikcerCell.Number.ToString();
		}

		private void UpdateNumberList()
		{
			_model.SetNumbers(_NumberPikcerCell.Min, _NumberPikcerCell.Max);
			Select(_NumberPikcerCell.Number);
		}

		private void UpdateTitle()
		{
			_titleLabel.Text = _NumberPikcerCell.Prompt.Properties.Title;
			_titleLabel.SizeToFit();
			_titleLabel.Frame = new CGRect(0, 0, 160, 44);
		}

		private void UpdateCommand() { _command = _NumberPikcerCell.SelectedCommand; }

		private void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_NumberPikcerCell.Number = _model.SelectedItem;
			ValueLabel.Text = _model.SelectedItem.ToString();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if ( DummyField is null )
				return; // For HotReload

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void Select( int number )
		{
			var idx = _model.Items.IndexOf(number);
			if ( idx == -1 )
			{
				number = _model.Items[0];
				idx = 0;
			}

			_picker.Select(idx, 0, false);
			_model.SelectedItem = number;
			_model.SelectedIndex = idx;
			_model.PreSelectedItem = number;
		}
	}
}