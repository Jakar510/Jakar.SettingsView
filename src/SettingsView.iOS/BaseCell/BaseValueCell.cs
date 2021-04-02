// unset

using System;
using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;

#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseValueCell<TValueView, TCell, TValueManager> : BaseCellView where TValueView : UIView, new()
																						 where TCell : CellBase
																						 where TValueManager : BaseViewManager<TValueView, TCell>
	{
		public ValueCellBase? ValueCell => Cell as ValueCellBase;
		public ValueTextCellBase? ValueTextCell => Cell as ValueTextCellBase;

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

		/// <summary>
		/// Vertical StackView that is arranged at left of the _TitleStack.( put Hint and ValueText)
		/// </summary>
		protected Stack _ValueStack { get; set; }

		protected HintView _Hint { get; set; }

		// private UIView _FieldWrapper { get; set; }

		protected TValueManager _Value { get; }


		protected BaseValueCell( Cell formsCell ) : base(formsCell)
		{
			_Icon = new IconView(this);

			_TitleStack = Stack.TitleStack();
			_ContentStack = Stack.ContentStack();
			_Title = new TitleView(this);
			_Description = new DescriptionView(this);

			_ValueStack = Stack.ValueStack();
			_Hint = new HintView(this);
			_Value = InstanceCreator.Create<TValueManager>(this);


			// _FieldWrapper = new UIView
			// 				{
			// 					AutosizesSubviews = true
			// 				};
			// _FieldWrapper.SetContentHuggingPriority(SVConstants.Layout.Priority.DefaultLow, UILayoutConstraintAxis.Horizontal);
			// _FieldWrapper.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.DefaultLow, UILayoutConstraintAxis.Horizontal);
			//
			// _FieldWrapper.AddSubview(_Value);
			// _ValueStack.AddArrangedSubview(_FieldWrapper);

			_Icon.Initialize(_MainStack);
			_Title.Initialize(_ContentStack);
			_Description.Initialize(_TitleStack);
			_TitleStack.AddArrangedSubview(_ContentStack);
			_MainStack.AddArrangedSubview(_TitleStack);

			_Hint.Initialize(_ValueStack);
			_Value.Initialize(_ValueStack);
			_MainStack.AddArrangedSubview(_ValueStack);

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

			if ( _Hint.Update(sender, e) ) { return; }

			if ( _Value.Update(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}
		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Icon.UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }

			if ( _Value.UpdateParent(sender, e) ) { return; }

			if ( e.IsEqual(TableView.RowHeightProperty) ) { UpdateMinRowHeight(_MainStack); }

			else { base.CellPropertyChanged(sender, e); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateMinRowHeight(_MainStack);
			_Icon.Update();
			_Title.Update();
			_Description.Update();
			_Hint.Update();
			_Value.Update();

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
				_Hint.Enable();
				_Value.Enable();
			}
			else
			{
				_Icon.Disable();
				_Title.Disable();
				_Description.Disable();
				_Hint.Disable();
				_Value.Disable();
			}
		}


		protected override void RunDispose()
		{
			_Title.Dispose();
			_Description.Dispose();
			_Icon.Dispose();
			_TitleStack.Dispose();
			// _ValueStack.RemoveArrangedSubview(_FieldWrapper);
			// _FieldWrapper.Dispose();
			_ValueStack.Dispose();
			_Value.Dispose();
			_Hint.Dispose();

			base.RunDispose();
		}
	}
}