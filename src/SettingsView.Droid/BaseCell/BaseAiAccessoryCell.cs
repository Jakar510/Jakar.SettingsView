using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.Api.Droid.Extensions;
using Jakar.Api.Extensions;
using Xamarin.Forms;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using AExtensions = Jakar.Api.Droid.Extensions;

#nullable enable
namespace Jakar.SettingsView.Droid.BaseCell
{
	[Android.Runtime.Preserve(AllMembers = true)]
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
			_AccessoryStack.Add(_Accessory, AExtensions.Layout.Wrap, AExtensions.Layout.Wrap, GravityFlags.Center);
		}
#pragma warning disable 8618
		protected BaseAiAccessoryCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
#pragma warning restore 8618


		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Accessory.Dispose();
			_AccessoryStack.Dispose();
		}
	}
}