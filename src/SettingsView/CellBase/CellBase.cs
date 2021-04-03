using System;
using System.ComponentModel;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class CellBase : Cell, IVisibleCell
	{
		public new event EventHandler? Tapped;

		internal new void OnTapped() { Tapped?.Invoke(this, EventArgs.Empty); }


		public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible),
																				   typeof(bool),
																				   typeof(CellBase),
																				   SvConstants.Cell.VISIBLE,
																				   defaultBindingMode: BindingMode.OneWay
																				  );

		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(CellBase), SvConstants.Cell.color);


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
			get => base.Parent as sv.SettingsView ?? throw new NullReferenceException(nameof(Parent));
			set => base.Parent = value;
		}

		internal Color GetBackground() =>
			BackgroundColor == SvConstants.Cell.color
				? Parent.CellBackgroundColor
				: BackgroundColor;

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