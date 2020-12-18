using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public class IconView : IDisposable
	{
		private CellBaseDescription _CurrentCell => _Cell.Cell as CellBaseDescription ?? throw new NullReferenceException(nameof(_CurrentCell));

		protected CancellationTokenSource? _IconTokenSource { get; set; }
		protected internal ImageView Icon { get; protected set; }
		protected CellBaseView _Cell { get; set; }
		protected float _IconRadius { get; set; }

		public IconView( CellBaseView baseView, ImageView? view )
		{
			Icon = view ?? throw new NullReferenceException(nameof(view));
			_Cell = baseView ?? throw new NullReferenceException(nameof(baseView));
		}


		protected internal bool UpdateIconRadius()
		{
			if ( _CurrentCell.IconRadius >= 0 ) { _IconRadius = _Cell.AndroidContext.ToPixels(_CurrentCell.IconRadius); }
			else if ( _Cell.CellParent != null ) { _IconRadius = _Cell.AndroidContext.ToPixels(_Cell.CellParent.CellIconRadius); }

			return true;
		}
		protected internal bool UpdateIconSize()
		{
			if ( Icon.LayoutParameters is null ) throw new NullReferenceException(nameof(Icon.LayoutParameters));

			Size size = GetIconSize();
			Icon.LayoutParameters.Width = (int) _Cell.AndroidContext.ToPixels(size.Width);
			Icon.LayoutParameters.Height = (int) _Cell.AndroidContext.ToPixels(size.Height);
			return true;
		}

		private Size GetIconSize()
		{
			if ( _CurrentCell.IconSize != default ) { return _CurrentCell.IconSize; }

			if ( _Cell.CellParent != null &&
				 _Cell.CellParent.CellIconSize != default ) { return _Cell.CellParent.CellIconSize; }

			return new Size(36, 36);
		}
		protected internal bool Refresh( bool forceLoad = false )
		{
			if ( _IconTokenSource != null &&
				 !_IconTokenSource.IsCancellationRequested )
			{
				//if previous task be alive, cancel. 
				_IconTokenSource.Cancel();
			}

			UpdateIconSize();

			if ( Icon.Drawable != null )
			{
				Icon.SetImageDrawable(null);
				Icon.SetImageBitmap(null);
			}

			if ( _CurrentCell.IconSource != null )
			{
				Icon.Visibility = ViewStates.Visible;
				if ( ImageCacheController.Instance.Get(_CurrentCell.IconSource.GetHashCode()) is Bitmap cache &&
					 !forceLoad )
				{
					Icon.SetImageBitmap(cache);
					_Cell.Invalidate();
					return true;
				}

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CurrentCell.IconSource.GetType());
				LoadIconImage(handler, _CurrentCell.IconSource);
			}
			else { Icon.Visibility = ViewStates.Gone; }

			return true;
		}
		protected internal bool LoadIconImage( IImageSourceHandler handler, ImageSource source )
		{
			_IconTokenSource = new CancellationTokenSource();
			CancellationToken token = _IconTokenSource.Token;
			Bitmap? image = null;

			// float scale = _Context.Resources?.DisplayMetrics?.Density ?? throw new NullReferenceException(nameof(_Context.Resources.DisplayMetrics.Density));
			Task.Run(async () =>
					 {
						 image = await handler.LoadImageAsync(source, _Cell.AndroidContext, token);
						 token.ThrowIfCancellationRequested();
						 image = CreateRoundImage(image);
					 }, token)
				.ContinueWith(t =>
							  {
								  if ( !t.IsCompleted ) return;
								  //entrust disposal of returned old image to Android OS.
								  ImageCacheController.Instance.Put(_CurrentCell.IconSource.GetHashCode(), image);

								  Device.BeginInvokeOnMainThread(() =>
																 {
																	 Task.Delay(50, token); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
																	 Icon.SetImageBitmap(image);
																	 _Cell.Invalidate();
																 });
							  }, token);

			return true;
		}
		protected internal Bitmap CreateRoundImage( Bitmap image )
		{
			using ( image )
			{
				using var clipArea = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888 ?? throw new NullReferenceException(nameof(Bitmap.Config.Argb8888)));
				using var canvas = new Canvas(clipArea ?? throw new NullReferenceException(nameof(clipArea)));
				using var paint = new Paint(PaintFlags.AntiAlias);
				canvas.DrawARGB(0, 0, 0, 0);
				canvas.DrawRoundRect(new RectF(0, 0, image.Width, image.Height), _IconRadius, _IconRadius, paint);


				paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

				using var rect = new Android.Graphics.Rect(0, 0, image.Width, image.Height);
				canvas.DrawBitmap(image, rect, rect, paint);

				return clipArea;
			}
		}
		protected internal bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBaseIcon.IconSizeProperty.PropertyName ||
				 e.PropertyName == CellBaseIcon.IconRadiusProperty.PropertyName ||
				 e.PropertyName == CellBaseIcon.IconSourceProperty.PropertyName ) { return Update(); }

			return false;
		}
		protected internal bool Update()
		{
			UpdateIconRadius();
			Refresh(true);
			_Cell.Invalidate();
			return true;
		}
		protected internal bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellIconRadiusProperty.PropertyName ) { return Update(); }

			if ( e.PropertyName == Shared.SettingsView.CellIconSizeProperty.PropertyName ) { return Update(); }

			return false;
		}

		public void Dispose()
		{
			_IconTokenSource?.Dispose();
			_IconTokenSource = null;
			Icon?.SetImageDrawable(null);
			Icon?.SetImageBitmap(null);
			Icon?.Dispose();
			Icon = null!;
		}
	}
}