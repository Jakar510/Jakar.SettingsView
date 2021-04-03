using System;
using System.ComponentModel;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class DescriptionView : BaseTextView<DescriptionCellBase, BaseCellView>
	{
		protected override IUseConfiguration _Config => _Cell.DescriptionConfig;


		public DescriptionView( BaseCellView renderer ) : base(renderer) { }

		public override void Initialize( Stack parent )
		{
			parent.AddArrangedSubview(Control);

			Control.AutoresizingMask = UIViewAutoresizing.FlexibleMargins | UIViewAutoresizing.FlexibleHeight;
			Control.TranslatesAutoresizingMaskIntoConstraints = true;

			Control.WidthAnchor.ConstraintEqualTo(parent.WidthAnchor).Active = true;

			base.Initialize(parent);
		}

		public override bool UpdateText() => UpdateText(_Cell.Description);

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == DescriptionCellBase.DescriptionProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == DescriptionCellBase.DescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == DescriptionCellBase.DescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

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