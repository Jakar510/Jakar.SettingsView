using System;
using System.Runtime.Remoting.Contexts;
using System.Windows.Input;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)] public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }


	[Foundation.Preserve(AllMembers = true)]
	public class NumberPickerCellView : BaseAiValueCell
	{
		protected NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell ?? throw new NullReferenceException(nameof(_NumberPickerCell));
		protected NumberPicker? _Picker { get; set; }
		protected AlertDialog? _Dialog { get; set; }
		protected string _Popup_Title { get; set; } = string.Empty;
		protected ICommand? _Command { get; set; }
		protected int _Max { get; set; } = 1;
		protected int _Min { get; set; } = 100;


		public NumberPickerCellView( Cell cell ) : base(cell) { }


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == NumberPickerCell.MinProperty.PropertyName ) { UpdateMin(); }
			else if ( e.PropertyName == NumberPickerCell.MaxProperty.PropertyName ) { UpdateMax(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdatePopupTitle(); }
			else if ( e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { CreateDialog(); }

		protected void CreateDialog()
		{
			_Picker = new NumberPicker(AndroidContext)
					  {
						  MinValue = _Min,
						  MaxValue = _Max,
						  Value = _NumberPickerCell.Number
					  };

			if ( _Dialog is not null ) return;
			using ( var builder = new AlertDialog.Builder(AndroidContext) )
			{
				builder.SetTitle(_Popup_Title);

				var parent = new FrameLayout(AndroidContext);
				parent.AddView(_Picker, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Center));
				builder.SetView(parent);

				builder.SetNegativeButton(Android.Resource.String.Cancel, Cancel);
				builder.SetPositiveButton(Android.Resource.String.Ok, Accept);

				_Dialog = builder.Create();
			}

			if ( _Dialog is null ) return;
			_Dialog.SetCanceledOnTouchOutside(true);
			_Dialog.DismissEvent += OnDialogOnDismissEvent;

			_Dialog.Show();
		}
		protected void Cancel( object o, DialogClickEventArgs args ) { ClearFocus(); }
		protected void Accept( object o, DialogClickEventArgs args )
		{
			if ( _Picker is not null )
			{
				_NumberPickerCell.Number = _Picker.Value;
				_Command?.Execute(_Picker.Value);
			}

			ClearFocus();
		}
		protected void OnDialogOnDismissEvent( object sender, EventArgs e )
		{
			if ( _Dialog is not null )
			{
				_Dialog.DismissEvent -= OnDialogOnDismissEvent;
				_Dialog.Dispose();
			}

			_Dialog = null;
			_Picker.RemoveFromParent();
		}
		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateMin();
			UpdateMax();
			UpdatePopupTitle();
			UpdateNumber();
			UpdateCommand();
		}

		protected void UpdateMin() { _Min = _NumberPickerCell.Min; }
		protected void UpdateMax() { _Max = _NumberPickerCell.Max; }
		protected void UpdateNumber() { _Value.Text = _NumberPickerCell.Number.ToString(); }

		protected void UpdatePopupTitle() { _Popup_Title = _NumberPickerCell.Prompt.Title; }
		protected void UpdateCommand() { _Command = _NumberPickerCell.SelectedCommand; }


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