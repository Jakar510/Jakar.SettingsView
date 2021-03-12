using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#nullable enable
namespace Jakar.SettingsView.Shared.sv
{
	[ContentProperty(nameof(Root))]
	public partial class SettingsView : TableView
	{
		internal static Action? _clearCache;
		private SettingsRoot? _root;

		public new SettingsRoot Root
		{
			get => _root ?? throw new NullReferenceException(nameof(_root));
			set
			{
				if ( _root is not null )
				{
					_root.SectionPropertyChanged -= OnSectionPropertyChanged;
					_root.CollectionChanged -= OnCollectionChanged;
					_root.SectionCollectionChanged -= OnSectionCollectionChanged;
				}

				_root = value;

				//transfer binding context to the children (maybe...)
				SetInheritedBindingContext(_root, BindingContext);

				if ( _root is not null )
				{
					_root.SectionPropertyChanged += OnSectionPropertyChanged;
					_root.CollectionChanged += OnCollectionChanged;
					_root.SectionCollectionChanged += OnSectionCollectionChanged;
				}

				OnModelChanged();
			}
		}

		public new SettingsModel Model { get; set; }

		public new event EventHandler? ModelChanged;
		public event NotifyCollectionChangedEventHandler? CollectionChanged;
		public event NotifyCollectionChangedEventHandler? SectionCollectionChanged;
		public event PropertyChangedEventHandler? SectionPropertyChanged;


		public SettingsView( IEnumerable<CellBase.CellBase> cells ) : this() { Add(new Section(cells)); }
		public SettingsView( IEnumerable<Section> sections ) : this() { Add(sections); }
		public SettingsView() : this(new SettingsRoot()) { }
		public SettingsView( SettingsRoot root ) : this(root, new SettingsModel(root)) { }
		public SettingsView( SettingsRoot root, SettingsModel model ) : this(root, model, LayoutOptions.FillAndExpand, LayoutOptions.FillAndExpand) { }
		public SettingsView( SettingsRoot root,
							 SettingsModel model,
							 LayoutOptions horizontal,
							 LayoutOptions vertical )
		{
			VerticalOptions = horizontal;
			HorizontalOptions = vertical;
			Root = root;
			Model = model;
			HasUnevenRows = true;
		}


		public void Add( Section section ) => Root.Add(section);
		public void Add( IEnumerable<Section> sections ) => Root.Add(sections);


		public static void ClearCache() { _clearCache?.Invoke(); }
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if ( Root is not null )
				SetInheritedBindingContext(Root, BindingContext);
		}

		private void OnSectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { SectionPropertyChanged?.Invoke(sender, e); }
		protected override void OnPropertyChanged( string? propertyName = null )
		{
			base.OnPropertyChanged(propertyName);
			if ( propertyName == HasUnevenRowsProperty.PropertyName ||
				 propertyName == HeaderHeightProperty.PropertyName ||
				 propertyName == HeaderFontSizeProperty.PropertyName ||
				 propertyName == HeaderFontFamilyProperty.PropertyName ||
				 propertyName == HeaderFontAttributesProperty.PropertyName ||
				 propertyName == HeaderTextColorProperty.PropertyName ||
				 propertyName == HeaderBackgroundColorProperty.PropertyName ||
				 // propertyName == HeaderTextVerticalAlignProperty.PropertyName ||
				 propertyName == HeaderPaddingProperty.PropertyName ||
				 propertyName == FooterFontSizeProperty.PropertyName ||
				 propertyName == FooterFontFamilyProperty.PropertyName ||
				 propertyName == FooterFontAttributesProperty.PropertyName ||
				 propertyName == FooterTextColorProperty.PropertyName ||
				 propertyName == FooterBackgroundColorProperty.PropertyName ||
				 propertyName == FooterPaddingProperty.PropertyName ) { OnModelChanged(); }
		}

		public void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			e.NewItems?.Cast<Section>()
			 .ForEach(section =>
					  {
						  section.Parent = this;

						  section.HeaderView.Parent = this;

						  section.FooterView.Parent = this;

						  foreach ( Cell cell in section )
						  {
							  object context = cell.BindingContext;
							  cell.Parent = this; // When setting the parent, the bindingcontext is updated too.
							  if ( context is not null )
							  {
								  cell.BindingContext = context; // so set the original bindingcontext again.
							  }
						  }
					  }
					 );
			//e.NewItems.Cast<Section>().SelectMany(x => x).ForEach(cell =>cell.Parent = this);

			CollectionChanged?.Invoke(sender, e);
		}
		public void OnSectionCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			e.NewItems?.Cast<Cell>().ForEach(cell => cell.Parent = this);

			SectionCollectionChanged?.Invoke(sender, e);
		}

		private new void OnModelChanged()
		{
			if ( _root is null ) { return; }

			foreach ( Section section in Root )
			{
				section.Parent = this;
				if ( section.HeaderView is not null ) { section.HeaderView.Parent = this; }

				if ( section.FooterView is not null ) { section.FooterView.Parent = this; }

				foreach ( Cell cell in section )
				{
					object context = cell.BindingContext;
					cell.Parent = this; // When setting the parent, the binding context is updated too.
					if ( context is not null )
					{
						cell.BindingContext = context; // so set the original binding context again.
					}
				}
			}

			// IEnumerable<Cell> cells = Root?.SelectMany(r => r);
			// if ( cells is null ) { return; }

			//foreach (Cell cell in cells)
			//{
			//    //ViewCell size is not decided if parent isn't set.
			//    cell.Parent = this;
			//}

			ModelChanged?.Invoke(this, EventArgs.Empty); //notify Native
		}


		//make the unnecessary property existing at TableView sealed.
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0051 // Remove unused private members
		private new int Intent { get; set; }
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE1006 // Naming Styles
	}
}