namespace Jakar.SettingsView.iOS.Cells
{
	[Foundation.Preserve(AllMembers = true)]
	public class CustomCellContent : UIView
	{
		private bool _disposed;
		private View? _View { get; set; }
		public CustomCell CustomCell { get; set; }
		private NSLayoutConstraint? _HeightConstraint { get; set; }

		private WeakReference<IVisualElementRenderer>? _RendererRef { get; set; }
		private nfloat _lastFrameWidth = -9999d.ToNFloat();
		private nfloat _lastMeasureWidth = -9999d.ToNFloat();
		private nfloat _lastMeasureHeight = -9999d.ToNFloat();

		public CustomCellContent( CustomCell customCell ) => CustomCell = customCell;


		protected override void Dispose( bool disposing )
		{
			if ( _disposed ) { return; }

			if ( disposing )
			{
				if ( _View is not null ) { _View.PropertyChanged -= CellPropertyChanged; }

				_HeightConstraint?.Dispose();
				_HeightConstraint = null;

				IVisualElementRenderer? renderer = null;
				if ( _RendererRef is not null &&
					 _RendererRef.TryGetTarget(out renderer) &&
					 renderer.Element is not null )
				{
					FormsInternals.DisposeModelAndChildrenRenderers(renderer.Element);
					_RendererRef = null;
				}

				renderer?.Dispose();

				_View = null;
			}

			_disposed = true;

			base.Dispose(disposing);
		}

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		protected virtual void UpdateIsEnabled()
		{
			if ( _View != null ) UserInteractionEnabled = _View.IsEnabled;
		}

		public virtual void UpdateCell( View? view, UITableView tableView )
		{
			if ( _View == view &&
				 !CustomCell.IsForceLayout ) { return; }

			CustomCell.IsForceLayout = false;

			if ( _View is not null ) { _View.PropertyChanged -= CellPropertyChanged; }

			_View = view;
			if ( _View is null )
			{
				if ( _RendererRef != null &&
					 _RendererRef.TryGetTarget(out IVisualElementRenderer renderer) )
				{
					renderer.Dispose();
					_RendererRef = null;
				}
			}
			else
			{
				_View.PropertyChanged += CellPropertyChanged;

				if ( _RendererRef is null ||
					 !_RendererRef.TryGetTarget(out IVisualElementRenderer renderer) ) { renderer = GetNewRenderer(); }
				else
				{
					if ( renderer.Element is not null &&
						 renderer == Platform.GetRenderer(renderer.Element) )
						renderer.Element.ClearValue(FormsInternals.RendererProperty);

					Type? type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(_View);
					// ReSharper disable once SuspiciousTypeConversion.Global
					Type? rendererType = renderer is IReflectableType reflectableType
											 ? reflectableType.GetTypeInfo().AsType()
											 : renderer.GetType();
					if ( rendererType == type ||
						 ( renderer.GetType() == FormsInternals.DefaultRenderer ) && type is null ) { renderer.SetElement(_View); }
					else
					{
						//when cells are getting reused the element could be already set to another cell
						//so we should dispose based on the renderer and not the renderer.Element
						FormsInternals.DisposeRendererAndChildren(renderer);
						renderer = GetNewRenderer();
					}
				}

				Platform.SetRenderer(_View, renderer);

				if ( !CustomCell.IsMeasureOnce ||
					 tableView.Frame.Width.Equals(_lastFrameWidth) )
				{
					_lastFrameWidth = tableView.Frame.Width;
					double height = double.PositiveInfinity;
					nfloat width = tableView.Frame.Width -
								   ( CustomCell.UseFullSize
										 ? 0
										 : 32 ); // BaseCellView  layout margin
					if ( renderer.Element != null )
					{
						SizeRequest result = renderer.Element.Measure(tableView.Frame.Width, height, MeasureFlags.IncludeMargins);
						_lastMeasureWidth = result.Request.Width.ToNFloat();
						if ( _View.HorizontalOptions.Alignment == LayoutAlignment.Fill ) { _lastMeasureWidth = width; }

						_lastMeasureHeight = result.Request.Height.ToNFloat();
					}

					if ( _HeightConstraint is not null )
					{
						_HeightConstraint.Active = false;
						_HeightConstraint?.Dispose();
					}

					_HeightConstraint = renderer.NativeView.HeightAnchor.ConstraintEqualTo(_lastMeasureHeight);
					_HeightConstraint.Priority = 999f;
					_HeightConstraint.Active = true;

					renderer.NativeView.UpdateConstraintsIfNeeded();
				}

				Layout.LayoutChildIntoBoundingRegion(_View, new Rectangle(0, 0, _lastMeasureWidth, _lastMeasureHeight));
			}

			UpdateNativeCell();
		}


		protected virtual IVisualElementRenderer GetNewRenderer()
		{
			IVisualElementRenderer? newRenderer = Platform.CreateRenderer(_View);
			_RendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
			AddSubview(newRenderer.NativeView);

			UIView? native = newRenderer.NativeView;
			native.TranslatesAutoresizingMaskIntoConstraints = false;

			native.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
			native.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
			native.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
			native.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;

			return newRenderer;
		}
	}
}