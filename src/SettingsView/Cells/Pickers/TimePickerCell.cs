using System;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class TimePickerCell : PromptCellBase<TimeSpan>
	{
		public static readonly BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(TimePickerCell), default(TimeSpan), BindingMode.TwoWay);
		public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(TimePickerCell), "t");

		public TimeSpan Time
		{
			get => (TimeSpan) GetValue(TimeProperty);
			set => SetValue(TimeProperty, value);
		}

		public string Format
		{
			get => (string) GetValue(FormatProperty);
			set => SetValue(FormatProperty, value);
		}

	}
}