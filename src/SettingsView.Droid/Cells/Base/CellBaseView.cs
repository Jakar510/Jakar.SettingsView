using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Cells.Base;
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
		protected internal ColorDrawable SelectedColor { get; set; }
		protected internal RippleDrawable Ripple { get; set; }



		protected CellBaseView( Context context, Cell cell ) : base(context)
		{
			AndroidContext = context;
			Cell = cell;

			BackgroundColor = new ColorDrawable();
			SelectedColor = new ColorDrawable(AColor.Argb(125, 180, 180, 180));

			Background = Ripple = CreateRippleDrawable();
		}
#pragma warning disable 8618 // _Cell
		protected CellBaseView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
#pragma warning restore 8618
		{
			AndroidContext = SettingsViewInit.Current;

			BackgroundColor ??= new ColorDrawable();
			SelectedColor ??= new ColorDrawable(AColor.Argb(125, 180, 180, 180));

			Ripple ??= CreateRippleDrawable();
			Background ??= Ripple;
		}


		protected static void AddAccessory( LinearLayout stack,
											AView view,
											int? width = null,
											int? height = null,
											[CallerMemberName] string caller = "" )
		{
			if ( stack is null ) throw new NullReferenceException(nameof(stack));
			try
			{
				using var layoutParams = new LinearLayout.LayoutParams(width ?? ViewGroup.LayoutParams.WrapContent, height ?? ViewGroup.LayoutParams.WrapContent);
				{
					stack.AddView(view, layoutParams);
				}
			}
			catch ( Exception e )
			{
				var temp = new StackTrace();
				System.Diagnostics.Debug.WriteLine(temp.ToString());
				throw;
			}
		}
		protected AView CreateContentView( int id, bool attach = true ) => CreateContentView(AndroidContext, this, id, attach);
		public static AView CreateContentView( Context context,
											   ViewGroup? root,
											   int id,
											   bool attach = true,
											   [CallerMemberName] string caller = "" )
		{
			Object? temp = context.GetSystemService(Context.LayoutInflaterService);
			var inflater = (LayoutInflater) ( temp ?? throw new NullReferenceException(nameof(Context.LayoutInflaterService)) );

			return inflater.Inflate(id, root, attach) ?? throw new InflateException($"ID: {id} not found. Called from {caller}");
		}
		protected RippleDrawable CreateRippleDrawable( AColor? color = null )
		{
			using var sel = new StateListDrawable();

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

			AColor rippleColor = color ?? AColor.Rgb(180, 180, 180);
			if ( CellParent != null &&
				 CellParent.SelectedColor != Color.Default )
			{ rippleColor = CellParent.SelectedColor.ToAndroid(); }

			return DrawableUtility.CreateRipple(rippleColor, sel);
		}


		protected internal virtual void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Cell.IsEnabledProperty.PropertyName ) { UpdateIsEnabled(); }
			else if ( e.PropertyName == CellBase.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
		}
		protected internal virtual void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Shared.SettingsView.SelectedColorProperty.PropertyName ) { UpdateSelectedColor(); }
		}
		protected internal virtual void SectionPropertyChanged( object sender, PropertyChangedEventArgs e ) { }

		protected internal virtual void RowSelected( SettingsViewRecyclerAdapter adapter, int position ) { }
		protected internal virtual bool RowLongPressed( SettingsViewRecyclerAdapter adapter, int position ) => false;


		protected internal virtual void UpdateCell()
		{
			UpdateIsEnabled();
			UpdateBackgroundColor();
			UpdateSelectedColor();
			Invalidate();
		}

		protected internal void UpdateWithForceLayout( Action updateAction )
		{
			updateAction();
			Invalidate();
		}
		protected internal bool UpdateWithForceLayout( Func<bool> updateAction )
		{
			bool result = updateAction();
			Invalidate();
			return result;
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
			if ( CellBase.BackgroundColor != Color.Default ) { BackgroundColor.Color = CellBase.BackgroundColor.ToAndroid(); }
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
				SelectedColor.Color = AColor.Argb(125, 180, 180, 180);
				Ripple.SetColor(DrawableUtility.GetPressedColorSelector(AColor.Rgb(180, 180, 180)));
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