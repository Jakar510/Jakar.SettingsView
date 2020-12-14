using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SwitchCell = Jakar.SettingsView.Shared.Cells.SwitchCell;
using SwitchCellRenderer = Jakar.SettingsView.Droid.Cells.SwitchCellRenderer;

[assembly: ExportRenderer(typeof(SwitchCell), typeof(SwitchCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class SwitchCellRenderer : CellBaseRenderer<SwitchCellView> { }

	[Preserve(AllMembers = true)]
	public class SwitchCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
	{
		protected SwitchCompat _Switch { get; set; }
		protected SwitchCell _SwitchCell => Cell as SwitchCell ?? throw new NullReferenceException(nameof(_SwitchCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		protected LinearLayout _AccessoryStack { get; set; }

		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }


		public SwitchCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.CommandCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CommandCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellDescription));
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CommandCellIndicator) ?? throw new NullReferenceException(nameof(_AccessoryStack));

			_Switch = new SwitchCompat(AndroidContext)
					  {
						  Gravity = GravityFlags.Right,
						  Focusable = false
					  };
			AddAccessory(_AccessoryStack, _Switch);

			_Switch.SetOnCheckedChangeListener(this);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		public SwitchCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.CommandCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.CommandCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.CommandCellDescription));
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.CommandCellIndicator) ?? throw new NullReferenceException(nameof(_AccessoryStack));

			_Switch = new SwitchCompat(AndroidContext)
					  {
						  Gravity = GravityFlags.Right,
						  Focusable = false
					  };
			AddAccessory(_AccessoryStack, _Switch);

			_Switch.SetOnCheckedChangeListener(this);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		protected override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateOn();
			base.UpdateCell();
		}

		protected override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == SwitchCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == SwitchCell.OnProperty.PropertyName ) { UpdateOn(); }
		}
		protected override void ParentPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}


		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _Switch.Checked = !_Switch.Checked; }

		public void OnCheckedChanged( CompoundButton? buttonView, bool isChecked ) { _SwitchCell.On = isChecked; }

		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Switch.Enabled = true;
			_Switch.Alpha = ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Switch.Enabled = false;
			_Switch.Alpha = DISABLED_ALPHA;
		}

		private void UpdateOn() { _Switch.Checked = _SwitchCell.On; }

		private void UpdateAccentColor()
		{
			if ( _SwitchCell.AccentColor != Color.Default ) { ChangeSwitchColor(_SwitchCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeSwitchColor(CellParent.CellAccentColor.ToAndroid()); }
		}

		private void ChangeSwitchColor( Android.Graphics.Color accent )
		{
			var trackColors = new ColorStateList(new[]
												 {
													 new[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 },
												 }, new int[]
													{
														Android.Graphics.Color.Argb(76, accent.R, accent.G, accent.B),
														Android.Graphics.Color.Argb(76, 117, 117, 117)
													});


			_Switch.TrackDrawable.SetTintList(trackColors);

			var thumbColors = new ColorStateList(new[]
												 {
													 new[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 },
												 }, new int[]
													{
														accent,
														Android.Graphics.Color.Argb(255, 244, 244, 244)
													});

			_Switch.ThumbDrawable.SetTintList(thumbColors);

			var ripple = _Switch.Background as RippleDrawable;
			Ripple.SetColor(trackColors);
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Switch.SetOnCheckedChangeListener(null);
				_Switch.Background?.Dispose();
				_Switch.Background = null;
				_Switch.ThumbDrawable?.Dispose();
				_Switch.ThumbDrawable = null;
				_Switch.Dispose();

				_CellLayout.Dispose();
				_AccessoryStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}