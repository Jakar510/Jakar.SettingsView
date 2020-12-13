using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface ICellSelectable : IBaseCell
	{
		public bool Selected { get; set; }
		public void UpdateBackgroundColor()
		{
			Selected = false;

			if ( _CellBase.BackgroundColor != Color.Default ) { BackgroundColor.Color = _CellBase.BackgroundColor.ToAndroid(); }
			else if ( CellParent != null && CellParent.CellBackgroundColor != Color.Default ) { BackgroundColor.Color = CellParent.CellBackgroundColor.ToAndroid(); }
			else { BackgroundColor.Color = Android.Graphics.Color.Transparent; }
		}
		public void UpdateSelectedColor()
		{
			if ( CellParent != null && CellParent.SelectedColor != Color.Default )
			{
				SelectedColor.Color = CellParent.SelectedColor.MultiplyAlpha(0.5).ToAndroid();
				Ripple.SetColor(DrawableUtility.GetPressedColorSelector(CellParent.SelectedColor.ToAndroid()));
			}
			else
			{
				SelectedColor.Color = Android.Graphics.Color.Argb(125, 180, 180, 180);
				Ripple.SetColor(DrawableUtility.GetPressedColorSelector(Android.Graphics.Color.Rgb(180, 180, 180)));
			}
		}


		public void UpdateIsEnabled() { SetEnabledAppearance(_CellBase.IsEnabled); }
		public void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { EnableCell(); }
			else { DisableCell(); }
		}

		public void EnableCell();
		public void DisableCell();


		public void UpdateSelected( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}
		public void UpdateSelected()
		{
			UpdateBackgroundColor();
			UpdateSelectedColor();

			UpdateIsEnabled();
		}
	}
}