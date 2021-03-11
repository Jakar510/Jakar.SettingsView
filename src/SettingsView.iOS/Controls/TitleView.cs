using System;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using BaseCellView = Jakar.SettingsView.iOS.BaseCell.BaseCellView;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
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
			ContentScaleFactor = (nfloat) _CurrentCell.TitleConfig.FontSize;
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
			if ( e.PropertyName == TitleCellBase.TitleProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == TitleCellBase.TitleColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == TitleCellBase.TitleFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == TitleCellBase.TitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == TitleCellBase.TitleFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == TitleCellBase.TitleAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleColorProperty.PropertyName ) { return UpdateBackgroundColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellTitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellTitleFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}