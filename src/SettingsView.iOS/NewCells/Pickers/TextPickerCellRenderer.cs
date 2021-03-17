using System;
using System.Collections.Generic;
using System.Windows.Input;
using CoreGraphics;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.NewCells;
using Jakar.SettingsView.iOS.NewCells.Sources;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TextPickerCell), typeof(TextPickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.NewCells
{
	[Foundation.Preserve(AllMembers = true)]
	public class TextPickerCellRenderer : CellBaseRenderer<TextPickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class TextPickerCellView : BasePickerCell
	{
		private TextPickerCell _TextPickerCell => Cell as TextPickerCell ?? throw new NullReferenceException(nameof(_TextPickerCell));
		protected TextPickerSource _Model { get; set; }
		protected UILabel _PopupTitle { get; set; }
		protected UIPickerView? _Picker { get; set; }
		protected ICommand? _Command { get; set; }


		public TextPickerCellView( Cell cell ) : base(cell)
		{
			_Model = new TextPickerSource();
			_PopupTitle = new UILabel
						  {
							  TextAlignment = UITextAlignment.Center
						  };
		}

		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(TextPickerCell.SelectedItemProperty) ) { UpdateSelectedItem(); }
			else if ( e.IsEqual(TextPickerCell.ItemsProperty) ) { UpdateItems(); }
			else if ( e.IsEqual(TextPickerCell.SelectedCommandProperty) ) { UpdateCommand(); }
			else if ( e.IsEqual(PopupConfig.TitleProperty) ) { UpdatePopupTitle(); }
			else { base.CellPropertyChanged(sender, e); }
		}


		protected override void SetUp()
		{
			_Model.UpdatePickerFromModel -= Model_UpdatePickerFromModel;
			_Picker?.Dispose();
			_Picker = null;

			_Picker = new UIPickerView();

			nfloat width = UIScreen.MainScreen.Bounds.Width;


			var toolbar = new UIToolbar(new CGRect(0, 0, (float) width, 44))
						  {
							  BarStyle = UIBarStyle.Default,
							  Translucent = true
						  };
			var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel,
												   ( o, e ) =>
												   {
													   _DummyField.ResignFirstResponder();
													   Select(_Model.PreSelectedItem);
												   }
												  );

			var labelButton = new UIBarButtonItem(_PopupTitle);
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
												 ( o, a ) =>
												 {
													 _Model.OnUpdatePickerFormModel();
													 _DummyField.ResignFirstResponder();
													 _Command?.Execute(_Model.SelectedItem);
												 }
												);

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

			_DummyField.InputView = _Picker;
			_DummyField.InputAccessoryView = toolbar;

			_Picker.Model = _Model;

			_Model.UpdatePickerFromModel += Model_UpdatePickerFromModel;
		}

		protected void UpdateSelectedItem()
		{
			Select(_TextPickerCell.SelectedItem);
			_Value.Text = _TextPickerCell.SelectedItem;
		}

		protected void UpdateItems()
		{
			IList<string?> items = _TextPickerCell.Items ?? new List<string?>();
			_Model.SetItems(items);
			// Force picker view to reload data from model after change
			// Otherwise it might access the model based on old view data
			// causing "Index was out of range" errors and the like.
			_Picker?.ReloadAllComponents();
			Select(_TextPickerCell.SelectedItem);
		}

		protected void UpdatePopupTitle()
		{
			_PopupTitle.Text = _TextPickerCell.Title;
			_PopupTitle.SizeToFit();
			_PopupTitle.Frame = new CGRect(0, 0, 160, 44);
		}

		protected void UpdateCommand() { _Command = _TextPickerCell.SelectedCommand; }

		protected void Model_UpdatePickerFromModel( object sender, EventArgs e )
		{
			_TextPickerCell.SelectedItem = _Model.SelectedItem;
			_Value.Text = _Model.SelectedItem?.ToString();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			_DummyField.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdatePopupTitle();
			UpdateSelectedItem();
			UpdateCommand();
			UpdateItems();
		}

		protected void Select( string? item )
		{
			int idx = _Model.Items.IndexOf(item);
			if ( idx == -1 )
			{
				item = _Model.Items.Count == 0
						   ? null
						   : _Model.Items[0];
				idx = 0;
			}

			_Picker?.Select(idx, 0, false);
			_Model.SelectedItem = item;
			_Model.SelectedIndex = idx;
			_Model.PreSelectedItem = item;
		}
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Picker?.Dispose();
				_Picker = null;
				_Model.Dispose();
				_PopupTitle.Dispose();
				_Command = null;
			}

			base.Dispose(disposing);
		}
	}
}