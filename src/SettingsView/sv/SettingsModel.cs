using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jakar.SettingsView.Shared.sv
{
	public class SettingsModel : TableModel
	{
		private static readonly BindableProperty PathProperty = BindableProperty.Create("Path", typeof(Tuple<int, int>), typeof(Cell), null);

		private readonly SettingsRoot _root;

		
		public SettingsModel( SettingsRoot settingsRoot ) => _root = settingsRoot;

		
		public override Cell GetCell( int section, int row )
		{
			var cell = (Cell) GetItem(section, row);
			SetPath(cell, new Tuple<int, int>(section, row));
			return cell;
		}
		public override object GetItem( int section, int row ) => _root.ElementAt(section)[row];
		public override int GetRowCount( int section ) => _root.ElementAt(section).Count;
		public override int GetSectionCount() => _root.Count();

		public virtual Section GetSection( int section ) => _root.ElementAtOrDefault(section);
		public virtual Section GetSectionFromCell( Cell cell ) { return _root.FirstOrDefault(x => x.Contains(cell)); }
		public virtual int GetSectionIndex( Section section ) => _root.IndexOf(section);

		public override string GetSectionTitle( int section ) => _root.ElementAt(section).Title;


		public virtual View GetSectionHeaderView( int section ) => _root.ElementAt(section).HeaderView.View;
		public virtual string GetFooterText( int section ) => _root.ElementAt(section).FooterText;
		public virtual View GetSectionFooterView( int section ) => _root.ElementAt(section).FooterView.View;

		protected override void OnRowSelected( object item )
		{
			base.OnRowSelected(item);

			( item as CellBase.CellBase )?.OnTapped();
		}

		// public virtual double GetHeaderHeight( int section ) => _root.ElementAt(section).HeaderHeight;
		// public virtual double GetHeaderHeight( int section ) => _root.ElementAt(section).HeaderView.Height;
		public virtual double GetHeaderHeight( int section ) => _root.ElementAt(section).HeaderView.HeightRequest;


		// this method no longer uses except for iOS.CellBaseRenderer.
		internal static Tuple<int, int> GetPath( Cell item )
		{
			if ( item is null ) throw new ArgumentNullException(nameof(item));

			return (Tuple<int, int>) item.GetValue(PathProperty);
		}


		// ReSharper disable once SuggestBaseTypeForParameter
		private static void SetPath( Cell item, Tuple<int, int> index )
		{
			item?.SetValue(PathProperty, index);
		}
	}
}