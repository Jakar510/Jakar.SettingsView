using System;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Number picker cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }

	/// <summary>
	/// Number picker cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class NumberPickerCellView : LabelCellView
	{
		private NumberPickerCell _NumberPikcerCell => Cell as NumberPickerCell;
		private APicker _picker;
		private AlertDialog _dialog;
		private Context _context;
		private string _title;
		private ICommand _command;
		private int _max;
		private int _min;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.NumberPickerCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public NumberPickerCellView( Context context, Cell cell ) : base(context, cell) => _context = context;

		public NumberPickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == NumberPickerCell.MinProperty.PropertyName ) { UpdateMin(); }
			else if ( e.PropertyName == NumberPickerCell.MaxProperty.PropertyName ) { UpdateMax(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == NumberPickerCell.PickerTitleProperty.PropertyName ) { UpdatePickerTitle(); }
			else if ( e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { CreateDialog(); }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateMin();
			UpdateMax();
			UpdatePickerTitle();
			UpdateNumber();
			UpdateCommand();
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
				_picker?.Dispose();
				_picker = null;
				_dialog?.Dispose();
				_dialog = null;
				_context = null;
				_command = null;
			}

			base.Dispose(disposing);
		}

		private void UpdateMin() { _min = _NumberPikcerCell.Min; }

		private void UpdateMax() { _max = _NumberPikcerCell.Max; }

		private void UpdateNumber() { VValueLabel.Text = _NumberPikcerCell.Number.ToString(); }

		private void UpdatePickerTitle() { _title = _NumberPikcerCell.PickerTitle; }

		private void UpdateCommand() { _command = _NumberPikcerCell.SelectedCommand; }

		private void CreateDialog()
		{
			_picker = new APicker(_context);
			_picker.MinValue = _min;
			_picker.MaxValue = _max;
			_picker.Value = _NumberPikcerCell.Number;

			if ( _dialog == null )
			{
				using ( var builder = new AlertDialog.Builder(_context) )
				{
					builder.SetTitle(_title);

					var parent = new FrameLayout(_context);
					parent.AddView(_picker, new Android.Widget.FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Center));
					builder.SetView(parent);


					builder.SetNegativeButton(Android.Resource.String.Cancel, ( o, args ) => { ClearFocus(); });
					builder.SetPositiveButton(Android.Resource.String.Ok, ( o, args ) =>
																				  {
																					  _NumberPikcerCell.Number = _picker.Value;
																					  _command?.Execute(_picker.Value);
																					  ClearFocus();
																				  });

					_dialog = builder.Create();
				}

				_dialog.SetCanceledOnTouchOutside(true);
				_dialog.DismissEvent += ( ss, ee ) =>
										{
											_dialog.Dispose();
											_dialog = null;
											_picker.RemoveFromParent();
											_picker.Dispose();
											_picker = null;
										};

				_dialog.Show();
			}
		}
	}
}