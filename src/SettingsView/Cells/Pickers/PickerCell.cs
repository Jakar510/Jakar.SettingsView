using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	public class PickerCell : BasePopupCell
	{
		public static readonly BindableProperty UseNaturalSortProperty = BindableProperty.Create(nameof(UseNaturalSort), typeof(bool), typeof(PickerCell), false);
		public static readonly BindableProperty UsePickToCloseProperty = BindableProperty.Create(nameof(UsePickToClose), typeof(bool), typeof(PickerCell), default(bool));
		public static readonly BindableProperty UseAutoValueTextProperty = BindableProperty.Create(nameof(UseAutoValueText), typeof(bool), typeof(PickerCell), true);
		public static readonly BindableProperty KeepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool), typeof(PickerCell), true);
		public static readonly BindableProperty SelectedItemsOrderKeyProperty = BindableProperty.Create(nameof(SelectedItemsOrderKey), typeof(string), typeof(PickerCell), default(string));
		public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(PickerCell), default(ICommand));
		public static readonly BindableProperty DisplayMemberProperty = BindableProperty.Create(nameof(DisplayMember), typeof(string), typeof(PickerCell), default(string));
		public static readonly BindableProperty SubDisplayMemberProperty = BindableProperty.Create(nameof(SubDisplayMember), typeof(string), typeof(PickerCell), default(string));


		public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems),
																								typeof(IList),
																								typeof(PickerCell),
																								default(IList),
																								BindingMode.TwoWay
																							   );

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
																							   typeof(object),
																							   typeof(PickerCell),
																							   default,
																							   BindingMode.TwoWay
																							  );

		public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode),
																								typeof(SelectionMode),
																								typeof(PickerCell),
																								SelectionMode.Multiple,
																								BindingMode.OneWay,
																								propertyChanged: ( bindable, old_value, new_value ) =>
																												 {
																													 if ( (SelectionMode) new_value == SelectionMode.Single ) { bindable.SetValue(MaxSelectedNumberProperty, 1); }
																												 }
																							   );

		public static readonly BindableProperty MaxSelectedNumberProperty = BindableProperty.Create(nameof(MaxSelectedNumber),
																									typeof(int),
																									typeof(PickerCell),
																									0,
																									BindingMode.OneWay,
																									coerceValue: ( bindable, value ) => (SelectionMode) bindable.GetValue(SelectionModeProperty) == SelectionMode.Single ? 1 : value
																								   );
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																							  typeof(IList),
																							  typeof(PickerCell),
																							  default(IList),
																							  BindingMode.OneWay,
																							  propertyChanging: ItemsSourceChanging
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

		public SelectionMode SelectionMode
		{
			get => (SelectionMode) GetValue(SelectionModeProperty);
			set => SetValue(SelectionModeProperty, value);
		}

		public int MaxSelectedNumber
		{
			get => (int) GetValue(MaxSelectedNumberProperty);
			set => SetValue(MaxSelectedNumberProperty, value);
		}

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

		public bool UseAutoValueText
		{
			get => (bool) GetValue(UseAutoValueTextProperty);
			set => SetValue(UseAutoValueTextProperty, value);
		}

		public bool UsePickToClose
		{
			get => (bool) GetValue(UsePickToCloseProperty);
			set => SetValue(UsePickToCloseProperty, value);
		}

		internal IList MergedSelectedList
		{
			get
			{
				if ( SelectionMode == SelectionMode.Single )
				{
					var list = new ArrayList();
					if ( SelectedItem != null ) { list.Add(SelectedItem); }

					return list;
				}

				return SelectedItems;
			}
		}

		//getters cache
		private static readonly ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>> DisplayValueCache = new ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>>();

		//DisplayMember getter
		internal Func<object, object> DisplayValue
		{
			get
			{
				if ( _getters == null ||
					 DisplayMember == null ) { return ( obj ) => obj; }

				if ( _getters.ContainsKey(DisplayMember) ) { return _getters[DisplayMember]; }

				return ( obj ) => obj;
			}
		}

		internal Func<object, object> SubDisplayValue
		{
			get
			{
				if ( _getters == null ||
					 SubDisplayMember == null ) { return ( obj ) => null; }

				if ( _getters.ContainsKey(SubDisplayMember) ) { return _getters[SubDisplayMember]; }
				else { return ( obj ) => null; }
			}
		}

		//OrderKey getter
		internal Func<object, object> KeyValue
		{
			get
			{
				if ( _getters == null ||
					 SelectedItemsOrderKey == null ) { return null; }

				if ( _getters.ContainsKey(SelectedItemsOrderKey) ) { return _getters[SelectedItemsOrderKey]; }

				return null;
			}
		}

		internal string GetSelectedItemsText()
		{
			List<string> sortedList = null;

			if ( MergedSelectedList == null ) { return string.Empty; }


			if ( KeyValue != null )
			{
				var dict = new Dictionary<object, string>();
				foreach ( object item in MergedSelectedList ) { dict.Add(KeyValue(item), DisplayValue(item).ToString()); }

				if ( UseNaturalSort ) { sortedList = dict.OrderBy(x => x.Key.ToString(), new NaturalComparer()).Select(x => x.Value).ToList(); }
				else { sortedList = dict.OrderBy(x => x.Key).Select(x => x.Value).ToList(); }
			}
			else
			{
				var strList = new List<string>();
				foreach ( object item in MergedSelectedList ) { strList.Add(DisplayValue(item).ToString()); }

				NaturalComparer comparer = UseNaturalSort ? new NaturalComparer() : null;
				sortedList = strList.OrderBy(x => x, comparer).ToList();
			}

			return string.Join(", ", sortedList.ToArray());
		}

		internal void InvokeCommand() { SelectedCommand?.Execute(SelectionMode == SelectionMode.Single ? SelectedItem : SelectedItems); }

		private Dictionary<string, Func<object, object>> _getters;

		private static void ItemsSourceChanging( BindableObject bindable, object oldValue, object newValue )
		{
			var control = bindable as PickerCell;
			if ( newValue == null ) { return; }

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
			switch ( type.FullName )
			{
				case "System.Boolean":
				case "System.Byte":
				case "System.SByte":
				case "System.Char":
				case "System.Int16":
				case "System.UInt16":
				case "System.Int32":
				case "System.UInt32":
				case "System.Int64":
				case "System.UInt64":
				case "System.Single":
				case "System.Double":
				case "System.Decimal":
				case "System.String":
				case "System.Object":
					return true;

				default:
					return false;
			}
		}
	}
}