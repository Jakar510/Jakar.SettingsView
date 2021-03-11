using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// [assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class TimePickerCellView : LabelCellView
	{
		protected TimePickerCell _TimePickerCell => Cell as TimePickerCell;
		protected UIDatePicker _Picker { get; set; }

		protected UITextField _DummyField { get; set; }

		protected UILabel _PickerTitle { get; set; }
		protected NSDate _preSelectedDate;

		public TimePickerCellView( Cell formsCell ) : base(formsCell)
		{
			_DummyField = new NoCaretField();
			_DummyField.BorderStyle = UITextBorderStyle.None;
			_DummyField.BackgroundColor = UIColor.Clear;
			ContentView.AddSubview(_DummyField);
			ContentView.SendSubviewToBack(_DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpTimePicker();
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdatePopupTitle();
			UpdateTime();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == TimePickerCell.TimeProperty.PropertyName ||
				 e.PropertyName == TimePickerCell.FormatProperty.PropertyName ) { UpdateTime(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdatePopupTitle(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			_DummyField.BecomeFirstResponder();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_DummyField.RemoveFromSuperview();
				_DummyField?.Dispose();
				_DummyField = null;
				_Picker.Dispose();
				_Picker = null;
				_PickerTitle?.Dispose();
				_PickerTitle = null;
			}

			base.Dispose(disposing);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			_DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		protected void SetUpTimePicker()
		{
			_Picker = new UIDatePicker
					  {
						  Mode = UIDatePickerMode.Time,
						  TimeZone = new NSTimeZone("UTC")
					  };
			if ( UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ) { _Picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; }

			_PickerTitle = new UILabel();
			_PickerTitle.TextAlignment = UITextAlignment.Center;

			nfloat width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   _DummyField.ResignFirstResponder();
													   Canceled();
												   }
												  );

			var labelButton = new UIBarButtonItem(_PickerTitle);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 _DummyField.ResignFirstResponder();
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

			_DummyField.InputView = _Picker;
			_DummyField.InputAccessoryView = toolbar;
		}

		protected void Canceled() { _Picker.Date = _preSelectedDate; }

		protected void Done()
		{
			_TimePickerCell.Time = _Picker.Date.ToDateTime() - new DateTime(1, 1, 1);
			ValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
			_preSelectedDate = _Picker.Date;
		}

		protected void UpdateTime()
		{
			_Picker.Date = new DateTime(1, 1, 1).Add(_TimePickerCell.Time).ToNSDate();
			ValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
			_preSelectedDate = _Picker.Date;
		}

		protected void UpdatePopupTitle()
		{
			_PickerTitle.Text = _TimePickerCell.Prompt.Title;
			_PickerTitle.SizeToFit();
			_PickerTitle.Frame = new CGRect(0, 0, 160, 44);
		}
	}
}