using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.RecyclerView.Widget;
using Java.Interop;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Preserve(AllMembers = true)]
	public class SettingsViewLayoutManager : GridLayoutManager
	{
		protected Shared.SettingsView? _SettingsView { get; set; }
		protected Context? _Context { get; set; }
		protected Dictionary<Android.Views.View, int>? _ItemHeights { get; set; } = new Dictionary<Android.Views.View, int>();


		public SettingsViewLayoutManager( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
		public SettingsViewLayoutManager( Context context, int spanCount ) : base(context, spanCount) { _Context = context; }
		public SettingsViewLayoutManager( Context context,
										  IAttributeSet attrs,
										  int defStyleAttr,
										  int defStyleRes ) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			_Context = context ?? throw new NullReferenceException(nameof(context));
		}
		public SettingsViewLayoutManager( Context context,
										  int spanCount,
										  int orientation,
										  bool reverseLayout ) : base(context, spanCount, orientation, reverseLayout)
		{
			_Context = context ?? throw new NullReferenceException(nameof(context));
		}
		public SettingsViewLayoutManager( Context context, Shared.SettingsView settingsView ) : this(context, 3, Horizontal, false)
		{
			_Context = context ?? throw new NullReferenceException(nameof(context));
			_SettingsView = settingsView ?? throw new NullReferenceException(nameof(settingsView));
		}


		public override int GetDecoratedMeasuredHeight( Android.Views.View child )
		{
			int height = base.GetDecoratedMeasuredHeight(child);
			if ( _ItemHeights != null ) _ItemHeights[child] = height;
			return height;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_ItemHeights?.Clear();
				_ItemHeights = null;
				_Context = null;
				_SettingsView = null;
			}

			base.Dispose(disposing);
		}

		public override void OnLayoutCompleted( RecyclerView.State state )
		{
			base.OnLayoutCompleted(state);

			int total = _ItemHeights?.Sum(x => x.Value) ?? 0;

			if ( _SettingsView != null ) _SettingsView.VisibleContentHeight = _Context.FromPixels(total);
		}
	}
}