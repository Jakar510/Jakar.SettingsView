using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.Controls;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.Misc;
using Xamarin.Forms;
using AContext = Android.Content.Context;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.BaseCell
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public abstract class BaseValueCell<TCell> : BaseAiDescriptionCell where TCell : AView
	{
		protected HintView _Hint { get; }
		protected TCell _Value { get; }
		protected LinearLayout _CellValueStack { get; }


		protected BaseValueCell( AContext context, Cell cell ) : base(context, cell)
		{
			RemoveAccessoryStack();
			_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
			_CellValueStack = ValueStack();

			_Value = InstanceCreator<BaseCellView, AContext, TCell>.Create(this, AndroidContext);
			_CellValueStack.Add(_Value, Extensions.Layout.Match, Extensions.Layout.Wrap, GravityFlags.Fill | GravityFlags.Center);
		}
		protected BaseValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
	}
	// public abstract class BaseValueCell : BaseAiDescriptionCell
	// {
	// 	protected HintView _Hint { get; }
	// 	protected ValueView _Value { get; }
	//
	//
	// 	protected BaseValueCell( AContext context, Cell cell ) : base(context, cell)
	// 	{
	// 		RemoveAccessoryStack();
	// 		RemoveEntryValue();
	// 		_Hint = BaseTextView.Create<HintView>(ContentView, this, Resource.Id.CellHint);
	// 		_Value = BaseTextView.Create<ValueView>(ContentView, this, Resource.Id.CellValue);
	// 	}
	// 	protected BaseValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
	// }
}