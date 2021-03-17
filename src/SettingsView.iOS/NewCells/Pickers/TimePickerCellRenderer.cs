using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.NewCells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.NewCells
{
	[Preserve(AllMembers = true)] public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

	[Preserve(AllMembers = true)]
	public class TimePickerCellView : BasePickerCell
	{
		protected TimePickerCell _TimePickerCell => Cell as TimePickerCell ?? throw new NullReferenceException(nameof(_TimePickerCell));
		protected UIDatePicker? _Dialog { get; set; }
		protected UIDatePicker? _Picker { get; set; }
		protected UILabel? _PickerTitle { get; set; }
		protected NSDate? _PreSelectedDate { get; set; }

		public TimePickerCellView( Cell cell ) : base(cell) { }


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(TimePickerCell.TimeProperty, TimePickerCell.FormatProperty) ) { UpdateTime(); }
			else if ( e.IsEqual(PopupConfig.TitleProperty.PropertyName) ) { UpdatePopupTitle(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		protected override void ShowPopup()
		{
			UpdatePopupTitle();
			base.ShowPopup();
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

			_Picker = new UIDatePicker
					  {
						  Mode = UIDatePickerMode.Time,
						  TimeZone = new NSTimeZone("UTC")
					  };
			if ( UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ) { _Picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; }

			_PickerTitle = new UILabel
						   {
							   TextAlignment = UITextAlignment.Center
						   };

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

		protected void Canceled()
		{
			if ( _PreSelectedDate is null ||
				 _Picker is null ) return;
			_Picker.Date = _PreSelectedDate;
		}

		protected void Done()
		{
			if ( _Picker is null ) return;
			_TimePickerCell.Time = _Picker.Date.ToDateTime() - new DateTime(1, 1, 1);
			_Value.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
			_PreSelectedDate = _Picker.Date;
		}

		protected void UpdateTime()
		{
			if ( _Picker is null ) return;
			_Picker.Date = new DateTime(1, 1, 1).Add(_TimePickerCell.Time).ToNSDate();
			_Value.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
			_PreSelectedDate = _Picker.Date;
		}

		protected void UpdatePopupTitle()
		{
			if ( _PickerTitle is null ) return;
			_PickerTitle.Text = _TimePickerCell.Prompt.Title;
			_PickerTitle.SizeToFit();
			_PickerTitle.Frame = new CGRect(0, 0, 160, 44);
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateTime();
			UpdatePopupTitle();
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