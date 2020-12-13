using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	/// <summary>
	/// Command cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }

	/// <summary>
	/// Command cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class CommandCellView : LabelCellView
	{
		internal Action Execute { get; set; }
		private CommandCell _CommandCell => Cell as CommandCell;
		private ICommand _command;
		private ImageView _indicatorView;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.CommandCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public CommandCellView( Context context, Cell cell ) : base(context, cell)
		{
			if ( !CellParent.ShowArrowIndicatorForAndroid ) { return; }

			if ( _CommandCell.HideArrowIndicator ) { return; }

			_indicatorView = new ImageView(context);
			_indicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);

			var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
						{ };

			using ( param ) { AccessoryStack.AddView(_indicatorView, param); }
		}

		public CommandCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

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

		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			Execute?.Invoke();
			if ( _CommandCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
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
				_indicatorView?.RemoveFromParent();
				_indicatorView?.SetImageDrawable(null);
				_indicatorView?.SetImageBitmap(null);
				_indicatorView?.Dispose();
				_indicatorView = null;
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
			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command.CanExecute(_CommandCell.CommandParameter));
		}
	}
}