using System.ComponentModel;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public class HintView : BaseTextView
	{
		public HintView( Context context ) : base(context) { }
		public HintView( CellBaseView baseView, Context context ) : base(baseView, context) { }
		public HintView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		protected internal override bool UpdateText()
		{
			Text = _Cell.CellBase.Description;
			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _Cell.CellBase.HintFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellBase.HintFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellHintFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _Cell.CellBase.HintTextColor != Color.Default ) { SetTextColor(_Cell.CellBase.HintTextColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellHintTextColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellHintTextColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFont()
		{
			string? family = _Cell.CellBase.HintFontFamily ?? _Cell.CellParent?.CellHintFontFamily;
			FontAttributes attr = _Cell.CellBase.HintFontAttributes ?? _Cell.CellParent?.CellHintFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}

		// protected internal  override bool UpdateAlignment() { _TextAlignment = _CellBase.HintTextAlignment; }

		protected internal override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.HintTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBase.HintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBase.HintFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBase.HintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBase.HintColorProperty.PropertyName ) { return UpdateColor(); }

			return false;
		}
		protected internal override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellHintTextColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellHintFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellHintTextColorProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellHintFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return false;
		}
		protected internal override void Update()
		{
			UpdateText();
			UpdateColor();
			UpdateFontSize();
			UpdateFont();
		}
	}
}