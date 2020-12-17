﻿using System;
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
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] internal class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : CellBaseView, View.IOnLongClickListener, View.IOnClickListener
	{
		// androidx.appcompat.widget.AppCompatButton
		protected internal AColor DefaultTextColor { get; }
		protected internal float DefaultFontSize { get; }
		private ButtonCell _ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(_ButtonCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		protected AButton _Button { get; set; }

		protected LinearLayout _AccessoryStack { get; }
		protected ICommand? _Command { get; set; }

		public ButtonCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.CellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.CellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			// _CellLayout.UseDefaultMargins = true;

			ContentView.FindViewById<ImageView>(Resource.Id.CellIcon)?.RemoveFromParent();
			ContentView.FindViewById<TitleView>(Resource.Id.CellTitle)?.RemoveFromParent();
			ContentView.FindViewById<DescriptionView>(Resource.Id.CellDescription)?.RemoveFromParent();
			ContentView.FindViewById<HintView>(Resource.Id.CellHint)?.RemoveFromParent();
			ContentView.FindViewById<LinearLayout>(Resource.Id.CellValueStack)?.RemoveFromParent();
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CellAccessoryStack) ?? throw new NullReferenceException(nameof(Resource.Id.CellValueStack));

			_Button = new AButton(AndroidContext);
			_Button.SetOnClickListener(this);
			_Button.SetOnLongClickListener(this);
			DefaultFontSize = _Button.TextSize;
			DefaultTextColor = new AColor(_Button.CurrentTextColor);


			_AccessoryStack.RemoveFromParent();
			var parameters = new LayoutParams()
							 {
								 ColumnSpec = InvokeSpec(0, Center),
								 RowSpec = InvokeSpec(0, Center),
								 Width = ViewGroup.LayoutParams.MatchParent,
								 Height = ViewGroup.LayoutParams.MatchParent,
								 BottomMargin = 4,
								 TopMargin = 4,
								 LeftMargin = 10,
								 RightMargin = 10,
							 };
			_CellLayout.AddView(_AccessoryStack, parameters);
			// _AccessoryStack.LayoutParameters = parameters;
			AddAccessory(_AccessoryStack, _Button, ViewGroup.LayoutParams.MatchParent);
		}
		public ButtonCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == ButtonCell.CommandProperty.PropertyName ||
				 e.PropertyName == ButtonCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }
			else if ( e.PropertyName == CellBase.TitleProperty.PropertyName ) { UpdateTitle(); }
			else if ( e.PropertyName == ButtonCell.TitleAlignmentProperty.PropertyName ) { UpdateTitleAlignment(); }
		}
		// protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e ) { base.ParentPropertyChanged(sender, e); }
		// protected internal override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

		public bool OnLongClick( View? v ) => true;
		public void OnClick( View? v ) { Run(); }

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
			UpdateTitleAlignment();
			base.UpdateCell();
		}
		protected void UpdateTitle() { _Button.Text = _ButtonCell.Title; }
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
				_CellLayout.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}