using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using BaseCellView = Jakar.SettingsView.Droid.BaseCell.BaseCellView;
using AContext = Android.Content.Context;
using Size = Xamarin.Forms.Size;

#nullable enable
namespace Jakar.SettingsView.Droid.Controls
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class IconView : ImageView, IUpdateIcon<BaseCell.BaseCellView, Bitmap, IImageSourceHandler>
	{
		private IconCellBase _CurrentCell => _Cell.Cell as IconCellBase ?? throw new NullReferenceException(nameof(_CurrentCell));

		protected Bitmap? _Image { get; set; }
		protected CancellationTokenSource? _IconTokenSource { get; set; }
		protected BaseCellView _Cell { get; set; }
		protected float _IconRadius { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public IconView( AContext context ) : base(context) { }
		public IconView( AContext context, IAttributeSet attributes ) : base(context, attributes) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


		public void SetCell( BaseCellView cell ) { _Cell = cell ?? throw new NullReferenceException(nameof(cell)); }
		public static IconView Create( Android.Views.View view, BaseCellView cell, int id )
		{
			IconView result = view.FindViewById<IconView>(id) ?? throw new NullReferenceException(nameof(id));
			result.SetCell(cell);
			return result;
		}

		public bool UpdateIconRadius()
		{
			_IconRadius = _Cell.AndroidContext.ToPixels(_CurrentCell.GetIconRadius());
			return true;
		}
		public bool UpdateIconSize()
		{
			if ( LayoutParameters is null ) throw new NullReferenceException(nameof(LayoutParameters));

			Size size = GetIconSize();
			LayoutParameters.Width = (int) _Cell.AndroidContext.ToPixels(size.Width);
			LayoutParameters.Height = (int) _Cell.AndroidContext.ToPixels(size.Height);
			return true;
		}
		public Size GetIconSize() => _CurrentCell.GetIconSize();


		public bool Refresh( bool forceLoad = false )
		{
			if ( _IconTokenSource != null &&
				 !_IconTokenSource.IsCancellationRequested )
			{
				//if previous task be alive, cancel. 
				_IconTokenSource.Cancel();
			}

			UpdateIconSize();

			if ( Drawable != null )
			{
				SetImageDrawable(null);
				SetImageBitmap(null);
			}

			if ( _CurrentCell.IconSource != null )
			{
				Visibility = ViewStates.Visible;
				if ( ImageCacheController.Instance.Get(_CurrentCell.IconSource.GetHashCode()) is Bitmap cache &&
					 !forceLoad )
				{
					SetImageBitmap(cache);
					_Cell.Invalidate();
					return true;
				}

				_IconTokenSource?.Dispose();
				_IconTokenSource = new CancellationTokenSource();
				var handler = Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IImageSourceHandler>(_CurrentCell.IconSource.GetType());
				LoadIconImage(handler, _CurrentCell.IconSource, _IconTokenSource.Token);
			}
			else { Visibility = ViewStates.Invisible; }

			return true;
		}
		public void LoadIconImage( IImageSourceHandler handler, ImageSource source, CancellationToken token ) { Task.Run(async () => { await LoadImage(handler, source, token).ConfigureAwait(true); }, token); }
		protected async Task LoadImage( IImageSourceHandler handler, ImageSource source, CancellationToken token )
		{
			_Image?.Dispose();
			_Image = await handler.LoadImageAsync(source, _Cell.AndroidContext, token);
			token.ThrowIfCancellationRequested();
			_Image = CreateRoundImage(_Image);
			// try
			// {
			// 	//entrust disposal of returned old image to Android OS.
			// 	ImageCacheController.Instance.Put(source.GetHashCode(), image);
			// }
			// catch ( Exception ) { }

			await Device.InvokeOnMainThreadAsync(async () =>
												 {
													 await Task.Delay(50, token); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
													 SetImageBitmap(_Image);
													 _Cell.Invalidate();
												 }
												);
			// image.Dispose();
		}
		// public void LoadIconImage( IImageSourceHandler handler, ImageSource source, CancellationToken token )
		// {
		// 	Bitmap? image = null;
		//
		// 	// float scale = _Context.Resources?.DisplayMetrics?.Density ?? throw new NullReferenceException(nameof(_Context.Resources.DisplayMetrics.Density));
		// 	Task.Run(async () =>
		// 			 {
		// 				 image = await handler.LoadImageAsync(source, _Cell.AndroidContext, token);
		// 				 token.ThrowIfCancellationRequested();
		// 				 image = CreateRoundImage(image);
		// 			 },
		// 			 token
		// 			)
		// 		.ContinueWith(t =>
		// 					  {
		// 						  if ( !t.IsCompleted ) return;
		// 						  if ( image is null ) return;
		// 						  //entrust disposal of returned old image to Android OS.
		// 						  int id = source.GetHashCode();
		// 						  try
		// 						  {
		// 							  ImageCacheController.Instance.Put(id, image);
		// 						  }
		// 						  catch ( Java.Lang.NullPointerException ) { }
		//
		// 						  Device.BeginInvokeOnMainThread(async () =>
		// 														 {
		// 															 await Task.Delay(50, token); // in case repeating the same source, sometimes the icon not be shown. by inserting delay it be shown.
		// 															 SetImageBitmap(image);
		// 															 _Cell.Invalidate();
		// 														 }
		// 														);
		// 					  },
		// 					  token
		// 					 );
		// }


		public Bitmap CreateRoundImage( Bitmap image )
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
			_Cell.Invalidate();
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
			_Image?.Dispose();
			_IconTokenSource?.Dispose();
			_IconTokenSource = null;
			SetImageDrawable(null);
			SetImageBitmap(null);
		}

	}
}