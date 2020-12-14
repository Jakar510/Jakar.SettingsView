﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SettingsViewRecyclerAdapter : RecyclerView.Adapter, AView.IOnClickListener, AView.IOnLongClickListener
	{
		private float _MinRowHeight => _Context.ToPixels(44);

		//Item click. correspond to AdapterView.IOnItemClickListener
		private int _SelectedIndex { get; set; } = -1;
		private AView _PreSelectedCell { get; set; }

		private Context _Context { get; }
		private Shared.SettingsView _SettingsView { get; set; }
		private RecyclerView _RecyclerView { get; }
		private ModelProxy _Proxy { get; set; }

		private List<CustomViewHolder> _viewHolders = new List<CustomViewHolder>();


		public SettingsViewRecyclerAdapter( Context context, Shared.SettingsView settingsView, RecyclerView recyclerView )
		{
			_Context = context;
			_SettingsView = settingsView;
			_RecyclerView = recyclerView;
			_Proxy = new ModelProxy(settingsView, this, recyclerView);

			_SettingsView.ModelChanged += SettingsView_ModelChanged;
			_SettingsView.SectionPropertyChanged += OnSectionPropertyChanged;
		}

		private void SettingsView_ModelChanged( object sender, EventArgs e )
		{
			if ( _RecyclerView != null )
			{
				_Proxy.FillProxy();
				NotifyDataSetChanged();
			}
		}

		private void OnSectionPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Section.IsVisibleProperty.PropertyName ) { UpdateSectionVisible((Section) sender); }
			else if ( e.PropertyName == TableSectionBase.TitleProperty.PropertyName || e.PropertyName == Section.HeaderViewProperty.PropertyName || e.PropertyName == Section.HeaderHeightProperty.PropertyName ) { UpdateSectionHeader((Section) sender); }
			else if ( e.PropertyName == Section.FooterTextProperty.PropertyName || e.PropertyName == Section.FooterViewProperty.PropertyName || e.PropertyName == Section.FooterVisibleProperty.PropertyName ) { UpdateSectionFooter((Section) sender); }
		}

		private void UpdateSectionVisible( Section section )
		{
			List<int> indexes = _Proxy.Select(( x, idx ) => new
															{
																idx,
																x.Section
															})
									  .Where(x => x.Section == section)
									  .Select(x => x.idx)
									  .ToList();
			NotifyItemRangeChanged(indexes[0], indexes.Count);
		}
		private void UpdateSectionHeader( Section section )
		{
			int index = _Proxy.FindIndex(x => x.Section == section);
			NotifyItemChanged(index);
		}
		private void UpdateSectionFooter( Section section )
		{
			int index = _Proxy.FindLastIndex(x => x.Section == section);
			NotifyItemChanged(index);
		}


		public override int ItemCount => _Proxy.Count;


		public override long GetItemId( int position ) => position;

		public override int GetItemViewType( int position ) => (int) _Proxy[position].ViewType;

		public override RecyclerView.ViewHolder OnCreateViewHolder( ViewGroup parent, int viewType )
		{
			using LayoutInflater inflater = LayoutInflater.FromContext(_Context);
			if ( inflater is null ) return null;

			CustomViewHolder viewHolder = ( (ViewType) viewType ) switch
										  {
											  ViewType.TextHeader => new HeaderViewHolder(inflater.Inflate(Resource.Layout.DefaultHeaderCell, parent, false)),

											  ViewType.TextFooter => new FooterViewHolder(inflater.Inflate(Resource.Layout.DefaultFooterCell, parent, false)),

											  ViewType.CustomHeader => new CustomHeaderViewHolder(new HeaderFooterContainer(_Context)),

											  ViewType.CustomFooter => new CustomFooterViewHolder(new HeaderFooterContainer(_Context)),

											  _ => CreateHolder(inflater, parent)
										  };

			_viewHolders.Add(viewHolder);

			return viewHolder;
		}
		private ContentBodyViewHolder CreateHolder( LayoutInflater inflater, ViewGroup parent )
		{
			var viewHolder = new ContentBodyViewHolder(inflater.Inflate(Resource.Layout.ContentCell, parent, false));
			viewHolder.ItemView.SetOnClickListener(this);
			viewHolder.ItemView.SetOnLongClickListener(this);
			return viewHolder;
		}

		public override void OnBindViewHolder( RecyclerView.ViewHolder holder, int position )
		{
			RowInfo rowInfo = _Proxy[position];

			var vHolder = holder as CustomViewHolder;
			vHolder.RowInfo = rowInfo;

			if ( !rowInfo.Section.IsVisible || ( rowInfo.ViewType == ViewType.CustomFooter && !rowInfo.Section.FooterVisible ) )
			{
				vHolder.ItemView.Visibility = ViewStates.Gone;
				vHolder.ItemView.SetMinimumHeight(0);
				vHolder.ItemView.LayoutParameters.Height = 0;
				return;
			}

			vHolder.ItemView.Visibility = ViewStates.Visible;

			switch ( rowInfo.ViewType )
			{
				case ViewType.TextHeader:
					BindHeaderView((HeaderViewHolder) vHolder);
					break;
				case ViewType.TextFooter:
					BindFooterView((FooterViewHolder) vHolder);
					break;
				case ViewType.CustomHeader:
					BindCustomHeaderFooterView(vHolder, rowInfo.Section.HeaderView);
					break;
				case ViewType.CustomFooter:
					BindCustomHeaderFooterView(vHolder, rowInfo.Section.FooterView);
					break;
				default:
					BindContentView((ContentBodyViewHolder) vHolder, position);
					break;
			}
		}


		public void OnClick( AView view )
		{
			int position = _RecyclerView.GetChildAdapterPosition(view);

			//TODO: It is desirable that the forms side has Selected property and reflects it.
			//      But do it at a later as iOS side doesn't have that process.
			DeselectRow();

			if ( !( view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody)?.GetChildAt(0) is CellBaseView cell ) ) return;
			if ( !_Proxy[position].Cell.IsEnabled )
			{
				//if FormsCell IsEnable is false, does nothing. 
				return;
			}

			_SettingsView.Model.RowSelected(_Proxy[position].Cell);

			cell.RowSelected(this, position);
		}

		public virtual bool OnLongClick( AView view )
		{
			int position = _RecyclerView.GetChildAdapterPosition(view);

			DeselectRow();

			if ( _Proxy[position].Section.UseDragSort ) { return false; }

			if ( !( view.FindViewById<LinearLayout>(Resource.Id.ContentCellBody)?.GetChildAt(0) is CellBaseView cell ) ) return true;
			if ( !_Proxy[position].Cell.IsEnabled )
			{
				//if FormsCell IsEnable is false, does nothing. 
				return false;
			}

			_SettingsView.Model.RowLongPressed(_Proxy[position].Cell);

			cell.RowLongPressed(this, position);


			return true;
		}


		public void DeselectRow()
		{
			if ( _PreSelectedCell != null )
			{
				_PreSelectedCell.Selected = false;
				_PreSelectedCell = null;
			}

			_SelectedIndex = -1;
		}

		public void SelectedRow( AView cell, int position )
		{
			_PreSelectedCell = cell;
			_SelectedIndex = position;
			cell.Selected = true;
		}


		private void BindHeaderView( HeaderViewHolder holder )
		{
			if ( holder is null ) throw new NullReferenceException(nameof(holder));
			if ( holder.RowInfo is null ) throw new NullReferenceException(nameof(holder.RowInfo));
			if ( holder.ItemView is null ) throw new NullReferenceException(nameof(holder.ItemView));
			
			Section section = holder.RowInfo.Section;
			AView view = holder.ItemView;

			//judging cell height
			int cellHeight =(int) _MinRowHeight;
			double individualHeight = section.HeaderHeight;

			if ( individualHeight > 0d ) { cellHeight = (int) _Context.ToPixels(individualHeight); }
			else if ( _SettingsView.HeaderHeight > -1 ) { cellHeight = (int) _Context.ToPixels(_SettingsView.HeaderHeight); }
			else
			{
				cellHeight = -1; // Height Auto
			}

			if ( cellHeight >= 0 )
			{
				view.SetMinimumHeight(cellHeight);
				if ( view.LayoutParameters != null ) view.LayoutParameters.Height = cellHeight;
			}

			//textview setting
			holder.TextView.SetPadding((int) view.Context.ToPixels(_SettingsView.HeaderPadding.Left), (int) view.Context.ToPixels(_SettingsView.HeaderPadding.Top), (int) view.Context.ToPixels(_SettingsView.HeaderPadding.Right), (int) view.Context.ToPixels(_SettingsView.HeaderPadding.Bottom));

			holder.TextView.Gravity = _SettingsView.HeaderTextVerticalAlign.ToNativeVertical() | GravityFlags.Left;
			holder.TextView.TextAlignment = Android.Views.TextAlignment.Gravity;
			holder.TextView.Typeface = FontUtility.CreateTypeface(_SettingsView.HeaderFontFamily, _SettingsView.HeaderFontAttributes);
			holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _SettingsView.HeaderFontSize);
			holder.TextView.SetBackgroundColor(_SettingsView.HeaderBackgroundColor.ToAndroid());

			// Single line was done away with.
			//holder.TextView.SetMaxLines(1);
			//holder.TextView.SetMinLines(1);
			//holder.TextView.Ellipsize = TextUtils.TruncateAt.End;

			if ( _SettingsView.HeaderTextColor != Color.Default ) { holder.TextView.SetTextColor(_SettingsView.HeaderTextColor.ToAndroid()); }

			//update text
			holder.TextView.Text = section.Title;
		}
		private void BindFooterView( FooterViewHolder holder )
		{
			Section section = holder.RowInfo.Section;
			AView view = holder.ItemView;

			//footer visible setting
			if ( string.IsNullOrEmpty(section.FooterText) )
			{
				//if text is empty, hidden (height 0)
				holder.TextView.Visibility = ViewStates.Gone;
				view.Visibility = ViewStates.Gone;
			}
			else
			{
				holder.TextView.Visibility = ViewStates.Visible;
				view.Visibility = ViewStates.Visible;
			}

			//textview setting
			holder.TextView.SetPadding((int) view.Context.ToPixels(_SettingsView.FooterPadding.Left), (int) view.Context.ToPixels(_SettingsView.FooterPadding.Top), (int) view.Context.ToPixels(_SettingsView.FooterPadding.Right), (int) view.Context.ToPixels(_SettingsView.FooterPadding.Bottom));

			holder.TextView.Typeface = FontUtility.CreateTypeface(_SettingsView.FooterFontFamily, _SettingsView.FooterFontAttributes);
			holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _SettingsView.FooterFontSize);
			holder.TextView.SetBackgroundColor(_SettingsView.FooterBackgroundColor.ToAndroid());
			if ( _SettingsView.FooterTextColor != Color.Default ) { holder.TextView.SetTextColor(_SettingsView.FooterTextColor.ToAndroid()); }

			//update text
			holder.TextView.Text = section.FooterText;
		}

		private void BindCustomHeaderFooterView( CustomViewHolder holder, Xamarin.Forms.View formsView )
		{
			if ( holder is null ) throw new NullReferenceException(nameof(holder));

			if ( !( holder.ItemView is HeaderFooterContainer nativeCell ) ) return;
			nativeCell.ViewHolder = holder;
			nativeCell.FormsCell = formsView;
		}


		private void BindContentView( ContentBodyViewHolder holder, int position )
		{
			if ( holder is null ) throw new NullReferenceException(nameof(holder));
			if ( holder.RowInfo is null ) throw new NullReferenceException(nameof(holder.RowInfo));
			if ( holder.ItemView is null ) throw new NullReferenceException(nameof(holder.ItemView));

			Cell formsCell = holder.RowInfo.Cell;
			AView layout = holder.ItemView;

			holder.RowInfo = _Proxy[position];

			AView nativeCell = holder.Body.GetChildAt(0);
			if ( nativeCell != null ) { holder.Body.RemoveViewAt(0); }

			nativeCell = CellFactory.GetCell(formsCell, nativeCell, _RecyclerView, _Context, _SettingsView);

			if ( position == _SelectedIndex )
			{
				DeselectRow();
				nativeCell.Selected = true;

				_PreSelectedCell = nativeCell;
			}

			var minHeight = (int) Math.Max(_Context.ToPixels(_SettingsView.RowHeight), _MinRowHeight);

			//it is necessary to set both
			layout.SetMinimumHeight(minHeight);
			nativeCell.SetMinimumHeight(minHeight);

			if ( layout.LayoutParameters is null ) return;
			if ( !_SettingsView.HasUnevenRows )
			{
				// if not Uneven, set the larger one of RowHeight and MinRowHeight.
				layout.LayoutParameters.Height = minHeight;
			}
			else if ( formsCell.Height > -1 )
			{
				// if the cell itself was specified height, set it.
				layout.SetMinimumHeight((int) _Context.ToPixels(formsCell.Height));
				// ReSharper disable once PossibleNullReferenceException
				layout.LayoutParameters.Height = (int) _Context.ToPixels(formsCell.Height);
			}
			else if ( formsCell is ViewCell viewCell )
			{
				// if used a view cell, calculate the size and layout it.
				SizeRequest size = viewCell.View.Measure(_SettingsView.Width, double.PositiveInfinity);
				viewCell.View.Layout(new Rectangle(0, 0, size.Request.Width, size.Request.Height));
				layout.LayoutParameters.Height = (int) _Context.ToPixels(size.Request.Height);
			}
			else
			{
				layout.LayoutParameters.Height = -2; //wrap_content
			}

			holder.Body.AddView(nativeCell, 0);
		}


		public void CellMoved( int fromPos, int toPos )
		{
			RowInfo tmp = _Proxy[fromPos];
			_Proxy.RemoveAt(fromPos);
			_Proxy.Insert(toPos, tmp);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_SettingsView.ModelChanged -= SettingsView_ModelChanged;
				_SettingsView.SectionPropertyChanged -= OnSectionPropertyChanged;
				_Proxy?.Dispose();
				_Proxy = null;
				_SettingsView = null;

				foreach ( CustomViewHolder holder in _viewHolders ) { holder.Dispose(); }

				_viewHolders.Clear();
				_viewHolders = null;
			}

			base.Dispose(disposing);
		}
	}
}