using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
[assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Date picker cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class DatePickerCellRenderer : CellBaseRenderer<DatePickerCellView> { }

	/// <summary>
	/// Date picker cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class DatePickerCellView : BaseLabelCellView<DatePickerCell>
	{

		public UITextField DummyField { get; set; }

		private NSDate? _PreSelectedDate { get; set; }
		private UIDatePicker? _Picker { get; set; }


		public DatePickerCellView( DatePickerCell formsCell ) : base(formsCell)
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

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void SetUpDatePicker()
		{
			_Picker = new UIDatePicker
					  {
						  Mode = UIDatePickerMode.Date,
						  TimeZone = new NSTimeZone("UTC")
					  };
			if ( UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ) { _Picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; }

			var width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new CGRect(0, 0, width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };

			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   CancelHandler
												  );

			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 DoneHandler
												);

			if ( !string.IsNullOrEmpty(Cell.TodayText) )
			{
				var labelButton = new UIBarButtonItem(Cell.TodayText, UIBarButtonItemStyle.Plain, ( sender, e ) => { SetToday(); });
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

			DummyField.InputView = _Picker;
			DummyField.InputAccessoryView = toolbar;
		}
		private void DoneHandler( object o, EventArgs a )
		{
			DummyField.ResignFirstResponder();
			Done();
		}
		private void CancelHandler( object o, EventArgs e )
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			DummyField.ResignFirstResponder();
			if ( _PreSelectedDate != null ) _Picker.Date = _PreSelectedDate;
		}

		private void SetToday()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			if ( _Picker.MinimumDate.ToDateTime() <= DateTime.Today &&
				 _Picker.MaximumDate.ToDateTime() >= DateTime.Today ) { _Picker.SetDate(DateTime.Today.ToNSDate(), true); }
		}

		private void Done()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			Cell.Date = _Picker.Date.ToDateTime().Date;
			var text = Cell.Date.ToString(Cell.Format);
			_Value.UpdateText(text);
			_PreSelectedDate = _Picker.Date;
		}

		private void UpdateDate()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			_Picker.SetDate(Cell.Date.ToNSDate(), false);
			var text = Cell.Date.ToString(Cell.Format);
			_Value.UpdateText(text);
			_PreSelectedDate = Cell.Date.ToNSDate();
		}

		private void UpdateMaximumDate()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			_Picker.MaximumDate = Cell.MaximumDate.ToNSDate();
			UpdateDate(); //without this code, selected date isn't sometimes correct when it is shown first.
		}

		private void UpdateMinimumDate()
		{
			if ( _Picker is null ) { throw new NullReferenceException(nameof(_Picker)); }

			_Picker.MinimumDate = Cell.MinimumDate.ToNSDate();
			UpdateDate();
		}

		private void UpdateTodayText()
		{
			SetUpDatePicker();
			UpdateDate();
			UpdateMaximumDate();
			UpdateMinimumDate();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				DummyField.RemoveFromSuperview();
				DummyField.Dispose();

				_Picker?.Dispose();
				_Picker = null;
			}

			base.Dispose(disposing);
		}
	}
}