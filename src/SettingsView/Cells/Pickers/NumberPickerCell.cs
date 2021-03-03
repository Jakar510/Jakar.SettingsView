using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class NumberPickerCell : BasePopupCell
	{
		public static BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(NumberPickerCell), default(ICommand));
		// public static BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitle), typeof(string), typeof(NumberPickerCell), default(string));
		public static BindableProperty MaxProperty = BindableProperty.Create(nameof(Max), typeof(int), typeof(NumberPickerCell), 9999);
		public static BindableProperty MinProperty = BindableProperty.Create(nameof(Min), typeof(int), typeof(NumberPickerCell), 0);
		public static BindableProperty NumberProperty = BindableProperty.Create(nameof(Number), typeof(int), typeof(NumberPickerCell), default(int), BindingMode.TwoWay);

		public int Number
		{
			get => (int) GetValue(NumberProperty);
			set => SetValue(NumberProperty, value);
		}

		public int Min
		{
			get => (int) GetValue(MinProperty);
			set => SetValue(MinProperty, value);
		}

		public int Max
		{
			get => (int) GetValue(MaxProperty);
			set => SetValue(MaxProperty, value);
		}

		// public string PickerTitle
		// {
		// 	get => (string) GetValue(PickerTitleProperty);
		// 	set => SetValue(PickerTitleProperty, value);
		// }

		public ICommand SelectedCommand
		{
			get => (ICommand) GetValue(SelectedCommandProperty);
			set => SetValue(SelectedCommandProperty, value);
		}
	}
}