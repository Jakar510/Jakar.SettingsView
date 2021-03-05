using System;
using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class HeaderFooterContainer : FrameLayout, INativeElementView
	{
		// Get internal members
		protected static readonly Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.Android.Platform+DefaultRenderer");

		public CustomViewHolder? ViewHolder { get; set; }
		protected IVisualElementRenderer? _Renderer { get; set; }


		private BaseHeaderFooterView? _view;

		public BaseHeaderFooterView? View
		{
			get => _view;
			set
			{
				if ( _view == value ) return;
				UpdateCell(value);
			}
		}

		public Element? Element => View;
		protected HeaderView? _Header => View as HeaderView;

		public HeaderFooterContainer( Context context ) : base(context) => Clickable = true;
		public HeaderFooterContainer( Context context, IAttributeSet? attrs ) : base(context, attrs) => Clickable = true;


		public override bool OnTouchEvent( MotionEvent? e )
		{
			_Header?.Clicked();
			return false;
		}


		protected override void OnLayout( bool changed,
										  int l,
										  int t,
										  int r,
										  int b )
		{
			// if ( _IsEmpty ) { return; }
			double width = Context.FromPixels(r - l);
			double height = Context.FromPixels(b - t);

			if ( _Renderer is null ) throw new NullReferenceException(nameof(_Renderer));
			Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_Renderer.Element, new Rectangle(0, 0, width, height));

			_Renderer.UpdateLayout();
		}

		protected override void OnMeasure( int widthMeasureSpec, int heightMeasureSpec )
		{
			int width = MeasureSpec.GetSize(widthMeasureSpec);
			if ( _Renderer is null )
			{
				SetMeasuredDimension(width, 0);
				return;
			}

			if ( ViewHolder?.RowInfo != null &&
				 ViewHolder.RowInfo.ViewType == ViewType.CustomFooter &&
				 !ViewHolder.RowInfo.Section.FooterVisible )
			{
				SetMeasuredDimension(width, 0);
				return;
			}

			SizeRequest measure = _Renderer.Element.Measure(Context.FromPixels(width), double.PositiveInfinity, MeasureFlags.IncludeMargins);
			var height = (int) Context.ToPixels(measure.Request.Height);

			SetMeasuredDimension(width, height);
		}

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
			else if ( e.PropertyName == VisualElement.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
			else if ( e.PropertyName == HeaderView.IsCollapsedProperty.PropertyName ||
					  e.PropertyName == HeaderView.IsCollapsibleProperty.PropertyName ) { ShowHideSection(); }
		}
		private void ShowHideSection() { View?.Section?.ShowHideSection(); }

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public void UpdateIsEnabled() { Enabled = _view?.IsEnabled ?? true; }

		public void UpdateCell( BaseHeaderFooterView? view )
		{
			if ( _view != null ) { _view.PropertyChanged -= CellPropertyChanged; }

			if ( view is null ) return;
			view.PropertyChanged += CellPropertyChanged;

			if ( _Renderer == null )
			{
				CreateNewRenderer(view);
				return;
			}

			if ( GetChildAt(0) is IVisualElementRenderer renderer )
			{
				Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_view) ?? DefaultRenderer;
				// ReSharper disable once SuspiciousTypeConversion.Global
				Type rendererType = renderer is IReflectableType reflectableType
										? reflectableType.GetTypeInfo().AsType()
										: renderer.GetType();
				if ( rendererType == viewHandlerType )
				{
					_view = view;
					_view.DisableLayout = true;
					foreach ( var element in _view.Descendants() )
					{
						var c = (VisualElement) element;
						c.DisableLayout = true;
					}

					renderer.SetElement(_view);

					Platform.SetRenderer(_view, _Renderer);

					_view.DisableLayout = false;
					foreach ( var element in _view.Descendants() )
					{
						var c = (VisualElement) element;
						c.DisableLayout = false;
					}

					var viewAsLayout = _view as Layout;
					viewAsLayout?.ForceLayout();

					UpdateNativeCell();
					Invalidate();
					return;
				}
			}

			_Renderer = Platform.CreateRendererWithContext(_view, Context);

			Platform.SetRenderer(_view, _Renderer);
			AddView(_Renderer.View);

			UpdateNativeCell();
			Invalidate();

			// 	var renderer = GetChildAt(0) as IVisualElementRenderer;
			// 	Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_formsCell) ?? DefaultRenderer;
			// 	var reflectableType = renderer as IReflectableType;
			// 	Type rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : ( renderer != null ? renderer.GetType() : typeof(Object) );
			// 	if ( renderer != null &&
			// 		 rendererType == viewHandlerType )
			// 	{
			// 		_formsCell = cell;
			// 		_formsCell.DisableLayout = true;
			// 		foreach ( var element in _formsCell.Descendants() )
			// 		{
			// 			var c = (VisualElement) element;
			// 			c.DisableLayout = true;
			// 		}
			//
			// 		renderer.SetElement(_formsCell);
			//
			// 		Platform.SetRenderer(_formsCell, _Renderer);
			//
			// 		_formsCell.DisableLayout = false;
			// 		foreach ( var element in _formsCell.Descendants() )
			// 		{
			// 			var c = (VisualElement) element;
			// 			c.DisableLayout = false;
			// 		}
			//
			// 		var viewAsLayout = _formsCell as Layout;
			// 		viewAsLayout?.ForceLayout();
			//
			// 		UpdateNativeCell();
			// 		Invalidate();
			//
			// 		return;
			// 	}
			//
			// 	RemoveView(_Renderer.View);
			// 	Platform.SetRenderer(_formsCell, null);
			// 	if ( _formsCell != null )
			// 	{
			// 		_formsCell.IsPlatformEnabled = false;
			// 		_Renderer.View.Dispose();
			// 	}
			//
			// 	_formsCell = cell;
			// }
		}
		protected virtual void CreateNewRenderer( BaseHeaderFooterView view )
		{
			_view = view;
			_Renderer = Platform.CreateRendererWithContext(_view, Context);
			AddView(_Renderer.View);
			Platform.SetRenderer(_view, _Renderer);

			_view.IsPlatformEnabled = true;
			UpdateNativeCell();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _view != null )
				{
					_view.PropertyChanged -= CellPropertyChanged;
					_view = null;
				}

				ViewHolder = null;

				_Renderer?.Dispose();
				_Renderer = null;
			}

			base.Dispose(disposing);
		}
	}
}