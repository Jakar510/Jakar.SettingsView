using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Enumerations;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Size = Xamarin.Forms.Size;


#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Preserve(AllMembers = true)]
	public class IconView : BaseViewManager<UIImageView, IconCellBase>, IUpdateIcon<BaseCellView, UIImage, IImageSourceHandler>, IInitializeControl
	{
		protected override IUseConfiguration _Config => throw new NotImplementedException(nameof(IUseConfiguration));

		protected Size                     _IconSize        { get; set; }
		internal  NSLayoutConstraint?      HeightConstraint { get; set; } = new();
		internal  NSLayoutConstraint?      WidthConstraint  { get; set; } = new();
		protected CancellationTokenSource? _IconTokenSource { get; set; }
		protected float                    _IconRadius      { get; set; }


		public IconView( BaseCellView renderer, IconCellBase cell ) : this(new UIImageView(), renderer, cell) { }

		public IconView( UIImageView control, BaseCellView renderer, IconCellBase cell ) : base(renderer,
																								cell,
																								control,
																								control.TintColor,
																								control.BackgroundColor,
																								-1
																							   )
		{
			Control.ClipsToBounds   = true;
			Control.BackgroundColor = UIColor.Clear;

			Control.CompressionPriorities(LayoutPriority.Highest, UILayoutConstraintAxis.Vertical, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 

			Control.HuggingPriority(LayoutPriority.Low,     UILayoutConstraintAxis.Vertical);
			Control.HuggingPriority(LayoutPriority.Highest, UILayoutConstraintAxis.Horizontal); //if possible, not to expand.
		}


		public override void Initialize( UIStackView root )
		{
			root.AddArrangedSubview(Control);

			// Control.TopAnchor.ConstraintEqualTo(root.TopAnchor).Active       = true;
			// Control.BottomAnchor.ConstraintEqualTo(root.BottomAnchor).Active = true;
			Control.LeftAnchor.ConstraintEqualTo(root.LeftAnchor).Active = true;

			Control.UpdateConstraintsIfNeeded();
			Control.LayoutIfNeeded();
			UpdateIconSize();
		}


		public bool UpdateIconSize()
		{
			Size size = _Cell.IconConfig.IconSize;

			// do nothing when current size is previous size
			if ( _IconSize == size ) { return false; }

			//remove previous constraint
			if ( HeightConstraint != null )
			{
				HeightConstraint.Active = false;
				HeightConstraint.Dispose();
			}

			if ( WidthConstraint != null )
			{
				WidthConstraint.Active = false;
				WidthConstraint.Dispose();
			}

			if ( size == default ) { size = SvConstants.Sv.Icon.size; }

			WidthConstraint          = Control.WidthAnchor.ConstraintEqualTo(size.Width.ToNFloat());
			WidthConstraint.Priority = LayoutPriority.High.ToFloat(); // fix warning-log:Unable to simultaneously satisfy constraints.
			WidthConstraint.Active   = true;

			HeightConstraint          = Control.HeightAnchor.ConstraintEqualTo(size.Height.ToNFloat());
			HeightConstraint.Priority = LayoutPriority.Highest.ToFloat(); // fix warning-log:Unable to simultaneously satisfy constraints.
			HeightConstraint.Active   = true;

			Control.UpdateConstraints();

			_IconSize = size;
			Renderer.SetNeedsLayout();
			return true;
		}

		public bool UpdateIconRadius()
		{
			Control.Layer.CornerRadius = _Cell.IconConfig.IconRadius.ToFloat();
			Renderer.SetNeedsLayout();
			return true;
		}

		protected void UpdateIcon()
		{
			if ( _IconTokenSource is not null &&
				 !_IconTokenSource.IsCancellationRequested ) { _IconTokenSource.Cancel(); }

			UpdateIconSize();

			Control.Image?.Dispose();
			Control.Image = null;

			if ( _Cell.IconSource is not null )
			{
				//hide IconView because UIStackView Distribution won't work when a image isn't set.
				Control.Hidden = false;

				if ( ImageCacheController.Instance.ObjectForKey(NSObject.FromObject(_Cell.IconSource.GetHashCode())) is UIImage cache )
				{
					Control.Image = cache;
					return;
				}

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_Cell.IconSource.GetType());
				LoadIconImage(handler, _Cell.IconSource);
			}
			else { Control.Hidden = true; }

			Renderer.SetNeedsLayout();
		}

		protected void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		{
			_IconTokenSource?.Cancel();
			_IconTokenSource?.Dispose();
			_IconTokenSource = null;

			_IconTokenSource = new CancellationTokenSource();
			CancellationToken token = _IconTokenSource.Token;
			UIImage?          image = null;

			var scale = (float) UIScreen.MainScreen.Scale;

			Task.Run(async () =>
					 {
						 if ( source is FontImageSource ) { DispatchQueue.MainQueue.DispatchSync(async () => { image = await handler.LoadImageAsync(source, token, scale: scale); }); }
						 else { image = await handler.LoadImageAsync(source, token, scale: scale); }

						 token.ThrowIfCancellationRequested();
					 },
					 token
					)
				.ContinueWith(t =>
							  {
								  if ( !t.IsCompleted ) return;

								  if ( image is null ||
									   _Cell.IconSource is null ) return;

								  ImageCacheController.Instance.SetObjectforKey(image, NSObject.FromObject(_Cell.IconSource.GetHashCode()));

								  Device.BeginInvokeOnMainThread(() =>
																 {
																	 Control.Image = image;
																	 Control.SetNeedsLayout();
																 }
																);
							  },
							  token
							 );
		}

		public Size GetIconSize() => _Cell.IconConfig.IconSize;


		public bool Refresh( bool forceLoad = false )
		{
			if ( _IconTokenSource is not null &&
				 !_IconTokenSource.IsCancellationRequested )
			{
				//if previous task be alive, cancel. 
				_IconTokenSource.Cancel();
			}

			UpdateIconSize();

			if ( _Cell.IconSource is not null )
			{
				Control.Hidden = false;

				if ( ImageCacheController.Instance.ObjectForKey(NSObject.FromObject(_Cell.IconSource.GetHashCode())) is UIImage cache )
				{
					Control.Image = cache;
					return true;
				}

				_IconTokenSource?.Dispose();
				_IconTokenSource = new CancellationTokenSource();

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_Cell.IconSource.GetType());
				LoadIconImage(handler, _Cell.IconSource, _IconTokenSource.Token);
			}
			else { Control.Hidden = true; }

			Renderer.SetNeedsLayout();
			return true;
		}

		public void LoadIconImage( IImageSourceHandler handler, ImageSource source, CancellationToken token )
		{
			Task.Run(async () => { await LoadImage(handler, source, token).ConfigureAwait(true); }, token);
		}

		protected async Task LoadImage( IImageSourceHandler handler, ImageSource source, CancellationToken token )
		{
			token.ThrowIfCancellationRequested();
			UIImage? image = null;

			var scale = (float) UIScreen.MainScreen.Scale;

			if ( source is FontImageSource ) { DispatchQueue.MainQueue.DispatchSync(async () => { image = await handler.LoadImageAsync(source, token, scale); }); }
			else { image = await handler.LoadImageAsync(source, token, scale); }

			if ( image is null ) return;

			// image = CreateRoundImage(image);

			if ( image is null ||
				 _Cell.IconSource is null ) return;

			ImageCacheController.Instance.SetObjectforKey(image, NSObject.FromObject(_Cell.IconSource.GetHashCode()));

			await Device.InvokeOnMainThreadAsync(async () =>
												 {
													 Control.Image?.Dispose();
													 Control.Image = null;

													 await Task.Delay(50, token); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
													 Control.Image = image;
													 Control.SetNeedsLayout();
												 }
												);

			image.Dispose();
		}

		public UIImage CreateRoundImage( UIImage image ) =>

			// using ( image )
			// {
			// 	// using var clipArea = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888 ?? throw new NullReferenceException(nameof(Bitmap.Config.Argb8888)));
			// 	// using var canvas = new Canvas(clipArea ?? throw new NullReferenceException(nameof(clipArea)));
			// 	// using var paint = new Paint(PaintFlags.AntiAlias);
			// 	// canvas.DrawARGB(0, 0, 0, 0);
			// 	// canvas.DrawRoundRect(new RectF(0, 0, image.Width, image.Height), _IconRadius, _IconRadius, paint);
			// 	//
			// 	// paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
			// 	//
			// 	// using var rect = new Android.Graphics.Rect(0, 0, image.Width, image.Height);
			// 	// canvas.DrawBitmap(image, rect, rect, paint);
			// 	//
			// 	// return clipArea;
			//
			// }
			image;

		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(IconCellBase.IconSizeProperty, IconCellBase.IconRadiusProperty, IconCellBase.IconSourceProperty) )
			{
				Update();
				return true;
			}

			return false;
		}

		public override void Update()
		{
			UpdateIconRadius();
			Refresh(true);
			base.Update();
		}

		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(Shared.sv.SettingsView.CellIconRadiusProperty, Shared.sv.SettingsView.CellIconSizeProperty) )
			{
				Update();
				return true;
			}

			return false;
		}

		// private void UpdateIconSize()
		// {
		// 	if ( _Icon is null ) return;
		// 	if ( _IconCell is null ) return;
		//
		// 	Size size = _IconCell.GetIconSize(); // size = new Size(32, 32); 
		//
		// 	//do nothing when current size is previous size
		// 	if ( size == _IconSize ) { return; }
		//
		// 	if ( _IconSize != default )
		// 	{
		// 		// remove previous constraint
		// 		if ( _IconConstraintHeight != null )
		// 		{
		// 			_IconConstraintHeight.Active = false;
		// 			_IconConstraintHeight?.Dispose();
		// 		}
		//
		// 		if ( _IconConstraintWidth != null )
		// 		{
		// 			_IconConstraintWidth.Active = false;
		// 			_IconConstraintWidth?.Dispose();
		// 		}
		// 	}
		//
		// 	_IconConstraintHeight = _Icon.HeightAnchor.ConstraintEqualTo((nfloat) size.Height);
		// 	_IconConstraintWidth = _Icon.WidthAnchor.ConstraintEqualTo((nfloat) size.Width);
		//
		// 	_IconConstraintHeight.Priority = SVConstants.Layout.Priority.HIGH; // fix warning-log:Unable to simultaneously satisfy constraints.
		// 	_IconConstraintHeight.Active = true;
		// 	_IconConstraintWidth.Active = true;
		//
		// 	_Icon.UpdateConstraints();
		//
		// 	_IconSize = size;
		// }
		// private void UpdateIconRadius()
		// {
		// 	if ( _Icon is null ) return; // for HotReload
		// 	if ( _IconCell is null ) return;
		//
		// 	_Icon.Layer.CornerRadius = _IconCell.GetIconRadius().ToNFloat();
		// }
		// private void UpdateIcon()
		// {
		// 	if ( _Icon is null ) return; // for HotReload
		// 	if ( _IconCell is null ) return;
		//
		// 	if ( _IconTokenSource is not null &&
		// 		 !_IconTokenSource.IsCancellationRequested ) { _IconTokenSource.Cancel(); }
		//
		// 	UpdateIconSize();
		//
		// 	if ( _Icon.Image is not null ) { _Icon.Image = null; }
		//
		// 	if ( _IconCell.IconSource is not null )
		// 	{
		// 		// hide IconView because UIStackView Distribution won't work when a image isn't set.
		// 		_Icon.Hidden = false;
		//
		// 		if ( ImageCacheController.Instance.ObjectForKey(FromObject(_IconCell.IconSource.GetHashCode())) is UIImage cache )
		// 		{
		// 			_Icon.Image = cache;
		// 			return;
		// 		}
		//
		// 		var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_IconCell.IconSource.GetType());
		// 		LoadIconImage(handler, _IconCell.IconSource);
		// 	}
		// 	else { _Icon.Hidden = true; }
		// }
		// private void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		// {
		// 	if ( _Icon is null ) return; // for HotReload
		// 	if ( _IconCell is null ) return;
		//
		// 	_IconTokenSource = new CancellationTokenSource();
		// 	CancellationToken token = _IconTokenSource.Token;
		// 	UIImage? image = null;
		//
		// 	var scale = (float) UIScreen.MainScreen.Scale;
		// 	Task.Run(async () =>
		// 			 {
		// 				 if ( source is FontImageSource ) { DispatchQueue.MainQueue.DispatchSync(async () => { image = await handler.LoadImageAsync(source, token, scale: scale); }); }
		// 				 else { image = await handler.LoadImageAsync(source, token, scale: scale); }
		//
		// 				 token.ThrowIfCancellationRequested();
		// 			 },
		// 			 token
		// 			)
		// 		.ContinueWith(t =>
		// 					  {
		// 						  if ( !t.IsCompleted ) return;
		// 						  if ( _IconCell.IconSource is not null &&
		// 							   image is not null )
		// 							  ImageCacheController.Instance.SetObjectforKey(image, FromObject(_IconCell.IconSource.GetHashCode()));
		// 						  BeginInvokeOnMainThread(() =>
		// 												  {
		// 													  _Icon.Image = image;
		// 													  SetNeedsLayout();
		// 												  }
		// 												 );
		// 					  },
		// 					  token
		// 					 );
		// }

		private bool _disposed;

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _disposed ) { return; }

				_disposed = true;

				Control.Image?.Dispose();

				WidthConstraint?.Dispose();
				HeightConstraint?.Dispose();

				_IconTokenSource?.Dispose();
				_IconTokenSource = null;
			}

			base.Dispose(disposing);
		}
	}
}
