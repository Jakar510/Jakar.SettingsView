using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;


namespace Jakar.SettingsView.Sample.Shared.Views.Cells
{
	public partial class SliderCell : CustomCell
	{
		public SliderCell() { InitializeComponent(); }

		public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(SliderCell), default(double), defaultBindingMode: BindingMode.TwoWay);

		public double Value
		{
			get => (double) GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}

		public static BindableProperty MinProperty = BindableProperty.Create(nameof(Min), typeof(double), typeof(SliderCell), default(double), defaultBindingMode: BindingMode.OneWay);

		public double Min
		{
			get => (double) GetValue(MinProperty);
			set => SetValue(MinProperty, value);
		}

		public static BindableProperty MaxProperty = BindableProperty.Create(nameof(Max), typeof(double), typeof(SliderCell), 1.0d, defaultBindingMode: BindingMode.OneWay);

		public double Max
		{
			get => (double) GetValue(MaxProperty);
			set => SetValue(MaxProperty, value);
		}

		public static BindableProperty ChangedCommandProperty = BindableProperty.Create(nameof(ChangedCommand), typeof(ICommand), typeof(SliderCell), default(ICommand), defaultBindingMode: BindingMode.OneWay);

		public ICommand ChangedCommand
		{
			get => (ICommand) GetValue(ChangedCommandProperty);
			set => SetValue(ChangedCommandProperty, value);
		}

		private void OnChanged( object sender, ValueChangedEventArgs e )
		{
			if ( ChangedCommand == null ) { return; }

			if ( ChangedCommand.CanExecute(null) ) { ChangedCommand.Execute(Value); }
		}
	}
}