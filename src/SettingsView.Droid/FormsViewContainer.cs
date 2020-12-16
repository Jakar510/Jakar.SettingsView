using System;
using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class FormsViewContainer : FrameLayout, INativeElementView
	{
		// Get internal members
		private static readonly Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.Android.Platform+DefaultRenderer");

		// public CustomViewHolder? ViewHolder { get; set; }
		public Element? Element => FormsView;
		public bool IsEmpty => _formsView is null;
		public bool IsMeasureOnce => CustomCell?.IsMeasureOnce ?? false;
		public CustomCell? CustomCell { get; set; }

		private IVisualElementRenderer? _Renderer { get; set; }


		public FormsViewContainer( Context context ) : base(context) => Clickable = true;
		public FormsViewContainer( Context context, Android.Util.IAttributeSet attributes ) : base(context, attributes) => Clickable = true;
		public FormsViewContainer( Context context, Android.Util.IAttributeSet attributes, int defStyleAttr ) : base(context, attributes, defStyleAttr) => Clickable = true;
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   int defStyleRes ) : base(context, attributes, defStyleAttr, defStyleRes) =>
			Clickable = true;


		public FormsViewContainer( Context context, Xamarin.Forms.View view ) : base(context) => FormsView = view;
		public FormsViewContainer( Context context, Android.Util.IAttributeSet attributes, Xamarin.Forms.View view ) : base(context, attributes)
		{
			Clickable = true;
			FormsView = view;
		}
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   Xamarin.Forms.View view ) : base(context, attributes, defStyleAttr)
		{
			Clickable = true;
			FormsView = view;
		}
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   int defStyleRes,
								   Xamarin.Forms.View view ) : base(context, attributes, defStyleAttr, defStyleRes)
		{
			Clickable = true;
			FormsView = view;
		}


		public FormsViewContainer( Context context, CustomCell cell ) : base(context) => CustomCell = cell;
		public FormsViewContainer( Context context, Android.Util.IAttributeSet attributes, CustomCell cell ) : base(context, attributes)
		{
			Clickable = true;
			CustomCell = cell;
		}
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   CustomCell cell ) : base(context, attributes, defStyleAttr)
		{
			Clickable = true;
			CustomCell = cell;
		}
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   int defStyleRes,
								   CustomCell cell ) : base(context, attributes, defStyleAttr, defStyleRes)
		{
			Clickable = true;
			CustomCell = cell;
		}


		public FormsViewContainer( Context context, Xamarin.Forms.View view, CustomCell cell ) : this(context, cell) { FormsView = view; }
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   Xamarin.Forms.View view,
								   CustomCell cell ) : this(context, attributes, cell)
		{
			FormsView = view;
		}
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   Xamarin.Forms.View view,
								   CustomCell cell ) : this(context, attributes, defStyleAttr, cell)
		{
			FormsView = view;
		}
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   int defStyleRes,
								   Xamarin.Forms.View view,
								   CustomCell cell ) : this(context, attributes, defStyleAttr, defStyleRes, cell)
		{
			FormsView = view;
		}


		private Xamarin.Forms.View? _formsView;

		public Xamarin.Forms.View? FormsView
		{
			get => _formsView;
			set
			{
				if ( _formsView == value ) return;
				UpdateCell(value);
			}
		}

		public override bool OnTouchEvent( MotionEvent? e ) => false; // pass to parent (ripple effect)


		protected override void OnLayout( bool changed,
										  int l,
										  int t,
										  int r,
										  int b )
		{
			if ( IsEmpty ) { return; }

			double width = Context.FromPixels(r - l);
			double height = Context.FromPixels(b - t);


			if ( _Renderer is null ) return;
			Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_Renderer.Element, new Rectangle(0, 0, width, height));

			_Renderer.UpdateLayout();
		}

		private int _heightCache;

		protected override void OnMeasure( int widthMeasureSpec, int heightMeasureSpec )
		{
			if ( _formsView is null )
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
				return;
			}

			_Renderer ??= Platform.CreateRendererWithContext(_formsView, Context);
			int width = MeasureSpec.GetSize(widthMeasureSpec);

			if ( IsMeasureOnce && _heightCache > 0 )
			{
				SetMeasuredDimension(width, _heightCache);
				return;
			}

			SizeRequest measure = _Renderer.Element.Measure(Context.FromPixels(width), double.PositiveInfinity, MeasureFlags.IncludeMargins);
			var height = (int) Context.ToPixels(measure.Request.Height);

			SetMeasuredDimension(width, height);
			_heightCache = height;
		}

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public void UpdateIsEnabled() { Enabled = _formsView?.IsEnabled ?? false; }

		protected virtual IVisualElementRenderer CreateNewRenderer( Xamarin.Forms.View cell )
		{
			_formsView = cell;
			IVisualElementRenderer? renderer = Platform.CreateRendererWithContext(_formsView, Context);
			AddView(renderer.View);
			Platform.SetRenderer(_formsView, renderer);

			_formsView.IsPlatformEnabled = true;
			UpdateNativeCell();
			return renderer;
		}

		public void UpdateCell( Xamarin.Forms.View? view )
		{
			if ( view is null || CustomCell != null && _formsView == view && !CustomCell.IsForceLayout ) { return; }

			if ( CustomCell != null ) CustomCell.IsForceLayout = false;

			if ( _formsView != null ) { _formsView.PropertyChanged -= CellPropertyChanged; }

			view.PropertyChanged += CellPropertyChanged;

			if ( _Renderer == null )
			{
				_Renderer = CreateNewRenderer(view);
				return;
			}

			var renderer = GetChildAt(0) as IVisualElementRenderer;
			Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_formsView) ?? DefaultRenderer;
			// ReSharper disable once SuspiciousTypeConversion.Global
			Type rendererType = renderer is IReflectableType reflectable ? reflectable.GetTypeInfo().AsType() : ( renderer != null ? renderer.GetType() : typeof(object) );
			if ( renderer != null &&
				 rendererType == viewHandlerType )
			{
				_formsView = view;

				_formsView.DisableLayout = true;
				foreach ( Element element in _formsView.Descendants() )
				{
					var c = (VisualElement) element;
					c.DisableLayout = true;
				}

				renderer.SetElement(_formsView);

				Platform.SetRenderer(_formsView, _Renderer);

				_formsView.DisableLayout = false;
				foreach ( Element element in _formsView.Descendants() )
				{
					var c = (VisualElement) element;
					c.DisableLayout = false;
				}

				if ( _formsView is Layout viewAsLayout )
					viewAsLayout.ForceLayout();

				UpdateNativeCell();
				Invalidate();

				return;
			}

			RemoveView(_Renderer.View);
			Platform.SetRenderer(_formsView, null);
			if ( _formsView != null )
			{
				_formsView.IsPlatformEnabled = false;
				_Renderer.View.Dispose();
			}

			_formsView = view;
			_Renderer = Platform.CreateRendererWithContext(_formsView, Context);

			Platform.SetRenderer(_formsView, _Renderer);
			AddView(_Renderer.View);

			UpdateNativeCell();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _formsView != null )
				{
					_formsView.PropertyChanged -= CellPropertyChanged;
					_formsView = null;
				}

				_Renderer?.View?.RemoveFromParent();
				_Renderer?.Dispose();
				_Renderer = null;
			}

			base.Dispose(disposing);
		}
	}
}