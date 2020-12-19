using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Jakar.SettingsView;
using Jakar.SettingsView.Droid;
using Jakar.SettingsView.Shared;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]

namespace Jakar.SettingsView.Droid
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SettingsViewRenderer : ViewRenderer<Shared.SettingsView, RecyclerView>
	{
		protected Page? _ParentPage { get; set; }
		protected SettingsViewRecyclerAdapter? _Adapter { get; set; }
		protected SettingsViewLayoutManager? _LayoutManager { get; set; }
		protected ItemTouchHelper? _ItemTouchHelper { get; set; }
		protected SettingsViewSimpleCallback? _SimpleCallback { get; set; }
		protected SVItemDecoration? _ItemDecoration { get; set; }
		protected Drawable? _Divider { get; set; }
		protected List<IVisualElementRenderer> _ShouldDisposeRenderers { get; } = new List<IVisualElementRenderer>();


		public SettingsViewRenderer( Context context ) : base(context) => AutoPackage = false;

		protected override void OnElementChanged( ElementChangedEventArgs<Shared.SettingsView> e )
		{
			base.OnElementChanged(e);
			Shared.SettingsView? settingsView = e.NewElement;
			if ( settingsView is null ) { return; }

			// Fix scrollbar visibility and flash. https://github.com/xamarin/Xamarin.Forms/pull/10893
			var recyclerView = new RecyclerView(new ContextThemeWrapper(Context, Resource.Style.settingsViewTheme), null, Resource.Attribute.settingsViewStyle);

			// When replaced, No animation.
			//(recyclerView.GetItemAnimator() as DefaultItemAnimator).SupportsChangeAnimations = false;

			_LayoutManager = new SettingsViewLayoutManager(Context ?? throw new NullReferenceException(nameof(Context)), settingsView);
			recyclerView.SetLayoutManager(_LayoutManager);

			_Divider = Context?.GetDrawable(Resource.Drawable.divider) ?? throw new NullReferenceException(nameof(Resource.Drawable.divider));
			_ItemDecoration = new SVItemDecoration(_Divider, settingsView);
			recyclerView.AddItemDecoration(_ItemDecoration);

			SetNativeControl(recyclerView);

			Control.Focusable = false;
			Control.DescendantFocusability = DescendantFocusability.AfterDescendants;

			UpdateSeparatorColor();
			UpdateBackgroundColor();
			UpdateRowHeight();

			_Adapter = new SettingsViewRecyclerAdapter(Context, settingsView, recyclerView);
			Control.SetAdapter(_Adapter);

			_SimpleCallback = new SettingsViewSimpleCallback(settingsView, ItemTouchHelper.Up | ItemTouchHelper.Down, 0);
			_ItemTouchHelper = new ItemTouchHelper(_SimpleCallback);
			_ItemTouchHelper.AttachToRecyclerView(Control);

			Element elm = Element;
			while ( elm != null )
			{
				elm = elm.Parent;
				if ( !( elm is Page page ) ) continue;
				_ParentPage = page;
				_ParentPage.Appearing += ParentPageAppearing;
				break;
			}

			settingsView.Root.CollectionChanged += RootCollectionChanged;
		}
		protected void RootCollectionChanged( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
		{
			if ( e.OldItems == null ) { return; }

			foreach ( Section section in e.OldItems )
			{
				if ( section.HeaderView != null )
				{
					IVisualElementRenderer header = Platform.GetRenderer(section.HeaderView);
					if ( header != null ) { _ShouldDisposeRenderers.Add(header); }
				}

				if ( section.FooterView is null ) continue;
				IVisualElementRenderer footer = Platform.GetRenderer(section.FooterView);
				if ( footer != null ) { _ShouldDisposeRenderers.Add(footer); }
			}
		}
		protected void ParentPageAppearing( object sender, EventArgs e ) { Device.BeginInvokeOnMainThread(() => _Adapter?.DeselectRow()); }
		protected override void OnElementPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.SettingsView.SeparatorColorProperty.PropertyName )
			{
				UpdateSeparatorColor();
				Control.InvalidateItemDecorations();
			}
			else if ( e.PropertyName == Shared.SettingsView.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
			else if ( e.PropertyName == TableView.RowHeightProperty.PropertyName ) { UpdateRowHeight(); }
			else if ( e.PropertyName == Shared.SettingsView.UseDescriptionAsValueProperty.PropertyName ) { _Adapter?.NotifyDataSetChanged(); }
			else if ( e.PropertyName == Shared.SettingsView.SelectedColorProperty.PropertyName ) { }
			else if ( e.PropertyName == Shared.SettingsView.ShowSectionTopBottomBorderProperty.PropertyName )
			{
				//_adapter.NotifyDataSetChanged();
				Control.InvalidateItemDecorations();
			}
			else if ( e.PropertyName == TableView.HasUnevenRowsProperty.PropertyName ) { _Adapter?.NotifyDataSetChanged(); }
			else if ( e.PropertyName == Shared.SettingsView.ScrollToTopProperty.PropertyName ) { UpdateScrollToTop(); }
			else if ( e.PropertyName == Shared.SettingsView.ScrollToBottomProperty.PropertyName ) { UpdateScrollToBottom(); }
		}

		protected void UpdateSeparatorColor() { _Divider?.SetTint(Element.SeparatorColor.ToAndroid()); }
		protected void UpdateRowHeight()
		{
			if ( Element.RowHeight < 0 ) { Element.RowHeight = Shared.SettingsView.MIN_ROW_HEIGHT; }
			else { _Adapter?.NotifyDataSetChanged(); }
		}
		protected void UpdateScrollToTop()
		{
			if ( !Element.ScrollToTop ) return;
			_LayoutManager?.ScrollToPosition(0);
			Element.ScrollToTop = false;
		}
		protected void UpdateScrollToBottom()
		{
			if ( !Element.ScrollToBottom ) return;
			if ( _Adapter != null ) { _LayoutManager?.ScrollToPosition(_Adapter.ItemCount - 1); }

			Element.ScrollToBottom = false;
		}
		protected new void UpdateBackgroundColor()
		{
			if ( Element.BackgroundColor != Color.Default ) { Control.SetBackgroundColor(Element.BackgroundColor.ToAndroid()); }
		}

		protected void DisposeChildRenderer( VisualElement view )
		{
			IVisualElementRenderer renderer = Platform.GetRenderer(view);
			if ( renderer is null ) return;
			if ( renderer.View.Handle != IntPtr.Zero )
			{
				renderer.View.RemoveFromParent();
				renderer.View.Dispose();
			}

			renderer.Dispose();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				foreach ( Section section in Element.Root )
				{
					if ( section.HeaderView != null ) { DisposeChildRenderer(section.HeaderView); }

					if ( section.FooterView != null ) { DisposeChildRenderer(section.FooterView); }
				}

				foreach ( IVisualElementRenderer renderer in _ShouldDisposeRenderers )
				{
					if ( renderer.View.Handle != IntPtr.Zero )
					{
						renderer.View.RemoveFromParent();
						renderer.View.Dispose();
					}

					renderer.Dispose();
				}

				_ShouldDisposeRenderers.Clear();

				Control.RemoveItemDecoration(_ItemDecoration);
				if ( _ParentPage != null )
					_ParentPage.Appearing -= ParentPageAppearing;
				_Adapter?.Dispose();
				_Adapter = null;
				_LayoutManager?.Dispose();
				_LayoutManager = null;
				_SimpleCallback?.Dispose();
				_SimpleCallback = null;
				_ItemTouchHelper?.Dispose();
				_ItemTouchHelper = null;

				_ItemDecoration?.Dispose();
				_ItemDecoration = null;
				_Divider?.Dispose();
				_Divider = null;

				Element.Root.CollectionChanged -= RootCollectionChanged;
			}

			base.Dispose(disposing);
		}
	}
}