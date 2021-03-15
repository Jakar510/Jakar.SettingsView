using System;
using Jakar.SettingsView.Shared.Config;
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

		protected BaseAiAccessoryCell( Cell cell ) : base(cell)
		{
			_Accessory = InstanceCreator.Create<TAccessory>(this);
			_ContentView.AddArrangedSubview(_Accessory);
			_Accessory.HeightAnchor.ConstraintEqualTo(_ContentView.HeightAnchor).Active = true;
			NSLayoutConstraint width = _Accessory.WidthAnchor.ConstraintEqualTo(_ContentView.WidthAnchor, SVConstants.Layout.ColumnFactors.Accessory);
			width.Active = true;
			width.Priority = SVConstants.Layout.Priority.DefaultHigh;
			_ContentView.AddConstraint(width);
			// AccessoryView
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Accessory.RemoveFromSuperview();
				_Accessory.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}