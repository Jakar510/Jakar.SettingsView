using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }


	[Preserve(AllMembers = true)]
	public class CustomCellView : BaseAiDescriptionCell
	{
		protected CustomCell _CustomCell => Cell as CustomCell ?? throw new NullReferenceException(nameof(_CustomCell));
		protected ICommand? _Command { get; set; }
		protected CustomCellContent _CoreView { get; set; }
		protected Dictionary<UIView, UIColor> _ColorCache { get; set; } = new();


		public CustomCellView( Cell cell ) : base(cell)
		{
			if ( _CustomCell.ShowArrowIndicator )
			{
				Accessory = UITableViewCellAccessory.DisclosureIndicator;
				EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;

				SetRightMarginZero();
			}

			// StackV.RemoveArrangedSubview(ContentStack);
			// StackV.RemoveArrangedSubview(DescriptionLabel);
			// ContentStack.RemoveFromSuperview();
			// DescriptionLabel.RemoveFromSuperview();

			_CoreView = new CustomCellContent(_CustomCell);
			_CoreView.WidthAnchor.ConstraintEqualTo(_RootView.WidthAnchor).Active = true;

			_CoreView.LeadingAnchor.ConstraintEqualTo(_RootView.LeadingAnchor).Active = true;
			_CoreView.TrailingAnchor.ConstraintEqualTo(_RootView.TrailingAnchor).Active = true;

			_CoreView.TopAnchor.ConstraintEqualTo(_RootView.TopAnchor).Active = true;
			_CoreView.BottomAnchor.ConstraintEqualTo(_RootView.BottomAnchor).Active = true;

			_CoreView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
			_CoreView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Vertical);

			_CoreView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			_CoreView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);

			if ( _CustomCell.UseFullSize )
			{
				_RootView.RemoveArrangedSubview(_ContentView);

				_Icon.RemoveFromSuperview();

				_ContentView.RemoveArrangedSubview(_TitleStack);
				_TitleStack.RemoveFromSuperview();
				_Title.RemoveFromSuperview();
				_Description.RemoveFromSuperview();

				_RootView.LayoutMargins = new UIEdgeInsets(0, 0, 0, 0);
				_RootView.Spacing = 0;
			}

			_RootView.AddArrangedSubview(_CoreView);
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			LayoutIfNeeded(); // let the layout immediately reflect when update constraints.
		}

		protected virtual void UpdateContent( UITableView? tableView ) { _CoreView.UpdateCell(_CustomCell.Content, tableView); }


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(CommandCell.CommandProperty, CommandCell.CommandParameterProperty) ) { UpdateCommand(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( !_CustomCell.IsSelectable ) { return; }

			Execute();
			if ( !_CustomCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }

			// base.RowSelected(tableView, indexPath);
		}

		protected internal override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			_CustomCell.LongCommand?.Execute(this);
			_CustomCell.SendLongCommand();

			return true;
			// return base.RowLongPressed(tableView, indexPath);
		}

		public override void SetHighlighted( bool highlighted, bool animated )
		{
			if ( !highlighted )
			{
				base.SetHighlighted(highlighted, animated);
				return;
			}

			// https://stackoverflow.com/questions/6745919/uitableviewcell-subview-disappears-when-cell-is-selected

			BackupSubviewsColor(_CoreView.Subviews[0], _ColorCache);

			base.SetHighlighted(highlighted, animated);

			RestoreSubviewsColor(_CoreView.Subviews[0], _ColorCache);
		}

		public override void SetSelected( bool selected, bool animated )
		{
			if ( !selected )
			{
				base.SetSelected(selected, animated);
				return;
			}

			base.SetSelected(selected, animated);

			RestoreSubviewsColor(_CoreView.Subviews[0], _ColorCache);
		}

		protected void BackupSubviewsColor( UIView view, Dictionary<UIView, UIColor> colors )
		{
			if ( view.BackgroundColor is null )  return;
			
			colors[view] = view.BackgroundColor;

			foreach ( UIView subView in view.Subviews ) { BackupSubviewsColor(subView, colors); }
		}
		protected void RestoreSubviewsColor( UIView view, Dictionary<UIView, UIColor> colors )
		{
			if ( colors.TryGetValue(view, out UIColor color) ) { view.BackgroundColor = color; }

			foreach ( UIView subView in view.Subviews ) { RestoreSubviewsColor(subView, colors); }
		}

		protected internal override void UpdateCell( UITableView? tableView )
		{
			base.UpdateCell(tableView);
			UpdateContent(tableView);
			UpdateCommand();
		}


		protected virtual void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _CustomCell.Command;

			if ( _Command != null )
			{
				_Command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_Command, EventArgs.Empty);
			}
			
			Execute();
		}
		protected void Execute()
		{
			if ( _Command is null ) { return; }

			if ( _Command.CanExecute(_CustomCell.CommandParameter) ) { _Command.Execute(_CustomCell.CommandParameter); }
		}

		protected override void UpdateIsEnabled()
		{
			if ( _Command != null &&
				 !_Command.CanExecute(_CustomCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		protected virtual void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !_CustomCell.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_CustomCell.CommandParameter) ?? _CustomCell.IsEnabled);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }
				
				_Command = null;

				_ColorCache.Clear();

				_CoreView.RemoveFromSuperview();
				_CoreView.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}