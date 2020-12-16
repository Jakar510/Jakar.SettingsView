using System;
using System.Collections;
using System.Collections.Generic;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class PickerAdapter : BaseAdapter<object>, AdapterView.IOnItemClickListener
	{
		public Action? CloseAction { get; set; }

		protected Android.Content.Context _Context { get; set; }
		protected Shared.SettingsView? _Parent { get; set; }
		protected PickerCell _PickerCell { get; set; }
		protected ListView _ListView { get; set; }
		protected IList _Source { get; set; }
		protected bool _UnLimited => _PickerCell.MaxSelectedNumber == 0;

		internal Color AccentColor { get; set; }
		internal Color TitleColor { get; set; }
		internal Color Background { get; set; }
		internal Color DetailColor { get; set; }
		internal double FontSize { get; set; }
		internal double DetailFontSize { get; set; }

		internal PickerAdapter( Android.Content.Context context, PickerCell pickerCell, ListView listView )
		{
			_Context = context;
			_ListView = listView;
			_PickerCell = pickerCell;
			_Parent = pickerCell.Parent as Shared.SettingsView;
			_Source = pickerCell.ItemsSource as IList;

			pickerCell.SelectedItems ??= new List<object>();

			if ( _Parent != null )
			{
				_ListView.SetBackgroundColor(_Parent.BackgroundColor.ToAndroid());
				_ListView.Divider = new ColorDrawable(_Parent.SeparatorColor.ToAndroid());
				_ListView.DividerHeight = 1;
			}

			SetUpProperties();
		}

		protected void SetUpProperties()
		{
			if ( _PickerCell.AccentColor != Xamarin.Forms.Color.Default ) { AccentColor = _PickerCell.AccentColor.ToAndroid(); }
			else if ( _Parent != null &&
					  _Parent.CellAccentColor != Xamarin.Forms.Color.Default ) { AccentColor = _Parent.CellAccentColor.ToAndroid(); }

			if ( _PickerCell.TitleColor != Xamarin.Forms.Color.Default ) { TitleColor = _PickerCell.TitleColor.ToAndroid(); }
			else if ( _Parent != null &&
					  _Parent.CellTitleColor != Xamarin.Forms.Color.Default ) { TitleColor = _Parent.CellTitleColor.ToAndroid(); }
			else { TitleColor = Color.Black; }

			if ( _PickerCell.TitleFontSize > 0 ) { FontSize = _PickerCell.TitleFontSize; }
			else if ( _Parent != null ) { FontSize = _Parent.CellTitleFontSize; }

			if ( _PickerCell.DescriptionColor != Xamarin.Forms.Color.Default ) { DetailColor = _PickerCell.DescriptionColor.ToAndroid(); }
			else if ( _Parent != null &&
					  _Parent.CellDescriptionColor != Xamarin.Forms.Color.Default ) { DetailColor = _Parent.CellDescriptionColor.ToAndroid(); }

			if ( _PickerCell.DescriptionFontSize > 0 ) { DetailFontSize = _PickerCell.DescriptionFontSize; }
			else if ( _Parent != null ) { DetailFontSize = _Parent.CellDescriptionFontSize; }

			if ( _PickerCell.BackgroundColor != Xamarin.Forms.Color.Default ) { Background = _PickerCell.BackgroundColor.ToAndroid(); }
			else if ( _Parent != null &&
					  _Parent.CellBackgroundColor != Xamarin.Forms.Color.Default ) { Background = _Parent.CellBackgroundColor.ToAndroid(); }
		}

		public void OnItemClick( AdapterView? parent,
								 AView? view,
								 int position,
								 long id )
		{
			if ( _ListView.ChoiceMode == ChoiceMode.Single || _UnLimited )
			{
				DoPickToClose();
				return;
			}

			if ( _ListView.CheckedItemCount > _PickerCell.MaxSelectedNumber )
			{
				_ListView.SetItemChecked(position, false);
				return;
			}

			DoPickToClose();
		}

		protected void DoPickToClose()
		{
			if ( _PickerCell.UsePickToClose &&
				 _ListView.CheckedItemCount == _PickerCell.MaxSelectedNumber ) { CloseAction?.Invoke(); }
		}
		internal void DoneSelect()
		{
			_PickerCell.SelectedItems.Clear();

			SparseBooleanArray? positions = _ListView.CheckedItemPositions;

			if ( positions != null )
			{
				for ( var i = 0; i < positions.Size(); i++ )
				{
					if ( !positions.ValueAt(i) ) continue;

					int index = positions.KeyAt(i);
					_PickerCell.SelectedItems.Add(_Source[index]);
				}
			}

			_PickerCell.SelectedItem = _PickerCell.SelectedItems.Count > 0 ? _PickerCell.SelectedItems[0] : null;
		}
		internal void RestoreSelect()
		{
			IList selectedList = _PickerCell.MergedSelectedList;

			if ( selectedList.Count == 0 ) { return; }

			for ( var i = 0; i < selectedList.Count; i++ )
			{
				if ( _PickerCell.MaxSelectedNumber >= 1 &&
					 i >= _PickerCell.MaxSelectedNumber ) { break; }

				object item = selectedList[i];
				int pos = _Source.IndexOf(item);
				if ( pos < 0 ) { continue; }

				_ListView.SetItemChecked(pos, true);
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

			if ( !( convertView is PickerInnerView view ) ) { return convertView; }

			view.UpdateCell(_PickerCell.DisplayValue(_Source[position]), _PickerCell.SubDisplayValue(_Source[position]));

			return convertView;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Parent = null;
				CloseAction = null;
			}

			base.Dispose(disposing);
		}
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	internal class PickerInnerView : RelativeLayout, ICheckable
	{
		protected TextView _TextLabel { get; set; }
		protected TextView _DetailLabel { get; set; }
		protected SimpleCheck _CheckBox { get; set; }
		protected LinearLayout _TextContainer { get; set; }

		internal PickerInnerView( Android.Content.Context context, PickerAdapter adapter ) : base(context)
		{
			LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			var padding = (int) context.ToPixels(8);
			SetPadding(padding, padding, padding, padding);

			SetBackgroundColor(adapter.Background);

			_TextLabel = new TextView(context)
						 {
							 Id = GenerateViewId()
						 };

			_DetailLabel = new TextView(context)
						   {
							   Id = GenerateViewId()
						   };

			_TextContainer = new LinearLayout(context)
							 {
								 Orientation = Orientation.Vertical
							 };

			using ( var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) )
			{
				_TextContainer.AddView(_TextLabel, param);
				_TextContainer.AddView(_DetailLabel, param);
			}

			_CheckBox = new SimpleCheck(context)
						{
							Focusable = false,
							Color = adapter.AccentColor
						};

			_TextLabel.SetTextColor(adapter.TitleColor);
			_TextLabel.SetTextSize(ComplexUnitType.Sp, (float) adapter.FontSize);
			_DetailLabel.SetTextColor(adapter.DetailColor);
			_DetailLabel.SetTextSize(ComplexUnitType.Sp, (float) adapter.DetailFontSize);
			SetBackgroundColor(adapter.Background);

			using ( var param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) )
			{
				param.AddRule(LayoutRules.AlignParentStart);
				param.AddRule(LayoutRules.CenterVertical);
				AddView(_TextContainer, param);
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
			get => _CheckBox.Selected;
			set => _CheckBox.Selected = value;
		}

		public void Toggle() { Checked = !Checked; }

		public void UpdateCell( object displayValue, object subDisplayValue )
		{
			_TextLabel.Text = $"{displayValue}";
			_DetailLabel.Text = $"{subDisplayValue}";

			_DetailLabel.Visibility = string.IsNullOrEmpty(_DetailLabel.Text) ? ViewStates.Gone : ViewStates.Visible;
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_TextLabel.Dispose();
				_DetailLabel.Dispose();
				_CheckBox.Dispose();
				_TextContainer.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}