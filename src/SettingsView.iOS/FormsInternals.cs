using System;
using System.Reflection;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
namespace Jakar.SettingsView.iOS
{
	public static class FormsInternals
	{
		// Get internal members... why? xamarin.forms why internal??
		public static readonly BindableProperty? RendererProperty = (BindableProperty?) typeof(Platform).GetField("RendererProperty", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null);
		public static readonly Type DefaultRenderer = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.iOS.Platform+DefaultRenderer");
		public static readonly Type ModalWrapper = typeof(Platform).Assembly.GetType("Xamarin.Forms.Platform.iOS.ModalWrapper");
		public static readonly MethodInfo? ModalWrapperDispose = ModalWrapper.GetMethod("Dispose");

		// From internal Platform class
		public static void DisposeModelAndChildrenRenderers( Element view )
		{
			IVisualElementRenderer renderer;
			foreach ( Element element in view.Descendants() )
			{
				if ( element is not VisualElement child ) continue;
				renderer = Platform.GetRenderer(child);
				child.ClearValue(RendererProperty);
				if ( renderer is null ) continue;
				renderer.NativeView.RemoveFromSuperview();
				renderer.Dispose();
			}

			if ( view is VisualElement visual )
			{
				renderer = Platform.GetRenderer(visual);
				if ( renderer is not null )
				{
					if ( renderer.ViewController?.ParentViewController is not null &&
						 renderer.ViewController.ParentViewController.GetType() == ModalWrapper )
					{
						object modalWrapper = Convert.ChangeType(renderer.ViewController.ParentViewController, ModalWrapper);
						ModalWrapperDispose?.Invoke(modalWrapper,
													new object[]
													{ }
												   );
					}

					renderer.NativeView.RemoveFromSuperview();
					renderer.Dispose();
				}
			}

			view.ClearValue(RendererProperty);
		}

		// From internal Platform class
		public static void DisposeRendererAndChildren( IVisualElementRenderer rendererToRemove )
		{
			if ( rendererToRemove.Element is not null &&
				 Platform.GetRenderer(rendererToRemove.Element) == rendererToRemove ) { rendererToRemove.Element.ClearValue(RendererProperty); }

			foreach ( UIView view in rendererToRemove.NativeView.Subviews )
			{
				if ( view is IVisualElementRenderer childRenderer )
					DisposeRendererAndChildren(childRenderer);
			}

			rendererToRemove.NativeView.RemoveFromSuperview();
			rendererToRemove.Dispose();
		}
	}
}