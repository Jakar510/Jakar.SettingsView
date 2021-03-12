using System;
using System.Runtime.Remoting.Contexts;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class DatePickerCellRenderer : CellBaseRenderer<DatePickerCellView> { }

	[Preserve(AllMembers = true)]
	public class DatePickerCellView : BasePickerCell
	{
		protected DatePickerCell _DatePickerCell => Cell as DatePickerCell ?? throw new NullReferenceException(nameof(_DatePickerCell));
		protected UIDatePicker? _Dialog { get; set; }
		protected NSDate? _PreSelectedDate { get; set; }

		public DatePickerCellView( Cell cell ) : base(cell) { }


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(DatePickerCell.DateProperty, DatePickerCell.FormatProperty) ) { UpdateDate(); }
			else if ( e.IsEqual(DatePickerCell.TodayTextProperty) ) { UpdateTodayText(); }
			else if ( e.IsEqual(DatePickerCell.MaximumDateProperty) ) { UpdateMaximumDate(); }
			else if ( e.IsEqual(DatePickerCell.MinimumDateProperty) ) { UpdateMinimumDate(); }
			else { base.CellPropertyChanged(sender, e); }
		}
		


		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			_DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}
		protected override void SetUp()
		{
			_DummyField.InputView = null;
			_DummyField.InputAccessoryView = null;
			_Dialog?.Dispose();
			_Dialog = null;

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
													   _DummyField.ResignFirstResponder();
													   if ( _PreSelectedDate != null ) _Dialog.Date = _PreSelectedDate;
												   }
												  );

			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 _DummyField.ResignFirstResponder();
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

			_DummyField.InputView = _Dialog;
			_DummyField.InputAccessoryView = toolbar;
		}
		protected void SetToday()
		{
			if ( _Dialog is null ) return;
			if ( _Dialog.MinimumDate.ToDateTime() <= DateTime.Today &&
				 _Dialog.MaximumDate.ToDateTime() >= DateTime.Today ) { _Dialog.SetDate(DateTime.Today.ToNSDate(), true); }
		}
		protected void Done()
		{
			if ( _Dialog is null ) return;
			_DatePickerCell.Date = _Dialog.Date.ToDateTime().Date;
			_Value.Text = _DatePickerCell.Date.ToString(_DatePickerCell.Format);
			_PreSelectedDate = _Dialog.Date;
		}

		protected void UpdateDate()
		{
			if ( _Dialog is null ) return;
			_Dialog.SetDate(_DatePickerCell.Date.ToNSDate(), false);
			_Value.Text = _DatePickerCell.Date.ToString(_DatePickerCell.Format);
			_PreSelectedDate = _DatePickerCell.Date.ToNSDate();
		}

		protected void UpdateMaximumDate()
		{
			if ( _Dialog is null ) return;
			_Dialog.MaximumDate = _DatePickerCell.MaximumDate.ToNSDate();
			UpdateDate(); // without this code, selected date isn't sometimes correct when it is shown first.
		}

		protected void UpdateMinimumDate()
		{
			if ( _Dialog is null ) return;
			_Dialog.MinimumDate = _DatePickerCell.MinimumDate.ToNSDate();
			UpdateDate();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateDate();
		}

		protected void UpdateTodayText()
		{
			SetUp();
			UpdateDate();
			UpdateMaximumDate();
			UpdateMinimumDate();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Dialog?.Dispose();
				_Dialog = null;
			}

			base.Dispose(disposing);
		}
	}
}