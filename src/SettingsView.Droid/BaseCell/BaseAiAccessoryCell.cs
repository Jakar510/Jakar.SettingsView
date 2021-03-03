using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using AContext = Android.Content.Context;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.BaseCell
{
	public abstract class BaseAiAccessoryCell<TAccessory> : BaseAiDescriptionCell where TAccessory : AView
	{
		protected LinearLayout _AccessoryStack { get; }
		protected TAccessory _Accessory { get; }


		protected BaseAiAccessoryCell( AContext context, Cell cell ) : base(context, cell)
		{
			RemoveHint();
			RemoveCellValueStack();

			_AccessoryStack = AccessoryStack();
			_Accessory = InstanceCreator<AContext, TAccessory>.Create(AndroidContext);
			_AccessoryStack.Add(_Accessory, Extensions.Layout.Wrap, Extensions.Layout.Wrap, GravityFlags.Center);
		}
		protected BaseAiAccessoryCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Accessory.Dispose();
			_AccessoryStack.Dispose();
		}
	}
}