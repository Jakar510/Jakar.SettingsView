﻿using System;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	/// <summary>
	/// Command cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }

	/// <summary>
	/// Command cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class CommandCellView : LabelCellView
	{
		internal Action Execute { get; set; }
		private CommandCell _CommandCell => Cell as CommandCell;
		private ICommand _command;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.OLD_Cells.CommandCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
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

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			Execute?.Invoke();
			if ( !_CommandCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
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
				if ( _command != null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

				Execute = null;
				_command = null;
			}

			base.Dispose(disposing);
		}

		private void UpdateCommand()
		{
			if ( _command != null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_command = _CommandCell.Command;

			if ( _command != null )
			{
				_command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_command, EventArgs.Empty);
			}

			Execute = () =>
					  {
						  if ( _command == null ) { return; }

						  if ( _command.CanExecute(_CommandCell.CommandParameter) ) { _command.Execute(_CommandCell.CommandParameter); }
					  };
		}

		/// <summary>
		/// Updates the is enabled.
		/// </summary>
		protected override void UpdateIsEnabled()
		{
			if ( _command != null &&
				 !_command.CanExecute(_CommandCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command.CanExecute(_CommandCell.CommandParameter));
		}
	}
}