﻿using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class DescriptionView : BaseTextView<BaseAiDescriptionCell>
	{
		private DescriptionCellBase _CurrentCell => _Renderer.Cell as DescriptionCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		public DescriptionView( BaseAiDescriptionCell renderer ) : base(renderer) { }


		public override bool UpdateText()
		{
			Text = _CurrentCell.Description;
			Hidden = string.IsNullOrEmpty(Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			ContentScaleFactor = _CurrentCell.DescriptionConfig.FontSize.ToNFloat();
			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}
		public override bool UpdateTextColor()
		{
			TextColor = _CurrentCell.DescriptionConfig.Color.ToUIColor();

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.DescriptionConfig.FontFamily;
			FontAttributes attr = _CurrentCell.DescriptionConfig.FontAttributes;
			var size = (float) _CurrentCell.DescriptionConfig.FontSize;

			Font = FontUtility.CreateNativeFont(family, size, attr);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.DescriptionConfig.TextAlignment;
			TextAlignment = alignment.ToUITextAlignment();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
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
			// if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionColorProperty.PropertyName ) { return UpdateBackgroundColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellDescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}