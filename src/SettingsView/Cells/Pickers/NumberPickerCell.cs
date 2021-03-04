using System.Windows.Input;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class NumberPickerCell : PopupCellBase
	{
		public static BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(NumberPickerCell), default(ICommand));
		// public static BindableProperty PopupTitleProperty = BindableProperty.Create(nameof(PopupTitle), typeof(string), typeof(NumberPickerCell), default(string));
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

		// public string PopupTitle
		// {
		// 	get => (string) GetValue(PopupTitleProperty);
		// 	set => SetValue(PopupTitleProperty, value);
		// }

		public ICommand SelectedCommand
		{
			get => (ICommand) GetValue(SelectedCommandProperty);
			set => SetValue(SelectedCommandProperty, value);
		}
	}
}