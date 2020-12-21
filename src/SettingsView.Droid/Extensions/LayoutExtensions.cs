using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Android.Views;
using Android.Widget;
using ARelativeLayout = Android.Widget.RelativeLayout;
using AGridLayout = Android.Widget.GridLayout;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using AObject = Java.Lang.Object;

#nullable enable
namespace Jakar.SettingsView.Droid.Extensions
{
	public enum Layout
	{
		Match,
		Wrap,
	}

	public enum GridSpec
	{
		BaselineAlignment,
		BottomAlignment,
		Center,
		End,
		Fill,
		LeftAlignment,
		RightAlignment,
		Start,
		TopAlignment
	}

	public static class LayoutExtensions
	{
		private static readonly Dictionary<Layout, int> LayoutMapper = new Dictionary<Layout, int>()
																	   {
																		   { Layout.Match, ViewGroup.LayoutParams.MatchParent },
																		   { Layout.Wrap, ViewGroup.LayoutParams.WrapContent },
																	   };


		private static readonly Dictionary<GridSpec, AGridLayout.Alignment?> SpecMapper = new Dictionary<GridSpec, AGridLayout.Alignment?>()
																						  {
																							  { GridSpec.BaselineAlignment, AGridLayout.BaselineAlighment },
																							  { GridSpec.BottomAlignment, AGridLayout.BottomAlighment },
																							  { GridSpec.Center, AGridLayout.Center },
																							  { GridSpec.End, AGridLayout.End },
																							  { GridSpec.Fill, AGridLayout.Fill },
																							  { GridSpec.LeftAlignment, AGridLayout.LeftAlighment },
																							  { GridSpec.RightAlignment, AGridLayout.RightAlighment },
																							  { GridSpec.Start, AGridLayout.Start },
																							  { GridSpec.TopAlignment, AGridLayout.TopAlighment },
																						  };


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


		// -----------------------------------------------------------------------------------------------------------------------------------------------------
		public static void Add( this LinearLayout stack,
								AView view,
								Layout width = Layout.Wrap,
								Layout height = Layout.Wrap,
								[CallerMemberName] string caller = "" )
		{
			if ( stack is null ) throw new NullReferenceException(nameof(stack));

			Run(() =>
				{
					using var layoutParams = new LinearLayout.LayoutParams(LayoutMapper[width], LayoutMapper[height]);
					{
						stack.AddView(view, layoutParams);
					}
				},
				caller
			   );
		}


		public static void Add( this ARelativeLayout stack,
								AView view,
								Layout width = Layout.Wrap,
								Layout height = Layout.Wrap,
								[CallerMemberName] string caller = "" )
		{
			if ( stack is null ) throw new NullReferenceException(nameof(stack));

			Run(() =>
				{
					using var layoutParams = new ARelativeLayout.LayoutParams(LayoutMapper[width], LayoutMapper[height]);
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
								GridSpec columnPos,
								GridSpec rowPos,
								Layout width = Layout.Wrap,
								Layout height = Layout.Wrap,
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
											   ColumnSpec = AGridLayout.InvokeSpec(column, SpecMapper[columnPos]),
											   RowSpec = AGridLayout.InvokeSpec(row, SpecMapper[rowPos]),
											   Width = LayoutMapper[width],
											   Height = LayoutMapper[height],
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
			AObject? temp = context.GetSystemService(AContext.LayoutInflaterService);
			var inflater = (LayoutInflater) ( temp ?? throw new NullReferenceException(nameof(AContext.LayoutInflaterService)) );

			return inflater.Inflate(id, root, attach) ?? throw new InflateException($"ID: {id} not found. Called from {caller}");
		}
	}
}