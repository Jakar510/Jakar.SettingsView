using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Controls;
using Xamarin.Forms;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiDescriptionCell : BaseAiTitledCell
	{
		protected IconView _Icon { get; }
		protected DescriptionView _Description { get; }

		protected BaseAiDescriptionCell( AContext context, Cell cell ) : base(context, cell)
		{
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CellIcon));
			_Description = BaseTextView.Create<DescriptionView>(ContentView, this, Resource.Id.CellDescription);
		}
		protected BaseAiDescriptionCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			_Icon.Update(sender, e);
			_Description.Update(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			// _Icon.UpdateParent(sender, e);
			_Description.UpdateParent(sender, e);
			_Icon.UpdateParent(sender, e);
		}

		protected override void EnableCell()
		{
			base.EnableCell();
			_Description.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Description.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Icon.Update();
			_Description.Update();
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Icon.Dispose();
			_Description.Dispose();
		}
	}
}