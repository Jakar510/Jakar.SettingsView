using System;
using System.ComponentModel;
using System.Reflection;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class CustomCellContent : UIView
	{
		protected WeakReference<IVisualElementRenderer>? _RendererRef { get; set; }
		protected bool _Disposed { get; set; }
		protected NSLayoutConstraint? _HeightConstraint { get; set; }
		protected View? _View { get; set; }
		protected CustomCell _CustomCell { get; set; }

		protected double _lastFrameWidth = -9999d;
		protected double _lastMeasureWidth = -9999d;
		protected double _lastMeasureHeight = -9999d;

		public CustomCellContent( CustomCell cell ) => _CustomCell = cell;

		protected override void Dispose( bool disposing )
		{
			if ( _Disposed ) { return; }

			if ( disposing )
			{
				if ( _View != null ) { _View.PropertyChanged -= CellPropertyChanged; }

				_HeightConstraint?.Dispose();
				_HeightConstraint = null;

				IVisualElementRenderer? renderer = null;
				if ( _RendererRef != null &&
					 _RendererRef.TryGetTarget(out renderer) &&
					 renderer.Element != null )
				{
					FormsInternals.DisposeModelAndChildrenRenderers(renderer.Element);
					_RendererRef = null;
				}

				renderer?.Dispose();

				_View = null;
			}

			_Disposed = true;

			base.Dispose(disposing);
		}

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		protected virtual void UpdateIsEnabled() { UserInteractionEnabled = _View?.IsEnabled ?? false; }

		public virtual void UpdateCell( View cell, UITableView? tableView )
		{
			if ( _View == cell &&
				 !_CustomCell.IsForceLayout ) { return; }

			_CustomCell.IsForceLayout = false;

			if ( _View != null ) { _View.PropertyChanged -= CellPropertyChanged; }

			_View = cell;
			_View.PropertyChanged += CellPropertyChanged;

			if ( _RendererRef == null ||
				 !_RendererRef.TryGetTarget(out IVisualElementRenderer renderer) ) { renderer = GetNewRenderer(); }
			else
			{
				if ( renderer.Element != null &&
					 renderer == Platform.GetRenderer(renderer.Element) )
					renderer.Element.ClearValue(FormsInternals.RendererProperty);

				Type type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(_View);
				// ReSharper disable once SuspiciousTypeConversion.Global
				if ( renderer is IReflectableType reflectable )
				{
					Type rendererType = reflectable.GetTypeInfo().AsType();
					if ( rendererType == type ||
						 ( renderer.GetType() == FormsInternals.DefaultRenderer ) && type == null )
						renderer.SetElement(_View);
					else
					{
						//when cells are getting reused the element could be already set to another cell
						//so we should dispose based on the renderer and not the renderer.Element
						FormsInternals.DisposeRendererAndChildren(renderer);
						renderer = GetNewRenderer();
					}
				}
			}

			Platform.SetRenderer(_View, renderer);

			if ( tableView != null &&
				 ( !_CustomCell.IsMeasureOnce || !tableView.Frame.Width.ToDouble().Equals(_lastFrameWidth) ) )
			{
				_lastFrameWidth = tableView.Frame.Width;
				nfloat width = tableView.Frame.Width -
							   ( _CustomCell.UseFullSize
									 ? 0
									 : 32 ); // CellBaseView layout margin
				if ( renderer.Element != null )
				{
					SizeRequest result = renderer.Element.Measure(tableView.Frame.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
					_lastMeasureWidth = result.Request.Width;
					if ( _View.HorizontalOptions.Alignment == LayoutAlignment.Fill ) { _lastMeasureWidth = width; }

					_lastMeasureHeight = result.Request.Height;
				}

				if ( _HeightConstraint != null )
				{
					_HeightConstraint.Active = false;
					_HeightConstraint?.Dispose();
				}

				_HeightConstraint = renderer.NativeView.HeightAnchor.ConstraintEqualTo((nfloat) _lastMeasureHeight);
				_HeightConstraint.Priority = 999f;
				_HeightConstraint.Active = true;

				renderer.NativeView.UpdateConstraintsIfNeeded();
			}

			Layout.LayoutChildIntoBoundingRegion(_View, new Rectangle(0, 0, _lastMeasureWidth, _lastMeasureHeight));

			UpdateNativeCell();
		}


		protected virtual IVisualElementRenderer GetNewRenderer()
		{
			IVisualElementRenderer newRenderer = Platform.CreateRenderer(_View);
			_RendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
			AddSubview(newRenderer.NativeView);

			UIView native = newRenderer.NativeView;
			native.TranslatesAutoresizingMaskIntoConstraints = false;

			native.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
			native.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
			native.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
			native.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;

			return newRenderer;
		}
	}
}