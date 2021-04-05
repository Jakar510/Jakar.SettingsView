using System;
using System.Collections.Generic;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;


#nullable enable
[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }



	[Preserve(AllMembers = true)]
	public class CustomCellView : BaseDescriptiveTitleCell<CustomCell>
	{
		protected ICommand? _Command { get; set; }
		protected CustomCellContent? _CoreView { get; set; }
		private readonly Dictionary<UIView, UIColor?> _colorCache = new();


		public CustomCellView( CustomCell formsCell ) : base(formsCell)
		{
			SelectionStyle = Cell.IsSelectable
								 ? UITableViewCellSelectionStyle.Default
								 : UITableViewCellSelectionStyle.None;


			if ( Cell.ShowArrowIndicator )
			{
				Accessory        = UITableViewCellAccessory.DisclosureIndicator;
				EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

				SetRightMarginZero(_MainStack);
			}

			// _TitleStack.RemoveArrangedSubview(_ValueStack);
			// _TitleStack.RemoveArrangedSubview(_Description);
			// _ValueStack.RemoveFromSuperview();
			// _Description.RemoveFromSuperview();

			_CoreView = new CustomCellContent(Cell);

			if ( Cell.UseFullSize )
			{
				_MainStack.RemoveArrangedSubview(_Icon.Control);
				_Icon.Control.RemoveFromSuperview();

				_MainStack.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
				_MainStack.Spacing       = 0;
			}

			if ( _TitleStack is null ) throw new NullReferenceException(nameof(_TitleStack));

			_TitleStack.AddArrangedSubview(_CoreView);
		}


		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			LayoutIfNeeded(); // let the layout immediately reflect when update constraints.
		}


		protected virtual void UpdateContent( UITableView tableView )
		{
			if ( _CoreView is null )
				return; // for HotReload;

			_CoreView.CustomCell = Cell;
			_CoreView.UpdateCell(Cell.Content, tableView);
		}


		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( !Cell.IsSelectable ) { return; }

			Run();
			if ( !Cell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		public override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			if ( Cell.LongCommand is null ) { return false; }

			Cell.SendLongCommand();

			return true;
		}

		public override void SetHighlighted( bool highlighted, bool animated )
		{
			if ( !highlighted )
			{
				base.SetHighlighted(highlighted, animated);
				return;
			}

			// https://stackoverflow.com/questions/6745919/uitableviewcell-subview-disappears-when-cell-is-selected

			BackupSubviewsColor(_CoreView?.Subviews[0], _colorCache);

			base.SetHighlighted(highlighted, animated);

			RestoreSubviewsColor(_CoreView?.Subviews[0], _colorCache);
		}

		public override void SetSelected( bool selected, bool animated )
		{
			if ( !selected )
			{
				base.SetSelected(selected, animated);
				return;
			}

			base.SetSelected(selected, animated);

			RestoreSubviewsColor(_CoreView?.Subviews[0], _colorCache);
		}

		private void BackupSubviewsColor( UIView? view, IDictionary<UIView, UIColor?> colors )
		{
			if ( view is null ) throw new NullReferenceException(nameof(view));

			colors[view] = view.BackgroundColor;

			foreach ( UIView subView in view.Subviews ) { BackupSubviewsColor(subView, colors); }
		}

		private void RestoreSubviewsColor( UIView? view, IReadOnlyDictionary<UIView, UIColor?> colors )
		{
			if ( view is null ) throw new NullReferenceException(nameof(view));

			if ( colors.TryGetValue(view, out UIColor? color) ) { view.BackgroundColor = color; }

			foreach ( UIView subView in view.Subviews ) { RestoreSubviewsColor(subView, colors); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdateContent(tableView);
			UpdateCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command is not null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_Command = null;

				_colorCache.Clear();

				_CoreView?.RemoveFromSuperview();
				_CoreView?.Dispose();
				_CoreView = null;
			}

			base.Dispose(disposing);
		}

		protected virtual void UpdateCommand()
		{
			if ( _Command is not null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = Cell.Command;

			if ( _Command is null ) return;
			_Command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_Command, EventArgs.Empty);
		}

		private void Run()
		{
			if ( _Command is null ) { return; }

			if ( _Command.CanExecute(Cell.CommandParameter) ) { _Command.Execute(Cell.CommandParameter); }
		}

		protected override void UpdateIsEnabled()
		{
			if ( _Command is not null &&
				 !_Command.CanExecute(Cell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		protected virtual void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( _CellBase is null || !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(Cell.CommandParameter) ?? false);
		}
	}
}
