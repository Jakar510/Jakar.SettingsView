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

		public static void SetSelectedValue( BindableObject    view, object? value ) { view.SetValue(SelectedValueProperty, value); }
		public static object? GetSelectedValue( BindableObject view ) => view.GetValue(SelectedValueProperty);

		public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(RadioCell));

		public object? Value
		{
			get => GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}
	}



	public static class RadioCellExtensions
	{
		public static void SetSelectedValue( this    BindableObject view, object? value ) { RadioCell.SetSelectedValue(view, value); }
		public static object? GetSelectedValue( this BindableObject view ) => RadioCell.GetSelectedValue(view);
	}
}
