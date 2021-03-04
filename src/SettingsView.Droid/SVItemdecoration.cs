using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.RecyclerView.Widget;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SVItemDecoration : RecyclerView.ItemDecoration
	{
		private Drawable _Drawable { get; set; }
		private Shared.sv.SettingsView _SettingsView { get; set; }

		public SVItemDecoration( Drawable drawable, Shared.sv.SettingsView settingsView )
		{
			_Drawable = drawable;
			_SettingsView = settingsView;
		}

		public override void GetItemOffsets( Rect outRect,
											 View view,
											 RecyclerView parent,
											 RecyclerView.State state )
		{
			outRect.Set(0, _Drawable.IntrinsicHeight, 0, 0);
		}

		public override void OnDraw( Canvas c, RecyclerView parent, RecyclerView.State state )
		{
			int left = parent.Left;
			int right = parent.Right;

			int childCount = parent.ChildCount;
			CustomViewHolder? prevHolder = null;
			for ( var i = 0; i < childCount; i++ )
			{
				View? child = parent.GetChildAt(i);

				if ( child is null ) continue;
				if ( !( parent.GetChildViewHolder(child) is CustomViewHolder holder ) ) continue;
				if ( holder.RowInfo is null ) continue;
				if ( ( prevHolder is IHeaderViewHolder && !_SettingsView.ShowSectionTopBottomBorder ) ||
					 ( holder is IFooterViewHolder && !_SettingsView.ShowSectionTopBottomBorder ) ||
					 ( holder is IFooterViewHolder && !holder.RowInfo.Section.Any() ) ||
					 ( holder is IHeaderViewHolder || !holder.RowInfo.Section.IsVisible ) )
				{
					prevHolder = holder;
					continue;
				}

				int bottom = child.Top;
				int top = bottom - _Drawable.IntrinsicHeight;
				_Drawable.SetBounds(left, top, right, bottom);
				_Drawable.Draw(c);

				prevHolder = holder;
			}
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { }

			base.Dispose(disposing);
		}
	}
}