namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class DescriptionView : BaseTextView<DescriptionCellBase, BaseCellView>
	{
		protected override IUseConfiguration _Config => _Cell.DescriptionConfig;


		public DescriptionView( BaseCellView renderer ) : base(renderer) { }

		public override void Initialize( UIStackView parent )
		{
			parent.AddArrangedSubview(Control);
			
			base.Initialize(parent);
		}

		public override bool UpdateText() => UpdateText(_Cell.Description);

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.IsEqual(DescriptionCellBase.descriptionProperty) ) { return UpdateText(); }

			if ( e.IsEqual(DescriptionCellBase.descriptionFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(DescriptionCellBase.descriptionFontFamilyProperty, DescriptionCellBase.descriptionFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(DescriptionCellBase.descriptionColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(DescriptionCellBase.descriptionAlignmentProperty) ) { return UpdateTextAlignment(); }

			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellDescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.cellDescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}