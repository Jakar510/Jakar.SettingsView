using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Rect = Android.Graphics.Rect;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface ICellIcon : IBaseCell
	{
		public ImageView IconView { get; set; }
		public void UpdateIconRadius()
		{
			if ( _CellBase.IconRadius >= 0 ) { IconRadius = AndroidContext.ToPixels(_CellBase.IconRadius); }
			else if ( CellParent != null ) { IconRadius = AndroidContext.ToPixels(CellParent.CellIconRadius); }
		}
		public void UpdateIconSize()
		{
			Size size;
			if ( _CellBase.IconSize != default ) { size = _CellBase.IconSize; }
			else if ( CellParent != null && CellParent.CellIconSize != default ) { size = CellParent.CellIconSize; }
			else { size = new Size(36, 36); }

			if ( IconView.LayoutParameters == null ) return;
			IconView.LayoutParameters.Width = (int) AndroidContext.ToPixels(size.Width);
			IconView.LayoutParameters.Height = (int) AndroidContext.ToPixels(size.Height);
		}
		public void RefreshIcon( bool forceLoad = false )
		{
			if ( IconTokenSource != null && !IconTokenSource.IsCancellationRequested )
			{
				//if previous task be alive, cancel. 
				IconTokenSource.Cancel();
			}

			UpdateIconSize();

			if ( IconView.Drawable != null )
			{
				IconView.SetImageDrawable(null);
				IconView.SetImageBitmap(null);
			}

			if ( _CellBase.IconSource != null )
			{
				IconView.Visibility = ViewStates.Visible;
				if ( ImageCacheController.Instance.Get(_CellBase.IconSource.GetHashCode()) is Bitmap cache && !forceLoad )
				{
					IconView.SetImageBitmap(cache);
					Invalidate();
					return;
				}

				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CellBase.IconSource.GetType());
				LoadIconImage(handler, _CellBase.IconSource);
			}
			else { IconView.Visibility = ViewStates.Gone; }
		}
		public void LoadIconImage( IImageSourceHandler handler, ImageSource source )
		{
			IconTokenSource = new CancellationTokenSource();
			CancellationToken token = IconTokenSource.Token;
			Bitmap image = null;

			// float scale = _Context.Resources?.DisplayMetrics?.Density ?? throw new NullReferenceException(nameof(_Context.Resources.DisplayMetrics.Density));
			Task.Run(async () =>
					 {
						 image = await handler.LoadImageAsync(source, AndroidContext, token);
						 token.ThrowIfCancellationRequested();
						 image = CreateRoundImage(image);
					 }, token)
				.ContinueWith(t =>
							  {
								  if ( !t.IsCompleted )
									  return;
								  //entrust disposal of returned old image to Android OS.
								  ImageCacheController.Instance.Put(_CellBase.IconSource.GetHashCode(), image);

								  Device.BeginInvokeOnMainThread(() =>
																 {
																	 Task.Delay(50, token); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
																	 IconView.SetImageBitmap(image);
																	 Invalidate();
																 });
							  }, token);
		}

		public Bitmap CreateRoundImage( Bitmap image )
		{
			using ( image )
			{
				using var clipArea = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888 ?? throw new NullReferenceException(nameof(Bitmap.Config.Argb8888)));
				using var canvas = new Canvas(clipArea ?? throw new NullReferenceException(nameof(clipArea)));
				using var paint = new Paint(PaintFlags.AntiAlias);
				canvas.DrawARGB(0, 0, 0, 0);
				canvas.DrawRoundRect(new RectF(0, 0, image.Width, image.Height), IconRadius, IconRadius, paint);


				paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));

				using var rect = new Rect(0, 0, image.Width, image.Height);
				canvas.DrawBitmap(image, rect, rect, paint);

				return clipArea;
			}
		}

		public void UpdateIcon( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.IconSizeProperty.PropertyName ) { UpdateIcon(); }
			else if ( e.PropertyName == CellBase.IconRadiusProperty.PropertyName )
			{
				UpdateIconRadius();
				RefreshIcon(true);
			}
		}
		public void UpdateIcon()
		{
			RefreshIcon();
			UpdateIconRadius();
			Invalidate();
		}
		protected void Dispose( bool disposing )
		{
			IconView?.Dispose();
			IconView = null;
		}
	}
}