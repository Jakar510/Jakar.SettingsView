using System;
using System.Collections.Generic;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }

	[Preserve(AllMembers = true)]
	public class CustomCellView : BaseCellView 
	{
		protected CustomCell CustomCell => Cell as CustomCell;
		protected Action Execute { get; set; }
		protected ICommand _command;
		protected CustomCellContent _coreView;
		private Dictionary<UIView, UIColor> _colorCache = new Dictionary<UIView, UIColor>();

		public CustomCellView( Cell formsCell ) : base(formsCell)
		{
			SelectionStyle = CustomCell.IsSelectable
								 ? UITableViewCellSelectionStyle.Default
								 : UITableViewCellSelectionStyle.None;
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			LayoutIfNeeded(); // let the layout immediately reflect when update constraints.
		}

		protected override void SetUpContentView()
		{
			base.SetUpContentView();

			if ( CustomCell.ShowArrowIndicator )
			{
				Accessory = UITableViewCellAccessory.DisclosureIndicator;
				EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

				SetRightMarginZero();
			}

			_StackV.RemoveArrangedSubview(_ContentStack);
			_StackV.RemoveArrangedSubview(DescriptionLabel);
			_ContentStack.RemoveFromSuperview();
			DescriptionLabel.RemoveFromSuperview();

			_coreView = new CustomCellContent();

			if ( CustomCell.UseFullSize )
			{
				_StackH.RemoveArrangedSubview(IconView);
				IconView.RemoveFromSuperview();

				_StackH.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
				_StackH.Spacing = 0;
			}

			_StackV.AddArrangedSubview(_coreView);
		}

		protected virtual void UpdateContent( UITableView tableView )
		{
			if ( _coreView is null )
				return; // for HotReload;

			_coreView.CustomCell = CustomCell;
			_coreView.UpdateCell(CustomCell.Content, tableView);
		}


		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( !CustomCell.IsSelectable ) { return; }

			Execute?.Invoke();
			if ( !CustomCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		public override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			if ( CustomCell.LongCommand is null ) { return false; }

			CustomCell.SendLongCommand();

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

			BackupSubviewsColor(_coreView.Subviews[0], _colorCache);

			base.SetHighlighted(highlighted, animated);

			RestoreSubviewsColor(_coreView.Subviews[0], _colorCache);
		}

		public override void SetSelected( bool selected, bool animated )
		{
			if ( !selected )
			{
				base.SetSelected(selected, animated);
				return;
			}

			base.SetSelected(selected, animated);

			RestoreSubviewsColor(_coreView.Subviews[0], _colorCache);
		}

		private void BackupSubviewsColor( UIView view, Dictionary<UIView, UIColor> colors )
		{
			colors[view] = view.BackgroundColor;

			foreach ( UIView subView in view.Subviews ) { BackupSubviewsColor(subView, colors); }
		}

		private void RestoreSubviewsColor( UIView view, Dictionary<UIView, UIColor> colors )
		{
			if ( colors.TryGetValue(view, out var color) ) { view.BackgroundColor = color; }

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

				Execute = null;
				_command = null;

				_colorCache.Clear();
				_colorCache = null;

				_coreView?.RemoveFromSuperview();
				_coreView?.Dispose();
				_coreView = null;
			}

			base.Dispose(disposing);
		}

		protected virtual void UpdateCommand()
		{
			if ( _command is not null ) { _command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_command = CustomCell.Command;

			if ( _command is not null )
			{
				_command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_command, EventArgs.Empty);
			}

			Execute = () =>
					  {
						  if ( _command is null ) { return; }

						  if ( _command.CanExecute(CustomCell.CommandParameter) ) { _command.Execute(CustomCell.CommandParameter); }
					  };
		}

		protected override void UpdateIsEnabled()
		{
			if ( _command is not null &&
				 !_command.CanExecute(CustomCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		protected virtual void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_command.CanExecute(CustomCell.CommandParameter));
		}
	}
}