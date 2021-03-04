// unset

using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.CellBase
{
	public class CheckableCellBase : DescriptionCellBase
	{
		public static BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked), typeof(bool), typeof(CheckboxCell), default(bool), BindingMode.TwoWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CheckboxCell"/> is checked.
		/// </summary>
		/// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
		public bool Checked
		{
			get => (bool) GetValue(CheckedProperty);
			set => SetValue(CheckedProperty, value);
		}

		
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(CheckboxCell), sv.SettingsView.DEFAULT_ACCENT_COLOR);

		/// <summary>
		/// Gets or sets the color of the accent true value of the check.
		/// </summary>
		/// <value>The color of the accent.</value>
		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}
		
		
		public static BindableProperty OffColorProperty = BindableProperty.Create(nameof(OffColor), typeof(Color), typeof(CheckboxCell), sv.SettingsView.DEFAULT_OFF_COLOR);

		/// <summary>
		/// Gets or sets the color of the false value of the check
		/// </summary>
		/// <value>The color of the false value of the check.</value>
		public Color OffColor
		{
			get => (Color) GetValue(OffColorProperty);
			set => SetValue(OffColorProperty, value);
		}
	}
}