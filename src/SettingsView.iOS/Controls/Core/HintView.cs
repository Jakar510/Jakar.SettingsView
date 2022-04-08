namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class HintView : BaseTextView<HintTextCellBase, BaseCellView>
	{
		protected override IUseConfiguration _Config => _Cell.HintConfig;


		public HintView( BaseCellView renderer ) : base(renderer) { }

		public override void Initialize( UIStackView parent )
		{
			parent.AddArrangedSubview(Control);
			
			base.Initialize(parent);
		}
		

		public override bool UpdateText() => UpdateText(_Cell.Title);

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == HintTextCellBase.hintProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == HintTextCellBase.hintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == HintTextCellBase.hintFontFamilyProperty.PropertyName ||
				 e.PropertyName == HintTextCellBase.hintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == HintTextCellBase.hintColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == HintTextCellBase.hintAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}

		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == Shared.sv.SettingsView.cellHintTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellHintAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellHintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.cellHintTextColorProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.cellHintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}
