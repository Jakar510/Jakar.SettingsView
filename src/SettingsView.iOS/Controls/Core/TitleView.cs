using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class TitleView : BaseTextView<BaseAiTitledCell>
	{
		public BaseAiTitledCell Renderer { get; }
		private TitleCellBase _CurrentCell => _Renderer.Cell as TitleCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));
		public TitleView( BaseAiTitledCell renderer ) : base(renderer) => Renderer = renderer;


		public override bool UpdateText()
		{
			Text = _CurrentCell.Title;
			Hidden = string.IsNullOrEmpty(Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			ContentScaleFactor = _CurrentCell.TitleConfig.FontSize.ToNFloat();
			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}
		public override bool UpdateTextColor()
		{
			TextColor = _CurrentCell.TitleConfig.Color.ToUIColor();

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.TitleConfig.FontFamily;
			FontAttributes attr = _CurrentCell.TitleConfig.FontAttributes;
			var size = (float) _CurrentCell.TitleConfig.FontSize;

			Font = FontUtility.CreateNativeFont(family, size, attr);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.TitleConfig.TextAlignment;
			TextAlignment = alignment.ToUITextAlignment();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(TitleCellBase.TitleProperty) ) { return UpdateText(); }

			if ( e.IsEqual(TitleCellBase.TitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(TitleCellBase.TitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(TitleCellBase.TitleFontFamilyProperty, TitleCellBase.TitleFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(TitleCellBase.TitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellTitleFontFamilyProperty, Shared.sv.SettingsView.CellTitleFontAttributesProperty) ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}