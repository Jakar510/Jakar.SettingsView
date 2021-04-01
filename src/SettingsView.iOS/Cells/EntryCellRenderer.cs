using System;
using CoreGraphics;
using Foundation;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.CellBase;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
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
	public class EntryCellView : BaseValueCell<AiEditText>
	{
		private AiEntryCell _EntryCell => Cell as AiEntryCell ?? throw new NullReferenceException(nameof(_EntryCell));
		internal UITextField ValueField { get; set; }
		private UIView _FieldWrapper { get; set; }
		private bool _hasFocus;


		public EntryCellView( Cell formsCell ) : base(formsCell)
		{
			ValueField = new UITextField
						 {
							 BorderStyle = UITextBorderStyle.None,
							 AutoresizingMask = UIViewAutoresizing.FlexibleDimensions,
							 ReturnKeyType = UIReturnKeyType.Done
						 };

			ValueField.EditingChanged += TextField_EditingChanged;
			ValueField.EditingDidBegin += ValueField_EditingDidBegin;
			ValueField.EditingDidEnd += ValueField_EditingDidEnd;
			ValueField.ShouldReturn = OnShouldReturn;

			_EntryCell.Focused += EntryCell_Focused;


			_FieldWrapper = new UIView
							{
								AutosizesSubviews = true
							};
			_FieldWrapper.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			_FieldWrapper.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);

			_FieldWrapper.AddSubview(ValueField);
			_ValueStack.AddArrangedSubview(_FieldWrapper);
		}


		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);

			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFont();
			UpdatePlaceholder();
			UpdateKeyboard();
			UpdateIsPassword();
			UpdateTextAlignment();
		}

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

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { ValueField.BecomeFirstResponder(); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ValueField.EditingChanged -= TextField_EditingChanged;
				ValueField.EditingDidBegin -= ValueField_EditingDidBegin;
				ValueField.EditingDidEnd -= ValueField_EditingDidEnd;
				_EntryCell.Focused -= EntryCell_Focused;
				ValueField.ShouldReturn = null;
				ValueField.RemoveFromSuperview();
				ValueField.Dispose();

				_ValueStack.RemoveArrangedSubview(_FieldWrapper);
				_FieldWrapper.Dispose();
			}

			base.Dispose(disposing);
		}

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
			ValueCellBase.ValueTextConfiguration config = _EntryCell.ValueTextConfig;
			string? family = config.FontFamily;
			FontAttributes attr = config.FontAttributes;

			ValueField.Font = FontUtility.CreateNativeFont(family, config.FontSize.ToFloat(), attr);

			//make the view height fit font size
			nfloat contentH = ValueField.IntrinsicContentSize.Height;
			CGRect bounds = ValueField.Bounds;
			ValueField.Bounds = new CGRect(0, 0, bounds.Width, contentH);
			_FieldWrapper.Bounds = new CGRect(0, 0, _FieldWrapper.Bounds.Width, contentH);
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


		private void TextField_EditingChanged( object sender, EventArgs e ) { _EntryCell.ValueText = ValueField.Text; }
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