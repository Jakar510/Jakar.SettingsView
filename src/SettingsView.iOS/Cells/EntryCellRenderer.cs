using System;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.iOS.Cells.EntryCellRenderer;

[assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Entry cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	/// <summary>
	/// Entry cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class EntryCellView : BaseCellView
	{
		private AiEntryCell _EntryCell => Cell as AiEntryCell;
		internal UITextField ValueField;
		private UIView _FieldWrapper;
		private bool _hasFocus = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.EntryCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public EntryCellView( Cell formsCell ) : base(formsCell)
		{
			ValueField = new UITextField()
						 {
							 BorderStyle = UITextBorderStyle.None
						 };
			ValueField.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			ValueField.ReturnKeyType = UIReturnKeyType.Done;
			ValueField.EditingChanged += _textField_EditingChanged;
			ValueField.EditingDidBegin += ValueField_EditingDidBegin;
			ValueField.EditingDidEnd += ValueField_EditingDidEnd;
			ValueField.ShouldReturn = OnShouldReturn;

			_EntryCell.Focused += EntryCell_Focused;


			_FieldWrapper = new UIView();
			_FieldWrapper.AutosizesSubviews = true;
			_FieldWrapper.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			_FieldWrapper.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);

			_FieldWrapper.AddSubview(ValueField);
			_ContentStack.AddArrangedSubview(_FieldWrapper);
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);

			if ( ValueField is null )
				return; // For HotReload

			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFont();
			UpdatePlaceholder();
			UpdateKeyboard();
			UpdateIsPassword();
			UpdateTextAlignment();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == ValueTextCellBase.ValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == ValueCellBase.ValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == ValueCellBase.ValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == ValueCellBase.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
			else if ( e.PropertyName == ValueCellBase.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == ValueCellBase.ValueTextAlignmentProperty.PropertyName ) { UpdateTextAlignment(); }

			else if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { UpdateKeyboard(); }

			else if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
					  e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { UpdatePlaceholder(); }

			else if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { UpdateIsPassword(); }
		}

		/// <summary>
		/// Parents the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName )
			{
				UpdateValueTextColor();
				ValueField.SetNeedsLayout(); // immediately reflect
			}
			else if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { ValueField.BecomeFirstResponder(); }

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ValueField.EditingChanged -= _textField_EditingChanged;
				ValueField.EditingDidBegin -= ValueField_EditingDidBegin;
				ValueField.EditingDidEnd -= ValueField_EditingDidEnd;
				_EntryCell.Focused -= EntryCell_Focused;
				ValueField.ShouldReturn = null;
				ValueField.RemoveFromSuperview();
				ValueField.Dispose();
				ValueField = null;

				_ContentStack?.RemoveArrangedSubview(_FieldWrapper);
				_FieldWrapper.Dispose();

				_FieldWrapper = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { ValueField.Alpha = 1.0f; }
			else { ValueField.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		private void UpdateValueText()
		{
			//Without this judging, TextField don't correctly work when inputting Japanese (maybe other 2byte languages either).
			if ( ValueField.Text != _EntryCell.ValueText ) { ValueField.Text = _EntryCell.ValueText; }
		}

		private void UpdateValueTextFont()
		{
			var family = _EntryCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			var attr = _EntryCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			if ( _EntryCell.ValueTextFontSize > 0 ) { ValueField.Font = FontUtility.CreateNativeFont(family, (float) _EntryCell.ValueTextFontSize, attr); }
			else if ( CellParent is not null ) { ValueField.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellValueTextFontSize, attr); }

			//make the view height fit font size
			var contentH = ValueField.IntrinsicContentSize.Height;
			var bounds = ValueField.Bounds;
			ValueField.Bounds = new CoreGraphics.CGRect(0, 0, bounds.Width, contentH);
			_FieldWrapper.Bounds = new CoreGraphics.CGRect(0, 0, _FieldWrapper.Bounds.Width, contentH);
		}

		private void UpdateValueTextColor()
		{
			if ( _EntryCell.ValueTextColor != Color.Default ) { ValueField.TextColor = _EntryCell.ValueTextColor.ToUIColor(); }
			else if ( CellParent is not null &&
					  CellParent.CellValueTextColor != Color.Default ) { ValueField.TextColor = CellParent.CellValueTextColor.ToUIColor(); }

			ValueField.SetNeedsLayout();
		}

		private void UpdateKeyboard() { ValueField.ApplyKeyboard(_EntryCell.Keyboard); }

		private void UpdatePlaceholder()
		{
			if ( !_EntryCell.PlaceholderColor.IsDefault )
			{
				ValueField.Placeholder = null;
				ValueField.AttributedPlaceholder = new NSAttributedString(_EntryCell.Placeholder, ValueField.Font, _EntryCell.PlaceholderColor.ToUIColor());
			}
			else
			{
				ValueField.AttributedPlaceholder = null;
				ValueField.Placeholder = _EntryCell.Placeholder;
			}
		}

		private void UpdateTextAlignment()
		{
			ValueField.TextAlignment = _EntryCell.ValueTextConfig.TextAlignment.ToUITextAlignment();
			ValueField.SetNeedsLayout();
		}

		private void UpdateIsPassword() { ValueField.SecureTextEntry = _EntryCell.IsPassword; }


		private void _textField_EditingChanged( object sender, EventArgs e ) { _EntryCell.ValueText = ValueField.Text; }


		private void ValueField_EditingDidBegin( object sender, EventArgs e ) { _hasFocus = true; }


		private void ValueField_EditingDidEnd( object sender, EventArgs e )
		{
			if ( !_hasFocus ) { return; }

			ValueField.ResignFirstResponder();
			_EntryCell.SendCompleted();
			_hasFocus = false;
		}

		private void EntryCell_Focused( object sender, EventArgs e ) { ValueField.BecomeFirstResponder(); }


		private bool OnShouldReturn( UITextField view )
		{
			_hasFocus = false;
			ValueField.ResignFirstResponder();
			_EntryCell.SendCompleted();
			return true;
		}
	}
}