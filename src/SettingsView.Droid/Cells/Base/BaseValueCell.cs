// unset

using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Controls;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RelativeLayout = Android.Widget.RelativeLayout;
using AContext = Android.Content.Context;

namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseValueCell<TCell> : BaseAiDescriptionCell where TCell : TextView
	{
		protected HintView _Hint { get; }
		protected TCell _Value { get; }
		protected RelativeLayout _CellValueStack { get; }
		protected BaseValueCell( AContext context, Cell cell ) : base(context, cell)
		{
			ContentView.FindViewById<RelativeLayout>(Resource.Id.CellAccessoryStack)?.RemoveFromParent();
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ContentView.FindViewById<RelativeLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			_Value = InstanceCreator<BaseCellView, AContext, TCell>.Create(this, AndroidContext);
			_CellValueStack.Add(_Value);
		}
		protected BaseValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
	}
}