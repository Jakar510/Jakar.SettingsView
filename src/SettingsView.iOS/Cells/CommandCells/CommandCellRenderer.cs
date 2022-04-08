[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]


namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }

	[Preserve(AllMembers = true)]
	public class CommandCellView : BaseDescriptiveTitleCell<CommandCell>
	{
		private ICommand? _command;

		public CommandCellView( CommandCell formsCell ) : base(formsCell)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

			if ( Cell.HideArrowIndicator ) { return; }

			Accessory = UITableViewCellAccessory.None;
			EditingAccessory = UITableViewCellAccessory.None;
			SetRightMarginZero(_MainStack);
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.commandProperty.PropertyName ||
				 e.PropertyName == CommandCell.commandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			Run();
			if ( !Cell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
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

			_command = Cell.Command;

			if ( _command is null ) return;
			_command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_command, EventArgs.Empty);
		}

		private void Run()
		{
			if ( _command is null ) { return; }

			if ( _command.CanExecute(Cell.CommandParameter) ) { _command.Execute(Cell.CommandParameter); }
		}

		protected override void UpdateIsEnabled()
		{
			if ( _command is not null &&
				 !_command.CanExecute(Cell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( _command is null ||
				 _CellBase is { IsEnabled: false } ) { return; }

			SetEnabledAppearance(_command.CanExecute(Cell.CommandParameter));
		}
	}
}