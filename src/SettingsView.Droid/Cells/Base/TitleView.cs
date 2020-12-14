using System;
using System.ComponentModel;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Cells.Base
{
	public class TitleView : BaseView
	{
		public TitleView( CellBaseView baseView, TextView view ) : base(baseView, view) { }
		protected internal override bool UpdateText()
		{
			Label.Text = _Cell.CellBase.Title;
			//hide TextView right padding when TextView.Text empty.
			Label.Visibility = string.IsNullOrEmpty(Label.Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _Cell.CellBase.TitleColor != Color.Default ) { Label.SetTextColor(_Cell.CellBase.TitleColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellTitleColor != Color.Default ) { Label.SetTextColor(_Cell.CellParent.CellTitleColor.ToAndroid()); }
			else { Label.SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _Cell.CellBase.TitleFontSize > 0 ) { Label.SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellBase.TitleFontSize); }
			else if ( _Cell.CellParent != null ) { Label.SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellTitleFontSize); }
			else { Label.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateFont()
		{
			string? family = _Cell.CellBase.HintFontFamily ?? _Cell.CellParent?.CellTitleFontFamily;
			FontAttributes attr = _Cell.CellBase.TitleFontAttributes ?? _Cell.CellParent?.CellTitleFontAttributes ?? FontAttributes.None;

			Label.Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		// protected void UpdateTitleAlignment() { _Label.TextAlignment = _CellBase.TitleTextAlignment; }

		protected internal override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.TitleProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBase.TitleColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBase.TitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBase.TitleFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			return false;
		}
		protected internal override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellTitleColorProperty.PropertyName )
			{ return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellTitleFontSizeProperty.PropertyName )
			{ return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellTitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellTitleFontAttributesProperty.PropertyName )
			{ return UpdateFont(); }

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