// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class BasePopupCell : CellBaseValue
	{
		public static BindableProperty PopupPageTitleProperty = BindableProperty.Create(nameof(PopupPageTitle), typeof(string), typeof(BasePopupCell), default(string));
		public static BindableProperty PopupTitleProperty = BindableProperty.Create(nameof(PopupTitle), typeof(string), typeof(BasePopupCell), default(string));
		public static BindableProperty PopupAcceptProperty = BindableProperty.Create(nameof(PopupAccept), typeof(string), typeof(BasePopupCell), "OK");
		public static BindableProperty PopupCancelProperty = BindableProperty.Create(nameof(PopupCancel), typeof(string), typeof(BasePopupCell), "Cancel");

		public static BindableProperty PopupSelectedColorProperty = BindableProperty.Create(nameof(PopupSelectedColor), typeof(Color), typeof(BasePopupCell), Color.Default);
		public static BindableProperty PopupBackGroundColorProperty = BindableProperty.Create(nameof(PopupBackGroundColor), typeof(Color), typeof(BasePopupCell), Color.White);
		public static BindableProperty PopupTextColorProperty = BindableProperty.Create(nameof(PopupTextColor), typeof(Color), typeof(BasePopupCell), Color.Black);
		public static readonly BindableProperty PopupAccentColorProperty = BindableProperty.Create(nameof(PopupAccentColor), typeof(Color), typeof(BasePopupCell), Color.Accent);

		public Color PopupSelectedColor
		{
			get => (Color) GetValue(PopupSelectedColorProperty);
			set => SetValue(PopupSelectedColorProperty, value);
		}

		public Color PopupTextColor
		{
			get => (Color) GetValue(PopupTextColorProperty);
			set => SetValue(PopupTextColorProperty, value);
		}

		public Color PopupBackGroundColor
		{
			get => (Color) GetValue(PopupBackGroundColorProperty);
			set => SetValue(PopupBackGroundColorProperty, value);
		}

		public Color PopupAccentColor
		{
			get => (Color) GetValue(PopupAccentColorProperty);
			set => SetValue(PopupAccentColorProperty, value);
		}

		public string PopupPageTitle
		{
			get => (string) GetValue(PopupPageTitleProperty);
			set => SetValue(PopupPageTitleProperty, value);
		}
		public string PopupTitle
		{
			get => (string) GetValue(PopupTitleProperty);
			set => SetValue(PopupTitleProperty, value);
		}

		public string PopupAccept
		{
			get => (string) GetValue(PopupAcceptProperty);
			set => SetValue(PopupAcceptProperty, value);
		}

		public string PopupCancel
		{
			get => (string) GetValue(PopupCancelProperty);
			set => SetValue(PopupCancelProperty, value);
		}
	}
}