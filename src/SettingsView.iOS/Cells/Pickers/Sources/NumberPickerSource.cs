using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

#nullable enable
namespace Jakar.SettingsView.iOS.Cells.Sources
{
	[Foundation.Preserve(AllMembers = true)]
	public class NumberPickerSource : BasePickerSource<int>
	{
		public void SetNumbers( int min, int max )
		{
			if ( min < 0 ) min = 0;
			if ( max < 0 ) max = 0;
			if ( min > max )
			{
				//Set min value to zero temporally, because it is sometimes min greater than max depending on the order which min and max value is bound.
				min = 0;
			}

			SetItems(Enumerable.Range(min, max - min + 1).ToList());
		}
	}
}