using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public enum ViewType
	{
		TextHeader,
		TextFooter,
		CustomHeader,
		CustomFooter,
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	public class RowInfo
	{
		public Section Section { get; set; }
		public Cell Cell { get; set; }
		public ViewType ViewType { get; set; }
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	public class ModelProxy : List<RowInfo>, IDisposable
	{
		public Dictionary<Type, int> ViewTypes { get; private set; }

		private SettingsModel _model;
		private SettingsRoot _root;
		private SettingsViewRecyclerAdapter _adapter;
		private RecyclerView _recyclerView;

		public ModelProxy( Shared.SettingsView settingsView, SettingsViewRecyclerAdapter adapter, RecyclerView recyclerView )
		{
			_model = settingsView.Model;
			_root = settingsView.Root;
			_adapter = adapter;
			_recyclerView = recyclerView;

			_root.SectionCollectionChanged += OnRootSectionCollectionChanged;
			_root.CollectionChanged += OnRootCollectionChanged;

			FillProxy();
		}

		public void Dispose()
		{
			_root.SectionCollectionChanged -= OnRootSectionCollectionChanged;
			_root.CollectionChanged -= OnRootCollectionChanged;
			_model = null;
			_root = null;
			_adapter = null;
			_recyclerView = null;
			this?.Clear();
			ViewTypes?.Clear();
			ViewTypes = null;
		}

		private void OnRootCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					if ( e.NewStartingIndex == -1 || e.NewItems == null ) { goto case NotifyCollectionChangedAction.Reset; }

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
					_adapter.NotifyDataSetChanged();
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
					if ( e.NewStartingIndex == -1 || e.NewItems == null ) { goto case NotifyCollectionChangedAction.Reset; }

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
					_adapter.NotifyDataSetChanged();
					break;

				default: throw new ArgumentOutOfRangeException();
			}
		}

		private void AddSection( NotifyCollectionChangedEventArgs e )
		{
			// regard as coming only one item.
			var section = e.NewItems[0] as Section;
			int startIndex = RowIndexFromParentCollection(e.NewStartingIndex);

			Insert(startIndex, new RowInfo
							   {
								   Section = section,
								   ViewType = section.HeaderView == null ? ViewType.TextHeader : ViewType.CustomHeader,
							   });

			for ( var i = 0; i < section.Count; i++ )
			{
				Cell cell = section[i];
				if ( !ViewTypes.ContainsKey(cell.GetType()) ) { ViewTypes.Add(cell.GetType(), GetNextTypeIndex()); }

				var rowInfo = new RowInfo
							  {
								  Section = section,
								  Cell = cell,
								  ViewType = (ViewType) ViewTypes[cell.GetType()],
							  };
				Insert(i + 1 + startIndex, rowInfo);
			}

			Insert(startIndex + section.Count() + 1, new RowInfo
													 {
														 Section = section,
														 ViewType = section.FooterView == null ? ViewType.TextFooter : ViewType.CustomFooter,
													 });

			_adapter.NotifyItemRangeInserted(startIndex, section.Count + 2); // add a header and footer
		}

		private void RemoveSection( NotifyCollectionChangedEventArgs e )
		{
			// regard as coming only one item.
			if ( e is null ) throw new NullReferenceException(nameof(e));
			if ( !( e.OldItems[0] is Section section ) ) throw new ArgumentException("Sender must be of type(Section)", nameof(e.OldItems));

			int startIndex = RowIndexFromParentCollection(e.OldStartingIndex);

			RemoveRange(startIndex, section.Count + 2);

			_adapter.NotifyItemRangeRemoved(startIndex, section.Count + 2);
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

				var rowInfo = new RowInfo
							  {
								  Section = section,
								  Cell = cell,
								  ViewType = (ViewType) ViewTypes[cell.GetType()],
							  };
				Insert(i + startIndex, rowInfo);
			}

			_adapter.NotifyItemRangeInserted(startIndex, newCells.Count);
		}

		private void RemoveCell( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e is null ) throw new NullReferenceException(nameof(e));
			if ( !( sender is Section section ) ) throw new ArgumentException("Sender must be of type(Section)", nameof(sender));

			int startIndex = RowIndexFromChildCollection(section, e.OldStartingIndex);
			RemoveAt(startIndex);

			_adapter.NotifyItemRangeRemoved(startIndex, 1);
		}

		private void ReplaceCell( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( e is null ) throw new NullReferenceException(nameof(e));
			if ( !( sender is Section section ) ) throw new ArgumentException("Sender must be of type(Section)", nameof(sender));

			int startIndex = RowIndexFromChildCollection(section, e.OldStartingIndex);
			if ( e.OldItems[0] is Cell repCell )
			{
				this[startIndex] = new RowInfo
								   {
									   Section = section,
									   Cell = repCell,
									   ViewType = (ViewType) ViewTypes[repCell.GetType()],
								   };
			}

			// Stop animation.
			if ( _recyclerView.GetItemAnimator() is DefaultItemAnimator animator ) { animator.SupportsChangeAnimations = false; }

			_adapter.NotifyItemRangeChanged(startIndex, 1);

			new Handler().PostDelayed(() =>
									  {
										  // Restart animation.
										  if ( _recyclerView.GetItemAnimator() is DefaultItemAnimator _animator ) { _animator.SupportsChangeAnimations = false; }
									  }, 100);
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
												   })
							 .GroupBy(x => x.Section);
			int? match = groups.ElementAtOrDefault(index)?.Min(x => x.index);

			return match ?? Count;
		}

		private int GetNextTypeIndex()
		{
			int idx = ViewTypes.Values.LastOrDefault();
			return idx == 0 ? Enum.GetNames(typeof(ViewType)).Length : idx + 1;
		}

		public void FillProxy()
		{
			Clear();

			ViewTypes = _root.SelectMany(x => x)
							 .Select(x => x.GetType())
							 .Distinct()
							 .Select(( type, idx ) => new
													  {
														  type,
														  index = idx
													  })
							 .ToDictionary(key => key.type, val => val.index + Enum.GetNames(typeof(ViewType)).Length);

			int sectionCount = _model.GetSectionCount();

			for ( var sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++ )
			{
				int sectionRowCount = _model.GetRowCount(sectionIndex);
				bool isTextHeader = _model.GetSectionHeaderView(sectionIndex) == null;
				Section curSection = _model.GetSection(sectionIndex);

				Add(new RowInfo
					{
						Section = curSection,
						ViewType = isTextHeader ? ViewType.TextHeader : ViewType.CustomHeader,
					});

				for ( var i = 0; i < sectionRowCount; i++ )
				{
					Cell cell = _model.GetCell(sectionIndex, i);
					Add(new RowInfo
						{
							Section = curSection,
							Cell = cell,
							ViewType = (ViewType) ViewTypes[cell.GetType()],
						});
				}

				bool isTextFooter = _model.GetSectionFooterView(sectionIndex) == null;

				Add(new RowInfo
					{
						Section = curSection,
						ViewType = isTextFooter ? ViewType.TextFooter : ViewType.CustomFooter,
					});
			}
		}
	}
}