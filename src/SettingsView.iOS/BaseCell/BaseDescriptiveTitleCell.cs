using System;
using System.ComponentModel;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Extensions;
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
		private Stack?           _titleStack;
		private Stack?           _contentStack;
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
		protected Stack _TitleStack
		{
			get => _titleStack ?? throw new NullReferenceException(nameof(_titleStack));
			set => _titleStack = value;
		}

		/// <summary>
		/// Vertical  StackView that is wrapper for the TitleView
		/// </summary>
		protected Stack _ContentStack
		{
			get => _contentStack ?? throw new NullReferenceException(nameof(_contentStack));
			set => _contentStack = value;
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
			_Icon = new IconView(this);

			_TitleStack = Stack.TitleStack();
			_ContentStack = Stack.ContentStack();
			_Title = new TitleView(this);
			_Description = new DescriptionView(this);

			_Icon.Initialize(_MainStack);
			_Title.Initialize(_ContentStack);
			_Description.Initialize(_TitleStack);
			_TitleStack.AddArrangedSubview(_ContentStack);
			_MainStack.AddArrangedSubview(_TitleStack);

			_Icon.WidthOfParent(_MainStack, 0, SvConstants.Layout.ColumnFactors.ICON);
			_TitleStack.RightExtended(_MainStack, _Icon);

			_ContentStack.HuggingPriority(SvConstants.Layout.Priority.Minimum, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);
			_ContentStack.CompressionPriorities(SvConstants.Layout.Priority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);

			_TitleStack.HuggingPriority(SvConstants.Layout.Priority.Minimum, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);
			_TitleStack.CompressionPriorities(SvConstants.Layout.Priority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);


			_MainStack.Root(this);

			double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SvConstants.Defaults.MIN_ROW_HEIGHT);
			_MinHeightConstraint = _MainStack.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
			_MinHeightConstraint.Priority = SvConstants.Layout.Priority.Highest; //  fix warning-log:Unable to simultaneously satisfy constraints. this is superior to any other view.
			_MinHeightConstraint.Active = true;

			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _MainStack.AccessibilityIdentifier = Cell.AutomationId; }

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
		// 	_Icon.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to expand.
		// 	_TitleStack.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_ContentStack.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_Title.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_Description.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		//
		//
		// 	_Icon.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 
		// 	_TitleStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_ContentStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_Title.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_Description.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		//
		// 	_Icon.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
		// 	_Icon.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);
		// 	_TitleStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
		// 	_TitleStack.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);
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
		// 	_MinHeightConstraint.Priority = SVConstants.Layout.Priority.HIGH; // this is superior to any other view.
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

			if ( e.IsEqual(TableView.RowHeightProperty) ) { UpdateMinRowHeight(_MainStack); }

			else { base.CellPropertyChanged(sender, e); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateMinRowHeight(_MainStack);
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

				_contentStack?.Dispose();
				_contentStack = null;
			}

			base.Dispose(disposing);
		}
	}
}
