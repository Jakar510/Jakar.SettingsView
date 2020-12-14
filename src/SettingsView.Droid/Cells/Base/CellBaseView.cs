using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AGridLayout = Android.Widget.GridLayout;
using AColor = Android.Graphics.Color;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;
using Object = Java.Lang.Object;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells.Base
{
	[Preserve(AllMembers = true)]
	public abstract class CellBaseView : AGridLayout, INativeElementView
	{
		protected internal const float DISABLED_ALPHA = 0.3f;
		protected internal const float ENABLED_ALPHA = 1.0f;

		protected internal Cell Cell { get; set; }
		public Element Element => Cell;
		protected internal CellBase CellBase => Cell as CellBase ?? throw new NullReferenceException(nameof(CellBase));
		protected internal Shared.SettingsView? CellParent => Cell.Parent as Shared.SettingsView; // ?? throw new NullReferenceException(nameof(_CellParent));


		protected internal Context AndroidContext { get; set; }


		protected internal ColorDrawable BackgroundColor { get; set; }
		protected internal RippleDrawable Ripple { get; set; }

		protected internal ColorDrawable SelectedColor { get; set; }


		protected CellBaseView( Context context, Cell cell ) : base(context)
		{
			AndroidContext = context;
			Cell = cell;

			BackgroundColor = new ColorDrawable();
			SelectedColor = new ColorDrawable(AColor.Argb(125, 180, 180, 180));

			var sel = new StateListDrawable();

			sel.AddState(new[]
						 {
							 Android.Resource.Attribute.StateSelected
						 }, SelectedColor);
			sel.AddState(new[]
						 {
							 -Android.Resource.Attribute.StateSelected
						 }, BackgroundColor);
			sel.SetExitFadeDuration(250);
			sel.SetEnterFadeDuration(250);

			AColor rippleColor = AColor.Rgb(180, 180, 180);
			if ( CellParent != null &&
				 CellParent.SelectedColor != Color.Default ) { rippleColor = CellParent.SelectedColor.ToAndroid(); }

			Ripple = DrawableUtility.CreateRipple(rippleColor, sel);
			Background = Ripple;
		}
#pragma warning disable 8618 // _Cell
		protected CellBaseView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
#pragma warning restore 8618
		{
			AndroidContext = SettingsViewInit.Current;

			BackgroundColor = new ColorDrawable();
			SelectedColor = new ColorDrawable(AColor.Argb(125, 180, 180, 180));

			var sel = new StateListDrawable();

			sel.AddState(new[]
						 {
							 Android.Resource.Attribute.StateSelected
						 }, SelectedColor);
			sel.AddState(new[]
						 {
							 -Android.Resource.Attribute.StateSelected
						 }, BackgroundColor);
			sel.SetExitFadeDuration(250);
			sel.SetEnterFadeDuration(250);

			AColor rippleColor = AColor.Rgb(180, 180, 180);
			if ( CellParent != null &&
				 CellParent.SelectedColor != Color.Default ) { rippleColor = CellParent.SelectedColor.ToAndroid(); }

			Background = Ripple = DrawableUtility.CreateRipple(rippleColor, sel);
		}

		protected static void AddAccessory( LinearLayout stack, AView view, int? width = null, int? height = null )
		{
			if ( stack is null ) throw new NullReferenceException(nameof(stack));
			using var layoutParams = new LinearLayout.LayoutParams(width ?? ViewGroup.LayoutParams.WrapContent, height ?? ViewGroup.LayoutParams.WrapContent);
			{
				stack.AddView(view, layoutParams);
			}
		}
		protected AView CreateContentView( int id )
		{
			Object? temp = AndroidContext.GetSystemService(Context.LayoutInflaterService);
			var inflater = (LayoutInflater) ( temp ?? throw new InflateException(id.ToString()) );

			return inflater.Inflate(id, this, true) ?? throw new InflateException(id.ToString());
		}


		protected virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
		}
		protected abstract void ParentPropertyChanged( object sender, PropertyChangedEventArgs e );
		protected virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

		protected virtual void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { }
		protected virtual bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position ) => false;


		protected virtual void UpdateCell()
		{
			UpdateIsEnabled();
			UpdateBackgroundColor();
			UpdateSelectedColor();
			// UpdateValue();
			Invalidate();
		}

		protected internal void UpdateWithForceLayout( Action updateAction )
		{
			updateAction();
			Invalidate();
		}


	#region Enable/Disable

		protected void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { EnableCell(); }
			else { DisableCell(); }
		}
		protected virtual void EnableCell()
		{
			// not to invoke a ripple effect and not to selected
			Focusable = false;
			DescendantFocusability = DescendantFocusability.AfterDescendants;
		}
		protected virtual void DisableCell()
		{
			// not to invoke a ripple effect and not to selected
			Focusable = true;
			DescendantFocusability = DescendantFocusability.BlockDescendants;
		}


		protected virtual void UpdateIsEnabled() { SetEnabledAppearance(CellBase.IsEnabled); }

	#endregion Enable/Disable

	#region Selectable

		protected void UpdateBackgroundColor()
		{
			if ( CellBase != null &&
				 CellBase.BackgroundColor != Color.Default ) { BackgroundColor.Color = CellBase.BackgroundColor.ToAndroid(); }
			else if ( CellParent != null &&
					  CellParent.CellBackgroundColor != Color.Default ) { BackgroundColor.Color = CellParent.CellBackgroundColor.ToAndroid(); }
			else { BackgroundColor.Color = AColor.Transparent; }
		}
		protected void UpdateSelectedColor()
		{
			if ( CellParent != null &&
				 CellParent.SelectedColor != Color.Default )
			{
				SelectedColor.Color = CellParent.SelectedColor.MultiplyAlpha(0.5).ToAndroid();
				Ripple.SetColor(DrawableUtility.GetPressedColorSelector(CellParent.SelectedColor.ToAndroid()));
			}
			else
			{
				SelectedColor.Color = Android.Graphics.Color.Argb(125, 180, 180, 180);
				Ripple.SetColor(DrawableUtility.GetPressedColorSelector(Android.Graphics.Color.Rgb(180, 180, 180)));
			}
		}

		protected virtual void UpdateSelected( object sender, PropertyChangedEventArgs e ) { }

	#endregion Selectable


		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				CellBase.PropertyChanged -= CellPropertyChanged;
				if ( CellParent != null ) CellParent.PropertyChanged -= ParentPropertyChanged;

				if ( CellBase.Section != null )
				{
					CellBase.Section.PropertyChanged -= SectionPropertyChanged;
					CellBase.Section = null;
				}
				
				BackgroundColor.Dispose();
				SelectedColor.Dispose();
				Ripple.Dispose();

				Background?.Dispose();
				Background = null;
			}

			base.Dispose(disposing);
		}
	}
}