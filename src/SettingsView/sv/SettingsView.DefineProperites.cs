using System.Collections.Specialized;
using DropEventArgs = Jakar.SettingsView.Shared.Events.DropEventArgs;



namespace Jakar.SettingsView.Shared.sv;

[Xamarin.Forms.Internals.Preserve(true, false)]
public partial class SettingsView
{
    public event EventHandler<DropEventArgs>? ItemDropped;

    public static readonly BindableProperty itemDroppedCommandProperty = BindableProperty.Create(nameof(ItemDroppedCommand), typeof(ICommand), typeof(SettingsView));

    public ICommand? ItemDroppedCommand
    {
        get => (ICommand?) GetValue(itemDroppedCommandProperty);
        set => SetValue(itemDroppedCommandProperty, value);
    }


#region Popups

    public static readonly BindableProperty popupCfgProperty = BindableProperty.Create(nameof(Popup),
                                                                                       typeof(CellPopupConfig),
                                                                                       typeof(SettingsView),
                                                                                       new CellPopupConfig(),
                                                                                       propertyChanging: PopupCfgPropertyChanging
                                                                                      );

    private static void PopupCfgPropertyChanging( BindableObject bindable, object oldValue, object newValue )
    {
        if ( oldValue is CellPopupConfig old )
        {
            old.Parent          =  null;
            old.PropertyChanged -= Config_OnPropertyChanged;
        }

        if ( newValue is not CellPopupConfig current ) return;
        current.Parent          =  (SettingsView) bindable;
        current.PropertyChanged -= Config_OnPropertyChanged;
    }

    private static void Config_OnPropertyChanged( object sender, PropertyChangedEventArgs e )
    {
        switch ( sender )
        {
            case SettingsView view:
                view.OnPropertyChanged(e);
                break;

            case CellPopupConfig cfg:
                cfg.Parent?.OnPropertyChanged(e);
                break;
        }
    }

    public CellPopupConfig Popup
    {
        get => (CellPopupConfig) GetValue(popupCfgProperty);
        set => SetValue(popupCfgProperty, value);
    }

#endregion


#region SettingsView Colors

    public static readonly     BindableProperty separatorColorProperty  = BindableProperty.Create(nameof(SeparatorColor),  typeof(Color), typeof(SettingsView), SvConstants.Sv.separator_Color);
    public static readonly     BindableProperty selectedColorProperty   = BindableProperty.Create(nameof(SelectedColor),   typeof(Color), typeof(SettingsView), SvConstants.Prompt.Selected.text_Color);
    public static readonly BindableProperty backgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SettingsView), SvConstants.Sv.background_Color);


    [TypeConverter(typeof(ColorTypeConverter))]
    public new Color BackgroundColor
    {
        get => (Color) GetValue(backgroundColorProperty);
        set => SetValue(backgroundColorProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color SeparatorColor
    {
        get => (Color) GetValue(separatorColorProperty);
        set => SetValue(separatorColorProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color SelectedColor
    {
        get => (Color) GetValue(selectedColorProperty);
        set => SetValue(selectedColorProperty, value);
    }

#endregion


#region Header

    public static readonly BindableProperty headerPaddingProperty = BindableProperty.Create(nameof(HeaderPadding), typeof(Thickness), typeof(SettingsView), SvConstants.Section.Header.padding);

    public static readonly BindableProperty headerTextColorProperty = BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(SettingsView), SvConstants.Section.Header.text_Color);

    public static readonly BindableProperty headerFontSizeProperty = BindableProperty.Create(nameof(HeaderFontSize),
                                                                                             typeof(double),
                                                                                             typeof(SettingsView),
                                                                                             SvConstants.Section.Header.FONT_SIZE,
                                                                                             defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView) bindable)
                                                                                            );

    public static readonly BindableProperty headerFontFamilyProperty = BindableProperty.Create(nameof(HeaderFontFamily), typeof(string), typeof(SettingsView));

    public static readonly BindableProperty headerFontAttributesProperty =
        BindableProperty.Create(nameof(HeaderFontAttributes), typeof(FontAttributes), typeof(SettingsView), SvConstants.Section.Header.font_Attributes);

    // public static BindableProperty HeaderTextVerticalAlignProperty = BindableProperty.Create(nameof(HeaderTextVerticalAlign), typeof(LayoutAlignment), typeof(SettingsView), LayoutAlignment.End);

    public static readonly BindableProperty headerBackgroundColorProperty =
        BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(SettingsView), SvConstants.Section.Header.background_Color);

    public static readonly BindableProperty headerHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(double), typeof(SettingsView), SvConstants.Section.Header.MIN_ROW_HEIGHT);


    // TODO: decide what to do with these: remove / re-purpose / implement
    public Thickness HeaderPadding
    {
        get => (Thickness) GetValue(headerPaddingProperty);
        set => SetValue(headerPaddingProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color HeaderTextColor
    {
        get => (Color) GetValue(headerTextColorProperty);
        set => SetValue(headerTextColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double HeaderFontSize
    {
        get => (double) GetValue(headerFontSizeProperty);
        set => SetValue(headerFontSizeProperty, value);
    }

    public string? HeaderFontFamily
    {
        get => (string?) GetValue(headerFontFamilyProperty);
        set => SetValue(headerFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes HeaderFontAttributes
    {
        get => (FontAttributes) GetValue(headerFontAttributesProperty);
        set => SetValue(headerFontAttributesProperty, value);
    }

    // public LayoutAlignment HeaderTextVerticalAlign
    // {
    // 	get => (LayoutAlignment) GetValue(HeaderTextVerticalAlignProperty);
    // 	set => SetValue(HeaderTextVerticalAlignProperty, value);
    // }

    public Color HeaderBackgroundColor
    {
        get => (Color) GetValue(headerBackgroundColorProperty);
        set => SetValue(headerBackgroundColorProperty, value);
    }

    public double HeaderHeight
    {
        get => (double) GetValue(headerHeightProperty);
        set => SetValue(headerHeightProperty, value);
    }

#endregion


#region Footer

    public static readonly BindableProperty footerTextColorProperty = BindableProperty.Create(nameof(FooterTextColor), typeof(Color), typeof(SettingsView), SvConstants.Section.Footer.text_Color);

    public static readonly BindableProperty footerFontSizeProperty = BindableProperty.Create(nameof(FooterFontSize),
                                                                                             typeof(double),
                                                                                             typeof(SettingsView),
                                                                                             SvConstants.Section.Footer.FONT_SIZE,
                                                                                             BindingMode.OneWay,
                                                                                             defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView) bindable)
                                                                                            );

    public static readonly BindableProperty footerFontFamilyProperty = BindableProperty.Create(nameof(FooterFontFamily), typeof(string), typeof(SettingsView));

    public static readonly BindableProperty footerFontAttributesProperty =
        BindableProperty.Create(nameof(FooterFontAttributes), typeof(FontAttributes), typeof(SettingsView), SvConstants.Section.Footer.font_Attributes);

    public static readonly BindableProperty footerBackgroundColorProperty =
        BindableProperty.Create(nameof(FooterBackgroundColor), typeof(Color), typeof(SettingsView), SvConstants.Section.Footer.background_Color);

    public static readonly BindableProperty footerPaddingProperty = BindableProperty.Create(nameof(FooterPadding), typeof(Thickness), typeof(SettingsView), SvConstants.Section.Footer.padding);


    // TODO: decide what to do with these: remove / re-purpose / implement
    [TypeConverter(typeof(ColorTypeConverter))]
    public Color FooterTextColor
    {
        get => (Color) GetValue(footerTextColorProperty);
        set => SetValue(footerTextColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double FooterFontSize
    {
        get => (double) GetValue(footerFontSizeProperty);
        set => SetValue(footerFontSizeProperty, value);
    }

    public string? FooterFontFamily
    {
        get => (string?) GetValue(footerFontFamilyProperty);
        set => SetValue(footerFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes FooterFontAttributes
    {
        get => (FontAttributes) GetValue(footerFontAttributesProperty);
        set => SetValue(footerFontAttributesProperty, value);
    }

    public Color FooterBackgroundColor
    {
        get => (Color) GetValue(footerBackgroundColorProperty);
        set => SetValue(footerBackgroundColorProperty, value);
    }

    public Thickness FooterPadding
    {
        get => (Thickness) GetValue(footerPaddingProperty);
        set => SetValue(footerPaddingProperty, value);
    }

#endregion


#region Cell Title

    public static readonly BindableProperty cellTitleColorProperty = BindableProperty.Create(nameof(CellTitleColor), typeof(Color), typeof(SettingsView), SvConstants.Sv.Title.text_Color);

    public static readonly BindableProperty cellTitleFontSizeProperty = BindableProperty.Create(nameof(CellTitleFontSize),
                                                                                                typeof(double),
                                                                                                typeof(SettingsView),
                                                                                                SvConstants.Sv.Title.Font.SIZE,
                                                                                                BindingMode.OneWay,
                                                                                                defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, (SettingsView) bindable)
                                                                                               );

    public static readonly BindableProperty cellTitleFontFamilyProperty = BindableProperty.Create(nameof(CellTitleFontFamily), typeof(string), typeof(SettingsView));

    public static readonly BindableProperty cellTitleFontAttributesProperty =
        BindableProperty.Create(nameof(CellTitleFontAttributes), typeof(FontAttributes), typeof(SettingsView), SvConstants.Sv.Title.Font.attributes);

    public static readonly  BindableProperty cellTitleAlignmentProperty = BindableProperty.Create(nameof(CellTitleAlignment), typeof(TextAlignment), typeof(SettingsView), SvConstants.Sv.Title.alignment);

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color CellTitleColor
    {
        get => (Color) GetValue(cellTitleColorProperty);
        set => SetValue(cellTitleColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double CellTitleFontSize
    {
        get => (double) GetValue(cellTitleFontSizeProperty);
        set => SetValue(cellTitleFontSizeProperty, value);
    }

    public string? CellTitleFontFamily
    {
        get => (string?) GetValue(cellTitleFontFamilyProperty);
        set => SetValue(cellTitleFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes CellTitleFontAttributes
    {
        get => (FontAttributes) GetValue(cellTitleFontAttributesProperty);
        set => SetValue(cellTitleFontAttributesProperty, value);
    }
		
    [TypeConverter(typeof(TextAlignmentConverter))]
    public TextAlignment CellTitleAlignment
    {
        get => (TextAlignment) GetValue(cellTitleAlignmentProperty);
        set => SetValue(cellTitleAlignmentProperty, value);
    }

#endregion


#region Cell Value

    public static readonly BindableProperty cellValueTextColorProperty = BindableProperty.Create(nameof(CellValueTextColor), typeof(Color), typeof(SettingsView), SvConstants.Sv.Value.text_Color);

    public static readonly  BindableProperty cellValueTextFontSizeProperty = BindableProperty.Create(nameof(CellValueTextFontSize), typeof(double), typeof(SettingsView), SvConstants.Sv.Value.Font.SIZE);

    public static readonly BindableProperty cellValueTextFontFamilyProperty = BindableProperty.Create(nameof(CellValueTextFontFamily), typeof(string), typeof(SettingsView));

    public static readonly BindableProperty cellValueTextFontAttributesProperty =
        BindableProperty.Create(nameof(CellValueTextFontAttributes), typeof(FontAttributes), typeof(SettingsView), SvConstants.Sv.Value.Font.attributes);

    public static readonly BindableProperty cellValueTextAlignmentProperty =
        BindableProperty.Create(nameof(CellValueTextAlignment), typeof(TextAlignment), typeof(SettingsView), SvConstants.Sv.Value.alignment);

    public static readonly BindableProperty cellPlaceholderColorProperty =
        BindableProperty.Create(nameof(CellPlaceholderColor), typeof(Color), typeof(SettingsView), SvConstants.Sv.Value.placeholder_Color);

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color CellPlaceholderColor
    {
        get => (Color) GetValue(cellPlaceholderColorProperty);
        set => SetValue(cellPlaceholderColorProperty, value);
    }

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color CellValueTextColor
    {
        get => (Color) GetValue(cellValueTextColorProperty);
        set => SetValue(cellValueTextColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double CellValueTextFontSize
    {
        get => (double) GetValue(cellValueTextFontSizeProperty);
        set => SetValue(cellValueTextFontSizeProperty, value);
    }

    public string? CellValueTextFontFamily
    {
        get => (string?) GetValue(cellValueTextFontFamilyProperty);
        set => SetValue(cellValueTextFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes CellValueTextFontAttributes
    {
        get => (FontAttributes) GetValue(cellValueTextFontAttributesProperty);
        set => SetValue(cellValueTextFontAttributesProperty, value);
    }
		
    [TypeConverter(typeof(TextAlignmentConverter))]
    public TextAlignment CellValueTextAlignment
    {
        get => (TextAlignment) GetValue(cellValueTextAlignmentProperty);
        set => SetValue(cellValueTextAlignmentProperty, value);
    }

#endregion


#region Cell Description

    public static readonly  BindableProperty cellDescriptionColorProperty = BindableProperty.Create(nameof(CellDescriptionColor), typeof(Color), typeof(SettingsView), SvConstants.Sv.Description.text_Color);

    public static readonly BindableProperty cellDescriptionFontSizeProperty =
        BindableProperty.Create(nameof(CellDescriptionFontSize), typeof(double), typeof(SettingsView), SvConstants.Sv.Description.Font.SIZE);

    public static readonly BindableProperty cellDescriptionFontFamilyProperty = BindableProperty.Create(nameof(CellDescriptionFontFamily), typeof(string), typeof(SettingsView));

    public static readonly BindableProperty cellDescriptionFontAttributesProperty =
        BindableProperty.Create(nameof(CellDescriptionFontAttributes), typeof(FontAttributes), typeof(SettingsView), SvConstants.Sv.Description.Font.attributes);

    public static readonly BindableProperty cellDescriptionAlignmentProperty =
        BindableProperty.Create(nameof(CellDescriptionAlignment), typeof(TextAlignment), typeof(SettingsView), SvConstants.Sv.Description.alignment);

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color CellDescriptionColor
    {
        get => (Color) GetValue(cellDescriptionColorProperty);
        set => SetValue(cellDescriptionColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double CellDescriptionFontSize
    {
        get => (double) GetValue(cellDescriptionFontSizeProperty);
        set => SetValue(cellDescriptionFontSizeProperty, value);
    }

    public string? CellDescriptionFontFamily
    {
        get => (string?) GetValue(cellDescriptionFontFamilyProperty);
        set => SetValue(cellDescriptionFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes CellDescriptionFontAttributes
    {
        get => (FontAttributes) GetValue(cellDescriptionFontAttributesProperty);
        set => SetValue(cellDescriptionFontAttributesProperty, value);
    }
		
    [TypeConverter(typeof(TextAlignmentConverter))]
    public TextAlignment CellDescriptionAlignment
    {
        get => (TextAlignment) GetValue(cellDescriptionAlignmentProperty);
        set => SetValue(cellDescriptionAlignmentProperty, value);
    }

#endregion


#region Cell Hint

    public static readonly BindableProperty cellHintTextColorProperty  = BindableProperty.Create(nameof(CellHintTextColor),  typeof(Color),  typeof(SettingsView), SvConstants.Sv.Hint.text_Color);
    public static readonly BindableProperty cellHintFontSizeProperty   = BindableProperty.Create(nameof(CellHintFontSize),   typeof(double), typeof(SettingsView), SvConstants.Sv.Hint.Font.SIZE);
    public static readonly BindableProperty cellHintFontFamilyProperty = BindableProperty.Create(nameof(CellHintFontFamily), typeof(string), typeof(SettingsView));

    public static readonly BindableProperty cellHintFontAttributesProperty =
        BindableProperty.Create(nameof(CellHintFontAttributes), typeof(FontAttributes), typeof(SettingsView), SvConstants.Sv.Hint.Font.attributes);

    public static readonly BindableProperty cellHintAlignmentProperty = BindableProperty.Create(nameof(CellHintAlignment), typeof(TextAlignment), typeof(SettingsView), SvConstants.Sv.Hint.alignment);

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color CellHintTextColor
    {
        get => (Color) GetValue(cellHintTextColorProperty);
        set => SetValue(cellHintTextColorProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    public double CellHintFontSize
    {
        get => (double) GetValue(cellHintFontSizeProperty);
        set => SetValue(cellHintFontSizeProperty, value);
    }

    public string? CellHintFontFamily
    {
        get => (string?) GetValue(cellHintFontFamilyProperty);
        set => SetValue(cellHintFontFamilyProperty, value);
    }

    [TypeConverter(typeof(FontAttributesConverter))]
    public FontAttributes CellHintFontAttributes
    {
        get => (FontAttributes) GetValue(cellHintFontAttributesProperty);
        set => SetValue(cellHintFontAttributesProperty, value);
    }
		
    [TypeConverter(typeof(TextAlignmentConverter))]
    public TextAlignment CellHintAlignment
    {
        get => (TextAlignment) GetValue(cellHintAlignmentProperty);
        set => SetValue(cellHintAlignmentProperty, value);
    }

#endregion


#region Cell Icon

    public static readonly BindableProperty cellIconSizeProperty   = BindableProperty.Create(nameof(CellIconSize),   typeof(Size),   typeof(SettingsView), SvConstants.Sv.Icon.size);
    public static readonly BindableProperty cellIconRadiusProperty = BindableProperty.Create(nameof(CellIconRadius), typeof(double), typeof(SettingsView), SvConstants.Sv.Icon.RADIUS);


    [TypeConverter(typeof(SizeConverter))]
    public Size CellIconSize
    {
        get => (Size) GetValue(cellIconSizeProperty);
        set => SetValue(cellIconSizeProperty, value);
    }

    public double CellIconRadius
    {
        get => (double) GetValue(cellIconRadiusProperty);
        set => SetValue(cellIconRadiusProperty, value);
    }

#endregion


#region ButtonCellSpecific

    public static readonly BindableProperty cellButtonBackgroundColorProperty =
        BindableProperty.Create(nameof(CellButtonBackgroundColor), typeof(Color), typeof(SettingsView), SvConstants.Cell.ButtonCell.background_Color);

    [TypeConverter(typeof(ColorTypeConverter))]
    public Color CellButtonBackgroundColor
    {
        get => (Color) GetValue(cellButtonBackgroundColorProperty);
        set => SetValue(cellButtonBackgroundColorProperty, value);
    }

#endregion


#region Cell Colors

    public static readonly BindableProperty cellBackgroundColorProperty = BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(SettingsView), SvConstants.Sv.background_Color);
    public static readonly BindableProperty cellOffColorProperty        = BindableProperty.Create(nameof(CellOffColor),        typeof(Color), typeof(SettingsView), SvConstants.Sv.off_Color);
    public static readonly BindableProperty cellAccentColorProperty     = BindableProperty.Create(nameof(CellAccentColor),     typeof(Color), typeof(SettingsView), SvConstants.Sv.accent_Color);

    public Color CellBackgroundColor
    {
        get => (Color) GetValue(cellBackgroundColorProperty);
        set => SetValue(cellBackgroundColorProperty, value);
    }

    public Color CellAccentColor
    {
        get => (Color) GetValue(cellAccentColorProperty);
        set => SetValue(cellAccentColorProperty, value);
    }

    public Color CellOffColor
    {
        get => (Color) GetValue(cellOffColorProperty);
        set => SetValue(cellOffColorProperty, value);
    }

#endregion


#region Scrolling

    public static readonly BindableProperty scrollToTopProperty = BindableProperty.Create(nameof(ScrollToTop),
                                                                                          typeof(bool),
                                                                                          typeof(SettingsView),
                                                                                          default(bool),
                                                                                          BindingMode.TwoWay
                                                                                         );

    public static readonly BindableProperty scrollToBottomProperty = BindableProperty.Create(nameof(ScrollToBottom),
                                                                                             typeof(bool),
                                                                                             typeof(SettingsView),
                                                                                             default(bool),
                                                                                             BindingMode.TwoWay
                                                                                            );

    public bool ScrollToBottom
    {
        get => (bool) GetValue(scrollToBottomProperty);
        set => SetValue(scrollToBottomProperty, value);
    }

    public bool ScrollToTop
    {
        get => (bool) GetValue(scrollToTopProperty);
        set => SetValue(scrollToTopProperty, value);
    }

#endregion


#region Templates

    public static readonly BindableProperty visibleContentHeightProperty = BindableProperty.Create(nameof(VisibleContentHeight),
                                                                                                   typeof(double),
                                                                                                   typeof(SettingsView),
                                                                                                   -1d,
                                                                                                   BindingMode.OneWayToSource
                                                                                                  );

    public static readonly BindableProperty itemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
                                                                                          typeof(IEnumerable),
                                                                                          typeof(SettingsView),
                                                                                          default(IEnumerable?),
                                                                                          BindingMode.OneWay,
                                                                                          propertyChanged: ItemsSourceChanged
                                                                                         );

    public static readonly BindableProperty itemTemplateProperty       = BindableProperty.Create(nameof(ItemTemplate),       typeof(DataTemplate), typeof(SettingsView));
    public static readonly BindableProperty templateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int),          typeof(SettingsView), default(int));

    public double VisibleContentHeight
    {
        get => (double) GetValue(visibleContentHeightProperty);
        set => SetValue(visibleContentHeightProperty, value);
    }

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?) GetValue(itemsSourceProperty);
        set => SetValue(itemsSourceProperty, value);
    }

    public DataTemplate? ItemTemplate
    {
        get => (DataTemplate?) GetValue(itemTemplateProperty);
        set => SetValue(itemTemplateProperty, value);
    }

    public int TemplateStartIndex
    {
        get => (int) GetValue(templateStartIndexProperty);
        set => SetValue(templateStartIndexProperty, value);
    }

#endregion


#region Android Only

    public static readonly BindableProperty useDescriptionAsValueProperty        = BindableProperty.Create(nameof(UseDescriptionAsValue),        typeof(bool), typeof(SettingsView), false);
    public static readonly BindableProperty showSectionTopBottomBorderProperty   = BindableProperty.Create(nameof(ShowSectionTopBottomBorder),   typeof(bool), typeof(SettingsView), true);
    public static readonly BindableProperty showArrowIndicatorForAndroidProperty = BindableProperty.Create(nameof(ShowArrowIndicatorForAndroid), typeof(bool), typeof(SettingsView), default(bool));

    //Only Android 
    public bool UseDescriptionAsValue
    {
        get => (bool) GetValue(useDescriptionAsValueProperty);
        set => SetValue(useDescriptionAsValueProperty, value);
    }

    //Only Android
    public bool ShowSectionTopBottomBorder
    {
        get => (bool) GetValue(showSectionTopBottomBorderProperty);
        set => SetValue(showSectionTopBottomBorderProperty, value);
    }

    //Only Android
    public bool ShowArrowIndicatorForAndroid
    {
        get => (bool) GetValue(showArrowIndicatorForAndroidProperty);
        set => SetValue(showArrowIndicatorForAndroidProperty, value);
    }

#endregion


    private int _TemplatedItemsCount { get; set; }

    private static void ItemsSourceChanged( BindableObject bindable, object oldValue, object newValue )
    {
        var settingsView = (SettingsView) bindable;

        if ( settingsView.ItemTemplate is null ) { return; }

        var oldValueAsEnumerable = oldValue as IList;
        var newValueAsEnumerable = newValue as IList;


        if ( oldValue is INotifyCollectionChanged oldObservableCollection ) { oldObservableCollection.CollectionChanged -= settingsView.OnItemsSourceCollectionChanged; }

        // keep the platform from notifying item changed event.
        settingsView.Root.CollectionChanged -= settingsView.OnCollectionChanged;

        if ( oldValueAsEnumerable is not null )
        {
            for ( int i = oldValueAsEnumerable.Count - 1; i >= 0; i-- ) { settingsView.Root.RemoveAt(settingsView.TemplateStartIndex + i); }
        }

        if ( newValueAsEnumerable is not null )
        {
            for ( var i = 0; i < newValueAsEnumerable.Count; i++ )
            {
                Section view = CreateChildViewFor(settingsView.ItemTemplate, newValueAsEnumerable[i], settingsView);
                settingsView.Root.Insert(settingsView.TemplateStartIndex + i, view);
            }

            settingsView._TemplatedItemsCount = newValueAsEnumerable.Count;
        }

        settingsView.Root.CollectionChanged += settingsView.OnCollectionChanged;

        if ( newValue is INotifyCollectionChanged newObservableCollection ) { newObservableCollection.CollectionChanged += settingsView.OnItemsSourceCollectionChanged; }


        // Notify manually ModelChanged.
        settingsView.OnModelChanged();
    }

    private void OnItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
    {
        switch ( e.Action )
        {
            case NotifyCollectionChangedAction.Replace:
            {
                //Root.RemoveAt(e.OldStartingIndex + TemplateStartIndex);

                object  item = e.NewItems[0];
                Section view = CreateChildViewFor(ItemTemplate, item, this);

                //Root.Insert(e.NewStartingIndex + TemplateStartIndex, view);
                Root[e.NewStartingIndex + TemplateStartIndex] = view;
                break;
            }

            case NotifyCollectionChangedAction.Add when e.NewItems is null: return;

            case NotifyCollectionChangedAction.Add:
            {
                for ( var i = 0; i < e.NewItems.Count; ++i )
                {
                    object  item = e.NewItems[i];
                    Section view = CreateChildViewFor(ItemTemplate, item, this);

                    Root.Insert(i + e.NewStartingIndex + TemplateStartIndex, view);
                    _TemplatedItemsCount++;
                }

                break;
            }

            case NotifyCollectionChangedAction.Remove:
            {
                if ( e.OldItems is not null )
                {
                    Root.RemoveAt(e.OldStartingIndex + TemplateStartIndex);
                    _TemplatedItemsCount--;
                }

                break;
            }

            case NotifyCollectionChangedAction.Reset:
            {
                Root.CollectionChanged -= OnCollectionChanged;
                // var source = ItemsSource as IList;
                for ( int i = _TemplatedItemsCount - 1; i >= 0; i-- ) { Root.RemoveAt(TemplateStartIndex + i); }

                Root.CollectionChanged += OnCollectionChanged;
                _TemplatedItemsCount   =  0;
                OnModelChanged();
                break;
            }

            case NotifyCollectionChangedAction.Move: break;

            default:
                return;
        }
    }

    internal void SendItemDropped( Section section, Cell cell )
    {
        var eventArgs = new DropEventArgs(section, cell);
        ItemDropped?.Invoke(this, eventArgs);
        ItemDroppedCommand?.Execute(eventArgs);
    }

    private static Section CreateChildViewFor( DataTemplate? template, object item, BindableObject container )
    {
        if ( template is DataTemplateSelector selector ) { template = selector.SelectTemplate(item, container); }

        if ( template is null ) throw new NullReferenceException(nameof(template));

        template.SetValue(BindingContextProperty, item); //Binding context

        return (Section) template.CreateContent();
    }
}