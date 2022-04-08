namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class ValueView<TCell> : BaseTextView<TCell, BaseLabelCellView<TCell>>, IRenderValue where TCell : ValueCellBase
	{
		protected override IUseConfiguration _Config => _Cell.ValueTextConfig;
		public ValueView( BaseLabelCellView<TCell> renderer ) : base(renderer) { }

		public override void Initialize( UIStackView parent )
		{
			parent.AddArrangedSubview(Control);
			
			base.Initialize(parent);
		}

		public override bool UpdateText()
		{
			if ( _Cell is not ValueTextCellBase cell ) { return false; }

			return UpdateText(cell.ValueText);
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == ValueTextCellBase.valueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == ValueCellBase.valueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == ValueCellBase.valueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == ValueCellBase.valueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == ValueCellBase.valueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == ValueCellBase.valueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return false;
		}

		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.cellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.cellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return false;
		}
	}
}
