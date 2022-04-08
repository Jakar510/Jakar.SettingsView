namespace Jakar.SettingsView.Shared.CellBase;

[Xamarin.Forms.Internals.Preserve(true, false)]
// [ContentProperty(nameof(nameof(Prompt)))]
public abstract class PromptCellBase<TValue> : ValueCellBase<TValue>
{
    // public static BindableProperty PopupTitleProperty = BindableProperty.Create(nameof(PopupTitle), typeof(string), typeof(PopupCellBase), "Select an item");
    // public static BindableProperty PopupAcceptProperty = BindableProperty.Create(nameof(PopupAccept), typeof(string), typeof(PopupCellBase), "OK");
    // public static BindableProperty PopupCancelProperty = BindableProperty.Create(nameof(PopupCancel), typeof(string), typeof(PopupCellBase), "Cancel");
    //
    // public static BindableProperty PopupSelectedColorProperty = BindableProperty.Create(nameof(PopupSelectedColor), typeof(Color), typeof(PopupConfig), Color.Default);
    // public static BindableProperty PopupBackgroundColorProperty = BindableProperty.Create(nameof(PopupBackgroundColor), typeof(Color), typeof(PopupConfig), Color.White);
    // public static BindableProperty PopupTitleColorProperty = BindableProperty.Create(nameof(PopupTitleColor), typeof(Color), typeof(PopupConfig), Color.Black);
    // public static BindableProperty PopupItemTextColorProperty = BindableProperty.Create(nameof(PopupItemTextColor), typeof(Color), typeof(PopupConfig), Color.Black);
    // public static BindableProperty PopupItemDescriptionColorProperty = BindableProperty.Create(nameof(PopupItemDescriptionTextColor), typeof(Color), typeof(PopupConfig), Color.SlateGray);
    // public static readonly BindableProperty PopupAccentColorProperty = BindableProperty.Create(nameof(PopupAccentColor), typeof(Color), typeof(PopupConfig), Color.Accent);
    //
    // public static readonly BindableProperty PopupTitleFontSizeProperty = BindableProperty.Create(nameof(PopupTitleFontSize), typeof(int), typeof(PopupConfig), 12);
    // public static readonly BindableProperty PopupItemFontSizeProperty = BindableProperty.Create(nameof(PopupItemFontSize), typeof(int), typeof(PopupConfig), 10);
    // public static readonly BindableProperty PopupItemDescriptionFontSizeProperty = BindableProperty.Create(nameof(PopupItemDescriptionFontSize), typeof(int), typeof(PopupConfig), 10);
    // public static readonly BindableProperty PopupSelectedFontSizeProperty = BindableProperty.Create(nameof(PopupSelectedFontSize), typeof(int), typeof(PopupConfig), 12);


    // public Color PopupBackgroundColor
    // {
    // 	get => (Color) GetValue(PopupBackgroundColorProperty);
    // 	set => SetValue(PopupBackgroundColorProperty, value);
    // }
    //
    //
    // public string PopupTitle
    // {
    // 	get => (string) GetValue(PopupTitleProperty);
    // 	set => SetValue(PopupTitleProperty, value);
    // }
    // public Color PopupTitleColor
    // {
    // 	get => (Color) GetValue(PopupTitleColorProperty);
    // 	set => SetValue(PopupTitleColorProperty, value);
    // }
    // public int PopupTitleFontSize
    // {
    // 	get => (int) GetValue(PopupTitleFontSizeProperty);
    // 	set => SetValue(PopupTitleFontSizeProperty, value);
    // }
    //
    //
    // public Color PopupItemTextColor
    // {
    // 	get => (Color) GetValue(PopupItemTextColorProperty);
    // 	set => SetValue(PopupItemTextColorProperty, value);
    // }
    // public int PopupItemFontSize
    // {
    // 	get => (int) GetValue(PopupItemFontSizeProperty);
    // 	set => SetValue(PopupItemFontSizeProperty, value);
    // }
    //
    // public Color PopupItemDescriptionTextColor
    // {
    // 	get => (Color) GetValue(PopupItemDescriptionColorProperty);
    // 	set => SetValue(PopupItemDescriptionColorProperty, value);
    // }
    // public int PopupItemDescriptionFontSize
    // {
    // 	get => (int) GetValue(PopupItemDescriptionFontSizeProperty);
    // 	set => SetValue(PopupItemDescriptionFontSizeProperty, value);
    // }
    //
    //
    // public Color PopupAccentColor
    // {
    // 	get => (Color) GetValue(PopupAccentColorProperty);
    // 	set => SetValue(PopupAccentColorProperty, value);
    // }
    // public int PopupSelectedFontSize
    // {
    // 	get => (int) GetValue(PopupSelectedFontSizeProperty);
    // 	set => SetValue(PopupSelectedFontSizeProperty, value);
    // }
    // public Color PopupSelectedColor
    // {
    // 	get => (Color) GetValue(PopupSelectedColorProperty);
    // 	set => SetValue(PopupSelectedColorProperty, value);
    // }
    //
    //
    // public string PopupAccept
    // {
    // 	get => (string) GetValue(PopupAcceptProperty);
    // 	set => SetValue(PopupAcceptProperty, value);
    // }
    // public string PopupCancel
    // {
    // 	get => (string) GetValue(PopupCancelProperty);
    // 	set => SetValue(PopupCancelProperty, value);
    // }

    public static readonly BindableProperty promptProperty = BindableProperty.Create(nameof(Prompt), typeof(PopupConfig), typeof(PromptCellBase<TValue>), new PopupConfig());

    public PopupConfig Prompt
    {
        get => (PopupConfig) GetValue(promptProperty);
        set => SetValue(promptProperty, value);
    }

    public static readonly BindableProperty isCircularPickerProperty = BindableProperty.Create(nameof(IsCircularPicker), typeof(bool), typeof(PromptCellBase<TValue>), true);

    public bool IsCircularPicker
    {
        get => (bool) GetValue(isCircularPickerProperty);
        set => SetValue(isCircularPickerProperty, value);
    }
}


// public class BaseCell<TValueType> : BaseCell
// {
// 	public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(BaseCell<TValueType>), default(ICommand));
//
// 	public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
// 																						  typeof(IList<TValueType>),
// 																						  typeof(BaseCell<TValueType>),
// 																						  default(IList<TValueType>),
// 																						  BindingMode.OneWay,
// 																						  propertyChanging: PickerCell.ItemsSourceChanging
// 																						 );
//
// 	public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems),
// 																							typeof(IList<TValueType>),
// 																							typeof(BaseCell<TValueType>),
// 																							default(IList<TValueType>),
// 																							BindingMode.TwoWay
// 																						   );
//
// 	public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
// 																						   typeof(TValueType),
// 																						   typeof(BaseCell<TValueType>),
// 																						   default,
// 																						   BindingMode.TwoWay
// 																						  );
//
// 	public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode),
// 																							typeof(SelectionMode),
// 																							typeof(BaseCell<TValueType>),
// 																							SelectionMode.Multiple,
// 																							BindingMode.OneWay,
// 																							propertyChanged: ( bindable, old_value, new_value ) =>
// 																											 {
// 																												 if ( (SelectionMode) new_value == SelectionMode.Single ) { bindable.SetValue(MaxSelectedNumberProperty, 1); }
// 																											 }
// 																						   );
//
// 	public static readonly BindableProperty MaxSelectedNumberProperty = BindableProperty.Create(nameof(MaxSelectedNumber),
// 																								typeof(int),
// 																								typeof(BaseCell<TValueType>),
// 																								0,
// 																								BindingMode.OneWay,
// 																								coerceValue: ( bindable, value ) =>
// 																											 {
// 																												 var temp = (SelectionMode) bindable.GetValue(SelectionModeProperty);
// 																												 return temp switch
// 																														{
// 																															SelectionMode.Single => 1,
// 																															SelectionMode.None => 0,
// 																															_ => value
// 																														};
// 																											 }
// 																							   );
//
//
// 	public IList<TValueType>? ItemsSource
// 	{
// 		get => (IList<TValueType>?) GetValue(ItemsSourceProperty);
// 		set => SetValue(ItemsSourceProperty, value);
// 	}
//
// 	public IList<TValueType> SelectedItems
// 	{
// 		get => (IList<TValueType>) GetValue(SelectedItemsProperty);
// 		set => SetValue(SelectedItemsProperty, value);
// 	}
//
// 	public TValueType SelectedItem
// 	{
// 		get => (TValueType) GetValue(SelectedItemProperty);
// 		set => SetValue(SelectedItemProperty, value);
// 	}
//
//
// 	public SelectionMode SelectionMode
// 	{
// 		get => (SelectionMode) GetValue(SelectionModeProperty);
// 		set
// 		{
// 			SetValue(SelectionModeProperty, value);
// 			MaxSelectedNumber = value switch
// 								{
// 									SelectionMode.Single => 1,
// 									SelectionMode.None => 0,
// 									_ => MaxSelectedNumber
// 								};
// 		}
// 	}
//
// 	public int MaxSelectedNumber
// 	{
// 		get => (int) GetValue(MaxSelectedNumberProperty);
// 		set => SetValue(MaxSelectedNumberProperty, value);
// 	}
//
//
// 	public ICommand? SelectedCommand
// 	{
// 		get => (ICommand) GetValue(SelectedCommandProperty);
// 		set => SetValue(SelectedCommandProperty, value);
// 	}
//
// 	internal void InvokeCommand() { SelectedCommand?.Execute(SelectionMode == SelectionMode.Single ? SelectedItem : SelectedItems); }
//
// 	internal IList<TValueType> MergedSelectedList
// 	{
// 		get
// 		{
// 			if ( SelectionMode != SelectionMode.Single )
// 				return SelectedItems;
// 			return new List<TValueType>
// 				   {
// 					   SelectedItem
// 				   };
// 		}
// 	}
//
// 	//
// 	// public string? DisplayMember
// 	// {
// 	// 	get => (string) GetValue(DisplayMemberProperty);
// 	// 	set => SetValue(DisplayMemberProperty, value);
// 	// }
// 	//
// 	// public string? SubDisplayMember
// 	// {
// 	// 	get => (string) GetValue(SubDisplayMemberProperty);
// 	// 	set => SetValue(SubDisplayMemberProperty, value);
// 	// }
// 	//
// 	//
// 	//
// 	// //getters cache
// 	// private static ConcurrentDictionary<Type, Dictionary<string, Func<object, TValueType>>> _DisplayValueCache { get; } = new ConcurrentDictionary<Type, Dictionary<string, Func<object, TValueType>>>();
// 	//
// 	// //DisplayMember getter
// 	// internal Func<object, TValueType> DisplayValue
// 	// {
// 	// 	get
// 	// 	{
// 	// 		if ( _Getters is null ||
// 	// 			 DisplayMember is null ) { return GetDefault; }
// 	//
// 	// 		return _Getters != null && _Getters.ContainsKey(DisplayMember) ? _Getters[DisplayMember] : GetDefault;
// 	// 	}
// 	// }
// 	//
// 	// internal Func<object, TValueType> SubDisplayValue
// 	// {
// 	// 	get
// 	// 	{
// 	// 		if ( _Getters == null ||
// 	// 			 SubDisplayMember == null ) { return GetNull; }
// 	//
// 	// 		return _Getters.ContainsKey(SubDisplayMember) ? _Getters[SubDisplayMember] : GetNull;
// 	// 	}
// 	// }
// 	//
// 	// private static object GetDefault( object o ) => o;
// 	// private static object? GetNull( object? o ) => null;
// 	//
// 	//
// 	// //OrderKey getter
// 	// internal Func<object, TValueType>? KeyValue
// 	// {
// 	// 	get
// 	// 	{
// 	// 		if ( _Getters == null ||
// 	// 			 SelectedItemsOrderKey == null ) { return null; }
// 	//
// 	// 		return _Getters.ContainsKey(SelectedItemsOrderKey) ? _Getters[SelectedItemsOrderKey] : null;
// 	// 	}
// 	// }
// 	//
// 	// internal string GetSelectedItemsText()
// 	// {
// 	// 	List<string>? sortedList;
// 	//
// 	// 	// if ( MergedSelectedList is null ) { return string.Empty; }
// 	//
// 	//
// 	// 	if ( KeyValue != null )
// 	// 	{
// 	// 		var dict = new Dictionary<TValueType, string>();
// 	// 		foreach ( TValueType item in MergedSelectedList ) { dict.Add(KeyValue(item), DisplayValue(item)?.ToString() ?? string.Empty); }
// 	//
// 	// 		if ( UseNaturalSort ) { sortedList = dict.OrderBy(x => x.Key?.ToString(), new NaturalComparer()).Select(x => x.Value).ToList(); }
// 	// 		else { sortedList = dict.OrderBy(x => x.Key).Select(x => x.Value).ToList(); }
// 	// 	}
// 	// 	else
// 	// 	{
// 	// 		var strList = new List<string>();
// 	// 		foreach ( TValueType item in MergedSelectedList ) { strList.Add(DisplayValue(item)?.ToString() ?? string.Empty); }
// 	//
// 	// 		NaturalComparer? comparer = UseNaturalSort ? new NaturalComparer() : null;
// 	// 		sortedList = strList.OrderBy(x => x, comparer).ToList();
// 	// 	}
// 	//
// 	// 	return string.Join(", ", sortedList.ToArray());
// 	// }
// 	//
// 	// private Dictionary<string, Func<object, TValueType>>? _Getters { get; set; }
// 	//
// 	// private static void ItemsSourceChanging( BindableObject bindable, TValueType oldValue, TValueType newValue )
// 	// {
// 	// 	if ( newValue == null ) { return; }
// 	//
// 	// 	if ( bindable is BaseCell<TValueType> control )
// 	// 		control.SetUpPropertyCache(newValue as IList);
// 	// }
// 	//
// 	// // Create all property getters
// 	// private static Dictionary<string, Func<object, TValueType>> CreateGetProperty( Type t )
// 	// {
// 	// 	IEnumerable<PropertyInfo> prop = t.GetRuntimeProperties().Where(x => x.DeclaringType == t && !x.Name.StartsWith("_", StringComparison.Ordinal));
// 	//
// 	// 	ParameterExpression target = Expression.Parameter(typeof(TValueType), "target");
// 	//
// 	// 	var dictGetter = new Dictionary<string, Func<object, TValueType>>();
// 	//
// 	// 	foreach ( PropertyInfo p in prop )
// 	// 	{
// 	// 		MemberExpression body = Expression.PropertyOrField(Expression.Convert(target, t), p.Name);
// 	//
// 	// 		Expression<Func<object, TValueType>> lambda = Expression.Lambda<Func<object, TValueType>>(Expression.Convert(body, typeof(TValueType)), target);
// 	//
// 	// 		dictGetter[p.Name] = lambda.Compile();
// 	// 	}
// 	//
// 	// 	return dictGetter;
// 	// }
// 	//
// 	// private void SetUpPropertyCache( IEnumerable? itemsSource )
// 	// {
// 	// 	Type[]? typeArg = itemsSource?.GetType().GenericTypeArguments;
// 	//
// 	// 	if ( typeArg is null )
// 	// 		return;
// 	//
// 	// 	if ( !typeArg.Any() ) { throw new ArgumentException("ItemsSource must be GenericType."); }
// 	//
// 	// 	// If the type is a system built-in-type, it doesn't create GetProperty.
// 	// 	if ( IsBuiltInType(typeArg[0]) )
// 	// 	{
// 	// 		_Getters = null;
// 	//
// 	// 		return;
// 	// 	}
// 	//
// 	// 	_Getters = _DisplayValueCache.GetOrAdd(typeArg[0], CreateGetProperty);
// 	// }
// 	//
// 	// protected static bool IsBuiltInType( Type type )
// 	// {
// 	// 	switch ( type.FullName )
// 	// 	{
// 	// 		case "System.Boolean":
// 	// 		case "System.Byte":
// 	// 		case "System.SByte":
// 	// 		case "System.Char":
// 	// 		case "System.Int16":
// 	// 		case "System.UInt16":
// 	// 		case "System.Int32":
// 	// 		case "System.UInt32":
// 	// 		case "System.Int64":
// 	// 		case "System.UInt64":
// 	// 		case "System.Single":
// 	// 		case "System.Double":
// 	// 		case "System.Decimal":
// 	// 		case "System.String":
// 	// 		case "System.Object":
// 	// 			return true;
// 	//
// 	// 		default:
// 	// 			return false;
// 	// 	}
// 	// }
// }