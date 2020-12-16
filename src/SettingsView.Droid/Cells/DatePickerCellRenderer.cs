﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(DatePickerCell), typeof(DatePickerCellRenderer))]

#nullable enable
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
		protected DatePickerCell _DatePickerCell => Cell as DatePickerCell ?? throw new NullReferenceException(nameof(_DatePickerCell));
		protected DatePickerDialog? _Dialog { get; set; }

		public DatePickerCellView( Context context, Cell cell ) : base(context, cell) { }
		public DatePickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateDate();
		}

		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == DatePickerCell.DateProperty.PropertyName ||
				 e.PropertyName == DatePickerCell.FormatProperty.PropertyName ) { UpdateDate(); }
			else if ( e.PropertyName == DatePickerCell.MaximumDateProperty.PropertyName ) { UpdateMaximumDate(); }
			else if ( e.PropertyName == DatePickerCell.MinimumDateProperty.PropertyName ) { UpdateMinimumDate(); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { ShowDialog(); }

		protected void ShowDialog()
		{
			_Dialog = CreateDatePickerDialog(_DatePickerCell.Date.Year, _DatePickerCell.Date.Month - 1, _DatePickerCell.Date.Day);

			UpdateMinimumDate();
			UpdateMaximumDate();

			if ( _DatePickerCell.MinimumDate > _DatePickerCell.MaximumDate ) { throw new ArgumentOutOfRangeException(nameof(DatePickerCell.MaximumDate), "MaximumDate must be greater than or equal to MinimumDate."); }

			if ( _Dialog is null ) return;
			_Dialog.CancelEvent += OnCancelButtonClicked;

			_Dialog.Show();
		}
		protected DatePickerDialog CreateDatePickerDialog( int year, int month, int day ) => new DatePickerDialog(AndroidContext, CallBack, year, month, day);
		protected void CallBack( object o, DatePickerDialog.DateSetEventArgs e )
		{
			_DatePickerCell.Date = e.Date;
			ClearFocus();
			if ( _Dialog != null ) _Dialog.CancelEvent -= OnCancelButtonClicked;

			_Dialog = null;
		}
		protected void OnCancelButtonClicked( object sender, EventArgs e ) { ClearFocus(); }

		protected void UpdateDate()
		{
			string format = _DatePickerCell.Format;
			_Value.Text = _DatePickerCell.Date.ToString(format);
		}
		protected void UpdateMaximumDate()
		{
			if ( _Dialog != null )
			{
				//when not to specify 23:59:59,last day can't be selected. 
				_Dialog.DatePicker.MaxDate = (long) _DatePickerCell.MaximumDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
			}
		}
		protected void UpdateMinimumDate()
		{
			if ( _Dialog != null ) { _Dialog.DatePicker.MinDate = (long) _DatePickerCell.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds; }
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Dialog != null )
				{
					_Dialog.CancelEvent -= OnCancelButtonClicked;
					_Dialog?.Dispose();
					_Dialog = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}