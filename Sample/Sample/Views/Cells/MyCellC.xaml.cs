using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.Views.Cells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyCellC : CustomCell
	{
		public MyCellC() { InitializeComponent(); }
	}
}