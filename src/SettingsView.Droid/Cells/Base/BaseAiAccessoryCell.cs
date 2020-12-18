using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiAccessoryCell<TAccessory> : BaseAiDescriptionCell where TAccessory : Android.Views.View
	{
		protected LinearLayout _AccessoryStack { get; }
		protected TAccessory _Accessory { get; }
		
		
		protected BaseAiAccessoryCell( Context context, Cell cell ) : base(context, cell)
		{
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			_Accessory = InstanceCreator<Context, TAccessory>.Create(AndroidContext);
			AddAccessory(_AccessoryStack, _Accessory);

			ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
			ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();
		}
		protected BaseAiAccessoryCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			_Accessory = InstanceCreator<Context, TAccessory>.Create(AndroidContext);
			AddAccessory(_AccessoryStack, _Accessory);

			ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
			ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();
		}


		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Accessory.Dispose();
			_AccessoryStack.Dispose();
		}
	}
}