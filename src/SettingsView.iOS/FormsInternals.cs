using System;
using System.Reflection;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS
{
	public static class FormsInternals
	{
		// Get internal members
		public static BindableProperty RendererProperty = (BindableProperty) typeof(Platform).GetField("RendererProperty", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		public static Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.iOS.Platform+DefaultRenderer");
		public static Type ModalWrapper = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.iOS.ModalWrapper");
		public static MethodInfo ModalWapperDispose = ModalWrapper.GetMethod("Dispose");

		// From internal Platform class
		public static void DisposeModelAndChildrenRenderers( Element view )
		{
			IVisualElementRenderer renderer;
			foreach ( VisualElement child in view.Descendants() )
			{
				renderer = Platform.GetRenderer(child);
				child.ClearValue(RendererProperty);

				if ( renderer != null )
				{
					renderer.NativeView.RemoveFromSuperview();
					renderer.Dispose();
				}
			}

			renderer = Platform.GetRenderer((VisualElement) view);
			if ( renderer != null )
			{
				if ( renderer.ViewController != null )
				{
					if ( renderer.ViewController.ParentViewController.GetType() == ModalWrapper )
					{
						object modalWrapper = Convert.ChangeType(renderer.ViewController.ParentViewController, ModalWrapper);
						ModalWapperDispose.Invoke(modalWrapper, new object[]
																{ });
					}
				}

				renderer.NativeView.RemoveFromSuperview();
				renderer.Dispose();
			}

			view.ClearValue(RendererProperty);
		}

		// From internal Platform class
		public static void DisposeRendererAndChildren( IVisualElementRenderer rendererToRemove )
		{
			if ( rendererToRemove == null )
				return;

			if ( rendererToRemove.Element != null &&
				 Platform.GetRenderer(rendererToRemove.Element) == rendererToRemove )
				rendererToRemove.Element.ClearValue(RendererProperty);

			UIView[] subviews = rendererToRemove.NativeView.Subviews;
			for ( var i = 0; i < subviews.Length; i++ )
			{
				var childRenderer = subviews[i] as IVisualElementRenderer;
				if ( childRenderer != null )
					DisposeRendererAndChildren(childRenderer);
			}

			rendererToRemove.NativeView.RemoveFromSuperview();
			rendererToRemove.Dispose();
		}
	}
}