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
	public partial class MyCellA : CustomCell
	{
		public MyCellA() { InitializeComponent(); }

		public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(MyCellA), default(string), defaultBindingMode: BindingMode.OneWay);

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
	}
}