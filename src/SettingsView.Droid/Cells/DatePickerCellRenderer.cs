﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
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
	public class DatePickerCellView : LabelCellView
	{
		private DatePickerCell _datePickerCell => Cell as DatePickerCell;
		private DatePickerDialog _dialog;
		private readonly Context _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.DatePickerCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public DatePickerCellView( Context context, Cell cell ) : base(context, cell) => _context = context;

		public DatePickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateDate();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == DatePickerCell.DateProperty.PropertyName ||
				 e.PropertyName == DatePickerCell.FormatProperty.PropertyName ) { UpdateDate(); }
			else if ( e.PropertyName == DatePickerCell.MaximumDateProperty.PropertyName ) { UpdateMaximumDate(); }
			else if ( e.PropertyName == DatePickerCell.MinimumDateProperty.PropertyName ) { UpdateMinimumDate(); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { ShowDialog(); }

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _dialog != null )
				{
					_dialog.CancelEvent -= OnCancelButtonClicked;
					_dialog?.Dispose();
					_dialog = null;
				}
			}

			base.Dispose(disposing);
		}

		private void ShowDialog()
		{
			CreateDatePickerDialog(_datePickerCell.Date.Year, _datePickerCell.Date.Month - 1, _datePickerCell.Date.Day);

			UpdateMinimumDate();
			UpdateMaximumDate();

			if ( _datePickerCell.MinimumDate > _datePickerCell.MaximumDate ) { throw new ArgumentOutOfRangeException(nameof(DatePickerCell.MaximumDate), "MaximumDate must be greater than or equal to MinimumDate."); }

			_dialog.CancelEvent += OnCancelButtonClicked;

			_dialog.Show();
		}

		private void CreateDatePickerDialog( int year, int month, int day )
		{
			_dialog = new DatePickerDialog(_context, ( o, e ) =>
													 {
														 _datePickerCell.Date = e.Date;
														 ClearFocus();
														 _dialog.CancelEvent -= OnCancelButtonClicked;

														 _dialog = null;
													 }, year, month, day);
		}

		private void OnCancelButtonClicked( object sender, EventArgs e ) { ClearFocus(); }

		private void UpdateDate()
		{
			string format = _datePickerCell.Format;
			VValueLabel.Text = _datePickerCell.Date.ToString(format);
		}

		private void UpdateMaximumDate()
		{
			if ( _dialog != null )
			{
				//when not to specify 23:59:59,last day can't be selected. 
				_dialog.DatePicker.MaxDate = (long) _datePickerCell.MaximumDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
			}
		}

		private void UpdateMinimumDate()
		{
			if ( _dialog != null ) { _dialog.DatePicker.MinDate = (long) _datePickerCell.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds; }
		}
	}
}