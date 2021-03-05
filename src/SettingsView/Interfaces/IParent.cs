
#nullable enable
namespace Jakar.SettingsView.Shared.Interfaces
{
	public interface IParent<TParent>
	{
		public TParent? Parent { get; set; }
	}
}