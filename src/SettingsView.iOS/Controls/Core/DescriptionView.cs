using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;


#nullable enable
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

			if ( e.IsEqual(DescriptionCellBase.DescriptionProperty) ) { return UpdateText(); }

			if ( e.IsEqual(DescriptionCellBase.DescriptionFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(DescriptionCellBase.DescriptionFontFamilyProperty, DescriptionCellBase.DescriptionFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(DescriptionCellBase.DescriptionColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(DescriptionCellBase.DescriptionAlignmentProperty) ) { return UpdateTextAlignment(); }

			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}