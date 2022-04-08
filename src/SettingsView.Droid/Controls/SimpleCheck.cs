

namespace Jakar.SettingsView.Droid.Controls;

[Preserve(AllMembers = true)]
public class SimpleCheck : ACheckBox // AView
{
    public AColor Color { get; set; }

    // protected Paint _paint = new Paint();
    protected AContext _Context { get; set; }


    public SimpleCheck( AContext context, AColor color ) : this(context) => Color = color;
    public SimpleCheck( AContext context ) : base(context)
    {
        _Context = context;
        SetWillNotDraw(true);
        // SetWillNotDraw(false);
        // _paint.SetStyle(Paint.Style.Stroke);
        // _paint.StrokeWidth = _Context.ToPixels(2);
        // _paint.AntiAlias = true;
    }
		
    // public override bool Selected
    // {
    // 	get => base.Selected;
    // 	set
    // 	{
    // 		base.Selected = value;
    // 		Invalidate();
    // 	}
    // }

    // protected override void OnDraw( Canvas? canvas )
    // {
    // 	base.OnDraw(canvas);
    //
    // 	if ( !base.Selected ||
    // 		 canvas is null ) { return; }
    //
    // 	_paint.Color = Color;
    //
    // 	// float centerX = 0.5f * canvas.Width;
    // 	// float centerY = 0.5f * canvas.Width;
    // 	// float radius = Math.Min(0.75f * canvas.Width, 0.75f * canvas.Height);
    // 	// canvas.DrawCircle(centerX, centerY, radius, _paint);
    //
    // 	float fromX = 0.22f * canvas.Width;
    // 	float fromY = 0.5f * canvas.Height;
    // 	float toX = 0.38F * canvas.Width;
    // 	float toY = 0.68F * canvas.Height;
    // 	canvas.DrawLine(fromX,
    // 					fromY,
    // 					toX,
    // 					toY,
    // 					_paint
    // 				   );
    //
    //
    // 	fromX = 0.36F * canvas.Width;
    // 	fromY = 0.66F * canvas.Height;
    // 	toX = 0.74F * canvas.Width;
    // 	toY = 0.28F * canvas.Height;
    // 	canvas.DrawLine(fromX,
    // 					fromY,
    // 					toX,
    // 					toY,
    // 					_paint
    // 				   );
    // }
    //
    // protected override void Dispose( bool disposing )
    // {
    // 	if ( disposing ) { _paint.Dispose(); }
    //
    // 	base.Dispose(disposing);
    // }
}