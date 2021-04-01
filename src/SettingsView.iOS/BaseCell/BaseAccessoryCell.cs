using System;
using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	public abstract class BaseAccessoryCell<TAccessory> : BaseCellView where TAccessory : UIView, IRenderAccessory
	{
		protected IconView _Icon { get; set; }

		/// <summary>
		/// Vertical StackView that is arranged at right. ( put Title and Description ) 
		/// </summary>
		protected Stack _TitleStack { get; set; }

		/// <summary>
		/// Vertical  StackView that is wrapper for the TitleView
		/// </summary>
		protected Stack _ContentStack { get; set; }

		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }

		protected TAccessory _Accessory
		{
			get => (TAccessory) ( AccessoryView ?? throw new NullReferenceException(nameof(AccessoryView)) );
			set => AccessoryView = EditingAccessoryView = value;
		}


		protected BaseAccessoryCell( Cell formsCell ) : base(formsCell)
		{
			_Icon = new IconView(this);

			_TitleStack = Stack.TitleStack();
			_ContentStack = Stack.ContentStack();
			_Title = new TitleView(this);
			_Description = new DescriptionView(this);

			_Accessory = InstanceCreator.Create<TAccessory>(this);

			_Icon.Initialize(_MainStack);
			_Title.Initialize(_ContentStack);
			_Description.Initialize(_TitleStack);
			_TitleStack.AddArrangedSubview(_ContentStack);
			_MainStack.AddArrangedSubview(_TitleStack);

			_ContentStack.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
			_ContentStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultHigh, UILayoutConstraintAxis.Vertical);

			
			_MainStack.Root(this);

			double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SVConstants.Defaults.MIN_ROW_HEIGHT);
			_MinHeightConstraint = _MainStack.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
			_MinHeightConstraint.Priority = SVConstants.Layout.Priority.HIGH; //  fix warning-log:Unable to simultaneously satisfy constraints. this is superior to any other view.
			_MinHeightConstraint.Active = true;

			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _MainStack.AccessibilityIdentifier = Cell.AutomationId; }
			
			SetNeedsLayout();
		}


		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Icon.Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Accessory.Update(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}
		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Icon.UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Accessory.UpdateParent(sender, e) ) { return; }

			if ( e.IsEqual(TableView.RowHeightProperty) ) { UpdateMinRowHeight(_MainStack); }

			else { base.CellPropertyChanged(sender, e); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateMinRowHeight(_MainStack);
			_Icon.Update();
			_Title.Update();
			_Description.Update();
			_Accessory.Update();

			base.UpdateCell(tableView);
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			base.SetEnabledAppearance(isEnabled);

			if ( isEnabled )
			{
				_Icon.Enable();
				_Title.Enable();
				_Description.Enable();
				_Accessory.Enable();
			}
			else
			{
				_Icon.Disable();
				_Title.Disable();
				_Description.Disable();
				_Accessory.Disable();
			}
		}


		protected override void RunDispose()
		{
			AccessoryView?.Dispose();
			EditingAccessoryView?.Dispose();

			_Title.Dispose();
			_Description.Dispose();
			_Icon.Dispose();
			_TitleStack.Dispose();
			_MainStack.Dispose();
			base.RunDispose();
		}
	}
}