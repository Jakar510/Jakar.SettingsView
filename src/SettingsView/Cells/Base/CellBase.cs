using System;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells.Base
{
	public class CellBase : Cell
	{
		public new event EventHandler Tapped;

		internal new void OnTapped() { Tapped?.Invoke(this, EventArgs.Empty); }

		
		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CellBase), default(Color), defaultBindingMode: BindingMode.OneWay);
		public Color BackgroundColor
		{
			get => (Color) GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		public Section Section { get; set; }

		public new SettingsView Parent
		{
			get => (SettingsView) base.Parent;
			set => base.Parent = value;
		}

		public virtual void Reload()
		{
			if ( Section == null ) { return; }

			int index = Section.IndexOf(this);
			if ( index < 0 ) { return; }

			// raise replace event manually.
			Cell temp = Section[index];
			Section[index] = temp;
		}
	}
}