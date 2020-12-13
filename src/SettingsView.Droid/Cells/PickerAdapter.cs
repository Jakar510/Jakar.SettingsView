﻿using System;
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

namespace Jakar.SettingsView.Droid.Cells
{
	[Android.Runtime.Preserve(AllMembers = true)]
	internal class PickerAdapter : BaseAdapter<object>, AdapterView.IOnItemClickListener
	{
		public Action CloseAction { get; set; }

		private Android.Content.Context _context;
		private Shared.SettingsView _parent;
		private PickerCell _pickerCell;
		private ListView _listview;
		private IList _source;
		private bool _unLimited => _pickerCell.MaxSelectedNumber == 0;

		internal Color _accentColor;
		internal Color _titleColor;
		internal Color _background;
		internal Color _detailColor;
		internal double _fontSize;
		internal double _detailFontSize;

		internal PickerAdapter( Android.Content.Context context, PickerCell pickerCell, ListView listview )
		{
			_context = context;
			_listview = listview;
			_pickerCell = pickerCell;
			_parent = pickerCell.Parent as Shared.SettingsView;
			_source = pickerCell.ItemsSource as IList;

			if ( pickerCell.SelectedItems == null ) { pickerCell.SelectedItems = new List<object>(); }

			if ( _parent != null )
			{
				_listview.SetBackgroundColor(_parent.BackgroundColor.ToAndroid());
				_listview.Divider = new ColorDrawable(_parent.SeparatorColor.ToAndroid());
				_listview.DividerHeight = 1;
			}

			SetUpProperties();
		}

		private void SetUpProperties()
		{
			if ( _pickerCell.AccentColor != Xamarin.Forms.Color.Default ) { _accentColor = _pickerCell.AccentColor.ToAndroid(); }
			else if ( _parent.CellAccentColor != Xamarin.Forms.Color.Default ) { _accentColor = _parent.CellAccentColor.ToAndroid(); }

			if ( _pickerCell.TitleColor != Xamarin.Forms.Color.Default ) { _titleColor = _pickerCell.TitleColor.ToAndroid(); }
			else if ( _parent != null &&
					  _parent.CellTitleColor != Xamarin.Forms.Color.Default ) { _titleColor = _parent.CellTitleColor.ToAndroid(); }
			else { _titleColor = Color.Black; }

			if ( _pickerCell.TitleFontSize > 0 ) { _fontSize = _pickerCell.TitleFontSize; }
			else if ( _parent != null ) { _fontSize = _parent.CellTitleFontSize; }

			if ( _pickerCell.DescriptionColor != Xamarin.Forms.Color.Default ) { _detailColor = _pickerCell.DescriptionColor.ToAndroid(); }
			else if ( _parent != null &&
					  _parent.CellDescriptionColor != Xamarin.Forms.Color.Default ) { _detailColor = _parent.CellDescriptionColor.ToAndroid(); }

			if ( _pickerCell.DescriptionFontSize > 0 ) { _detailFontSize = _pickerCell.DescriptionFontSize; }
			else if ( _parent != null ) { _detailFontSize = _parent.CellDescriptionFontSize; }

			if ( _pickerCell.BackgroundColor != Xamarin.Forms.Color.Default ) { _background = _pickerCell.BackgroundColor.ToAndroid(); }
			else if ( _parent != null &&
					  _parent.CellBackgroundColor != Xamarin.Forms.Color.Default ) { _background = _parent.CellBackgroundColor.ToAndroid(); }
		}

		public void OnItemClick( AdapterView parent,
								 AView view,
								 int position,
								 long id )
		{
			if ( _listview.ChoiceMode == ChoiceMode.Single || _unLimited )
			{
				DoPickToClose();
				return;
			}

			if ( _listview.CheckedItemCount > _pickerCell.MaxSelectedNumber )
			{
				_listview.SetItemChecked(position, false);
				return;
			}

			DoPickToClose();
		}

		private void DoPickToClose()
		{
			if ( _pickerCell.UsePickToClose &&
				 _listview.CheckedItemCount == _pickerCell.MaxSelectedNumber ) { CloseAction?.Invoke(); }
		}

		internal void DoneSelect()
		{
			_pickerCell.SelectedItems.Clear();

			SparseBooleanArray? positions = _listview.CheckedItemPositions;

			for ( var i = 0; i < positions.Size(); i++ )
			{
				if ( !positions.ValueAt(i) ) continue;

				int index = positions.KeyAt(i);
				_pickerCell.SelectedItems.Add(_source[index]);
			}

			_pickerCell.SelectedItem = _pickerCell.SelectedItems.Count > 0 ? _pickerCell.SelectedItems[0] : null;
		}

		internal void RestoreSelect()
		{
			IList selectedList = _pickerCell.MergedSelectedList;

			if ( selectedList.Count == 0 ) { return; }

			for ( var i = 0; i < selectedList.Count; i++ )
			{
				if ( _pickerCell.MaxSelectedNumber >= 1 &&
					 i >= _pickerCell.MaxSelectedNumber ) { break; }

				object item = selectedList[i];
				int pos = _source.IndexOf(item);
				if ( pos < 0 ) { continue; }

				_listview.SetItemChecked(pos, true);
			}

			if ( _listview.CheckedItemPositions.IndexOfKey(0) < 0 ) { return; }

			_listview.SetSelection(_listview.CheckedItemPositions.KeyAt(0));
		}

		/// <summary>
		/// Gets the <see cref="T:Jakar.SettingsView.Droid.Cells.PickerAdapter"/> with the specified position.
		/// </summary>
		/// <param name="position">Position.</param>
		public override object this[ int position ] => _source[position];

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public override int Count => _source.Count;

		/// <summary>
		/// Gets the item identifier.
		/// </summary>
		/// <returns>The item identifier.</returns>
		/// <param name="position">Position.</param>
		public override long GetItemId( int position ) => position;

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="position">Position.</param>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
		public override AView GetView( int position, AView convertView, ViewGroup parent )
		{
			if ( convertView == null ) { convertView = new PickerInnerView(_context, this); }

			( convertView as PickerInnerView ).UpdateCell(_pickerCell.DisplayValue(_source[position]), _pickerCell.SubDisplayValue(_source[position]));

			return convertView;
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_parent = null;
				_pickerCell = null;
				_source = null;
				_listview = null;
				_context = null;
				CloseAction = null;
			}

			base.Dispose(disposing);
		}
	}

	[Android.Runtime.Preserve(AllMembers = true)]
	internal class PickerInnerView : RelativeLayout, ICheckable
	{
		private TextView _textLabel;
		private TextView _detailLabel;
		private SimpleCheck _checkBox;
		private LinearLayout _textContainr;

		internal PickerInnerView( Android.Content.Context context, PickerAdapter adapter ) : base(context)
		{
			LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			var padding = (int) context.ToPixels(8);
			SetPadding(padding, padding, padding, padding);

			SetBackgroundColor(adapter._background);

			_textLabel = new TextView(context);
			_textLabel.Id = GenerateViewId();

			_detailLabel = new TextView(context);
			_detailLabel.Id = GenerateViewId();

			_textContainr = new LinearLayout(context);
			_textContainr.Orientation = Orientation.Vertical;

			using ( var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) )
			{
				_textContainr.AddView(_textLabel, param);
				_textContainr.AddView(_detailLabel, param);
			}

			_checkBox = new SimpleCheck(context);
			_checkBox.Focusable = false;

			_textLabel.SetTextColor(adapter._titleColor);
			_textLabel.SetTextSize(ComplexUnitType.Sp, (float) adapter._fontSize);
			_detailLabel.SetTextColor(adapter._detailColor);
			_detailLabel.SetTextSize(ComplexUnitType.Sp, (float) adapter._detailFontSize);
			_checkBox.Color = adapter._accentColor;
			SetBackgroundColor(adapter._background);

			using ( var param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) )
			{
				param.AddRule(LayoutRules.AlignParentStart);
				param.AddRule(LayoutRules.CenterVertical);
				AddView(_textContainr, param);
			}

			using ( var param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent)
								{
									Width = (int) context.ToPixels(30),
									Height = (int) context.ToPixels(30)
								} )
			{
				param.AddRule(LayoutRules.AlignParentEnd);
				param.AddRule(LayoutRules.CenterVertical);
				AddView(_checkBox, param);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Droid.Cells.PickerInnerView"/> is checked.
		/// </summary>
		/// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
		public bool Checked
		{
			get => _checkBox.Selected;
			set => _checkBox.Selected = value;
		}

		/// <summary>
		/// Toggle this instance.
		/// </summary>
		public void Toggle() { _checkBox.Selected = !_checkBox.Selected; }

		/// <summary>
		/// Updates the cell.
		/// </summary>
		/// <param name="displayValue">Display value.</param>
		/// <param name="subDisplayValue">Sub display value.</param>
		public void UpdateCell( object displayValue, object subDisplayValue )
		{
			_textLabel.Text = $"{displayValue}";
			_detailLabel.Text = $"{subDisplayValue}";

			_detailLabel.Visibility = string.IsNullOrEmpty(_detailLabel.Text) ? ViewStates.Gone : ViewStates.Visible;
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_textLabel?.Dispose();
				_textLabel = null;
				_detailLabel?.Dispose();
				_detailLabel = null;
				_checkBox?.Dispose();
				_checkBox = null;
				_textContainr?.Dispose();
				_textContainr = null;
			}

			base.Dispose(disposing);
		}
	}
}