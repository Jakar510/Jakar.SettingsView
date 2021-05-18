using System;
using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Enumerations;
using Jakar.Api.iOS.Extensions;
using Jakar.Api.iOS.Extensions.Layout;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms;


#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	public abstract class BaseDescriptiveTitleCell<TCell> : BaseCellView<TCell> where TCell : DescriptionCellBase
	{
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


		protected BaseDescriptiveTitleCell( TCell formsCell ) : base(formsCell)
		{
			_Icon = new IconView(this, Cell);

			_TitleStack  = Stack.Title();
			_Title       = new TitleView(this);
			_Description = new DescriptionView(this);

			_Icon.Initialize(_MainStack);
			_Title.Initialize(_TitleStack);
			_Description.Initialize(_TitleStack);
			_MainStack.AddArrangedSubview(_TitleStack);
			_TitleStack.HeightOf(_MainStack, 1);
			_TitleStack.UpdateConstraintsIfNeeded();

			_Icon.Control.WidthOf(_MainStack, SvConstants.Layout.ColumnFactors.ICON);
			_TitleStack.WidthOf(_MainStack, SvConstants.Layout.ColumnFactors.TITLE_STACK);

			// _TitleStack.RightExtended(_MainStack, _Icon.Control)
			// _TitleStack.HuggingPriority(LayoutPriority.Low, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);
			// _TitleStack.CompressionPriorities(LayoutPriority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);

			this.SetContent(_MainStack);

			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _MainStack.AccessibilityIdentifier = Cell.AutomationId; }

			_MainStack.UpdateConstraintsIfNeeded();
			SetNeedsLayout();
		}


		// protected virtual void SetUpContentView()
		// {
		// 	//Outer HorizontalStackView
		// 	_MainStack = Stack.MainStack();
		//
		// 	_Icon = new IconView(this);
		//
		// 	_MainStack.AddArrangedSubview(_Icon);
		//
		// 	//VerticalStackView that is arranged at right. ( put main parts and Description ) 
		// 	_TitleStack = Stack.TitleStack();
		//
		// 	//HorizontalStackView that is arranged at upper in right.( put LabelText and ValueText)
		// 	_ContentStack = Stack.ContentStack();
		//
		// 	_Title = new TitleView(this);
		// 	_Description = new DescriptionView(this)
		// 				   {
		// 					   Lines = 0,
		// 					   LineBreakMode = UILineBreakMode.WordWrap
		// 				   };
		// 	
		// 	_ContentStack.AddArrangedSubview(_Title);
		// 	
		// 	_TitleStack.AddArrangedSubview(_ContentStack);
		// 	_TitleStack.AddArrangedSubview(_Description);
		// 	
		// 	_MainStack.AddArrangedSubview(_TitleStack);
		//
		// 	_Icon.SetContentHuggingPriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to expand.
		// 	_TitleStack.SetContentHuggingPriority(LayoutPriority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_ContentStack.SetContentHuggingPriority(LayoutPriority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_Title.SetContentHuggingPriority(LayoutPriority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_Description.SetContentHuggingPriority(LayoutPriority.LOW, UILayoutConstraintAxis.Horizontal);
		//
		//
		// 	_Icon.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 
		// 	_TitleStack.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_ContentStack.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_Title.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_Description.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Horizontal);
		//
		// 	_Icon.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Vertical);
		// 	_Icon.SetContentHuggingPriority(LayoutPriority.LOW, UILayoutConstraintAxis.Vertical);
		// 	_TitleStack.SetContentCompressionResistancePriority(LayoutPriority.HIGH, UILayoutConstraintAxis.Vertical);
		// 	_TitleStack.SetContentHuggingPriority(LayoutPriority.LOW, UILayoutConstraintAxis.Vertical);
		//
		// 	ContentView.AddSubview(_MainStack);
		//
		// 	_MainStack.TranslatesAutoresizingMaskIntoConstraints = false;
		// 	_MainStack.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
		// 	_MainStack.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
		// 	_MainStack.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
		// 	_MainStack.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
		//
		//
		// 	double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SVConstants.Defaults.MIN_ROW_HEIGHT);
		// 	_MinHeightConstraint = _MainStack.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
		// 	// fix warning-log:Unable to simultaneously satisfy constraints.
		// 	_MinHeightConstraint.Priority = LayoutPriority.HIGH; // this is superior to any other view.
		// 	_MinHeightConstraint.Active = true;
		//
		// 	if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _ContentStack.AccessibilityIdentifier = Cell.AutomationId; }
		// }


		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.Update(sender, e) ) { return; }

			if ( _Icon.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Icon.UpdateParent(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}

		public override void UpdateCell( UITableView tableView )
		{
			_Icon.Update();
			_Title.Update();
			_Description.Update();

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
			}
			else
			{
				_Icon.Disable();
				_Title.Disable();
				_Description.Disable();
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
