using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Converters;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;
using DropEventArgs = Jakar.SettingsView.Shared.Events.DropEventArgs;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public partial class SettingsView
	{
		internal void ParentOnPropertyChanged( object sender, PropertyChangedEventArgs e ) { OnPropertyChanged(e.PropertyName); }

		public event EventHandler<DropEventArgs>? ItemDropped;

		public static BindableProperty ItemDroppedCommandProperty = BindableProperty.Create(nameof(ItemDroppedCommand), typeof(ICommand), typeof(SettingsView), default(ICommand));

		public ICommand ItemDroppedCommand
		{
			get => (ICommand) GetValue(ItemDroppedCommandProperty);
			set => SetValue(ItemDroppedCommandProperty, value);
		}


	#region Popups

		public static BindableProperty PopupCfgProperty = BindableProperty.Create(nameof(Popup),
																				  typeof(CellPopupConfig),
																				  typeof(SettingsView),
																				  new CellPopupConfig(),
																				  propertyChanging: PopupCfgPropertyChanging
																				 );

		private static void PopupCfgPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			if ( oldValue is CellPopupConfig old )
			{
				old.Parent = null;
				old.PropertyChanged -= Config_OnPropertyChanged;
			}

			if ( newValue is not CellPopupConfig current ) return;
			current.Parent = (SettingsView) bindable;
			current.PropertyChanged -= Config_OnPropertyChanged;
		}
		private static void Config_OnPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch ( sender )
			{
				case SettingsView view:
					view.OnPropertyChanged(e.PropertyName);
					break;

				case CellPopupConfig cfg:
					cfg.Parent?.OnPropertyChanged(e.PropertyName);
					break;
			}
		}

		public CellPopupConfig Popup
		{
			get => (CellPopupConfig) GetValue(PopupCfgProperty);
			set => SetValue(PopupCfgProperty, value);
		}

	#endregion


	#region SettingsView Colors

		public static BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(SettingsView), SVConstants.SV.SEPARATOR_COLOR);
		public static BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(SettingsView), SVConstants.Prompt.Selected.TEXT_COLOR);
		public new static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SettingsView), SVConstants.SV.BACKGROUND_COLOR);


		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public new Color BackgroundColor
		{
			get => (Color) GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color SeparatorColor
		{
			get => (Color) GetValue(SeparatorColorProperty);
			set => SetValue(SeparatorColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color SelectedColor
		{
			get => (Color) GetValue(SelectedColorProperty);
			set => SetValue(SelectedColorProperty, value);
		}

	#endregion

	#region Header

		public static BindableProperty HeaderPaddingProperty = BindableProperty.Create(nameof(HeaderPadding), typeof(Thickness), typeof(SettingsView), SVConstants.Section.Header.PADDING);

		public static BindableProperty HeaderTextColorProperty = BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(SettingsView), SVConstants.Section.Header.TEXT_COLOR);

		public static BindableProperty HeaderFontSizeProperty = BindableProperty.Create(nameof(HeaderFontSize),
																						typeof(double),
																						typeof(SettingsView),
																						SVConstants.Section.Header.FONT_SIZE,
																						defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView) bindable)
																					   );

		public static BindableProperty HeaderFontFamilyProperty = BindableProperty.Create(nameof(HeaderFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty HeaderFontAttributesProperty = BindableProperty.Create(nameof(HeaderFontAttributes), typeof(FontAttributes), typeof(SettingsView), SVConstants.Section.Header.FONT_ATTRIBUTES);

		// public static BindableProperty HeaderTextVerticalAlignProperty = BindableProperty.Create(nameof(HeaderTextVerticalAlign), typeof(LayoutAlignment), typeof(SettingsView), LayoutAlignment.End);

		public static BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(SettingsView), SVConstants.Section.Header.BACKGROUND_COLOR);

		public static BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(double), typeof(SettingsView), SVConstants.Section.Header.MinRowHeight);


		// TODO: decide what to do with these: remove / re-purpose / implement
		public Thickness HeaderPadding
		{
			get => (Thickness) GetValue(HeaderPaddingProperty);
			set => SetValue(HeaderPaddingProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color HeaderTextColor
		{
			get => (Color) GetValue(HeaderTextColorProperty);
			set => SetValue(HeaderTextColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
		public double HeaderFontSize
		{
			get => (double) GetValue(HeaderFontSizeProperty);
			set => SetValue(HeaderFontSizeProperty, value);
		}

		public string? HeaderFontFamily
		{
			get => (string?) GetValue(HeaderFontFamilyProperty);
			set => SetValue(HeaderFontFamilyProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes HeaderFontAttributes
		{
			get => (FontAttributes) GetValue(HeaderFontAttributesProperty);
			set => SetValue(HeaderFontAttributesProperty, value);
		}

		// public LayoutAlignment HeaderTextVerticalAlign
		// {
		// 	get => (LayoutAlignment) GetValue(HeaderTextVerticalAlignProperty);
		// 	set => SetValue(HeaderTextVerticalAlignProperty, value);
		// }

		public Color HeaderBackgroundColor
		{
			get => (Color) GetValue(HeaderBackgroundColorProperty);
			set => SetValue(HeaderBackgroundColorProperty, value);
		}

		public double HeaderHeight
		{
			get => (double) GetValue(HeaderHeightProperty);
			set => SetValue(HeaderHeightProperty, value);
		}

	#endregion

	#region Footer

		public static BindableProperty FooterTextColorProperty = BindableProperty.Create(nameof(FooterTextColor), typeof(Color), typeof(SettingsView), SVConstants.Section.Footer.TEXT_COLOR);

		public static BindableProperty FooterFontSizeProperty = BindableProperty.Create(nameof(FooterFontSize),
																						typeof(double),
																						typeof(SettingsView),
																						SVConstants.Section.Footer.FONT_SIZE,
																						BindingMode.OneWay,
																						defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView) bindable)
																					   );

		public static BindableProperty FooterFontFamilyProperty = BindableProperty.Create(nameof(FooterFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty FooterFontAttributesProperty = BindableProperty.Create(nameof(FooterFontAttributes), typeof(FontAttributes), typeof(SettingsView), SVConstants.Section.Footer.FONT_ATTRIBUTES);

		public static BindableProperty FooterBackgroundColorProperty = BindableProperty.Create(nameof(FooterBackgroundColor), typeof(Color), typeof(SettingsView), SVConstants.Section.Footer.BACKGROUND_COLOR);

		public static BindableProperty FooterPaddingProperty = BindableProperty.Create(nameof(FooterPadding), typeof(Thickness), typeof(SettingsView), SVConstants.Section.Footer.PADDING);


		// TODO: decide what to do with these: remove / re-purpose / implement
		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color FooterTextColor
		{
			get => (Color) GetValue(FooterTextColorProperty);
			set => SetValue(FooterTextColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
		public double FooterFontSize
		{
			get => (double) GetValue(FooterFontSizeProperty);
			set => SetValue(FooterFontSizeProperty, value);
		}

		public string? FooterFontFamily
		{
			get => (string?) GetValue(FooterFontFamilyProperty);
			set => SetValue(FooterFontFamilyProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes FooterFontAttributes
		{
			get => (FontAttributes) GetValue(FooterFontAttributesProperty);
			set => SetValue(FooterFontAttributesProperty, value);
		}

		public Color FooterBackgroundColor
		{
			get => (Color) GetValue(FooterBackgroundColorProperty);
			set => SetValue(FooterBackgroundColorProperty, value);
		}

		public Thickness FooterPadding
		{
			get => (Thickness) GetValue(FooterPaddingProperty);
			set => SetValue(FooterPaddingProperty, value);
		}

	#endregion

	#region Cell Title

		public static BindableProperty CellTitleColorProperty = BindableProperty.Create(nameof(CellTitleColor), typeof(Color), typeof(SettingsView), SVConstants.SV.Title.TEXT_COLOR);

		public static BindableProperty CellTitleFontSizeProperty = BindableProperty.Create(nameof(CellTitleFontSize),
																						   typeof(double),
																						   typeof(SettingsView),
																						   SVConstants.SV.Title.Font.Size,
																						   BindingMode.OneWay,
																						   defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, (SettingsView) bindable)
																						  );

		public static BindableProperty CellTitleFontFamilyProperty = BindableProperty.Create(nameof(CellTitleFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty CellTitleFontAttributesProperty = BindableProperty.Create(nameof(CellTitleFontAttributes), typeof(FontAttributes), typeof(SettingsView), SVConstants.SV.Title.Font.Attributes);
		public static BindableProperty CellTitleAlignmentProperty = BindableProperty.Create(nameof(CellTitleAlignment), typeof(TextAlignment), typeof(SettingsView), SVConstants.SV.Title.Alignment);

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color CellTitleColor
		{
			get => (Color) GetValue(CellTitleColorProperty);
			set => SetValue(CellTitleColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
		public double CellTitleFontSize
		{
			get => (double) GetValue(CellTitleFontSizeProperty);
			set => SetValue(CellTitleFontSizeProperty, value);
		}

		public string? CellTitleFontFamily
		{
			get => (string?) GetValue(CellTitleFontFamilyProperty);
			set => SetValue(CellTitleFontFamilyProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes CellTitleFontAttributes
		{
			get => (FontAttributes) GetValue(CellTitleFontAttributesProperty);
			set => SetValue(CellTitleFontAttributesProperty, value);
		}

		public TextAlignment CellTitleAlignment
		{
			get => (TextAlignment) GetValue(CellTitleAlignmentProperty);
			set => SetValue(CellTitleAlignmentProperty, value);
		}

	#endregion

	#region Cell Value

		public static BindableProperty CellValueTextColorProperty = BindableProperty.Create(nameof(CellValueTextColor), typeof(Color), typeof(SettingsView), SVConstants.SV.Value.TEXT_COLOR);

		public static BindableProperty CellValueTextFontSizeProperty = BindableProperty.Create(nameof(CellValueTextFontSize), typeof(double), typeof(SettingsView), SVConstants.SV.Value.Font.Size);

		public static BindableProperty CellValueTextFontFamilyProperty = BindableProperty.Create(nameof(CellValueTextFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty CellValueTextFontAttributesProperty = BindableProperty.Create(nameof(CellValueTextFontAttributes), typeof(FontAttributes), typeof(SettingsView), SVConstants.SV.Value.Font.Attributes);
		public static BindableProperty CellValueTextAlignmentProperty = BindableProperty.Create(nameof(CellValueTextAlignment), typeof(TextAlignment), typeof(SettingsView), SVConstants.SV.Value.Alignment);

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color CellValueTextColor
		{
			get => (Color) GetValue(CellValueTextColorProperty);
			set => SetValue(CellValueTextColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
		public double CellValueTextFontSize
		{
			get => (double) GetValue(CellValueTextFontSizeProperty);
			set => SetValue(CellValueTextFontSizeProperty, value);
		}

		public string? CellValueTextFontFamily
		{
			get => (string?) GetValue(CellValueTextFontFamilyProperty);
			set => SetValue(CellValueTextFontFamilyProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes CellValueTextFontAttributes
		{
			get => (FontAttributes) GetValue(CellValueTextFontAttributesProperty);
			set => SetValue(CellValueTextFontAttributesProperty, value);
		}

		public TextAlignment CellValueTextAlignment
		{
			get => (TextAlignment) GetValue(CellValueTextAlignmentProperty);
			set => SetValue(CellValueTextAlignmentProperty, value);
		}

	#endregion

	#region Cell Description

		public static BindableProperty CellDescriptionColorProperty = BindableProperty.Create(nameof(CellDescriptionColor), typeof(Color), typeof(SettingsView), SVConstants.SV.Description.TEXT_COLOR);

		public static BindableProperty CellDescriptionFontSizeProperty = BindableProperty.Create(nameof(CellDescriptionFontSize), typeof(double), typeof(SettingsView), SVConstants.SV.Description.Font.Size);

		public static BindableProperty CellDescriptionFontFamilyProperty = BindableProperty.Create(nameof(CellDescriptionFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty CellDescriptionFontAttributesProperty = BindableProperty.Create(nameof(CellDescriptionFontAttributes), typeof(FontAttributes), typeof(SettingsView), SVConstants.SV.Description.Font.Attributes);
		public static BindableProperty CellDescriptionAlignmentProperty = BindableProperty.Create(nameof(CellDescriptionAlignment), typeof(TextAlignment), typeof(SettingsView), SVConstants.SV.Description.Alignment);

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color CellDescriptionColor
		{
			get => (Color) GetValue(CellDescriptionColorProperty);
			set => SetValue(CellDescriptionColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
		public double CellDescriptionFontSize
		{
			get => (double) GetValue(CellDescriptionFontSizeProperty);
			set => SetValue(CellDescriptionFontSizeProperty, value);
		}

		public string? CellDescriptionFontFamily
		{
			get => (string?) GetValue(CellDescriptionFontFamilyProperty);
			set => SetValue(CellDescriptionFontFamilyProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes CellDescriptionFontAttributes
		{
			get => (FontAttributes) GetValue(CellDescriptionFontAttributesProperty);
			set => SetValue(CellDescriptionFontAttributesProperty, value);
		}

		public TextAlignment CellDescriptionAlignment
		{
			get => (TextAlignment) GetValue(CellDescriptionAlignmentProperty);
			set => SetValue(CellDescriptionAlignmentProperty, value);
		}

	#endregion

	#region Cell Hint

		public static BindableProperty CellHintTextColorProperty = BindableProperty.Create(nameof(CellHintTextColor), typeof(Color), typeof(SettingsView), SVConstants.SV.Hint.TEXT_COLOR);
		public static BindableProperty CellHintFontSizeProperty = BindableProperty.Create(nameof(CellHintFontSize), typeof(double), typeof(SettingsView), SVConstants.SV.Hint.Font.Size);
		public static BindableProperty CellHintFontFamilyProperty = BindableProperty.Create(nameof(CellHintFontFamily), typeof(string), typeof(SettingsView), default(string?));
		public static BindableProperty CellHintFontAttributesProperty = BindableProperty.Create(nameof(CellHintFontAttributes), typeof(FontAttributes), typeof(SettingsView), SVConstants.SV.Hint.Font.Attributes);
		public static BindableProperty CellHintAlignmentProperty = BindableProperty.Create(nameof(CellHintAlignment), typeof(TextAlignment), typeof(SettingsView), SVConstants.SV.Hint.Alignment);

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color CellHintTextColor
		{
			get => (Color) GetValue(CellHintTextColorProperty);
			set => SetValue(CellHintTextColorProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontSizeConverter))]
		public double CellHintFontSize
		{
			get => (double) GetValue(CellHintFontSizeProperty);
			set => SetValue(CellHintFontSizeProperty, value);
		}

		public string? CellHintFontFamily
		{
			get => (string?) GetValue(CellHintFontFamilyProperty);
			set => SetValue(CellHintFontFamilyProperty, value);
		}

		[Xamarin.Forms.TypeConverter(typeof(FontAttributesConverter))]
		public FontAttributes CellHintFontAttributes
		{
			get => (FontAttributes) GetValue(CellHintFontAttributesProperty);
			set => SetValue(CellHintFontAttributesProperty, value);
		}

		public TextAlignment CellHintAlignment
		{
			get => (TextAlignment) GetValue(CellHintAlignmentProperty);
			set => SetValue(CellHintAlignmentProperty, value);
		}

	#endregion

	#region Cell Icon

		public static BindableProperty CellIconSizeProperty = BindableProperty.Create(nameof(CellIconSize), typeof(Size), typeof(SettingsView), SVConstants.SV.Icon.Size);
		public static BindableProperty CellIconRadiusProperty = BindableProperty.Create(nameof(CellIconRadius), typeof(double), typeof(SettingsView), SVConstants.SV.Icon.Radius);


		[Xamarin.Forms.TypeConverter(typeof(SizeConverter))]
		public Size CellIconSize
		{
			get => (Size) GetValue(CellIconSizeProperty);
			set => SetValue(CellIconSizeProperty, value);
		}

		public double CellIconRadius
		{
			get => (double) GetValue(CellIconRadiusProperty);
			set => SetValue(CellIconRadiusProperty, value);
		}

	#endregion

	#region ButtonCellSpecific

		public static BindableProperty CellButtonBackgroundColorProperty = BindableProperty.Create(nameof(CellButtonBackgroundColor), typeof(Color), typeof(SettingsView), SVConstants.Cell.ButtonCell.BACKGROUND_COLOR);

		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public Color CellButtonBackgroundColor
		{
			get => (Color) GetValue(CellButtonBackgroundColorProperty);
			set => SetValue(CellButtonBackgroundColorProperty, value);
		}

	#endregion

	#region Cell Colors

		public static BindableProperty CellBackgroundColorProperty = BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(SettingsView), SVConstants.SV.BACKGROUND_COLOR);
		public static BindableProperty CellOffColorProperty = BindableProperty.Create(nameof(CellOffColor), typeof(Color), typeof(SettingsView), SVConstants.SV.OFF_COLOR);
		public static BindableProperty CellAccentColorProperty = BindableProperty.Create(nameof(CellAccentColor), typeof(Color), typeof(SettingsView), SVConstants.SV.ACCENT_COLOR);

		public Color CellBackgroundColor
		{
			get => (Color) GetValue(CellBackgroundColorProperty);
			set => SetValue(CellBackgroundColorProperty, value);
		}

		public Color CellAccentColor
		{
			get => (Color) GetValue(CellAccentColorProperty);
			set => SetValue(CellAccentColorProperty, value);
		}

		public Color CellOffColor
		{
			get => (Color) GetValue(CellOffColorProperty);
			set => SetValue(CellOffColorProperty, value);
		}

	#endregion

	#region Scrolling

		public static BindableProperty ScrollToTopProperty = BindableProperty.Create(nameof(ScrollToTop),
																					 typeof(bool),
																					 typeof(SettingsView),
																					 default(bool),
																					 BindingMode.TwoWay
																					);

		public static BindableProperty ScrollToBottomProperty = BindableProperty.Create(nameof(ScrollToBottom),
																						typeof(bool),
																						typeof(SettingsView),
																						default(bool),
																						BindingMode.TwoWay
																					   );

		public bool ScrollToBottom
		{
			get => (bool) GetValue(ScrollToBottomProperty);
			set => SetValue(ScrollToBottomProperty, value);
		}

		public bool ScrollToTop
		{
			get => (bool) GetValue(ScrollToTopProperty);
			set => SetValue(ScrollToTopProperty, value);
		}

	#endregion

	#region Templates

		public static BindableProperty VisibleContentHeightProperty = BindableProperty.Create(nameof(VisibleContentHeight),
																							  typeof(double),
																							  typeof(SettingsView),
																							  -1d,
																							  BindingMode.OneWayToSource
																							 );

		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																					 typeof(IEnumerable),
																					 typeof(SettingsView),
																					 default(IEnumerable?),
																					 BindingMode.OneWay,
																					 propertyChanged: ItemsSourceChanged
																					);

		public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SettingsView), default(DataTemplate?));
		public static BindableProperty TemplateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int), typeof(SettingsView), default(int));

		public double VisibleContentHeight
		{
			get => (double) GetValue(VisibleContentHeightProperty);
			set => SetValue(VisibleContentHeightProperty, value);
		}

		public IEnumerable? ItemsSource
		{
			get => (IEnumerable?) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public DataTemplate? ItemTemplate
		{
			get => (DataTemplate?) GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}

		public int TemplateStartIndex
		{
			get => (int) GetValue(TemplateStartIndexProperty);
			set => SetValue(TemplateStartIndexProperty, value);
		}

	#endregion

	#region Android Only

		public static BindableProperty UseDescriptionAsValueProperty = BindableProperty.Create(nameof(UseDescriptionAsValue), typeof(bool), typeof(SettingsView), false);
		public static BindableProperty ShowSectionTopBottomBorderProperty = BindableProperty.Create(nameof(ShowSectionTopBottomBorder), typeof(bool), typeof(SettingsView), true);
		public static BindableProperty ShowArrowIndicatorForAndroidProperty = BindableProperty.Create(nameof(ShowArrowIndicatorForAndroid), typeof(bool), typeof(SettingsView), default(bool));

		//Only Android 
		public bool UseDescriptionAsValue
		{
			get => (bool) GetValue(UseDescriptionAsValueProperty);
			set => SetValue(UseDescriptionAsValueProperty, value);
		}

		//Only Android
		public bool ShowSectionTopBottomBorder
		{
			get => (bool) GetValue(ShowSectionTopBottomBorderProperty);
			set => SetValue(ShowSectionTopBottomBorderProperty, value);
		}

		//Only Android
		public bool ShowArrowIndicatorForAndroid
		{
			get => (bool) GetValue(ShowArrowIndicatorForAndroidProperty);
			set => SetValue(ShowArrowIndicatorForAndroidProperty, value);
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

					object item = e.NewItems[0];
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
						object item = e.NewItems[i];
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
					var source = ItemsSource as IList;
					for ( int i = _TemplatedItemsCount - 1; i >= 0; i-- ) { Root.RemoveAt(TemplateStartIndex + i); }

					Root.CollectionChanged += OnCollectionChanged;
					_TemplatedItemsCount = 0;
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
}