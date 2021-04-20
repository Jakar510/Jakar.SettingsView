using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class Section : TableSectionBase<Cell>
	{
		public static readonly BindableProperty ItemTemplateProperty       = BindableProperty.Create(nameof(ItemTemplate),       typeof(DataTemplate), typeof(Section));
		public static readonly BindableProperty UseDragSortProperty        = BindableProperty.Create(nameof(UseDragSort),        typeof(bool),         typeof(Section), default(bool));
		public static readonly BindableProperty TemplateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int),          typeof(Section), default(int));
		public static readonly BindableProperty IsVisibleProperty          = BindableProperty.Create(nameof(IsVisible),          typeof(bool),         typeof(Section), true);

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																							  typeof(IList),
																							  typeof(Section),
																							  default(IList?),
																							  BindingMode.OneWay,
																							  propertyChanged: ItemsChanged
																							 );


		public static readonly BindableProperty HeaderViewProperty = BindableProperty.Create(nameof(HeaderView),
																							 typeof(ISectionHeader),
																							 typeof(Section),
																							 new DefaultHeaderView(),
																							 propertyChanging: HeaderViewPropertyChanging
																							);

		public static readonly BindableProperty FooterViewProperty = BindableProperty.Create(nameof(FooterView),
																							 typeof(ISectionFooter),
																							 typeof(Section),
																							 new DefaultFooterView(),
																							 propertyChanging: FooterViewPropertyChanging
																							);

		private static void HeaderViewPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			if ( bindable is not Section section ) return;

			if ( oldValue is ISectionHeader old ) { Update(old, null); }

			if ( newValue is ISectionHeader item ) { Update(item, section); }
		}

		private static void FooterViewPropertyChanging( BindableObject bindable, object oldValue, object newValue )
		{
			if ( bindable is not Section section ) return;

			if ( oldValue is ISectionFooter old ) { Update(old, null); }

			if ( newValue is ISectionFooter item ) { Update(item, section); }
		}

		private static void Update( ISectionFooterHeader item, Section? section )
		{
			item.Section        = section;
			item.BindingContext = section?.BindingContext;
		}


		public new static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
																							typeof(string),
																							typeof(BaseHeaderFooterView),
																							default(string?),
																							propertyChanging: ( bindable, old_value, new_value ) =>
																											  {
																												  if ( old_value == new_value ) return;

																												  if ( bindable is Section section )
																												  {
																													  section.HeaderView.SetText(new_value?.ToString());
																												  }
																											  }
																						   );

		public new static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor),
																								typeof(Color),
																								typeof(BaseHeaderFooterView),
																								SvConstants.Section.Header.text_Color,
																								propertyChanging: ( bindable, old_value, new_value ) =>
																												  {
																													  if ( old_value == new_value ) return;

																													  if ( bindable is Section section )
																													  {
																														  section.HeaderView.SetTextColor((Color) new_value);
																													  }
																												  }
																							   );

		public static readonly BindableProperty FooterTextProperty = BindableProperty.Create(nameof(FooterText),
																							 typeof(string),
																							 typeof(Section),
																							 default(string),
																							 propertyChanging: ( bindable, old_value, new_value ) =>
																											   {
																												   if ( old_value == new_value ) return;

																												   if ( bindable is Section section )
																												   {
																													   section.FooterView.SetText(new_value?.ToString());
																												   }
																											   }
																							);

		public static readonly BindableProperty FooterVisibleProperty = BindableProperty.Create(nameof(FooterVisible), typeof(bool), typeof(Section), SvConstants.Section.Footer.VISIBLE);

		// --------------------------------------------------------------------------------------

		public new string? Title
		{
			get => (string?) GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}


		[Xamarin.Forms.TypeConverter(typeof(ColorTypeConverter))]
		public new Color TextColor
		{
			get => (Color) GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}

		public DefaultHeaderView? DefaultHeader =>
			HeaderView is DefaultHeaderView header
				? header
				: null;

		public ISectionHeader HeaderView
		{
			get => (ISectionHeader) GetValue(HeaderViewProperty);
			set => SetValue(HeaderViewProperty, value);
		}

		// --------------------------------------------------------------------------------------

		public DefaultFooterView? DefaultFooter =>
			FooterView is DefaultFooterView footer
				? footer
				: null;

		public ISectionFooter FooterView
		{
			get => (ISectionFooter) GetValue(FooterViewProperty);
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

		// --------------------------------------------------------------------------------------

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


		private int templatedItemsCount;

		// --------------------------------------------------------------------------------------

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

		// --------------------------------------------------------------------------------------

		internal List<Cell> Cache { get; private set; } = new();

		public SettingsView? Parent { get; set; }

		public Section()
		{
			CollectionChanged += OnCollectionChanged;
			PropertyChanged   += OnPropertyChanged;
			TextColor         =  SvConstants.Section.Header.text_Color;
		}

		public Section( string       title ) : this() => Title = title;
		public Section( SettingsView parent ) : this(parent, string.Empty) { }

		public Section( SettingsView parent, string? title ) : this()
		{
			Title  = title;
			Parent = parent;
		}

		public Section( Cell              cell ) : this() { Add(cell); }
		public Section( IEnumerable<Cell> cells ) : this() { Add(cells); }
		public Section( params Cell[]     cells ) : this() { Add(cells); }
		public Section( SettingsView      parent, Cell              cell ) : this(parent) { Add(cell); }
		public Section( SettingsView      parent, IEnumerable<Cell> cells ) : this(parent) { Add(cells); }
		public Section( SettingsView      parent, params Cell[]     cells ) : this(parent) { Add(cells); }
		public Section( SettingsView      parent, string?           title, Cell              cell ) : this(parent, title) { Add(cell); }
		public Section( SettingsView      parent, string?           title, IEnumerable<Cell> cells ) : this(parent, title) { Add(cells); }
		public Section( SettingsView      parent, string?           title, params Cell[]     cells ) : this(parent, title) { Add(cells); }

		~Section()
		{
			if ( Debugger.IsAttached ) { Console.WriteLine($"------------ Section \"{Title}\" is being destructed. ------------"); }

			Cache.Clear();
			Clear();
		}


		public event NotifyCollectionChangedEventHandler? SectionCollectionChanged;

		public event PropertyChangedEventHandler? SectionPropertyChanged;

		private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { SectionCollectionChanged?.Invoke(this, e); }
		private void OnPropertyChanged( object   sender, PropertyChangedEventArgs         e ) { SectionPropertyChanged?.Invoke(this, e); }

		public void Collapse()
		{
			if ( IsCollapsible )
			{
				if ( IsCollapsed ) return;
				IsCollapsed = true;

				if ( Cache.Count == 0 ) { Cache = new List<Cell>(this); }

				Clear();
			}
			else { Expand(); }
		}

		public void Expand()
		{
			ShowVisibleCells();
			IsCollapsed = false;
		}

		internal void ShowHideSection()
		{
			if ( IsCollapsed ) { Collapse(); }
			else { Expand(); }
		}

		internal void ShowVisibleCells()
		{
			Clear();

			foreach ( Cell cell in Cache )
			{
				// ReSharper disable once SuspiciousTypeConversion.Global
				switch ( cell )
				{
					case CellBase.CellBase baseCell:
					{
						if ( baseCell.IsVisible ) base.Add(baseCell);
						break;
					}

					case IVisibleCell iCell:
					{
						if ( iCell.IsVisible ) base.Add(cell);
						break;
					}

					default:
						base.Add(cell);
						break;
				}
			}
		}

		internal void UpdateHeader()
		{
			HeaderView.SetText(Title);
			HeaderView.SetTextColor(TextColor);
		}

		internal void UpdateFooter() { FooterView.SetText(FooterText); }

		public new void Add( Cell cell )
		{
			Cache.Add(cell);
			base.Add(cell);
		}

		public new void Add( IEnumerable<Cell> cells )
		{
			foreach ( Cell item in cells ) { Add(item); }
		}

		public void Add( params Cell[] cells )
		{
			foreach ( Cell item in cells ) { Add(item); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			HeaderView.BindingContext = BindingContext;
			FooterView.BindingContext = BindingContext;
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
					Cell?  view = CreateChildViewFor(ItemTemplate, item, this);
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
							Cell?  view = CreateChildViewFor(ItemTemplate, item, this);

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
