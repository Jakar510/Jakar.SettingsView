using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	public class Section : TableSectionBase<Cell>
	{
		public static BindableProperty FooterTextProperty = BindableProperty.Create(nameof(FooterText), typeof(string), typeof(Section), default(string));
		public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(Section), default(DataTemplate));
		public static BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(double), typeof(Section), -1d);
		public static BindableProperty HeaderViewProperty = BindableProperty.Create(nameof(HeaderView), typeof(View), typeof(Section), default(View));
		public static BindableProperty FooterViewProperty = BindableProperty.Create(nameof(FooterView), typeof(View), typeof(Section), default(View));
		public static BindableProperty FooterVisibleProperty = BindableProperty.Create(nameof(FooterVisible), typeof(bool), typeof(Section), true);
		public static BindableProperty UseDragSortProperty = BindableProperty.Create(nameof(UseDragSort), typeof(bool), typeof(Section), default(bool));
		public static BindableProperty TemplateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int), typeof(Section), default(int));
		public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(Section), true);

		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource),
																					 typeof(IList),
																					 typeof(Section),
																					 default(IList),
																					 BindingMode.OneWay,
																					 propertyChanged: ItemsChanged
																					);


		public Section()
		{
			CollectionChanged += OnCollectionChanged;
			PropertyChanged += OnPropertyChanged;
		}
		public Section( string title ) : this() => Title = title;
		public Section( SettingsView settingsView )
		{
			Parent = settingsView;
			CollectionChanged += OnCollectionChanged;
			PropertyChanged += OnPropertyChanged;
		}
		public Section( SettingsView settingsView, string title )
		{
			Title = title;
			Parent = settingsView;
			CollectionChanged += OnCollectionChanged;
			PropertyChanged += OnPropertyChanged;
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

		private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { SectionCollectionChanged?.Invoke(this, e); }

		private void OnPropertyChanged( object sender, PropertyChangedEventArgs e ) { SectionPropertyChanged?.Invoke(this, e); }


		public event NotifyCollectionChangedEventHandler? SectionCollectionChanged;

		public event PropertyChangedEventHandler? SectionPropertyChanged;

		public SettingsView? Parent { get; set; }

		public bool IsVisible
		{
			get => (bool) GetValue(IsVisibleProperty);
			set => SetValue(IsVisibleProperty, value);
		}

		public string? FooterText
		{
			get => (string?) GetValue(FooterTextProperty);
			set => SetValue(FooterTextProperty, value);
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

		public double HeaderHeight
		{
			get => (double) GetValue(HeaderHeightProperty);
			set => SetValue(HeaderHeightProperty, value);
		}

		public View? HeaderView
		{
			get => (View?) GetValue(HeaderViewProperty);
			set => SetValue(HeaderViewProperty, value);
		}

		public View? FooterView
		{
			get => (View?) GetValue(FooterViewProperty);
			set => SetValue(FooterViewProperty, value);
		}

		public bool FooterVisible
		{
			get => (bool) GetValue(FooterVisibleProperty);
			set => SetValue(FooterVisibleProperty, value);
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