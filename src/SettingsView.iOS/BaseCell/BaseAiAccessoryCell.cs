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

			_Accessory.HeightAnchor.ConstraintEqualTo(_ContentView.HeightAnchor).Active = true;
			var valueStackWidth = NSLayoutConstraint.Create(_Accessory,
															NSLayoutAttribute.Width,
															NSLayoutRelation.Equal,
															_ContentView,
															NSLayoutAttribute.Width,
															SVConstants.Layout.ColumnFactors.Accessory,
															SVConstants.Layout.Factor.Zero
														   );
			_Accessory.AddConstraint(valueStackWidth);
			_ContentView.AddArrangedSubview(_Accessory);
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