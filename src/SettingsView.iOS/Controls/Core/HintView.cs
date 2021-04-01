using System;
using System.ComponentModel;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	[Foundation.Preserve(AllMembers = true)]
	public class HintView : BaseTextView
	{
		private HintTextCellBase _CurrentCell => _Renderer.Cell as HintTextCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		public HintView( BaseCellView renderer ) : base(renderer)
		{
			LineBreakMode = UILineBreakMode.Clip;
			Lines = 0;
			TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
			AdjustsFontSizeToFitWidth = true;
			BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
			TextAlignment = UITextAlignment.Right;
			AdjustsLetterSpacingToFitWidth = true;
		}

		public override void Initialize( Stack parent )
		{
			parent.AddSubview(this);

			TranslatesAutoresizingMaskIntoConstraints = false;
			TopAnchor.ConstraintEqualTo(parent.TopAnchor, 2).Active = true;
			LeftAnchor.ConstraintEqualTo(parent.LeftAnchor, 16).Active = true;
			RightAnchor.ConstraintEqualTo(parent.RightAnchor, -10).Active = true;
			// RightAnchor.ConstraintEqualTo(parent.ContentView.RightAnchor, -10).Active = true;
			BottomAnchor.ConstraintLessThanOrEqualTo(parent.BottomAnchor, -12).Active = true;
			
			base.Initialize(parent);
		}


		public override void SetUsed( Cell cell ) { SetUsed(cell.IsValueCell()); }
		public override bool UpdateText()
		{
			Text = _CurrentCell.Hint;
			Hidden = string.IsNullOrEmpty(Text);

			return true;
		}
		public override bool UpdateFontSize()
		{
			ContentScaleFactor = _CurrentCell.HintConfig.FontSize.ToNFloat();
			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}
		public override bool UpdateTextColor()
		{
			TextColor = _CurrentCell.HintConfig.Color.ToUIColor();

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.HintConfig.FontFamily;
			FontAttributes attr = _CurrentCell.HintConfig.FontAttributes;
			var size = (float) _CurrentCell.HintConfig.FontSize;

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
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == HintTextCellBase.HintProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == HintTextCellBase.HintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == HintTextCellBase.HintFontFamilyProperty.PropertyName ||
				 e.PropertyName == HintTextCellBase.HintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == HintTextCellBase.HintColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == HintTextCellBase.HintAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( !_IsAvailable ) return false;

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintTextColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintAlignmentProperty.PropertyName ) { return UpdateTextAlignment(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellHintTextColorProperty.PropertyName ||
				 e.PropertyName == Shared.sv.SettingsView.CellHintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}