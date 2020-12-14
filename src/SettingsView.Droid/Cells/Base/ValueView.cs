using System.ComponentModel;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Cells.Base
{
	public class ValueView : BaseView
	{
		private LabelCell _LabelCell => _Cell.Cell as LabelCell;
		public ValueView( CellBaseView baseView, TextView view ) : base(baseView, view) { }


		protected internal override bool UpdateText()
		{
			Label.Text = _LabelCell.ValueText;
			Label.Visibility = string.IsNullOrEmpty(Label.Text) ? ViewStates.Gone : ViewStates.Visible;

			return true;
		}
		protected internal override bool UpdateFontSize()
		{
			if ( _LabelCell.ValueTextFontSize > 0 ) { Label.SetTextSize(ComplexUnitType.Sp, (float) _LabelCell.ValueTextFontSize); }
			else if ( _Cell.CellParent != null ) { Label.SetTextSize(ComplexUnitType.Sp, (float) _Cell.CellParent.CellValueTextFontSize); }
			else { Label.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }

			return true;
		}
		protected internal override bool UpdateColor()
		{
			if ( _LabelCell.ValueTextColor != Color.Default ) { Label.SetTextColor(_LabelCell.ValueTextColor.ToAndroid()); }
			else if ( _Cell.CellParent != null &&
					  _Cell.CellParent.CellValueTextColor != Color.Default ) { Label.SetTextColor(_Cell.CellParent.CellValueTextColor.ToAndroid()); }
			else { Label.SetTextColor(DefaultTextColor); }

			return true;
		}
		protected internal override bool UpdateFont()
		{
			string? family = _LabelCell.ValueTextFontFamily ?? _Cell.CellParent?.CellValueTextFontFamily;
			FontAttributes attr = _LabelCell.ValueTextFontAttributes ?? _Cell.CellParent?.CellValueTextFontAttributes ?? FontAttributes.None;

			Label.Typeface = FontUtility.CreateTypeface(family, attr);

			return true;
		}
		// protected void UpdateValueTextAlignment() { _Label.TextAlignment = _CellBase.ValueTextTextAlignment; }

		protected internal override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == LabelCell.ValueTextProperty.PropertyName ) { return UpdateText(); }

			if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == LabelCell.ValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == LabelCell.ValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }

			if ( e.PropertyName == LabelCell.ValueTextColorProperty.PropertyName ) { return UpdateColor(); }

			return false;
		}
		protected internal override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellValueTextColorProperty.PropertyName ) { return UpdateColor(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontSizeProperty.PropertyName ) { return UpdateFontSize(); }

			if ( e.PropertyName == Shared.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
				 e.PropertyName == Shared.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { return UpdateFont(); }
			
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