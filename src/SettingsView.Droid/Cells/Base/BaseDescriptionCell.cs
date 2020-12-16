using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseDescriptionCell : BaseTitledCell
	{
		protected IconView _Icon { get; }
		protected DescriptionView _Description { get; }

		protected BaseDescriptionCell( Context context, Cell cell ) : base(context, cell)
		{
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CellIcon));
			_Description = BaseTextView.Create<DescriptionView>(ContentView, this, Resource.Id.CellDescription);
		}
		protected BaseDescriptionCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CellIcon));
			_Description = BaseTextView.Create<DescriptionView>(ContentView, this, Resource.Id.CellDescription);
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Icon.Dispose();
			_Description.Dispose();
		}
	}
}