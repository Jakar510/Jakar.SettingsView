using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Cells
{
	public class TextPickerCell : BasePopupCell
	{
		public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(TextPickerCell), new List<string>());
		public static BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(TextPickerCell), default(ICommand));
		public static BindableProperty PageTitleProperty = BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(PickerCell), default(string));
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(PickerCell), Color.Default);
		public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(TextPickerCell), default, BindingMode.TwoWay);
		public static BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(TextPickerCell), default(string));
		public static BindableProperty PopupCancelTextProperty = BindableProperty.Create(nameof(PopupCancelText), typeof(string), typeof(TextPickerCell), "Cancel");
		public static BindableProperty PopupAcceptTextProperty = BindableProperty.Create(nameof(PopupAcceptText), typeof(string), typeof(TextPickerCell), "Ok");
		public static BindableProperty IsCircularPickerProperty = BindableProperty.Create(nameof(IsCircularPicker), typeof(bool), typeof(TextPickerCell), true);

		public IList<string> Items 
		{
			get => (IList<string>) GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		public string PageTitle
		{
			get => (string) GetValue(PageTitleProperty);
			set => SetValue(PageTitleProperty, value);
		}

		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}

		public string SelectedItem
		{
			get => (string) GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public string PickerTitle
		{
			get => (string) GetValue(PickerTitleProperty);
			set => SetValue(PickerTitleProperty, value);
		}

		public string PopupCancelText
		{
			get => (string) GetValue(PopupCancelTextProperty);
			set => SetValue(PopupCancelTextProperty, value);
		}

		public string PopupAcceptText
		{
			get => (string) GetValue(PopupAcceptTextProperty);
			set => SetValue(PopupAcceptTextProperty, value);
		}

		public ICommand SelectedCommand
		{
			get => (ICommand) GetValue(SelectedCommandProperty);
			set => SetValue(SelectedCommandProperty, value);
		}

		public bool IsCircularPicker
		{
			get => (bool) GetValue(IsCircularPickerProperty);
			set => SetValue(IsCircularPickerProperty, value);
		}
	}
}