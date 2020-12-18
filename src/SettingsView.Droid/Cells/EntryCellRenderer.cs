using System;
using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Shared.Cells;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.Droid.Cells.EntryCellRenderer;
using Object = Java.Lang.Object;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Preserve(AllMembers = true)]
	public class EntryCellView : BaseAiEntryCell
	{
		public EntryCellView( Context context, Cell cell ) : base(context, cell)
		{
			Click += EntryCellView_Click;
			Click += EditTextOnClick;
			_EntryCell.Focused += EntryCell_Focused;
		}
		public EntryCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Value.Init(_EntryCell, this);

			Click += EntryCellView_Click;
			Click += EditTextOnClick;
			_EntryCell.Focused += EntryCell_Focused;
		}

		// protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.CellPropertyChanged(sender, e); }
		// protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.ParentPropertyChanged(sender, e); }
		// protected internal override void UpdateCell() { base.UpdateCell(); }


		protected void EditTextOnClick( object sender, EventArgs e ) { _Value.PerformSelectAction(); }
		protected void EntryCellView_Click( object sender, EventArgs e )
		{
			RequestFocus();
			_Value.PerformSelectAction();
			ShowKeyboard(_Value); // EntryCellView_Click
		}
		protected void EntryCell_Focused( object sender, EventArgs e )
		{
			RequestFocus();
			ShowKeyboard(_Value);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				Click -= EntryCellView_Click;
				_EntryCell.Focused -= EntryCell_Focused;
				_Value.RemoveFromParent();
				_Value.SetOnEditorActionListener(null);
				_Value.RemoveTextChangedListener(this);
				OnFocusChangeListener = null;
				_Value.ClearFocusAction = null;
				_Value.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}