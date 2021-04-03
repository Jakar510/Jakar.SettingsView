using System;
using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	public class LabelCellView : BaseLabelCellView<LabelCell>
	{
		public LabelCellView( LabelCell formsCell ) : base(formsCell)
		{
			// ValueLabel.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			// ValueLabel.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);
		}

		// public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		// {
		// 	base.CellPropertyChanged(sender, e);
		//
		// 	if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { UpdateValueText(); }
		// 	else if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ||
		// 			  e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
		// 			  e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		// 	else if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
		// }
		//
		// public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		// {
		// 	base.ParentPropertyChanged(sender, e);
		//
		// 	if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
		// 	else if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ||
		// 			  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
		// 			  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		// }

		// public override void UpdateCell( UITableView tableView )
		// {
		// 	UpdateValueText();
		// 	UpdateValueTextColor();
		// 	UpdateValueTextFont();
		// 	base.UpdateCell(tableView);
		// }

		// protected override void SetEnabledAppearance( bool isEnabled )
		// {
		// 	if ( isEnabled ) { ValueLabel.Alpha = 1f; }
		// 	else { ValueLabel.Alpha = 0.3f; }
		//
		// 	base.SetEnabledAppearance(isEnabled);
		// }

		// protected void UpdateValueText()
		// {
		// 	if ( ValueLabel is null ) return;     // for HotReload
		// 	if ( ValueTextCell is null ) return; // for HotReload
		//
		// 	ValueLabel.Text = ValueTextCell.ValueText;
		// }
		//
		// private void UpdateValueTextFont()
		// {
		// 	if ( ValueLabel is null ) return;     // for HotReload
		// 	if ( ValueTextCell is null ) return; // for HotReload
		//
		// 	ValueCellBase.ValueTextConfiguration config = ValueTextCell.ValueTextConfig;
		// 	ValueLabel.Font = FontUtility.CreateNativeFont(config.FontFamily, config.FontSize.ToFloat(), config.FontAttributes);
		// }
		//
		// private void UpdateValueTextColor()
		// {
		// 	if ( ValueLabel is null ) return;     // for HotReload
		// 	if ( ValueTextCell is null ) return; // for HotReload
		//
		// 	ValueLabel.TextColor = ValueTextCell.ValueTextConfig.Color.ToUIColor();
		// }

		// protected override void Dispose( bool disposing )
		// {
		// 	if ( disposing )
		// 	{
		// 		_ValueStack?.RemoveArrangedSubview(ValueLabel);
		// 		ValueLabel.Dispose();
		// 	}
		//
		// 	base.Dispose(disposing);
		// }
	}
}