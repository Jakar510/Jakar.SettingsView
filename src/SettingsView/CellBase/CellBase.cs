using System;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	public class CellBase : Cell
	{
		public new event EventHandler? Tapped;

		internal new void OnTapped() { Tapped?.Invoke(this, EventArgs.Empty); }


		public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible),
																				   typeof(bool),
																				   typeof(CellBase),
																				   true,
																				   defaultBindingMode: BindingMode.OneWay
																				  );
		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CellBase), Color.Default);


		public bool IsVisible
		{
			get => (bool) GetValue(IsVisibleProperty);
			set => SetValue(IsVisibleProperty, value);
		}

		public Color BackgroundColor
		{
			get => (Color) GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		public Section? Section { get; set; }

		public new sv.SettingsView Parent
		{
			get => (sv.SettingsView) base.Parent;
			set => base.Parent = value;
		}

		public virtual void Reload()
		{
			if ( Section is null ) { return; }

			int index = Section.IndexOf(this);
			if ( index < 0 ) { return; }

			// raise replace event manually.
			Cell temp = Section[index];
			Section[index] = temp;
		}
	}
}