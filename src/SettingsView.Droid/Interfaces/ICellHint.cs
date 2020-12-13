using System.ComponentModel;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface ICellHint : IBaseCell
	{
		public TextView HintLabel { get; set; }

		public void UpdateHintText()
		{
			string msg = _CellBase.HintText;
			if ( string.IsNullOrEmpty(msg) )
			{
				HintLabel.Visibility = ViewStates.Invisible;
				return;
			}

			HintLabel.Text = msg;
			HintLabel.Visibility = ViewStates.Visible;
		}
		public void UpdateHintTextColor()
		{
			if ( _CellBase.HintTextColor != Color.Default ) { HintLabel.SetTextColor(_CellBase.HintTextColor.ToAndroid()); }
			else if ( CellParent != null && CellParent.CellHintTextColor != Color.Default ) { HintLabel.SetTextColor(CellParent.CellHintTextColor.ToAndroid()); }
			else { HintLabel.SetTextColor(DefaultTextColor); }
		}
		public void UpdateHintFontSize()
		{
			if ( _CellBase.HintFontSize > 0 ) { HintLabel.SetTextSize(ComplexUnitType.Sp, (float) _CellBase.HintFontSize); }
			else if ( CellParent != null ) { HintLabel.SetTextSize(ComplexUnitType.Sp, (float) CellParent.CellHintFontSize); }
			else { HintLabel.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }
		}
		public void UpdateHintFont()
		{
			string family = _CellBase.HintFontFamily ?? CellParent?.CellHintFontFamily;
			if ( CellParent is null )
				return;
			FontAttributes attr = _CellBase.HintFontAttributes ?? CellParent.CellHintFontAttributes;

			HintLabel.Typeface = FontUtility.CreateTypeface(family, attr);
		}

		public void UpdateHint( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.HintTextProperty.PropertyName ) { UpdateWithForceLayout(UpdateHintText); }
			else if ( e.PropertyName == CellBase.HintTextColorProperty.PropertyName ) { UpdateHintTextColor(); }
		}
		public void UpdateHint()
		{
			UpdateHintText();
			UpdateHintTextColor();
			UpdateHintFontSize();
			UpdateHintFont();
		}
		protected void Dispose( bool disposing )
		{
			HintLabel?.Dispose();
			HintLabel = null;
		}
	}
}