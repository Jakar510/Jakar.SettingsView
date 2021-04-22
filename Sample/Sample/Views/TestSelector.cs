using Xamarin.Forms;
using static Jakar.SettingsView.Sample.Shared.ViewModels.DataTemplateTestViewModel;

namespace Jakar.SettingsView.Sample.Shared.Views
{
	public class TestSelector : DataTemplateSelector
	{
		public DataTemplate TemplateA { get; set; }
		public DataTemplate TemplateB { get; set; }

		protected override DataTemplate OnSelectTemplate( object item, BindableObject container )
		{
			var hoge = item as Person;
			return hoge.Name.Contains("A") ? TemplateA : TemplateB;
		}
	}
}