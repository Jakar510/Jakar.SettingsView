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

			if ( e.IsEqual(TitleCellBase.TitleProperty) ) { return UpdateText(); }

			if ( e.IsEqual(TitleCellBase.TitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(TitleCellBase.TitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(TitleCellBase.TitleFontFamilyProperty, TitleCellBase.TitleFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(TitleCellBase.TitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellTitleFontFamilyProperty, Shared.sv.SettingsView.CellTitleFontAttributesProperty) ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}