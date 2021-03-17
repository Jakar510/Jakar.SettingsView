using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class ValueView : BaseTextView
	{
		private ValueCellBase _CurrentCell => _Renderer.Cell as ValueCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));
		private ValueTextCellBase? _CurrentTextCell => _CurrentCell as ValueTextCellBase;


		public ValueView( BaseCellView renderer ) : base(renderer) => Initialize();


		public override void SetUsed( Cell cell ) { SetUsed(cell.IsValueCell()); }
		public override bool UpdateText() => _CurrentTextCell is not null && UpdateText(_CurrentTextCell.ValueText);
		public bool UpdateText( string? text )
		{
			Text = text;
			Hidden = string.IsNullOrEmpty(Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			ContentScaleFactor = _CurrentCell.ValueTextConfig.FontSize.ToNFloat();

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
			if ( !_IsAvailable ) return false;

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
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}