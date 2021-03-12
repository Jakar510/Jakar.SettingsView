using System;
using System.ComponentModel;
using Android.Content;
using Android.Util;
using Android.Views;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using TextAlignment = Xamarin.Forms.TextAlignment;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls.Core
{
	public class HintView : BaseTextView
	{
		[Android.Runtime.Preserve(AllMembers = true)]
		private HintTextCellBase _CurrentCell => _Cell.Cell as HintTextCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		public HintView( Context context ) : base(context) { }
		public HintView( BaseCellView baseView, Context context ) : base(baseView, context) { }
		public HintView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		public override bool UpdateText()
		{
			Text = _CurrentCell.Hint;
			Visibility = string.IsNullOrEmpty(Text)
							 ? ViewStates.Gone
							 : ViewStates.Visible;

			return true;
		}
		public override bool UpdateFontSize()
		{
			SetTextSize(ComplexUnitType.Sp, (float) _CurrentCell.HintConfig.FontSize);
			// SetTextSize(ComplexUnitType.Sp, DefaultFontSize);

			return true;
		}
		public override bool UpdateTextColor()
		{
			SetTextColor(_CurrentCell.HintConfig.Color.ToAndroid());
			SetTextColor(DefaultTextColor);

			return true;
		}
		public override bool UpdateFont()
		{
			string? family = _CurrentCell.HintConfig.FontFamily;
			FontAttributes attr = _CurrentCell.DescriptionConfig.FontAttributes;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		public override bool UpdateTextAlignment()
		{
			TextAlignment alignment = _CurrentCell.HintConfig.TextAlignment;
			TextAlignment = alignment.ToAndroidTextAlignment();
			Gravity = alignment.ToGravityFlags();

			return true;
		}

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(HintTextCellBase.HintProperty) ) { return UpdateText(); }

			if ( e.IsEqual(HintTextCellBase.HintFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(HintTextCellBase.HintFontFamilyProperty, HintTextCellBase.HintFontAttributesProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(HintTextCellBase.HintColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(HintTextCellBase.HintAlignmentProperty) ) { return UpdateTextAlignment(); }
			
			return base.Update(sender, e);
		}
		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellHintTextColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellHintAlignmentProperty) ) { return UpdateTextAlignment(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellHintFontSizeProperty) ) { return UpdateFontSize(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellHintTextColorProperty, Shared.sv.SettingsView.CellHintFontAttributesProperty) ) { return UpdateFont(); }

			return base.UpdateParent(sender, e);
		}
	}
}