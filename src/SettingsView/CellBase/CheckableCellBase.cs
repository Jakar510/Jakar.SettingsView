// unset

using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.CellBase
{
	public abstract class CheckableCellBase : DescriptionCellBase
	{
		public static BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color?), typeof(CheckboxCell), SVConstants.Cell.COLOR);
		public static BindableProperty OffColorProperty = BindableProperty.Create(nameof(OffColor), typeof(Color?), typeof(CheckboxCell), SVConstants.Cell.COLOR);

		public static BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked),
																				 typeof(bool),
																				 typeof(CheckboxCell),
																				 default(bool),
																				 BindingMode.TwoWay
																				);

		public bool Checked
		{
			get => (bool) GetValue(CheckedProperty);
			set => SetValue(CheckedProperty, value);
		}


		// [TypeConverter(typeof(NullableColorTypeConverter))]
		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}


		// [TypeConverter(typeof(NullableColorTypeConverter))]
		public Color OffColor
		{
			get => (Color) GetValue(OffColorProperty);
			set => SetValue(OffColorProperty, value);
		}


		internal Color GetAccentColor() =>
			AccentColor == SVConstants.Cell.COLOR
				? Parent.CellAccentColor
				: AccentColor;
		internal Color GetOffColor() =>
			OffColor == SVConstants.Cell.COLOR
				? Parent.CellOffColor
				: OffColor;
	}
}