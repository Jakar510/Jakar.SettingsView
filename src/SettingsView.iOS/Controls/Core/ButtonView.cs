using System;
using System.ComponentModel;
using System.Windows.Input;
using Jakar.Api.Extensions;
using Jakar.Api.iOS.Enumerations;
using Jakar.Api.iOS.Extensions;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Controls.Manager;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Interfaces;
using UIKit;
using Xamarin.Forms.Platform.iOS;


#nullable enable
namespace Jakar.SettingsView.iOS.Controls.Core
{
	public class ButtonView : BaseTextViewManager<UIButton, ButtonCell>
	{
		protected static readonly UIControlState[] _controlStates =
		{
			UIControlState.Normal,
			UIControlState.Highlighted,
			UIControlState.Disabled
		};

		protected override IUseConfiguration _Config => _Cell.TitleConfig;

		public ICommand? Command { get; set; }
		public ICommand? LongClickCommand { get; set; }

		protected UILongPressGestureRecognizer _Recognizer { get; set; }


		public ButtonView( ButtonCellView renderer ) : this(new UIButton(), renderer) { }

		public ButtonView( UIButton button, ButtonCellView renderer ) : base(renderer,
																			 renderer.ButtonCell,
																			 button,
																			 button.TitleLabel.TextColor,
																			 button.BackgroundColor,
																			 button.ContentScaleFactor
																			)
		{
			Control.AutoresizingMask    = UIViewAutoresizing.All;
			Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			Control.VerticalAlignment   = UIControlContentVerticalAlignment.Center;

			Control.CompressionPriorities(LayoutPriority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);

			Control.UserInteractionEnabled = Control.Enabled = true;

			_Recognizer       =  new UILongPressGestureRecognizer(RunLong);
			Control.TouchDown += OnClick;              // https://stackoverflow.com/a/51593238/9530917
			Control.AddGestureRecognizer(_Recognizer); // https://stackoverflow.com/a/6179591/9530917
		}

		public override void Initialize( UIStackView parent )
		{
			parent.AddArrangedSubview(Control);

			Control.LeftAnchor.ConstraintEqualTo(parent.LeftAnchor).Active   = true;
			Control.RightAnchor.ConstraintEqualTo(parent.RightAnchor).Active = true;

			Control.BottomAnchor.ConstraintEqualTo(parent.BottomAnchor).Active = true;
			Control.TopAnchor.ConstraintEqualTo(parent.TopAnchor).Active       = true;

			Control.UpdateConstraintsIfNeeded();
		}


		public override bool Update( object sender, PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(ButtonCell.CommandProperty, ButtonCell.CommandParameterProperty) ) { return UpdateCommand(); }

			if ( e.IsOneOf(ButtonCell.LongClickCommandProperty, ButtonCell.LongClickCommandParameterProperty) ) { return UpdateLongClickCommand(); }

			if ( e.IsEqual(ButtonCell.ButtonBackgroundColorProperty) ) { return UpdateButtonBackgroundColor(); }

			if ( e.IsOneOf(TitleCellBase.TitleFontAttributesProperty, TitleCellBase.TitleFontFamilyProperty, TitleCellBase.TitleFontSizeProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(TitleCellBase.TitleProperty) ) { return UpdateText(); }

			if ( e.IsEqual(TitleCellBase.TitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(TitleCellBase.TitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			return false;
		}

		public override bool UpdateParent( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.sv.SettingsView.CellButtonBackgroundColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.PropertyName == Shared.sv.SettingsView.CellBackgroundColorProperty.PropertyName ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleColorProperty) ) { return UpdateTextColor(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleFontSizeProperty) ) { return UpdateFont(); }

			if ( e.IsEqual(Shared.sv.SettingsView.CellTitleAlignmentProperty) ) { return UpdateTextAlignment(); }

			if ( e.IsOneOf(Shared.sv.SettingsView.CellTitleFontFamilyProperty, Shared.sv.SettingsView.CellTitleFontAttributesProperty) ) { return UpdateFont(); }

			return false;
		}


		protected void OnClick( object sender, EventArgs e ) { Run(); }
		public void Run() => Run(_Cell.CommandParameter);

		protected void Run( in object? parameter )
		{
			if ( Command is null ||
				 !_Cell.IsEnabled ) { return; }

			if ( Command.CanExecute(parameter) ) { Command.Execute(parameter); }
		}


		public void RunLong() => RunLong(_Cell.LongClickCommandParameter);

		protected void RunLong( in object? parameter )
		{
			if ( LongClickCommand is null ||
				 !_Cell.IsEnabled ) { return; }

			if ( LongClickCommand.CanExecute(parameter) ) { LongClickCommand.Execute(parameter); }
		}


		public override void Update()
		{
			base.Update();

			UpdateCommand();
			UpdateLongClickCommand();
		}

		protected bool UpdateCommand()
		{
			if ( Command != null ) { Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			Command = _Cell.Command;

			if ( Command is null ) { return true; }

			Command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(Command, EventArgs.Empty);

			return true;
		}

		protected bool UpdateLongClickCommand()
		{
			if ( LongClickCommand != null ) { LongClickCommand.CanExecuteChanged -= LockClickCommand_CanExecuteChanged; }

			LongClickCommand = _Cell.LongClickCommand;

			if ( LongClickCommand is null ) { return true; }

			LongClickCommand.CanExecuteChanged += LockClickCommand_CanExecuteChanged;

			LockClickCommand_CanExecuteChanged(LongClickCommand, EventArgs.Empty);

			return true;
		}


		public override bool UpdateFont()
		{
			IUseConfiguration cfg = _Config;

			UIFont font = string.IsNullOrWhiteSpace(cfg.FontFamily)
							  ? UIFont.SystemFontOfSize(cfg.FontSize.ToFloat())
							  : FontUtility.CreateNativeFont(cfg.FontFamily, cfg.FontSize.ToFloat(), cfg.FontAttributes);

			Control.TitleLabel.Font = font;
			Control.Font            = font;
			return true;
		}

		public override bool UpdateFontSize()
		{
			Control.TitleLabel.MinimumFontSize = _Config.FontSize.ToNFloat();

			return true;
		}

		public override bool UpdateText() => UpdateText(_Cell.Title);

		public override bool UpdateText( string? s )
		{
			foreach ( UIControlState state in _controlStates ) { Control.SetTitle(s, state); }

			return true;
		}

		public override bool UpdateTextAlignment()
		{
			Control.TitleLabel.TextAlignment = _Config.TextAlignment.ToUITextAlignment();

			return true;
		}

		public override bool UpdateTextColor()
		{
			var color = _Config.Color.ToUIColor();
			foreach ( UIControlState state in _controlStates ) { Control.SetTitleColor(color, state); }

			foreach ( UIControlState state in _controlStates ) { Control.SetTitleShadowColor(color, state); }

			Control.TintColor = color;

			return true;
		}

		public bool UpdateButtonBackgroundColor()
		{
			Control.BackgroundColor = _Cell.GetButtonColor().ToUIColor();

			return true;
		}


		protected override void UpdateIsEnabled() { SetEnabledAppearance(ShouldBeEnabled()); }

		protected bool ShouldBeEnabled()
		{
			bool click     = Command != null && !Command.CanExecute(_Cell.CommandParameter);
			bool longClick = LongClickCommand != null && !LongClickCommand.CanExecute(_Cell.LongClickCommandParameter);
			return _Cell.IsEnabled && ( click || longClick );
		}

		private void Command_CanExecuteChanged( object          sender, EventArgs e ) { UpdateIsEnabled(); }
		private void LockClickCommand_CanExecuteChanged( object sender, EventArgs e ) { UpdateIsEnabled(); }


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( Command is not null ) { Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				if ( LongClickCommand is not null ) { LongClickCommand.CanExecuteChanged -= Command_CanExecuteChanged; }

				Command          = null;
				LongClickCommand = null;

				Control.TouchDown -= OnClick;

				// TouchUpInside -= OnClick;

				Control.RemoveGestureRecognizer(_Recognizer);
				_Recognizer.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
