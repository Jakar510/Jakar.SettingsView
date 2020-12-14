using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	[Preserve(AllMembers = true)]
	public class CheckboxCellView : CellBaseView, CompoundButton.IOnCheckedChangeListener
	{
		protected AppCompatCheckBox _Checkbox { get; set; }
		protected CheckboxCell _CheckboxCell => Cell as CheckboxCell ?? throw new NullReferenceException(nameof(_CheckboxCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }
		protected LinearLayout _AccessoryStack { get; set; }

		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }


		public CheckboxCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.AccessoryCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellLayout));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellDescription));
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.AccessoryCellStack) ?? throw new NullReferenceException(nameof(_AccessoryStack));

			_Checkbox = new AppCompatCheckBox(context)
						{
							Focusable = false,
							Gravity = GravityFlags.Right
						};
			_Checkbox.SetOnCheckedChangeListener(this);
			AddAccessory(_AccessoryStack, _Checkbox);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}
		public CheckboxCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.AccessoryCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.AccessoryCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellLayout));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.AccessoryCellDescription));
			_AccessoryStack = ContentView.FindViewById<LinearLayout>(Resource.Id.AccessoryCellStack) ?? throw new NullReferenceException(nameof(_AccessoryStack));

			_Checkbox = new AppCompatCheckBox(AndroidContext)
						{
							Focusable = false,
							Gravity = GravityFlags.Right
						};
			_Checkbox.SetOnCheckedChangeListener(this);
			AddAccessory(_AccessoryStack, _Checkbox);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		protected override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }


			if ( e.PropertyName == CheckboxCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			if ( e.PropertyName == CheckboxCell.CheckedProperty.PropertyName ) { UpdateChecked(); }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }


			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _Checkbox.Checked = !_Checkbox.Checked; }


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
		public void OnCheckedChanged( CompoundButton? buttonView, bool isChecked )
		{
			_CheckboxCell.Checked = isChecked;
			buttonView?.JumpDrawablesToCurrentState();
		}

		protected override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell();
		}

		protected void UpdateChecked() { _Checkbox.Checked = _CheckboxCell.Checked; }
		protected void UpdateAccentColor()
		{
			if ( _CheckboxCell.AccentColor != Color.Default ) { ChangeCheckColor(_CheckboxCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToAndroid()); }
		}


		protected void ChangeCheckColor( Android.Graphics.Color accent )
		{
			var colorList = new ColorStateList(new int[][]
											   {
												   new int[]
												   {
													   Android.Resource.Attribute.StateChecked
												   },
												   new int[]
												   {
													   -Android.Resource.Attribute.StateChecked
												   },
											   }, new int[]
												  {
													  accent,
													  accent
												  });

			_Checkbox.SupportButtonTintList = colorList;

			var rippleColor = new ColorStateList(new int[][]
												 {
													 new int[]
													 {
														 Android.Resource.Attribute.StateChecked
													 },
													 new int[]
													 {
														 -Android.Resource.Attribute.StateChecked
													 }
												 }, new int[]
													{
														Android.Graphics.Color.Argb(76, accent.R, accent.G, accent.B),
														Android.Graphics.Color.Argb(76, 117, 117, 117)
													});
			var ripple = _Checkbox.Background as RippleDrawable;
			ripple.SetColor(rippleColor);
			_Checkbox.Background = ripple;
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Checkbox.SetOnCheckedChangeListener(null);
				_Checkbox.Dispose();

				_Title.Dispose();
				_Description.Dispose();

				_CellLayout.Dispose();
				_AccessoryStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}