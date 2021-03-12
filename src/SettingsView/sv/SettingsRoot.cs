using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.sv
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public class SettingsRoot : TableSectionBase<Section>
	{
		public SettingsRoot() => CollectionChanged += OnCollectionChanged;
		~SettingsRoot() { CollectionChanged -= OnCollectionChanged; }
		
		public event EventHandler<NotifyCollectionChangedEventArgs> SectionCollectionChanged;
		
		public event PropertyChangedEventHandler SectionPropertyChanged;

		protected void ChildCollectionChanged( object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs ) { SectionCollectionChanged?.Invoke(sender, notifyCollectionChangedEventArgs); }

		protected void ChildPropertyChanged( object sender, PropertyChangedEventArgs e ) { SectionPropertyChanged?.Invoke(sender, e); }

		protected void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs args )
		{
			if ( args.OldItems is not null )
			{
				foreach ( Section section in args.OldItems )
				{
					section.SectionCollectionChanged -= ChildCollectionChanged;
					section.SectionPropertyChanged -= ChildPropertyChanged;
				}
			}

			if ( args.NewItems is null ) return;
			foreach ( Section section in args.NewItems )
			{
				section.SectionCollectionChanged += ChildCollectionChanged;
				section.SectionPropertyChanged += ChildPropertyChanged;
			}
		}
	}
}