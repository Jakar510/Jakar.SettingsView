[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]


namespace Jakar.SettingsView.iOS
{
    [Preserve(AllMembers = true)]
    public class SettingsViewRenderer : ViewRenderer<Shared.sv.SettingsView, UITableView>, IUITableViewDragDelegate, IUITableViewDropDelegate
    {
        // internal static string TextHeaderId { get; } = "textHeaderView";
        // internal static string TextFooterId { get; } = "textFooterView";
        internal static string CustomHeaderId { get; } = "customHeaderView";
        internal static string CustomFooterId { get; } = "customFooterView";


        protected Page?                 _ParentPage          { get; set; }
        protected KeyboardInsetTracker? _InsetTracker        { get; set; }
        protected UITableView?          _TableView           { get; set; }
        protected IDisposable?          _ContentSizeObserver { get; set; }

        protected       bool  _disposed;
        protected const float TOP_INSET = 36;

        protected override void OnElementChanged( ElementChangedEventArgs<Shared.sv.SettingsView> e )
        {
            base.OnElementChanged(e);

            if ( e.OldElement is not null )
            {
                e.OldElement.CollectionChanged        -= OnCollectionChanged;
                e.OldElement.SectionCollectionChanged -= OnSectionCollectionChanged;
                e.OldElement.SectionPropertyChanged   -= OnSectionPropertyChanged;
            }

            if ( e.NewElement is null ) return;
            _TableView = new UITableView(CGRect.Empty, UITableViewStyle.Grouped);

            if ( UIDevice.CurrentDevice.CheckSystemVersion(11, 0) )
            {
                _TableView.DragDelegate = this;
                _TableView.DropDelegate = this;

                _TableView.DragInteractionEnabled = true;
                _TableView.Source                 = new SettingsTableSource(Element);
            }
            else
            {
                _TableView.Editing                      = true;
                _TableView.AllowsSelectionDuringEditing = true;

                // When Editing is true, for some reason, UITableView top margin is displayed. force removing the margin by the following code.
                _TableView.ContentInset = new UIEdgeInsets(-TOP_INSET, 0, 0, 0);

                _TableView.Source = new SettingsLegacyTableSource(Element);
            }

            SetNativeControl(_TableView);
            _TableView.ScrollEnabled       = true;
            _TableView.RowHeight           = UITableView.AutomaticDimension;
            _TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            _TableView.CellLayoutMarginsFollowReadableWidth = false;

            _TableView.SectionHeaderHeight          = UITableView.AutomaticDimension;
            _TableView.EstimatedSectionHeaderHeight = UITableView.AutomaticDimension;

            _TableView.SectionFooterHeight          = UITableView.AutomaticDimension;
            _TableView.EstimatedSectionFooterHeight = UITableView.AutomaticDimension;

            // _TableView.RegisterClassForHeaderFooterViewReuse(typeof(TextHeaderView), TextHeaderId);
            // _TableView.RegisterClassForHeaderFooterViewReuse(typeof(TextFooterView), TextFooterId);

            _TableView.RegisterClassForHeaderFooterViewReuse(typeof(CustomHeaderView), CustomHeaderId);
            _TableView.RegisterClassForHeaderFooterViewReuse(typeof(CustomFooterView), CustomFooterId);

            e.NewElement.CollectionChanged        += OnCollectionChanged;
            e.NewElement.SectionCollectionChanged += OnSectionCollectionChanged;
            e.NewElement.SectionPropertyChanged   += OnSectionPropertyChanged;

            UpdateBackgroundColor();
            UpdateSeparator();
            UpdateRowHeight();

            Element elm = Element;

            while ( elm is not null )
            {
                elm = elm.Parent;
                if ( elm is not Page page ) continue;
                _ParentPage = page;
                break;
            }

            if ( _ParentPage is null ) throw new NullReferenceException(nameof(_ParentPage));
            _ParentPage.Appearing += ParentPageAppearing;

            _InsetTracker = new KeyboardInsetTracker(_TableView,
                                                     () => Control.Window,
                                                     insets => Control.ContentInset = Control.ScrollIndicatorInsets = insets,
                                                     point =>
                                                     {
                                                         CGPoint offset = Control.ContentOffset;
                                                         offset.Y += point.Y;
                                                         Control.SetContentOffset(offset, true);
                                                     }
                                                    );

            _ContentSizeObserver = _TableView.AddObserver(nameof(OnContentSizeChanged), NSKeyValueObservingOptions.New, OnContentSizeChanged);
        }

        protected void OnContentSizeChanged( NSObservedChange change ) { Element.VisibleContentHeight = Control.ContentSize.Height; }

        protected void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSections(e); }

        protected void OnSectionCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            int sectionIdx = Element.Model.GetSectionIndex((Section)sender);
            UpdateItems(e, sectionIdx, false);
        }

        protected void OnSectionPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            if ( sender is not Section section ) throw new InvalidOperationException(nameof(sender));

            if ( e.IsEqual(Section.isVisibleProperty) ) { UpdateSectionVisible(section); }
            else if ( e.IsOneOf(Section.titleProperty,
                                Section.textColorProperty,
                                Section.headerViewProperty,
                                Section.footerTextProperty,
                                Section.footerViewProperty
                               ) ) { UpdateSectionNoAnimation(section); }
            else if ( e.IsEqual(Section.footerVisibleProperty) ) { UpdateSectionFade(section); }
        }

        protected void UpdateSectionVisible( Section section )
        {
            // int secIndex = Element.Model.GetSectionIndex(section);
            // Control.BeginUpdates();
            // Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Automatic);
            // section.ShowVisibleCells();
            // Control.EndUpdates();
            Reload(section, UITableViewRowAnimation.Automatic);
        }

        protected void UpdateSectionNoAnimation( Section section )
        {
            // int secIndex = Element.Model.GetSectionIndex(section);
            // Control.BeginUpdates();
            // Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.None);
            // section.ShowVisibleCells();
            // Control.EndUpdates();
            Reload(section, UITableViewRowAnimation.None);
        }

        protected void UpdateSectionFade( Section section )
        {
            // int secIndex = Element.Model.GetSectionIndex(section);
            // Control.BeginUpdates();
            // Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Fade);
            // section.ShowVisibleCells();
            // Control.EndUpdates();
            Reload(section, UITableViewRowAnimation.Fade);
        }

        protected void Reload( Section section, UITableViewRowAnimation animation ) => Reload(section, NSIndexSet.FromIndex(Element.Model.GetSectionIndex(section)), animation);

        protected void Reload( Section section, NSIndexSet index, UITableViewRowAnimation animation )
        {
            Control.BeginUpdates();
            Control.ReloadSections(index, animation);
            section.ShowVisibleCells();
            Control.EndUpdates();
        }

        protected void Reload( NSIndexSet index, UITableViewRowAnimation animation )
        {
            Section? section = Element.Model.GetSection(index.FirstIndex.ToInt());
            if ( section is null ) throw new NullReferenceException(nameof(section));
            Reload(section, index, animation);
        }

        protected void Reload( int index, UITableViewRowAnimation animation ) => Reload(NSIndexSet.FromIndex(index), animation);
        protected void Reload( int index ) => Reload(NSIndexSet.FromIndex(index), UITableViewRowAnimation.Automatic);

        protected void UpdateSections( NotifyCollectionChangedEventArgs e )
        {
            switch ( e.Action )
            {
                case NotifyCollectionChangedAction.Add:
                    if ( e.NewStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    // Control.BeginUpdates();
                    // Control.InsertSections(NSIndexSet.FromIndex(e.NewStartingIndex), UITableViewRowAnimation.Automatic);
                    // Control.EndUpdates();
                    Reload(e.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    // Control.BeginUpdates();
                    // Control.DeleteSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
                    // Control.EndUpdates();
                    Reload(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    // Control.BeginUpdates();
                    // Control.ReloadSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
                    // Control.EndUpdates();
                    Reload(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:

                    Control.ReloadData();
                    return;

                default: throw new ArgumentOutOfRangeException(nameof(e.Action));
            }
        }

        protected void UpdateItems( NotifyCollectionChangedEventArgs e, int section, bool resetWhenGrouped )
        {
            // This means the UITableView hasn't rendered any cells yet
            // so there's no need to synchronize the rows on the UITableView
            if ( Control.IndexPathsForVisibleRows is null &&
                 e.Action != NotifyCollectionChangedAction.Reset )
                return;

            switch ( e.Action )
            {
                case NotifyCollectionChangedAction.Add:
                    if ( e.NewStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    Control.BeginUpdates();
                    Control.InsertRows(GetPaths(section, e.NewStartingIndex, e.NewItems.Count), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    Control.BeginUpdates();
                    Control.DeleteRows(GetPaths(section, e.OldStartingIndex, e.OldItems.Count), UITableViewRowAnimation.Automatic);
                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Move:
                    if ( e.OldStartingIndex == -1 ||
                         e.NewStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    Control.BeginUpdates();

                    for ( var i = 0; i < e.OldItems.Count; i++ )
                    {
                        int oldIndex = e.OldStartingIndex;
                        int newIndex = e.NewStartingIndex;

                        if ( e.NewStartingIndex < e.OldStartingIndex )
                        {
                            oldIndex += i;
                            newIndex += i;
                        }

                        Control.MoveRow(NSIndexPath.FromRowSection(oldIndex, section), NSIndexPath.FromRowSection(newIndex, section));
                    }

                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Replace:
                    if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

                    Control.BeginUpdates();
                    Control.ReloadRows(GetPaths(section, e.OldStartingIndex, e.OldItems.Count), UITableViewRowAnimation.None);
                    Control.EndUpdates();

                    break;

                case NotifyCollectionChangedAction.Reset:
                    Control.ReloadData();
                    return;

                default: throw new ArgumentOutOfRangeException(nameof(e.Action));
            }
        }

        protected virtual NSIndexPath[] GetPaths( int section, int index, int count )
        {
            var paths = new NSIndexPath[count];
            for ( var i = 0; i < paths.Length; i++ ) { paths[i] = NSIndexPath.FromRowSection(index + i, section); }

            return paths;
        }


        protected void ParentPageAppearing( object sender, EventArgs e ) { _TableView?.DeselectRow(_TableView.IndexPathForSelectedRow, true); }

        public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint ) =>
            Control.GetSizeRequest(widthConstraint, heightConstraint, SvConstants.Defaults.MIN_ROW_HEIGHT, SvConstants.Defaults.MIN_ROW_HEIGHT);

        protected override void UpdateNativeWidget()
        {
            if ( Element.Opacity < 1 )
            {
                if ( !Control.Layer.ShouldRasterize )
                {
                    Control.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
                    Control.Layer.ShouldRasterize    = true;
                }
            }
            else
                Control.Layer.ShouldRasterize = false;

            base.UpdateNativeWidget();
        }

        protected override void OnElementPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            if ( e.IsEqual(Shared.sv.SettingsView.separatorColorProperty) ) { UpdateSeparator(); }
            else if ( e.IsEqual(Shared.sv.SettingsView.backgroundColorProperty) ) { UpdateBackgroundColor(); }
            else if ( e.IsEqual(TableView.RowHeightProperty) ) { UpdateRowHeight(); }
            else if ( e.IsEqual(Shared.sv.SettingsView.scrollToTopProperty) ) { UpdateScrollToTop(); }
            else if ( e.IsEqual(Shared.sv.SettingsView.scrollToBottomProperty) ) { UpdateScrollToBottom(); }
            else { base.OnElementPropertyChanged(sender, e); }
        }


        protected void UpdateRowHeight()
        {
            if ( _TableView is null ) throw new NullReferenceException(nameof(_TableView));

            _TableView.EstimatedRowHeight = Math.Max(Element.RowHeight, SvConstants.Defaults.MIN_ROW_HEIGHT).ToNFloat();
            _TableView.ReloadData();
        }

        protected void UpdateBackgroundColor() { Control.BackgroundColor = Element.BackgroundColor.ToUIColor(); }
        protected void UpdateSeparator() { Control.SeparatorColor        = Element.SeparatorColor.ToUIColor(); }

        protected void UpdateScrollToTop()
        {
            if ( !Element.ScrollToTop ) return;
            if ( _TableView is null ) throw new NullReferenceException(nameof(_TableView));

            if ( _TableView.NumberOfSections() == 0 )
            {
                Element.ScrollToTop = false;
                return;
            }

            var  sectionIdx = 0;
            nint rows       = _TableView.NumberOfRowsInSection(sectionIdx);

            if ( rows > 0 )
            {
                _TableView.SetContentOffset(new CGPoint(0, TOP_INSET), false);

                //_tableview.ScrollToRow(NSIndexPath.Create(0, 0), UITableViewScrollPosition.Top, false);
            }

            Element.ScrollToTop = false;
        }

        protected void UpdateScrollToBottom()
        {
            if ( _TableView is null ) throw new NullReferenceException(nameof(_TableView));
            if ( !Element.ScrollToBottom ) return;

            nint sectionIdx = _TableView.NumberOfSections() - 1;

            if ( sectionIdx < 0 )
            {
                Element.ScrollToBottom = false;
                return;
            }

            nint rowIdx = _TableView.NumberOfRowsInSection(sectionIdx) - 1;

            if ( sectionIdx >= 0 &&
                 rowIdx >= 0 ) { _TableView.ScrollToRow(NSIndexPath.Create(sectionIdx, rowIdx), UITableViewScrollPosition.Top, false); }

            Element.ScrollToBottom = false;
        }

        protected override void Dispose( bool disposing )
        {
            if ( _ParentPage is not null ) _ParentPage.Appearing -= ParentPageAppearing;

            if ( _disposed ) { return; }

            if ( disposing )
            {
                _ContentSizeObserver?.Dispose();
                _ContentSizeObserver             =  null;
                Element.CollectionChanged        -= OnCollectionChanged;
                Element.SectionCollectionChanged -= OnSectionCollectionChanged;
                Element.SectionPropertyChanged   -= OnSectionPropertyChanged;
                _InsetTracker?.Dispose();
                _InsetTracker = null;
                foreach ( UIView subview in Subviews ) { DisposeSubviews(subview); }

                _TableView = null;
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        protected void DisposeSubviews( UIView view )
        {
            var ver = view as IVisualElementRenderer;

            if ( ver is null )
            {
                foreach ( UIView subView in view.Subviews ) { DisposeSubviews(subView); }

                view.RemoveFromSuperview();
            }

            view.Dispose();
        }

        public UIDragItem[] GetItemsForBeginningDragSession( UITableView tableView, IUIDragSession session, NSIndexPath indexPath )
        {
            Section? section = Element.Model.GetSection(indexPath.Section);

            if ( !( section?.UseDragSort ?? false ) )
            {
                return new UIDragItem[]
                       { };
            }

            Cell cell = Element.Model.GetCell(indexPath.Section, indexPath.Row);

            if ( !cell.IsEnabled )
            {
                return new UIDragItem[]
                       { };
            }

            // set "sectionIndex,rowIndex" as string
            NSData data = NSData.FromString($"{indexPath.Section},{indexPath.Row}");

            var itemProvider = new NSItemProvider();

            itemProvider.RegisterDataRepresentation(UTType.PlainText,
                                                    NSItemProviderRepresentationVisibility.All,
                                                    delegate( ItemProviderDataCompletionHandler completion_handler )
                                                    {
                                                        completion_handler(data, null!);
                                                        return null!;

                                                        // return new NSProgress();
                                                    }
                                                   );

            return new[]
                   {
                       new UIDragItem(itemProvider)
                   };
        }

        /// <summary>
        /// Performs the drop.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="coordinator">Coordinator.</param>
        public void PerformDrop( UITableView tableView, IUITableViewDropCoordinator coordinator )
        {
            NSIndexPath? destinationIndexPath = coordinator.DestinationIndexPath;
            if ( destinationIndexPath is null ) { return; }

            coordinator.Session.LoadObjects<NSString>(items =>
                                                      {
                                                          List<int> path = items[0]
                                                                          .ToString()
                                                                          .Split(new[]
                                                                                 {
                                                                                     ','
                                                                                 },
                                                                                 StringSplitOptions.None
                                                                                )
                                                                          .Select(int.Parse)
                                                                          .ToList();

                                                          int secIdx = path[0];
                                                          int rowIdx = path[1];


                                                          Section? section     = Element.Model.GetSection(secIdx);
                                                          Section? destSection = Element.Model.GetSection(destinationIndexPath.Section);
                                                          if ( section is null ) { return; }

                                                          if ( destSection is null ) { return; }

                                                          if ( !destSection.UseDragSort ) { return; }

                                                          // save scroll position
                                                          CGPoint         offset   = Control.ContentOffset;
                                                          UITableViewCell fromCell = Control.CellAt(NSIndexPath.FromRowSection(rowIdx, secIdx));

                                                          if ( section.ItemsSource is null )
                                                          {
                                                              // Don't use PerformBatchUpdates. Because can't cancel animations well.
                                                              Control.BeginUpdates();

                                                              Cell cell = section.DeleteCellWithoutNotify(rowIdx);
                                                              destSection.InsertCellWithoutNotify(cell, destinationIndexPath.Row);
                                                              Control.DeleteRows(GetPaths(secIdx,                       rowIdx,                   1), UITableViewRowAnimation.None);
                                                              Control.InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

                                                              Control.EndUpdates();

                                                              Element.SendItemDropped(destSection, cell);
                                                          }
                                                          else
                                                          {
                                                              // Don't use PerformBatchUpdates. Because can't cancel animations well.
                                                              Control.BeginUpdates();

                                                              ( Cell? cell, object? item ) = section.DeleteSourceItemWithoutNotify(rowIdx);
                                                              destSection.InsertSourceItemWithoutNotify(cell, item, destinationIndexPath.Row);
                                                              Control.DeleteRows(GetPaths(secIdx,                       rowIdx,                   1), UITableViewRowAnimation.None);
                                                              Control.InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

                                                              Control.EndUpdates();
                                                              if ( cell is not null ) Element.SendItemDropped(destSection, cell);
                                                          }

                                                          // Cancel animations and restore the scroll position.
                                                          UITableViewCell toCell = Control.CellAt(destinationIndexPath);
                                                          toCell.Layer.RemoveAllAnimations();
                                                          fromCell.Layer.RemoveAllAnimations();
                                                          Control.Layer.RemoveAllAnimations();
                                                          Control.SetContentOffset(offset, false);

                                                          // nothing occur, even if use the following code.
                                                          //coordinator.DropItemToRow(coordinator.Items.First().DragItem, destinationIndexPath);
                                                      }
                                                     );
        }

        [Export("tableView:canHandleDropSession:")]
        public bool CanHandleDropSession( UITableView tableView, IUIDropSession session ) => session.CanLoadObjects(typeof(NSString));

        [Export("tableView:dropSessionDidEnter:")]
        public void DropSessionDidEnter( UITableView tableView, IUIDropSession session ) { }

        [Export("tableView:dropSessionDidEnd:")]
        public void DropSessionDidEnd( UITableView tableView, IUIDropSession session ) { }

        [Export("tableView:dropSessionDidExit:")]
        public void DropSessionDidExit( UITableView tableView, IUIDropSession session ) { }

        [Export("tableView:dropSessionDidUpdate:withDestinationIndexPath:")]
        public UITableViewDropProposal DropSessionDidUpdate( UITableView tableView, IUIDropSession session, NSIndexPath destinationIndexPath )
        {
            // this dragging is from UITableView.
            if ( !tableView.HasActiveDrag ) return new UITableViewDropProposal(UIDropOperation.Cancel);

            return session.Items.Length > 1
                       ? new UITableViewDropProposal(UIDropOperation.Cancel)
                       : new UITableViewDropProposal(UIDropOperation.Move, UITableViewDropIntent.Automatic);
        }
    }
}
