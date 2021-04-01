using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiSwitchCellRenderer = Jakar.SettingsView.iOS.Cells.SwitchCellRenderer;
using AiSwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;

[assembly: ExportRenderer(typeof(AiSwitchCell), typeof(AiSwitchCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class SwitchCellView : BaseAccessoryCell<AiSwitch>
	{
		private AiSwitchCell _SwitchCell => Cell as AiSwitchCell ?? throw new NullReferenceException(nameof(_SwitchCell));

		public SwitchCellView( Cell formsCell ) : base(formsCell) => _Accessory.ValueChanged += Switch_ValueChanged;

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CheckableCellBase.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			else if ( e.PropertyName == CheckableCellBase.CheckedProperty.PropertyName ) { UpdateOn(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
			else { base.ParentPropertyChanged(sender, e); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);

			UpdateAccentColor();
			UpdateOn();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _Accessory.ValueChanged -= Switch_ValueChanged; }

			base.Dispose(disposing);
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			_Accessory.Alpha = isEnabled
								   ? SVConstants.Cell.ENABLED_ALPHA
								   : SVConstants.Cell.DISABLED_ALPHA;

			base.SetEnabledAppearance(isEnabled);
		}

		private void Switch_ValueChanged( object sender, EventArgs e )
		{
			_SwitchCell.Checked = _Accessory.On;
			_SwitchCell.ValueChangedHandler.SendValueChanged(_Accessory.On);
		}

		private void UpdateOn()
		{
			if ( _Accessory.On != _SwitchCell.Checked ) { _Accessory.On = _SwitchCell.Checked; }
		}

		private void UpdateAccentColor() { _Accessory.OnTintColor = _SwitchCell.GetAccentColor().ToUIColor(); }
	}

	public class AiSwitch : UISwitch, IRenderAccessory
	{
		public void Initialize( Stack parent ) { throw new NotImplementedException(); }
		public void Update() { throw new NotImplementedException(); }
		public bool Update( object sender, PropertyChangedEventArgs e ) => throw new NotImplementedException();
		public bool UpdateParent( object sender, PropertyChangedEventArgs e ) => throw new NotImplementedException();
		public void Enable() { throw new NotImplementedException(); }
		public void Disable() { throw new NotImplementedException(); }
	}
}