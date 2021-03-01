using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Controls;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using AContext = Android.Content.Context;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiAccessoryCell<TAccessory> : BaseAiDescriptionCell where TAccessory : AView
	{
		protected LinearLayout _AccessoryStack { get; }
		protected TAccessory _Accessory { get; }


		protected BaseAiAccessoryCell( AContext context, Cell cell ) : base(context, cell)
		{
			ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
			ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();

			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			_Accessory = InstanceCreator<AContext, TAccessory>.Create(AndroidContext);

			_AccessoryStack.Add(_Accessory);
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