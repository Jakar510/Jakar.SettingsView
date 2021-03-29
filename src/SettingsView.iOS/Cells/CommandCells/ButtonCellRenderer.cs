using System;
using System.ComponentModel;
using System.Windows.Input;
using CoreText;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using Jakar.SettingsView.Shared.Misc;
using UIKit;
using WatchKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : BaseCellView
	{
		protected static readonly UIControlState[] _controlStates =
		{
			UIControlState.Normal,
			UIControlState.Highlighted,
			UIControlState.Disabled
		};

		protected internal UIColor? DefaultTextColor { get; set; }
		protected internal nfloat DefaultFontSize { get; set; }
		private ButtonCell _ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(_ButtonCell));

		protected UIButton? _Button { get; set; }
		protected UILongPressGestureRecognizer? _Recognizer { get; set; }
		protected ICommand? _Command { get; set; }
		protected ICommand? _LockClickCommand { get; set; }

		public ButtonCellView( Cell formsCell ) : base(formsCell) => SelectionStyle = UITableViewCellSelectionStyle.Default;
		protected override void SetUpContentView()
		{
			var insets = new UIEdgeInsets(SVConstants.Cell.PADDING.Top.ToNFloat(), SVConstants.Cell.PADDING.Left.ToNFloat(), SVConstants.Cell.PADDING.Bottom.ToNFloat(), SVConstants.Cell.PADDING.Right.ToNFloat());
			_Button = new UIButton(UIButtonType.RoundedRect)
					  {
						  AutoresizingMask = UIViewAutoresizing.All,
						  HorizontalAlignment = UIControlContentHorizontalAlignment.Center,
						  VerticalAlignment = UIControlContentVerticalAlignment.Center,
						  ContentEdgeInsets = insets,
						  // TitleEdgeInsets = insets
					  };

			DefaultFontSize = _Button.TitleLabel.ContentScaleFactor;
			DefaultTextColor = _Button.TitleLabel.TextColor;

			_Recognizer = new UILongPressGestureRecognizer(RunLong);
			_Button.TouchUpInside += OnClick;          // https://stackoverflow.com/a/51593238/9530917
			_Button.AddGestureRecognizer(_Recognizer); // https://stackoverflow.com/a/6179591/9530917


			ContentView.AddSubview(_Button);
			_Button.CenterXAnchor.ConstraintEqualTo(ContentView.CenterXAnchor).Active = true;
			_Button.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

			_Button.WidthAnchor.ConstraintEqualTo(ContentView.WidthAnchor).Active = true;
			_Button.HeightAnchor.ConstraintEqualTo(ContentView.HeightAnchor).Active = true;

			UpdateConstraintsIfNeeded();
			LayoutIfNeeded();
		}


		protected void OnClick( object sender, EventArgs e ) { Run(); }
		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(ButtonCell.CommandProperty, ButtonCell.CommandParameterProperty) ) { UpdateCommand(); }

			else if ( e.IsOneOf(ButtonCell.LongClickCommandProperty, ButtonCell.LongClickCommandParameterProperty) ) { UpdateLockClickCommand(); }

			else if ( e.IsEqual(ButtonCell.ButtonBackgroundColorProperty) ) { UpdateButtonColor(); }

			else if ( e.IsOneOf(TitleCellBase.TitleFontAttributesProperty, TitleCellBase.TitleFontFamilyProperty, TitleCellBase.TitleFontSizeProperty) ) { UpdateFont(); }

			else if ( e.IsEqual(TitleCellBase.TitleProperty) ) { UpdateTitle(); }
			else if ( e.IsEqual(TitleCellBase.TitleColorProperty) ) { UpdateTitleColor(); }
			else if ( e.IsEqual(TitleCellBase.TitleAlignmentProperty) ) { UpdateTitleAlignment(); }

			else { base.CellPropertyChanged(sender, e); }
		}

		protected void Run()
		{
			if ( _Command is null ) { return; }

			if ( _Command.CanExecute(_ButtonCell.CommandParameter) ) { _Command.Execute(_ButtonCell.CommandParameter); }
		}
		protected void RunLong()
		{
			if ( _LockClickCommand is null ) { return; }

			if ( _LockClickCommand.CanExecute(_ButtonCell.LongClickCommandParameter) ) { _LockClickCommand.Execute(_ButtonCell.LongClickCommandParameter); }
		}
		protected void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _ButtonCell.Command;

			if ( _Command is null ) return;
			_Command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_Command, EventArgs.Empty);
		}
		protected void UpdateLockClickCommand()
		{
			if ( _LockClickCommand != null ) { _LockClickCommand.CanExecuteChanged -= LockClickCommand_CanExecuteChanged; }

			_LockClickCommand = _ButtonCell.LongClickCommand;

			if ( _LockClickCommand is null )
				return;
			_LockClickCommand.CanExecuteChanged += LockClickCommand_CanExecuteChanged;
			LockClickCommand_CanExecuteChanged(_LockClickCommand, EventArgs.Empty);
		}


		public override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			RunLong();
			return true;
			// return base.RowLongPressed(tableView, indexPath);
		}
		public override void RowSelected( UITableView adapter, NSIndexPath position ) { Run(); }

		protected void EnableCell()
		{
			if ( _Button is null ) return;

			_Button.Enabled = true;
			_Button.Alpha = SVConstants.Cell.ENABLED_ALPHA;
		}
		protected void DisableCell()
		{
			if ( _Button is null ) return;

			_Button.Enabled = false;
			_Button.Alpha = SVConstants.Cell.DISABLED_ALPHA;
		}

		public override void UpdateCell( UITableView tableView )
		{
			UpdateCommand();
			UpdateLockClickCommand();
			UpdateTitle();
			UpdateTitleAlignment();
			base.UpdateCell(tableView);
		}
		protected void UpdateFont()
		{
			if ( _Button is null ) return;
			TitleCellBase.TitleConfiguration cfg = _ButtonCell.TitleConfig;
			UIFont font = string.IsNullOrWhiteSpace(cfg.FontFamily)
							  ? UIFont.SystemFontOfSize(_ButtonCell.TitleConfig.FontSize.ToFloat())
							  : FontUtility.CreateNativeFont(cfg.FontFamily, _ButtonCell.TitleConfig.FontSize.ToFloat(), cfg.FontAttributes);
			_Button.TitleLabel.Font = font;
		}
		protected void UpdateTitle()
		{
			if ( _Button is null ) return;
			string? s = _ButtonCell.Title;

			foreach ( UIControlState state in _controlStates ) { _Button.SetTitle(s, state); }
		}
		protected void UpdateTitleColor()
		{
			if ( _Button is null ) return;

			var color = _ButtonCell.TitleConfig.Color.ToUIColor();
			foreach ( UIControlState state in _controlStates ) { _Button.SetTitleColor(color, state); }

			foreach ( UIControlState state in _controlStates ) { _Button.SetTitleShadowColor(color, state); }

			_Button.TintColor = color;
		}

		internal bool UpdateText() => UpdateText(_ButtonCell.Title);
		internal bool UpdateText( string? s )
		{
			if ( _Button is null ) return false;
		
			_Button.SetTitle(s, UIControlState.Normal);
			_Button.SetTitle(s, UIControlState.Highlighted);
			_Button.SetTitle(s, UIControlState.Selected);
			_Button.SetTitle(s, UIControlState.Disabled);
			TitleCellBase.TitleConfiguration cfg = _ButtonCell.TitleConfig;
		
			UIFont font = string.IsNullOrWhiteSpace(cfg.FontFamily)
							  ? UIFont.SystemFontOfSize(_ButtonCell.TitleConfig.FontSize.ToFloat())
							  : FontUtility.CreateNativeFont(cfg.FontFamily, _ButtonCell.TitleConfig.FontSize.ToFloat(), cfg.FontAttributes);
			_Button.TitleLabel.Font = font;
		
			var color = cfg.Color.ToUIColor();
			_Button.SetTitleColor(color, UIControlState.Normal);
			_Button.SetTitleColor(color, UIControlState.Highlighted);
			_Button.SetTitleColor(color, UIControlState.Selected);
			_Button.SetTitleColor(color, UIControlState.Disabled);
			
			return true;
		}

		protected internal bool UpdateButtonColor()
		{
			if ( _Button is null ) return true;

			_Button.BackgroundColor = _ButtonCell.GetButtonColor().ToUIColor();
			return true;
		}

		protected void UpdateTitleAlignment()
		{
			if ( _Button is null ) return;

			_Button.TitleLabel.TextAlignment = _ButtonCell.TitleConfig.TextAlignment.ToUITextAlignment();
		}


		protected bool ShouldBeEnabled() => _Command != null && !_Command.CanExecute(_ButtonCell.CommandParameter) || _LockClickCommand != null && !_LockClickCommand.CanExecute(_ButtonCell.LongClickCommandParameter);
		protected override void UpdateIsEnabled() { SetEnabledAppearance(ShouldBeEnabled()); }

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !Cell.IsEnabled ) { return; }

			SetEnabledAppearance(ShouldBeEnabled());
		}
		private void LockClickCommand_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !Cell.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_ButtonCell.LongClickCommandParameter) ?? true);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command is not null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				if ( _LockClickCommand is not null ) { _LockClickCommand.CanExecuteChanged -= Command_CanExecuteChanged; }

				if ( _Button is not null )
				{
					_Button.TouchUpInside -= OnClick;
					if ( _Recognizer is not null ) _Button.RemoveGestureRecognizer(_Recognizer);
				}

				_Recognizer?.Dispose();
				_Recognizer = null;

				DefaultTextColor?.Dispose();
				DefaultTextColor = null;

				_Command = null;
				_LockClickCommand = null;
			}

			base.Dispose(disposing);
		}
	}
}