// unset

using System;
using System.Collections.Generic;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.Shared.CellBase
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class CheckableCellBase : DescriptionCellBase
	{
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color?), typeof(CheckboxCell), SvConstants.Cell.color);
		public static readonly BindableProperty OffColorProperty    = BindableProperty.Create(nameof(OffColor),    typeof(Color?), typeof(CheckboxCell), SvConstants.Cell.color);

		public static readonly BindableProperty CheckedProperty = BindableProperty.Create(nameof(Checked),
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


		private IUseCheckableConfiguration? _config;

		protected internal IUseCheckableConfiguration CheckableConfig
		{
			get
			{
				_config ??= new CheckableConfiguration(this);
				return _config;
			}
		}



		public class CheckableConfiguration : IUseCheckableConfiguration
		{
			private readonly CheckableCellBase _cell;
			public CheckableConfiguration( CheckableCellBase cell ) => _cell = cell;

			public Color AccentColor =>
				_cell.AccentColor == SvConstants.Cell.color
					? _cell.Parent.CellAccentColor
					: _cell.AccentColor;

			public Color OffColor =>
				_cell.OffColor == SvConstants.Cell.color
					? _cell.Parent.CellOffColor
					: _cell.OffColor;
		}
	}



	[Xamarin.Forms.Internals.Preserve(true, false)]
	public abstract class CheckableCellBase<TValue> : CheckableCellBase, IValueChanged<TValue>
	{
		public event EventHandler<SVValueChangedEventArgs<TValue>>? ValueChanged;
		void IValueChanged<TValue>.SendValueChanged( TValue              value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
		void IValueChanged<TValue>.SendValueChanged( IEnumerable<TValue> value ) => ValueChanged?.Invoke(this, new SVValueChangedEventArgs<TValue>(value));
		internal IValueChanged<TValue> ValueChangedHandler => this;
	}
}
