using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.Controls;
using Jakar.SettingsView.iOS.Controls.Core;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Enumerations;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


#nullable enable
namespace Jakar.SettingsView.iOS.BaseCell
{
	public abstract class BaseCellView : CellTableViewCell
	{
		/// <summary>
		/// Outer Horizontal StackView
		/// </summary>
		protected Stack _MainStack { get; }

		protected CellBase? _CellBase => Cell as CellBase;

		public Shared.sv.SettingsView? CellParent => Cell?.Parent as Shared.sv.SettingsView;

		protected NSLayoutConstraint? _MinHeightConstraint { get; set; }


		protected BaseCellView( Cell formsCell ) : base(UITableViewCellStyle.Default, formsCell.GetType().FullName)
		{
			_MainStack = Stack.MainStack();

			SelectionStyle = UITableViewCellSelectionStyle.None;
			Cell = formsCell;

			// remove existing views 
			ImageView.RemoveFromSuperview();
			ImageView.Hidden = true;

			TextLabel.RemoveFromSuperview();
			TextLabel.Hidden = true;

			UpdateSelectedColor();
		}


		public virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }

			else if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}

		public virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsEqual(Shared.sv.SettingsView.CellBackgroundColorProperty) ) { UpdateBackgroundColor(); }

			else if ( e.IsEqual(Shared.sv.SettingsView.SelectedColorProperty) ) { UpdateSelectedColor(); }
		}

		public virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }


		public virtual void RowSelected( UITableView    tableView, NSIndexPath indexPath ) { }
		public virtual bool RowLongPressed( UITableView tableView, NSIndexPath indexPath ) => false;


		public void UpdateWithForceLayout( Action updateAction )
		{
			updateAction();
			SetNeedsLayout();
		}

		public bool UpdateWithForceLayout( Func<bool> updateAction )
		{
			bool result = updateAction();
			SetNeedsLayout();
			return result;
		}


		private void UpdateSelectedColor()
		{
			if ( CellParent is null ||
				 CellParent.SelectedColor == Color.Default ) return;

			if ( SelectedBackgroundView is not null ) { SelectedBackgroundView.BackgroundColor = CellParent.SelectedColor.ToUIColor(); }
			else
			{
				SelectedBackgroundView = new UIView
										 {
											 BackgroundColor = CellParent.SelectedColor.ToUIColor()
										 };
			}
		}

		private void UpdateBackgroundColor()
		{
			if ( _CellBase is null ) return; // for HotReload
			BackgroundColor = _CellBase.GetBackground().ToUIColor();
		}

		protected virtual void UpdateIsEnabled() { SetEnabledAppearance(Cell.IsEnabled); }

		protected virtual void SetEnabledAppearance( bool isEnabled ) { UserInteractionEnabled = isEnabled; }

		// protected virtual void SetUpContentView()
		// {
		// 	SetUpHintLabel();
		//
		// 	//remove existing views 
		// 	ImageView.RemoveFromSuperview();
		// 	TextLabel.RemoveFromSuperview();
		// 	ImageView.Hidden = true;
		// 	TextLabel.Hidden = true;
		//
		// 	//Outer HorizontalStackView
		// 	_StackH = new UIStackView
		// 			  {
		// 				  Axis = UILayoutConstraintAxis.Horizontal,
		// 				  Alignment = UIStackViewAlignment.Center,
		// 				  Spacing = 16,
		// 				  Distribution = UIStackViewDistribution.Fill,
		// 				  LayoutMargins = new UIEdgeInsets(6, 16, 6, 16),
		// 				  LayoutMarginsRelativeArrangement = true
		// 			  };
		// 	//set margin
		//
		// 	IconView = new UIImageView();
		//
		// 	//round corners
		// 	IconView.ClipsToBounds = true;
		//
		// 	_StackH.AddArrangedSubview(IconView);
		//
		// 	UpdateIconSize();
		//
		// 	//VerticalStackView that is arranged at right. ( put main parts and Description ) 
		// 	_StackV = new UIStackView
		// 			  {
		// 				  Axis = UILayoutConstraintAxis.Vertical,
		// 				  Alignment = UIStackViewAlignment.Fill,
		// 				  Spacing = 4,
		// 				  Distribution = UIStackViewDistribution.Fill,
		// 			  };
		//
		// 	//HorizontalStackView that is arranged at upper in right.( put LabelText and ValueText)
		// 	_ContentStack = new UIStackView
		// 					{
		// 						Axis = UILayoutConstraintAxis.Horizontal,
		// 						Alignment = UIStackViewAlignment.Fill,
		// 						Spacing = 6,
		// 						Distribution = UIStackViewDistribution.Fill,
		// 					};
		//
		// 	TitleLabel = new UILabel();
		// 	DescriptionLabel = new UILabel
		// 					   {
		// 						   Lines = 0,
		// 						   LineBreakMode = UILineBreakMode.WordWrap
		// 					   };
		//
		//
		// 	_ContentStack.AddArrangedSubview(TitleLabel);
		//
		// 	_StackV.AddArrangedSubview(_ContentStack);
		// 	_StackV.AddArrangedSubview(DescriptionLabel);
		//
		// 	_StackH.AddArrangedSubview(_StackV);
		//
		// 	IconView.SetContentHuggingPriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to expand.
		// 	_StackV.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	_ContentStack.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	TitleLabel.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		// 	DescriptionLabel.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Horizontal);
		//
		//
		// 	IconView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal); //if possible, not to shrink. 
		// 	_StackV.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	_ContentStack.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	TitleLabel.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		// 	DescriptionLabel.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Horizontal);
		//
		// 	IconView.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
		// 	IconView.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);
		// 	_StackV.SetContentCompressionResistancePriority(SVConstants.Layout.Priority.HIGH, UILayoutConstraintAxis.Vertical);
		// 	_StackV.SetContentHuggingPriority(SVConstants.Layout.Priority.LOW, UILayoutConstraintAxis.Vertical);
		//
		// 	ContentView.AddSubview(_StackH);
		//
		// 	_StackH.TranslatesAutoresizingMaskIntoConstraints = false;
		// 	_StackH.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
		// 	_StackH.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor).Active = true;
		// 	_StackH.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
		// 	_StackH.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
		//
		//
		// 	double minHeight = Math.Max(CellParent?.RowHeight ?? -1, SVConstants.Defaults.MIN_ROW_HEIGHT);
		// 	_MinHeightConstraint = _StackH.HeightAnchor.ConstraintGreaterThanOrEqualTo(minHeight.ToNFloat());
		// 	// fix warning-log:Unable to simultaneously satisfy constraints.
		// 	_MinHeightConstraint.Priority = SVConstants.Layout.Priority.HIGH; // this is superior to any other view.
		// 	_MinHeightConstraint.Active = true;
		//
		// 	if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _ContentStack.AccessibilityIdentifier = Cell.AutomationId; }
		//
		// 	UpdateSelectedColor();
		// }

		protected void UpdateMinRowHeight( in Stack mainStack )
		{
			if ( CellParent is null ) throw new NullReferenceException(nameof(CellParent));

			if ( _MinHeightConstraint is not null )
			{
				_MinHeightConstraint.Active = false;
				_MinHeightConstraint.Dispose();
				_MinHeightConstraint = null;
			}

			if ( CellParent.HasUnevenRows )
			{
				_MinHeightConstraint = mainStack.HeightAnchor.ConstraintGreaterThanOrEqualTo(CellParent.RowHeight);
				_MinHeightConstraint.Priority = SvConstants.Layout.Priority.Highest;
				_MinHeightConstraint.Active = true;
			}

			mainStack.UpdateConstraints();
		}

		public virtual void UpdateCell( UITableView tableView )
		{
			UpdateBackgroundColor();
			UpdateIsEnabled();
			SetNeedsLayout();
		}

		protected void SetRightMarginZero( in Stack mainStack )
		{
			if ( !UIDevice.CurrentDevice.CheckSystemVersion(11, 0) ) return;
			UIEdgeInsets margins = mainStack.LayoutMargins;
			margins.Right = 0;
			mainStack.LayoutMargins = margins;
		}

		// protected virtual void SetUpContentView( in CellType cellType )
		// {
		// 	switch ( cellType )
		// 	{
		// 		// Title only
		// 		case CellType.ButtonCell:
		// 			break;
		//
		//
		// 		// Icon, Description and Title, Hint, Entry only
		// 		case CellType.EntryCell:
		// 		{
		// 			_ValueStack = Stack.ValueStack();
		// 			DescriptiveTitleSetup(_ValueStack);
		//
		// 			_Hint = new HintView(this);
		// 			_Hint.Initialize(_ValueStack);
		//
		// 			_EntryView = new AiEditText();
		// 			_EntryView.Initialize(_ValueStack);
		//
		// 			_ValueStack.Value(_MainStack, _TitleStack ?? throw new NullReferenceException(nameof(_TitleStack)));
		// 			break;
		// 		}
		//
		//
		// 		// Icon, Description and Title only
		// 		case CellType.CustomCell:
		// 		case CellType.CommandCell:
		//
		// 		// Icon, Description and Title only with accessory
		// 		case CellType.RadioCell:
		// 		case CellType.CheckboxCell:
		// 		case CellType.SwitchCell:
		// 		{
		// 			DescriptiveTitleSetup();
		// 			break;
		// 		}
		//
		//
		// 		// Icon, Description and Title, Hint, Value only
		// 		case CellType.LabelCell:
		//
		// 		//  popup handled on override
		// 		case CellType.PickerCell:
		// 		case CellType.TextPickerCell:
		// 		case CellType.NumberPickerCell:
		// 		case CellType.DatePickerCell:
		// 		case CellType.TimePickerCell:
		// 		{
		// 			_ValueStack = Stack.ValueStack();
		// 			DescriptiveTitleSetup(_ValueStack);
		//
		// 			_Hint = new HintView(this);
		// 			_Hint.Initialize(_ValueStack);
		//
		// 			_Value = new ValueView(this);
		// 			_Value.Initialize(_ValueStack);
		//
		// 			_ValueStack.Value(_MainStack, _TitleStack ?? throw new NullReferenceException(nameof(_TitleStack)));
		// 			break;
		// 		}
		//
		//
		// 		// forms cell
		// 		case CellType.EntryCell_Forms:
		// 		case CellType.ViewCell_Forms:
		// 		case CellType.ImageCell_Forms:
		// 		case CellType.SwitchCell_Forms:
		// 		case CellType.TextCell_Forms:
		// 			break;
		//
		//
		// 		// not implemented
		// 		case CellType.SpacerCell:
		// 		case CellType.EditorCell:
		// 		case CellType.IPCell:
		//
		// 		// FAILED
		// 		case CellType.Unknown:
		// 		default: throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
		// 	}
		//
		// 	UpdateSelectedColor();
		// }
		//
		// private void DescriptiveTitleSetup()
		// {
		// 	_Icon = new IconView(this)
		// 				{
		// 					ClipsToBounds = true
		// 				};
		//
		// 	_Icon.Initialize(_MainStack);
		//
		// 	_TitleStack = Stack.TitleStack();
		//
		// 	_Title = new TitleView(this);
		// 	_Title.Initialize(_TitleStack);
		//
		// 	_Description = new DescriptionView(this);
		// 	_Description.Initialize(_TitleStack);
		//
		// 	_TitleStack.Title(_MainStack, _Icon);
		// }
		// private void DescriptiveTitleSetup( in Stack value )
		// {
		// 	_Icon = new IconView(this)
		// 				{
		// 					ClipsToBounds = true
		// 				};
		//
		// 	_Icon.Initialize(_MainStack);
		//
		// 	_TitleStack = Stack.TitleStack();
		//
		// 	_Title = new TitleView(this);
		// 	_Title.Initialize(_TitleStack);
		//
		// 	_Description = new DescriptionView(this);
		// 	_Description.Initialize(_TitleStack);
		//
		// 	_TitleStack.Title(_MainStack, value, _Icon);
		// }

		// protected virtual void AccessoryCellSetup() { DescriptiveTitleSetup(); }


		protected bool _disposed;

		protected override void Dispose( bool disposing )
		{
			if ( disposing && !_disposed )
			{
				_disposed = true;

				if ( _CellBase != null )
				{
					_CellBase.PropertyChanged -= CellPropertyChanged;

					if ( _CellBase.Section is not null )
					{
						_CellBase.Section.PropertyChanged -= SectionPropertyChanged;
						_CellBase.Section = null;
					}
				}

				if ( CellParent != null ) CellParent.PropertyChanged -= ParentPropertyChanged;

				SelectedBackgroundView?.Dispose();
				SelectedBackgroundView = null;

				Cell = null;

				_MainStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}



	[Preserve(AllMembers = true)]
	public abstract class BaseCellView<TCell> : BaseCellView where TCell : CellBase
	{
		public new TCell Cell
		{
			get => base.Cell as TCell ?? throw new NullReferenceException(nameof(Cell));
			set => base.Cell = value;
		}

		protected BaseCellView( TCell formsCell ) : base(formsCell) { }
	}
}
