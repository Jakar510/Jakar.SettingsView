using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.sv
{
	/// <summary>
	/// Settings root.
	/// </summary>
	public class SettingsRoot : TableSectionBase<Section>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Shared.sv.SettingsRoot"/> class.
		/// </summary>
		public SettingsRoot() => CollectionChanged += OnCollectionChanged;
		~SettingsRoot() { CollectionChanged -= OnCollectionChanged; }

		/// <summary>
		/// Occurs when section collection changed.
		/// </summary>
		public event EventHandler<NotifyCollectionChangedEventArgs> SectionCollectionChanged;

		/// <summary>
		/// Occurs when section property changed.
		/// </summary>
		public event PropertyChangedEventHandler SectionPropertyChanged;

		private void ChildCollectionChanged( object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs ) { SectionCollectionChanged?.Invoke(sender, notifyCollectionChangedEventArgs); }

		private void ChildPropertyChanged( object sender, PropertyChangedEventArgs e ) { SectionPropertyChanged?.Invoke(sender, e); }

		private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs args )
		{
			if ( args.NewItems != null )
			{
				foreach ( Section section in args.NewItems )
				{
					section.SectionCollectionChanged += ChildCollectionChanged;
					section.SectionPropertyChanged += ChildPropertyChanged;
				}
			}

			if ( args.OldItems is null )
				return;
			foreach ( Section section in args.OldItems )
			{
				section.SectionCollectionChanged -= ChildCollectionChanged;
				section.SectionPropertyChanged -= ChildPropertyChanged;
			}
		}
	}
}