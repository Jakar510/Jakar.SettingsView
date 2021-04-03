using System;
using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.Api.Extensions;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using Jakar.SettingsView.Shared.Misc;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class HeaderFooterContainer : FrameLayout, INativeElementView
	{
		// Get internal members
		protected static readonly Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.Android.Platform+DefaultRenderer");

		public Element? Element => _Content?.View;
		public CustomViewHolder? ViewHolder { get; protected set; }
		protected IVisualElementRenderer? _Renderer { get; set; }


		protected ISectionFooterHeader? _content;

		protected ISectionFooterHeader? _Content
		{
			get => _content;
			set
			{
				if ( _content == value ) return;

				if ( _content is not null ) { _content.PropertyChanged -= CellPropertyChanged; }

				if ( value is null ) return;
				value.PropertyChanged += CellPropertyChanged;

				if ( _Renderer is null )
				{
					_content = value;
					_Renderer = Platform.CreateRendererWithContext(_content.View, Context);
					AddView(_Renderer.View);
					Platform.SetRenderer(_content.View, _Renderer);

					_content.IsPlatformEnabled = true;
					Update();
					return;
				}

				if ( GetChildAt(0) is IVisualElementRenderer renderer )
				{
					Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_Content) ?? DefaultRenderer;
					// ReSharper disable once SuspiciousTypeConversion.Global
					Type rendererType = renderer is IReflectableType reflectableType
											? reflectableType.GetTypeInfo().AsType()
											: renderer.GetType();
					if ( rendererType == viewHandlerType )
					{
						_content = value;
						_content.DisableLayout = true;
						foreach ( var element in _content.View.Descendants() )
						{
							if ( element is VisualElement v ) v.DisableLayout = true;
						}

						renderer.SetElement(_content.View);
						Platform.SetRenderer(_content.View, _Renderer);

						_content.DisableLayout = false;
						foreach ( var element in _content.View.Descendants() )
						{
							if ( element is VisualElement v ) v.DisableLayout = false;
						}

						var viewAsLayout = _content.View as Layout;
						viewAsLayout?.ForceLayout();

						Update();
						Invalidate();
						return;
					}
				}

				if ( _content is null ) throw new NullReferenceException(nameof(_content));
				_Renderer = Platform.CreateRendererWithContext(_content.View, Context);

				Platform.SetRenderer(_content.View, _Renderer);
				AddView(_Renderer.View);

				Update();
				Invalidate();
			}
		}

		protected ISectionHeader? _Header => _Content as ISectionHeader;


		public HeaderFooterContainer( Context context ) : base(context) => Clickable = true;
		public HeaderFooterContainer( Context context, IAttributeSet? attrs ) : base(context, attrs) => Clickable = true;
		public HeaderFooterContainer( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) => Clickable = true;


		public void SetContent( ISectionHeader content, Section section, CustomViewHolder holder ) => SetContent(content, section, holder, SvConstants.Section.Header.MIN_ROW_HEIGHT);
		public void SetContent( ISectionFooter content, Section section, CustomViewHolder holder ) => SetContent(content, section, holder, SvConstants.Section.Footer.MIN_ROW_HEIGHT);
		protected void SetContent( ISectionFooterHeader content,
								   Section section,
								   CustomViewHolder holder,
								   double minHeight )
		{
			ViewHolder = holder;
			content.Section = section;
			content.View.HeightRequest = Math.Max(minHeight, content.View.HeightRequest);
			_Content = content;
		}


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

			if ( _Renderer.Element is null ) return;
			SizeRequest measure = _Renderer.Element.Measure(Context.FromPixels(width), double.PositiveInfinity, MeasureFlags.IncludeMargins);
			// double minHeight = _Content is ISectionHeader
			// ? SVConstants.Section.Header.MinRowHeight
			// : SVConstants.Section.Footer.MinRowHeight;
			// var height = (int) Context.ToPixels(Math.Max(measure.Request.Height, minHeight));
			var height = (int) Context.ToPixels(measure.Request.Height);

			SetMeasuredDimension(width, height);
		}

		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Cell.IsEnabledProperty) ) { UpdateIsEnabled(); }
			else if ( e.IsEqual(Section.TitleProperty) ) { UpdateTitle(); }
			else if ( e.IsEqual(Section.TextColorProperty) ) { UpdateTextColor(); }
			else if ( e.IsEqual(VisualElement.IsEnabledProperty) ) { UpdateIsEnabled(); }
			else if ( e.IsEqual(HeaderView.IsCollapsedProperty) ) { ShowHideSection(); }
			else if ( e.IsEqual(HeaderView.IsCollapsibleProperty) ) { UpdateIsCollapsible(); }
		}
		protected void UpdateIsCollapsible() { Clickable = _Header?.IsCollapsible ?? false; }
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


		public virtual void Update()
		{
			UpdateIsEnabled();
			UpdateIsCollapsible();
			UpdateTitle();
			UpdateTextColor();
		}

		public void UpdateIsEnabled() { Enabled = _Content?.IsEnabled ?? true; }


		// public void UpdateCell( ISectionFooterHeader? view )
		// {
		// 		var renderer = GetChildAt(0) as IVisualElementRenderer;
		// 		Type viewHandlerType = Registrar.Registered.GetHandlerTypeForObject(_formsCell) ?? DefaultRenderer;
		// 		var reflectableType = renderer as IReflectableType;
		// 		Type rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : ( renderer != null ? renderer.GetType() : typeof(Object) );
		// 		if ( renderer != null &&
		// 			 rendererType == viewHandlerType )
		// 		{
		// 			_formsCell = cell;
		// 			_formsCell.DisableLayout = true;
		// 			foreach ( var element in _formsCell.Descendants() )
		// 			{
		// 				var c = (VisualElement) element;
		// 				c.DisableLayout = true;
		// 			}
		// 	
		// 			renderer.SetElement(_formsCell);
		// 	
		// 			Platform.SetRenderer(_formsCell, _Renderer);
		// 	
		// 			_formsCell.DisableLayout = false;
		// 			foreach ( var element in _formsCell.Descendants() )
		// 			{
		// 				var c = (VisualElement) element;
		// 				c.DisableLayout = false;
		// 			}
		// 	
		// 			var viewAsLayout = _formsCell as Layout;
		// 			viewAsLayout?.ForceLayout();
		// 	
		// 			UpdateNativeCell();
		// 			Invalidate();
		// 	
		// 			return;
		// 		}
		// 	
		// 		RemoveView(_Renderer.View);
		// 		Platform.SetRenderer(_formsCell, null);
		// 		if ( _formsCell != null )
		// 		{
		// 			_formsCell.IsPlatformEnabled = false;
		// 			_Renderer.View.Dispose();
		// 		}
		// 	
		// 		_formsCell = cell;
		// 	}
		// }
		// protected virtual void CreateNewRenderer( ISectionFooterHeader view )
		// {
		// 	Content = view;
		// 	_Renderer = Platform.CreateRendererWithContext(Content.View, Context);
		// 	AddView(_Renderer.View);
		// 	Platform.SetRenderer(Content.View, _Renderer);
		//
		// 	Content.IsPlatformEnabled = true;
		// 	UpdateNativeCell();
		// }


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _content is not null )
				{
					_content.PropertyChanged -= CellPropertyChanged;
					_content = null;
				}

				ViewHolder = null;

				_Renderer?.Dispose();
				_Renderer = null;
			}

			base.Dispose(disposing);
		}
	}
}