﻿using System.ComponentModel;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.iOS.Cells;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LabelCell), typeof(LabelCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Label cell renderer.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class LabelCellRenderer : CellBaseRenderer<LabelCellView> { }

	/// <summary>
	/// Label cell view.
	/// </summary>
	[Foundation.Preserve(AllMembers = true)]
	public class LabelCellView : CellBaseView
	{
		/// <summary>
		/// Gets or sets the value label.
		/// </summary>
		/// <value>The value label.</value>
		public UILabel ValueLabel { get; set; }

		private LabelCell _LabelCell => Cell as LabelCell;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.LabelCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public LabelCellView( Cell formsCell ) : base(formsCell)
		{
			ValueLabel = new UILabel();
			ValueLabel.TextAlignment = UITextAlignment.Right;

			ContentStack.AddArrangedSubview(ValueLabel);
			ValueLabel.SetContentHuggingPriority(100f, UILayoutConstraintAxis.Horizontal);
			ValueLabel.SetContentCompressionResistancePriority(100f, UILayoutConstraintAxis.Horizontal);
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == LabelCell.ValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == LabelCell.ValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == LabelCell.ValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == LabelCell.ValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
			else if ( e.PropertyName == LabelCell.ValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
		}

		/// <summary>
		/// Parents the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.ParentPropertyChanged(sender, e);

			if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextColorProperty.PropertyName ) { UpdateValueTextColor(); }
			else if ( e.PropertyName == Shared.sv.SettingsView.CellValueTextFontSizeProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontFamilyProperty.PropertyName ||
					  e.PropertyName == Shared.sv.SettingsView.CellValueTextFontAttributesProperty.PropertyName ) { UpdateWithForceLayout(UpdateValueTextFont); }
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			UpdateValueText();
			UpdateValueTextColor();
			UpdateValueTextFont();
			base.UpdateCell(tableView);
		}

		/// <summary>
		/// Sets the enabled appearance.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		protected override void SetEnabledAppearance( bool isEnabled )
		{
			if ( isEnabled ) { ValueLabel.Alpha = 1f; }
			else { ValueLabel.Alpha = 0.3f; }

			base.SetEnabledAppearance(isEnabled);
		}

		/// <summary>
		/// Updates the value text.
		/// </summary>
		protected void UpdateValueText() { ValueLabel.Text = _LabelCell.ValueText; }

		private void UpdateValueTextFont()
		{
			if ( ValueLabel.Font is null )
				return; // for HotReload

			string family = _LabelCell.ValueTextFontFamily ?? CellParent.CellValueTextFontFamily;
			FontAttributes attr = _LabelCell.ValueTextFontAttributes ?? CellParent.CellValueTextFontAttributes;

			if ( _LabelCell.ValueTextFontSize > 0 ) { ValueLabel.Font = FontUtility.CreateNativeFont(family, (float) _LabelCell.ValueTextFontSize, attr); }
			else if ( CellParent != null ) { ValueLabel.Font = FontUtility.CreateNativeFont(family, (float) CellParent.CellValueTextFontSize, attr); }
		}

		private void UpdateValueTextColor()
		{
			if ( _LabelCell.ValueTextColor != Color.Default ) { ValueLabel.TextColor = _LabelCell.ValueTextColor.ToUIColor(); }
			else if ( CellParent != null &&
					  CellParent.CellValueTextColor != Color.Default ) { ValueLabel.TextColor = CellParent.CellValueTextColor.ToUIColor(); }
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				ContentStack.RemoveArrangedSubview(ValueLabel);
				ValueLabel.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}