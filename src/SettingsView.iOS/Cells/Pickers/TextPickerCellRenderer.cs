using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Input;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)] public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class TextPickerCellView : BaseAiValueCell
	{
		private TextPickerCell _TextPickerCell => Cell as TextPickerCell ?? throw new NullReferenceException(nameof(_TextPickerCell));
		private NumberPicker? _Picker { get; set; }
		private AlertDialog? _Dialog { get; set; }
		private string _PopupTitle { get; set; } = string.Empty;
		private ICommand? _Command { get; set; }


		public TextPickerCellView( Cell cell ) : base(cell) { }

		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == TextPickerCell.SelectedItemProperty.PropertyName ) { UpdateSelectedItem(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdatePopupTitle(); }
			else if ( e.PropertyName == TextPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { CreateDialog(); }


		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdatePopupTitle();
			UpdateSelectedItem();
			UpdateCommand();
		}

		private void UpdateSelectedItem() { _Value.Text = _TextPickerCell.SelectedItem?.ToString(); }
		private void UpdatePopupTitle() { _PopupTitle = _TextPickerCell.Prompt.Title; }
		private void UpdateCommand() { _Command = _TextPickerCell.SelectedCommand; }

		private void CreateDialog()
		{
			if ( _TextPickerCell.Items == null ||
				 _TextPickerCell.Items.Count == 0 ) { return; }

			_Picker?.Dispose();
			_Picker = new NumberPicker(AndroidContext)
					  {
						  DescendantFocusability = DescendantFocusability.BlockDescendants,
						  MinValue = 0,
						  MaxValue = _TextPickerCell.Items.Count - 1,
						  WrapSelectorWheel = _TextPickerCell.IsCircularPicker,
						  Value = Math.Max(_TextPickerCell.Items.IndexOf(_TextPickerCell.SelectedItem), 0),
					  };

			_Picker.SetBackgroundColor(_TextPickerCell.Prompt.BackgroundColor.ToAndroid());
			_Picker.SetTextColor(_TextPickerCell.Prompt.ItemColor.ToAndroid());
			_Picker.SetDisplayedValues(_TextPickerCell.Items.ToArray());

			if ( _Dialog != null ) return;
			using ( var builder = new AlertDialog.Builder(AndroidContext) )
			{
				builder.SetTitle(_PopupTitle);

				var parent = new FrameLayout(AndroidContext);
				parent.SetBackgroundColor(_TextPickerCell.Prompt.BackgroundColor.ToAndroid());

				parent.AddView(_Picker, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Center));

				builder.SetView(parent);
				builder.SetNegativeButton(_TextPickerCell.Prompt.Cancel, CancelAndClosePopup);
				builder.SetPositiveButton(_TextPickerCell.Prompt.Accept, AcceptAndClosePopup);

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