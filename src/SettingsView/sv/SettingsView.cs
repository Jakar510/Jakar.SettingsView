using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jakar.SettingsView.Shared.sv
{
	/// <summary>
	/// Settings view.
	/// </summary>
	[ContentProperty("Root")]
	public partial class SettingsView : TableView
	{
		public const int MIN_ROW_HEIGHT = 60;
		internal static Action _clearCache;
		/// <summary>
		/// Clears the cache.
		/// </summary>
		public static void ClearCache() { _clearCache?.Invoke(); }

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public new SettingsModel Model { get; set; }

		/// <summary>
		/// Occurs when model changed.
		/// </summary>
		public new event EventHandler ModelChanged;

		/// <summary>
		/// Occurs when collection changed.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// Occurs when section collection changed.
		/// </summary>
		public event NotifyCollectionChangedEventHandler SectionCollectionChanged;

		/// <summary>
		/// Occurs when section property changed.
		/// </summary>
		public event PropertyChangedEventHandler SectionPropertyChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Shared.sv.SettingsView"/> class.
		/// </summary>
		public SettingsView()
		{
			VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
			Root = new SettingsRoot();
			Model = new SettingsModel(Root);
		}


		private SettingsRoot _root;

		/// <summary>
		/// Gets or sets the root.
		/// </summary>
		/// <value>The root.</value>
		public new SettingsRoot Root
		{
			get => _root;
			set
			{
				if ( _root != null )
				{
					_root.SectionPropertyChanged -= OnSectionPropertyChanged;
					_root.CollectionChanged -= OnCollectionChanged;
					_root.SectionCollectionChanged -= OnSectionCollectionChanged;
				}

				_root = value;

				//transfer binding context to the children (maybe...)
				SetInheritedBindingContext(_root, BindingContext);

				_root.SectionPropertyChanged += OnSectionPropertyChanged;
				_root.CollectionChanged += OnCollectionChanged;
				_root.SectionCollectionChanged += OnSectionCollectionChanged;
				OnModelChanged();
			}
		}

		/// <summary>
		/// Ons the binding context changed.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if ( Root != null )
				SetInheritedBindingContext(Root, BindingContext);
		}

		private void OnSectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { SectionPropertyChanged?.Invoke(sender, e); }

		/// <summary>
		/// Ons the property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected override void OnPropertyChanged( string propertyName = null )
		{
			base.OnPropertyChanged(propertyName);
			if ( propertyName == HasUnevenRowsProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderHeightProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderFontSizeProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderFontFamilyProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderFontAttributesProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderTextColorProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderBackgroundColorProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderTextVerticalAlignProperty.PropertyName ||
				 propertyName == Shared.SettingsView.HeaderPaddingProperty.PropertyName ||
				 propertyName == Shared.SettingsView.FooterFontSizeProperty.PropertyName ||
				 propertyName == Shared.SettingsView.FooterFontFamilyProperty.PropertyName ||
				 propertyName == Shared.SettingsView.FooterFontAttributesProperty.PropertyName ||
				 propertyName == Shared.SettingsView.FooterTextColorProperty.PropertyName ||
				 propertyName == Shared.SettingsView.FooterBackgroundColorProperty.PropertyName ||
				 propertyName == Shared.SettingsView.FooterPaddingProperty.PropertyName ) { OnModelChanged(); }
		}

		/// <summary>
		/// CollectionChanged by the section
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			e.NewItems?.Cast<Section>()
			 .ForEach(section =>
					  {
						  section.Parent = this;

						  if ( section.HeaderView != null ) { section.HeaderView.Parent = this; }

						  if ( section.FooterView != null ) { section.FooterView.Parent = this; }

						  foreach ( Cell cell in section )
						  {
							  object context = cell.BindingContext;
							  cell.Parent = this; // When setting the parent, the bindingcontext is updated too.
							  if ( context != null )
							  {
								  cell.BindingContext = context; // so set the original bindingcontext again.
							  }
						  }
					  });
			//e.NewItems.Cast<Section>().SelectMany(x => x).ForEach(cell =>cell.Parent = this);

			CollectionChanged?.Invoke(sender, e);
		}

		/// <summary>
		/// CollectionChanged by the child in section
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">The ${ParameterType} instance containing the event data.</param>
		public void OnSectionCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			e.NewItems?.Cast<Cell>().ForEach(cell => cell.Parent = this);

			SectionCollectionChanged?.Invoke(sender, e);
		}

		private new void OnModelChanged()
		{
			if ( Root == null ) { return; }

			foreach ( Section section in Root )
			{
				section.Parent = this;
				if ( section.HeaderView != null ) { section.HeaderView.Parent = this; }

				if ( section.FooterView != null ) { section.FooterView.Parent = this; }

				foreach ( Cell cell in section )
				{
					object context = cell.BindingContext;
					cell.Parent = this; // When setting the parent, the bindingcontext is updated too.
					if ( context != null )
					{
						cell.BindingContext = context; // so set the original bindingcontext again.
					}
				}
			}

			IEnumerable<Cell> cells = Root?.SelectMany(r => r);
			if ( cells == null ) { return; }


			//foreach (Cell cell in cells)
			//{
			//    //ViewCell size is not decided if parent isn't set.
			//    cell.Parent = this;
			//}

			//notify Native
			ModelChanged?.Invoke(this, EventArgs.Empty);
		}

		//make the unnecessary property existing at TableView sealed.
		private new int Intent { get; set; }
	}
}