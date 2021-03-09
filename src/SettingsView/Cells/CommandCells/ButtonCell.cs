using System;
using System.ComponentModel;
using System.Windows.Input;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class ButtonCell : TitleCellBase // IBorderElement
	{
		public static readonly BindableProperty ButtonBackgroundColorProperty = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(TitleCellBase), SVConstants.Cell.COLOR);
		public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonCell), default(ICommand));
		public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonCell));
		public static BindableProperty LongClickCommandProperty = BindableProperty.Create(nameof(LongClickCommand), typeof(ICommand), typeof(ButtonCell), default(ICommand));
		public static BindableProperty LongClickCommandParameterProperty = BindableProperty.Create(nameof(LongClickCommandParameter), typeof(object), typeof(ButtonCell));


		// public static readonly BindableProperty BackgroundBrushProperty = BindableProperty.Create(nameof(BackgroundBrush), typeof(Brush), typeof(ButtonCell), SVConstants.Cell.Brush);
		// public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(ButtonCell), SVConstants.Defaults.BorderWidth);
		// public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(ButtonCell), SVConstants.Defaults.CornerRadius);
		//
		// public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor),
		// 																					  typeof(Color),
		// 																					  typeof(ButtonCell),
		// 																					  SVConstants.Cell.COLOR,
		// 																					  propertyChanged: OnBorderColorPropertyChanged
		// 																					 );

		// private static void OnBorderColorPropertyChanged( BindableObject bindable, object oldValue, object newValue ) { ( (IBorderElement) bindable ).OnBorderColorPropertyChanged((Color) oldValue, (Color) newValue); }


		public Color ButtonBackgroundColor
		{
			get => (Color) GetValue(ButtonBackgroundColorProperty);
			set => SetValue(ButtonBackgroundColorProperty, value);
		}

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

		public ICommand LongClickCommand
		{
			get => (ICommand) GetValue(LongClickCommandProperty);
			set => SetValue(LongClickCommandProperty, value);
		}

		public object LongClickCommandParameter
		{
			get => GetValue(LongClickCommandParameterProperty);
			set => SetValue(LongClickCommandParameterProperty, value);
		}

		internal Color GetButtonColor() =>
			ButtonBackgroundColor == SVConstants.Cell.COLOR
				? Parent.CellButtonBackgroundColor
				: ButtonBackgroundColor;


		// public Color BorderColor
		// {
		// 	get => (Color) GetValue(BorderColorProperty);
		// 	set => SetValue(BorderColorProperty, value);
		// }
		//
		// public int CornerRadius
		// {
		// 	get => (int) GetValue(CornerRadiusProperty);
		// 	set => SetValue(CornerRadiusProperty, value);
		// }
		//
		// public double BorderWidth
		// {
		// 	get => (double) GetValue(BorderWidthProperty);
		// 	set => SetValue(BorderWidthProperty, value);
		// }
		//
		// public Brush BackgroundBrush
		// {
		// 	get => (Brush) GetValue(BackgroundBrushProperty);
		// 	set => SetValue(BackgroundBrushProperty, value);
		// }
		//
		// void IBorderElement.OnBorderColorPropertyChanged( Color oldValue, Color newValue ) {  }
		// bool IBorderElement.IsCornerRadiusSet() => !CornerRadius.Equals(SVConstants.Defaults.CornerRadius);
		// bool IBorderElement.IsBackgroundColorSet() => ButtonBackgroundColor != SVConstants.Cell.COLOR;
		// bool IBorderElement.IsBackgroundSet() => BackgroundColor != SVConstants.Cell.COLOR;
		// bool IBorderElement.IsBorderColorSet() => BorderColor != SVConstants.Cell.COLOR;
		// bool IBorderElement.IsBorderWidthSet() => !BorderWidth.Equals(SVConstants.Defaults.BorderWidth);
		//
		// Color IBorderElement.BorderColor => BorderColor;
		// int IBorderElement.CornerRadius => CornerRadius;
		// Brush IBorderElement.Background => BackgroundBrush;
		// double IBorderElement.BorderWidth => BorderWidth;
		// int IBorderElement.CornerRadiusDefaultValue => (int) CornerRadiusProperty.DefaultValue;
		// Color IBorderElement.BorderColorDefaultValue => (Color) BorderColorProperty.DefaultValue;
		// double IBorderElement.BorderWidthDefaultValue => (double) BorderWidthProperty.DefaultValue;
	}
}