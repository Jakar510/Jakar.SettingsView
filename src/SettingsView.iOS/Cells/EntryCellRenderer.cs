using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.iOS.Interfaces;
using UIKit;
using Xamarin.Forms;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.iOS.Cells.EntryCellRenderer;

#nullable enable
[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Entry cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Preserve(AllMembers = true)]
	public class EntryCellView : BaseValueCell<UITextField, AiEntryCell, AiEditText>, IEntryCellRenderer
	{
		public EntryCellView( AiEntryCell formsCell ) : base(formsCell)
		{
			// _Value.EditingChanged += TextField_EditingChanged;
			// _Value.EditingDidBegin += Value_EditingDidBegin;
			// _Value.EditingDidEnd += Value_EditingDidEnd;
			// _Value.ShouldReturn = OnShouldReturn;

			// EntryCell.Focused += EntryCell_Focused;
		}


		// public override void UpdateCell( UITableView tableView )
		// {
		// 	base.UpdateCell(tableView);
		// 	_Value.Update();
		// }


		// public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		// {
		// 	base.CellPropertyChanged(sender, e);
		// 	if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { UpdateValueText(); }
		// 	else if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ||
		// 			  e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
		// 			  e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		// 	else if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
		// 	else if ( e.PropertyName == ValueCellBase.ValueTextAlignmentProperty.PropertyName ) { UpdateTextAlignment(); }
		//
		// 	else if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { UpdateKeyboard(); }
		//
		// 	else if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
		// 			  e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { UpdatePlaceholder(); }
		//
		// 	else if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { UpdateIsPassword(); }
		// }
		//
		// public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		// {
		// 	base.ParentPropertyChanged(sender, e);
		// 	if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName )
		// 	{
		// 		UpdateValueTextColor();
		// 		_Value.SetNeedsLayout(); // immediately reflect
		// 	}
		// 	else if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ||
		// 			  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
		// 			  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		// }

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { _Value.Control.BecomeFirstResponder(); }

		// protected override void SetEnabledAppearance( bool isEnabled )
		// {
		// 	if ( isEnabled ) { _Value.Alpha = 1.0f; }
		// 	else { _Value.Alpha = 0.3f; }
		//
		// 	base.SetEnabledAppearance(isEnabled);
		// }

		// private void UpdateValueText()
		// {
		// 	//Without this judging, TextField don't correctly work when inputting Japanese (maybe other 2byte languages either).
		// 	if ( _Value.Text != EntryCell.ValueText ) { _Value.Text = EntryCell.ValueText; }
		// }
		//
		// private void UpdateValueTextFont()
		// {
		// 	ValueCellBase.ValueTextConfiguration config = EntryCell.ValueTextConfig;
		// 	string? family = config.FontFamily;
		// 	FontAttributes attr = config.FontAttributes;
		//
		// 	_Value.Font = FontUtility.CreateNativeFont(family, config.FontSize.ToFloat(), attr);
		//
		// 	//make the view height fit font size
		// 	nfloat contentH = _Value.IntrinsicContentSize.Height;
		// 	CGRect bounds = _Value.Bounds;
		// 	_Value.Bounds = new CGRect(0, 0, bounds.Width, contentH);
		// 	// _FieldWrapper.Bounds = new CGRect(0, 0, _FieldWrapper.Bounds.Width, contentH);
		// }
		//
		// private void UpdateValueTextColor()
		// {
		// 	if ( EntryCell.ValueTextColor != Color.Default ) { _Value.TextColor = EntryCell.ValueTextColor.ToUIColor(); }
		// 	else if ( CellParent is not null &&
		// 			  CellParent.CellValueTextColor != Color.Default ) { _Value.TextColor = CellParent.CellValueTextColor.ToUIColor(); }
		//
		// 	_Value.SetNeedsLayout();
		// }
		//
		// private void UpdateKeyboard() { _Value.ApplyKeyboard(EntryCell.Keyboard); }
		//
		// private void UpdatePlaceholder()
		// {
		// 	if ( !EntryCell.PlaceholderColor.IsDefault )
		// 	{
		// 		_Value.Placeholder = null;
		// 		_Value.AttributedPlaceholder = new NSAttributedString(EntryCell.Placeholder, _Value.Font, EntryCell.PlaceholderColor.ToUIColor());
		// 	}
		// 	else
		// 	{
		// 		_Value.AttributedPlaceholder = null;
		// 		_Value.Placeholder = EntryCell.Placeholder;
		// 	}
		// }
		//
		// private void UpdateTextAlignment()
		// {
		// 	_Value.TextAlignment = EntryCell.ValueTextConfig.TextAlignment.ToUITextAlignment();
		// 	_Value.SetNeedsLayout();
		// }
		//
		// private void UpdateIsPassword() { _Value.SecureTextEntry = EntryCell.IsPassword; }


		// private void TextField_EditingChanged( object sender, EventArgs e ) { EntryCell.ValueText = _Value.Text; }
		// private void Value_EditingDidBegin( object sender, EventArgs e ) { _hasFocus = true; }
		// private void Value_EditingDidEnd( object sender, EventArgs e )
		// {
		// 	if ( !_hasFocus ) { return; }
		//
		// 	_Value.ResignFirstResponder();
		// 	EntryCell.SendCompleted();
		// 	_hasFocus = false;
		// }
		// private void EntryCell_Focused( object sender, EventArgs e ) { _Value.BecomeFirstResponder(); }


		// private bool OnShouldReturn( UITextField view )
		// {
		// 	_hasFocus = false;
		// 	_Value.ResignFirstResponder();
		// 	EntryCell.SendCompleted();
		// 	return true;
		// }

		public void DoneEdit() { Cell.SendCompleted(); }

		// protected override void RunDispose()
		// {
		// 	base.RunDispose();
		//
		// }
		// protected override void Dispose( bool disposing )
		// {
		// 	if ( disposing )
		// 	{
		// 		_Value.Dispose();
		// 	}
		//
		// 	base.Dispose(disposing);
		// }
	}
}