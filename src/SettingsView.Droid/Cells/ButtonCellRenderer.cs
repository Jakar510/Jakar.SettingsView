using System;
using System.ComponentModel;
using System.Windows.Input;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Jakar.SettingsView.Droid.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AButton = Android.Widget.Button;
using AColor = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] internal class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : CellBaseView
	{
		protected internal AColor DefaultTextColor { get; set; }
		protected internal float DefaultFontSize { get; set; }
		private ButtonCell _ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(_ButtonCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		protected AButton _Button { get; set; }

		internal Action? Execute { get; set; }
		protected ICommand? _Command { get; set; }

		public ButtonCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.ButtonCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.ButtonCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Button = ContentView.FindViewById<AButton>(Resource.Id.ButtonCellButton) ?? throw new NullReferenceException(nameof(_Button));
		}
		public ButtonCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.ButtonCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.ButtonCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Button = ContentView.FindViewById<AButton>(Resource.Id.ButtonCellButton) ?? throw new NullReferenceException(nameof(_Button));
		}


		protected override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == ButtonCell.CommandProperty.PropertyName ||
				 e.PropertyName == ButtonCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
			else if ( e.PropertyName == ButtonCell.TitleAlignmentProperty.PropertyName ) { UpdateTitleAlignment(); }
			else if ( e.PropertyName == ButtonCell.TitleAlignmentProperty.PropertyName ) { UpdateTitleAlignment(); }
		}
		protected override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e ) { }
		protected override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { Execute?.Invoke(); }

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

		protected override void UpdateCell()
		{
			UpdateCommand();
			UpdateTitle();
			UpdateTitleAlignment();
			base.UpdateCell();
		}
		protected void UpdateTitle() { _Button.Text = _ButtonCell.Title; }
		protected void UpdateTitleAlignment() { _Button.Gravity = _ButtonCell.TitleAlignment.ToGravityFlags(); }

		private void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _ButtonCell.Command;

			if ( _Command != null )
			{
				_Command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_Command, EventArgs.Empty);
			}

			Execute = Run;
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

				Execute = null;
				_Command = null;

				_Button.Dispose();
				_CellLayout.Dispose(); 
			}

			base.Dispose(disposing);
		}
	}
}