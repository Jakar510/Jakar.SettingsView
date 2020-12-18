﻿using System;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class DatePickerCell : CellBaseValue
	{
		/// <summary>
		/// The date property.
		/// </summary>
		public static BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DatePickerCell), default(DateTime), defaultBindingMode: BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The date.</value>
		public DateTime Date
		{
			get => (DateTime) GetValue(DateProperty);
			set => SetValue(DateProperty, value);
		}

		/// <summary>
		/// The maximum date property.
		/// </summary>
		public static BindableProperty MaximumDateProperty = BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(DatePickerCell), new DateTime(2100, 12, 31), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the maximum date.
		/// </summary>
		/// <value>The maximum date.</value>
		public DateTime MaximumDate
		{
			get => (DateTime) GetValue(MaximumDateProperty);
			set => SetValue(MaximumDateProperty, value);
		}

		/// <summary>
		/// The minimum date property.
		/// </summary>
		public static BindableProperty MinimumDateProperty = BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(DatePickerCell), new DateTime(1900, 1, 1), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the minimum date.
		/// </summary>
		/// <value>The minimum date.</value>
		public DateTime MinimumDate
		{
			get => (DateTime) GetValue(MinimumDateProperty);
			set => SetValue(MinimumDateProperty, value);
		}

		/// <summary>
		/// The format property.
		/// </summary>
		public static BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(DatePickerCell), "d", defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the format.
		/// </summary>
		/// <value>The format.</value>
		public string Format
		{
			get => (string) GetValue(FormatProperty);
			set => SetValue(FormatProperty, value);
		}

		/// <summary>
		/// The today text property.
		/// </summary>
		public static BindableProperty TodayTextProperty = BindableProperty.Create(nameof(TodayText), typeof(string), typeof(DatePickerCell), default(string), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the today text.
		/// </summary>
		/// <value>The today text.</value>
		public string TodayText
		{
			get => (string) GetValue(TodayTextProperty);
			set => SetValue(TodayTextProperty, value);
		}

	}
}