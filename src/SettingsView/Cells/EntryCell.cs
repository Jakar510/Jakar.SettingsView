using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Entry cell.
	/// </summary>
	public class EntryCell : CellBase, IEntryCellController
	{
		public static readonly BindableProperty ValueTextProperty = BindableProperty.Create(nameof(ValueText), typeof(string), typeof(EntryCell), default(string), defaultBindingMode: BindingMode.TwoWay, propertyChanging: ValueTextPropertyChanging);
		public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(EntryCell), -1, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty ValueTextColorProperty = BindableProperty.Create(nameof(ValueTextColor), typeof(Color), typeof(EntryCell), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty ValueTextFontSizeProperty = BindableProperty.Create(nameof(ValueTextFontSize), typeof(double), typeof(EntryCell), -1.0d, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty ValueTextFontFamilyProperty = BindableProperty.Create(nameof(ValueTextFontFamily), typeof(string), typeof(EntryCell), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty ValueTextFontAttributesProperty = BindableProperty.Create(nameof(ValueTextFontAttributes), typeof(FontAttributes?), typeof(EntryCell), null, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryCell), Keyboard.Default, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty CompletedCommandProperty = BindableProperty.Create(nameof(CompletedCommand), typeof(ICommand), typeof(EntryCell), default(ICommand), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryCell), default(string), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(EntryCell), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create(nameof(TextAlignment), typeof(TextAlignment), typeof(EntryCell), TextAlignment.End, defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(EntryCell), default(Color), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryCell), default(bool), defaultBindingMode: BindingMode.OneWay);
		public static readonly BindableProperty OnSelectActionProperty = BindableProperty.Create(nameof(IsPassword), typeof(SelectAction), typeof(EntryCell), default(SelectAction), defaultBindingMode: BindingMode.OneWay);



		private static void ValueTextPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			var maxlength = (int) bindable.GetValue(MaxLengthProperty);

			if ( maxlength < 0 ) return;

			string newString = newValue?.ToString() ?? string.Empty;

			if ( newString.Length <= maxlength ) return;
			string oldString = oldValue?.ToString() ?? string.Empty;
			if ( oldString.Length > maxlength )
			{
				string trimStr = oldString.Substring(0, maxlength);
				bindable.SetValue(ValueTextProperty, trimStr);
			}
			else { bindable.SetValue(ValueTextProperty, oldString); }
		}

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


		public Color ValueTextColor
		{
			get => (Color) GetValue(ValueTextColorProperty);
			set => SetValue(ValueTextColorProperty, value);
		}


		
		[TypeConverter(typeof(FontSizeConverter))]
		public double ValueTextFontSize
		{
			get => (double) GetValue(ValueTextFontSizeProperty);
			set => SetValue(ValueTextFontSizeProperty, value);
		}


		public string ValueTextFontFamily
		{
			get => (string) GetValue(ValueTextFontFamilyProperty);
			set => SetValue(ValueTextFontFamilyProperty, value);
		}


		public FontAttributes? ValueTextFontAttributes
		{
			get => (FontAttributes?) GetValue(ValueTextFontAttributesProperty);
			set => SetValue(ValueTextFontAttributesProperty, value);
		}


		public Keyboard Keyboard
		{
			get => (Keyboard) GetValue(KeyboardProperty);
			set => SetValue(KeyboardProperty, value);
		}



		public event EventHandler Completed;
		public void SendCompleted()
		{
			Completed?.Invoke(this, EventArgs.Empty);
			if ( CompletedCommand != null )
			{
				if ( CompletedCommand.CanExecute(null) ) { CompletedCommand.Execute(null); }
			}
		}


		public ICommand CompletedCommand
		{
			get => (ICommand) GetValue(CompletedCommandProperty);
			set => SetValue(CompletedCommandProperty, value);
		}


		public string Placeholder
		{
			get => (string) GetValue(PlaceholderProperty);
			set => SetValue(PlaceholderProperty, value);
		}


		public Color PlaceholderColor
		{
			get => (Color) GetValue(PlaceholderColorProperty);
			set => SetValue(PlaceholderColorProperty, value);
		}


		public TextAlignment TextAlignment
		{
			get => (TextAlignment) GetValue(TextAlignmentProperty);
			set => SetValue(TextAlignmentProperty, value);
		}

		
		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}

		
		public bool IsPassword
		{
			get => (bool) GetValue(IsPasswordProperty);
			set => SetValue(IsPasswordProperty, value);
		}

		
		public SelectAction OnSelectAction
		{
			get => (SelectAction) GetValue(OnSelectActionProperty);
			set => SetValue(OnSelectActionProperty, value);
		}




		internal event EventHandler Focused;
		internal void SendFocus() { Focused?.Invoke(this, EventArgs.Empty); }
		public void SetFocus() => SendFocus();


		public enum SelectAction
		{
			None,
			Start,
			End,
			All
		}
	}
}