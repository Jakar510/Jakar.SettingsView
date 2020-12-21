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
	public class PickerCell : CellBaseValue
	{
		/// <summary>
		/// The page title property.
		/// </summary>
		public static readonly BindableProperty PageTitleProperty = BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(PickerCell), default(string));

		/// <summary>
		/// Gets or sets the page title.
		/// </summary>
		/// <value>The page title.</value>
		public string PageTitle
		{
			get => (string) GetValue(PageTitleProperty);
			set => SetValue(PageTitleProperty, value);
		}

		/// <summary>
		/// The items source property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																							  typeof(IList),
																							  typeof(PickerCell),
																							  default(IList),
																							  BindingMode.OneWay,
																							  propertyChanging: ItemsSourceChanging
																							 );

		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IList ItemsSource
		{
			get => (IList) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		/// <summary>
		/// The display member property.
		/// </summary>
		public static readonly BindableProperty DisplayMemberProperty = BindableProperty.Create(nameof(DisplayMember), typeof(string), typeof(PickerCell), default(string));

		/// <summary>
		/// Gets or sets the display member.
		/// </summary>
		/// <value>The display member.</value>
		public string DisplayMember
		{
			get => (string) GetValue(DisplayMemberProperty);
			set => SetValue(DisplayMemberProperty, value);
		}

		/// <summary>
		/// The sub display member property.
		/// </summary>
		public static readonly BindableProperty SubDisplayMemberProperty = BindableProperty.Create(nameof(SubDisplayMember), typeof(string), typeof(PickerCell), default(string));

		/// <summary>
		/// Gets or sets the sub display member.
		/// </summary>
		/// <value>The sub display member.</value>
		public string SubDisplayMember
		{
			get => (string) GetValue(SubDisplayMemberProperty);
			set => SetValue(SubDisplayMemberProperty, value);
		}

		/// <summary>
		/// The selected items property.
		/// </summary>
		public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems),
																								typeof(IList),
																								typeof(PickerCell),
																								default(IList),
																								BindingMode.TwoWay
																							   );

		/// <summary>
		/// Gets or sets the selected items.
		/// </summary>
		/// <value>The selected items.</value>
		public IList SelectedItems
		{
			get => (IList) GetValue(SelectedItemsProperty);
			set => SetValue(SelectedItemsProperty, value);
		}

		/// <summary>
		/// The selected item property.
		/// </summary>
		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
																							   typeof(object),
																							   typeof(PickerCell),
																							   default,
																							   BindingMode.TwoWay
																							  );

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		/// <value>The selected item.</value>
		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		/// <summary>
		/// The selection mode property.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the selection mode.
		/// </summary>
		/// <value>The selection mode.</value>
		public SelectionMode SelectionMode
		{
			get => (SelectionMode) GetValue(SelectionModeProperty);
			set => SetValue(SelectionModeProperty, value);
		}

		/// <summary>
		/// The max selected number property.
		/// </summary>
		public static readonly BindableProperty MaxSelectedNumberProperty = BindableProperty.Create(nameof(MaxSelectedNumber),
																									typeof(int),
																									typeof(PickerCell),
																									0,
																									BindingMode.OneWay,
																									coerceValue: ( bindable, value ) => (SelectionMode) bindable.GetValue(SelectionModeProperty) == SelectionMode.Single ? 1 : value
																								   );

		/// <summary>
		/// Gets or sets the max selected number.
		/// </summary>
		/// <value>The max selected number.</value>
		public int MaxSelectedNumber
		{
			get => (int) GetValue(MaxSelectedNumberProperty);
			set => SetValue(MaxSelectedNumberProperty, value);
		}

		/// <summary>
		/// The keep selected until back property.
		/// </summary>
		public static readonly BindableProperty KeepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool), typeof(PickerCell), true);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.PickerCell"/> keep selected
		/// until back.
		/// </summary>
		/// <value><c>true</c> if keep selected until back; otherwise, <c>false</c>.</value>
		public bool KeepSelectedUntilBack
		{
			get => (bool) GetValue(KeepSelectedUntilBackProperty);
			set => SetValue(KeepSelectedUntilBackProperty, value);
		}

		/// <summary>
		/// The accent color property.
		/// </summary>
		public static readonly BindableProperty AccentColorProperty = BindableProperty.Create(nameof(AccentColor), typeof(Color), typeof(PickerCell), Color.Default);

		/// <summary>
		/// Gets or sets the color of the accent.
		/// </summary>
		/// <value>The color of the accent.</value>
		public Color AccentColor
		{
			get => (Color) GetValue(AccentColorProperty);
			set => SetValue(AccentColorProperty, value);
		}

		/// <summary>
		/// The selected items order key property.
		/// </summary>
		public static readonly BindableProperty SelectedItemsOrderKeyProperty = BindableProperty.Create(nameof(SelectedItemsOrderKey), typeof(string), typeof(PickerCell), default(string));

		/// <summary>
		/// Gets or sets the selected items order key.
		/// </summary>
		/// <value>The selected items order key.</value>
		public string SelectedItemsOrderKey
		{
			get => (string) GetValue(SelectedItemsOrderKeyProperty);
			set => SetValue(SelectedItemsOrderKeyProperty, value);
		}

		/// <summary>
		/// The selected command property.
		/// </summary>
		public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(PickerCell), default(ICommand));

		/// <summary>
		/// Gets or sets the selected command.
		/// </summary>
		/// <value>The selected command.</value>
		public ICommand SelectedCommand
		{
			get => (ICommand) GetValue(SelectedCommandProperty);
			set => SetValue(SelectedCommandProperty, value);
		}

		/// <summary>
		/// The use natural sort property.
		/// </summary>
		public static readonly BindableProperty UseNaturalSortProperty = BindableProperty.Create(nameof(UseNaturalSort), typeof(bool), typeof(PickerCell), false);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.PickerCell"/> use natural sort.
		/// </summary>
		/// <value><c>true</c> if use natural sort; otherwise, <c>false</c>.</value>
		public bool UseNaturalSort
		{
			get => (bool) GetValue(UseNaturalSortProperty);
			set => SetValue(UseNaturalSortProperty, value);
		}

		/// <summary>
		/// The use auto value text property.
		/// </summary>
		public static readonly BindableProperty UseAutoValueTextProperty = BindableProperty.Create(nameof(UseAutoValueText), typeof(bool), typeof(PickerCell), true);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.PickerCell"/> use auto value text.
		/// </summary>
		/// <value><c>true</c> if use auto value text; otherwise, <c>false</c>.</value>
		public bool UseAutoValueText
		{
			get => (bool) GetValue(UseAutoValueTextProperty);
			set => SetValue(UseAutoValueTextProperty, value);
		}

		/// <summary>
		/// The use pick to close property.
		/// </summary>
		public static readonly BindableProperty UsePickToCloseProperty = BindableProperty.Create(nameof(UsePickToClose), typeof(bool), typeof(PickerCell), default(bool));

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.PickerCell"/> use pick to close.
		/// </summary>
		/// <value><c>true</c> if use pick to close; otherwise, <c>false</c>.</value>
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