using System;
using System.Linq;
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

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Text picker cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	/// <summary>
	/// Text picker cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class TextPickerCellView : LabelCellView
	{
		private TextPickerCell _TextPickerCell => Cell as TextPickerCell;
		private APicker _picker;
		private AlertDialog _dialog;
		private Context _context;
		private string _title;
		private ICommand _command;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.TextPickerCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public TextPickerCellView( Context context, Cell cell ) : base(context, cell) => _context = context;

		public TextPickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName ) { UpdateSelectedItem(); }
			else if ( e.PropertyName == TextPickerCell.PickerTitleProperty.PropertyName ) { UpdatePickerTitle(); }
			else if ( e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
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
			UpdatePickerTitle();
			UpdateSelectedItem();
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

		private void UpdateSelectedItem() { VValueLabel.Text = _TextPickerCell.SelectedItem?.ToString(); }

		private void UpdatePickerTitle() { _title = _TextPickerCell.PickerTitle; }

		private void UpdateCommand() { _command = _TextPickerCell.SelectedCommand; }

		private void CreateDialog()
		{
			if ( _TextPickerCell.Items == null ||
				 _TextPickerCell.Items.Count == 0 ) { return; }

			string[] displayValues = _TextPickerCell.Items.Cast<object>().Select(x => x.ToString()).ToArray();

			_picker = new APicker(_context);
			_picker.DescendantFocusability = DescendantFocusability.BlockDescendants;
			_picker.MinValue = 0;
			_picker.MaxValue = _TextPickerCell.Items.Count - 1;
			_picker.SetDisplayedValues(displayValues);
			_picker.Value = Math.Max(_TextPickerCell.Items.IndexOf(_TextPickerCell.SelectedItem), 0);
			_picker.WrapSelectorWheel = _TextPickerCell.IsCircularPicker;

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
																					  _TextPickerCell.SelectedItem = _TextPickerCell.Items[_picker.Value];
																					  _command?.Execute(_TextPickerCell.Items[_picker.Value]);
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