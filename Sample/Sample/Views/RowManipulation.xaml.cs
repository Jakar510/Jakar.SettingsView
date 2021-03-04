using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using Jakar.SettingsView;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.sv;

namespace Sample.Views
{
	public partial class RowManipulation : ContentPage
	{
		public RowManipulation() { InitializeComponent(); }

		private void AddFirstClicked( object sender, EventArgs e ) { settings.Root[0].Insert(0, CreateCell()); }

		private void AddLastClicked( object sender, EventArgs e ) { settings.Root[0].Add(CreateCell()); }

		private void Add2ndClicked( object sender, EventArgs e ) { settings.Root[0].Insert(1, CreateCell()); }

		private void DelFirstClicked( object sender, EventArgs e ) { settings.Root[0].RemoveAt(0); }

		private void DelLastClicked( object sender, EventArgs e ) { settings.Root[0].Remove(section.Last()); }

		private void Del2ndClicked( object sender, EventArgs e ) { settings.Root[0].RemoveAt(1); }

		private void Replace1Clicked( object sender, EventArgs e ) { settings.Root[0][0] = CreateCell(); }

		private void AddSecFirstClicked( object sender, EventArgs e ) { settings.Root.Insert(0, CreateSection()); }

		private void AddSecLastClicked( object sender, EventArgs e ) { settings.Root.Add(CreateSection()); }

		private void AddSec2ndClicked( object sender, EventArgs e ) { settings.Root.Insert(1, CreateSection()); }

		private void DelSecFirstClicked( object sender, EventArgs e ) { settings.Root.RemoveAt(0); }

		private void DelSecLastClicked( object sender, EventArgs e ) { settings.Root.Remove(settings.Root.Last()); }

		private void DelSec2ndClicked( object sender, EventArgs e ) { settings.Root.RemoveAt(1); }

		private void ReplaceSec1Clicked( object sender, EventArgs e ) { settings.Root[0] = CreateSection(); }

		private void ShowHide1stClicked( object sender, EventArgs e ) { settings.Root[0].IsVisible = !settings.Root[0].IsVisible; }


		private Cell CreateCell() =>
			new LabelCell
			{
				Title = "AddCell",
				ValueText = "addcell",
				Description = "add cell",
				Hint = "hint"
			};

		private Section CreateSection()
		{
			var sec = new Section()
					  {
						  Title = "Additional Section",
						  FooterText = "Footer"
					  };
			sec.Add(new LabelCell
					{
						Title = "AddCell",
						ValueText = "addcell",
						Description = "add cell in new section",
						Hint = "hint"
					});
			return sec;
		}
	}
}