using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Cells
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class TextPickerCell : PromptCellBase<string>
	{
		public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IList<string>), typeof(TextPickerCell), new List<string>());
		public static BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(TextPickerCell), default(ICommand));
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(PickerCell), Color.Default);

		public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
																					  typeof(string),
																					  typeof(TextPickerCell),
																					  default,
																					  BindingMode.TwoWay
																					 );


		public IList<string>? Items
		{
			get => (IList<string>?) GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}


		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}

		public string? SelectedItem
		{
			get => (string?) GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public ICommand? SelectedCommand
		{
			get => (ICommand?) GetValue(SelectedCommandProperty);
			set => SetValue(SelectedCommandProperty, value);
		}
	}
}