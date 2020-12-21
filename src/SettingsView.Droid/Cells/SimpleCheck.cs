using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using AContext = Android.Content.Context;
using AView = Android.Views.View;

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class SimpleCheck : AView
	{
		public Color Color { get; set; }

		protected Paint _paint = new Paint();
		protected AContext _Context { get; set; }


		public SimpleCheck( AContext context ) : base(context)
		{
			_Context = context;
			SetWillNotDraw(false);
		}

		public override bool Selected
		{
			get => base.Selected;
			set
			{
				base.Selected = value;
				Invalidate();
			}
		}

		protected override void OnDraw( Canvas? canvas )
		{
			base.OnDraw(canvas);

			if ( !base.Selected ||
				 canvas is null ) { return; }

			_paint.SetStyle(Paint.Style.Stroke);
			_paint.Color = Color;
			_paint.StrokeWidth = _Context.ToPixels(2);
			_paint.AntiAlias = true;

			float fromX = 22f / 100f * canvas.Width;
			float fromY = 52f / 100f * canvas.Height;
			float toX = 38f / 100f * canvas.Width;
			float toY = 68f / 100f * canvas.Height;

			canvas.DrawLine(fromX,
							fromY,
							toX,
							toY,
							_paint
						   );

			fromX = 36f / 100f * canvas.Width;
			fromY = 66f / 100f * canvas.Height;

			toX = 74f / 100f * canvas.Width;
			toY = 28f / 100f * canvas.Height;

			canvas.DrawLine(fromX,
							fromY,
							toX,
							toY,
							_paint
						   );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing ) { _paint.Dispose(); }

			base.Dispose(disposing);
		}
	}
}