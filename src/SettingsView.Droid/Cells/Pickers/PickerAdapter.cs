using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.Controls;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;
using ListView = Android.Widget.ListView;
using RelativeLayout = Android.Widget.RelativeLayout;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)]
	public class PickerAdapter : BaseAdapter<object>, AdapterView.IOnItemClickListener
	{
		// public Action? CloseAction { get; set; }

		protected Context _Context { get; set; }
		protected PickerCellView? _Renderer { get; set; }
		protected Shared.sv.SettingsView _Parent { get; set; }
		protected PickerCell _PickerCell { get; set; }

		protected ListView _ListView { get; set; }

		protected IList _Source { get; set; }

		internal AColor AccentColor { get; set; }
		internal AColor BackgroundColor { get; set; }
		internal AColor TitleTextColor { get; set; }
		internal AColor TextColor { get; set; }
		internal AColor DetailColor { get; set; }
		internal float FontSize { get; set; }
		internal float DetailFontSize { get; set; }

		internal PickerAdapter( Context context,
								PickerCellView renderer,
								PickerCell pickerCell,
								ListView listView ) : base()
		{
			_Context = context;
			_Renderer = renderer;
			_ListView = listView;
			_PickerCell = pickerCell;
			_Parent = pickerCell.Parent;
			_Source = pickerCell.ItemsSource ?? new List<object?>();

			pickerCell.SelectedItems ??= new List<object?>();

			_ListView.SetBackgroundColor(pickerCell.Popup.BackgroundColor.ToAndroid());
			_ListView.Divider = new ColorDrawable(_PickerCell.Popup.SeparatorColor.ToAndroid());
			_ListView.DividerHeight = 1;


			if ( pickerCell.Popup.BackgroundColor != Color.Default ) { BackgroundColor = pickerCell.Popup.BackgroundColor.ToAndroid(); }
			// else if ( _Parent.CellPopup.BackgroundColor != Color.Default ) { BackgroundColor = _Parent.CellPopup.BackgroundColor.ToAndroid(); }
			else { BackgroundColor = Color.White.ToAndroid(); }


			if ( pickerCell.Popup.AccentColor != Color.Default ) { AccentColor = pickerCell.Popup.AccentColor.ToAndroid(); }
			// else if ( _Parent.CellPopup.AccentColor != Color.Default ) { AccentColor = _Parent.CellPopup.AccentColor.ToAndroid(); }
			else { BackgroundColor = Color.Accent.ToAndroid(); }


			if ( pickerCell.Popup.TitleColor != Color.Default ) { TitleTextColor = pickerCell.Popup.TitleColor.ToAndroid(); }
			// else if ( _Parent.CellTitleColor != Color.Default ) { TitleTextColor = _Parent.CellTitleColor.ToAndroid(); }
			else { TitleTextColor = Color.Black.ToAndroid(); }


			if ( pickerCell.Popup.ItemColor != Color.Default ) { TextColor = pickerCell.Popup.ItemColor.ToAndroid(); }
			// else if ( _Parent.CellTitleColor != Color.Default ) { TextColor = _Parent.CellTitleColor.ToAndroid(); }
			else { TextColor = Color.Black.ToAndroid(); }


			if ( pickerCell.Popup.TitleFontSize > 0 ) { FontSize = (float) pickerCell.Popup.TitleFontSize; }
			// else if ( _Parent.CellPopup.FontSize > 0 ) { FontSize = _Parent.CellPopup.FontSize; }
			else { FontSize = 12; }


			if ( pickerCell.Popup.ItemDescriptionColor != Color.Default ) { DetailColor = pickerCell.Popup.ItemDescriptionColor.ToAndroid(); }
			// else if ( _Parent.CellPopup.DescriptionTextColor != Color.Default ) { DetailColor = _Parent.CellPopup.DescriptionTextColor.ToAndroid(); }

			if ( pickerCell.Popup.ItemDescriptionFontSize > 0 ) { DetailFontSize = (float) pickerCell.Popup.ItemDescriptionFontSize; }
			// else if ( pickerCell.CellPopup.DescriptionFontSize > 0 ) { DetailFontSize = pickerCell.CellPopup.DescriptionFontSize; }
			else { DetailFontSize = 10; }
		}

		public void OnItemClick( AdapterView? parent,
								 AView? view,
								 int position,
								 long id )
		{
			if ( _ListView.ChoiceMode == ChoiceMode.Single ||
				 _PickerCell.IsSingleMode )
			{
				DoPickToClose();
				return;
			}

			if ( _PickerCell.IsValidMode &&
				 !_PickerCell.IsUnLimited &&
				 _ListView.CheckedItemCount > _PickerCell.MaxSelectedNumber )
			{
				_ListView.SetItemChecked(position, false);
				return;
			}

			DoPickToClose();
		}

		protected void DoPickToClose()
		{
			if ( _PickerCell.UsePickToClose &&
				 _ListView.CheckedItemCount == _PickerCell.MaxSelectedNumber ) { _Renderer?.CloseAction(); }
		}
		internal void DoneSelect()
		{
			_PickerCell.SelectedItems.Clear();
			SparseBooleanArray? positions = _ListView.CheckedItemPositions;

			if ( positions is not null )
			{
				for ( var i = 0; i < positions.Size(); i++ )
				{
					if ( !positions.ValueAt(i) ) continue;

					int index = positions.KeyAt(i);
					_PickerCell.SelectedItems.Add(_Source[index]);
				}
			}

			_PickerCell.SelectedItem = _PickerCell.IsSingleMode && _PickerCell.MergedSelectedList.Count > 0
										   ? _PickerCell.SelectedItems[0]
										   : null;
		}
		internal void RestoreSelect()
		{
			if ( _PickerCell.SelectedItems.Count == 0 ) { return; }

			for ( var i = 0; i < _PickerCell.SelectedItems.Count; i++ )
			{
				if ( _PickerCell.MaxSelectedNumber >= 1 &&
					 i >= _PickerCell.MaxSelectedNumber ) { break; }

				object? item = _PickerCell.SelectedItems[i];
				int pos = _Source?.IndexOf(item) ?? -1;
				switch ( pos )
				{
					case < 0: continue;

					default:
						_ListView.SetItemChecked(pos, true);
						break;
				}
			}

			if ( _ListView.CheckedItemPositions is null ||
				 _ListView.CheckedItemPositions.IndexOfKey(0) < 0 ) { return; }

			_ListView.SetSelection(_ListView.CheckedItemPositions.KeyAt(0));
		}


		public override object this[ int position ] => _Source[position];
		public override int Count => _Source.Count;
		public override long GetItemId( int position ) => position;


		public override AView GetView( int position, AView? convertView, ViewGroup? parent )
		{
			convertView ??= new PickerInnerView(_Context, this);

			if ( convertView is not PickerInnerView view ) return convertView;

			view.UpdateCell(_PickerCell.DisplayValue(_Source[position]), _PickerCell.SubDisplayValue(_Source[position]));
			return view;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Parent = null;
				_Renderer = null;
				// _TitleLabel.Dispose();
			}

			base.Dispose(disposing);
		}
	}

	[Preserve(AllMembers = true)]
	internal class PickerInnerView : RelativeLayout, ICheckable
	{
		protected TextView _Title { get; set; }
		protected TextView _Description { get; set; }
		protected SimpleCheck _CheckBox { get; set; }
		protected LinearLayout _Container { get; set; }

		internal PickerInnerView( Context context, PickerAdapter adapter ) : base(context)
		{
			LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			var padding = (int) context.ToPixels(8);
			SetPadding(padding, padding, padding, padding);

			SetBackgroundColor(adapter.BackgroundColor);


			_Title = new TextView(context)
					 {
						 Id = GenerateViewId()
					 };
			_Title.SetBackgroundColor(adapter.BackgroundColor);
			_Title.SetTextColor(adapter.TextColor);
			_Title.SetTextSize(ComplexUnitType.Sp, (float) adapter.FontSize);


			_Description = new TextView(context)
						   {
							   Id = GenerateViewId()
						   };
			_Description.SetBackgroundColor(adapter.BackgroundColor);
			_Description.SetTextColor(adapter.DetailColor);
			_Description.SetTextSize(ComplexUnitType.Sp, (float) adapter.DetailFontSize);


			_CheckBox = new SimpleCheck(context, adapter.AccentColor)
						{
							Focusable = false,
						};
			_CheckBox.SetBackgroundColor(adapter.BackgroundColor);
			_CheckBox.SetHighlightColor(adapter.AccentColor);


			_Container = new LinearLayout(context)
						 {
							 Orientation = Orientation.Vertical
						 };
			_Container.SetBackgroundColor(adapter.BackgroundColor);


			using ( var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) )
			{
				_Container.AddView(_Title, param);
				_Container.AddView(_Description, param);
			}


			using ( var param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) )
			{
				param.AddRule(LayoutRules.AlignParentStart);
				param.AddRule(LayoutRules.CenterVertical);
				AddView(_Container, param);
			}


			using ( var param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent)
								{
									Width = (int) context.ToPixels(30),
									Height = (int) context.ToPixels(30)
								} )
			{
				param.AddRule(LayoutRules.AlignParentEnd);
				param.AddRule(LayoutRules.CenterVertical);
				AddView(_CheckBox, param);
			}
		}


		public bool Checked
		{
			get => _CheckBox.Checked;
			set => _CheckBox.Checked = value;
		}

		public void Toggle() { Checked = !Checked; }

		public void UpdateCell( object displayValue, object subDisplayValue )
		{
			_Title.Text = $"{displayValue}";

			_Description.Text = $"{subDisplayValue}";
			_Description.Visibility = string.IsNullOrEmpty(_Description.Text)
										  ? ViewStates.Gone
										  : ViewStates.Visible;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Title.Dispose();
				_Description.Dispose();
				_CheckBox.Dispose();
				_Container.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}