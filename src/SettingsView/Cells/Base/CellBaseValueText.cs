// unset

using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBaseValueText : CellBaseValue
	{
		public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(CellBaseValueText), -1);
		public static readonly BindableProperty ValueTextProperty = BindableProperty.Create(nameof(ValueText), typeof(string), typeof(CellBaseValueText), default(string), BindingMode.TwoWay, propertyChanging: ValueTextPropertyChanging);
		public static readonly BindableProperty ValueTextAlignmentProperty = BindableProperty.Create(nameof(ValueTextAlignment), typeof(TextAlignment?), typeof(CellBaseValueText));
		
		public string ValueText
		{
			get => (string) GetValue(ValueTextProperty);
			set => SetValue(ValueTextProperty, value);
		}
		public int MaxLength
		{
			get => (int) GetValue(MaxLengthProperty);
			set => SetValue(MaxLengthProperty, value);
		}
		public TextAlignment? ValueTextAlignment
		{
			get => (TextAlignment?) GetValue(ValueTextAlignmentProperty);
			set => SetValue(ValueTextAlignmentProperty, value);
		}

		private static void ValueTextPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			var maxlength = (int) bindable.GetValue(MaxLengthProperty);

			if ( maxlength < 0 )
				return;

			string newString = newValue?.ToString() ?? string.Empty;

			if ( newString.Length <= maxlength )
				return;
			string oldString = oldValue?.ToString() ?? string.Empty;
			if ( oldString.Length > maxlength )
			{
				string trimStr = oldString.Substring(0, maxlength);
				bindable.SetValue(ValueTextProperty, trimStr);
			}
			else
			{ bindable.SetValue(ValueTextProperty, oldString); }
		}
	}
}