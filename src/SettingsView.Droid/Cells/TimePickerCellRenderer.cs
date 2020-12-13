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

namespace Jakar.SettingsView.Droid.Cells
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
		private TimePickerDialog _dialog;
		private readonly Context _context;
		private string _title;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.TimePickerCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public TimePickerCellView( Context context, Cell cell ) : base(context, cell) => _context = context;

		public TimePickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateTime();
			UpdatePickerTitle();
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
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { CreateDialog(); }


		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_dialog?.Dispose();
				_dialog = null;
			}

			base.Dispose(disposing);
		}

		private void CreateDialog()
		{
			if ( _dialog == null )
			{
				bool is24HourFormat = DateFormat.Is24HourFormat(_context);
				_dialog = new TimePickerDialog(_context, TimeSelected, _TimePickerCell.Time.Hours, _TimePickerCell.Time.Minutes, is24HourFormat);

				var title = new TextView(_context);

				if ( !string.IsNullOrEmpty(_title) )
				{
					title.Gravity = Android.Views.GravityFlags.Center;
					title.SetPadding(10, 10, 10, 10);
					title.Text = _title;
					_dialog.SetCustomTitle(title);
				}

				_dialog.SetCanceledOnTouchOutside(true);

				_dialog.DismissEvent += ( ss, ee ) =>
										{
											title.Dispose();
											_dialog.Dispose();
											_dialog = null;
										};

				_dialog.Show();
			}
		}

		private void UpdateTime() { VValueLabel.Text = DateTime.Today.Add(_TimePickerCell.Time).ToString(_TimePickerCell.Format); }

		private void UpdatePickerTitle() { _title = _TimePickerCell.PickerTitle; }

		private void TimeSelected( object sender, TimePickerDialog.TimeSetEventArgs e )
		{
			_TimePickerCell.Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
			UpdateTime();
		}
	}
}