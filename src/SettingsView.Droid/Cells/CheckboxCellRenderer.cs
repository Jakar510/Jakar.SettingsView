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
using Jakar.SettingsView.Shared.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckboxCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class CheckboxCellRenderer : CellBaseRenderer<CheckboxCellView> { }

	[Preserve(AllMembers = true)]
	public class CheckboxCellView : BaseAiAccessoryCell<AppCompatCheckBox>, CompoundButton.IOnCheckedChangeListener
	{
		protected CheckboxCell _AccessoryCell => Cell as CheckboxCell ?? throw new NullReferenceException(nameof(_AccessoryCell));


		public CheckboxCellView( Context context, Cell cell ) : base(context, cell)
		{
			_Accessory.Focusable = false;
			_Accessory.Gravity = GravityFlags.Right;
			_Accessory.SetOnCheckedChangeListener(this);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}
		public CheckboxCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			_Accessory.Focusable = false;
			_Accessory.Gravity = GravityFlags.Right;
			_Accessory.SetOnCheckedChangeListener(this);

			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == BaseCheckableCell.AccentColorProperty.PropertyName ) { UpdateAccentColor(); }

			else if ( e.PropertyName == BaseCheckableCell.CheckedProperty.PropertyName ) { UpdateChecked(); }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			if ( e.PropertyName == Shared.SettingsView.CellAccentColorProperty.PropertyName ) { UpdateAccentColor(); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { _Accessory.Checked = !_Accessory.Checked; }


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
			_AccessoryCell.Checked = isChecked;
			buttonView?.JumpDrawablesToCurrentState();
		}

		protected internal override void UpdateCell()
		{
			UpdateAccentColor();
			UpdateChecked();
			base.UpdateCell();
		}

		protected void UpdateChecked() { _Accessory.Checked = _AccessoryCell.Checked; }
		protected void UpdateAccentColor()
		{
			if ( _AccessoryCell.AccentColor != Color.Default ) { ChangeCheckColor(_AccessoryCell.AccentColor.ToAndroid()); }
			else if ( CellParent != null &&
					  CellParent.CellAccentColor != Color.Default ) { ChangeCheckColor(CellParent.CellAccentColor.ToAndroid()); }
		}


		protected void ChangeCheckColor( Android.Graphics.Color accent )
		{
			_Accessory.SupportButtonTintList = new ColorStateList(new[]
																  {
																	  new[]
																	  {
																		  Android.Resource.Attribute.StateChecked
																	  },
																	  new[]
																	  {
																		  -Android.Resource.Attribute.StateChecked
																	  },
																  },
																  new int[]
																  {
																	  accent,
																	  Android.Graphics.Color.Argb(76, 117, 117, 117)
																  }
																 );

			RippleDrawable ripple = ( _Accessory.Background as RippleDrawable ) ?? CreateRippleDrawable(accent);
			ripple.SetColor(new ColorStateList(new[]
											   {
												   new[]
												   {
													   Android.Resource.Attribute.StateChecked
												   },
												   new[]
												   {
													   -Android.Resource.Attribute.StateChecked
												   }
											   },
											   new int[]
											   {
												   accent,
												   Android.Graphics.Color.Argb(76, 117, 117, 117)
											   }
											  )
						   );
			_Accessory.Background ??= ripple;
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Accessory.SetOnCheckedChangeListener(null);
				_Accessory.Dispose();

				_Title.Dispose();
				_Description.Dispose();

				_CellLayout.Dispose();
				_AccessoryStack.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}