using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Jakar.SettingsView.Droid.Cells.Controls;
using Xamarin.Forms;
using AContext = Android.Content.Context;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseAiValueCell : BaseValueCell<ValueView> //<TCellTitle, TCell> : CellBaseView where TCell: TextView where TCellTitle : BaseView<TCell>, new()
	{
		protected BaseAiValueCell( AContext context, Cell cell ) : base(context, cell) { }
		protected BaseAiValueCell( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			_Value.Update(sender, e);
			_Hint.Update(sender, e);
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			_Value.UpdateParent(sender, e);
			_Hint.UpdateParent(sender, e);
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
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