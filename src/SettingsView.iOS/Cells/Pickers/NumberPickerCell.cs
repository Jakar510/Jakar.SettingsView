using System;
using System.Runtime.Remoting.Contexts;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

[assembly: ExportRenderer(typeof(NumberPickerCell), typeof(NumberPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class NumberPickerCellRenderer : CellBaseRenderer<NumberPickerCellView> { }


	[Preserve(AllMembers = true)]
	public class NumberPickerCellView : BasePickerCell
	{
		protected NumberPickerCell _NumberPickerCell => Cell as NumberPickerCell ?? throw new NullReferenceException(nameof(_NumberPickerCell));
		protected UILabel _PopupTitle { get; set; }
		protected NumberPickerSource _Model { get; set; }
		protected UIPickerView? _Dialog { get; set; }
		protected ICommand? _Command { get; set; }


		public NumberPickerCellView( Cell cell ) : base(cell)
		{
			_Model = new NumberPickerSource();
			_PopupTitle = new UILabel
						  {
							  TextAlignment = UITextAlignment.Center
						  };
		}


		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(NumberPickerCell.MinProperty, NumberPickerCell.MaxProperty) ) { UpdateNumberList(); }
			else if ( e.PropertyName == NumberPickerCell.NumberProperty.PropertyName ) { UpdateNumber(); }
			else if ( e.PropertyName == PopupConfig.TitleProperty.PropertyName ) { UpdatePopupTitle(); }
			else if ( e.PropertyName == NumberPickerCell.SelectedCommandProperty.PropertyName ) { UpdateCommand(); }
			else { base.CellPropertyChanged(sender, e); }
		}

		protected override void SetUp()
		{
			_Dialog = new UIPickerView();

			nfloat width = UIScreen.MainScreen.Bounds.Width;


			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelHandler);

			var labelButton = new UIBarButtonItem(_PopupTitle);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneHandler);

			toolbar.SetItems(new[]
							 {
								 cancelButton,
								 spacer,
								 labelButton,
								 spacer,
								 doneButton
							 },
							 false
							);

			_DummyField.InputView = _Dialog;
			_DummyField.InputAccessoryView = toolbar;

			_Dialog.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}
		private void CancelHandler( object o, EventArgs e )
		{
			_DummyField.ResignFirstResponder();
			Select(_Model.PreSelectedItem);
		}
		private void DoneHandler( object o, EventArgs a )
		{
			_Model.OnUpdatePickerFormModel();
			_DummyField.ResignFirstResponder();
			_Command?.Execute(_Model.SelectedItem);
			_NumberPickerCell.ValueChangedHandler.SendValueChanged(_Model.SelectedItem);
		}


		protected void UpdateNumber()
		{
			Select(_NumberPickerCell.Number);
			_Value.Text = _NumberPickerCell.Number.ToString();
		}
		protected void UpdateNumberList()
		{
			_Model.SetNumbers(_NumberPickerCell.Min, _NumberPickerCell.Max);
			Select(_NumberPickerCell.Number);
		}
		protected void UpdatePopupTitle()
		{
			_PopupTitle.Text = _NumberPickerCell.Title;
			_PopupTitle.SizeToFit();
			_PopupTitle.Frame = new CGRect(0, 0, 160, 44);
		}

		protected void UpdateCommand() { _Command = _NumberPickerCell.SelectedCommand; }
		protected void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_NumberPickerCell.Number = _Model.SelectedItem;
			_Value.Text = _Model.SelectedItem.ToString();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			_DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		protected void Select( int number )
		{
			int idx = _Model.Items.IndexOf(number);
			if ( idx == -1 )
			{
				number = _Model.Items[0];
				idx = 0;
			}

			_Dialog?.Select(idx, 0, false);
			_Model.SelectedItem = number;
			_Model.SelectedIndex = idx;
			_Model.PreSelectedItem = number;
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateNumberList();
			UpdatePopupTitle();
			UpdateNumber();
			UpdateCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
				_PopupTitle.Dispose();
				_Model.Dispose();
				_Dialog?.Dispose();
				_Command = null;
			}

			base.Dispose(disposing);
		}
	}
}