using System;
using System.ComponentModel;
using System.Reflection;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Misc;
using Jakar.SettingsView.Shared.sv;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
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
		private ISectionFooterHeader? _content;
		protected WeakReference<IVisualElementRenderer>? _RendererRef { get; set; }
		protected bool _Disposed { get; set; }
		protected UITableView? _TableView { get; set; }
		protected NSLayoutConstraint? _HeightConstraint { get; set; }

		protected ISectionFooterHeader? _Content
		{
			get => _content;
			set
			{
				if ( _content == value ) { return; }

				if ( _content != null ) { _content.PropertyChanged -= CellPropertyChanged; }

				_content = value;
				if ( _content is null ) return;
				if ( _TableView is null ) throw new NullReferenceException(nameof(_TableView));

				_content.PropertyChanged += CellPropertyChanged;

				if ( _RendererRef == null ||
					 !_RendererRef.TryGetTarget(out IVisualElementRenderer renderer) )
				{
					renderer = GetNewRenderer(_content, ContentView);
					_RendererRef = new WeakReference<IVisualElementRenderer>(renderer);
				}
				else
				{
					if ( renderer.Element != null &&
						 renderer == Platform.GetRenderer(renderer.Element) )
						renderer.Element.ClearValue(FormsInternals.RendererProperty);

					Type type = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerTypeForObject(_content.View);
					// ReSharper disable once SuspiciousTypeConversion.Global
					Type rendererType = renderer is IReflectableType reflectableType
											? reflectableType.GetTypeInfo().AsType()
											: renderer.GetType();
					if ( rendererType == type ||
						 ( renderer.GetType() == FormsInternals.DefaultRenderer ) && type == null )
						renderer.SetElement(_content.View);
					else
					{
						//when cells are getting reused the element could be already set to another cell so we should dispose based on the renderer and not the renderer.Element
						FormsInternals.DisposeRendererAndChildren(renderer);
						renderer = GetNewRenderer(_content, ContentView);
						_RendererRef = new WeakReference<IVisualElementRenderer>(renderer);
					}
				}

				Platform.SetRenderer(_content.View, renderer);


				if ( renderer.Element != null )
				{
					SizeRequest result = renderer.Element.Measure(_TableView.Frame.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
					double finalW = result.Request.Width;
					if ( _content.View.HorizontalOptions.Alignment == LayoutAlignment.Fill ) { finalW = _TableView.Frame.Width; }

					var finalH = (float) result.Request.Height;

					UpdateNativeCell();

					if ( _HeightConstraint != null )
					{
						_HeightConstraint.Active = false;
						_HeightConstraint?.Dispose();
					}

					_HeightConstraint = renderer.NativeView.HeightAnchor.ConstraintEqualTo(finalH);
					_HeightConstraint.Priority = SVConstants.Layout.Priority.HIGH;
					_HeightConstraint.Active = true;
					renderer.NativeView.AddConstraint(_HeightConstraint);

					Layout.LayoutChildIntoBoundingRegion(_content.View, new Rectangle(0, 0, finalW, finalH));
				}

				foreach ( var element in _content.View.Descendants() )
				{
					if ( element is VisualElement v )
						v.DisableLayout = false;
				}

				renderer.NativeView.UpdateConstraintsIfNeeded();
				renderer.NativeView.SetNeedsDisplay();
			}
		}

		protected ISectionHeader? _Header => _content as ISectionHeader;

		public CustomHeaderFooterView( IntPtr handle ) : base(handle) { }

		protected override void Dispose( bool disposing )
		{
			if ( _Disposed ) { return; }

			if ( disposing )
			{
				if ( _Content != null ) { _Content.PropertyChanged -= CellPropertyChanged; }

				_TableView?.Dispose();
				_TableView = null;

				_HeightConstraint?.Dispose();
				_HeightConstraint = null;

				if ( _RendererRef != null &&
					 _RendererRef.TryGetTarget(out IVisualElementRenderer renderer) &&
					 renderer.Element != null )
				{
					FormsInternals.DisposeModelAndChildrenRenderers(renderer.Element);
					_RendererRef = null;
					renderer.Dispose();
				}

				_Content = null;
			}

			_Disposed = true;

			base.Dispose(disposing);
		}

		public virtual void UpdateNativeCell() { UpdateIsEnabled(); }

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Cell.IsEnabledProperty) ) { UpdateIsEnabled(); }
			else if ( e.IsEqual(Section.TitleProperty) ) { UpdateTitle(); }
			else if ( e.IsEqual(Section.TextColorProperty) ) { UpdateTextColor(); }
			else if ( e.IsEqual(VisualElement.IsEnabledProperty) ) { UpdateIsEnabled(); }
			else if ( e.IsEqual(HeaderView.IsCollapsedProperty) ) { ShowHideSection(); }
			else if ( e.IsEqual(HeaderView.IsCollapsibleProperty) ) { UpdateIsCollapsible(); }
		}

		protected virtual void UpdateIsEnabled() { UserInteractionEnabled = _Content?.IsEnabled ?? false; }

		public void SetContent( ISectionHeader content, Section section, UITableView table ) => SetContent(content, section, table, SVConstants.Section.Header.MinRowHeight);
		public void SetContent( ISectionFooter content, Section section, UITableView table ) => SetContent(content, section, table, SVConstants.Section.Footer.MinRowHeight);
		protected void SetContent( ISectionFooterHeader content,
								   Section section,
								   UITableView table,
								   double minHeight )
		{
			_TableView = table;

			content.Section = section;
			content.View.HeightRequest = Math.Max(minHeight, content.View.HeightRequest);

			_Content = content;
		}
		protected static IVisualElementRenderer GetNewRenderer( ISectionFooterHeader content, UIView ContentView )
		{
			IVisualElementRenderer newRenderer = Platform.CreateRenderer(content.View);
			ContentView.AddSubview(newRenderer.NativeView);

			UIView native = newRenderer.NativeView;
			native.TranslatesAutoresizingMaskIntoConstraints = false;

			native.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
			native.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
			native.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
			native.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
			ContentView.HeightAnchor.ConstraintGreaterThanOrEqualTo(content.View.HeightRequest.ToNFloat()).Active = true;

			return newRenderer;
		}


		public virtual void Update()
		{
			UpdateIsEnabled();
			UpdateIsCollapsible();
			UpdateTitle();
			UpdateTextColor();
		}

		protected void UpdateIsCollapsible() { UserInteractionEnabled = _Header?.IsCollapsible ?? false; }
		protected void ShowHideSection() { _Content?.Section?.ShowHideSection(); }
		protected void UpdateTitle()
		{
			if ( _Content is null ) throw new NullReferenceException(nameof(_Content));
			if ( _Content.Section is null ) throw new NullReferenceException(nameof(_Content.Section));

			if ( _Content is ISectionHeader header ) { header.SetText(_Content.Section.Title); }

			if ( _Content is ISectionFooter footer ) { footer.SetText(_Content.Section.FooterText); }
		}
		protected void UpdateTextColor()
		{
			if ( _Content is null ) throw new NullReferenceException(nameof(_Content));
			if ( _Content.Section is null ) throw new NullReferenceException(nameof(_Content.Section));

			_Content.SetTextColor(_Content.Section.TextColor);
		}
	}
}