using System;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using AiEntryCell = Jakar.SettingsView.Shared.Cells.EntryCell;
using EntryCellRenderer = Jakar.SettingsView.iOS.OLD_Cells.EntryCellRenderer;

// [assembly: ExportRenderer(typeof(AiEntryCell), typeof(EntryCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class EntryCellRenderer : CellBaseRenderer<EntryCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class EntryCellView : CellBaseView
	{
		protected AiEntryCell _EntryCell => Cell as AiEntryCell;
		internal UITextField _Value { get; set; }
		protected UIView _FieldWrapper { get; set; }
		protected bool _HasFocus { get; set; }

		public EntryCellView( Cell formsCell ) : base(formsCell)
		{
			_Value = new UITextField // TODO: enable multi-line entries
						 {
							 BorderStyle = UITextBorderStyle.None,
							 AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
							 ReturnKeyType = UIReturnKeyType.Done,
						 };

			_Value.TouchUpInside += ValueFieldOnTouchUpInside;
			_Value.EditingChanged += TextField_EditingChanged;
			_Value.EditingDidBegin += ValueField_EditingDidBegin;
			_Value.EditingDidEnd += ValueField_EditingDidEnd;
			_Value.ShouldReturn = OnShouldReturn;

			_EntryCell.Focused += EntryCell_Focused;


			_FieldWrapper = new UIView
							{
								AutosizesSubviews = true
							};
			_FieldWrapper.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			_FieldWrapper.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);

			_FieldWrapper.AddSubview(_Value);
			_ContentStack.AddArrangedSubview(_FieldWrapper);
		}

		public override void UpdateCell( UITableView tableView = null )
		{
			base.UpdateCell(tableView);

			if ( _Value is null ) ; // For HotReload

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
			else if ( e.PropertyName == AiEntryCell.KeyboardProperty.PropertyName ) { UpdateKeyboard(); }
			else if ( e.PropertyName == AiEntryCell.PlaceholderProperty.PropertyName ||
					  e.PropertyName == AiEntryCell.PlaceholderColorProperty.PropertyName ) { UpdatePlaceholder(); }
			else if ( e.PropertyName == AiEntryCell.TextAlignmentProperty.PropertyName ) { UpdateTextAlignment(); }
			else if ( e.PropertyName == AiEntryCell.IsPasswordProperty.PropertyName ) { UpdateIsPassword(); }
			else if ( e.PropertyName == AiEntryCell.OnSelectActionProperty.PropertyName ) { UpdateSelectAction(); }
		}

		public override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName )
			{
				UpdateValueTextColor();
				_Value.SetNeedsLayout(); // immediately reflect
			}
			else if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath ) { _Value.BecomeFirstResponder(); }

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			_Value.Alpha = isEnabled
								   ? 1.0f
								   : 0.3f;

			base.SetEnabledAppearance(isEnabled);
		}


		protected void UpdateValueText()
		{
			//Without this judging, TextField don't correctly work when inputting Japanese (maybe other 2byte languages either).
			if ( _Value.Text != _EntryCell.ValueText ) { _Value.Text = _EntryCell.ValueText; }
		}
		protected void UpdateValueTextFont()
		{
			string family = _EntryCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _EntryCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			if ( _EntryCell.ValueTextFontSize > 0 ) { _Value.Font = FontUtility.CreateNativeFont(family, (float) _EntryCell.ValueTextFontSize, attr); }
			else if ( CellParent != null ) { _Value.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellValueTextFontSize, attr); }

			//make the view height fit font size
			nfloat contentH = _Value.IntrinsicContentSize.Height;
			CGRect bounds = _Value.Bounds;
			_Value.Bounds = new CGRect(0, 0, bounds.Width, contentH);

			_FieldWrapper.Bounds = new CGRect(0, 0, _FieldWrapper.Bounds.Width, contentH);
		}
		protected void UpdateValueTextColor()
		{
			if ( _EntryCell.ValueTextColor != Color.Default ) { _Value.TextColor = _EntryCell.ValueTextColor.ToUIColor(); }
			else if ( CellParent != null &&
					  CellParent.CellValueTextColor != Color.Default ) { _Value.TextColor = CellParent.CellValueTextColor.ToUIColor(); }

			_Value.SetNeedsLayout();
		}
		protected void UpdateKeyboard() { _Value.ApplyKeyboard(_EntryCell.Keyboard); }
		protected void UpdatePlaceholder()
		{
			if ( !_EntryCell.PlaceholderColor.IsDefault )
			{
				_Value.Placeholder = null;
				_Value.AttributedPlaceholder = new NSAttributedString(_EntryCell.Placeholder, _Value.Font, _EntryCell.PlaceholderColor.ToUIColor());
			}
			else
			{
				_Value.AttributedPlaceholder = null;
				_Value.Placeholder = _EntryCell.Placeholder;
			}
		}
		protected void UpdateTextAlignment()
		{
			_Value.TextAlignment = _EntryCell.ValueTextConfig.TextAlignment.ToUITextAlignment();
			_Value.SetNeedsLayout();
		}
		protected void UpdateIsPassword() { _Value.SecureTextEntry = _EntryCell.IsPassword; }


		protected void ValueFieldOnTouchUpInside( object sender, EventArgs e ) { PerformSelectAction(); }
		protected void TextField_EditingChanged( object sender, EventArgs e ) { _EntryCell.ValueText = _Value.Text; }
		protected void ValueField_EditingDidBegin( object sender, EventArgs e ) { _HasFocus = true; }
		protected void ValueField_EditingDidEnd( object sender, EventArgs e )
		{
			if ( !_HasFocus ) { return; }

			_Value.ResignFirstResponder();
			_EntryCell.SendCompleted();
			_HasFocus = false;
		}
		protected void EntryCell_Focused( object sender, EventArgs e ) { _Value.BecomeFirstResponder(); }
		protected bool OnShouldReturn( UITextField view )
		{
			_HasFocus = false;
			_Value.ResignFirstResponder();
			_EntryCell.SendCompleted();

			return true;
		}
		protected void PerformSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift

			UITextPosition start = _Value.BeginningOfDocument;
			UITextPosition end = _Value.EndOfDocument;
			switch ( _EntryCell.OnSelectAction )
			{
				case AiEntryCell.SelectAction.None:
					break;

				case AiEntryCell.SelectAction.Start:
					_Value.SelectedTextRange = _Value.GetTextRange(start, start);
					_Value.Select(_Value);

					break;

				case AiEntryCell.SelectAction.End:
					_Value.SelectedTextRange = _Value.GetTextRange(end, end);
					_Value.Select(_Value);

					break;

				case AiEntryCell.SelectAction.All:
					_Value.SelectedTextRange = _Value.GetTextRange(start, end);
					_Value.Select(_Value);
					//ValueField.SelectAll(ValueField);

					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		protected void UpdateSelectAction()
		{
			// https://stackoverflow.com/questions/34922331/getting-and-setting-cursor-position-of-uitextfield-and-uitextview-in-swift

			_Value.ClearsOnBeginEditing = _EntryCell.OnSelectAction switch
											  {
												  AiEntryCell.SelectAction.None => false,
												  AiEntryCell.SelectAction.Start => false,
												  AiEntryCell.SelectAction.End => false,
												  AiEntryCell.SelectAction.All => true,
												  _ => throw new ArgumentOutOfRangeException()
											  };
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
				_Value = null;

				_ContentStack.RemoveArrangedSubview(_FieldWrapper);
				_FieldWrapper.Dispose();
				_FieldWrapper = null;
			}

			base.Dispose(disposing);
		}
	}
}