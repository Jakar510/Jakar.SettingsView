using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Checkbox cell.
	/// </summary>
	public class CheckboxCell : CellBase
	{
		/// <summary>
		/// The checked property.
		/// </summary>
		public static BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked), typeof(bool), typeof(CheckboxCell), default(bool), defaultBindingMode: BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CheckboxCell"/> is checked.
		/// </summary>
		/// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
		public bool Checked
		{
			get => (bool) GetValue(CheckedProperty);
			set => SetValue(CheckedProperty, value);
		}

		/// <summary>
		/// The accent color property.
		/// </summary>
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(CheckboxCell), default(Color), defaultBindingMode: BindingMode.OneWay);

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