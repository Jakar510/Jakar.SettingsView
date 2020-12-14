using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Text picker cell.
	/// </summary>
	public class TextPickerCell : LabelCell
	{
		public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList), typeof(TextPickerCell), new List<object>(), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(TextPickerCell), default(ICommand), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty PageTitleProperty = BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(PickerCell), default(string), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(PickerCell), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TextPickerCell), default(object), defaultBindingMode: BindingMode.TwoWay);
		public static BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(TextPickerCell), default(string), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty PopupCancelTextProperty = BindableProperty.Create(nameof(PopupCancelText), typeof(string), typeof(TextPickerCell), "Cancel", defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty PopupAcceptTextProperty = BindableProperty.Create(nameof(PopupAcceptText), typeof(string), typeof(TextPickerCell), "Ok", defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty IsCircularPickerProperty = BindableProperty.Create(nameof(IsCircularPicker), typeof(bool), typeof(TextPickerCell), true, defaultBindingMode: BindingMode.OneWay);

		public IList Items
		{
			get => (IList) GetValue(ItemsProperty);
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


		public object SelectedItem
		{
			get => (object) GetValue(SelectedItemProperty);
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


#pragma warning disable IDE1006 // Naming Styles
		// ReSharper disable once UnusedMember.Local
#pragma warning disable IDE0051 // Remove unused private members
		private new string ValueText { get; set; }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
	}
}