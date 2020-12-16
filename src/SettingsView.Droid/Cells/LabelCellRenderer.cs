using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	[Preserve(AllMembers = true)]
	public class LabelCellView : CellBaseView
	{
		protected LabelCell _LabelCell => Cell as LabelCell ?? throw new NullReferenceException(nameof(_LabelCell));

		protected internal Android.Views.View ContentView { get; set; }
		protected GridLayout _CellLayout { get; set; }

		protected IconView _Icon { get; set; }
		protected TitleView _Title { get; set; }
		protected DescriptionView _Description { get; set; }
		protected HintView _Hint { get; set; }
		protected ValueView _Value { get; set; }


		public LabelCellView( Context context, Cell cell ) : base(context, cell)
		{
			ContentView = CreateContentView(Resource.Layout.DisplayCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.DisplayCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.DisplayCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellDescription));
			_Hint = new HintView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellHintText));
			_Value = new ValueView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellValueText))
					 {
						 Label =
						 {
							 Ellipsize = TextUtils.TruncateAt.End,
							 Gravity = GravityFlags.Right
						 }
					 };

			_Value.Label.SetSingleLine(true);
		}
		public LabelCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer)
		{
			ContentView = CreateContentView(Resource.Layout.DisplayCellLayout);
			_CellLayout = ContentView.FindViewById<GridLayout>(Resource.Id.DisplayCellLayout) ?? throw new NullReferenceException(nameof(_CellLayout));
			_Icon = new IconView(this, ContentView.FindViewById<ImageView>(Resource.Id.DisplayCellIcon));
			_Title = new TitleView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellTitle));
			_Description = new DescriptionView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellDescription));
			_Hint = new HintView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellHintText));
			_Value = new ValueView(this, ContentView.FindViewById<TextView>(Resource.Id.DisplayCellValueText))
					 {
						 Label =
						 {
							 Ellipsize = TextUtils.TruncateAt.End,
							 Gravity = GravityFlags.Right
						 }
					 };

			_Value.Label.SetSingleLine(true);
		}

		protected internal override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( _Value.Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Hint.Update(sender, e) ) { return; }

			// if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ) { UpdateValueTextFontSize(); }
		}
		protected internal override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);
			if ( _Value.UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }
		}


		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
			_Hint.Enable();
			_Value.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
			_Hint.Disable();
			_Value.Disable();
		}
		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			_Title.Update();
			_Description.Update();
			_Hint.Update();
			_Value.Update();
			_Icon.Update();
		}
		// private void UpdateUseDescriptionAsValue()
		// {
		// 	if ( !_LabelCell.IgnoreUseDescriptionAsValue &&
		// 		 CellParent != null &&
		// 		 CellParent.UseDescriptionAsValue )
		// 	{
		// 		// _Value = DescriptionLabel;
		// 		_Description.Label.Visibility = ViewStates.Visible;
		// 		_Value.Label.Visibility = ViewStates.Gone;
		// 	}
		// 	else
		// 	{
		// 		// _Value = _Value.Label;
		// 		_Value.Label.Visibility = ViewStates.Visible;
		// 	}
		// }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_Icon.Dispose();
				_Title.Dispose();
				_Description.Dispose();
				_Hint.Dispose();
				_Value.Dispose();

				_CellLayout.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}