using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SettingsViewRecyclerAdapter : RecyclerView.Adapter, AView.IOnClickListener, AView.IOnLongClickListener
	{
		protected float _MinRowHeight => AndroidContext.ToPixels(Shared.sv.SettingsView.MIN_ROW_HEIGHT);

		//Item click. correspond to AdapterView.IOnItemClickListener
		protected int _SelectedIndex { get; set; } = -1;
		protected AView? _PreSelectedCell { get; set; }

		protected internal Context AndroidContext { get; }
		protected Shared.sv.SettingsView _SettingsView { get; set; }
		protected RecyclerView _RecyclerView { get; }
		protected ModelProxy _Proxy { get; set; }

		protected List<CustomViewHolder> _viewHolders = new List<CustomViewHolder>();


		public SettingsViewRecyclerAdapter( Context context, Shared.sv.SettingsView settingsView, RecyclerView recyclerView )
		{
			AndroidContext = context;
			_SettingsView = settingsView;
			_RecyclerView = recyclerView;
			_Proxy = new ModelProxy(settingsView, this, recyclerView);

			_SettingsView.ModelChanged += SettingsView_ModelChanged;
			_SettingsView.SectionPropertyChanged += OnSectionPropertyChanged;
		}

		protected void SettingsView_ModelChanged( object sender, EventArgs e )
		{
			if ( _RecyclerView != null )
			{
				_Proxy.FillProxy();
				NotifyDataSetChanged();
			}
		}

		protected void OnSectionPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Section.IsVisibleProperty.PropertyName ) { UpdateSectionVisible((Section) sender); }
			else if ( e.PropertyName == TableSectionBase.TitleProperty.PropertyName ||
					  e.PropertyName == Section.HeaderViewProperty.PropertyName ||
					  e.PropertyName == Section.HeaderHeightProperty.PropertyName ) { UpdateSectionHeader((Section) sender); }
			else if ( e.PropertyName == Section.FooterTextProperty.PropertyName ||
					  e.PropertyName == Section.FooterViewProperty.PropertyName ||
					  e.PropertyName == Section.FooterVisibleProperty.PropertyName ) { UpdateSectionFooter((Section) sender); }
		}

		protected void UpdateSectionVisible( Section section )
		{
			List<int> indexes = _Proxy.Select(( x, idx ) => new
															{
																idx,
																x.Section
															}
											 )
									  .Where(x => x.Section == section)
									  .Select(x => x.idx)
									  .ToList();
			NotifyItemRangeChanged(indexes[0], indexes.Count);
		}
		protected void UpdateSectionHeader( Section section )
		{
			int index = _Proxy.FindIndex(x => x.Section == section);
			NotifyItemChanged(index);
		}
		protected void UpdateSectionFooter( Section section )
		{
			int index = _Proxy.FindLastIndex(x => x.Section == section);
			NotifyItemChanged(index);
		}


		public override int ItemCount => _Proxy.Count;


		public override long GetItemId( int position ) => position;

		public override int GetItemViewType( int position ) => (int) _Proxy[position].ViewType;

		public override RecyclerView.ViewHolder OnCreateViewHolder( ViewGroup parent, int viewType )
		{
			CustomViewHolder viewHolder = ( (ViewType) viewType ) switch
										  {
											  ViewType.TextHeader => new HeaderViewHolder(AndroidContext.CreateContentView(parent, Resource.Layout.DefaultHeaderCell, false) ?? throw new NullReferenceException(nameof(Resource.Layout.DefaultHeaderCell))),

											  ViewType.TextFooter => new FooterViewHolder(AndroidContext.CreateContentView(parent, Resource.Layout.DefaultFooterCell, false) ?? throw new NullReferenceException(nameof(Resource.Layout.DefaultFooterCell))),

											  ViewType.CustomHeader => new CustomHeaderViewHolder(new HeaderFooterContainer(AndroidContext)),

											  ViewType.CustomFooter => new CustomFooterViewHolder(new HeaderFooterContainer(AndroidContext)),

											  _ => CreateHolder(parent)
										  };

			_viewHolders.Add(viewHolder);

			return viewHolder;
		}
		protected ContentBodyViewHolder CreateHolder( ViewGroup parent )
		{
			AView view = AndroidContext.CreateContentView(parent, Resource.Layout.ContentCell, false) ?? throw new NullReferenceException(nameof(Resource.Layout.CellLayout));
			var viewHolder = new ContentBodyViewHolder(view);
			viewHolder.ItemView.SetOnClickListener(this);
			viewHolder.ItemView.SetOnLongClickListener(this);
			return viewHolder;
		}

		public override void OnBindViewHolder( RecyclerView.ViewHolder holder, int position )
		{
			RowInfo rowInfo = _Proxy[position];

			if ( !( holder is CustomViewHolder viewHolder ) ) return;
			viewHolder.RowInfo = rowInfo;

			if ( !rowInfo.Section.IsVisible ||
				 ( rowInfo.ViewType == ViewType.CustomFooter && !rowInfo.Section.FooterVisible ) )
			{
				viewHolder.ItemView.Visibility = ViewStates.Gone;
				viewHolder.ItemView.SetMinimumHeight(0);
				if ( viewHolder.ItemView.LayoutParameters != null ) viewHolder.ItemView.LayoutParameters.Height = 0;
				return;
			}

			viewHolder.ItemView.Visibility = ViewStates.Visible;

			switch ( rowInfo.ViewType )
			{
				case ViewType.TextHeader:
					BindHeaderView((HeaderViewHolder) viewHolder);
					break;

				case ViewType.TextFooter:
					BindFooterView((FooterViewHolder) viewHolder);
					break;

				case ViewType.CustomHeader:
					BindCustomHeaderFooterView(viewHolder, rowInfo.Section.HeaderView);
					break;

				case ViewType.CustomFooter:
					BindCustomHeaderFooterView(viewHolder, rowInfo.Section.FooterView);
					break;

				default:
					BindContentView((ContentBodyViewHolder) viewHolder, position);
					break;
			}
		}


		public void OnClick( AView? view )
		{
			int position = _RecyclerView.GetChildAdapterPosition(view);

			//TODO: It is desirable that the forms side has Selected property and reflects it.
			//      But do it at a later as iOS side doesn't have that process.
			DeselectRow();

			AView? child = GetChild(view);
			if ( !( child is BaseCellView cell ) ||
				 !( _Proxy[position].Cell?.IsEnabled ?? false ) ) //if Xamarin.Forms.Cell.IsEnable is false, does nothing. 
			{
				return;
			}

			_SettingsView.Model.RowSelected(_Proxy[position].Cell);

			cell.RowSelected(this, position);
		}
		public virtual bool OnLongClick( AView? view )
		{
			int position = _RecyclerView.GetChildAdapterPosition(view);

			DeselectRow();

			if ( _Proxy[position].Section.UseDragSort ) { return false; }

			bool? result = null;
			if ( GetChild(view) is BaseCellView cell ) { result = cell.RowLongPressed(this, position); }

			if ( !( _Proxy[position].Cell?.IsEnabled ?? false ) )
			{
				//if FormsCell IsEnable is false, does nothing. 
				return false;
			}

			_SettingsView.Model.RowLongPressed(_Proxy[position].Cell);

			return result ?? true;
		}
		protected AView? GetChild( AView? view ) => view?.FindViewById<LinearLayout>(Resource.Id.ContentCellBody)?.GetChildAt(0);


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


		protected void BindHeaderView( HeaderViewHolder holder )
		{
			if ( holder is null ) throw new NullReferenceException(nameof(holder));
			if ( holder.RowInfo is null ) throw new NullReferenceException(nameof(holder.RowInfo));
			if ( holder.ItemView is null ) throw new NullReferenceException(nameof(holder.ItemView));

			Section section = holder.RowInfo.Section;
			AView view = holder.ItemView;

			//judging cell height
			int cellHeight;
			double individualHeight = section.HeaderHeight;

			if ( individualHeight > 0d ) { cellHeight = (int) AndroidContext.ToPixels(individualHeight); }
			else if ( _SettingsView.HeaderHeight > -1 ) { cellHeight = (int) AndroidContext.ToPixels(_SettingsView.HeaderHeight); }
			else
			{
				cellHeight = -1; // Height Auto
			}

			if ( cellHeight >= 0 )
			{
				view.SetMinimumHeight(Shared.sv.SettingsView.MIN_ROW_HEIGHT / 2);
				if ( view.LayoutParameters != null ) view.LayoutParameters.Height = cellHeight;
			}

			//text view setting
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
		protected void BindFooterView( FooterViewHolder holder )
		{
			Section? section = holder.RowInfo?.Section;
			AView view = holder.ItemView;

			//footer visible setting
			if ( string.IsNullOrEmpty(section?.FooterText) )
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

			// //judging cell height
			// if ( section != null )
			// {
			// 	double individualHeight = section.FooterHeight;
			//
			// 	int cellHeight;
			// 	if ( individualHeight > 0d ) { cellHeight = (int) _Context.ToPixels(individualHeight); }
			// 	else if ( _SettingsView.HeaderHeight > -1 ) { cellHeight = (int) _Context.ToPixels(_SettingsView.HeaderHeight); }
			// 	else
			// 	{
			// 		cellHeight = -1; // Height Auto
			// 	}
			//
			// 	if ( cellHeight >= 0 )
			// 	{
			// 		view.SetMinimumHeight(Shared.SettingsView.MIN_ROW_HEIGHT / 2);
			// 		if ( view.LayoutParameters != null )
			// 			view.LayoutParameters.Height = cellHeight;
			// 	}
			// }


			//text view setting
			holder.TextView.SetPadding((int) view.Context.ToPixels(_SettingsView.FooterPadding.Left), (int) view.Context.ToPixels(_SettingsView.FooterPadding.Top), (int) view.Context.ToPixels(_SettingsView.FooterPadding.Right), (int) view.Context.ToPixels(_SettingsView.FooterPadding.Bottom));

			holder.TextView.Typeface = FontUtility.CreateTypeface(_SettingsView.FooterFontFamily, _SettingsView.FooterFontAttributes);
			holder.TextView.SetTextSize(Android.Util.ComplexUnitType.Sp, (float) _SettingsView.FooterFontSize);
			holder.TextView.SetBackgroundColor(_SettingsView.FooterBackgroundColor.ToAndroid());
			if ( _SettingsView.FooterTextColor != Color.Default ) { holder.TextView.SetTextColor(_SettingsView.FooterTextColor.ToAndroid()); }

			//update text
			holder.TextView.Text = section?.FooterText;
		}

		protected void BindCustomHeaderFooterView( CustomViewHolder holder, Xamarin.Forms.View formsView )
		{
			switch ( holder )
			{
				case CustomHeaderViewHolder header:
					BindCustomHeaderFooterView(header, formsView);
					return;

				case CustomFooterViewHolder footer:
					BindCustomHeaderFooterView(footer, formsView);
					return;
			}
		}
		protected void BindCustomHeaderFooterView( CustomHeaderViewHolder holder, Xamarin.Forms.View formsView )
		{
			if ( holder.ItemView is null ) return;
			holder.ItemView.ViewHolder = holder;
			holder.ItemView.FormsCell = formsView;
		}
		protected void BindCustomHeaderFooterView( CustomFooterViewHolder holder, Xamarin.Forms.View formsView )
		{
			if ( holder.ItemView is null ) return;
			holder.ItemView.ViewHolder = holder;
			holder.ItemView.FormsCell = formsView;
		}
		protected void BindContentView( ContentBodyViewHolder holder, int position )
		{
			if ( holder is null ) throw new NullReferenceException(nameof(holder));
			if ( holder.RowInfo is null ) throw new NullReferenceException(nameof(holder.RowInfo));
			if ( holder.ItemView is null ) throw new NullReferenceException(nameof(holder.ItemView));

			Cell? formsCell = holder.RowInfo.Cell;
			AView layout = holder.ItemView;

			holder.RowInfo = _Proxy[position];

			AView? nativeCell = holder.Body.GetChildAt(0);
			if ( nativeCell != null ) { holder.Body.RemoveViewAt(0); }

			nativeCell = CellFactory.GetCell(formsCell,
											 nativeCell,
											 _RecyclerView,
											 AndroidContext,
											 _SettingsView
											);

			if ( position == _SelectedIndex )
			{
				DeselectRow();
				nativeCell.Selected = true;

				_PreSelectedCell = nativeCell;
			}

			var minHeight = (int) Math.Max(AndroidContext.ToPixels(_SettingsView.RowHeight), _MinRowHeight);

			//it is necessary to set both
			layout.SetMinimumHeight(minHeight);
			nativeCell.SetMinimumHeight(minHeight);

			if ( layout.LayoutParameters != null )
			{
				if ( !_SettingsView.HasUnevenRows )
				{
					// if not Uneven, set the larger one of RowHeight and MinRowHeight.
					layout.LayoutParameters.Height = minHeight;
				}
				else if ( formsCell != null &&
						  formsCell.Height > -1 )
				{
					// if the cell itself was specified height, set it.
					layout.SetMinimumHeight((int) AndroidContext.ToPixels(formsCell.Height));
					layout.LayoutParameters.Height = (int) AndroidContext.ToPixels(formsCell.Height);
				}
				else if ( formsCell is ViewCell viewCell )
				{
					// if used a view cell, calculate the size and layout it.
					SizeRequest size = viewCell.View.Measure(_SettingsView.Width, double.PositiveInfinity);
					viewCell.View.Layout(new Rectangle(0, 0, size.Request.Width, size.Request.Height));
					layout.LayoutParameters.Height = (int) AndroidContext.ToPixels(size.Request.Height);
				}
				else { layout.LayoutParameters.Height = ViewGroup.LayoutParams.WrapContent; }
			}

			holder.Body.AddView(nativeCell, 0);

			
			double height = layout.Height;
			double cellHeight = holder.Body.Height;
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
				_Proxy.Dispose();

				foreach ( CustomViewHolder holder in _viewHolders ) { holder.Dispose(); }

				_viewHolders.Clear();
			}

			base.Dispose(disposing);
		}
	}
}