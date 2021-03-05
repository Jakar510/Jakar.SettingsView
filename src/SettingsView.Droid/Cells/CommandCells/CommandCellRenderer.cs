using System;
using System.ComponentModel;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }


	[Preserve(AllMembers = true)]
	public class CommandCellView : BaseAiDescriptionCell
	{
		protected ICommand? _Command { get; set; }
		protected CommandCell _CommandCell => Cell as CommandCell ?? throw new NullReferenceException(nameof(_CommandCell));

		protected LinearLayout _AccessoryStack { get; }
		protected ImageView _Accessory { get; set; }


		public CommandCellView( Context context, Cell cell ) : base(context, cell)
		{
			_Accessory = new ImageView(AndroidContext);
			_AccessoryStack = AccessoryStack();
			_AccessoryStack.Add(_Accessory, Extensions.Layout.Wrap, Extensions.Layout.Wrap);

			RemoveHint();
			RemoveCellValueStack();

			if ( !( CellParent?.ShowArrowIndicatorForAndroid ?? false ) ||
				 _CommandCell.HideArrowIndicator ) { return; }

			_Accessory.RemoveFromParent();
			_AccessoryStack.RemoveFromParent();
		}
		public CommandCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			Run();
			if ( _CommandCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }
		}

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateCommand();
		}
		protected void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _CommandCell.Command;

			if ( _Command is null ) return;
			_Command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_Command, EventArgs.Empty);
		}
		protected virtual void Run()
		{
			if ( _Command == null ) { return; }

			if ( _Command.CanExecute(_CommandCell.CommandParameter) ) { _Command.Execute(_CommandCell.CommandParameter); }
		}
		protected override void UpdateIsEnabled()
		{
			if ( _Command != null &&
				 !_Command.CanExecute(_CommandCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}


		protected void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !Cell.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_CommandCell.CommandParameter) ?? true);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_Command = null;

				_Title.Dispose();

				_Description.Dispose();

				_Accessory.RemoveFromParent();
				_Accessory.SetImageDrawable(null);
				_Accessory.SetImageBitmap(null);
				_Accessory.Dispose();

				_CellLayout.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}