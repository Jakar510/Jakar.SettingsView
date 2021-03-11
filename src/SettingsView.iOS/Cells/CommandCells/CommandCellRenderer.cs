using System;
using System.ComponentModel;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }


	[Foundation.Preserve(AllMembers = true)]
	public class CommandCellView : BaseAiDescriptionCell
	{
		protected ICommand? _Command { get; set; }
		protected CommandCell _CommandCell => Cell as CommandCell ?? throw new NullReferenceException(nameof(_CommandCell));

		protected UIImageView _Accessory { get; set; }


		public CommandCellView( Cell cell ) : base(cell)
		{
			_Accessory = new UIImageView();

			if ( !( CellParent?.ShowArrowIndicatorForAndroid ?? false ) ||
				 _CommandCell.HideArrowIndicator ) { return; }

			_Accessory.RemoveFromSuperview();
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		protected internal override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			Run();
			return _CommandCell.KeepSelectedUntilBack;
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

				_Accessory.RemoveFromSuperview();
				_Accessory.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}