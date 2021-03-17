using System;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }

	[Preserve(AllMembers = true)]
	public class CommandCellView : LabelCellView
	{
		internal Action Execute { get; set; }
		private CommandCell _CommandCell => Cell as CommandCell;
		private ICommand _command;

		public CommandCellView( Cell formsCell ) : base(formsCell)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Default;

			if ( !_CommandCell.HideArrowIndicator )
			{
				Accessory = UITableViewCellAccessory.DisclosureIndicator;
				EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
				SetRightMarginZero();
			}
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			Execute?.Invoke();
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

				Execute = null;
				_command = null;
			}

			base.Dispose(disposing);
		}

		private void UpdateCommand()
		{
			if ( _command is not null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_command = _CommandCell.Command;

			if ( _command is not null )
			{
				_command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_command, EventArgs.Empty);
			}

			Execute = () =>
					  {
						  if ( _command is null ) { return; }

						  if ( _command.CanExecute(_CommandCell.CommandParameter) ) { _command.Execute(_CommandCell.CommandParameter); }
					  };
		}

		protected override void UpdateIsEnabled()
		{
			if ( _command is not null &&
				 !_command.CanExecute(_CommandCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command.CanExecute(_CommandCell.CommandParameter));
		}
	}
}