﻿using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Switch cell.
	/// </summary>
	public class SwitchCell : CellBase
	{
		/// <summary>
		/// The on property.
		/// </summary>
		public static BindableProperty OnProperty = BindableProperty.Create(nameof(On), typeof(bool), typeof(SwitchCell), default(bool), defaultBindingMode: BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.SwitchCell"/> is on.
		/// </summary>
		/// <value><c>true</c> if on; otherwise, <c>false</c>.</value>
		public bool On
		{
			get => (bool) GetValue(OnProperty);
			set => SetValue(OnProperty, value);
		}

		/// <summary>
		/// The accent color property.
		/// </summary>
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(SwitchCell), default(Color), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the color of the accent.
		/// </summary>
		/// <value>The color of the accent.</value>
		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}
	}
}