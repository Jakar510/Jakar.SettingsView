[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

	[Preserve(AllMembers = true)]
	public class TimePickerCellView : BaseLabelCellView<TimePickerCell>
	{
		private UIDatePicker? _Picker { get; set; }

		public UITextField DummyField { get; set; }

		private UILabel? _TitleLabel { get; set; }
		private NSDate? _PreSelectedDate { get; set; }


		public TimePickerCellView( TimePickerCell formsCell ) : base(formsCell)
		{
			DummyField = new NoCaretField();
			DummyField.BorderStyle = UITextBorderStyle.None;
			DummyField.BackgroundColor = UIColor.Clear;
			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpTimePicker();
		}


		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdatePickerTitle();
			UpdateTime();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == TimePickerCell.timeProperty.PropertyName ||
				 e.PropertyName == TimePickerCell.formatProperty.PropertyName ) { UpdateTime(); }
			else if ( e.PropertyName == PopupConfig.titleProperty.PropertyName ) { UpdatePickerTitle(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				DummyField.RemoveFromSuperview();
				DummyField.Dispose();

				_Picker?.Dispose();
				_Picker = null;

				_TitleLabel?.Dispose();
				_TitleLabel = null;
			}

			base.Dispose(disposing);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void SetUpTimePicker()
		{
			_Picker = new UIDatePicker
					  {
						  Mode = UIDatePickerMode.Time,
						  TimeZone = new NSTimeZone("UTC")
					  };
			if ( UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ) { _Picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; }

			_TitleLabel = new UILabel();
			_TitleLabel.TextAlignment = UITextAlignment.Center;

			var width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   DummyField.ResignFirstResponder();
													   Canceled();
												   }
												  );

			var labelButton = new UIBarButtonItem(_TitleLabel);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 DummyField.ResignFirstResponder();
													 Done();
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

			DummyField.InputView = _Picker;
			DummyField.InputAccessoryView = toolbar;
		}

		private void Canceled()
		{
			if ( _Picker is null ) return;
			if ( _PreSelectedDate != null ) { _Picker.Date = _PreSelectedDate; }
		}

		private void Done()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			Cell.Time = _Picker.Date.ToDateTime() - new DateTime(1, 1, 1);
			var text = DateTime.Today.Add(Cell.Time).ToString(Cell.Format);
			_Value.UpdateText(text);
			_PreSelectedDate = _Picker.Date;
		}

		private void UpdateTime()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			_Picker.Date = new DateTime(1, 1, 1).Add(Cell.Time).ToNSDate();
			var text = DateTime.Today.Add(Cell.Time).ToString(Cell.Format);
			_Value.UpdateText(text);
			_PreSelectedDate = _Picker.Date;
		}

		private void UpdatePickerTitle()
		{
			if ( _TitleLabel is null ) { return; }

			_TitleLabel.Text = Cell.Prompt.Properties.Title;
			_TitleLabel.SizeToFit();
			_TitleLabel.Frame = new CGRect(0, 0, 160, 44);
		}
	}
}