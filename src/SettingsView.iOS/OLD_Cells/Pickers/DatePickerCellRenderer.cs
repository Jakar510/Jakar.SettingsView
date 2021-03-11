using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// [assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)]
	// public class DatePickerCellRenderer : CellBaseRenderer<DatePickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class DatePickerCellView : LabelCellView
	{
		protected DatePickerCell _DatePickerCell => Cell as DatePickerCell;

		public UITextField DummyField { get; set; }

		protected NSDate _preSelectedDate;
		protected UIDatePicker _Dialog { get; set; }

		public DatePickerCellView( Cell formsCell ) : base(formsCell)
		{
			DummyField = new NoCaretField();
			DummyField.BorderStyle = UITextBorderStyle.None;
			DummyField.BackgroundColor = UIColor.Clear;
			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpDatePicker();
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdateMaximumDate();
			UpdateMinimumDate();
			UpdateDate();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == DatePickerCell.DateProperty.PropertyName ||
				 e.PropertyName == DatePickerCell.FormatProperty.PropertyName ) { UpdateDate(); }
			else if ( e.PropertyName == DatePickerCell.TodayTextProperty.PropertyName ) { UpdateTodayText(); }
			else if ( e.PropertyName == DatePickerCell.MaximumDateProperty.PropertyName ) { UpdateMaximumDate(); }
			else if ( e.PropertyName == DatePickerCell.MinimumDateProperty.PropertyName ) { UpdateMinimumDate(); }
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
				DummyField?.Dispose();
				DummyField = null;
				_Dialog.Dispose();
				_Dialog = null;
			}

			base.Dispose(disposing);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		protected void SetUpDatePicker()
		{
			_Dialog = new UIDatePicker
					  {
						  Mode = UIDatePickerMode.Date,
						  TimeZone = new NSTimeZone("UTC")
					  };
			if ( UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ) { _Dialog.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; }

			nfloat width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new CGRect(0, 0, width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };

			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   DummyField.ResignFirstResponder();
													   _Dialog.Date = _preSelectedDate;
												   }
												  );

			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 DummyField.ResignFirstResponder();
													 Done();
												 }
												);

			if ( !string.IsNullOrEmpty(_DatePickerCell.TodayText) )
			{
				var labelButton = new UIBarButtonItem(_DatePickerCell.TodayText, UIBarButtonItemStyle.Plain, ( sender, e ) => { SetToday(); });
				var fixSpacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace)
								{
									Width = 20
								};
				toolbar.SetItems(new[]
								 {
									 cancelButton,
									 spacer,
									 labelButton,
									 fixSpacer,
									 doneButton
								 },
								 false
								);
			}
			else
			{
				toolbar.SetItems(new[]
								 {
									 cancelButton,
									 spacer,
									 doneButton
								 },
								 false
								);
			}

			DummyField.InputView = _Dialog;
			DummyField.InputAccessoryView = toolbar;
		}

		protected void SetToday()
		{
			if ( _Dialog.MinimumDate.ToDateTime() <= DateTime.Today &&
				 _Dialog.MaximumDate.ToDateTime() >= DateTime.Today ) { _Dialog.SetDate(DateTime.Today.ToNSDate(), true); }
		}

		protected void Done()
		{
			_DatePickerCell.Date = _Dialog.Date.ToDateTime().Date;
			ValueLabel.Text = _DatePickerCell.Date.ToString(_DatePickerCell.Format);
			_preSelectedDate = _Dialog.Date;
		}

		protected void UpdateDate()
		{
			_Dialog.SetDate(_DatePickerCell.Date.ToNSDate(), false);
			ValueLabel.Text = _DatePickerCell.Date.ToString(_DatePickerCell.Format);
			_preSelectedDate = _DatePickerCell.Date.ToNSDate();
		}

		protected void UpdateMaximumDate()
		{
			_Dialog.MaximumDate = _DatePickerCell.MaximumDate.ToNSDate();
			UpdateDate(); // without this code, selected date isn't sometimes correct when it is shown first.
		}

		protected void UpdateMinimumDate()
		{
			_Dialog.MinimumDate = _DatePickerCell.MinimumDate.ToNSDate();
			UpdateDate();
		}

		protected void UpdateTodayText()
		{
			SetUpDatePicker();
			UpdateDate();
			UpdateMaximumDate();
			UpdateMinimumDate();
		}
	}
}