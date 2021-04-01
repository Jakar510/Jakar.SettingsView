// unset

using Foundation;
using Jakar.SettingsView.iOS.Controls;
using UIKit;

namespace Jakar.SettingsView.iOS.Interfaces
{
	[Preserve(AllMembers = true)]
	public interface IInitializeControl
	{
		public void Initialize( Stack parent );
	}
}