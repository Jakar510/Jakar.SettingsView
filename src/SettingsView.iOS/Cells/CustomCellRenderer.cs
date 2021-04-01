using System;
using System.Collections.Generic;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Enumerations;
using UIKit;
using Xamarin.Forms;

#nullable enable
[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }

	[Preserve(AllMembers = true)]
	public class CustomCellView : BaseDescriptiveTitleCell
	{
		protected CustomCell _CustomCell => Cell as CustomCell ?? throw new NullReferenceException(nameof(_CustomCell));

		protected ICommand? _command;
		protected CustomCellContent? _coreView;
		private readonly Dictionary<UIView, UIColor?> _colorCache = new();


		public CustomCellView( Cell formsCell ) : base(formsCell)
		{
			SelectionStyle = _CustomCell.IsSelectable
								 ? UITableViewCellSelectionStyle.Default
								 : UITableViewCellSelectionStyle.None;

			
			if ( _CustomCell.ShowArrowIndicator )
			{
				Accessory = UITableViewCellAccessory.DisclosureIndicator;
				EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

				SetRightMarginZero(_MainStack);
			}

			// _TitleStack.RemoveArrangedSubview(_ValueStack);
			// _TitleStack.RemoveArrangedSubview(_Description);
			// _ValueStack.RemoveFromSuperview();
			// _Description.RemoveFromSuperview();

			_coreView = new CustomCellContent(_CustomCell);

			if ( _CustomCell.UseFullSize )
			{
				_MainStack.RemoveArrangedSubview(_Icon);
				_Icon.RemoveFromSuperview();

				_MainStack.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
				_MainStack.Spacing = 0;
			}

			if ( _TitleStack is null ) throw new NullReferenceException(nameof(_TitleStack));

			_TitleStack.AddArrangedSubview(_coreView);
		}


		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			LayoutIfNeeded(); // let the layout immediately reflect when update constraints.
		}
		

		protected virtual void UpdateContent( UITableView tableView )
		{
			if ( _coreView is null )
				return; // for HotReload;

			_coreView.CustomCell = _CustomCell;
			_coreView.UpdateCell(_CustomCell.Content, tableView);
		}


		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( !_CustomCell.IsSelectable ) { return; }

			Run();
			if ( !_CustomCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		public override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			if ( _CustomCell.LongCommand is null ) { return false; }

			_CustomCell.SendLongCommand();

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

			BackupSubviewsColor(_coreView?.Subviews[0], _colorCache);

			base.SetHighlighted(highlighted, animated);

			RestoreSubviewsColor(_coreView?.Subviews[0], _colorCache);
		}

		public override void SetSelected( bool selected, bool animated )
		{
			if ( !selected )
			{
				base.SetSelected(selected, animated);
				return;
			}

			base.SetSelected(selected, animated);

			RestoreSubviewsColor(_coreView?.Subviews[0], _colorCache);
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
				if ( _command is not null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_command = null;

				_colorCache.Clear();

				_coreView?.RemoveFromSuperview();
				_coreView?.Dispose();
				_coreView = null;
			}

			base.Dispose(disposing);
		}

		protected virtual void UpdateCommand()
		{
			if ( _command is not null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_command = _CustomCell.Command;

			if ( _command is null ) return;
			_command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_command, EventArgs.Empty);
		}

		private void Run()
		{
			if ( _command is null ) { return; }

			if ( _command.CanExecute(_CustomCell.CommandParameter) ) { _command.Execute(_CustomCell.CommandParameter); }
		}

		protected override void UpdateIsEnabled()
		{
			if ( _command is not null &&
				 !_command.CanExecute(_CustomCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		protected virtual void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( _CellBase is null || !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command?.CanExecute(_CustomCell.CommandParameter) ?? false);
		}
	}
}