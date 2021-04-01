using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration;
using Size = Xamarin.Forms.Size;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class IconView : UIImageView, IUpdateIcon<BaseCellView, UIImage, IImageSourceHandler>, IInitializeControl
	{
		protected IconCellBase _CurrentCell => _Renderer.Cell as IconCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));
		
		protected Size _iconSize;
		internal NSLayoutConstraint? HeightConstraint { get; set; } = new();
		internal NSLayoutConstraint? WidthConstraint { get; set; } = new();
		protected CancellationTokenSource? _IconTokenSource { get; set; }
		protected float _IconRadius { get; set; }
		protected BaseCellView _Renderer { get; private set; }


		public IconView( BaseCellView renderer ) : base()
		{
			_Renderer = renderer;

			ClipsToBounds = true;
			BackgroundColor = UIColor.Clear;
			
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
			SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);
			SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 
			SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);      //if possible, not to expand.
		}


		public void Initialize( Stack parent )
		{
			parent.AddArrangedSubview(this);
			// parent.BringSubviewToFront(this);
			
			TopAnchor.ConstraintEqualTo(parent.TopAnchor).Active = true;
			BottomAnchor.ConstraintEqualTo(parent.BottomAnchor).Active = true;
			LeftAnchor.ConstraintEqualTo(parent.LeftAnchor).Active = true;
			
			UpdateConstraintsIfNeeded();
			LayoutIfNeeded();
			UpdateIconSize();
		}
		public void SetCell( BaseCellView renderer ) { _Renderer = renderer ?? throw new NullReferenceException(nameof(renderer)); }

		
		public bool UpdateIconSize()
		{
			Size size;
			if ( _CurrentCell.IconSize != default ) { size = _CurrentCell.GetIconSize(); }
			else if ( _CurrentCell.Parent.CellIconSize != default ) { size = _CurrentCell.Parent.CellIconSize; }
			else { size = new Size(32, 32); }

			//do nothing when current size is previous size
			if ( size == _iconSize ) { return false; }

			if ( _iconSize != default )
			{
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
			}

			WidthConstraint = WidthAnchor.ConstraintEqualTo(size.Width.ToNFloat());
			WidthConstraint.Active = true;

			HeightConstraint = HeightAnchor.ConstraintEqualTo(size.Height.ToNFloat());
			HeightConstraint.Priority = SVConstants.Layout.Priority.HIGH; // fix warning-log:Unable to simultaneously satisfy constraints.
			HeightConstraint.Active = true;

			UpdateConstraints();

			_iconSize = size;
			_Renderer.SetNeedsLayout();
			return true;
		}
		public bool UpdateIconRadius()
		{
			Layer.CornerRadius = (float) _CurrentCell.GetIconRadius();
			_Renderer.SetNeedsLayout();
			return true;
		}
		protected void UpdateIcon()
		{
			if ( _IconTokenSource is not null &&
				 !_IconTokenSource.IsCancellationRequested ) { _IconTokenSource.Cancel(); }

			UpdateIconSize();

			Image?.Dispose();
			Image = null;

			if ( _CurrentCell.IconSource is not null )
			{
				//hide IconView because UIStackView Distribution won't work when a image isn't set.
				Hidden = false;

				if ( ImageCacheController.Instance.ObjectForKey(FromObject(_CurrentCell.IconSource.GetHashCode())) is UIImage cache )
				{
					Image = cache;
					return;
				}

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CurrentCell.IconSource.GetType());
				LoadIconImage(handler, _CurrentCell.IconSource);
			}
			else { Hidden = true; }

			_Renderer.SetNeedsLayout();
		}
		protected void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		{
			_IconTokenSource?.Cancel();
			_IconTokenSource?.Dispose();
			_IconTokenSource = null;

			_IconTokenSource = new CancellationTokenSource();
			CancellationToken token = _IconTokenSource.Token;
			UIImage? image = null;

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
									   _CurrentCell.IconSource is null ) return;
								  ImageCacheController.Instance.SetObjectforKey(image, FromObject(_CurrentCell.IconSource.GetHashCode()));
								  BeginInvokeOnMainThread(() =>
														  {
															  Image = image;
															  SetNeedsLayout();
														  }
														 );
							  },
							  token
							 );
		}

		public Size GetIconSize() => _CurrentCell.GetIconSize();


		public bool Refresh( bool forceLoad = false )
		{
			if ( _IconTokenSource is not null &&
				 !_IconTokenSource.IsCancellationRequested )
			{
				//if previous task be alive, cancel. 
				_IconTokenSource.Cancel();
			}

			UpdateIconSize();

			if ( _CurrentCell.IconSource is not null )
			{
				Hidden = false;
				if ( ImageCacheController.Instance.ObjectForKey(FromObject(_CurrentCell.IconSource.GetHashCode())) is UIImage cache )
				{
					Image = cache;
					return true;
				}

				_IconTokenSource?.Dispose();
				_IconTokenSource = new CancellationTokenSource();

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CurrentCell.IconSource.GetType());
				LoadIconImage(handler, _CurrentCell.IconSource, _IconTokenSource.Token);
			}
			else { Hidden = true; }

			_Renderer.SetNeedsLayout();
			return true;
		}
		public void LoadIconImage( IImageSourceHandler handler, ImageSource source, CancellationToken token ) { Task.Run(async () => { await LoadImage(handler, source, token).ConfigureAwait(true); }, token); }
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
				 _CurrentCell.IconSource is null ) return;

			ImageCacheController.Instance.SetObjectforKey(image, FromObject(_CurrentCell.IconSource.GetHashCode()));
			await Device.InvokeOnMainThreadAsync(async () =>
												 {
													 Image?.Dispose();
													 Image = null;

													 await Task.Delay(50, token); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
													 Image = image;
													 SetNeedsLayout();
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
		public bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == IconCellBase.IconSizeProperty.PropertyName ||
				 e.PropertyName == IconCellBase.IconRadiusProperty.PropertyName ||
				 e.PropertyName == IconCellBase.IconSourceProperty.PropertyName ) { return Update(); }

			return false;
		}
		public bool Update()
		{
			UpdateIconRadius();
			Refresh(true);
			// _Cell.Invalidate();
			return true;
		}
		public bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellIconRadiusProperty.PropertyName ) { return Update(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellIconSizeProperty.PropertyName ) { return Update(); }

			return false;
		}
		
		public void Enable() { Alpha = SVConstants.Cell.ENABLED_ALPHA; }
		public void Disable() { Alpha = SVConstants.Cell.DISABLED_ALPHA; }
		
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


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				RemoveFromSuperview();
				Image?.Dispose();
				
				WidthConstraint?.Dispose();
				HeightConstraint?.Dispose();

				_IconTokenSource?.Dispose();
				_IconTokenSource = null;
			}

			base.Dispose(disposing);
		}
	}
}