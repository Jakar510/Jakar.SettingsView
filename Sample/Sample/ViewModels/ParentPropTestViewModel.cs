namespace Jakar.SettingsView.Sample.Shared.ViewModels
{
	public class ParentPropTestViewModel : ViewModelBase
	{
		public ParentPropTestViewModel() { }

		protected override void ParentChanged( object obj ) { base.ParentChanged(obj); }
	}
}