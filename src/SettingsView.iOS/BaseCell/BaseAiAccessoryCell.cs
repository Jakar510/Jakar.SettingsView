using System;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseAiAccessoryCell<TAccessory> : BaseAiDescriptionCell where TAccessory : UIView
	{
		protected TAccessory _Accessory { get; }
		// protected UIStackView AccessoryStack { get; }

		protected BaseAiAccessoryCell( Cell cell ) : base(cell) { _Accessory = InstanceCreator.Create<TAccessory>(this); }

		protected override void Dispose( bool disposing )
		{
			base.Dispose(disposing);
			_Accessory.RemoveFromSuperview();
			_Accessory.Dispose();
			// _AccessoryStack.Dispose();
		}
	}
}