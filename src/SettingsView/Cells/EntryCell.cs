using System;
using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.Cells
{
	public class EntryCell : CellBaseValueText, IEntryCellController
	{
		public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryCell), Keyboard.Default);
		public static readonly BindableProperty CompletedCommandProperty = BindableProperty.Create(nameof(CompletedCommand), typeof(ICommand), typeof(EntryCell), default(ICommand));
		public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryCell), default(string));
		public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(EntryCell), Color.Default);
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(EntryCell), Color.Default);
		public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryCell), default(bool));
		public static readonly BindableProperty OnSelectActionProperty = BindableProperty.Create(nameof(IsPassword), typeof(SelectAction), typeof(EntryCell), default(SelectAction));


		public Keyboard Keyboard
		{
			get => (Keyboard) GetValue(KeyboardProperty);
			set => SetValue(KeyboardProperty, value);
		}


		public event EventHandler? Completed;
		public void SendCompleted()
		{
			Completed?.Invoke(this, EventArgs.Empty);
			if ( CompletedCommand?.CanExecute(null) ?? false ) { CompletedCommand.Execute(null); }
		}


		public ICommand? CompletedCommand
		{
			get => (ICommand?) GetValue(CompletedCommandProperty);
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


		public event EventHandler<TextChangedEventArgs>? TextChanged;
		internal void SendTextChanged( string oldTextValue, string newTextValue ) => SendTextChanged(new TextChangedEventArgs(oldTextValue, newTextValue));
		internal void SendTextChanged( TextChangedEventArgs args) { TextChanged?.Invoke(this, args); }


		internal event EventHandler? Focused;
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