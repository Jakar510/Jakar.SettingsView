using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Jakar.SettingsView.Shared.Config;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	public class Section : TableSectionBase<Cell>
	{
		public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(Section), default(DataTemplate));
		public static BindableProperty UseDragSortProperty = BindableProperty.Create(nameof(UseDragSort), typeof(bool), typeof(Section), default(bool));
		public static BindableProperty TemplateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int), typeof(Section), default(int));
		public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(Section), true);

		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																					 typeof(IList),
																					 typeof(Section),
																					 default(IList?),
																					 BindingMode.OneWay,
																					 propertyChanged: ItemsChanged
																					);

		public static BindableProperty HeaderViewProperty = BindableProperty.Create(nameof(HeaderView),
																					typeof(HeaderView),
																					typeof(Section),
																					new DefaultHeaderView(),
																					propertyChanging: HeaderViewPropertyChanging
																				   );

		private static void HeaderViewPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			if ( bindable is not Section section ) return;
			if ( oldValue is HeaderView oldHeader ) { oldHeader.Section = null; }

			if ( newValue is HeaderView newHeader ) { newHeader.Section = section; }
		}

		public static BindableProperty FooterViewProperty = BindableProperty.Create(nameof(FooterView),
																					typeof(FooterView),
																					typeof(Section),
																					new DefaultFooterView(),
																					propertyChanging: FooterViewPropertyChanging
																				   );

		private static void FooterViewPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			if ( bindable is not Section section )
				return;
			if ( oldValue is FooterView oldFooter ) { oldFooter.Section = null; }

			if ( newValue is FooterView newFooter ) { newFooter.Section = section; }
		}

		public new static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
																							typeof(string),
																							typeof(BaseHeaderFooterView),
																							default(string?),
																							propertyChanged: ( bindable, value, newValue ) =>
																											 {
																												 if ( bindable is Section section )
																												 {
																													 section.HeaderView.Title = newValue?.ToString();
																												 }
																											 }
																						   );

		public new static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
																								typeof(Color),
																								typeof(BaseHeaderFooterView),
																								SVConstants.HEADER_TITLE_COLOR,
																								propertyChanged: ( bindable, value, newValue ) =>
																												 {
																													 if ( bindable is Section section ) { section.HeaderView.TitleColor = (Color) newValue; }
																												 }
																							   );


		public static BindableProperty FooterTextProperty = BindableProperty.Create(nameof(FooterText),
																					typeof(string),
																					typeof(Section),
																					default(string),
																					propertyChanged: ( bindable, value, newValue ) =>
																									 {
																										 if ( bindable is Section section ) { section.FooterView.Title = newValue?.ToString(); }
																									 }
																				   );

		public static BindableProperty FooterVisibleProperty = BindableProperty.Create(nameof(FooterVisible), typeof(bool), typeof(Section), true);


		public new string? Title
		{
			get => (string?) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		public new Color TextColor
		{
			get => (Color) GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		public HeaderView HeaderView
		{
			get => (HeaderView) GetValue(HeaderViewProperty);
			set => SetValue(HeaderViewProperty, value);
		}

		public FooterView FooterView
		{
			get => (FooterView) GetValue(FooterViewProperty);
			set => SetValue(FooterViewProperty, value);
		}

		public string? FooterText
		{
			get => (string?) GetValue(FooterTextProperty);
			set => SetValue(FooterTextProperty, value);
		}

		public bool FooterVisible
		{
			get => (bool) GetValue(FooterVisibleProperty);
			set => SetValue(FooterVisibleProperty, value);
		}


		public bool IsVisible
		{
			get => (bool) GetValue(IsVisibleProperty);
			set => SetValue(IsVisibleProperty, value);
		}


		public DataTemplate? ItemTemplate
		{
			get => (DataTemplate?) GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}

		public IList? ItemsSource
		{
			get => (IList?) GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public bool UseDragSort
		{
			get => (bool) GetValue(UseDragSortProperty);
			set => SetValue(UseDragSortProperty, value);
		}

		public int TemplateStartIndex
		{
			get => (int) GetValue(TemplateStartIndexProperty);
			set => SetValue(TemplateStartIndexProperty, value);
		}

		public SettingsView? Parent { get; set; }

		private int templatedItemsCount;

		public bool IsCollapsible
		{
			get => HeaderView.IsCollapsible;
			set => HeaderView.IsCollapsible = value;
		}

		public bool IsCollapsed
		{
			get => HeaderView.IsCollapsed;
			set => HeaderView.IsCollapsed = value;
		}

		internal List<Cell> Cache { get; private set; }


		public Section()
		{
			CollectionChanged += OnCollectionChanged;
			PropertyChanged += OnPropertyChanged;
			Cache = new List<Cell>();
		}
		public Section( string title ) : this() => Title = title;
		public Section( SettingsView settingsView ) : this(settingsView, string.Empty) { }
		public Section( SettingsView settingsView, string? title ) : this()
		{
			Title = title;
			Parent = settingsView;
		}
		public Section( Cell cell ) : this() { Add(cell); }
		public Section( IEnumerable<Cell> cells ) : this() { Add(cells); }
		public Section( SettingsView settingsView, Cell cell ) : this(settingsView) { Add(cell); }
		public Section( SettingsView settingsView, IEnumerable<Cell> cells ) : this(settingsView) { Add(cells); }
		public Section( SettingsView settingsView, string? title, Cell cell ) : this(settingsView, title) { Add(cell); }
		public Section( SettingsView settingsView, string? title, IEnumerable<Cell> cells ) : this(settingsView, title) { Add(cells); }


		public event NotifyCollectionChangedEventHandler? SectionCollectionChanged;

		public event PropertyChangedEventHandler? SectionPropertyChanged;

		private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { SectionCollectionChanged?.Invoke(this, e); }
		private void OnPropertyChanged( object sender, PropertyChangedEventArgs e ) { SectionPropertyChanged?.Invoke(this, e); }

		public void Collapse()
		{
			if ( IsCollapsible )
			{
				if ( IsCollapsed ) return;
				IsCollapsed = true;
				Cache = new List<Cell>(this);
			}
			else { Expand(true); }
		}
		public void Expand() => Expand(false);
		internal void Expand( bool force )
		{
			if ( !force &&
				 !IsCollapsed ) return;
			ShowVisibleCells();
			IsCollapsed = false;
		}
		internal void ShowHideSection()
		{
			if ( IsCollapsed ) { Collapse(); }
			else { Expand(); }
		}
		internal void ChildVisibilityChanged() => ShowVisibleCells();
		private void ShowVisibleCells()
		{
			if ( Cache.Count != Count ) { Cache = new List<Cell>(this); }

			Clear();
			foreach ( Cell cell in Cache )
			{
				if ( cell is CellBase.CellBase baseCell )
				{
					if ( baseCell.IsVisible ) base.Add(cell);
				}
				else { base.Add(cell); }
			}
		}

		internal void UpdateTitle() { HeaderView.Title = Title; }

		public new void Add( Cell cell )
		{
			Cache.Add(cell);
			base.Add(cell);
		}
		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		public new void Add( IEnumerable<Cell> cells )
		{
			Cache.AddRange(cells);
			base.Add(cells);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if ( HeaderView != null ) { HeaderView.BindingContext = BindingContext; }

			if ( FooterView != null ) { FooterView.BindingContext = BindingContext; }
		}

		public void MoveSourceItemWithoutNotify( int from, int to )
		{
			CollectionChanged -= OnCollectionChanged;
			var notifyCollection = ItemsSource as INotifyCollectionChanged;
			if ( notifyCollection != null ) { notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged; }

			if ( ItemsSource is null ) throw new NullReferenceException(nameof(ItemsSource));

			object tmp = ItemsSource[from];
			ItemsSource.RemoveAt(from);
			ItemsSource.Insert(to, tmp);

			Cell tmpCell = this[from];
			RemoveAt(from);
			Insert(to, tmpCell);

			if ( notifyCollection != null ) { notifyCollection.CollectionChanged += OnItemsSourceCollectionChanged; }

			CollectionChanged += OnCollectionChanged;
		}
		public void MoveCellWithoutNotify( int from, int to )
		{
			CollectionChanged -= OnCollectionChanged;
			Cell tmp = this[from];
			RemoveAt(from);
			Insert(to, tmp);
			CollectionChanged += OnCollectionChanged;
		}
		public (Cell? Cell, object? Item) DeleteSourceItemWithoutNotify( int from )
		{
			CollectionChanged -= OnCollectionChanged;
			if ( ItemsSource is INotifyCollectionChanged notifyCollection ) { notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged; }

			(Cell deletedCell, object? deletedItem) result;
			if ( ItemsSource is null )
			{
				Cell deleted = this[from];
				RemoveAt(from);
				result = ( deleted, null );
			}
			else
			{
				object deletedItem = ItemsSource[from];
				ItemsSource.RemoveAt(from);

				Cell deletedCell = this[from];
				RemoveAt(from);
				result = ( deletedCell, deletedItem );
			}

			if ( ItemsSource is INotifyCollectionChanged notify ) { notify.CollectionChanged += OnItemsSourceCollectionChanged; }

			CollectionChanged += OnCollectionChanged;

			return result;
		}
		public void InsertSourceItemWithoutNotify( Cell? cell, object? item, int to )
		{
			if ( cell is null ) return;

			CollectionChanged -= OnCollectionChanged;
			var notifyCollection = ItemsSource as INotifyCollectionChanged;
			if ( notifyCollection != null ) { notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged; }

			ItemsSource?.Insert(to, item);
			Insert(to, cell);

			if ( notifyCollection != null ) { notifyCollection.CollectionChanged += OnItemsSourceCollectionChanged; }

			CollectionChanged += OnCollectionChanged;
		}
		public Cell DeleteCellWithoutNotify( int from )
		{
			Cell deletedCell = this[from];
			CollectionChanged -= OnCollectionChanged;
			RemoveAt(from);
			CollectionChanged += OnCollectionChanged;
			return deletedCell;
		}
		public void InsertCellWithoutNotify( Cell cell, int to )
		{
			CollectionChanged -= OnCollectionChanged;
			Insert(to, cell);
			CollectionChanged += OnCollectionChanged;
		}

		private static void ItemsChanged( BindableObject bindable, object oldValue, object newValue )
		{
			if ( bindable is not Section section ) return;
			if ( section.ItemTemplate is null ) { return; }

			if ( oldValue is INotifyCollectionChanged oldObservableCollection ) { oldObservableCollection.CollectionChanged -= section.OnItemsSourceCollectionChanged; }

			// keep the platform from notifying item changed event.
			section.CollectionChanged -= section.OnCollectionChanged;

			if ( oldValue is IList oldValueAsEnumerable )
			{
				for ( int i = oldValueAsEnumerable.Count - 1; i >= 0; i-- ) { section.RemoveAt(section.TemplateStartIndex + i); }
			}

			if ( newValue is IList newValueAsEnumerable )
			{
				for ( var i = 0; i < newValueAsEnumerable.Count; i++ )
				{
					Cell? view = CreateChildViewFor(section.ItemTemplate, newValueAsEnumerable[i], section);
					if ( view is null ) continue;
					view.Parent = section.Parent;
					section.Insert(section.TemplateStartIndex + i, view);
				}

				section.templatedItemsCount = newValueAsEnumerable.Count;
			}

			section.CollectionChanged += section.OnCollectionChanged;

			if ( newValue is INotifyCollectionChanged newObservableCollection ) { newObservableCollection.CollectionChanged += section.OnItemsSourceCollectionChanged; }

			// Notify manually Collection Reset.
			section.SectionCollectionChanged?.Invoke(section, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		private void OnItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Replace:
				{
					//RemoveAt(e.OldStartingIndex + TemplateStartIndex);

					object item = e.NewItems[0];
					Cell? view = CreateChildViewFor(ItemTemplate, item, this);
					if ( view is null ) return;

					//Insert(e.NewStartingIndex + TemplateStartIndex, view);
					this[e.NewStartingIndex + TemplateStartIndex] = view;
					break;
				}

				case NotifyCollectionChangedAction.Add:
				{
					if ( e.NewItems != null )
					{
						for ( var i = 0; i < e.NewItems.Count; i++ )
						{
							object item = e.NewItems[i];
							Cell? view = CreateChildViewFor(ItemTemplate, item, this);

							if ( view is null ) continue;
							Insert(i + e.NewStartingIndex + TemplateStartIndex, view);
							templatedItemsCount++;
						}
					}

					break;
				}

				case NotifyCollectionChangedAction.Remove:
				{
					if ( e.OldItems != null )
					{
						RemoveAt(e.OldStartingIndex + TemplateStartIndex);
						templatedItemsCount--;
					}

					break;
				}

				case NotifyCollectionChangedAction.Reset:
				{
					for ( int i = templatedItemsCount - 1; i >= 0; i-- ) { RemoveAt(TemplateStartIndex + i); }

					templatedItemsCount = 0;
					break;
				}

				case NotifyCollectionChangedAction.Move: break;

				default:
					return;
			}
		}


		private static Cell? CreateChildViewFor( DataTemplate? template, object item, BindableObject container )
		{
			switch ( template )
			{
				case null: return null;

				case DataTemplateSelector selector:
					template = selector.SelectTemplate(item, container);
					template.SetValue(BindingContextProperty, item);
					return (Cell) template.CreateContent();

				default:
					//Binding context
					template.SetValue(BindingContextProperty, item);

					return (Cell) template.CreateContent();
			}
		}
	}
}