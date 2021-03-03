using System;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Time picker cell.
	/// </summary>
	public class TimePickerCell : BasePopupCell
	{
		public static BindableProperty TimeProperty = BindableProperty.Create(nameof(Time), typeof(TimeSpan), typeof(TimePickerCell), default(TimeSpan), BindingMode.TwoWay);
		public static BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(TimePickerCell), "t");
		public static BindableProperty PickerTitleProperty = BindableProperty.Create(nameof(PickerTitleProperty), typeof(string), typeof(TimePickerCell), default(string));

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

		public string PickerTitle
		{
			get => (string) GetValue(PickerTitleProperty);
			set => SetValue(PickerTitleProperty, value);
		}
	}
}