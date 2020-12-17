using System;
using System.ComponentModel;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CommandCell), typeof(CommandCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class CommandCellRenderer : CellBaseRenderer<CommandCellView> { }


	[Preserve(AllMembers = true)]
	public class CommandCellView : BaseDescriptionCell
	{
		protected Action? Execute { get; set; }
		protected ICommand? _Command { get; set; }
		protected CommandCell _CommandCell => Cell as CommandCell ?? throw new NullReferenceException(nameof(_CommandCell));

		protected LinearLayout _AccessoryStack { get; }
		protected ImageView _Accessory { get; set; }


		public CommandCellView( Context context, Cell cell ) : base(context, cell)
		{
			_Accessory = new ImageView(AndroidContext);
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			AddAccessory(_AccessoryStack, _Accessory);
			
			ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
			ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();

			if ( !( CellParent?.ShowArrowIndicatorForAndroid ?? false ) ||
				 _CommandCell.HideArrowIndicator ) { return; }

			_AccessoryStack.RemoveFromParent();
			_Accessory.RemoveFromParent();
		}
		public CommandCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Accessory = new ImageView(AndroidContext);
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));
			AddAccessory(_AccessoryStack, _Accessory);

			ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
			ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();

			if ( !( CellParent?.ShowArrowIndicatorForAndroid ?? false ) ||
				 _CommandCell.HideArrowIndicator )
			{ return; }

			_AccessoryStack.RemoveFromParent();
			_Accessory.RemoveFromParent();
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			Execute?.Invoke();
			if ( _CommandCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }
		}

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
		}

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateCommand();
		}
		protected void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _CommandCell.Command;

			if ( _Command != null )
			{
				_Command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_Command, EventArgs.Empty);
			}

			Execute = () =>
					  {
						  if ( _Command == null ) { return; }

						  if ( _Command.CanExecute(_CommandCell.CommandParameter) ) { _Command.Execute(_CommandCell.CommandParameter); }
					  };
		}
		protected override void UpdateIsEnabled()
		{
			if ( _Command != null &&
				 !_Command.CanExecute(_CommandCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}


		protected void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_CommandCell.CommandParameter) ?? true);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				Execute = null;
				_Command = null;

				_Title.Dispose();

				_Description.Dispose();

				_Accessory.RemoveFromParent();
				_Accessory.SetImageDrawable(null);
				_Accessory.SetImageBitmap(null);
				_Accessory.Dispose();

				_CellLayout.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}