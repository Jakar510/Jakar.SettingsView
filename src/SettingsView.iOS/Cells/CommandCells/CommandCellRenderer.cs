using System;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }

	[Preserve(AllMembers = true)]
	public class CommandCellView : BaseDescriptiveTitleCell
	{
		private CommandCell _CommandCell => Cell as CommandCell ?? throw new NullReferenceException(nameof(_CommandCell));
		private ICommand? _command;

		public CommandCellView( Cell formsCell ) : base(formsCell)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

			if ( _CommandCell.HideArrowIndicator ) { return; }

			Accessory = UITableViewCellAccessory.None;
			EditingAccessory = UITableViewCellAccessory.None;
			SetRightMarginZero(_MainStack);
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			Run();
			if ( !_CommandCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdateCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _command is not null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_command = null;
			}

			base.Dispose(disposing);
		}

		private void UpdateCommand()
		{
			if ( _command is not null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_command = _CommandCell.Command;

			if ( _command is null ) return;
			_command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_command, EventArgs.Empty);
		}

		private void Run()
		{
			if ( _command is null ) { return; }

			if ( _command.CanExecute(_CommandCell.CommandParameter) ) { _command.Execute(_CommandCell.CommandParameter); }
		}

		protected override void UpdateIsEnabled()
		{
			if ( _command is not null &&
				 !_command.CanExecute(_CommandCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( _command is null ||
				 _CellBase != null && !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command.CanExecute(_CommandCell.CommandParameter));
		}
	}
}