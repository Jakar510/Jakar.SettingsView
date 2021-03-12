using System;
using System.ComponentModel;
using System.Windows.Input;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared.CellBase;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Config;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
[assembly: ExportRenderer(typeof(ButtonCell), typeof(ButtonCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] internal class ButtonCellRenderer : CellBaseRenderer<ButtonCellView> { }

	[Preserve(AllMembers = true)]
	public class ButtonCellView : BaseCellView // IBorderVisualElementRenderer
	{
		protected internal UIColor? DefaultTextColor { get; }
		protected internal nfloat DefaultFontSize { get; }
		private ButtonCell _ButtonCell => Cell as ButtonCell ?? throw new NullReferenceException(nameof(_ButtonCell));

		protected UIButton _Button { get; set; }
		protected ICommand? _Command { get; set; }
		protected ICommand? _LockClickCommand { get; set; }

		public ButtonCellView( Cell cell ) : base(cell)
		{
			_Button = new UIButton()
					  {
						  HorizontalAlignment = UIControlContentHorizontalAlignment.Fill, 
						  VerticalAlignment = UIControlContentVerticalAlignment.Fill
					  };
			DefaultFontSize = _Button.TitleLabel.ContentScaleFactor;
			DefaultTextColor = _Button.TitleLabel.TextColor;
			_ContentView.AddSubview(_Button);
		}


		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == ButtonCell.CommandProperty.PropertyName ||
				 e.PropertyName == ButtonCell.CommandParameterProperty.PropertyName ) { UpdateCommand(); }

			else if ( e.PropertyName == ButtonCell.LongClickCommandProperty.PropertyName ||
					  e.PropertyName == ButtonCell.LongClickCommandParameterProperty.PropertyName ) { UpdateLockClickCommand(); }

			else if ( e.PropertyName == ButtonCell.ButtonBackgroundColorProperty.PropertyName ) { UpdateButtonColor(); }

			else if ( e.PropertyName == TitleCellBase.TitleProperty.PropertyName ) { UpdateTitle(); }

			else if ( e.PropertyName == TitleCellBase.TitleFontAttributesProperty.PropertyName ||
					  e.PropertyName == TitleCellBase.TitleFontFamilyProperty.PropertyName ) { UpdateFont(); }

			else if ( e.PropertyName == TitleCellBase.TitleFontSizeProperty.PropertyName ) { UpdateFontSize(); }

			else if ( e.PropertyName == TitleCellBase.TitleColorProperty.PropertyName ) { UpdateColor(); }

			else if ( e.PropertyName == TitleCellBase.TitleAlignmentProperty.PropertyName ) { UpdateTitleAlignment(); }
		}

		public bool OnLongClick( View? v )
		{
			RunLong();
			return true;
		}
		public void OnClick( View? v ) { Run(); }
		protected void Run()
		{
			if ( _Command is null ) { return; }

			if ( _Command.CanExecute(_ButtonCell.CommandParameter) ) { _Command.Execute(_ButtonCell.CommandParameter); }
		}
		protected void RunLong()
		{
			if ( _LockClickCommand is null ) { return; }

			if ( _LockClickCommand.CanExecute(_ButtonCell.LongClickCommandParameter) ) { _LockClickCommand.Execute(_ButtonCell.LongClickCommandParameter); }
		}
		private void UpdateCommand()
		{
			if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

			_Command = _ButtonCell.Command;

			if ( _Command is null )
				return;
			_Command.CanExecuteChanged += Command_CanExecuteChanged;
			Command_CanExecuteChanged(_Command, EventArgs.Empty);
		}
		private void UpdateLockClickCommand()
		{
			if ( _LockClickCommand != null ) { _LockClickCommand.CanExecuteChanged -= LockClickCommand_CanExecuteChanged; }

			_LockClickCommand = _ButtonCell.LongClickCommand;

			if ( _LockClickCommand is null )
				return;
			_LockClickCommand.CanExecuteChanged += LockClickCommand_CanExecuteChanged;
			LockClickCommand_CanExecuteChanged(_LockClickCommand, EventArgs.Empty);
		}


		protected internal override bool RowLongPressed( UITableView tableView, NSIndexPath indexPath )
		{
			RunLong();
			return base.RowLongPressed(tableView, indexPath);
		}
		protected internal override void RowSelected( UITableView adapter, NSIndexPath position ) { Run(); }

		protected override void EnableCell()
		{
			base.EnableCell();
			_Button.Enabled = true;
			_Button.Alpha = SVConstants.Cell.ENABLED_ALPHA;
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Button.Enabled = false;
			_Button.Alpha = SVConstants.Cell.DISABLED_ALPHA;
		}

		protected internal override void UpdateCell()
		{
			UpdateCommand();
			UpdateLockClickCommand();
			UpdateTitle();
			UpdateFont();
			UpdateFontSize();
			UpdateColor();
			UpdateTitleAlignment();
			base.UpdateCell();
		}
		protected void UpdateTitle() { _Button.TitleLabel.Text = _ButtonCell.Title; }
		protected void UpdateFontSize()
		{
			_Button.TitleLabel.MinimumFontSize = (nfloat) _ButtonCell.TitleConfig.FontSize;
			// _Button.SetTextSize(ComplexUnitType.Sp, DefaultFontSize); 
		}
		protected void UpdateFont()
		{
			string? family = _ButtonCell.TitleConfig.FontFamily;
			FontAttributes attr = _ButtonCell.TitleConfig.FontAttributes;

			_Button.TitleLabel.Font = FontUtility.CreateNativeFont(family, (float) _ButtonCell.TitleConfig.FontSize, attr);
		}
		protected internal bool UpdateColor()
		{
			_Button.TitleLabel.TextColor = _ButtonCell.TitleConfig.Color.ToUIColor();
			// _Button.SetTextColor(DefaultTextColor); 

			return true;
		}
		protected internal bool UpdateButtonColor()
		{
			_Button.TitleLabel.BackgroundColor = _ButtonCell.GetButtonColor().ToUIColor();
			// AColor color = _ButtonCell.GetButtonColor().ToAndroid();
			// BackgroundColor.Color = color;
			// _Button.Background = CreateRippleDrawable(color);
			return true;
		}
		protected void UpdateTitleAlignment() { _Button.TitleLabel.TextAlignment = _ButtonCell.TitleConfig.TextAlignment.ToUITextAlignment(); }


		protected override void UpdateIsEnabled()
		{
			if ( _Command != null &&
				 !_Command.CanExecute(_ButtonCell.CommandParameter) ) { return; }

			base.UpdateIsEnabled();
		}

		private void Command_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !Cell.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_ButtonCell.CommandParameter) ?? true);
		}
		private void LockClickCommand_CanExecuteChanged( object sender, EventArgs e )
		{
			if ( !Cell.IsEnabled ) { return; }

			SetEnabledAppearance(_Command?.CanExecute(_ButtonCell.LongClickCommandParameter) ?? true);
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( _Command != null ) { _Command.CanExecuteChanged -= Command_CanExecuteChanged; }

				_Command = null;
				_LockClickCommand = null;

				_Button.Dispose();
			}

			base.Dispose(disposing);
		}

		// --------------------- IBorderVisualElementRenderer ---------------------
		// public bool UseDefaultPadding() => true;
		// public bool UseDefaultShadow() => true;
		// public bool IsShadowEnabled() => false;
		// public float ShadowRadius { get; } = 1;
		// public float ShadowDx { get; } = 1;
		// public float ShadowDy { get; } = 1;
		// public AColor ShadowColor { get; } = Color.Accent.ToAndroid();
		// public VisualElement Element { get; }
		// public AView View { get; }
		// public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
	}


	// internal class BorderDrawable : Drawable
	// {
	// 	public const int DefaultCornerRadius = 2; // Default value for Android material button.
	//
	// 	private readonly Func<double, float> _convertToPixels;
	// 	private bool _isDisposed;
	// 	private Bitmap? _normalBitmap;
	// 	private bool _pressed;
	// 	private Bitmap? _pressedBitmap;
	// 	private float _paddingLeft;
	// 	private float _paddingTop;
	// 	private readonly Color _defaultColor;
	// 	private readonly bool _drawOutlineWithBackground;
	// 	private AColor _shadowColor;
	// 	private float _shadowDx;
	// 	private float _shadowDy;
	// 	private float _shadowRadius;
	//
	// 	private float _PaddingLeft
	// 	{
	// 		get => ( _paddingLeft / 2f ) + _shadowDx;
	// 		set => _paddingLeft = value;
	// 	}
	//
	// 	private float _PaddingTop
	// 	{
	// 		get => ( _paddingTop / 2f ) + _shadowDy;
	// 		set => _paddingTop = value;
	// 	}
	//
	// 	public BorderDrawable( Func<double, float> convertToPixels, Color defaultColor, bool drawOutlineWithBackground )
	// 	{
	// 		_convertToPixels = convertToPixels;
	// 		_pressed = false;
	// 		_defaultColor = defaultColor;
	// 		_drawOutlineWithBackground = drawOutlineWithBackground;
	// 	}
	//
	// 	public IBorderElement? BorderElement { get; set; }
	//
	// 	public override bool IsStateful => true;
	//
	// 	public override int Opacity => 0;
	//
	// 	public override void Draw( Canvas canvas )
	// 	{
	// 		//Bounds = new Rect(Bounds.Left, Bounds.Top, Bounds.Right + (int)_convertToPixels(10), Bounds.Bottom + (int)_convertToPixels(10));
	// 		int width = Bounds.Width();
	// 		int height = Bounds.Height();
	//
	// 		if ( width <= 0 ||
	// 			 height <= 0 )
	// 			return;
	//
	// 		if ( _normalBitmap == null ||
	// 			 ( _normalBitmap?.IsDisposed() ?? true ) ||
	// 			 ( _pressedBitmap?.IsDisposed() ?? true ) ||
	// 			 _normalBitmap.Height != height ||
	// 			 _normalBitmap.Width != width )
	// 			Reset();
	//
	// 		if ( BorderElement != null &&
	// 			 !_drawOutlineWithBackground &&
	// 			 BorderElement.BackgroundColor == Color.Default ) return;
	//
	// 		Bitmap bitmap;
	// 		if ( GetState().Contains(Android.Resource.Attribute.StatePressed) )
	// 		{
	// 			_pressedBitmap ??= CreateBitmap(true, width, height);
	// 			bitmap = _pressedBitmap;
	// 		}
	// 		else
	// 		{
	// 			_normalBitmap ??= CreateBitmap(false, width, height);
	// 			bitmap = _normalBitmap;
	// 		}
	//
	// 		canvas.DrawBitmap(bitmap, 0, 0, new Paint());
	// 	}
	//
	// 	public BorderDrawable SetShadow( float dy,
	// 									 float dx,
	// 									 AColor color,
	// 									 float radius )
	// 	{
	// 		_shadowDx = dx;
	// 		_shadowDy = dy;
	// 		_shadowColor = color;
	// 		_shadowRadius = radius;
	// 		return this;
	// 	}
	//
	// 	public BorderDrawable SetPadding( float top, float left )
	// 	{
	// 		_paddingTop = top;
	// 		_paddingLeft = left;
	// 		return this;
	// 	}
	//
	// 	public void Reset()
	// 	{
	// 		if ( _normalBitmap != null )
	// 		{
	// 			if ( !_normalBitmap.IsDisposed() )
	// 			{
	// 				_normalBitmap.Recycle();
	// 				_normalBitmap.Dispose();
	// 			}
	//
	// 			_normalBitmap = null;
	// 		}
	//
	// 		if ( _pressedBitmap != null )
	// 		{
	// 			if ( !_pressedBitmap.IsDisposed() )
	// 			{
	// 				_pressedBitmap.Recycle();
	// 				_pressedBitmap.Dispose();
	// 			}
	//
	// 			_pressedBitmap = null;
	// 		}
	// 	}
	//
	// 	public override void SetAlpha( int alpha ) { }
	//
	// 	public override void SetColorFilter( ColorFilter? cf ) { }
	//
	// 	public Color BackgroundColor =>
	// 		BorderElement.BackgroundColor == Color.Default
	// 			? _defaultColor
	// 			: BorderElement.BackgroundColor;
	//
	// 	public Color PressedBackgroundColor => BackgroundColor.AddLuminosity(-.12); //<item name="highlight_alpha_material_light" format="float" type="dimen">0.12</item>
	//
	// 	protected override void Dispose( bool disposing )
	// 	{
	// 		if ( _isDisposed )
	// 			return;
	//
	// 		_isDisposed = true;
	//
	// 		if ( disposing )
	// 			Reset();
	//
	// 		base.Dispose(disposing);
	// 	}
	//
	// 	protected override bool OnStateChange( int[]? state )
	// 	{
	// 		if ( state is null ) return false;
	//
	// 		bool old = _pressed;
	// 		_pressed = state.Contains(Android.Resource.Attribute.StatePressed);
	// 		if ( _pressed == old ) return false;
	// 		InvalidateSelf();
	// 		return true;
	// 	}
	//
	// 	private Bitmap CreateBitmap( bool pressed, int width, int height )
	// 	{
	// 		Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
	// 		using ( var canvas = new Canvas(bitmap) )
	// 		{
	// 			DrawBackground(canvas, width, height, pressed);
	// 			if ( _drawOutlineWithBackground )
	// 				DrawOutline(canvas, width, height);
	// 		}
	//
	// 		return bitmap;
	// 	}
	//
	// 	private void DrawBackground( Canvas canvas,
	// 								 int width,
	// 								 int height,
	// 								 bool pressed )
	// 	{
	// 		var paint = new Paint
	// 					{
	// 						AntiAlias = true
	// 					};
	// 		var path = new Path();
	//
	// 		float borderRadius = ConvertCornerRadiusToPixels();
	//
	// 		RectF rect = new RectF(0, 0, width, height);
	//
	// 		rect.Inset(_PaddingLeft, _PaddingTop);
	//
	// 		path.AddRoundRect(rect, borderRadius, borderRadius, Path.Direction.Cw);
	//
	// 		paint.Color = pressed
	// 						  ? PressedBackgroundColor.ToAndroid()
	// 						  : BackgroundColor.ToAndroid();
	// 		paint.SetStyle(Paint.Style.Fill);
	// 		paint.SetShadowLayer(_shadowRadius, _shadowDx, _shadowDy, _shadowColor);
	//
	// 		if ( BorderElement.IsBackgroundSet() )
	// 		{
	// 			Brush background = BorderElement.Background;
	// 			paint.UpdateBackground(background, height, width);
	// 		}
	//
	// 		canvas.DrawPath(path, paint);
	// 	}
	//
	// 	private float ConvertCornerRadiusToPixels()
	// 	{
	// 		int cornerRadius = DefaultCornerRadius;
	//
	// 		if ( BorderElement.IsCornerRadiusSet() &&
	// 			 BorderElement.CornerRadius != (int) BorderElement.CornerRadiusDefaultValue )
	// 			cornerRadius = BorderElement.CornerRadius;
	//
	// 		return _convertToPixels(cornerRadius);
	// 	}
	//
	// 	public RectF GetPaddingBounds( int width, int height )
	// 	{
	// 		RectF rect = new RectF(0, 0, width, height);
	// 		rect.Inset(_PaddingLeft, _PaddingTop);
	// 		return rect;
	// 	}
	//
	// 	public void DrawCircle( Canvas canvas,
	// 							int width,
	// 							int height,
	// 							Action<Canvas> finishDraw )
	// 	{
	// 		try
	// 		{
	// 			var radius = (float) BorderElement.CornerRadius;
	// 			if ( radius <= 0 )
	// 			{
	// 				finishDraw(canvas);
	// 				return;
	// 			}
	//
	// 			float borderThickness = _convertToPixels(BorderElement.BorderWidth);
	//
	// 			using ( var path = new Path() )
	// 			{
	// 				float borderWidth = _convertToPixels(BorderElement.BorderWidth);
	// 				float inset = borderWidth / 2;
	//
	// 				// adjust border radius so outer edge of stroke is same radius as border radius of background
	// 				float borderRadius = Math.Max(ConvertCornerRadiusToPixels() - inset, 0);
	//
	// 				RectF rect = new RectF(0, 0, width, height);
	// 				rect.Inset(_PaddingLeft, _PaddingTop);
	// 				path.AddRoundRect(rect, borderRadius, borderRadius, Path.Direction.Ccw);
	//
	// 				canvas.Save();
	// 				canvas.ClipPath(path);
	//
	// 				finishDraw(canvas);
	// 			}
	//
	// 			canvas.Restore();
	// 			return;
	// 		}
	// 		catch ( Exception ex ) { Xamarin.Forms.Internals.Log.Warning(nameof(BorderDrawable), $"Unable to create circle image: {ex}"); }
	//
	// 		finishDraw(canvas);
	// 	}
	//
	// 	public void DrawOutline( Canvas canvas, int width, int height )
	// 	{
	// 		if ( BorderElement.BorderWidth <= 0 )
	// 			return;
	//
	// 		using var paint = new Paint
	// 						  {
	// 							  AntiAlias = true
	// 						  };
	// 		using var path = new Path();
	// 		float borderWidth = _convertToPixels(BorderElement.BorderWidth);
	// 		float inset = borderWidth / 2;
	//
	// 		// adjust border radius so outer edge of stroke is same radius as border radius of background
	// 		float borderRadius = Math.Max(ConvertCornerRadiusToPixels() - inset, 0);
	//
	// 		RectF rect = new RectF(0, 0, width, height);
	// 		rect.Inset(inset + _PaddingLeft, inset + _PaddingTop);
	//
	// 		path.AddRoundRect(rect, borderRadius, borderRadius, Path.Direction.Cw);
	// 		paint.StrokeWidth = borderWidth;
	// 		paint.SetStyle(Paint.Style.Stroke);
	// 		paint.Color = BorderElement.BorderColor.ToAndroid();
	//
	// 		canvas.DrawPath(path, paint);
	// 	}
	// }
	//
	// internal class BorderBackgroundManager : IDisposable
	// {
	// 	private Drawable? _defaultDrawable;
	// 	private RippleDrawable? _rippleDrawable;
	// 	private bool _drawableEnabled;
	// 	private bool _disposed;
	// 	private IBorderVisualElementRenderer _renderer;
	// 	private IBorderElement? _borderElement;
	//
	// 	private VisualElement _Element => _renderer?.Element;
	// 	private AView? _Control => _renderer?.View;
	// 	private readonly bool _drawOutlineWithBackground;
	//
	// 	public bool DrawOutlineWithBackground { get; set; } = true;
	// 	public BorderDrawable? BackgroundDrawable { get; private set; }
	//
	// 	public BorderBackgroundManager( IBorderVisualElementRenderer renderer ) : this(renderer, true) { }
	//
	// 	public BorderBackgroundManager( IBorderVisualElementRenderer renderer, bool drawOutlineWithBackground )
	// 	{
	// 		_renderer = renderer;
	// 		_renderer.ElementChanged += OnElementChanged;
	// 		_drawOutlineWithBackground = drawOutlineWithBackground;
	// 	}
	//
	// 	private void OnElementChanged( object sender, VisualElementChangedEventArgs e )
	// 	{
	// 		if ( e.OldElement != null )
	// 		{
	// 			if ( e.OldElement is INotifyPropertyChanged oldElement )
	// 				oldElement.PropertyChanged -= BorderElementPropertyChanged;
	// 		}
	//
	// 		if ( e.NewElement != null )
	// 		{
	// 			if ( _BorderPropertyChanged != null ) { _BorderPropertyChanged.PropertyChanged -= BorderElementPropertyChanged; }
	//
	// 			BorderElement = (IBorderElement) e.NewElement;
	//
	// 			if ( _BorderPropertyChanged != null )
	// 				_BorderPropertyChanged.PropertyChanged += BorderElementPropertyChanged;
	// 		}
	//
	// 		Reset();
	// 		UpdateDrawable();
	// 	}
	//
	// 	public IBorderElement? BorderElement
	// 	{
	// 		get => _borderElement;
	// 		private set
	// 		{
	// 			_borderElement = value;
	// 			_BorderPropertyChanged = value as INotifyPropertyChanged;
	// 		}
	// 	}
	//
	// 	private INotifyPropertyChanged? _BorderPropertyChanged { get; set; }
	//
	// 	public void UpdateDrawable()
	// 	{
	// 		if ( BorderElement is null ||
	// 			 _Control is null )
	// 			return;
	//
	// 		bool cornerRadiusIsDefault = !BorderElement.IsCornerRadiusSet() || ( BorderElement.CornerRadius == BorderElement.CornerRadiusDefaultValue || BorderElement.CornerRadius == BorderDrawable.DefaultCornerRadius );
	// 		bool backgroundColorIsDefault = !BorderElement.IsBackgroundColorSet() || BorderElement.BackgroundColor == (Color) VisualElement.BackgroundColorProperty.DefaultValue;
	// 		bool backgroundIsDefault = !BorderElement.IsBackgroundSet() || BorderElement.Background == (Brush) VisualElement.BackgroundProperty.DefaultValue;
	// 		bool borderColorIsDefault = !BorderElement.IsBorderColorSet() || BorderElement.BorderColor == (Color) BorderElement.BorderColorDefaultValue;
	// 		bool borderWidthIsDefault = !BorderElement.IsBorderWidthSet() || BorderElement.BorderWidth == BorderElement.BorderWidthDefaultValue;
	//
	// 		if ( backgroundColorIsDefault &&
	// 			 backgroundIsDefault &&
	// 			 cornerRadiusIsDefault &&
	// 			 borderColorIsDefault &&
	// 			 borderWidthIsDefault )
	// 		{
	// 			if ( !_drawableEnabled )
	// 				return;
	//
	// 			if ( _defaultDrawable != null )
	// 				_Control.SetBackground(_defaultDrawable);
	//
	// 			_drawableEnabled = false;
	// 			Reset();
	// 		}
	// 		else
	// 		{
	// 			if ( _Control.Context is null ) throw new NullReferenceException(nameof(_Control.Context));
	// 			BackgroundDrawable ??= new BorderDrawable(_Control.Context.ToPixels, Forms.GetColorButtonNormal(_Control.Context), _drawOutlineWithBackground);
	//
	// 			BackgroundDrawable.BorderElement = BorderElement;
	//
	// 			bool useDefaultPadding = _renderer.UseDefaultPadding();
	//
	// 			int paddingTop = useDefaultPadding
	// 								 ? _Control.PaddingTop
	// 								 : 0;
	// 			int paddingLeft = useDefaultPadding
	// 								  ? _Control.PaddingLeft
	// 								  : 0;
	//
	// 			bool useDefaultShadow = _renderer.UseDefaultShadow();
	//
	// 			// Use no shadow by default for API < 16
	// 			float shadowRadius = 0;
	// 			float shadowDy = 0;
	// 			float shadowDx = 0;
	// 			AColor shadowColor = Color.Transparent.ToAndroid();
	// 			// Add Android's default material shadow if we want it
	// 			if ( useDefaultShadow )
	// 			{
	// 				shadowRadius = 2;
	// 				shadowDy = 4;
	// 				shadowDx = 0;
	// 				shadowColor = BackgroundDrawable.PressedBackgroundColor.ToAndroid();
	// 			}
	// 			// Otherwise get values from the control (but only for supported APIs)
	// 			else // if ( (int) Forms.SdkInt >= 16 )
	// 			{
	// 				shadowRadius = _renderer.ShadowRadius;
	// 				shadowDy = _renderer.ShadowDy;
	// 				shadowDx = _renderer.ShadowDx;
	// 				shadowColor = _renderer.ShadowColor;
	// 			}
	//
	// 			BackgroundDrawable.SetPadding(paddingTop, paddingLeft);
	// 			if ( _renderer.IsShadowEnabled() ) { BackgroundDrawable.SetShadow(shadowDy, shadowDx, shadowColor, shadowRadius); }
	//
	// 			if ( _drawableEnabled )
	// 				return;
	//
	// 			_defaultDrawable ??= _Control.Background;
	//
	// 			if ( !backgroundColorIsDefault || _drawOutlineWithBackground )
	// 			{
	// 				AColor rippleColor = BackgroundDrawable.PressedBackgroundColor.ToAndroid();
	// 				_rippleDrawable = new RippleDrawable(ColorStateList.ValueOf(rippleColor), BackgroundDrawable, null);
	// 				_Control.SetBackground(_rippleDrawable);
	// 				// if ( Forms.IsLollipopOrNewer )
	// 				// {
	// 				// 	AColor rippleColor = _backgroundDrawable.PressedBackgroundColor.ToAndroid();
	// 				// 	_rippleDrawable = new RippleDrawable(ColorStateList.ValueOf(rippleColor), _backgroundDrawable, null);
	// 				// 	Control.SetBackground(_rippleDrawable);
	// 				// }
	// 				// else { Control.SetBackground(_backgroundDrawable); }
	// 			}
	//
	// 			_drawableEnabled = true;
	// 		}
	//
	// 		_Control.Invalidate();
	// 	}
	//
	// 	public void Reset()
	// 	{
	// 		if ( _drawableEnabled )
	// 		{
	// 			_drawableEnabled = false;
	// 			BackgroundDrawable?.Reset();
	// 			BackgroundDrawable = null;
	// 			_rippleDrawable = null;
	// 		}
	// 	}
	//
	// 	public void UpdateBackgroundColor() { UpdateDrawable(); }
	//
	// 	public void Dispose() { Dispose(true); }
	//
	// 	protected virtual void Dispose( bool disposing )
	// 	{
	// 		if ( !_disposed )
	// 		{
	// 			if ( disposing )
	// 			{
	// 				BackgroundDrawable?.Dispose();
	// 				BackgroundDrawable = null;
	// 				_defaultDrawable?.Dispose();
	// 				_defaultDrawable = null;
	// 				_rippleDrawable?.Dispose();
	// 				_rippleDrawable = null;
	// 				if ( _BorderPropertyChanged != null ) { _BorderPropertyChanged.PropertyChanged -= BorderElementPropertyChanged; }
	//
	// 				BorderElement = null;
	//
	// 				if ( _renderer != null )
	// 				{
	// 					_renderer.ElementChanged -= OnElementChanged;
	// 					_renderer = null;
	// 				}
	// 			}
	//
	// 			_disposed = true;
	// 		}
	// 	}
	//
	// 	private void BorderElementPropertyChanged( object sender, PropertyChangedEventArgs e )
	// 	{
	// 		if ( _renderer.View.IsDisposed() ) { return; }
	//
	// 		if ( e.PropertyName.Equals(ButtonCell.BorderColorProperty.PropertyName) ||
	// 			 e.PropertyName.Equals(ButtonCell.BorderWidthProperty.PropertyName) ||
	// 			 e.PropertyName.Equals(ButtonCell.CornerRadiusProperty.PropertyName) ||
	// 			 e.PropertyName.Equals(ButtonCell.BackgroundBrushProperty.PropertyName) ||
	// 			 e.PropertyName.Equals(CellBase.BackgroundColorProperty.PropertyName)
	// 			 // e.PropertyName.Equals(Specifics.Button.UseDefaultPaddingProperty.PropertyName) ||
	// 			 // e.PropertyName.Equals(Specifics.Button.UseDefaultShadowProperty.PropertyName) ||
	// 			 // e.PropertyName.Equals(Specifics.ImageButton.IsShadowEnabledProperty.PropertyName) ||
	// 			 // e.PropertyName.Equals(Specifics.ImageButton.ShadowColorProperty.PropertyName) ||
	// 			 // e.PropertyName.Equals(Specifics.ImageButton.ShadowOffsetProperty.PropertyName) ||
	// 			 // e.PropertyName.Equals(Specifics.ImageButton.ShadowRadiusProperty.PropertyName) 
	// 			 )
	// 		{
	// 			Reset();
	// 			UpdateDrawable();
	// 		}
	// 	}
	// }
}