using System;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }


	[Preserve(AllMembers = true)]
	public class NumberPickerCellView : LabelCellView
	{
		protected NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell ?? throw new NullReferenceException(nameof(_NumberPickerCell));
		protected APicker? _Picker { get; set; }
		protected AlertDialog? _Dialog { get; set; }
		protected string _Popup_Title { get; set; } = string.Empty;
		protected ICommand? _Command { get; set; }
		protected int _Max { get; set; } = 1;
		protected int _Min { get; set; } = 100;


		public NumberPickerCellView( Context context, Cell cell ) : base(context, cell) { }
		public NumberPickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == NumberPickerCell.MinProperty.PropertyName ) { UpdateMin(); }
			else if ( e.PropertyName == NumberPickerCell.MaxProperty.PropertyName ) { UpdateMax(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == NumberPickerCell.PickerTitleProperty.PropertyName ) { UpdatePickerTitle(); }
			else if ( e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
		}


		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { CreateDialog(); }


		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateMin();
			UpdateMax();
			UpdatePickerTitle();
			UpdateNumber();
			UpdateCommand();
		}


		private void UpdateMin() { _Min = _NumberPickerCell.Min; }
		private void UpdateMax() { _Max = _NumberPickerCell.Max; }
		private void UpdateNumber() { _Value.Text = _NumberPickerCell.Number.ToString(); }

		private void UpdatePickerTitle() { _Popup_Title = _NumberPickerCell.PickerTitle; }
		private void UpdateCommand() { _Command = _NumberPickerCell.SelectedCommand; }

		private void CreateDialog()
		{
			_Picker = new APicker(AndroidContext)
					  {
						  MinValue = _Min,
						  MaxValue = _Max,
						  Value = _NumberPickerCell.Number
					  };

			if ( _Dialog != null ) return;
			using ( var builder = new AlertDialog.Builder(AndroidContext) )
			{
				builder.SetTitle(_Popup_Title);

				var parent = new FrameLayout(AndroidContext);
				parent.AddView(_Picker, new Android.Widget.FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Center));
				builder.SetView(parent);


				builder.SetNegativeButton(Android.Resource.String.Cancel, ( o, args ) => { ClearFocus(); });
				builder.SetPositiveButton(Android.Resource.String.Ok, ( o, args ) =>
																	  {
																		  _NumberPickerCell.Number = _Picker.Value;
																		  _Command?.Execute(_Picker.Value);
																		  ClearFocus();
																	  });

				_Dialog = builder.Create();
			}

			if ( _Dialog == null ) return;
			_Dialog.SetCanceledOnTouchOutside(true);
			_Dialog.DismissEvent += ( ss, ee ) =>
									{
										_Dialog.Dispose();
										_Dialog = null;
										_Picker.RemoveFromParent();
										_Picker.Dispose();
										_Picker = null;
									};

			_Dialog.Show();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Picker?.Dispose();
				_Picker = null;
				_Dialog?.Dispose();
				_Dialog = null;
				_Command = null;
			}

			base.Dispose(disposing);
		}
	}
}