﻿using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Command cell.
	/// </summary>
	public class CommandCell : CellBaseDescription
	{
		/// <summary>
		/// The command property.
		/// </summary>
		public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CommandCell), default(ICommand), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		/// <value>The command.</value>
		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		/// <summary>
		/// The command parameter property.
		/// </summary>
		public static BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CommandCell), default, defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the command parameter.
		/// </summary>
		/// <value>The command parameter.</value>
		public object CommandParameter
		{
			get => GetValue(CommandParameterProperty);
			set => SetValue(CommandParameterProperty, value);
		}

		/// <summary>
		/// The keep selected until back property.
		/// </summary>
		public static BindableProperty KeepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool), typeof(CommandCell), default(bool), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CommandCell"/> keep selected
		/// until back.
		/// </summary>
		/// <value><c>true</c> if keep selected until back; otherwise, <c>false</c>.</value>
		public bool KeepSelectedUntilBack
		{
			get => (bool) GetValue(KeepSelectedUntilBackProperty);
			set => SetValue(KeepSelectedUntilBackProperty, value);
		}

		public static BindableProperty HideArrowIndicatorProperty = BindableProperty.Create(nameof(HideArrowIndicator), typeof(bool), typeof(CommandCell), default(bool), defaultBindingMode: BindingMode.OneWay);

		public bool HideArrowIndicator
		{
			get => (bool) GetValue(HideArrowIndicatorProperty);
			set => SetValue(HideArrowIndicatorProperty, value);
		}
	}
}