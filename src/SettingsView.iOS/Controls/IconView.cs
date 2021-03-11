using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration;
using BaseCellView = Jakar.SettingsView.iOS.BaseCell.BaseCellView;
using Size = Xamarin.Forms.Size;

#nullable enable
namespace Jakar.SettingsView.iOS.Controls
{
	[Foundation.Preserve(AllMembers = true)]
	public class IconView : UIImageView, IUpdateIcon<BaseAiDescriptionCell, UIImage, IImageSourceHandler>
	{
		private IconCellBase _CurrentCell => _Renderer.Cell as IconCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		protected Size _iconSize;
		internal NSLayoutConstraint HeightConstraint { get; set; } = new();
		internal NSLayoutConstraint WidthConstraint { get; set; } = new();
		internal NSLayoutConstraint MinHeightConstraint { get; set; } = new();
		protected CancellationTokenSource? _IconTokenSource { get; set; }
		protected float _IconRadius { get; set; }
		protected BaseAiDescriptionCell _Renderer { get; private set; }


		public IconView( BaseAiDescriptionCell renderer ) : base() => _Renderer = renderer;
		public void SetCell( BaseAiDescriptionCell renderer ) { _Renderer = renderer ?? throw new NullReferenceException(nameof(renderer)); }

		// public static IconView Create( View view, BaseCellView cell, int id )
		// {
		// 	IconView result = view.FindViewById<IconView>(id) ?? throw new NullReferenceException(nameof(id));
		// 	result.SetCell(cell);
		// 	return result;
		// }

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
				HeightConstraint.Active = false;
				WidthConstraint.Active = false;
				HeightConstraint?.Dispose();
				WidthConstraint?.Dispose();
			}

			HeightConstraint = HeightAnchor.ConstraintEqualTo((nfloat) size.Height);
			WidthConstraint = WidthAnchor.ConstraintEqualTo((nfloat) size.Width);

			HeightConstraint.Priority = 999f; // fix warning-log:Unable to simultaneously satisfy constraints.
			HeightConstraint.Active = true;
			WidthConstraint.Active = true;

			UpdateConstraints();

			_iconSize = size;
			return true;
		}

		public bool UpdateIconRadius()
		{
			Layer.CornerRadius = (float) _CurrentCell.GetIconRadius();
			return true;
		}

		private void UpdateIcon()
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
		}

		private void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		{
			_iconTokenSource = new CancellationTokenSource();
			CancellationToken token = _iconTokenSource.Token;
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

		public UIImage CreateRoundImage( UIImage image )
		{
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
			return image;
		}
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

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_IconTokenSource?.Dispose();
			_IconTokenSource = null;
		}
	}
}