using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Widget;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	public abstract class BaseView : IDisposable
	{
		private bool _disposedValue;

		protected internal Color DefaultTextColor { get; set; }
		protected internal float DefaultFontSize { get; set; }
		protected CellBaseView _Cell { get; set; }
		protected internal TextView Label { get; protected set; }
		protected BaseView( CellBaseView baseView, TextView view )
		{
			Label = view ?? throw new NullReferenceException(nameof(view));
			_Cell = baseView ?? throw new NullReferenceException(nameof(baseView));
		}
		protected internal void Enable() { Label.Alpha = CellBaseView.ENABLED_ALPHA; }
		protected internal void Disable() { Label.Alpha = CellBaseView.DISABLED_ALPHA; }

		protected internal abstract bool UpdateText();
		protected internal abstract bool UpdateColor();
		protected internal abstract bool UpdateFontSize();
		protected internal abstract bool UpdateFont();
		// protected void UpdateTitleAlignment() { _Label.TextAlignment = _CellBase.TitleTextAlignment; }

		protected internal abstract bool Update( object sender, PropertyChangedEventArgs e );
		protected internal abstract void Update();
		protected internal abstract bool UpdateParent( object sender, PropertyChangedEventArgs e );


		protected virtual void Dispose( bool disposing )
		{
			if ( !_disposedValue )
			{
				if ( disposing )
				{
					Label.Dispose();
					Label = null;
					_Cell = null;
				}

				// free unmanaged resources (unmanaged objects) and override finalizer   // set large fields to null
				_disposedValue = true;
			}
		}

		// override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~BaseView()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}