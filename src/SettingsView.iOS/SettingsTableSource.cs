using ObjCRuntime;


namespace Jakar.SettingsView.iOS
{
	[Preserve(AllMembers = true)]
	public class SettingsTableSource : UITableViewSource
	{
		protected Shared.sv.SettingsView _SettingsView { get; set; }

		private bool _disposed;

		public SettingsTableSource( Shared.sv.SettingsView settingsView )
		{
			_SettingsView = settingsView;
			_SettingsView.ModelChanged += OnSettingsViewOnModelChanged;
		}
		private void OnSettingsViewOnModelChanged( object sender, EventArgs e )
		{
			if ( sender is not UITableView tableView ) return;
			tableView.ReloadData();
			// reflect a dynamic cell height
			tableView.PerformBatchUpdates(null, null);
		}
		public override nint NumberOfSections( UITableView tableView ) => _SettingsView.Model.GetSectionCount();

		public override UITableViewCell GetCell( UITableView tableView, NSIndexPath indexPath )
		{
			Cell cell = _SettingsView.Model.GetCell(indexPath.Section, indexPath.Row);

			string id = cell.GetType().FullName;

			var renderer = (CellRenderer) Xamarin.Forms.Internals.Registrar.Registered.GetHandler<IRegisterable>(cell.GetType());

			UITableViewCell reusableCell = tableView.DequeueReusableCell(id);             // get recycle cell
			UITableViewCell nativeCell = renderer.GetCell(cell, reusableCell, tableView); // get native cell

			// Sometimes iOS returns a dequeued cell whose Layer is hidden. This prevents it from showing up, so lets turn it back on!
			if ( nativeCell.Layer.Hidden ) { nativeCell.Layer.Hidden = false; }

			nativeCell.LayoutSubviews(); // Because the layer was hidden we need to layout the cell by hand

			if ( nativeCell is not BaseCellView ) { nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None; } // no selected background for third party cells.

			return nativeCell;
		}
		public override nfloat GetHeightForRow( UITableView tableView, NSIndexPath indexPath ) // TODO: fix this
		{
			if ( !_SettingsView.HasUnevenRows ) { return Math.Max(tableView.EstimatedRowHeight.ToDouble(), SvConstants.Defaults.MIN_ROW_HEIGHT).ToNFloat(); }

			Cell cell = _SettingsView.Model.GetCell(indexPath.Section, indexPath.Row);
			double height = cell.Height;

			return height.Equals(-1)
					   ? Math.Max(tableView.RowHeight.ToDouble(), SvConstants.Defaults.MIN_ROW_HEIGHT).ToNFloat() // automatic height
					   : height.ToNFloat();                                                                       // individual height

			// return h.Equals(-1)
			// 		   ? tableView.RowHeight // automatic height
			// 		   : h.ToNFloat();       // individual height
		}


		public override nfloat GetHeightForHeader( UITableView tableView, nint sectionID )
		{
			Section? section = _SettingsView.Model.GetSection((int) sectionID);
			if ( section is null ) throw new NullReferenceException(nameof(section));
			return section.IsVisible
					   ? UITableView.AutomaticDimension
					   : nfloat.Epsilon;

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
		public override UIView GetViewForHeader( UITableView tableView, nint sectionId )
		{
			Section? section = _SettingsView.Model.GetSection((int) sectionId);
			if ( section is null ) throw new NullReferenceException(nameof(section));
			return GetNativeSectionHeaderFooterView(section.HeaderView, tableView, section);
		}
		// public override UIView GetViewForHeader( UITableView tableView, nint sectionId )
		// {
		// 	if ( tableView.DequeueReusableHeaderFooterView(SettingsViewRenderer.TextHeaderId) is not TextHeaderView headerView ) { return new UIView(); } // for HotReload
		// 	
		// 	headerView.Label.Text = _SettingsView.Model.GetSectionTitle((int) section);
		// 	headerView.Label.TextColor = _SettingsView.HeaderTextColor == Color.Default
		// 									 ? UIColor.Gray
		// 									 : _SettingsView.HeaderTextColor.ToUIColor();
		// 	headerView.Label.Font = FontUtility.CreateNativeFont(_SettingsView.HeaderFontFamily, (float) _SettingsView.HeaderFontSize, _SettingsView.HeaderFontAttributes);
		// 	//UIFont.SystemFontOfSize((nfloat)_settingsView.HeaderFontSize);
		// 	headerView.BackgroundView.BackgroundColor = _SettingsView.HeaderBackgroundColor.ToUIColor();
		// 	headerView.Label.Padding = _SettingsView.HeaderPadding.ToUIEdgeInsets();
		// 	
		// 	Section sec = _SettingsView.Model.GetSection((int) section);
		// 	headerView.SetVerticalAlignment(_SettingsView.HeaderTextVerticalAlign);
		// 	// if ( sec.HeaderHeight != -1 ||
		// 	// 	 _settingsView.HeaderHeight != -1 ) { headerView.SetVerticalAlignment(_settingsView.HeaderTextVerticalAlign); }
		// 	
		// 	return headerView;
		// }


		public override nfloat GetHeightForFooter( UITableView tableView, nint sectionId )
		{
			Section? section = _SettingsView.Model.GetSection((int) sectionId);
			if ( section is null ) throw new NullReferenceException(nameof(section));

			return section.IsVisible && section.FooterVisible
					   ? UITableView.AutomaticDimension
					   : nfloat.Epsilon;
		}
		public override UIView GetViewForFooter( UITableView tableView, nint sectionId )
		{
			Section? section = _SettingsView.Model.GetSection((int) sectionId);
			if ( section is null ) throw new NullReferenceException(nameof(section));
			return GetNativeSectionHeaderFooterView(section.FooterView, tableView, section);
		}
		// public override UIView GetViewForFooter( UITableView tableView, nint sectionId )
		// {
		// 	string text = _SettingsView.Model.GetFooterText((int) sectionId);
		// 	
		// 	if ( string.IsNullOrEmpty(text) ) { return new UIView(CGRect.Empty); }
		// 	
		// 	if ( tableView.DequeueReusableHeaderFooterView(SettingsViewRenderer.TextFooterId) is not TextFooterView footerView ) { return new UIView(); } // for HotReload
		// 	
		// 	footerView.Label.Text = text;
		// 	footerView.Label.TextColor = _SettingsView.FooterTextColor == Color.Default
		// 									 ? UIColor.Gray
		// 									 : _SettingsView.FooterTextColor.ToUIColor();
		// 	footerView.Label.Font = FontUtility.CreateNativeFont(_SettingsView.FooterFontFamily, (float) _SettingsView.FooterFontSize, _SettingsView.FooterFontAttributes);
		// 	//UIFont.SystemFontOfSize((nfloat)_settingsView.FooterFontSize);
		// 	footerView.BackgroundView.BackgroundColor = _SettingsView.FooterBackgroundColor.ToUIColor();
		// 	footerView.Label.Padding = _SettingsView.FooterPadding.ToUIEdgeInsets();
		// 	
		// 	return footerView;
		// }


		protected CustomHeaderView GetNativeSectionHeaderFooterView( ISectionHeader header, UITableView tableView, Section section )
		{
			var nativeView = (CustomHeaderView) tableView.DequeueReusableHeaderFooterView(SettingsViewRenderer.CustomHeaderId);
			nativeView.SetContent(header, section, tableView);

			return nativeView;
		}
		protected CustomFooterView GetNativeSectionHeaderFooterView( ISectionFooter footer, UITableView tableView, Section section )
		{
			var nativeView = (CustomFooterView) tableView.DequeueReusableHeaderFooterView(SettingsViewRenderer.CustomFooterId);
			nativeView.SetContent(footer, section, tableView);

			return nativeView;
		}

		public override nint RowsInSection( UITableView tableview, nint sectionId )
		{
			Section? section = _SettingsView.Model.GetSection((int) sectionId);
			return ( section?.IsVisible ?? false )
					   ? section.Count
					   : 0;
		}


		public override bool ShouldShowMenu( UITableView tableView, NSIndexPath rowAtIndexPath )
		{
			if ( _SettingsView.Model.GetSection(rowAtIndexPath.Section)?.UseDragSort ?? false ) { return false; }

			var ret = false;
			if ( tableView.CellAt(rowAtIndexPath) is BaseCellView  cell )
			{
				System.Diagnostics.Debug.WriteLine("LongTap");
				ret = cell.RowLongPressed(tableView, rowAtIndexPath);
			}

			if ( !ret ) return false;
			_SettingsView.Model.RowLongPressed(_SettingsView.Model.GetCell(rowAtIndexPath.Section, rowAtIndexPath.Row));
			BeginInvokeOnMainThread(async () =>
									{
										await Task.Delay(250);
										tableView.CellAt(rowAtIndexPath).SetSelected(false, true);
									}
								   );

			return true;
		}
		// TODO: what is this? what it do?
		public override bool CanPerformAction( UITableView tableView,
											   Selector action,
											   NSIndexPath indexPath,
											   NSObject? sender ) =>
			false;
		// TODO: what is this? what it do?
		public override void PerformAction( UITableView tableView,
											Selector action,
											NSIndexPath indexPath,
											NSObject? sender ) { }

		public override string[] SectionIndexTitles( UITableView tableView ) => _SettingsView.Model.GetSectionIndexTitles();

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			_SettingsView.Model.RowSelected(indexPath.Section, indexPath.Row);

			if ( tableView.CellAt(indexPath) is BaseCellView  cell ) { cell.RowSelected(tableView, indexPath); }
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing && !_disposed )
			{
				_SettingsView.ModelChanged -= OnSettingsViewOnModelChanged;
				_disposed = true;
			}


			base.Dispose(disposing);
		}
	}
}