using System;
using System.ComponentModel;
using Android.Runtime;
using Jakar.SettingsView.Droid.Controls;
using Xamarin.Forms;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.BaseCell
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public abstract class BaseAiValueCell : BaseValueCell<ValueView> //<TCellTitle, TCell> : CellBaseView where TCell: TextView where TCellTitle : BaseView<TCell>, new()
	{
		protected BaseAiValueCell( AContext context, Cell cell ) : base(context, cell) { }
		protected BaseAiValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Value.Update(sender, e) ) return;
			if ( _Hint.Update(sender, e) ) return;
			base.CellPropertyChanged(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Value.UpdateParent(sender, e) ) return;
			if ( _Hint.UpdateParent(sender, e) ) return;
			base.ParentPropertyChanged(sender, e);
		}

		// protected void UpdateUseDescriptionAsValue()
		// {
		// 	if ( !_LabelCell.IgnoreUseDescriptionAsValue &&
		// 		 CellParent != null &&
		// 		 CellParent.UseDescriptionAsValue )
		// 	{
		// 		// _Value = DescriptionLabel;
		// 		_Description.Label.Visibility = ViewStates.Visible;
		// 		_Value.Label.Visibility = ViewStates.Gone;
		// 	}
		// 	else
		// 	{
		// 		// _Value = _Value.Label;
		// 		_Value.Label.Visibility = ViewStates.Visible;
		// 	}
		// }

		protected override void EnableCell()
		{
			base.EnableCell();
			_Hint.Enable();
			_Value.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Hint.Disable();
			_Value.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Hint.Update();
			_Value.Update();
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Hint.Dispose();
			_Value.Dispose();
			_CellValueStack.Dispose();
		}
	}
}