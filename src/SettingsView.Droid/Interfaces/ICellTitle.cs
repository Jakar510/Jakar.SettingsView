using System.ComponentModel;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface ICellTitle : IBaseCell
	{
		public TextView TitleLabel { get; set; }

		public void UpdateTitleText()
		{
			TitleLabel.Text = _CellBase.Title;
			//hide TextView right padding when TextView.Text empty.
			TitleLabel.Visibility = string.IsNullOrEmpty(TitleLabel.Text) ? ViewStates.Gone : ViewStates.Visible;
		}
		public void UpdateTitleColor()
		{
			if ( _CellBase.TitleColor != Color.Default ) { TitleLabel.SetTextColor(_CellBase.TitleColor.ToAndroid()); }
			else if ( CellParent != null && CellParent.CellTitleColor != Color.Default ) { TitleLabel.SetTextColor(CellParent.CellTitleColor.ToAndroid()); }
			else { TitleLabel.SetTextColor(DefaultTextColor); }
		}
		public void UpdateTitleFontSize()
		{
			if ( _CellBase.TitleFontSize > 0 ) { TitleLabel.SetTextSize(ComplexUnitType.Sp, (float) _CellBase.TitleFontSize); }
			else if ( CellParent != null ) { TitleLabel.SetTextSize(ComplexUnitType.Sp, (float) CellParent.CellTitleFontSize); }
			else { TitleLabel.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }
		}
		public void UpdateTitleFont()
		{
			if ( CellParent is null ) return;
			string family = _CellBase.TitleFontFamily ?? CellParent?.CellTitleFontFamily;
			FontAttributes attr = _CellBase.TitleFontAttributes ?? CellParent.CellTitleFontAttributes;

			TitleLabel.Typeface = FontUtility.CreateTypeface(family, attr);
		}


		public void UpdateTitle( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.TitleProperty.PropertyName )
			{ UpdateTitleText(); }
			else if ( e.PropertyName == CellBase.TitleColorProperty.PropertyName )
			{ UpdateTitleColor(); }
			else if ( e.PropertyName == CellBase.TitleFontSizeProperty.PropertyName )
			{ UpdateTitleFontSize(); }
			else if ( e.PropertyName == CellBase.TitleFontFamilyProperty.PropertyName || e.PropertyName == CellBase.TitleFontAttributesProperty.PropertyName )
			{ UpdateTitleFont(); }
		}
		public void UpdateTitle()
		{
			UpdateTitleText();
			UpdateTitleColor();
			UpdateTitleFontSize();
			UpdateTitleFont();
		}


		protected void Dispose( bool disposing )
		{
			TitleLabel?.Dispose();
			TitleLabel = null;
		}
	}
}