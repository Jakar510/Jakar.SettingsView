using System;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class DatePickerCell : PromptCellBase<DateTime>
	{
		public static BindableProperty DateProperty = BindableProperty.Create(nameof(Date),
																			  typeof(DateTime),
																			  typeof(DatePickerCell),
																			  default(DateTime),
																			  BindingMode.TwoWay
																			 );
		public static BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(DatePickerCell), new DateTime(2100, 12, 31));
		public static BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(DatePickerCell), new DateTime(1900, 1, 1));
		public static BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(DatePickerCell), "d");
		public static BindableProperty TodayTextProperty = BindableProperty.Create(nameof(TodayText), typeof(string), typeof(DatePickerCell), default(string));

		public DateTime Date
		{
			get => (DateTime) GetValue(DateProperty);
			set => SetValue(DateProperty, value);
		}

		public DateTime MaximumDate
		{
			get => (DateTime) GetValue(MaximumDateProperty);
			set => SetValue(MaximumDateProperty, value);
		}

		public DateTime MinimumDate
		{
			get => (DateTime) GetValue(MinimumDateProperty);
			set => SetValue(MinimumDateProperty, value);
		}

		public string Format
		{
			get => (string) GetValue(FormatProperty);
			set => SetValue(FormatProperty, value);
		}

		public string TodayText
		{
			get => (string) GetValue(TodayTextProperty);
			set => SetValue(TodayTextProperty, value);
		}
	}
}