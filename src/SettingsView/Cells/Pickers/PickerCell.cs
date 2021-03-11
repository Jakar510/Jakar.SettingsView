using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Converters;
using Jakar.SettingsView.Shared.Enumerations;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class PickerCell : PromptCellBase<object>
	{
		public static readonly BindableProperty UseNaturalSortProperty = BindableProperty.Create(nameof(UseNaturalSort), typeof(bool), typeof(PickerCell), false);
		public static readonly BindableProperty UsePickToCloseProperty = BindableProperty.Create(nameof(UsePickToClose), typeof(bool), typeof(PickerCell), default(bool));
		public static readonly BindableProperty KeepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool), typeof(PickerCell), true);
		public static readonly BindableProperty SelectedItemsOrderKeyProperty = BindableProperty.Create(nameof(SelectedItemsOrderKey), typeof(string), typeof(PickerCell), default(string));
		public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(PickerCell), default(ICommand));
		public static readonly BindableProperty DisplayMemberProperty = BindableProperty.Create(nameof(DisplayMember), typeof(string), typeof(PickerCell), default(string));
		public static readonly BindableProperty SubDisplayMemberProperty = BindableProperty.Create(nameof(SubDisplayMember), typeof(string), typeof(PickerCell), default(string));


		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																							  typeof(IList),
																							  typeof(PickerCell),
																							  default(IList),
																							  BindingMode.OneWay,
																							  propertyChanging: ItemsSourceChanging
																							 );

		public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems),
																								typeof(IList),
																								typeof(PickerCell),
																								default(IList),
																								BindingMode.TwoWay
																								// propertyChanged: SelectedItemPropertyChanged
																							   );

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
																							   typeof(object),
																							   typeof(PickerCell),
																							   default,
																							   BindingMode.TwoWay
																							   // propertyChanged: SelectedItemPropertyChanged
																							  );

		// private static void SelectedItemPropertyChanged( BindableObject bindable, object oldValue, object newValue )
		// {
		// 	if ( bindable is PickerCell cell ) cell.InvokeSelectedEvent();
		// }


		public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode),
																								typeof(SelectMode),
																								typeof(PickerCell),
																								SelectMode.NotSet,
																								BindingMode.OneWay,
																								propertyChanged: ( bindable, old_value, new_value ) =>
																												 {
																													 if ( old_value == new_value ) return;
																													 int mode = new_value switch
																																{
																																	SelectMode.Unlimited => -1,
																																	SelectMode.Single => 1,
																																	_ => (int) bindable.GetValue(MaxSelectedNumberProperty)
																																};
																													 bindable.SetValue(MaxSelectedNumberProperty, mode);
																												 }
																							   );

		public static readonly BindableProperty MaxSelectedNumberProperty = BindableProperty.Create(nameof(MaxSelectedNumber),
																									typeof(int),
																									typeof(PickerCell),
																									-1,
																									BindingMode.OneWay,
																									coerceValue: ( bindable, value ) =>
																												 {
																													 return (SelectMode) bindable.GetValue(SelectionModeProperty) switch
																															{
																																SelectMode.Unlimited => -1,
																																SelectMode.Single => 1,
																																_ => value
																															};
																												 }
																								   );


		public IList ItemsSource
		{
			get => (IList) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public string DisplayMember
		{
			get => (string) GetValue(DisplayMemberProperty);
			set => SetValue(DisplayMemberProperty, value);
		}

		public string SubDisplayMember
		{
			get => (string) GetValue(SubDisplayMemberProperty);
			set => SetValue(SubDisplayMemberProperty, value);
		}

		public IList SelectedItems
		{
			get => (IList) GetValue(SelectedItemsProperty);
			set => SetValue(SelectedItemsProperty, value);
		}

		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public SelectMode SelectionMode
		{
			get => (SelectMode) GetValue(SelectionModeProperty);
			set => SetValue(SelectionModeProperty, value);
		}

		public int MaxSelectedNumber
		{
			get => (int) GetValue(MaxSelectedNumberProperty);
			set
			{
				SetValue(MaxSelectedNumberProperty, value);
				SelectionMode = value switch
								{
									< 0 => SelectMode.Unlimited,
									0 => SelectMode.NotSet,
									1 => SelectMode.Single,
									> 1 => SelectMode.Multiple,
								};
			}
		}

		internal IList MergedSelectedList
		{
			get
			{
				switch ( SelectionMode )
				{
					case SelectMode.Single:
						var list = new List<object>();
						if ( SelectedItem != null ) { list.Add(SelectedItem); }

						return list;

					case SelectMode.NotSet:
					case SelectMode.Multiple:
					case SelectMode.Unlimited:
						return SelectedItems;

					default: throw new ArgumentOutOfRangeException(nameof(SelectionMode));
				}
			}
		}

		internal bool IsUnLimited => SelectionMode == SelectMode.Unlimited;
		internal bool IsSingleMode => SelectionMode == SelectMode.Single;
		internal bool IsValidMode => SelectionMode != SelectMode.NotSet;
		internal void InvokeSelectedEvent() { SelectedEvent?.Invoke(this, new ItemChanged(MergedSelectedList, SelectionMode)); }
		public EventHandler<ItemChanged> SelectedEvent;

		public bool KeepSelectedUntilBack
		{
			get => (bool) GetValue(KeepSelectedUntilBackProperty);
			set => SetValue(KeepSelectedUntilBackProperty, value);
		}


		public string SelectedItemsOrderKey
		{
			get => (string) GetValue(SelectedItemsOrderKeyProperty);
			set => SetValue(SelectedItemsOrderKeyProperty, value);
		}

		public ICommand SelectedCommand
		{
			get => (ICommand) GetValue(SelectedCommandProperty);
			set => SetValue(SelectedCommandProperty, value);
		}

		public bool UseNaturalSort
		{
			get => (bool) GetValue(UseNaturalSortProperty);
			set => SetValue(UseNaturalSortProperty, value);
		}

		public bool UsePickToClose
		{
			get => (bool) GetValue(UsePickToCloseProperty);
			set => SetValue(UsePickToCloseProperty, value);
		}

		internal void InvokeCommand()
		{
			SelectedCommand?.Execute(SelectionMode == SelectMode.Single
										 ? SelectedItem
										 : SelectedItems
									);
		}

		//getters cache
		private static readonly ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>> DisplayValueCache = new();

		//Row.Title getter
		internal Func<object, object> DisplayValue
		{
			get
			{
				if ( _getters is null ||
					 DisplayMember is null ) { return ( obj ) => obj; }

				return _getters.ContainsKey(DisplayMember)
						   ? _getters[DisplayMember]
						   : ( obj ) => obj;
			}
		}

		//Row.Description getter
		internal Func<object, object> SubDisplayValue
		{
			get
			{
				if ( _getters is null ||
					 SubDisplayMember is null ) { return ( obj ) => null; }

				return _getters.ContainsKey(SubDisplayMember)
						   ? _getters[SubDisplayMember]
						   : ( obj ) => null;
			}
		}

		//OrderKey getter
		internal Func<object, object> KeyValue
		{
			get
			{
				if ( _getters is null ||
					 SelectedItemsOrderKey is null ) { return null; }

				if ( _getters.ContainsKey(SelectedItemsOrderKey) ) { return _getters[SelectedItemsOrderKey]; }

				return null;
			}
		}
		private Dictionary<string, Func<object, object>> _getters;

		internal string GetSelectedItemsText()
		{
			IList? ITEMS = MergedSelectedList;
			if ( ITEMS is null ) return string.Empty;

			List<string> sortedList;
			if ( KeyValue is not null )
			{
				var dict = new Dictionary<object, string>();
				foreach ( object item in ITEMS )
				{
					object key = KeyValue(item);
					var value = DisplayValue(item).ToString();
					dict.Add(key, value);
				}

				sortedList = UseNaturalSort
								 ? dict.OrderBy(x => x.Key.ToString(), new NaturalComparer()).Select(x => x.Value).ToList()
								 : dict.OrderBy(x => x.Key).Select(x => x.Value).ToList();
			}
			else
			{
				List<string> strList = ( from object item in ITEMS select DisplayValue(item).ToString() ).ToList();

				NaturalComparer comparer = UseNaturalSort
											   ? new NaturalComparer()
											   : null;
				sortedList = strList.OrderBy(x => x, comparer).ToList();
			}

			var trace = new StackTrace();
			Console.WriteLine('\n');
			Console.WriteLine('\n');
			Console.Write(trace);
			Console.WriteLine('\n');
			Console.WriteLine('\n');
			return string.Join(", ", sortedList.ToArray());
		}


		internal static void ItemsSourceChanging( BindableObject bindable, object oldValue, object newValue )
		{
			var control = bindable as PickerCell;
			if ( newValue is null ) { return; }

			control?.SetUpPropertyCache(newValue as IList);
		}

		// Create all property getters
		private static Dictionary<string, Func<object, object>> CreateGetProperty( Type t )
		{
			IEnumerable<PropertyInfo> prop = t.GetRuntimeProperties().Where(x => x.DeclaringType == t && !x.Name.StartsWith("_", StringComparison.Ordinal));

			ParameterExpression target = Expression.Parameter(typeof(object), "target");

			var dictGetter = new Dictionary<string, Func<object, object>>();

			foreach ( PropertyInfo p in prop )
			{
				MemberExpression body = Expression.PropertyOrField(Expression.Convert(target, t), p.Name);

				Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), target);

				dictGetter[p.Name] = lambda.Compile();
			}

			return dictGetter;
		}

		private void SetUpPropertyCache( IEnumerable itemsSource )
		{
			Type[] typeArg = itemsSource.GetType().GenericTypeArguments;

			if ( !typeArg.Any() ) { throw new ArgumentException("ItemsSource must be GenericType."); }

			// If the type is a system built-in-type, it doesn't create GetProperty.
			if ( IsBuiltInType(typeArg[0]) )
			{
				_getters = null;

				return;
			}

			_getters = DisplayValueCache.GetOrAdd(typeArg[0], CreateGetProperty);
		}

		private static bool IsBuiltInType( Type type )
		{
			return type.FullName switch
				   {
					   "System.Boolean" => true,
					   "System.Byte" => true,
					   "System.SByte" => true,
					   "System.Char" => true,
					   "System.Int16" => true,
					   "System.UInt16" => true,
					   "System.Int32" => true,
					   "System.UInt32" => true,
					   "System.Int64" => true,
					   "System.UInt64" => true,
					   "System.Single" => true,
					   "System.Double" => true,
					   "System.Decimal" => true,
					   "System.String" => true,
					   "System.Object" => true,
					   _ => false
				   };
		}


		public class ItemChanged : EventArgs
		{
			public IEnumerable Items { get; }
			public SelectMode Mode { get; }

			public ItemChanged( IEnumerable items, SelectMode mode )
			{
				Items = items;
				Mode = mode;
			}
		}
	}
}