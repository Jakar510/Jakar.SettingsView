using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class LabelCell : CellBaseValueText
	{
		/// <summary>
		/// The ignore use description as value property.
		/// </summary>
		public static BindableProperty IgnoreUseDescriptionAsValueProperty = BindableProperty.Create(nameof(IgnoreUseDescriptionAsValue), typeof(bool), typeof(LabelCell), false);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.LabelCell"/> ignore use
		/// description as value.
		/// </summary>
		/// <value><c>true</c> if ignore use description as value; otherwise, <c>false</c>.</value>
		public bool IgnoreUseDescriptionAsValue
		{
			get => (bool) GetValue(IgnoreUseDescriptionAsValueProperty);
			set => SetValue(IgnoreUseDescriptionAsValueProperty, value);
		}
	}
}