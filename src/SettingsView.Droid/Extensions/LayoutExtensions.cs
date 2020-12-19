using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Views;
using Android.Widget;
using ARelativeLayout = Android.Widget.RelativeLayout;
using AGridLayout = Android.Widget.GridLayout;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using Object = Java.Lang.Object;

#nullable enable
namespace Jakar.SettingsView.Droid.Extensions
{
	public static class LayoutExtensions
	{
		private static void Run( Action action, string caller )
		{
			try { action(); }
			catch ( Exception )
			{
				var temp = new StackTrace();
				Debug.WriteLine($" __________________________________________ {caller} __________________________________________ ");
				Debug.WriteLine(temp.ToString());
				Debug.WriteLine($" __________________________________________ {caller} __________________________________________ ");
				throw;
			}
		}
		public static void Add( this LinearLayout stack,
								AView view,
								int? width = null,
								int? height = null,
								[CallerMemberName] string caller = "" )
		{
			if ( stack is null ) throw new NullReferenceException(nameof(stack));

			Run(() =>
				{
					using var layoutParams = new LinearLayout.LayoutParams(width ?? ViewGroup.LayoutParams.MatchParent, height ?? ViewGroup.LayoutParams.MatchParent);
					// using var layoutParams = new LinearLayout.LayoutParams(width ?? ViewGroup.LayoutParams.WrapContent, height ?? ViewGroup.LayoutParams.WrapContent);
					{
						stack.AddView(view, layoutParams);
					}
				},
				caller
			   );
		}
		public static void Add( this ARelativeLayout stack,
								AView view,
								int? width = null,
								int? height = null,
								[CallerMemberName] string caller = "" )
		{
			if ( stack is null ) throw new NullReferenceException(nameof(stack));

			Run(() =>
				{
					using var layoutParams = new ARelativeLayout.LayoutParams(width ?? ViewGroup.LayoutParams.MatchParent, height ?? ViewGroup.LayoutParams.MatchParent);
					{
						stack.AddView(view, layoutParams);
					}
				},
				caller
			   );
		}
		public static void Add( this AGridLayout stack,
								AView view,
								int row,
								int column,
								AGridLayout.Alignment? columnPos = null,
								AGridLayout.Alignment? rowPos = null,
								int? width = null,
								int? height = null,
								int bottomMargin = 4,
								int topMargin = 4,
								int leftMargin = 10,
								int rightMargin = 10,
								[CallerMemberName] string caller = "" )
		{
			if ( stack is null )
				throw new NullReferenceException(nameof(stack));

			Run(() =>
				{
					using var parameters = new AGridLayout.LayoutParams()
										   {
											   ColumnSpec = AGridLayout.InvokeSpec(column, columnPos ?? AGridLayout.Center),
											   RowSpec = AGridLayout.InvokeSpec(row, rowPos ?? AGridLayout.Center),
											   Width = width ?? ViewGroup.LayoutParams.WrapContent,
											   Height = height ?? ViewGroup.LayoutParams.WrapContent,
											   BottomMargin = bottomMargin,
											   TopMargin = topMargin,
											   LeftMargin = leftMargin,
											   RightMargin = rightMargin,
										   };
					{
						stack.AddView(view, parameters);
					}
				},
				caller
			   );
		}


		public static AView CreateContentView( this AContext context,
											   ViewGroup? root,
											   int id,
											   bool attach = true,
											   [CallerMemberName] string caller = "" )
		{
			Object? temp = context.GetSystemService(Context.LayoutInflaterService);
			var inflater = (LayoutInflater) ( temp ?? throw new NullReferenceException(nameof(Context.LayoutInflaterService)) );

			return inflater.Inflate(id, root, attach) ?? throw new InflateException($"ID: {id} not found. Called from {caller}");
		}
	}
}