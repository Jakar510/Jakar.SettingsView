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

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	internal class FormsViewContainer : FrameLayout, INativeElementView
	{
		// Get internal members
		private static readonly Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.Android.Platform+DefaultRenderer");

		public CustomViewHolder ViewHolder { get; set; }
		public Element Element => FormsCell;
		public bool IsEmpty => _formsCell == null;
		public bool IsMeasureOnce => CustomCell?.IsMeasureOnce ?? false;
		public CustomCell CustomCell { get; set; }

		private IVisualElementRenderer _Renderer { get; set; }


		public FormsViewContainer( Context context ) : base(context) => Clickable = true;
		public FormsViewContainer( Context context, Android.Util.IAttributeSet attributes ) : base(context, attributes) => Clickable = true;
		public FormsViewContainer( Context context, Android.Util.IAttributeSet attributes, int defStyleAttr ) : base(context, attributes, defStyleAttr) => Clickable = true;
		public FormsViewContainer( Context context,
								   Android.Util.IAttributeSet attributes,
								   int defStyleAttr,
								   int defStyleRes ) : base(context, attributes, defStyleAttr, defStyleRes) =>
			Clickable = true;

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


		protected override void OnLayout( bool changed,
										  int l,
										  int t,
										  int r,
										  int b )
		{
			if ( IsEmpty ) { return; }

			double width = Context.FromPixels(r - l);
			double height = Context.FromPixels(b - t);


			Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_Renderer.Element, new Rectangle(0, 0, width, height));

			_Renderer.UpdateLayout();
		}

		private int _heightCache;

		protected override void OnMeasure( int widthMeasureSpec, int heightMeasureSpec )
		{
			if ( _formsCell is null )
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
				return;
			}

			_Renderer ??= Platform.CreateRendererWithContext(_formsCell, Context);
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

		public void UpdateIsEnabled() { Enabled = _formsCell.IsEnabled; }

		protected virtual void CreateNewRenderer( Xamarin.Forms.View cell )
		{
			_formsCell = cell;
			_Renderer = Platform.CreateRendererWithContext(_formsCell, Context);
			AddView(_Renderer.View);
			Platform.SetRenderer(_formsCell, _Renderer);

			_formsCell.IsPlatformEnabled = true;
			UpdateNativeCell();
		}

		public void UpdateCell( Xamarin.Forms.View cell )
		{
			if ( _formsCell == cell &&
				 !CustomCell.IsForceLayout ) { return; }

			CustomCell.IsForceLayout = false;

			if ( _formsCell != null ) { _formsCell.PropertyChanged -= CellPropertyChanged; }

			cell.PropertyChanged += CellPropertyChanged;

			if ( _Renderer == null )
			{
				CreateNewRenderer(cell);
				return;
			}

			var renderer = GetChildAt(0) as IVisualElementRenderer;
			Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_formsCell) ?? DefaultRenderer;
			// ReSharper disable once SuspiciousTypeConversion.Global
			Type rendererType = renderer is IReflectableType reflectable ? reflectable.GetTypeInfo().AsType() : ( renderer != null ? renderer.GetType() : typeof(object) );
			if ( renderer != null &&
				 rendererType == viewHandlerType )
			{
				_formsCell = cell;
				_formsCell.DisableLayout = true;
				foreach ( Element element in _formsCell.Descendants() )
				{
					var c = (VisualElement) element;
					c.DisableLayout = true;
				}

				renderer.SetElement(_formsCell);

				Platform.SetRenderer(_formsCell, _Renderer);

				_formsCell.DisableLayout = false;
				foreach ( Element element in _formsCell.Descendants() )
				{
					var c = (VisualElement) element;
					c.DisableLayout = false;
				}

				if ( _formsCell is Layout viewAsLayout ) viewAsLayout.ForceLayout();

				UpdateNativeCell();
				Invalidate();

				return;
			}

			RemoveView(_Renderer.View);
			Platform.SetRenderer(_formsCell, null);
			_formsCell.IsPlatformEnabled = false;
			_Renderer.View.Dispose();

			_formsCell = cell;
			_Renderer = Platform.CreateRendererWithContext(_formsCell, Context);

			Platform.SetRenderer(_formsCell, _Renderer);
			AddView(_Renderer.View);

			UpdateNativeCell();
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _formsCell != null )
				{
					_formsCell.PropertyChanged -= CellPropertyChanged;
					_formsCell = null;
				}

				CustomCell = null;

				ViewHolder = null;

				_Renderer?.View?.RemoveFromParent();
				_Renderer?.Dispose();
				_Renderer = null;
			}

			base.Dispose(disposing);
		}
	}
}