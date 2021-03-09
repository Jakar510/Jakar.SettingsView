using System.Windows.Input;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class CommandCell : DescriptionCellBase
	{
		public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CommandCell), default(ICommand));
		public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CommandCell));
		public static BindableProperty KeepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool), typeof(CommandCell), default(bool));
		public static BindableProperty HideArrowIndicatorProperty = BindableProperty.Create(nameof(HideArrowIndicator), typeof(bool), typeof(CommandCell), default(bool));

		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		public bool KeepSelectedUntilBack
		{
			get => (bool) GetValue(KeepSelectedUntilBackProperty);
			set => SetValue(KeepSelectedUntilBackProperty, value);
		}

		public bool HideArrowIndicator
		{
			get => (bool) GetValue(HideArrowIndicatorProperty);
			set => SetValue(HideArrowIndicatorProperty, value);
		}
	}
}