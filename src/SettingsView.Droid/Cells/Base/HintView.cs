using System.ComponentModel;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Cells.Base
{
	public class HintView : BaseView
	{
		public HintView( CellBaseView baseView, TextView view ) : base(baseView, view) { }

		protected void UpdateHintText()
		{
			string msg = _Cell.CellBase.HintText;
			if ( string.IsNullOrEmpty(msg) )
			{
				Label.Visibility = ViewStates.Invisible;
				return;
			}

			Label.Text = msg;
			Label.Visibility = ViewStates.Visible;
		}
		protected void UpdateHintTextColor()
		{
			if ( _Cell.CellBase.HintTextColor != Color.Default ) { Label.SetTextColor(_Cell.CellBase.HintTextColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellHintTextColor != Color.Default ) { Label.SetTextColor(_Cell.CellParent.CellHintTextColor.ToAndroid()); }
			else { Label.SetTextColor(DefaultTextColor); }
		}
		// protected void UpdateHintAlignment() { _Label.TextAlignment = _CellBase.HintTextAlignment; }

		protected void UpdateHint( object sender, PropertyChangedEventArgs e ) { }
		protected internal override bool UpdateText()
		{
			Label.Text = _Cell.CellBase.Description;
			Label.Visibility = string.IsNullOrEmpty(Label.Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _Cell.CellBase.HintFontSize > 0 ) { Label.SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellBase.HintFontSize); }
			else if ( _Cell.CellParent != null ) { Label.SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellHintFontSize); }
			else { Label.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _Cell.CellBase.DescriptionColor != Color.Default ) { Label.SetTextColor(_Cell.CellBase.DescriptionColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellDescriptionColor != Color.Default ) { Label.SetTextColor(_Cell.CellParent.CellDescriptionColor.ToAndroid()); }
			else { Label.SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFont()
		{
			string? family = _Cell.CellBase.HintFontFamily ?? _Cell.CellParent?.CellHintFontFamily;
			FontAttributes attr = _Cell.CellBase.HintFontAttributes ?? _Cell.CellParent?.CellHintFontAttributes ?? FontAttributes.None;

			Label.Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		// protected void UpdateDescriptionAlignment() { _Label.TextAlignment = _CellBase.DescriptionTextAlignment; }

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