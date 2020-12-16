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
	public class EntryCellView : BaseEntryCell
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


		/*
		 
		protected override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Hint.Update(sender, e) ) { return; }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Hint.Enable();
			Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Hint.Disable();
			Disable();
		}

		 */

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( _Value.Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Hint.Update(sender, e) ) { return; }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Value.UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }
		}


		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Value.Update();
		}


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