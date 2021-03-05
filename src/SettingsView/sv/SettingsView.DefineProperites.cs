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
				old.Parent = null;
			if ( newValue is CellPopupConfig current )
				current.Parent = (SettingsView) bindable;
		}

		public CellPopupConfig Popup
		{
			get => (CellPopupConfig) GetValue(PopupCfgProperty);
			set => SetValue(PopupCfgProperty, value);
		}

	#endregion


	#region SettingsView Colors

		public static BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(SettingsView), Color.FromRgb(199, 199, 204));

		public static BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(SettingsView), Color.Default);

		public new static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SettingsView), Color.Default);


		public new Color BackgroundColor
		{
			get => (Color) GetValue(BackgroundColorProperty);
			set => SetValue(BackgroundColorProperty, value);
		}

		public Color SeparatorColor
		{
			get => (Color) GetValue(SeparatorColorProperty);
			set => SetValue(SeparatorColorProperty, value);
		}

		public Color SelectedColor
		{
			get => (Color) GetValue(SelectedColorProperty);
			set => SetValue(SelectedColorProperty, value);
		}

	#endregion

	#region Header

		public static BindableProperty HeaderPaddingProperty = BindableProperty.Create(nameof(HeaderPadding), typeof(Thickness), typeof(SettingsView), new Thickness(14, 8, 8, 8));

		public static BindableProperty HeaderTextColorProperty = BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty HeaderFontSizeProperty = BindableProperty.Create(nameof(HeaderFontSize),
																						typeof(double),
																						typeof(SettingsView),
																						-1.0d,
																						defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView) bindable)
																					   );

		public static BindableProperty HeaderFontFamilyProperty = BindableProperty.Create(nameof(HeaderFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty HeaderFontAttributesProperty = BindableProperty.Create(nameof(HeaderFontAttributes), typeof(FontAttributes), typeof(SettingsView), FontAttributes.None);

		public static BindableProperty HeaderTextVerticalAlignProperty = BindableProperty.Create(nameof(HeaderTextVerticalAlign), typeof(LayoutAlignment), typeof(SettingsView), LayoutAlignment.End);

		public static BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(double), typeof(SettingsView), -1d);

		public Thickness HeaderPadding
		{
			get => (Thickness) GetValue(HeaderPaddingProperty);
			set => SetValue(HeaderPaddingProperty, value);
		}

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

		public FontAttributes HeaderFontAttributes
		{
			get => (FontAttributes) GetValue(HeaderFontAttributesProperty);
			set => SetValue(HeaderFontAttributesProperty, value);
		}

		public LayoutAlignment HeaderTextVerticalAlign
		{
			get => (LayoutAlignment) GetValue(HeaderTextVerticalAlignProperty);
			set => SetValue(HeaderTextVerticalAlignProperty, value);
		}

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

		public static BindableProperty FooterTextColorProperty = BindableProperty.Create(nameof(FooterTextColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty FooterFontSizeProperty = BindableProperty.Create(nameof(FooterFontSize),
																						typeof(double),
																						typeof(SettingsView),
																						-1.0d,
																						BindingMode.OneWay,
																						defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Small, (SettingsView) bindable)
																					   );

		public static BindableProperty FooterFontFamilyProperty = BindableProperty.Create(nameof(FooterFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty FooterFontAttributesProperty = BindableProperty.Create(nameof(FooterFontAttributes), typeof(FontAttributes), typeof(SettingsView), FontAttributes.None);

		public static BindableProperty FooterBackgroundColorProperty = BindableProperty.Create(nameof(FooterBackgroundColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty FooterPaddingProperty = BindableProperty.Create(nameof(FooterPadding), typeof(Thickness), typeof(SettingsView), new Thickness(14, 8, 14, 8));

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

		public static BindableProperty CellTitleColorProperty = BindableProperty.Create(nameof(CellTitleColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty CellTitleFontSizeProperty = BindableProperty.Create(nameof(CellTitleFontSize),
																						   typeof(double),
																						   typeof(SettingsView),
																						   -1.0,
																						   BindingMode.OneWay,
																						   defaultValueCreator: bindable => Device.GetNamedSize(NamedSize.Default, (SettingsView) bindable)
																						  );

		public static BindableProperty CellTitleFontFamilyProperty = BindableProperty.Create(nameof(CellTitleFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty CellTitleFontAttributesProperty = BindableProperty.Create(nameof(CellTitleFontAttributes), typeof(FontAttributes), typeof(SettingsView), FontAttributes.None);
		public static BindableProperty CellTitleAlignmentProperty = BindableProperty.Create(nameof(CellTitleAlignment), typeof(TextAlignment), typeof(SettingsView), TextAlignment.Start);

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

		public static BindableProperty CellValueTextColorProperty = BindableProperty.Create(nameof(CellValueTextColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty CellValueTextFontSizeProperty = BindableProperty.Create(nameof(CellValueTextFontSize), typeof(double), typeof(SettingsView), -1.0d);

		public static BindableProperty CellValueTextFontFamilyProperty = BindableProperty.Create(nameof(CellValueTextFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty CellValueTextFontAttributesProperty = BindableProperty.Create(nameof(CellValueTextFontAttributes), typeof(FontAttributes), typeof(SettingsView), FontAttributes.None);
		public static BindableProperty CellValueTextAlignmentProperty = BindableProperty.Create(nameof(CellValueTextAlignment), typeof(TextAlignment), typeof(SettingsView), TextAlignment.End);

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

		public static BindableProperty CellDescriptionColorProperty = BindableProperty.Create(nameof(CellDescriptionColor), typeof(Color), typeof(SettingsView), Color.Default);

		public static BindableProperty CellDescriptionFontSizeProperty = BindableProperty.Create(nameof(CellDescriptionFontSize), typeof(double), typeof(SettingsView), -1.0d);

		public static BindableProperty CellDescriptionFontFamilyProperty = BindableProperty.Create(nameof(CellDescriptionFontFamily), typeof(string), typeof(SettingsView), default(string?));

		public static BindableProperty CellDescriptionFontAttributesProperty = BindableProperty.Create(nameof(CellDescriptionFontAttributes), typeof(FontAttributes), typeof(SettingsView), FontAttributes.None);
		public static BindableProperty CellDescriptionAlignmentProperty = BindableProperty.Create(nameof(CellDescriptionAlignment), typeof(TextAlignment), typeof(SettingsView), TextAlignment.Start);

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

		public static BindableProperty CellHintTextColorProperty = BindableProperty.Create(nameof(CellHintTextColor), typeof(Color), typeof(SettingsView), Color.Red);
		public static BindableProperty CellHintFontSizeProperty = BindableProperty.Create(nameof(CellHintFontSize), typeof(double), typeof(SettingsView), 10.0d);
		public static BindableProperty CellHintFontFamilyProperty = BindableProperty.Create(nameof(CellHintFontFamily), typeof(string), typeof(SettingsView), default(string));
		public static BindableProperty CellHintFontAttributesProperty = BindableProperty.Create(nameof(CellHintFontAttributes), typeof(FontAttributes), typeof(SettingsView), FontAttributes.None);
		public static BindableProperty CellHintAlignmentProperty = BindableProperty.Create(nameof(CellHintAlignment), typeof(TextAlignment), typeof(SettingsView), TextAlignment.End);

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

		public string CellHintFontFamily
		{
			get => (string) GetValue(CellHintFontFamilyProperty);
			set => SetValue(CellHintFontFamilyProperty, value);
		}

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

		public static Size DefaultIconSize => new Size(36, 36);

		public static BindableProperty CellIconSizeProperty = BindableProperty.Create(nameof(CellIconSize), typeof(Size), typeof(SettingsView), default(Size?));

		public static BindableProperty CellIconRadiusProperty = BindableProperty.Create(nameof(CellIconRadius), typeof(double), typeof(SettingsView), 6.0d);

		[Xamarin.Forms.TypeConverter(typeof(SizeConverter))]
		public Size? CellIconSize
		{
			get => (Size?) GetValue(CellIconSizeProperty);
			set => SetValue(CellIconSizeProperty, value);
		}

		public double CellIconRadius
		{
			get => (double) GetValue(CellIconRadiusProperty);
			set => SetValue(CellIconRadiusProperty, value);
		}

	#endregion

	#region Cell Colors

		public static readonly Color DEFAULT_ACCENT_COLOR = Color.Accent;
		public static readonly Color DEFAULT_OFF_COLOR = Color.FromRgba(117, 117, 117, 76);


		public static BindableProperty CellBackgroundColorProperty = BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(SettingsView), Color.Default);

		public Color CellBackgroundColor
		{
			get => (Color) GetValue(CellBackgroundColorProperty);
			set => SetValue(CellBackgroundColorProperty, value);
		}


		public static BindableProperty CellAccentColorProperty = BindableProperty.Create(nameof(CellAccentColor), typeof(Color), typeof(SettingsView), DEFAULT_ACCENT_COLOR);


		public Color CellAccentColor
		{
			get => (Color) GetValue(CellAccentColorProperty);
			set => SetValue(CellAccentColorProperty, value);
		}

		public static BindableProperty CellOffColorProperty = BindableProperty.Create(nameof(CellOffColor), typeof(Color), typeof(SettingsView), DEFAULT_OFF_COLOR);

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

		public double VisibleContentHeight
		{
			get => (double) GetValue(VisibleContentHeightProperty);
			set => SetValue(VisibleContentHeightProperty, value);
		}


		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																					 typeof(IEnumerable),
																					 typeof(SettingsView),
																					 default(IEnumerable?),
																					 BindingMode.OneWay,
																					 propertyChanged: ItemsSourceChanged
																					);

		public IEnumerable? ItemsSource
		{
			get => (IEnumerable?) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}


		public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SettingsView), default(DataTemplate?));

		public DataTemplate? ItemTemplate
		{
			get => (DataTemplate?) GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}


		public static BindableProperty TemplateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int), typeof(SettingsView), default(int));

		public int TemplateStartIndex
		{
			get => (int) GetValue(TemplateStartIndexProperty);
			set => SetValue(TemplateStartIndexProperty, value);
		}

	#endregion

	#region Android Only

		//Only Android 
		public static BindableProperty UseDescriptionAsValueProperty = BindableProperty.Create(nameof(UseDescriptionAsValue), typeof(bool), typeof(SettingsView), false);

		public bool UseDescriptionAsValue
		{
			get => (bool) GetValue(UseDescriptionAsValueProperty);
			set => SetValue(UseDescriptionAsValueProperty, value);
		}

		//Only Android
		public static BindableProperty ShowSectionTopBottomBorderProperty = BindableProperty.Create(nameof(ShowSectionTopBottomBorder), typeof(bool), typeof(SettingsView), true);

		public bool ShowSectionTopBottomBorder
		{
			get => (bool) GetValue(ShowSectionTopBottomBorderProperty);
			set => SetValue(ShowSectionTopBottomBorderProperty, value);
		}

		//Only Android
		public static BindableProperty ShowArrowIndicatorForAndroidProperty = BindableProperty.Create(nameof(ShowArrowIndicatorForAndroid), typeof(bool), typeof(SettingsView), default(bool));

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