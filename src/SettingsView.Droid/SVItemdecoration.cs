using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SVItemdecoration : RecyclerView.ItemDecoration
	{
		private Drawable _drawable;
		private Shared.SettingsView _settingsView;

		public SVItemdecoration( Drawable drawable, Shared.SettingsView settingsView )
		{
			_drawable = drawable;
			_settingsView = settingsView;
		}

		public override void GetItemOffsets( Rect outRect,
											 View view,
											 RecyclerView parent,
											 RecyclerView.State state )
		{
			outRect.Set(0, _drawable.IntrinsicHeight, 0, 0);
		}

		public override void OnDraw( Canvas c, RecyclerView parent, RecyclerView.State state )
		{
			int left = parent.Left;
			int right = parent.Right;

			int childCount = parent.ChildCount;
			CustomViewHolder prevHolder = null;
			for ( var i = 0; i < childCount; i++ )
			{
				View? child = parent.GetChildAt(i);
				var holder = parent.GetChildViewHolder(child) as CustomViewHolder;

				if ( prevHolder != null && prevHolder is IHeaderViewHolder && !_settingsView.ShowSectionTopBottomBorder ||
					 holder is IFooterViewHolder && !_settingsView.ShowSectionTopBottomBorder ||
					 holder is IFooterViewHolder && !holder.RowInfo.Section.Any() ||
					 holder is IHeaderViewHolder ||
					 !holder.RowInfo.Section.IsVisible )
				{
					prevHolder = holder;
					continue;
				}

				int bottom = child.Top;
				int top = bottom - _drawable.IntrinsicHeight;
				_drawable.SetBounds(left, top, right, bottom);
				_drawable.Draw(c);

				prevHolder = holder;
			}
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_settingsView = null;
				_drawable = null;
			}

			base.Dispose(disposing);
		}
	}
}