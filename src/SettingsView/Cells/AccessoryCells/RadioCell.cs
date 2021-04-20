using System;
using Jakar.SettingsView.Shared.CellBase;
using Xamarin.Forms;


namespace Jakar.SettingsView.Shared.Cells
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class RadioCell : CheckableCellBase<object>
	{
		public static readonly BindableProperty SelectedValueProperty = BindableProperty.CreateAttached("SelectedValue",
																										typeof(object),
																										typeof(RadioCell),
																										default,
																										BindingMode.TwoWay
																									   );

		public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(RadioCell));

		public object? Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}
	}



	public static class RadioCellExtensions
	{
		public static void SetSelectedValue( this    BindableObject view, object? value ) { view.SetValue(RadioCell.SelectedValueProperty, value); }
		public static object? GetSelectedValue( this BindableObject view ) => view.GetValue(RadioCell.SelectedValueProperty);
	}
}
