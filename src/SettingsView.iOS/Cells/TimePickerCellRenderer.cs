using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.iOS.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Time picker cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

	/// <summary>
	/// Time picker cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class TimePickerCellView : LabelCellView
	{
		private TimePickerCell _TimePickerCell => Cell as TimePickerCell;
		private UIDatePicker _picker;

		/// <summary>
		/// Gets or sets the dummy field.
		/// </summary>
		/// <value>The dummy field.</value>
		public UITextField DummyField { get; set; }

		private UILabel _titleLabel;
		private NSDate _preSelectedDate;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.TimePickerCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public TimePickerCellView( Cell formsCell ) : base(formsCell)
		{
			DummyField = new NoCaretField();
			DummyField.BorderStyle = UITextBorderStyle.None;
			DummyField.BackgroundColor = UIColor.Clear;
			ContentView.AddSubview(DummyField);
			ContentView.SendSubviewToBack(DummyField);

			SelectionStyle = UITableViewCellSelectionStyle.Default;

			SetUpTimePicker();
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdatePickerTitle();
			UpdateTime();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == TimePickerCell.TimeProperty.PropertyName ||
				 e.PropertyName == TimePickerCell.FormatProperty.PropertyName ) { UpdateTime(); }
			else if ( e.PropertyName == TimePickerCell.PickerTitleProperty.PropertyName ) { UpdatePickerTitle(); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			tableView.DeselectRow(indexPath, true);
			DummyField.BecomeFirstResponder();
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				DummyField.RemoveFromSuperview();
				DummyField?.Dispose();
				DummyField = null;
				_picker.Dispose();
				_picker = null;
				_titleLabel?.Dispose();
				_titleLabel = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Layouts the subviews.
		/// </summary>
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		private void SetUpTimePicker()
		{
			_picker = new UIDatePicker
					  {
						  Mode = UIDatePickerMode.Time,
						  TimeZone = new NSTimeZone("UTC")
					  };
			if ( UIDevice.CurrentDevice.CheckSystemVersion(13, 4) ) { _picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels; }

			_titleLabel = new UILabel();
			_titleLabel.TextAlignment = UITextAlignment.Center;

			nfloat width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, ( o, e ) =>
																				 {
																					 DummyField.ResignFirstResponder();
																					 Canceled();
																				 });

			var labelButton = new UIBarButtonItem(_titleLabel);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, ( o, a ) =>
																			 {
																				 DummyField.ResignFirstResponder();
																				 Done();
																			 });

			toolbar.SetItems(new[]
							 {
								 cancelButton,
								 spacer,
								 labelButton,
								 spacer,
								 doneButton
							 }, false);

			DummyField.InputView = _picker;
			DummyField.InputAccessoryView = toolbar;
		}

		private void Canceled() { _picker.Date = _preSelectedDate; }

		private void Done()
		{
			_TimePickerCell.Time = _picker.Date.ToDateTime() - new DateTime(1, 1, 1);
			ValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
			_preSelectedDate = _picker.Date;
		}

		private void UpdateTime()
		{
			_picker.Date = new DateTime(1, 1, 1).Add(_TimePickerCell.Time).ToNSDate();
			ValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format);
			_preSelectedDate = _picker.Date;
		}

		private void UpdatePickerTitle()
		{
			_titleLabel.Text = _TimePickerCell.PickerTitle;
			_titleLabel.SizeToFit();
			_titleLabel.Frame = new CGRect(0, 0, 160, 44);
		}
	}
}