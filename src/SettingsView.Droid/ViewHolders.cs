using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Xamarin.Forms;
using AView = Android.Views.View;

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	internal class CustomViewHolder : RecyclerView.ViewHolder
	{
		public RowInfo RowInfo { get; set; }

		public CustomViewHolder( AView view ) : base(view) { }

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
	internal interface IHeaderViewHolder { }

	[Android.Runtime.Preserve(AllMembers = true)]
	internal interface IFooterViewHolder { }
	[Android.Runtime.Preserve(AllMembers = true)]
	internal interface ICustomViewHolder
	{
		public LinearLayout Body { get; set; }
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	internal interface IDefaultViewHolder
	{
		public ImageView IconView { get; set; }
		public TextView TextView { get; set; }
	}




	[Android.Runtime.Preserve(AllMembers = true)]
	internal class HeaderViewHolder : CustomViewHolder, IHeaderViewHolder, IDefaultViewHolder
	{
		public ImageView IconView { get; set; }
		public TextView TextView { get; set; }

		public HeaderViewHolder( AView view ) : base(view)
		{
			TextView = view.FindViewById<TextView>(Resource.Id.HeaderCellTextDefault);
			IconView = view.FindViewById<ImageView>(Resource.Id.HeaderIconDefault);
		} 

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				TextView?.Dispose();
				TextView = null;
			}

			base.Dispose(disposing);
		}
	}
	[Android.Runtime.Preserve(AllMembers = true)]
	internal class FooterViewHolder : CustomViewHolder, IFooterViewHolder, IDefaultViewHolder
	{
		public ImageView IconView { get; set; }
		public TextView TextView { get; set; }

		public FooterViewHolder( AView view ) : base(view)
		{
			TextView = view.FindViewById<TextView>(Resource.Id.FooterCellTextDefault);
			IconView = view.FindViewById<ImageView>(Resource.Id.FooterIconDefault);
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				TextView?.Dispose();
				TextView = null;
			}

			base.Dispose(disposing);
		}
	}



	[Android.Runtime.Preserve(AllMembers = true)]
	internal class CustomHeaderViewHolder : CustomViewHolder, IHeaderViewHolder, ICustomViewHolder
	{
		public LinearLayout Body { get; set; }

		public CustomHeaderViewHolder( AView view ) : base(view) => Body = view.FindViewById<LinearLayout>(Resource.Id.HeaderStack);
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	internal class CustomFooterViewHolder : CustomViewHolder, IFooterViewHolder, ICustomViewHolder
	{
		public LinearLayout Body { get; set; }

		public CustomFooterViewHolder( AView view ) : base(view) => Body = view.FindViewById<LinearLayout>(Resource.Id.FooterStack);
	}



	[Android.Runtime.Preserve(AllMembers = true)]
	internal class ContentBodyViewHolder : CustomViewHolder
	{
		public LinearLayout Body { get; protected set; }

		public ContentBodyViewHolder( AView view ) : base(view) => Body = view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody);

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				AView nativeCell = Body.GetChildAt(0);
				if ( nativeCell is INativeElementView nativeElementView )
				{
					// If a ViewCell is used, it stops the ViewCellContainer from executing the dispose method.
					// Because if the AiForms.Effects is used and a ViewCellContainer is disposed, it crashes.
					if ( !( nativeElementView.Element is ViewCell ) ) { nativeCell?.Dispose(); }
				}

				Body?.Dispose();
				Body = null;
				ItemView.SetOnClickListener(null);
				ItemView.SetOnLongClickListener(null);
			}

			base.Dispose(disposing);
		}
	}
}