// unset

using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.sv
{
	[Xamarin.Forms.Internals.Preserve(true, false)]
	public interface IIcons
	{
		public ImageSource GetImageSource( string name );
	}

	[Xamarin.Forms.Internals.Preserve(true, false)]
	public static class Icons
	{
		private static readonly IIcons service = DependencyService.Get<IIcons>();

		public static ImageSource CollapseSolidBlack => service.GetImageSource("CollapseSolidBlack");
		public static ImageSource CollapseSolidWhite => service.GetImageSource("CollapseSolidWhite");
		public static ImageSource ExpandSolidBlack => service.GetImageSource("ExpandSolidBlack");
		public static ImageSource ExpandSolidWhite => service.GetImageSource("ExpandSolidWhite");
	}
}