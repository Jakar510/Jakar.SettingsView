namespace Jakar.SettingsView.iOS
{
	[Foundation.Preserve(AllMembers = true)]
	public class KeyboardInsetTracker : IDisposable
	{
		// https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Platform.iOS/Renderers/KeyboardInsetTracker.cs

		private bool _disposed;
		private Func<UIWindow> _FetchWindow { get; }
		private Action<PointF>? _SetContentOffset { get; }
		private Action<UIEdgeInsets> _SetInsetAction { get; }
		private UIScrollView _TargetView { get; }
		private UIEdgeInsets _CurrentInset { get; set; }
		private RectangleF _LastKeyboardRect { get; set; }
		private ShellScrollViewTracker? _ShellScrollViewTracker { get; set; }


		public KeyboardInsetTracker( UIScrollView targetView, Func<UIWindow> fetchWindow, Action<UIEdgeInsets> setInsetAction ) : this(targetView, fetchWindow, setInsetAction, null) { }

		public KeyboardInsetTracker( UIScrollView targetView,
									 Func<UIWindow> fetchWindow,
									 Action<UIEdgeInsets> setInsetAction,
									 Action<PointF>? setContentOffset ) : this(targetView,
																			   fetchWindow,
																			   setInsetAction,
																			   setContentOffset,
																			   null
																			  ) { }

		public KeyboardInsetTracker( UIScrollView targetView,
									 Func<UIWindow> fetchWindow,
									 Action<UIEdgeInsets> setInsetAction,
									 Action<PointF>? setContentOffset,
									 IVisualElementRenderer? renderer )
		{
			_SetContentOffset = setContentOffset;
			_TargetView = targetView;
			_FetchWindow = fetchWindow;
			_SetInsetAction = setInsetAction;
			KeyboardObserver.KeyboardWillShow += OnKeyboardShown;
			KeyboardObserver.KeyboardWillHide += OnKeyboardHidden;
			if ( renderer != null )
				_ShellScrollViewTracker = new ShellScrollViewTracker(renderer);
		}

		public void Dispose()
		{
			if ( _disposed )
				return;

			_disposed = true;

			KeyboardObserver.KeyboardWillShow -= OnKeyboardShown;
			KeyboardObserver.KeyboardWillHide -= OnKeyboardHidden;

			_ShellScrollViewTracker?.Dispose();
			_ShellScrollViewTracker = null;
		}

		//This method allows us to update the insets if the Frame changes
		internal void UpdateInsets()
		{
			//being called from LayoutSubviews but keyboard wasn't shown yet
			if ( _LastKeyboardRect.IsEmpty )
				return;

			UIWindow window = _FetchWindow();
			// Code left verbose to make its operation more obvious
			if ( window == null )
			{
				// we are not currently displayed and can safely ignore this
				// most likely this renderer is on a page which is currently not displayed (e.g. in NavController)
				return;
			}

			var field = _TargetView.GetType()
								   .GetMethod("FindFirstResponder")
								   ?.Invoke(_TargetView,
											new object[]
											{ }
										   ) as UIView;

			//the view that is triggering the keyboard is not inside our UITableView?
			//if (field == null)
			//	return;

			CGSize boundsSize = _TargetView.Frame.Size;

			//since our keyboard frame is RVC CoordinateSpace, lets convert it to our targetView CoordinateSpace
			CGRect rect = _TargetView.Superview.ConvertRectFromView(_LastKeyboardRect, null);
			//let's see how much does it cover our target view
			CGRect overlay = RectangleF.Intersect(rect, _TargetView.Frame);

			_CurrentInset = _TargetView.ContentInset;
			_SetInsetAction(new UIEdgeInsets(0, 0, overlay.Height, 0));

			if ( field is not UITextView ||
				 _SetContentOffset is null ) return;
			nfloat keyboardTop = boundsSize.Height - overlay.Height;
			CGPoint fieldPosition = field.ConvertPointToView(field.Frame.Location, _TargetView.Superview);
			nfloat fieldBottom = fieldPosition.Y + field.Frame.Height;
			nfloat offset = fieldBottom - keyboardTop;
			if ( offset > 0 )
				_SetContentOffset(new PointF(0, offset));
		}

		public void OnLayoutSubviews() => _ShellScrollViewTracker?.OnLayoutSubviews();

		private void OnKeyboardHidden( object sender, UIKeyboardEventArgs args )
		{
			if ( _ShellScrollViewTracker == null ||
				 !_ShellScrollViewTracker.Reset() )
				_SetInsetAction(new UIEdgeInsets(0, 0, 0, 0));

			_LastKeyboardRect = RectangleF.Empty;
		}

		private void OnKeyboardShown( object sender, UIKeyboardEventArgs args )
		{
			_LastKeyboardRect = args.FrameEnd;
			UpdateInsets();
		}
	}


	public static class KeyboardObserver
	{
		static KeyboardObserver()
		{
			UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShown);
			UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHidden);
		}

		public static event EventHandler<UIKeyboardEventArgs>? KeyboardWillHide;

		public static event EventHandler<UIKeyboardEventArgs>? KeyboardWillShow;

		private static void OnKeyboardHidden( object sender, UIKeyboardEventArgs args )
		{
			EventHandler<UIKeyboardEventArgs>? handler = KeyboardWillHide;
			handler?.Invoke(sender, args);
		}

		private static void OnKeyboardShown( object sender, UIKeyboardEventArgs args )
		{
			EventHandler<UIKeyboardEventArgs>? handler = KeyboardWillShow;
			handler?.Invoke(sender, args);
		}
	}
}