using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Jakar.SettingsView.Shared.Cells;
using Xamarin.Forms;
using AColor = Android.Graphics.Color;

namespace Jakar.SettingsView.Droid.Interfaces
{
	internal interface IBaseCell
	{
		public Cell Cell { get; set; }
		public Element Element => Cell;
		protected CellBase _CellBase => Cell as CellBase;
		public Shared.SettingsView CellParent => Cell.Parent as Shared.SettingsView;
		public Context AndroidContext { get; set; }
		public CancellationTokenSource IconTokenSource { get; set; }
		public AColor DefaultTextColor { get; set; }
		public ColorDrawable BackgroundColor { get; set; }
		public ColorDrawable SelectedColor { get; set; }
		public RippleDrawable Ripple { get; set; }
		public float DefaultFontSize { get; set; }
		public float IconRadius { get; set; }

		public void Invalidate();
		public void UpdateWithForceLayout( Action updateAction )
		{
			updateAction();
			Invalidate();
		}
	}
}