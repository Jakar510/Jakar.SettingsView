using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using PointF = CoreGraphics.CGPoint;
using RectangleF = CoreGraphics.CGRect;

namespace Jakar.SettingsView.iOS
{
	[Foundation.Preserve(AllMembers = true)]
	public class KeyboardInsetTracker : IDisposable
	{
		// https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Platform.iOS/Renderers/KeyboardInsetTracker.cs
		private readonly Func<UIWindow> _fetchWindow;
		private readonly Action<PointF> _setContentOffset;
		private readonly Action<UIEdgeInsets> _setInsetAction;
		private readonly UIScrollView _targetView;
		private bool _disposed;
		private UIEdgeInsets _currentInset;
		private RectangleF _lastKeyboardRect;
		private ShellScrollViewTracker _shellScrollViewTracker;


		public KeyboardInsetTracker( UIScrollView targetView, Func<UIWindow> fetchWindow, Action<UIEdgeInsets> setInsetAction ) : this(targetView, fetchWindow, setInsetAction, null) { }

		public KeyboardInsetTracker( UIScrollView targetView,
									 Func<UIWindow> fetchWindow,
									 Action<UIEdgeInsets> setInsetAction,
									 Action<PointF> setContentOffset ) : this(targetView,
																			  fetchWindow,
																			  setInsetAction,
																			  setContentOffset,
																			  null
																			 ) { }

		public KeyboardInsetTracker( UIScrollView targetView,
									 Func<UIWindow> fetchWindow,
									 Action<UIEdgeInsets> setInsetAction,
									 Action<PointF> setContentOffset,
									 IVisualElementRenderer renderer )
		{
			_setContentOffset = setContentOffset;
			_targetView = targetView;
			_fetchWindow = fetchWindow;
			_setInsetAction = setInsetAction;
			KeyboardObserver.KeyboardWillShow += OnKeyboardShown;
			KeyboardObserver.KeyboardWillHide += OnKeyboardHidden;
			if ( renderer != null )
				_shellScrollViewTracker = new ShellScrollViewTracker(renderer);
		}

		public void Dispose()
		{
			if ( _disposed )
				return;

			_disposed = true;

			KeyboardObserver.KeyboardWillShow -= OnKeyboardShown;
			KeyboardObserver.KeyboardWillHide -= OnKeyboardHidden;

			_shellScrollViewTracker?.Dispose();
			_shellScrollViewTracker = null;
		}

		//This method allows us to update the insets if the Frame changes
		internal void UpdateInsets()
		{
			//being called from LayoutSubviews but keyboard wasn't shown yet
			if ( _lastKeyboardRect.IsEmpty )
				return;

			UIWindow window = _fetchWindow();
			// Code left verbose to make its operation more obvious
			if ( window == null )
			{
				// we are not currently displayed and can safely ignore this
				// most likely this renderer is on a page which is currently not displayed (e.g. in NavController)
				return;
			}

			var field = (UIView) _targetView.GetType()
											.GetMethod("FindFirstResponder")
											?.Invoke(_targetView,
													 new object[]
													 { }
													);

			//the view that is triggering the keyboard is not inside our UITableView?
			//if (field == null)
			//	return;

			CGSize boundsSize = _targetView.Frame.Size;

			//since our keyboard frame is RVC CoordinateSpace, lets convert it to our targetView CoordinateSpace
			CGRect rect = _targetView.Superview.ConvertRectFromView(_lastKeyboardRect, null);
			//let's see how much does it cover our target view
			CGRect overlay = RectangleF.Intersect(rect, _targetView.Frame);

			_currentInset = _targetView.ContentInset;
			_setInsetAction(new UIEdgeInsets(0, 0, overlay.Height, 0));

			if ( field is not UITextView ||
				 _setContentOffset is null ) return;
			nfloat keyboardTop = boundsSize.Height - overlay.Height;
			CGPoint fieldPosition = field.ConvertPointToView(field.Frame.Location, _targetView.Superview);
			nfloat fieldBottom = fieldPosition.Y + field.Frame.Height;
			nfloat offset = fieldBottom - keyboardTop;
			if ( offset > 0 )
				_setContentOffset(new PointF(0, offset));
		}

		public void OnLayoutSubviews() => _shellScrollViewTracker?.OnLayoutSubviews();

		private void OnKeyboardHidden( object sender, UIKeyboardEventArgs args )
		{
			if ( _shellScrollViewTracker == null ||
				 !_shellScrollViewTracker.Reset() )
				_setInsetAction(new UIEdgeInsets(0, 0, 0, 0));

			_lastKeyboardRect = RectangleF.Empty;
		}

		private void OnKeyboardShown( object sender, UIKeyboardEventArgs args )
		{
			_lastKeyboardRect = args.FrameEnd;
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

		public static event EventHandler<UIKeyboardEventArgs> KeyboardWillHide;

		public static event EventHandler<UIKeyboardEventArgs> KeyboardWillShow;

		private static void OnKeyboardHidden( object sender, UIKeyboardEventArgs args )
		{
			EventHandler<UIKeyboardEventArgs> handler = KeyboardWillHide;
			handler?.Invoke(sender, args);
		}

		private static void OnKeyboardShown( object sender, UIKeyboardEventArgs args )
		{
			EventHandler<UIKeyboardEventArgs> handler = KeyboardWillShow;
			handler?.Invoke(sender, args);
		}
	}
}