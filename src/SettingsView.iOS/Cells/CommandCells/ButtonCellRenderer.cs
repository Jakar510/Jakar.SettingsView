using System;
using System.ComponentModel;
using Foundation;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Enumerations;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;


[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : BaseCellView
	{
		protected internal ButtonCell ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(ButtonCell));

		protected ButtonView _Button { get; set; }

		public ButtonCellView( Cell formsCell ) : base(formsCell)
		{
			SelectionStyle = UITableViewCellSelectionStyle.None;

			_Button = new ButtonView(this);
			_Button.Initialize(_MainStack);


			this.SetContent(_MainStack);

			double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SvConstants.Defaults.MIN_ROW_HEIGHT);
			_MinHeightConstraint = _MainStack.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
			_MinHeightConstraint.Priority = LayoutPriority.Highest.ToFloat(); //  fix warning-log:Unable to simultaneously satisfy constraints. this is superior to any other view.
			_MinHeightConstraint.Active = true;

			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _MainStack.AccessibilityIdentifier = Cell.AutomationId; }

			SetNeedsLayout();
		}


		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Button.Update(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}


		public override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			_Button.RunLong();
			return true;
			// return base.RowLongPressed(tableView, indexPath);
		}
		public override void RowSelected( UITableView adapter, NSIndexPath position ) { _Button.Run(); }


		protected void EnableCell() { _Button.SetEnabledAppearance(true); }
		protected void DisableCell() { _Button.SetEnabledAppearance(false); }

		public override void UpdateCell( UITableView tableView )
		{
			_Button.Update();
			base.UpdateCell(tableView);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _Button.Dispose(); }

			base.Dispose(disposing);
		}
	}
}