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
	public class CommandCellView : CellBaseView
	{
		protected Action? Execute { get; set; }
		protected ICommand? _Command { get; set; }
		protected CommandCell _CommandCell => Cell as CommandCell ?? throw new NullReferenceException(nameof(_CommandCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }

		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }
		protected ImageView _IndicatorView { get; set; }


		public CommandCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.CommandCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CommandCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellDescription));
			_IndicatorView = ContentView.FindViewById<ImageView>(Resource.Id.CommandCellIndicator) ?? throw new NullReferenceException(nameof(_IndicatorView));

			if ( !( CellParent?.ShowArrowIndicatorForAndroid ?? false ) ||
				 _CommandCell.HideArrowIndicator ) { return; }

			_IndicatorView.RemoveFromParent();
		}
		public CommandCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.CommandCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CommandCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellDescription));
			_IndicatorView = ContentView.FindViewById<ImageView>(Resource.Id.CommandCellIndicator) ?? throw new NullReferenceException(nameof(_IndicatorView));

			if ( !( CellParent?.ShowArrowIndicatorForAndroid ?? false ) ||
				 _CommandCell.HideArrowIndicator ) { return; }

			_IndicatorView.RemoveFromParent();
		}

		protected override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }
		}

		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
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

		protected override void UpdateCell()
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

			SetEnabledAppearance(_Command.CanExecute(_CommandCell.CommandParameter));
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

				_IndicatorView.RemoveFromParent();
				_IndicatorView.SetImageDrawable(null);
				_IndicatorView.SetImageBitmap(null);
				_IndicatorView.Dispose();

				_CellLayout.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}