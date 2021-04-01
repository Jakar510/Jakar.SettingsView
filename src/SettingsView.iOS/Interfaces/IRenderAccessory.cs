// unset

using System;
using System.ComponentModel;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Controls;
using UIKit;

namespace Jakar.SettingsView.iOS.Interfaces
{
	public interface IRenderAccessory : IInitializeControl, IDisposable 
	{
		public void Update();
		public bool Update( object sender, PropertyChangedEventArgs e );
		public bool UpdateParent( object sender, PropertyChangedEventArgs e );

		public void Enable();
		public void Disable();
	}
}