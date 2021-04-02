// unset

using System;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IUpdateIcon<in TCell, TImage, in Thandler> : IDisposable
	{
		public void Enable();
		public void Disable();

		public bool UpdateIconRadius();
		public bool UpdateIconSize();

		public void SetCell( TCell cell );
		public Size GetIconSize();
		public bool Refresh( bool forceLoad = false );
		public void LoadIconImage( Thandler handler, ImageSource source, CancellationToken token );
		public TImage CreateRoundImage( TImage image );

		public bool UpdateParent( object sender, PropertyChangedEventArgs e );
		public bool Update( object sender, PropertyChangedEventArgs e );
		public bool Update();
	}
}