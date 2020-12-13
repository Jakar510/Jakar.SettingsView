using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared
{
	/// <summary>
	/// Section.
	/// </summary>
	public class Section : TableSectionBase<Cell>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Shared.Section"/> class.
		/// </summary>
		public Section()
		{
			CollectionChanged += OnCollectionChanged;
			PropertyChanged += OnPropertyChanged;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Shared.Section"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		public Section( string title ) : this() => Title = title;

		/// <summary>
		/// Ons the binding context changed.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if ( HeaderView != null ) { HeaderView.BindingContext = BindingContext; }

			if ( FooterView != null ) { FooterView.BindingContext = BindingContext; }
		}

		/// <summary>
		/// Moves the source item without notify.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
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

		/// <summary>
		/// Moves the cell without notify.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		public void MoveCellWithoutNotify( int from, int to )
		{
			CollectionChanged -= OnCollectionChanged;
			Cell tmp = this[from];
			RemoveAt(from);
			Insert(to, tmp);
			CollectionChanged += OnCollectionChanged;
		}

		public (Cell Cell, Object Item) DeleteSourceItemWithoutNotify( int from )
		{
			CollectionChanged -= OnCollectionChanged;
			var notifyCollection = ItemsSource as INotifyCollectionChanged;
			if ( notifyCollection != null ) { notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged; }

			object deletedItem = ItemsSource[from];
			ItemsSource.RemoveAt(from);

			Cell deletedCell = this[from];
			RemoveAt(from);

			if ( notifyCollection != null ) { notifyCollection.CollectionChanged += OnItemsSourceCollectionChanged; }

			CollectionChanged += OnCollectionChanged;

			return ( deletedCell, deletedItem );
		}

		public void InsertSourceItemWithoutNotify( Cell cell, Object item, int to )
		{
			CollectionChanged -= OnCollectionChanged;
			var notifyCollection = ItemsSource as INotifyCollectionChanged;
			if ( notifyCollection != null ) { notifyCollection.CollectionChanged -= OnItemsSourceCollectionChanged; }

			ItemsSource.Insert(to, item);
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


		/// <summary>
		/// Occurs when section collection changed.
		/// </summary>
		public event NotifyCollectionChangedEventHandler SectionCollectionChanged;

		/// <summary>
		/// Occurs when section property changed.
		/// </summary>
		public event PropertyChangedEventHandler SectionPropertyChanged;

		public SettingsView Parent { get; set; }

		/// <summary>
		/// The is visible property.
		/// </summary>
		public static BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(Section), true, defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Section"/> is visible.
		/// </summary>
		/// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get => (bool) GetValue(IsVisibleProperty);
			set => SetValue(IsVisibleProperty, value);
		}

		/// <summary>
		/// The footer text property.
		/// </summary>
		public static BindableProperty FooterTextProperty = BindableProperty.Create(nameof(FooterText), typeof(string), typeof(Section), default(string), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the footer text.
		/// </summary>
		/// <value>The footer text.</value>
		public string FooterText
		{
			get => (string) GetValue(FooterTextProperty);
			set => SetValue(FooterTextProperty, value);
		}

		/// <summary>
		/// The item template property.
		/// </summary>
		public static BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(Section), default(DataTemplate), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public DataTemplate ItemTemplate
		{
			get => (DataTemplate) GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}

		/// <summary>
		/// The items source property.
		/// </summary>
		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(Section), default(IList), defaultBindingMode: BindingMode.OneWay, propertyChanged: ItemsChanged);

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
		/// The header height property.
		/// </summary>
		public static BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(double), typeof(Section), -1d, defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the height of the header.
		/// </summary>
		/// <value>The height of the header.</value>
		public double HeaderHeight
		{
			get => (double) GetValue(HeaderHeightProperty);
			set => SetValue(HeaderHeightProperty, value);
		}

		/// <summary>
		/// The header view property.
		/// </summary>
		public static BindableProperty HeaderViewProperty = BindableProperty.Create(nameof(HeaderView), typeof(View), typeof(Section), default(View), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the header view.
		/// </summary>
		/// <value>The header view.</value>
		public View HeaderView
		{
			get => (View) GetValue(HeaderViewProperty);
			set => SetValue(HeaderViewProperty, value);
		}

		/// <summary>
		/// The footer view property.
		/// </summary>
		public static BindableProperty FooterViewProperty = BindableProperty.Create(nameof(FooterView), typeof(View), typeof(Section), default(View), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the footer view.
		/// </summary>
		/// <value>The footer view.</value>
		public View FooterView
		{
			get => (View) GetValue(FooterViewProperty);
			set => SetValue(FooterViewProperty, value);
		}

		public static BindableProperty FooterVisibleProperty = BindableProperty.Create(nameof(FooterVisible), typeof(bool), typeof(Section), true, defaultBindingMode: BindingMode.OneWay);

		public bool FooterVisible
		{
			get => (bool) GetValue(FooterVisibleProperty);
			set => SetValue(FooterVisibleProperty, value);
		}

		/// <summary>
		/// The use drag sort property.
		/// </summary>
		public static BindableProperty UseDragSortProperty = BindableProperty.Create(nameof(UseDragSort), typeof(bool), typeof(Section), default(bool), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Section"/> use drag sort.
		/// </summary>
		/// <value><c>true</c> if use drag sort; otherwise, <c>false</c>.</value>
		public bool UseDragSort
		{
			get => (bool) GetValue(UseDragSortProperty);
			set => SetValue(UseDragSortProperty, value);
		}

		public static BindableProperty TemplateStartIndexProperty = BindableProperty.Create(nameof(TemplateStartIndex), typeof(int), typeof(Section), default(int), defaultBindingMode: BindingMode.OneWay);

		public int TemplateStartIndex
		{
			get => (int) GetValue(TemplateStartIndexProperty);
			set => SetValue(TemplateStartIndexProperty, value);
		}

		private int templatedItemsCount;

		private static void ItemsChanged( BindableObject bindable, object oldValue, object newValue )
		{
			var section = (Section) bindable;

			if ( section.ItemTemplate == null ) { return; }

			IList oldValueAsEnumerable;
			IList newValueAsEnumerable;
			try
			{
				oldValueAsEnumerable = oldValue as IList;
				newValueAsEnumerable = newValue as IList;
			}
			catch ( Exception e ) { throw e; }

			var oldObservableCollection = oldValue as INotifyCollectionChanged;

			if ( oldObservableCollection != null ) { oldObservableCollection.CollectionChanged -= section.OnItemsSourceCollectionChanged; }

			// keep the platform from notifying itemchanged event.
			section.CollectionChanged -= section.OnCollectionChanged;

			if ( oldValueAsEnumerable != null )
			{
				for ( int i = oldValueAsEnumerable.Count - 1; i >= 0; i-- ) { section.RemoveAt(section.TemplateStartIndex + i); }
			}

			if ( newValueAsEnumerable != null )
			{
				for ( var i = 0; i < newValueAsEnumerable.Count; i++ )
				{
					Cell view = CreateChildViewFor(section.ItemTemplate, newValueAsEnumerable[i], section);
					view.Parent = section.Parent;
					section.Insert(section.TemplateStartIndex + i, view);
				}

				section.templatedItemsCount = newValueAsEnumerable.Count;
			}

			section.CollectionChanged += section.OnCollectionChanged;

			var newObservableCollection = newValue as INotifyCollectionChanged;

			if ( newObservableCollection != null ) { newObservableCollection.CollectionChanged += section.OnItemsSourceCollectionChanged; }

			// Notify manually Collection Reset.
			section.SectionCollectionChanged?.Invoke(section, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		private void OnItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e.Action == NotifyCollectionChangedAction.Replace )
			{
				//RemoveAt(e.OldStartingIndex + TemplateStartIndex);

				object item = e.NewItems[0];
				Cell view = CreateChildViewFor(ItemTemplate, item, this);

				//Insert(e.NewStartingIndex + TemplateStartIndex, view);
				this[e.NewStartingIndex + TemplateStartIndex] = view;
			}

			else if ( e.Action == NotifyCollectionChangedAction.Add )
			{
				if ( e.NewItems != null )
				{
					for ( var i = 0; i < e.NewItems.Count; i++ )
					{
						object item = e.NewItems[i];
						Cell view = CreateChildViewFor(ItemTemplate, item, this);

						Insert(i + e.NewStartingIndex + TemplateStartIndex, view);
						templatedItemsCount++;
					}
				}
			}

			else if ( e.Action == NotifyCollectionChangedAction.Remove )
			{
				if ( e.OldItems != null )
				{
					RemoveAt(e.OldStartingIndex + TemplateStartIndex);
					templatedItemsCount--;
				}
			}

			else if ( e.Action == NotifyCollectionChangedAction.Reset )
			{
				//this.Clear();
				var source = ItemsSource as IList;
				for ( int i = templatedItemsCount - 1; i >= 0; i-- ) { RemoveAt(TemplateStartIndex + i); }

				templatedItemsCount = 0;
			}

			else { return; }
		}


		private static Cell CreateChildViewFor( DataTemplate template, object item, BindableObject container )
		{
			var selector = template as DataTemplateSelector;

			if ( selector != null ) { template = selector.SelectTemplate(item, container); }

			//Binding context
			template.SetValue(BindingContextProperty, item);

			return (Cell) template.CreateContent();
		}
	}
}