using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.RecyclerView.Widget;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SettingsViewSimpleCallback : ItemTouchHelper.SimpleCallback
	{
		private Shared.SettingsView _SettingsView { get; set; }
		private RowInfo? _FromInfo { get; set; }
		private Queue<(RowInfo from, RowInfo to)> _moveHistory = new Queue<(RowInfo from, RowInfo to)>();

		public SettingsViewSimpleCallback( Shared.SettingsView settingsView, int dragDirs, int swipeDirs ) : base(dragDirs, swipeDirs) => _SettingsView = settingsView;

		public override bool OnMove( RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target )
		{
			System.Diagnostics.Debug.WriteLine("OnMove");
			if ( !( viewHolder is ContentBodyViewHolder fromContentHolder ) )
			{
				System.Diagnostics.Debug.WriteLine("Cannot move no ContentHolder");
				return false;
			}

			int fromPos = viewHolder.AdapterPosition;
			int toPos = target.AdapterPosition;

			if ( fromPos < toPos )
			{
				// disallow a Footer when drag is from up to down.
				if ( target is IFooterViewHolder )
				{
					System.Diagnostics.Debug.WriteLine("Up To Down disallow Footer");
					return false;
				}
			}
			else
			{
				// disallow a Header when drag is from down to up.
				if ( target is IHeaderViewHolder )
				{
					System.Diagnostics.Debug.WriteLine("Down To Up disallow Header");
					return false;
				}
			}


			if ( target is CustomViewHolder toContentHolder )
			{
				Section section = fromContentHolder.RowInfo.Section;
				if ( section == null ||
					 !section.UseDragSort )
				{
					System.Diagnostics.Debug.WriteLine("From Section Not UseDragSort");
					return false;
				}

				Section toSection = toContentHolder.RowInfo.Section;
				if ( toSection == null ||
					 !toSection.UseDragSort )
				{
					System.Diagnostics.Debug.WriteLine("To Section Not UseDragSort");
					return false;
				}

				RowInfo toInfo = toContentHolder.RowInfo;
				System.Diagnostics.Debug.WriteLine($"Set ToInfo Section:{_SettingsView.Root.IndexOf(toInfo.Section)} Cell:{toInfo.Section.IndexOf(toInfo.Cell)}");

				// save moved changes 
				if ( _FromInfo != null &&
					 toContentHolder.RowInfo != null ) { _moveHistory.Enqueue(( _FromInfo, toContentHolder.RowInfo )); }
			}

			if ( recyclerView.GetAdapter() is SettingsViewRecyclerAdapter settingsAdapter )
			{
				settingsAdapter.CellMoved(fromPos, toPos);       //caches update
				settingsAdapter.NotifyItemMoved(fromPos, toPos); //rows update
			}


			System.Diagnostics.Debug.WriteLine($"Move Completed from:{fromPos} to:{toPos}");

			return true;
		}

		private void DataSourceMoved()
		{
			if ( !_moveHistory.Any() ) { return; }

			Cell cell = _moveHistory.Peek().from.Cell;
			Section section = _moveHistory.Last().to.Section;
			while ( _moveHistory.Any() )
			{
				(RowInfo @from, RowInfo to) pos = _moveHistory.Dequeue();
				DataSourceMoved(pos.from, pos.to);
			}

			_SettingsView.SendItemDropped(section, cell);
		}

		private void DataSourceMoved( RowInfo from, RowInfo to )
		{
			int fromPos = from.Section.IndexOf(from.Cell);
			int toPos = to.Section.IndexOf(to.Cell);
			if ( toPos < 0 )
			{
				// if Header, insert the first.s
				toPos = 0;
			}

			if ( from.Section.ItemsSource == null )
			{
				System.Diagnostics.Debug.WriteLine($"Update Sections from:{fromPos} to:{toPos}");
				Cell cell = from.Section.DeleteCellWithoutNotify(fromPos);
				to.Section.InsertCellWithoutNotify(cell, toPos);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"UpdateSource from:{fromPos} to:{toPos}");
				(Cell Cell, object Item) deletedSet = from.Section.DeleteSourceItemWithoutNotify(fromPos);
				to.Section.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, toPos);
			}

			from.Section = to.Section;
		}

		public override void ClearView( RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder )
		{
			System.Diagnostics.Debug.WriteLine("On ClearView");
			base.ClearView(recyclerView, viewHolder);

			viewHolder.ItemView.Alpha = 1.0f;
			viewHolder.ItemView.ScaleX = 1.0f;
			viewHolder.ItemView.ScaleY = 1.0f;

			// DataSource Update
			DataSourceMoved();

			_moveHistory.Clear();
			_FromInfo = null;
		}

		public override int GetDragDirs( RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder )
		{
			if ( !( viewHolder is ContentBodyViewHolder contentHolder ) ) return base.GetDragDirs(recyclerView, viewHolder);

			if ( contentHolder.RowInfo != null )
			{
				Section? section = contentHolder.RowInfo.Section; // ?? throw new NullReferenceException(nameof(contentHolder.RowInfo.Section));

				if ( section != null &&
					 !section.UseDragSort ) { return 0; }

				if ( !contentHolder.RowInfo.Cell.IsEnabled ) { return 0; }

				// save start info.
				_FromInfo = contentHolder.RowInfo;
				System.Diagnostics.Debug.WriteLine($"DragDirs Section:{_SettingsView.Root.IndexOf(_FromInfo.Section)} Cell:{_FromInfo.Section.IndexOf(_FromInfo.Cell)}");
			}

			return base.GetDragDirs(recyclerView, viewHolder);
		}

		public override void OnSelectedChanged( RecyclerView.ViewHolder? viewHolder, int actionState )
		{
			base.OnSelectedChanged(viewHolder, actionState);
			if ( viewHolder is null ) { return; }

			if ( actionState != ItemTouchHelper.ActionStateDrag ) return;
			viewHolder.ItemView.Alpha = 0.9f;
			viewHolder.ItemView.ScaleX = 1.04f;
			viewHolder.ItemView.ScaleY = 1.04f;
		}


		public override void OnSwiped( RecyclerView.ViewHolder viewHolder, int direction ) { throw new NotImplementedException(); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_SettingsView = null;
				_moveHistory.Clear();
				_moveHistory = null;
			}

			base.Dispose(disposing);
		}
	}
}