using System.Linq;
using System.Reflection;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Sample.Views;
using Unity;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]

namespace Sample
{
	public partial class App : PrismApplication
	{
		public App( IPlatformInitializer initializer = null ) : base(initializer) { }

		protected override void OnInitialized()
		{
			InitializeComponent();
			
			Xamarin.Forms.Svg.SvgImageSource.RegisterAssembly();
			// Unity.ResolutionFailedException: 'Resolution failed with error: Exception has been thrown by the target of an invocation. For more detailed information run Unity in debug mode: new UnityContainer().AddExtension(new Diagnostic())'

			NavigationService.NavigateAsync("MyNavigationPage/MainPage");
			//MainPage = new AppShell();
		}
		protected override IContainerExtension CreateContainerExtension()
		{
			// https://stackoverflow.com/a/60890148/9530917
			var container = new UnityContainer();
			container.AddExtension(new Diagnostic());
			return new UnityContainerExtension(container);
		}
		protected override void RegisterTypes( IContainerRegistry containerRegistry )
		{
			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<ContentPage>();

			GetType().GetTypeInfo().Assembly.DefinedTypes.Where(t => t.Namespace?.EndsWith(".Views", System.StringComparison.Ordinal) ?? false).ForEach(t => { containerRegistry.RegisterForNavigation(t.AsType(), t.Name); });
		}
	}
}