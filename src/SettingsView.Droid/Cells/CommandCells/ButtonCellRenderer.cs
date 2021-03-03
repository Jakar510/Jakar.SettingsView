using System;
using System.ComponentModel;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Extensions;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AButton = Android.Widget.Button;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;


#nullable enable
[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] internal class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : BaseCell.BaseCellView, AView.IOnLongClickListener, AView.IOnClickListener
	{
		protected internal AColor DefaultTextColor { get; }
		protected internal float DefaultFontSize { get; }
		private ButtonCell _ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(_ButtonCell));

		protected AButton _Button { get; set; }
		protected ICommand? _Command { get; set; }

		public ButtonCellView( Context context, Cell cell ) : base(context, cell)
		{
			_Button = new AButton(AndroidContext);
			_Button.SetOnClickListener(this);
			_Button.SetOnLongClickListener(this);
			DefaultFontSize = _Button.TextSize;
			DefaultTextColor = new AColor(_Button.CurrentTextColor);
			
			this.Add(_Button,
					 0,
					 0,
					 GridSpec.Fill,
					 GridSpec.Fill,
					 Extensions.Layout.Match,
					 Extensions.Layout.Match
					);
		}
		public ButtonCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == ButtonCell.CommandProperty.PropertyName ||
				 e.PropertyName == ButtonCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }

			else if ( e.PropertyName == CellBaseTitle.TitleProperty.PropertyName ) { UpdateTitle(); }

			else if ( e.PropertyName == CellBaseTitle.TitleFontAttributesProperty.PropertyName ||
					  e.PropertyName == CellBaseTitle.TitleFontFamilyProperty.PropertyName ) { UpdateFont(); }

			else if ( e.PropertyName == CellBaseTitle.TitleFontSizeProperty.PropertyName ) { UpdateFontSize(); }

			else if ( e.PropertyName == CellBaseTitle.TitleColorProperty.PropertyName ) { UpdateColor(); }

			else if ( e.PropertyName == ButtonCell.TitleAlignmentProperty.PropertyName ) { UpdateTitleAlignment(); }
		}

		public bool OnLongClick( AView? v ) => true;
		public void OnClick( AView? v ) { Run(); }

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { Run(); }

		protected override void EnableCell()
		{
			base.EnableCell();
			_Button.Alpha = ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Button.Alpha = DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateCommand();
			UpdateTitle();
			UpdateFont();
			UpdateFontSize();
			UpdateColor();
			UpdateTitleAlignment();
			base.UpdateCell();
		}
		protected void UpdateTitle() { _Button.Text = _ButtonCell.Title; }
		protected void UpdateFontSize()
		{
			if ( _ButtonCell.TitleFontSize > 0 ) { _Button.SetTextSize(ComplexUnitType.Sp, (float) _ButtonCell.TitleFontSize); }
			else if ( _ButtonCell.Parent != null ) { _Button.SetTextSize(ComplexUnitType.Sp, (float) _ButtonCell.Parent.CellValueTextFontSize); }
			else { _Button.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); }
		}
		protected void UpdateFont()
		{
			string? family = _ButtonCell.TitleFontFamily ?? _ButtonCell.Parent?.CellValueTextFontFamily;
			FontAttributes attr = _ButtonCell.TitleFontAttributes ?? _ButtonCell.Parent?.CellValueTextFontAttributes ?? FontAttributes.None;

			_Button.Typeface = FontUtility.CreateTypeface(family, attr);
		}
		protected internal bool UpdateColor()
		{
			if ( _ButtonCell.TitleColor != Color.Default ) { _Button.SetTextColor(_ButtonCell.TitleColor.ToAndroid()); }
			else if ( _ButtonCell.Parent != null &&
					  _ButtonCell.Parent.CellValueTextColor != Color.Default ) { _Button.SetTextColor(_ButtonCell.Parent.CellValueTextColor.ToAndroid()); }
			else { _Button.SetTextColor(DefaultTextColor); }

			return true;
		}
		protected void UpdateTitleAlignment() { _Button.TextAlignment = _ButtonCell.TitleAlignment.ToAndroidTextAlignment(); }

		private void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _ButtonCell.Command;

			if ( _Command is null ) return;
			_Command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_Command, EventArgs.Empty);
		}
		protected void Run()
		{
			if ( _Command == null ) { return; }

			if ( _Command.CanExecute(_ButtonCell.CommandParameter) ) { _Command.Execute(_ButtonCell.CommandParameter); }
		}

		protected override void UpdateIsEnabled()
		{
			if ( _Command != null &&
				 !_Command.CanExecute(_ButtonCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_ButtonCell.CommandParameter) ?? true);
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_Command = null;

				_Button.Dispose();
				// _CellLayout.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}