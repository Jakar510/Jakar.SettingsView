using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public enum ViewType
	{
		// TextHeader,
		// TextFooter,
		CustomHeader,
		CustomFooter,
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	public class RowInfo
	{
		public Section Section { get; set; }
		public Cell? Cell { get; set; }
		public ViewType ViewType { get; set; }
		public RowInfo( Section section, ViewType viewType ) : this(section, null, viewType) { }
		public RowInfo( Section section, Cell? cell, ViewType viewType )
		{
			Section = section;
			Cell = cell;
			ViewType = viewType;
		}
	}


	[Android.Runtime.Preserve(AllMembers = true)]
	public class ModelProxy : List<RowInfo>, IDisposable
	{
		public Dictionary<Type, int> ViewTypes { get; private set; }

		private SettingsModel _Model { get; set; }
		private SettingsRoot _Root { get; set; }
		private SettingsViewRecyclerAdapter _Adapter { get; set; }
		private RecyclerView _RecyclerView { get; set; }

		public ModelProxy( Shared.sv.SettingsView settingsView, SettingsViewRecyclerAdapter adapter, RecyclerView recyclerView )
		{
			_Model = settingsView.Model;
			_Root = settingsView.Root;
			_Adapter = adapter;
			_RecyclerView = recyclerView;

			_Root.SectionCollectionChanged += OnRootSectionCollectionChanged;
			_Root.CollectionChanged += OnRootCollectionChanged;

			ViewTypes = UpdateTypes();
		}

		private void OnRootCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					if ( e.NewStartingIndex == -1 ||
						 e.NewItems == null ) { goto case NotifyCollectionChangedAction.Reset; }

					AddSection(e);
					break;
				case NotifyCollectionChangedAction.Remove:
					if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

					RemoveSection(e);
					break;
				case NotifyCollectionChangedAction.Replace: // no support on Android.
				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Reset:
					FillProxy();
					_Adapter.NotifyDataSetChanged();
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}


		private void OnRootSectionCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					if ( e.NewStartingIndex == -1 ||
						 e.NewItems == null ) { goto case NotifyCollectionChangedAction.Reset; }

					AddCell(sender, e);
					break;
				case NotifyCollectionChangedAction.Remove:
					if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

					RemoveCell(sender, e);
					break;
				case NotifyCollectionChangedAction.Replace:
					if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

					ReplaceCell(sender, e);
					break;

				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Reset:
					FillProxy();
					_Adapter.NotifyDataSetChanged();
					break;

				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void AddSection( NotifyCollectionChangedEventArgs e )
		{
			// regard as coming only one item.
			if ( e.NewItems[0] is not Section section ) return;
			int startIndex = RowIndexFromParentCollection(e.NewStartingIndex);
			Insert(startIndex, new RowInfo(section, ViewType.CustomHeader));
			// Insert(startIndex, new RowInfo(section, section.HeaderView == null ? ViewType.TextHeader : ViewType.CustomHeader));

			for ( var i = 0; i < section.Count; i++ )
			{
				Cell cell = section[i];
				if ( !ViewTypes.ContainsKey(cell.GetType()) ) { ViewTypes.Add(cell.GetType(), GetNextTypeIndex()); }

				var rowInfo = new RowInfo(section, cell, (ViewType) ViewTypes[cell.GetType()]);

				Insert(i + 1 + startIndex, rowInfo);
			}

			Insert(startIndex + section.Count() + 1, new RowInfo(section, ViewType.CustomFooter));
			// Insert(startIndex + section.Count() + 1, new RowInfo(section, section.FooterView == null ? ViewType.TextFooter : ViewType.CustomFooter));

			_Adapter.NotifyItemRangeInserted(startIndex, section.Count + 2); // add a header and footer
		}

		private void RemoveSection( NotifyCollectionChangedEventArgs e )
		{
			// regard as coming only one item.
			if ( e is null ) throw new NullReferenceException(nameof(e));
			if ( e.OldItems[0] is not Section section ) throw new ArgumentException("Sender must be of type(Section)", nameof(e.OldItems));

			int startIndex = RowIndexFromParentCollection(e.OldStartingIndex);

			RemoveRange(startIndex, section.Count + 2);

			_Adapter.NotifyItemRangeRemoved(startIndex, section.Count + 2);
		}


		private void AddCell( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( !( sender is Section section ) ) throw new ArgumentException("Sender must be of type(Section)", nameof(sender));

			int startIndex = RowIndexFromChildCollection(section, e.NewStartingIndex);
			List<Cell> newCells = e.NewItems.Cast<Cell>().ToList();
			for ( var i = 0; i < newCells.Count; i++ )
			{
				Cell cell = newCells[i];
				if ( !ViewTypes.ContainsKey(cell.GetType()) ) { ViewTypes.Add(cell.GetType(), GetNextTypeIndex()); }

				var rowInfo = new RowInfo(section, cell, (ViewType) ViewTypes[cell.GetType()]);
				Insert(i + startIndex, rowInfo);
			}

			_Adapter.NotifyItemRangeInserted(startIndex, newCells.Count);
		}

		private void RemoveCell( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e is null ) throw new NullReferenceException(nameof(e));
			if ( !( sender is Section section ) ) throw new ArgumentException("Sender must be of type(Section)", nameof(sender));

			int startIndex = RowIndexFromChildCollection(section, e.OldStartingIndex);
			RemoveAt(startIndex);

			_Adapter.NotifyItemRangeRemoved(startIndex, 1);
		}

		private void ReplaceCell( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e is null ) throw new NullReferenceException(nameof(e));
			if ( !( sender is Section section ) ) throw new ArgumentException("Sender must be of type(Section)", nameof(sender));

			int startIndex = RowIndexFromChildCollection(section, e.OldStartingIndex);
			if ( e.OldItems[0] is Cell repCell ) { this[startIndex] = new RowInfo(section, repCell, (ViewType) ViewTypes[repCell.GetType()]); }

			// Stop animation.
			if ( _RecyclerView.GetItemAnimator() is DefaultItemAnimator animator ) { animator.SupportsChangeAnimations = false; }

			_Adapter.NotifyItemRangeChanged(startIndex, 1);

			new Handler().PostDelayed(() =>
									  {
										  // Restart animation.
										  if ( _RecyclerView.GetItemAnimator() is DefaultItemAnimator _animator ) { _animator.SupportsChangeAnimations = false; }
									  },
									  100
									 );
		}

		private int RowIndexFromChildCollection( Section section, int index )
		{
			if ( section is null ) throw new NullReferenceException(nameof(section));

			int targetSectionIndex = this.IndexOf(x => x.Section == section);

			if ( targetSectionIndex < 0 ) return -1;

			return index + targetSectionIndex + 1;
		}

		private int RowIndexFromParentCollection( int index )
		{
			var groups = this.Select(( x, idx ) => new
												   {
													   index = idx,
													   x.Section
												   }
									)
							 .GroupBy(x => x.Section);
			int? match = groups.ElementAtOrDefault(index)?.Min(x => x.index);

			return match ?? Count;
		}

		private int GetNextTypeIndex()
		{
			int idx = ViewTypes.Values.LastOrDefault();
			return idx == 0
					   ? Enum.GetNames(typeof(ViewType)).Length
					   : idx + 1;
		}

		public void FillProxy() { ViewTypes = UpdateTypes(); }
		protected Dictionary<Type, int> UpdateTypes()
		{
			Clear();

			Dictionary<Type, int> viewTypes = _Root.SelectMany(x => x)
												   .Select(x => x.GetType())
												   .Distinct()
												   .Select(( type, idx ) => new
																			{
																				type,
																				index = idx
																			}
														  )
												   .ToDictionary(key => key.type, val => val.index + Enum.GetNames(typeof(ViewType)).Length);

			int sectionCount = _Model.GetSectionCount();

			for ( var sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++ )
			{
				int sectionRowCount = _Model.GetRowCount(sectionIndex);
				Section? curSection = _Model.GetSection(sectionIndex);
				// bool isTextHeader = _Model.GetSectionHeaderView(sectionIndex) is null;
				// Add(new RowInfo(curSection, isTextHeader ? ViewType.TextHeader : ViewType.CustomHeader));
				if ( curSection is null ) { continue; }

				Add(new RowInfo(curSection, ViewType.CustomHeader));

				for ( var i = 0; i < sectionRowCount; i++ )
				{
					Cell cell = _Model.GetCell(sectionIndex, i);
					Add(new RowInfo(curSection, cell, (ViewType) viewTypes[cell.GetType()]));
				}

				// bool isTextFooter = _Model.GetSectionFooterView(sectionIndex) == null;
				// Add(new RowInfo(curSection, isTextFooter ? ViewType.TextFooter : ViewType.CustomFooter));
				Add(new RowInfo(curSection, ViewType.CustomFooter));
			}

			return viewTypes;
		}

		public void Dispose()
		{
			_Root.SectionCollectionChanged -= OnRootSectionCollectionChanged;
			_Root.CollectionChanged -= OnRootCollectionChanged;
			Clear();
			ViewTypes.Clear();
		}
	}
}