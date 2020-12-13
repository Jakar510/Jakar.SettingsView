using System.ComponentModel;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface ICellDescription : IBaseCell
	{
		public TextView DescriptionLabel { get; set; }
		public void UpdateDescriptionText()
		{
			DescriptionLabel.Text = _CellBase.Description;
			DescriptionLabel.Visibility = string.IsNullOrEmpty(DescriptionLabel.Text) ? ViewStates.Gone : ViewStates.Visible;
		}
		public void UpdateDescriptionFontSize()
		{
			if ( _CellBase.DescriptionFontSize > 0 ) { DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float) _CellBase.DescriptionFontSize); }
			else if ( CellParent != null ) { DescriptionLabel.SetTextSize(ComplexUnitType.Sp, (float) CellParent.CellDescriptionFontSize); }
			else { DescriptionLabel.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }
		}
		public void UpdateDescriptionFont()
		{
			string family = _CellBase.DescriptionFontFamily ?? CellParent?.CellDescriptionFontFamily;
			FontAttributes attr = _CellBase.DescriptionFontAttributes ?? CellParent.CellDescriptionFontAttributes;

			DescriptionLabel.Typeface = FontUtility.CreateTypeface(family, attr);
		}
		public void UpdateDescriptionColor()
		{
			if ( _CellBase.DescriptionColor != Color.Default ) { DescriptionLabel.SetTextColor(_CellBase.DescriptionColor.ToAndroid()); }
			else if ( CellParent != null && CellParent.CellDescriptionColor != Color.Default ) { DescriptionLabel.SetTextColor(CellParent.CellDescriptionColor.ToAndroid()); }
			else { DescriptionLabel.SetTextColor(DefaultTextColor); }
		}


		public void UpdateDescription( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.DescriptionProperty.PropertyName )
			{ UpdateDescriptionText(); }
			else if ( e.PropertyName == CellBase.DescriptionFontSizeProperty.PropertyName )
			{ UpdateDescriptionFontSize(); }
			else if ( e.PropertyName == CellBase.DescriptionFontFamilyProperty.PropertyName || e.PropertyName == CellBase.DescriptionFontAttributesProperty.PropertyName )
			{ UpdateDescriptionFont(); }
			else if ( e.PropertyName == CellBase.DescriptionColorProperty.PropertyName )
			{ UpdateDescriptionColor(); }
		}
		public void UpdateDescription()
		{
			UpdateDescriptionText();
			UpdateDescriptionColor();
			UpdateDescriptionFontSize();
			UpdateDescriptionFont();
		}
		protected void Dispose( bool disposing )
		{
			DescriptionLabel?.Dispose();
			DescriptionLabel = null;
		}
	}
}