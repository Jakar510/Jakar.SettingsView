using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Text.Format;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TimePickerCell), typeof(TimePickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)]
	public class TimePickerCellRenderer : CellBaseRenderer<TimePickerCellView> { }

	[Preserve(AllMembers = true)]
	public class TimePickerCellView : LabelCellView
	{
		protected TimePickerCell _TimePickerCell => Cell as TimePickerCell ?? throw new NullReferenceException(nameof(_TimePickerCell));
		protected TimePickerDialog? _Dialog { get; set; }
		protected string _PopupTitle { get; set; } = string.Empty;


		public TimePickerCellView( Context context, Cell cell ) : base(context, cell) { }
		public TimePickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == TimePickerCell.TimeProperty.PropertyName ||
				 e.PropertyName == TimePickerCell.FormatProperty.PropertyName ) { UpdateTime(); }
			else if ( e.PropertyName == TimePickerCell.PickerTitleProperty.PropertyName ) { UpdatePickerTitle(); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { CreateDialog(); }


		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateTime();
			UpdatePickerTitle();
		}
		private void CreateDialog()
		{
			if ( _Dialog != null ) return;
			_Dialog = new TimePickerDialog(AndroidContext, TimeSelected, _TimePickerCell.Time.Hours, _TimePickerCell.Time.Minutes, DateFormat.Is24HourFormat(AndroidContext));

			var title = new TextView(AndroidContext);

			if ( !string.IsNullOrEmpty(_PopupTitle) )
			{
				title.Gravity = Android.Views.GravityFlags.Center;
				title.SetPadding(10, 10, 10, 10);
				title.Text = _PopupTitle;
				_Dialog.SetCustomTitle(title);
			}

			_Dialog.SetCanceledOnTouchOutside(true);

			_Dialog.DismissEvent += ( ss, ee ) =>
									{
										title.Dispose();
										_Dialog.Dispose();
										_Dialog = null;
									};

			_Dialog.Show();
		}
		private void UpdateTime() { _Value.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format); }
		private void UpdatePickerTitle() { _PopupTitle = _TimePickerCell.PickerTitle; }
		private void TimeSelected( object sender, TimePickerDialog.TimeSetEventArgs e )
		{
			_TimePickerCell.Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
			UpdateTime();
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