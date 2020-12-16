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
using ARelativeLayout = Android.Widget.RelativeLayout;

[assembly: ExportRenderer(typeof(CustomCell), typeof(CustomCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class CustomCellRenderer : CellBaseRenderer<CustomCellView> { }


	[Preserve(AllMembers = true)]
	public class CustomCellView : CellBaseView
	{
		protected CustomCell _CustomCell => Cell as CustomCell ?? throw new NullReferenceException(nameof(_CustomCell));

		protected Action? _Execute { get; set; }
		protected ICommand? _Command { get; set; }

		// protected ImageView _IndicatorView { get; set; }
		// protected LinearLayout _CoreView { get; set; }

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		private FormsViewContainer _Container { get; }

		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }


		public CustomCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.ContentCell);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.ContentCell) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.ContentCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.ContentCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.ContentCellDescription));
			_Container = ContentView.FindViewById<FormsViewContainer>(Resource.Id.ContentCellBody) ?? throw new NullReferenceException(nameof(_CellLayout));

			if ( !_CustomCell.ShowArrowIndicator ) { return; }

			// _Container = new FormsViewContainer(Context);
			if ( !_CustomCell.UseFullSize ) return;
			_Icon.Icon.RemoveFromParent();
			_Title.Label.RemoveFromParent();
			_Description.Label.RemoveFromParent();

			float rMargin = _CustomCell.ShowArrowIndicator ? AndroidContext.ToPixels(10) : 0;
			_Container.SetPadding(0, 0, (int) rMargin, 0);
			_CellLayout.SetPadding(0, 0, 0, 0);
		}
		public CustomCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


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
			if ( !_CustomCell.IsSelectable ) { return; }

			_Execute?.Invoke();
			if ( _CustomCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }
		}
		protected internal override bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( _CustomCell.LongCommand == null ) { return false; }

			_CustomCell.SendLongCommand();

			return true;
		}

		public void UpdateContent()
		{
			_Container.CustomCell = _CustomCell;
			_Container.FormsCell = _CustomCell.Content;
		}
		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateContent();
			UpdateCommand();

			_Icon.Update();
			_Title.Update();
			_Description.Update();
		}

		private void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _CustomCell.Command;

			if ( _Command != null )
			{
				_Command.CanExecuteChanged += Command_CanExecuteChanged;
				Command_CanExecuteChanged(_Command, EventArgs.Empty);
			}

			_Execute = () =>
					   {
						   if ( _Command == null ) { return; }

						   if ( _Command.CanExecute(_CustomCell.CommandParameter) ) { _Command.Execute(_CustomCell.CommandParameter); }
					   };
		}
		protected override void UpdateIsEnabled()
		{
			if ( _Command != null &&
				 !_Command.CanExecute(_CustomCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_Command.CanExecute(_CustomCell.CommandParameter));
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_Execute = null;
				_Command = null;
				// _IndicatorView?.RemoveFromParent();
				// _IndicatorView?.SetImageDrawable(null);
				// _IndicatorView?.SetImageBitmap(null);
				// _IndicatorView?.Dispose();

				// _CoreView?.RemoveFromParent();
				// _CoreView?.Dispose();

				_Icon.Dispose();
				_Title.Dispose();
				_Description.Dispose();
				_Container.RemoveFromParent();
				_Container.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}