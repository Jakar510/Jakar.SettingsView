using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using BaseCellView = Jakar.SettingsView.iOS.BaseCell.BaseCellView;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class ValueView : BaseTextView<BaseAiValueCell>
	{
		private ValueCellBase _CurrentCell => _Renderer.Cell as ValueCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));
		private ValueTextCellBase? _CurrentTextCell => _CurrentCell as ValueTextCellBase;


		public ValueView( BaseAiValueCell renderer ) : base(renderer) => Initialize();


		public override bool UpdateText() => _CurrentTextCell is not null && UpdateText(_CurrentTextCell.ValueText);
		public bool UpdateText( string? text )
		{
			Text = text;
			Hidden = string.IsNullOrEmpty(Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			ContentScaleFactor = (nfloat) _CurrentCell.ValueTextConfig.FontSize;

			return true;
		}
		public override bool UpdateTextColor()
		{
			TextColor = _CurrentCell.ValueTextConfig.Color.ToUIColor();

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.ValueTextConfig.FontFamily;
			FontAttributes attr = _CurrentCell.ValueTextConfig.FontAttributes;
			var size = (float) _CurrentCell.ValueTextConfig.FontSize;

			Font = FontUtility.CreateNativeFont(family, size, attr);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.ValueTextConfig.TextAlignment;
			TextAlignment = alignment.ToUITextAlignment();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == ValueCellBase.ValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			// if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			// if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateBackgroundColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}