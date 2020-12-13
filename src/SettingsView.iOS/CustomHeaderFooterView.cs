using System;
using System.ComponentModel;
using System.Reflection;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS
{
	public class CustomHeaderView : CustomHeaderFooterView
	{
		public CustomHeaderView( IntPtr handle ) : base(handle) { }
	}

	public class CustomFooterView : CustomHeaderFooterView
	{
		public CustomFooterView( IntPtr handle ) : base(handle) { }
	}

	public class CustomHeaderFooterView : UITableViewHeaderFooterView
	{
		private WeakReference<IVisualElementRenderer> _rendererRef;
		private bool _disposed;
		private NSLayoutConstraint _heightConstraint;
		private View _formsCell;

		public CustomHeaderFooterView( IntPtr handle ) : base(handle) { }

		protected override void Dispose( bool disposing )
		{
			if ( _disposed ) { return; }

			if ( disposing )
			{
				if ( _formsCell != null ) { _formsCell.PropertyChanged -= CellPropertyChanged; }

				_heightConstraint?.Dispose();
				_heightConstraint = null;

				IVisualElementRenderer renderer = null;
				if ( _rendererRef != null &&
					 _rendererRef.TryGetTarget(out renderer) &&
					 renderer.Element != null )
				{
					FormsInternals.DisposeModelAndChildrenRenderers(renderer.Element);
					_rendererRef = null;
				}

				renderer?.Dispose();


				_formsCell = null;
			}

			_disposed = true;

			base.Dispose(disposing);
		}

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		protected virtual void UpdateIsEnabled() { UserInteractionEnabled = _formsCell.IsEnabled; }

		public virtual void UpdateCell( View cell, UITableView tableView )
		{
			if ( _formsCell == cell ) { return; }

			if ( _formsCell != null ) { _formsCell.PropertyChanged -= CellPropertyChanged; }

			_formsCell = cell;
			_formsCell.PropertyChanged += CellPropertyChanged;

			IVisualElementRenderer renderer;
			if ( _rendererRef == null ||
				 !_rendererRef.TryGetTarget(out renderer) ) { renderer = GetNewRenderer(); }
			else
			{
				if ( renderer.Element != null &&
					 renderer == Platform.GetRenderer(renderer.Element) )
					renderer.Element.ClearValue(FormsInternals.RendererProperty);

				Type type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(_formsCell);
				var reflectableType = renderer as IReflectableType;
				Type rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : renderer.GetType();
				if ( rendererType == type ||
					 ( renderer.GetType() == FormsInternals.DefaultRenderer ) && type == null )
					renderer.SetElement(_formsCell);
				else
				{
					//when cells are getting reused the element could be already set to another cell
					//so we should dispose based on the renderer and not the renderer.Element
					FormsInternals.DisposeRendererAndChildren(renderer);
					renderer = GetNewRenderer();
				}
			}

			Platform.SetRenderer(_formsCell, renderer);

			double height = double.PositiveInfinity;
			SizeRequest result = renderer.Element.Measure(tableView.Frame.Width, height, MeasureFlags.IncludeMargins);
			double finalW = result.Request.Width;
			if ( _formsCell.HorizontalOptions.Alignment == LayoutAlignment.Fill ) { finalW = tableView.Frame.Width; }

			var finalH = (float) result.Request.Height;

			UpdateNativeCell();

			if ( _heightConstraint != null )
			{
				_heightConstraint.Active = false;
				_heightConstraint?.Dispose();
			}

			_heightConstraint = renderer.NativeView.HeightAnchor.ConstraintEqualTo(finalH);
			_heightConstraint.Priority = 999f;
			_heightConstraint.Active = true;

			Layout.LayoutChildIntoBoundingRegion(_formsCell, new Rectangle(0, 0, finalW, finalH));

			renderer.NativeView.UpdateConstraintsIfNeeded();
		}

		protected virtual IVisualElementRenderer GetNewRenderer()
		{
			IVisualElementRenderer newRenderer = Platform.CreateRenderer(_formsCell);
			_rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
			ContentView.AddSubview(newRenderer.NativeView);

			UIView native = newRenderer.NativeView;
			native.TranslatesAutoresizingMaskIntoConstraints = false;

			native.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			native.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
			native.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
			native.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;

			return newRenderer;
		}
	}
}