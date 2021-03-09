﻿using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Extensions;
using Jakar.SettingsView.Shared;
using Jakar.SettingsView.Shared.sv;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Jakar.SettingsView.iOS
{
	/// <summary>
	/// Settings table source.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class SettingsTableSource : UITableViewSource
	{
		/// <summary>
		/// The table view.
		/// </summary>
		protected UITableView _tableView;

		/// <summary>
		/// The settings view.
		/// </summary>
		protected Shared.sv.SettingsView _settingsView;

		private bool _disposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.SettingsTableSource"/> class.
		/// </summary>
		/// <param name="settingsView">Settings view.</param>
		public SettingsTableSource( Shared.sv.SettingsView settingsView )
		{
			_settingsView = settingsView;
			_settingsView.ModelChanged += ( sender, e ) =>
										  {
											  if ( _tableView != null )
											  {
												  _tableView.ReloadData();
												  // reflect a dynamic cell height
												  _tableView.PerformBatchUpdates(null, null);
											  }
										  };
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell( UITableView tableView, NSIndexPath indexPath )
		{
			//get forms cell
			Cell cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);

			string id = cell.GetType().FullName;

			var renderer = (CellRenderer) Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IRegisterable>(cell.GetType());

			//get recycle cell
			UITableViewCell reusableCell = tableView.DequeueReusableCell(id);
			//get native cell
			UITableViewCell nativeCell = renderer.GetCell(cell, reusableCell, tableView);

			UITableViewCell cellWithContent = nativeCell;

			// Sometimes iOS for returns a dequeued cell whose Layer is hidden. 
			// This prevents it from showing up, so lets turn it back on!
			if ( cellWithContent.Layer.Hidden )
				cellWithContent.Layer.Hidden = false;

			// Because the layer was hidden we need to layout the cell by hand
			cellWithContent?.LayoutSubviews();

			//selected background
			if ( !( nativeCell is CellBaseView ) ) { nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None; }

			return nativeCell;
		}

		/// <summary>
		/// Gets the height for row.
		/// </summary>
		/// <returns>The height for row.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override nfloat GetHeightForRow( UITableView tableView, NSIndexPath indexPath )
		{
			if ( !_settingsView.HasUnevenRows ) { return tableView.EstimatedRowHeight; }

			Cell cell = _settingsView.Model.GetCell(indexPath.Section, indexPath.Row);
			double h = cell.Height;

			if ( h == -1 )
			{
				//automatic height
				return tableView.RowHeight;
			}

			//individual height
			return (nfloat) h;
		}

		/// <summary>
		/// section header height
		/// </summary>
		/// <returns>The height for header.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="section">Section.</param>
		public override nfloat GetHeightForHeader( UITableView tableView, nint sectionID )
		{
			Section section = _settingsView.Model.GetSection((int) sectionID);

			return !section.IsVisible
					   ? nfloat.Epsilon
					   : UITableView.AutomaticDimension;
			// if ( !section.IsVisible ) { return nfloat.Epsilon; }
			//
			// return UITableView.AutomaticDimension; // automatic height
			//
			// double individualHeight = section.HeaderHeight;
			//
			// if ( individualHeight > 0d ) { return (nfloat) individualHeight; }
			//
			// if ( _settingsView.HeaderHeight == -1d ) { return UITableView.AutomaticDimension; }
			//
			// return (nfloat) _settingsView.HeaderHeight;
		}

		/// <summary>
		/// Gets the view for header.
		/// </summary>
		/// <returns>The view for header.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="section">Section.</param>
		public override UIView GetViewForHeader( UITableView tableView, nint section )
		{
			View formsView = _settingsView.Model.GetSectionHeaderView((int) section);
			if ( formsView != null ) { return GetNativeSectionHeaderFooterView(formsView, tableView, true); }


			var headerView = _tableView.DequeueReusableHeaderFooterView(SettingsViewRenderer.TextHeaderId) as TextHeaderView;
			if ( headerView is null )
			{
				// for HotReload
				return new UIView();
			}

			headerView.Label.Text = _settingsView.Model.GetSectionTitle((int) section);
			headerView.Label.TextColor = _settingsView.HeaderTextColor == Color.Default ? UIColor.Gray : _settingsView.HeaderTextColor.ToUIColor();
			headerView.Label.Font = FontUtility.CreateNativeFont(_settingsView.HeaderFontFamily, (float) _settingsView.HeaderFontSize, _settingsView.HeaderFontAttributes);
			//UIFont.SystemFontOfSize((nfloat)_settingsView.HeaderFontSize);
			headerView.BackgroundView.BackgroundColor = _settingsView.HeaderBackgroundColor.ToUIColor();
			headerView.Label.Padding = _settingsView.HeaderPadding.ToUIEdgeInsets();

			Section sec = _settingsView.Model.GetSection((int) section);
			headerView.SetVerticalAlignment(_settingsView.HeaderTextVerticalAlign);
			// if ( sec.HeaderHeight != -1 ||
			// 	 _settingsView.HeaderHeight != -1 ) { headerView.SetVerticalAlignment(_settingsView.HeaderTextVerticalAlign); }

			return headerView;
		}

		/// <summary>
		/// section footer height
		/// </summary>
		/// <returns>The height for footer.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="section">Section.</param>
		public override nfloat GetHeightForFooter( UITableView tableView, nint section )
		{
			Section sec = _settingsView.Model.GetSection((int) section);

			if ( !sec.IsVisible ) { return nfloat.Epsilon; }

			if ( !sec.FooterVisible ) { return nfloat.Epsilon; }

			if ( sec.FooterView != null )
			{
				return UITableView.AutomaticDimension; // automatic height
			}

			string footerText = sec.FooterText;

			if ( string.IsNullOrEmpty(footerText) )
			{
				//hide footer
				return nfloat.Epsilon; // must not zero
			}

			return UITableView.AutomaticDimension;
		}

		/// <summary>
		/// Gets the view for footer.
		/// </summary>
		/// <returns>The view for footer.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="section">Section.</param>
		public override UIView GetViewForFooter( UITableView tableView, nint section )
		{
			View formsView = _settingsView.Model.GetSectionFooterView((int) section);
			if ( formsView != null ) { return GetNativeSectionHeaderFooterView(formsView, tableView, false); }

			string text = _settingsView.Model.GetFooterText((int) section);

			if ( string.IsNullOrEmpty(text) ) { return new UIView(CGRect.Empty); }

			var footerView = _tableView.DequeueReusableHeaderFooterView(SettingsViewRenderer.TextFooterId) as TextFooterView;

			if ( footerView is null )
			{
				// for HotReload
				return new UIView();
			}

			footerView.Label.Text = text;
			footerView.Label.TextColor = _settingsView.FooterTextColor == Color.Default ? UIColor.Gray : _settingsView.FooterTextColor.ToUIColor();
			footerView.Label.Font = FontUtility.CreateNativeFont(_settingsView.FooterFontFamily, (float) _settingsView.FooterFontSize, _settingsView.FooterFontAttributes);
			//UIFont.SystemFontOfSize((nfloat)_settingsView.FooterFontSize);
			footerView.BackgroundView.BackgroundColor = _settingsView.FooterBackgroundColor.ToUIColor();
			footerView.Label.Padding = _settingsView.FooterPadding.ToUIEdgeInsets();

			return footerView;
		}

		private UIView GetNativeSectionHeaderFooterView( View formsView, UITableView tableView, bool isHeader )
		{
			string idString = isHeader ? SettingsViewRenderer.CustomHeaderId : SettingsViewRenderer.CustomFooterId;
			var nativeView = tableView.DequeueReusableHeaderFooterView(idString) as CustomHeaderFooterView;
			nativeView.UpdateCell(formsView, tableView);

			return nativeView;
		}

		/// <summary>
		/// Numbers the of sections.
		/// </summary>
		/// <returns>The of sections.</returns>
		/// <param name="tableView">Table view.</param>
		public override nint NumberOfSections( UITableView tableView )
		{
			_tableView = tableView;
			return _settingsView.Model.GetSectionCount();
		}

		/// <summary>
		/// Rowses the in section.
		/// </summary>
		/// <returns>The in section.</returns>
		/// <param name="tableview">Tableview.</param>
		/// <param name="section">Section.</param>
		public override nint RowsInSection( UITableView tableview, nint section )
		{
			Section sec = _settingsView.Model.GetSection((int) section);
			return sec.IsVisible ? sec.Count : 0;
		}


		public override bool ShouldShowMenu( UITableView tableView, NSIndexPath rowAtindexPath )
		{
			if ( _settingsView.Model.GetSection(rowAtindexPath.Section).UseDragSort ) { return false; }

			var ret = false;
			if ( tableView.CellAt(rowAtindexPath) is CellBaseView cell )
			{
				System.Diagnostics.Debug.WriteLine("LongTap");
				ret = cell.RowLongPressed(tableView, rowAtindexPath);
			}

			if ( ret )
			{
				_settingsView.Model.RowLongPressed(_settingsView.Model.GetCell(rowAtindexPath.Section, rowAtindexPath.Row));
				BeginInvokeOnMainThread(async () =>
										{
											await Task.Delay(250);
											tableView.CellAt(rowAtindexPath).SetSelected(false, true);
										});
			}

			return ret;
		}

		public override bool CanPerformAction( UITableView tableView,
											   Selector action,
											   NSIndexPath indexPath,
											   NSObject sender ) =>
			false;

		public override void PerformAction( UITableView tableView,
											Selector action,
											NSIndexPath indexPath,
											NSObject sender ) { }

		/// <summary>
		/// Title text string array (unknown what to do ) 
		/// </summary>
		/// <returns>The index titles.</returns>
		/// <param name="tableView">Table view.</param>
		public override string[] SectionIndexTitles( UITableView tableView ) => _settingsView.Model.GetSectionIndexTitles();

		/// <summary>
		/// processing when row is selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			_settingsView.Model.RowSelected(indexPath.Section, indexPath.Row);

			if ( tableView.CellAt(indexPath) is CellBaseView cell ) { cell.RowSelected(tableView, indexPath); }
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( !_disposed )
			{
				_settingsView = null;
				_tableView = null;
			}

			_disposed = true;

			base.Dispose(disposing);
		}
	}
}