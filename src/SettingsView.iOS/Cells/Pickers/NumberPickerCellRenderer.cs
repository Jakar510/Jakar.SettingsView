﻿[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }



	[Preserve(AllMembers = true)]
	public class NumberPickerCellView : BaseLabelCellView<NumberPickerCell>
	{
		public UITextField DummyField { get; set; }

		private NumberPickerSource? _Model { get; set; }
		private UILabel? _TitleLabel { get; set; }
		private UIPickerView? _Picker { get; set; }
		private ICommand? _Command { get; set; }


		public NumberPickerCellView( NumberPickerCell formsCell ) : base(formsCell)
		{
			DummyField = new NoCaretField
						 {
							 BorderStyle     = UITextBorderStyle.None,
							 BackgroundColor = UIColor.Clear
						 };

			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpPicker();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == NumberPickerCell.minProperty.PropertyName ||
				 e.PropertyName == NumberPickerCell.maxProperty.PropertyName ) { UpdateNumberList(); }
			else if ( e.PropertyName == NumberPickerCell.numberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == PopupConfig.titleProperty.PropertyName ) { UpdateTitle(); }
			else if ( e.PropertyName == NumberPickerCell.selectedCommandProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);

			UpdateNumberList();
			UpdateNumber();
			UpdateTitle();
			UpdateCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Model != null )
				{
					_Model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
					DummyField.RemoveFromSuperview();
					DummyField.Dispose();
					_TitleLabel?.Dispose();
					_TitleLabel = null;
					_Model?.Dispose();
				}

				_Model = null;
				_Picker?.Dispose();
				_Picker  = null;
				_Command = null;
			}

			base.Dispose(disposing);
		}

		private void SetUpPicker()
		{
			_Picker = new UIPickerView();

			nfloat width = UIScreen.MainScreen.Bounds.Width;

			_TitleLabel = new UILabel
						  {
							  TextAlignment = UITextAlignment.Center
						  };

			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle    = UIBarStyle.Default,
							  Translucent = true
						  };

			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   CancelHandler
												  );

			var labelButton = new UIBarButtonItem(_TitleLabel);
			var spacer      = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 DoneHandler
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

			DummyField.InputView          = _Picker;
			DummyField.InputAccessoryView = toolbar;

			_Model        = new NumberPickerSource();
			_Picker.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		private void CancelHandler( object o, EventArgs e )
		{
			DummyField.ResignFirstResponder();
			Select(_Model?.PreSelectedItem ?? -1);
		}

		private void DoneHandler( object o, EventArgs a )
		{
			if ( _Model is null ) return;
			_Model.OnUpdatePickerFormModel();
			DummyField.ResignFirstResponder();
			_Command?.Execute(_Model.SelectedItem);
		}

		private void UpdateNumber()
		{
			Select(Cell.Number);
			string text = Cell.Number.ToString();
			_Value.UpdateText(text);
		}

		private void UpdateNumberList()
		{
			_Model?.SetNumbers(Cell.Min, Cell.Max);
			Select(Cell.Number);
		}

		private void UpdateTitle()
		{
			if ( _TitleLabel is null ) return;
			_TitleLabel.Text = Cell.Prompt.Properties.Title;
			_TitleLabel.SizeToFit();
			_TitleLabel.Frame = new CGRect(0, 0, 160, 44);
		}

		private void UpdateCommand() { _Command = Cell.SelectedCommand; }

		private void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			if ( _Model is null ) return;
			Cell.Number = _Model.SelectedItem;
			string text = _Model.SelectedItem.ToString();
			_Value.UpdateText(text);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void Select( int number )
		{
			if ( _Model is null ) return;
			int idx = _Model.Items.IndexOf(number);

			if ( idx == -1 )
			{
				number = _Model.Items[0];
				idx    = 0;
			}

			_Picker?.Select(idx, 0, false);
			_Model.SelectedItem    = number;
			_Model.SelectedIndex   = idx;
			_Model.PreSelectedItem = number;
		}
	}
}
