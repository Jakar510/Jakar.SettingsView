using System;
using Android.Content;
using Android.Runtime;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	[Preserve(AllMembers = true)]
	public class LabelCellView : BaseAiValueCell
	{
		protected LabelCell _LabelCell => Cell as LabelCell ?? throw new NullReferenceException(nameof(_LabelCell));


		public LabelCellView( Context context, Cell cell ) : base(context, cell) { }

		public LabelCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }
	}
}