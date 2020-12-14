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
		protected Action? _Execute { get; set; }
		protected CustomCell _CustomCell => Cell as CustomCell ?? throw new NullReferenceException(nameof(_CustomCell));
		protected ICommand? _Command { get; set; }
		protected ImageView _IndicatorView { get; set; }
		protected LinearLayout _CoreView { get; set; }
		private FormsViewContainer _Container { get; set; }

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		protected LinearLayout _AccessoryStack { get; set; }

		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }
		protected HintView _HintView { get; set; }


		public CustomCellView( Context context, Cell cell ) : base(context, cell)
		{
			if ( !_CustomCell.ShowArrowIndicator ) { return; }

			_IndicatorView = new ImageView(context);
			_IndicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);

			using var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			{
				_AccessoryStack.AddView(_IndicatorView, param);
			}

			_Container = new FormsViewContainer(Context);


			_CoreView = FindViewById<LinearLayout>(Resource.Id.ContentCellBody);

			if ( _CustomCell.UseFullSize )
			{
				_Icon.RemoveFromParent();
				var layout = FindViewById<ARelativeLayout>(Resource.Id.CellLayout);
				float rMargin = _CustomCell.ShowArrowIndicator ? AndroidContext.ToPixels(10) : 0;
				layout.SetPadding(0, 0, (int) rMargin, 0);
				_CoreView.SetPadding(0, 0, 0, 0);
			}

			_CoreView.AddView(_Container);
		}
		public CustomCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		
		protected override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == CommandCell.CommandProperty.PropertyName ||
				 e.PropertyName == CommandCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
		}
		protected override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e ) { }


		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( !_CustomCell.IsSelectable ) { return; }

			_Execute?.Invoke();
			if ( _CustomCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }
		}
		protected override bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position )
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
		protected override void UpdateCell()
		{
			base.UpdateCell();
			UpdateContent();
			UpdateCommand();
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_Execute = null;
				_Command = null;
				_IndicatorView?.RemoveFromParent();
				_IndicatorView?.SetImageDrawable(null);
				_IndicatorView?.SetImageBitmap(null);
				_IndicatorView?.Dispose();

				_CoreView?.RemoveFromParent();
				_CoreView?.Dispose();

				_Container?.RemoveFromParent();
				_Container?.Dispose();
			}

			base.Dispose(disposing);
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
	}
}