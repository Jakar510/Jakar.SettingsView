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
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using APicker = Android.Widget.NumberPicker;

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	[Preserve(AllMembers = true)]
	public class TextPickerCellView : LabelCellView
	{
		private TextPickerCell _TextPickerCell => Cell as TextPickerCell ?? throw new NullReferenceException(nameof(_TextPickerCell));
		private APicker? _Picker { get; set; }
		private AlertDialog? _Dialog { get; set; }
		private string _PickerTitle { get; set; } = string.Empty;
		private ICommand? _Command { get; set; }
		

		public TextPickerCellView( Context context, Cell cell ) : base(context, cell) {  }
		public TextPickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName ) { UpdateSelectedItem(); }
			else if ( e.PropertyName == TextPickerCell.PickerTitleProperty.PropertyName ) { UpdatePickerTitle(); }
			else if ( e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
		}

		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { CreateDialog(); }


		protected override void UpdateCell()
		{
			base.UpdateCell();
			UpdatePickerTitle();
			UpdateSelectedItem();
			UpdateCommand();
		}

		private void UpdateSelectedItem() { _Value.Label.Text = _TextPickerCell.SelectedItem?.ToString(); }

		private void UpdatePickerTitle() { _PickerTitle = _TextPickerCell.PickerTitle; }

		private void UpdateCommand() { _Command = _TextPickerCell.SelectedCommand; }

		private void CreateDialog()
		{
			if ( _TextPickerCell.Items == null ||
				 _TextPickerCell.Items.Count == 0 ) { return; }

			string[] displayValues = _TextPickerCell.Items.Cast<object>().Select(x => x.ToString()).ToArray();

			_Picker = new APicker(AndroidContext)
					  {
						  DescendantFocusability = DescendantFocusability.BlockDescendants,
						  MinValue = 0,
						  MaxValue = _TextPickerCell.Items.Count - 1,
						  WrapSelectorWheel = _TextPickerCell.IsCircularPicker,
						  Value = Math.Max(_TextPickerCell.Items.IndexOf(_TextPickerCell.SelectedItem), 0)
					  };
			_Picker.SetDisplayedValues(displayValues);

			if ( _Dialog != null ) return;
			using ( var builder = new AlertDialog.Builder(AndroidContext) )
			{
				builder.SetTitle(_PickerTitle);

				var parent = new FrameLayout(AndroidContext);
				parent.AddView(_Picker, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Center));

				builder.SetView(parent);
				builder.SetNegativeButton(_TextPickerCell.PopupCancelText, CancelAndClosePopup);
				builder.SetPositiveButton(_TextPickerCell.PopupAcceptText, AcceptAndClosePopup);

				_Dialog = builder.Create();
			}

			if ( _Dialog is null ) return;
			_Dialog.SetCanceledOnTouchOutside(true);
			_Dialog.DismissEvent += OnDialogOnDismissEvent;

			_Dialog.Show();
		}
		private void CancelAndClosePopup( object o, DialogClickEventArgs args ) { ClearFocus(); }
		private void AcceptAndClosePopup( object o, DialogClickEventArgs args )
		{
			if ( _Picker != null )
			{
				_TextPickerCell.SelectedItem = _TextPickerCell.Items[_Picker.Value];
				_Command?.Execute(_TextPickerCell.Items[_Picker.Value]);
			}

			ClearFocus();
		}
		private void OnDialogOnDismissEvent( object ss, EventArgs ee )
		{
			if ( _Dialog != null )
			{
				_Dialog.DismissEvent -= OnDialogOnDismissEvent;
				_Dialog.Dispose();
			}

			_Dialog = null;
			_Picker.RemoveFromParent();
			_Picker?.Dispose();
			_Picker = null;
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