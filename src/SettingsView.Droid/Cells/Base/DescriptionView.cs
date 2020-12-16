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
	public class DescriptionView : BaseTextView
	{
		public DescriptionView( Context context ) : base(context) { }
		public DescriptionView( CellBaseView baseView, Context context ) : base(baseView, context) { }
		public DescriptionView( Context context, IAttributeSet attributes ) : base(context, attributes) { }


		protected internal override bool UpdateText()
		{
			Text = _Cell.CellBase.Description;
			Visibility = string.IsNullOrEmpty(Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _Cell.CellBase.DescriptionFontSize > 0 ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellBase.DescriptionFontSize); }
			else if ( _Cell.CellParent != null ) { SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellDescriptionFontSize); }
			else { SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _Cell.CellBase.DescriptionColor != Color.Default ) { SetTextColor(_Cell.CellBase.DescriptionColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellDescriptionColor != Color.Default ) { SetTextColor(_Cell.CellParent.CellDescriptionColor.ToAndroid()); }
			else { SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFont()
		{

			string? family = _Cell.CellBase.DescriptionFontFamily ?? _Cell.CellParent?.CellDescriptionFontFamily;
			FontAttributes attr = _Cell.CellBase.DescriptionFontAttributes ?? _Cell.CellParent?.CellDescriptionFontAttributes ?? FontAttributes.None;

			Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}

		// protected internal override bool UpdateAlignment() { _TextAlignment = _CellBase.DescriptionTextAlignment; }

		protected internal override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.DescriptionProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == CellBase.DescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == CellBase.DescriptionFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == CellBase.DescriptionColorProperty.PropertyName ) { return UpdateColor(); }

			return false;
		}
		protected internal override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellDescriptionColorProperty.PropertyName )
			{ return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontSizeProperty.PropertyName )
			{ return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellDescriptionFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellDescriptionFontAttributesProperty.PropertyName )
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