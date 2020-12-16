using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(RadioCell), typeof(RadioCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class RadioCellRenderer : CellBaseRenderer<RadioCellView> { }

	[Preserve(AllMembers = true)]
	public class RadioCellView : CellBaseView
	{
		protected RadioCell _RadioCell => Cell as RadioCell ?? throw new NullReferenceException(nameof(_RadioCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		protected LinearLayout _AccessoryStack { get; set; }

		private SimpleCheck _SimpleCheck { get; set; }
		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }

		private object _SelectedValue
		{
			get => RadioCell.GetSelectedValue(_RadioCell.Section) ?? RadioCell.GetSelectedValue(CellParent);
			set
			{
				if ( RadioCell.GetSelectedValue(_RadioCell.Section) != null ) { RadioCell.SetSelectedValue(_RadioCell.Section, value); }
				else { RadioCell.SetSelectedValue(CellParent, value); }
			}
		}


		public RadioCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.AccessoryCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.AccessoryCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellDescription));
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.AccessoryCellStack) ?? throw new NullReferenceException(nameof(_AccessoryStack));

			_SimpleCheck = new SimpleCheck(AndroidContext)
						   {
							   Focusable = false
						   };
			AddAccessory(_AccessoryStack, _SimpleCheck);
		}
		public RadioCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.AccessoryCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.AccessoryCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellDescription));
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.AccessoryCellStack) ?? throw new NullReferenceException(nameof(_AccessoryStack));

			_SimpleCheck = new SimpleCheck(AndroidContext)
						   {
							   Focusable = false
						   };
			AddAccessory(_AccessoryStack, _SimpleCheck);
		}


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
				_SimpleCheck.Invalidate();
			}

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName )
			{
				UpdateAccentColor();
				_SimpleCheck.Invalidate();
			}
			else if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}
		protected internal override void SectionPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.SectionPropertyChanged(sender, e);
			if ( e.PropertyName == RadioCell.SelectedValueProperty.PropertyName ) { UpdateSelectedValue(); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( !_SimpleCheck.Selected ) { _SelectedValue = _RadioCell.Value; }
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_SimpleCheck.Enabled = true;
			_SimpleCheck.Alpha = ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_SimpleCheck.Enabled = false;
			_SimpleCheck.Alpha = DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateSelectedValue();
			base.UpdateCell();
		}
		private void UpdateSelectedValue() { _SimpleCheck.Selected = _RadioCell.Value.GetType().IsValueType ? Equals(_RadioCell.Value, _SelectedValue) : ReferenceEquals(_RadioCell.Value, _SelectedValue); }
		private void UpdateAccentColor()
		{
			if ( !_RadioCell.AccentColor.IsDefault ) { _SimpleCheck.Color = _RadioCell.AccentColor.ToAndroid(); }
			else if ( CellParent != null &&
					  !CellParent.CellAccentColor.IsDefault ) { _SimpleCheck.Color = CellParent.CellAccentColor.ToAndroid(); }
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_SimpleCheck.RemoveFromParent();
				_SimpleCheck.Dispose();

				_Title.Dispose();

				_Description.Dispose();

				_CellLayout.Dispose();
				_AccessoryStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}