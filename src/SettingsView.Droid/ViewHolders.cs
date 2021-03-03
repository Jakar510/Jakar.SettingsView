using System;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class CustomViewHolder : RecyclerView.ViewHolder
	{
		public RowInfo? RowInfo { get; set; }

		public CustomViewHolder( AView view ) : base(view) { }
		public CustomViewHolder( AView view, RowInfo info ) : base(view) => RowInfo = info;

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ItemView?.Dispose();
				ItemView = null;
			}

			base.Dispose(disposing);
		}
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	public interface IHeaderViewHolder { }

	[Android.Runtime.Preserve(AllMembers = true)]
	public interface IFooterViewHolder { }

	[Android.Runtime.Preserve(AllMembers = true)]
	public interface ICustomViewHolder { }

	[Android.Runtime.Preserve(AllMembers = true)]
	public interface IDefaultViewHolder
	{
		public ImageView IconView { get; set; }
		public TextView TextView { get; set; }
	}


	[Android.Runtime.Preserve(AllMembers = true)]
	public class HeaderViewHolder : CustomViewHolder, IHeaderViewHolder, IDefaultViewHolder
	{
		public ImageView IconView { get; set; }
		public TextView TextView { get; set; }

		public HeaderViewHolder( AView view ) : base(view)
		{
			TextView = view.FindViewById<TextView>(Resource.Id.HeaderCellTextDefault) ?? throw new NullReferenceException(nameof(view));
			IconView = view.FindViewById<ImageView>(Resource.Id.HeaderIconDefault) ?? throw new NullReferenceException(nameof(view));
		}
		public HeaderViewHolder( AView view, RowInfo info ) : base(view, info)
		{
			TextView = view.FindViewById<TextView>(Resource.Id.HeaderCellTextDefault) ?? throw new NullReferenceException(nameof(view));
			IconView = view.FindViewById<ImageView>(Resource.Id.HeaderIconDefault) ?? throw new NullReferenceException(nameof(view));
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { TextView.Dispose(); }

			base.Dispose(disposing);
		}
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	public class FooterViewHolder : CustomViewHolder, IFooterViewHolder, IDefaultViewHolder
	{
		public ImageView IconView { get; set; }
		public TextView TextView { get; set; }

		public FooterViewHolder( AView view ) : base(view)
		{
			TextView = view.FindViewById<TextView>(Resource.Id.FooterCellTextDefault) ?? throw new NullReferenceException(nameof(view));
			IconView = view.FindViewById<ImageView>(Resource.Id.FooterIconDefault) ?? throw new NullReferenceException(nameof(view));
		}
		public FooterViewHolder( AView view, RowInfo info ) : base(view, info)
		{
			TextView = view.FindViewById<TextView>(Resource.Id.FooterCellTextDefault) ?? throw new NullReferenceException(nameof(view));
			IconView = view.FindViewById<ImageView>(Resource.Id.FooterIconDefault) ?? throw new NullReferenceException(nameof(view));
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { TextView.Dispose(); }

			base.Dispose(disposing);
		}
	}


	[Android.Runtime.Preserve(AllMembers = true)]
	public class CustomHeaderViewHolder : CustomViewHolder, IHeaderViewHolder, ICustomViewHolder
	{
		public new HeaderFooterContainer? ItemView
		{
			get => base.ItemView as HeaderFooterContainer;
			set => base.ItemView = value;
		}
		public CustomHeaderViewHolder( AView view ) : base(view) { }
		public CustomHeaderViewHolder( AView view, RowInfo info ) : base(view, info) { }
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	public class CustomFooterViewHolder : CustomViewHolder, IFooterViewHolder, ICustomViewHolder
	{
		public new HeaderFooterContainer? ItemView
		{
			get => base.ItemView as HeaderFooterContainer;
			set => base.ItemView = value;
		}

		public CustomFooterViewHolder( AView view ) : base(view) { }
		public CustomFooterViewHolder( AView view, RowInfo info ) : base(view, info) { }
	}


	[Android.Runtime.Preserve(AllMembers = true)]
	public class ContentBodyViewHolder : CustomViewHolder
	{
		public LinearLayout Body { get; protected set; }

		public ContentBodyViewHolder( AView view ) : base(view) => Body = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody) ?? throw new NullReferenceException(nameof(Resource.Id.ContentCellBody));
		public ContentBodyViewHolder( AView view, RowInfo info ) : this(view) => RowInfo = info;
		
		// public ContentBodyViewHolder( AView view, Context context ) : base(view) => Body = new FormsViewContainer(context);
		// public ContentBodyViewHolder( AView view, RowInfo info, Context context ) : base(view, info) => Body = new FormsViewContainer(context);

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				AView? nativeCell = Body.GetChildAt(0);
				if ( nativeCell is INativeElementView nativeElementView )
				{
					// If a ViewCell is used, it stops the ViewCellContainer from executing the dispose method.
					// Because if the AiForms.Effects is used and a ViewCellContainer is disposed, it crashes.
					if ( !( nativeElementView.Element is ViewCell ) ) { nativeCell.Dispose(); }
				}

				Body.Dispose();
				ItemView.SetOnClickListener(null);
				ItemView.SetOnLongClickListener(null);
			}

			base.Dispose(disposing);
		}
	}
}