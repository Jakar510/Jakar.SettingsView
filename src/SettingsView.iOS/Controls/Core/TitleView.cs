namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class TitleView : BaseTextView<TitleCellBase, BaseCellView>
	{
		protected override IUseConfiguration _Config => _Cell.TitleConfig;

		public TitleView( BaseCellView renderer ) : base(renderer) { }

		public override void Initialize( UIStackView parent )
		{
			parent.AddArrangedSubview(Control);

			base.Initialize(parent);
		}

		public override bool UpdateText() => UpdateText(_Cell.Title);


		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.IsEqual(TitleCellBase.titleProperty) ) { return UpdateText(); }

			if ( e.IsEqual(TitleCellBase.titleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(TitleCellBase.titleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(TitleCellBase.titleFontFamilyProperty, TitleCellBase.titleFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(TitleCellBase.titleAlignmentProperty) ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.IsEqual(Shared.sv.SettingsView.cellTitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.cellTitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsEqual(Shared.sv.SettingsView.cellTitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.cellTitleFontFamilyProperty, Shared.sv.SettingsView.cellTitleFontAttributesProperty) ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}