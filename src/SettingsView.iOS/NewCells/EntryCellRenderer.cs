using System;
using System.ComponentModel;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.iOS.Interfaces;
using UIKit;
using Xamarin.Forms;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.iOS.NewCells.EntryCellRenderer;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.NewCells
{
	[Preserve(AllMembers = true)] public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Preserve(AllMembers = true)]
	public class EntryCellView : BaseValueCell<AiEditText>, IEntryCellRenderer
	{
		protected bool _HasFocus { get; set; }
		protected AiEntryCell _EntryCell => Cell as AiEntryCell ?? throw new NullReferenceException(nameof(_EntryCell));

		public EntryCellView( Cell cell ) : base(cell)
		{
			_EntryCell.Focused += EntryCell_Focused;

			_Value.Init(_EntryCell, this);
			_Value.TouchUpInside += ValueFieldOnTouchUpInside;
			_Value.EditingChanged += TextField_EditingChanged;
			_Value.EditingDidBegin += ValueField_EditingDidBegin;
			_Value.EditingDidEnd += ValueField_EditingDidEnd;
			_Value.ShouldReturn = OnShouldReturn;
		}


		protected void ValueFieldOnTouchUpInside( object sender, EventArgs e ) { _Value.PerformSelectAction(); }
		protected void TextField_EditingChanged( object sender, EventArgs e )
		{
			_EntryCell.ValueText = _Value.Text;
			_EntryCell.ValueChangedHandler.SendValueChanged(_Value.Text);
		}
		protected void ValueField_EditingDidBegin( object sender, EventArgs e ) { _HasFocus = true; }
		protected void ValueField_EditingDidEnd( object sender, EventArgs e ) { DoneEdit(); }
		protected void EntryCell_Focused( object sender, EventArgs e ) { _Value.BecomeFirstResponder(); }
		protected bool OnShouldReturn( UITextField view )
		{
			_HasFocus = false;
			_Value.ResignFirstResponder();
			_EntryCell.SendCompleted();

			return true;
		}

		public void DoneEdit()
		{
			if ( !_HasFocus ) { return; }

			_Value.ResignFirstResponder();
			_EntryCell.SendCompleted();
			_HasFocus = false;
		}


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


		protected override void EnableCell()
		{
			base.EnableCell();
			_Value.Enable();
			_Hint.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Value.Disable();
			_Hint.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Hint.Update();
			_Value.Update();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Value.TouchUpInside -= ValueFieldOnTouchUpInside;
				_Value.EditingChanged -= TextField_EditingChanged;
				_Value.EditingDidBegin -= ValueField_EditingDidBegin;
				_Value.EditingDidEnd -= ValueField_EditingDidEnd;
				_EntryCell.Focused -= EntryCell_Focused;

				_Value.ShouldReturn = null;

				_Value.RemoveFromSuperview();
				_Value.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}