using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Extensions;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Button cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	/// <summary>
	/// Button cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class ButtonCellView : CellBaseView
	{
		internal Action Execute { get; set; }
		private ButtonCell _ButtonCell => Cell as ButtonCell;
		private ICommand _command;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.ButtonCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public ButtonCellView( Context context, Cell cell ) : base(context, cell) => DescriptionLabel.Visibility = Android.Views.ViewStates.Gone;

		public ButtonCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == ButtonCell.CommandProperty.PropertyName ||
				 e.PropertyName == ButtonCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
			else if ( e.PropertyName == ButtonCell.TitleAlignmentProperty.PropertyName ) { UpdateTitleAlignment(); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { Execute?.Invoke(); }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateCommand();
			UpdateTitleAlignment();
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

		private void UpdateTitleAlignment() { TitleLabel.Gravity = _ButtonCell.TitleAlignment.ToGravityFlags(); }

		private void UpdateCommand()
		{
			if ( _command != null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_command = _ButtonCell.Command;

			if ( _command != null )
			{
				_command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_command, EventArgs.Empty);
			}

			Execute = () =>
					  {
						  if ( _command == null ) { return; }

						  if ( _command.CanExecute(_ButtonCell.CommandParameter) ) { _command.Execute(_ButtonCell.CommandParameter); }
					  };
		}

		/// <summary>
		/// Updates the is enabled.
		/// </summary>
		protected override void UpdateIsEnabled()
		{
			if ( _command != null &&
				 !_command.CanExecute(_ButtonCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command.CanExecute(_ButtonCell.CommandParameter));
		}
	}
}