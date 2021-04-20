﻿using System;
using Jakar.SettingsView.Shared.sv;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.Shared.Events
{
	public class RowSelectedEventHandler : EventArgs
	{
		public Section Section { get; set; }
		public Cell    Cell    { get; set; }

		public RowSelectedEventHandler( Section section, Cell cell )
		{
			Section = section;
			Cell    = cell;
		}
	}
}