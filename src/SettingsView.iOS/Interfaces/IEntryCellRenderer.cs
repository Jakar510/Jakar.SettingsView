using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Jakar.SettingsView.iOS.Interfaces
{
	[Foundation.Preserve(AllMembers = true)]
	public interface IEntryCellRenderer
	{
		public void UpdateWithForceLayout( Action updateAction );
		public bool UpdateWithForceLayout( Func<bool> updateAction );
		public void DoneEdit();
	}
}