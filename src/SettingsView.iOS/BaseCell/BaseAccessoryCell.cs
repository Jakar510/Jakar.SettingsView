using System;
using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Enumerations;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.iOS.Interfaces;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	public abstract class BaseAccessoryCell<TCell, TAccessory> : BaseCellView<TCell> where TAccessory : UIView, IRenderAccessory
																					 where TCell : CheckableCellBase
	{
		private TAccessory?      _accessory;
		private IconView?        _icon;
		private UIStackView?     _titleStack;
		private TitleView?       _title;
		private DescriptionView? _description;


		public IUseConfiguration TitleConfiguration       => Cell.TitleConfig;
		public IUseConfiguration DescriptionConfiguration => Cell.DescriptionConfig;


		protected IconView _Icon
		{
			get => _icon ?? throw new NullReferenceException(nameof(_icon));
			set => _icon = value;
		}

		/// <summary>
		/// Vertical StackView that is arranged at right. ( put Title and Description ) 
		/// </summary>
		protected UIStackView _TitleStack
		{
			get => _titleStack ?? throw new NullReferenceException(nameof(_titleStack));
			set => _titleStack = value;
		}

		protected TitleView _Title
		{
			get => _title ?? throw new NullReferenceException(nameof(_title));
			set => _title = value;
		}

		protected DescriptionView _Description
		{
			get => _description ?? throw new NullReferenceException(nameof(_description));
			set => _description = value;
		}

		protected TAccessory _Accessory
		{
			get => _accessory ?? throw new NullReferenceException(nameof(_accessory));
			set
			{
				_accessory    = value;
				AccessoryView = EditingAccessoryView = value;
			}
		}


		protected BaseAccessoryCell( TCell formsCell ) : base(formsCell)
		{
			_Icon = new IconView(this, Cell);

			_TitleStack   = Stack.Title();
			_Title        = new TitleView(this);
			_Description  = new DescriptionView(this);

			_Accessory = InstanceCreator.Create<TAccessory>(this);

			_Icon.Initialize(_MainStack);
			_Title.Initialize(_TitleStack);
			_Description.Initialize(_TitleStack);
			_MainStack.AddArrangedSubview(_TitleStack);

			_MainStack.AddArrangedSubview(_Accessory);

			_Icon.Control.WidthOfParent(_MainStack, 0, SvConstants.Layout.ColumnFactors.ICON);
			_TitleStack.RightExtended(_MainStack, _Icon.Control);

			_TitleStack.HuggingPriority(LayoutPriority.Minimum, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);
			_TitleStack.CompressionPriorities(LayoutPriority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);


			this.SetContent(_MainStack);

			double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SvConstants.Defaults.MIN_ROW_HEIGHT);
			_MinHeightConstraint          = _MainStack.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
			_MinHeightConstraint.Priority = LayoutPriority.Highest.ToFloat(); //  fix warning-log:Unable to simultaneously satisfy constraints. this is superior to any other view.
			_MinHeightConstraint.Active   = true;

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


		protected override void Dispose( bool disposing )
		{
			if ( disposing && !_disposed )
			{
				_icon?.Dispose();
				_icon = null;

				_title?.Dispose();
				_title = null;

				_description?.Dispose();
				_title = null;

				_title?.Dispose();
				_title = null;

				_titleStack?.Dispose();
				_titleStack = null;
			}

			base.Dispose(disposing);
		}
	}
}
