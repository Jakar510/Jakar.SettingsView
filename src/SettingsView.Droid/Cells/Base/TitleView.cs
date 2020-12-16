using System;
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
	public class TitleView : BaseTextView
	{
		public TitleView( Context context ) : base(context) { }
		public TitleView( CellBaseView baseView, Context context ) : base(baseView, context) { }
		public TitleView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		protected internal override bool UpdateText()
		{
			Text = _Cell.CellBase.Title;
			//hide TextView right padding when TextView.Text empty.
			// Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _Cell.CellBase.TitleColor != Color.Default ) { SetTextColor(_Cell.CellBase.TitleColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellTitleColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellTitleColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _Cell.CellBase.TitleFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellBase.TitleFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellTitleFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateFont()
		{
			string? family = _Cell.CellBase.HintFontFamily ?? _Cell.CellParent?.CellTitleFontFamily;
			FontAttributes attr = _Cell.CellBase.TitleFontAttributes ?? _Cell.CellParent?.CellTitleFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}

		// protected internal  override bool UpdateTitleAlignment() { _TextAlignment = _CellBase.TitleTextAlignment; }

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
			if ( e.PropertyName == Shared.SettingsView.CellTitleColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellTitleFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellTitleFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellTitleFontAttributesProperty.PropertyName ) { return UpdateFont(); }

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