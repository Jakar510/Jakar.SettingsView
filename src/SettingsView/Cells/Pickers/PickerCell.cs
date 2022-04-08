namespace Jakar.SettingsView.Shared.Cells;

[Xamarin.Forms.Internals.Preserve(true, false)]
public class PickerCell : PromptCellBase<object>
{
    public static readonly BindableProperty useNaturalSortProperty        = BindableProperty.Create(nameof(UseNaturalSort),        typeof(bool),     typeof(PickerCell), false);
    public static readonly BindableProperty usePickToCloseProperty        = BindableProperty.Create(nameof(UsePickToClose),        typeof(bool),     typeof(PickerCell), default(bool));
    public static readonly BindableProperty keepSelectedUntilBackProperty = BindableProperty.Create(nameof(KeepSelectedUntilBack), typeof(bool),     typeof(PickerCell), true);
    public static readonly BindableProperty selectedItemsOrderKeyProperty = BindableProperty.Create(nameof(SelectedItemsOrderKey), typeof(string),   typeof(PickerCell));
    public static readonly BindableProperty selectedCommandProperty       = BindableProperty.Create(nameof(SelectedCommand),       typeof(ICommand), typeof(PickerCell));
    public static readonly BindableProperty displayMemberProperty         = BindableProperty.Create(nameof(DisplayMember),         typeof(string),   typeof(PickerCell));
    public static readonly BindableProperty subDisplayMemberProperty      = BindableProperty.Create(nameof(SubDisplayMember),      typeof(string),   typeof(PickerCell));


    public static readonly BindableProperty itemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
                                                                                          typeof(IList),
                                                                                          typeof(PickerCell),
                                                                                          default(IList),
                                                                                          BindingMode.OneWay,
                                                                                          propertyChanging: ItemsSourceChanging
                                                                                         );

    public static readonly BindableProperty selectedItemsProperty = BindableProperty.Create(nameof(SelectedItems),
                                                                                            typeof(IList),
                                                                                            typeof(PickerCell),
                                                                                            default(IList),
                                                                                            BindingMode.TwoWay

                                                                                            // propertyChanged: SelectedItemPropertyChanged
                                                                                           );

    public static readonly BindableProperty selectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
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


    public static readonly BindableProperty selectionModeProperty = BindableProperty.Create(nameof(SelectionMode),
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
                                                                                                                                _ => (int)bindable.GetValue(maxSelectedNumberProperty)
                                                                                                                            };

                                                                                                                 bindable.SetValue(maxSelectedNumberProperty, mode);
                                                                                                             }
                                                                                           );

    public static readonly BindableProperty maxSelectedNumberProperty = BindableProperty.Create(nameof(MaxSelectedNumber),
                                                                                                typeof(int),
                                                                                                typeof(PickerCell),
                                                                                                -1,
                                                                                                BindingMode.OneWay,
                                                                                                coerceValue: ( bindable, value ) =>
                                                                                                             {
                                                                                                                 return (SelectMode)bindable.GetValue(selectionModeProperty) switch
                                                                                                                        {
                                                                                                                            SelectMode.Unlimited => -1,
                                                                                                                            SelectMode.Single    => 1,
                                                                                                                            _                    => value
                                                                                                                        };
                                                                                                             }
                                                                                               );


    public IList? ItemsSource
    {
        get => (IList?)GetValue(itemsSourceProperty);
        set => SetValue(itemsSourceProperty, value);
    }

    public string? DisplayMember
    {
        get => (string?)GetValue(displayMemberProperty);
        set => SetValue(displayMemberProperty, value);
    }

    public string? SubDisplayMember
    {
        get => (string?)GetValue(subDisplayMemberProperty);
        set => SetValue(subDisplayMemberProperty, value);
    }

    public IList? SelectedItems
    {
        get => (IList?)GetValue(selectedItemsProperty);
        set => SetValue(selectedItemsProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(selectedItemProperty);
        set => SetValue(selectedItemProperty, value);
    }

    public SelectMode SelectionMode
    {
        get => (SelectMode)GetValue(selectionModeProperty);
        set => SetValue(selectionModeProperty, value);
    }

    public int MaxSelectedNumber
    {
        get => (int)GetValue(maxSelectedNumberProperty);
        set
        {
            SetValue(maxSelectedNumberProperty, value);

            SelectionMode = value switch
                            {
                                < 0 => SelectMode.Unlimited,
                                0   => SelectMode.NotSet,
                                1   => SelectMode.Single,
                                > 1 => SelectMode.Multiple,
                            };
        }
    }

    internal IList? MergedSelectedList
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

    internal bool IsUnLimited  => SelectionMode == SelectMode.Unlimited;
    internal bool IsSingleMode => SelectionMode == SelectMode.Single;
    internal bool IsValidMode  => SelectionMode != SelectMode.NotSet;

    public bool KeepSelectedUntilBack
    {
        get => (bool)GetValue(keepSelectedUntilBackProperty);
        set => SetValue(keepSelectedUntilBackProperty, value);
    }


    public string? SelectedItemsOrderKey
    {
        get => (string?)GetValue(selectedItemsOrderKeyProperty);
        set => SetValue(selectedItemsOrderKeyProperty, value);
    }

    public ICommand? SelectedCommand
    {
        get => (ICommand?)GetValue(selectedCommandProperty);
        set => SetValue(selectedCommandProperty, value);
    }

    public bool UseNaturalSort
    {
        get => (bool)GetValue(useNaturalSortProperty);
        set => SetValue(useNaturalSortProperty, value);
    }

    public bool UsePickToClose
    {
        get => (bool)GetValue(usePickToCloseProperty);
        set => SetValue(usePickToCloseProperty, value);
    }


    // ---------------------------------------------------------------------------------------


    internal void SendValueChanged()
    {
        if ( IsSingleMode ) { ValueChangedHandler.SendValueChanged(SelectedItem ?? throw new NullReferenceException(nameof(SelectedItem))); }
        else { ValueChangedHandler.SendValueChanged(SelectedItems ?? throw new NullReferenceException(nameof(SelectedItems))); }
    }

    internal void InvokeSelectedCommand()
    {
        SelectedCommand?.Execute(IsSingleMode
                                     ? SelectedItem
                                     : SelectedItems
                                );
    }

    //getters cache
    private static readonly ConcurrentDictionary<Type, Dictionary<string, Func<object, object?>>> _displayValueCache = new();

    //Row.Title getter
    internal Func<object, object?> DisplayValue
    {
        get
        {
            if ( _getters is null ||
                 DisplayMember is null ) { return ( obj ) => obj; }

            return _getters.ContainsKey(DisplayMember)
                       ? _getters[DisplayMember]
                       : DefaultDisplayValue;
        }
    }

    private object DefaultDisplayValue( object obj ) => obj;

    //Row.Description getter
    internal Func<object, object?> SubDisplayValue
    {
        get
        {
            if ( _getters is null ||
                 SubDisplayMember is null ) { return ( obj ) => null; }

            return _getters.ContainsKey(SubDisplayMember)
                       ? _getters[SubDisplayMember]
                       : DefaultSubValueGetter;
        }
    }

    private static object? DefaultSubValueGetter( object obj ) => null;

    //OrderKey getter
    internal Func<object, object?>? KeyValue
    {
        get
        {
            if ( _getters is null ||
                 SelectedItemsOrderKey is null ) { return null; }

            if ( _getters.ContainsKey(SelectedItemsOrderKey) ) { return _getters[SelectedItemsOrderKey]; }

            return null;
        }
    }

    private Dictionary<string, Func<object, object?>>? _getters;

    internal string GetSelectedItemsText()
    {
        IList? items = MergedSelectedList;
        if ( items is null ) return string.Empty;

        List<string> sortedList;

        if ( KeyValue is not null )
        {
            var dict = new Dictionary<object, string>();

            foreach ( object item in items )
            {
                object? key   = KeyValue?.Invoke(item);
                var     value = DisplayValue?.Invoke(item)?.ToString();

                if ( key is null ||
                     string.IsNullOrWhiteSpace(value) ) continue;

                dict.Add(key, value);
            }

            sortedList = UseNaturalSort
                             ? dict.OrderBy(x => x.Key.ToString(), new NaturalComparer()).Select(x => x.Value).ToList()
                             : dict.OrderBy(x => x.Key).Select(x => x.Value).ToList();
        }
        else
        {
            List<string> strList = ( from object item in items select DisplayValue?.Invoke(item)?.ToString() ).ToList();

            NaturalComparer? comparer = UseNaturalSort
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
        return string.Join(", ", sortedList);
    }


    internal static void ItemsSourceChanging( BindableObject bindable, object? oldValue, object? newValue )
    {
        if ( bindable is not PickerCell control ) return;
        if ( newValue is null ) { return; }

        control.SetUpPropertyCache(newValue as IList);
    }

    // Create all property getters
    private static Dictionary<string, Func<object, object?>> CreateGetProperty( Type type )
    {
        IEnumerable<PropertyInfo> prop = type.GetRuntimeProperties().Where(x => x.DeclaringType == type && !x.Name.StartsWith("_", StringComparison.Ordinal));

        ParameterExpression target = Expression.Parameter(typeof(object), "target");

        var dictGetter = new Dictionary<string, Func<object, object?>>();

        foreach ( PropertyInfo p in prop )
        {
            MemberExpression body = Expression.PropertyOrField(Expression.Convert(target, type), p.Name);

            Expression<Func<object, object?>> lambda = Expression.Lambda<Func<object, object?>>(Expression.Convert(body, typeof(object)), target);

            dictGetter[p.Name] = lambda.Compile();
        }

        return dictGetter;
    }

    private void SetUpPropertyCache( IEnumerable? itemsSource )
    {
        if ( itemsSource is null ) return;
        Type[] typeArg = itemsSource.GetType().GenericTypeArguments;

        if ( !typeArg.Any() ) { throw new ArgumentException("ItemsSource must be GenericType."); }

        // If the type is a system built-in-type, it doesn't create GetProperty.
        if ( typeArg[0].IsBuiltInType() )
        {
            _getters = null;

            return;
        }

        _getters = _displayValueCache.GetOrAdd(typeArg[0], CreateGetProperty);
    }
}
