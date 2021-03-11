using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// [assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class LabelCellView : CellBaseView
	{
		public UILabel ValueLabel { get; set; }

		private LabelCell _LabelCell => Cell as LabelCell;

		public LabelCellView( Cell formsCell ) : base(formsCell)
		{
			ValueLabel = new UILabel
						 {
							 TextAlignment = UITextAlignment.Right
						 };

			ContentStack.AddArrangedSubview(ValueLabel);
			ValueLabel.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			ValueLabel.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);
		}

		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
			else if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFont();
			base.UpdateCell(tableView);
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { ValueLabel.Alpha = 1f; }
			else { ValueLabel.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		protected void UpdateValueText() { ValueLabel.Text = _LabelCell.ValueText; }

		private void UpdateValueTextFont()
		{
			if ( ValueLabel.Font is null )
				return; // for HotReload

			string family = _LabelCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _LabelCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			if ( _LabelCell.ValueTextFontSize > 0 ) { ValueLabel.Font = FontUtility.CreateNativeFont(family, (float) _LabelCell.ValueTextFontSize, attr); }
			else if ( CellParent != null ) { ValueLabel.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellValueTextFontSize, attr); }
		}

		private void UpdateValueTextColor()
		{
			if ( _LabelCell.ValueTextColor != Color.Default ) { ValueLabel.TextColor = _LabelCell.ValueTextColor.ToUIColor(); }
			else if ( CellParent != null &&
					  CellParent.CellValueTextColor != Color.Default ) { ValueLabel.TextColor = CellParent.CellValueTextColor.ToUIColor(); }
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
				ContentStack.RemoveArrangedSubview(ValueLabel);
				ValueLabel.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}