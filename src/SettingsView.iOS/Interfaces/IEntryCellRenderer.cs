using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;

namespace Jakar.SettingsView.iOS.Interfaces
{
	[Preserve(AllMembers = true)]
	public interface IEntryCellRenderer
	{
		public void UpdateWithForceLayout( Action updateAction );
		public bool UpdateWithForceLayout( Func<bool> updateAction );

		public void DoneEdit();
	}
}