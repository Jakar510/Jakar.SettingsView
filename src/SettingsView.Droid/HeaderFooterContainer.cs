﻿using System;
using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class HeaderFooterContainer : FrameLayout, INativeElementView
	{
		// Get internal members
		protected static readonly Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.Android.Platform+DefaultRenderer");

		public CustomViewHolder ViewHolder { get; set; }
		public Element Element => FormsCell;
		protected bool IsEmpty => _formsCell == null;

		protected IVisualElementRenderer _renderer;


		public HeaderFooterContainer( Context context ) : base(context) => Clickable = true;
		public HeaderFooterContainer( Context context, IAttributeSet? attrs ) : base(context, attrs) => Clickable = true;


		private Xamarin.Forms.View _formsCell;

		public Xamarin.Forms.View FormsCell
		{
			get => _formsCell;
			set
			{
				if ( _formsCell == value )
					return;
				UpdateCell(value);
			}
		}

		public override bool OnTouchEvent( MotionEvent e ) => false; // pass to parent (ripple effect)

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _formsCell != null )
				{
					_formsCell.PropertyChanged -= CellPropertyChanged;
					_formsCell = null;
				}

				ViewHolder = null;

				//_renderer?.View?.RemoveFromParent();
				//_renderer?.Dispose();
				_renderer = null;
			}

			base.Dispose(disposing);
		}

		protected override void OnLayout( bool changed,
										  int l,
										  int t,
										  int r,
										  int b )
		{
			if ( IsEmpty ) { return; }

			double width = Context.FromPixels(r - l);
			double height = Context.FromPixels(b - t);


			Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_renderer.Element, new Rectangle(0, 0, width, height));

			_renderer.UpdateLayout();
		}

		protected override void OnMeasure( int widthMeasureSpec, int heightMeasureSpec )
		{
			int width = MeasureSpec.GetSize(widthMeasureSpec);
			if ( _renderer == null )
			{
				SetMeasuredDimension(width, 0);
				return;
			}

			if ( ViewHolder.RowInfo.ViewType == ViewType.CustomFooter &&
				 !ViewHolder.RowInfo.Section.FooterVisible )
			{
				SetMeasuredDimension(width, 0);
				return;
			}

			SizeRequest measure = _renderer.Element.Measure(Context.FromPixels(width), double.PositiveInfinity, MeasureFlags.IncludeMargins);
			var height = (int) Context.ToPixels(measure.Request.Height);

			SetMeasuredDimension(width, height);
		}

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public void UpdateIsEnabled() { Enabled = _formsCell.IsEnabled; }

		public void UpdateCell( Xamarin.Forms.View cell )
		{
			if ( _formsCell != null ) { _formsCell.PropertyChanged -= CellPropertyChanged; }

			cell.PropertyChanged += CellPropertyChanged;

			if ( _renderer == null )
			{
				CreateNewRenderer(cell);
				return;
			}

			var renderer = GetChildAt(0) as IVisualElementRenderer;
			Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_formsCell) ?? DefaultRenderer;
			var reflectableType = renderer as IReflectableType;
			Type rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : ( renderer != null ? renderer.GetType() : typeof(Object) );
			if ( renderer != null &&
				 rendererType == viewHandlerType )
			{
				_formsCell = cell;
				_formsCell.DisableLayout = true;
				foreach ( VisualElement c in _formsCell.Descendants() ) { c.DisableLayout = true; }

				renderer.SetElement(_formsCell);

				Platform.SetRenderer(_formsCell, _renderer);

				_formsCell.DisableLayout = false;
				foreach ( VisualElement c in _formsCell.Descendants() ) { c.DisableLayout = false; }

				var viewAsLayout = _formsCell as Layout;
				viewAsLayout?.ForceLayout();

				UpdateNativeCell();
				Invalidate();

				return;
			}

			RemoveView(_renderer.View);
			Platform.SetRenderer(_formsCell, null);
			_formsCell.IsPlatformEnabled = false;
			_renderer.View.Dispose();

			_formsCell = cell;
			_renderer = Platform.CreateRendererWithContext(_formsCell, Context);

			Platform.SetRenderer(_formsCell, _renderer);
			AddView(_renderer.View);

			UpdateNativeCell();
		}

		protected virtual void CreateNewRenderer( Xamarin.Forms.View cell )
		{
			_formsCell = cell;
			_renderer = Platform.CreateRendererWithContext(_formsCell, Context);
			AddView(_renderer.View);
			Platform.SetRenderer(_formsCell, _renderer);

			_formsCell.IsPlatformEnabled = true;
			UpdateNativeCell();
		}
	}
}