using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CoreGraphics;
using Foundation;
using Jakar.SettingsView;
using Jakar.SettingsView.iOS;
using Jakar.SettingsView.Shared;
using MobileCoreServices;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SettingsView), typeof(SettingsViewRenderer))]

namespace Jakar.SettingsView.iOS
{
	/// <summary>
	/// Settings view renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class SettingsViewRenderer : ViewRenderer<Shared.SettingsView, UITableView>, IUITableViewDragDelegate, IUITableViewDropDelegate
	{
		internal static readonly string TextHeaderId = "textHeaderView";
		internal static readonly string TextFooterId = "textFooterView";
		internal static readonly string CustomHeaderId = "customHeaderView";
		internal static readonly string CustomFooterId = "customFooterView";
		private Page _parentPage;
		private KeyboardInsetTracker _insetTracker;
		internal static float MinRowHeight = 48;
		private UITableView _tableview;
		private IDisposable _contentSizeObserver;

		private bool _disposed;
		private float _topInset;

		/// <summary>
		/// Ons the element changed.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged( ElementChangedEventArgs<Shared.SettingsView> e )
		{
			base.OnElementChanged(e);

			if ( e.OldElement != null )
			{
				e.OldElement.CollectionChanged -= OnCollectionChanged;
				e.OldElement.SectionCollectionChanged -= OnSectionCollectionChanged;
				e.OldElement.SectionPropertyChanged -= OnSectionPropertyChanged;
			}

			if ( e.NewElement != null )
			{
				_tableview = new UITableView(CGRect.Empty, UITableViewStyle.Grouped);

				if ( UIDevice.CurrentDevice.CheckSystemVersion(11, 0) )
				{
					_tableview.DragDelegate = this;
					_tableview.DropDelegate = this;

					_tableview.DragInteractionEnabled = true;
					_tableview.Source = new SettingsTableSource(Element);
				}
				else
				{
					_tableview.Editing = true;
					_tableview.AllowsSelectionDuringEditing = true;
					// When Editing is true, for some reason, UITableView top margin is displayed.
					// force removing the margin by the following code.
					_topInset = 36;
					_tableview.ContentInset = new UIEdgeInsets(-_topInset, 0, 0, 0);

					_tableview.Source = new SettingsLagacyTableSource(Element);
				}

				SetNativeControl(_tableview);
				_tableview.ScrollEnabled = true;
				_tableview.RowHeight = UITableView.AutomaticDimension;
				_tableview.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

				_tableview.CellLayoutMarginsFollowReadableWidth = false;

				_tableview.SectionHeaderHeight = UITableView.AutomaticDimension;
				_tableview.EstimatedSectionHeaderHeight = UITableView.AutomaticDimension;

				_tableview.SectionFooterHeight = UITableView.AutomaticDimension;
				_tableview.EstimatedSectionFooterHeight = UITableView.AutomaticDimension;

				_tableview.RegisterClassForHeaderFooterViewReuse(typeof(TextHeaderView), TextHeaderId);
				_tableview.RegisterClassForHeaderFooterViewReuse(typeof(TextFooterView), TextFooterId);
				_tableview.RegisterClassForHeaderFooterViewReuse(typeof(CustomHeaderView), CustomHeaderId);
				_tableview.RegisterClassForHeaderFooterViewReuse(typeof(CustomFooterView), CustomFooterId);

				e.NewElement.CollectionChanged += OnCollectionChanged;
				e.NewElement.SectionCollectionChanged += OnSectionCollectionChanged;
				e.NewElement.SectionPropertyChanged += OnSectionPropertyChanged;

				UpdateBackgroundColor();
				UpdateSeparator();
				UpdateRowHeight();

				Element elm = Element;
				while ( elm != null )
				{
					elm = elm.Parent;
					if ( elm is Page ) { break; }
				}

				_parentPage = elm as Page;
				_parentPage.Appearing += ParentPageAppearing;

				_insetTracker = new KeyboardInsetTracker(_tableview, () => Control.Window, insets => Control.ContentInset = Control.ScrollIndicatorInsets = insets, point =>
																																									{
																																										CGPoint offset = Control.ContentOffset;
																																										offset.Y += point.Y;
																																										Control.SetContentOffset(offset, true);
																																									});

				_contentSizeObserver = _tableview.AddObserver("contentSize", NSKeyValueObservingOptions.New, OnContentSizeChanged);
			}
		}

		private void OnContentSizeChanged( NSObservedChange change ) { Element.VisibleContentHeight = Control.ContentSize.Height; }

		private void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSections(e); }

		private void OnSectionCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			int sectionIdx = Element.Model.GetSectionIndex((Section) sender);
			UpdateItems(e, sectionIdx, false);
		}

		private void OnSectionPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == Section.IsVisibleProperty.PropertyName ) { UpdateSectionVisible((Section) sender); }
			else if ( e.PropertyName == TableSectionBase.TitleProperty.PropertyName ||
					  e.PropertyName == Section.HeaderViewProperty.PropertyName ||
					  e.PropertyName == Section.HeaderHeightProperty.PropertyName ||
					  e.PropertyName == Section.FooterTextProperty.PropertyName ||
					  e.PropertyName == Section.FooterViewProperty.PropertyName ) { UpdateSectionNoAnimation((Section) sender); }
			else if ( e.PropertyName == Section.FooterVisibleProperty.PropertyName ) { UpdateSectionFade((Section) sender); }
		}

		private void UpdateSectionVisible( Section section )
		{
			int secIndex = Element.Model.GetSectionIndex(section);
			Control.BeginUpdates();
			Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Automatic);
			Control.EndUpdates();
		}

		private void UpdateSectionNoAnimation( Section section )
		{
			int secIndex = Element.Model.GetSectionIndex(section);
			Control.BeginUpdates();
			Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.None);
			Control.EndUpdates();
		}

		private void UpdateSectionFade( Section section )
		{
			int secIndex = Element.Model.GetSectionIndex(section);
			Control.BeginUpdates();
			Control.ReloadSections(NSIndexSet.FromIndex(secIndex), UITableViewRowAnimation.Fade);
			Control.EndUpdates();
		}


		private void UpdateSections( NotifyCollectionChangedEventArgs e )
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
					if ( e.NewStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

					Control.BeginUpdates();
					Control.InsertSections(NSIndexSet.FromIndex(e.NewStartingIndex), UITableViewRowAnimation.Automatic);
					Control.EndUpdates();
					break;
				case NotifyCollectionChangedAction.Remove:
					if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

					Control.BeginUpdates();
					Control.DeleteSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
					Control.EndUpdates();
					break;

				case NotifyCollectionChangedAction.Replace:
					if ( e.OldStartingIndex == -1 ) { goto case NotifyCollectionChangedAction.Reset; }

					Control.BeginUpdates();
					Control.ReloadSections(NSIndexSet.FromIndex(e.OldStartingIndex), UITableViewRowAnimation.Automatic);
					Control.EndUpdates();
					break;

				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Reset:

					Control.ReloadData();
					return;
			}
		}

		private void UpdateItems( NotifyCollectionChangedEventArgs e, int section, bool resetWhenGrouped )
		{
			// This means the UITableView hasn't rendered any cells yet
			// so there's no need to synchronize the rows on the UITableView
			if ( Control.IndexPathsForVisibleRows == null &&
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
						int oldi = e.OldStartingIndex;
						int newi = e.NewStartingIndex;

						if ( e.NewStartingIndex < e.OldStartingIndex )
						{
							oldi += i;
							newi += i;
						}

						Control.MoveRow(NSIndexPath.FromRowSection(oldi, section), NSIndexPath.FromRowSection(newi, section));
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
			}
		}

		protected virtual NSIndexPath[] GetPaths( int section, int index, int count )
		{
			var paths = new NSIndexPath[count];
			for ( var i = 0; i < paths.Length; i++ ) { paths[i] = NSIndexPath.FromRowSection(index + i, section); }

			return paths;
		}


		private void ParentPageAppearing( object sender, EventArgs e ) { _tableview.DeselectRow(_tableview.IndexPathForSelectedRow, true); }

		/// <summary>
		/// Gets the size of the desired.
		/// </summary>
		/// <returns>The desired size.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="heightConstraint">Height constraint.</param>
		public override SizeRequest GetDesiredSize( double widthConstraint, double heightConstraint ) => Control.GetSizeRequest(widthConstraint, heightConstraint, MinRowHeight, MinRowHeight);

		/// <summary>
		/// Updates the native widget.
		/// </summary>
		protected override void UpdateNativeWidget()
		{
			if ( Element.Opacity < 1 )
			{
				if ( !Control.Layer.ShouldRasterize )
				{
					Control.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
					Control.Layer.ShouldRasterize = true;
				}
			}
			else
				Control.Layer.ShouldRasterize = false;

			base.UpdateNativeWidget();
		}

		/// <summary>
		/// Ons the element property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged(sender, e);
			if ( e.PropertyName == Shared.SettingsView.SeparatorColorProperty.PropertyName ) { UpdateSeparator(); }
			else if ( e.PropertyName == Shared.SettingsView.BackgroundColorProperty.PropertyName ) { UpdateBackgroundColor(); }
			else if ( e.PropertyName == TableView.RowHeightProperty.PropertyName ) { UpdateRowHeight(); }
			else if ( e.PropertyName == Shared.SettingsView.ScrollToTopProperty.PropertyName ) { UpdateScrollToTop(); }
			else if ( e.PropertyName == Shared.SettingsView.ScrollToBottomProperty.PropertyName ) { UpdateScrollToBottom(); }
		}


		private void UpdateRowHeight()
		{
			_tableview.EstimatedRowHeight = Math.Max((float) Element.RowHeight, MinRowHeight);
			_tableview.ReloadData();
		}

		private void UpdateBackgroundColor()
		{
			Color color = Element.BackgroundColor;
			if ( color != Color.Default ) { Control.BackgroundColor = color.ToUIColor(); }
		}

		private void UpdateSeparator()
		{
			Color color = Element.SeparatorColor;
			Control.SeparatorColor = color.ToUIColor();
		}

		private void UpdateScrollToTop()
		{
			if ( Element.ScrollToTop )
			{
				if ( _tableview.NumberOfSections() == 0 )
				{
					Element.ScrollToTop = false;
					return;
				}

				var sectionIdx = 0;
				nint rows = _tableview.NumberOfRowsInSection(sectionIdx);
				if ( rows > 0 )
				{
					_tableview.SetContentOffset(new CGPoint(0, _topInset), false);
					//_tableview.ScrollToRow(NSIndexPath.Create(0, 0), UITableViewScrollPosition.Top, false);
				}

				Element.ScrollToTop = false;
			}
		}

		private void UpdateScrollToBottom()
		{
			if ( Element.ScrollToBottom )
			{
				nint sectionIdx = _tableview.NumberOfSections() - 1;
				if ( sectionIdx < 0 )
				{
					Element.ScrollToBottom = false;
					return;
				}

				nint rowIdx = _tableview.NumberOfRowsInSection(sectionIdx) - 1;

				if ( sectionIdx >= 0 &&
					 rowIdx >= 0 ) { _tableview.ScrollToRow(NSIndexPath.Create(sectionIdx, rowIdx), UITableViewScrollPosition.Top, false); }

				Element.ScrollToBottom = false;
			}
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			_parentPage.Appearing -= ParentPageAppearing;

			if ( _disposed ) { return; }

			if ( disposing )
			{
				_contentSizeObserver.Dispose();
				_contentSizeObserver = null;
				Element.CollectionChanged -= OnCollectionChanged;
				Element.SectionCollectionChanged -= OnSectionCollectionChanged;
				Element.SectionPropertyChanged -= OnSectionPropertyChanged;
				_insetTracker?.Dispose();
				_insetTracker = null;
				foreach ( UIView subview in Subviews ) { DisposeSubviews(subview); }

				_tableview = null;
			}

			_disposed = true;

			base.Dispose(disposing);
		}

		private void DisposeSubviews( UIView view )
		{
			var ver = view as IVisualElementRenderer;

			if ( ver == null )
			{
				foreach ( UIView subView in view.Subviews ) { DisposeSubviews(subView); }

				view.RemoveFromSuperview();
			}

			view.Dispose();
		}

		/// <summary>
		/// Gets the items for beginning drag session.
		/// </summary>
		/// <returns>The items for beginning drag session.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="session">Session.</param>
		/// <param name="indexPath">Index path.</param>
		public UIDragItem[] GetItemsForBeginningDragSession( UITableView tableView, IUIDragSession session, NSIndexPath indexPath )
		{
			Section section = Element.Model.GetSection(indexPath.Section);
			if ( !section.UseDragSort )
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
			itemProvider.RegisterDataRepresentation(UTType.PlainText, NSItemProviderRepresentationVisibility.All, ( completionHandler ) =>
																												  {
																													  completionHandler(data, null);
																													  return null;
																												  });

			return new UIDragItem[]
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
			if ( destinationIndexPath == null ) { return; }

			coordinator.Session.LoadObjects<NSString>(items =>
													  {
														  List<int> path = items[0]
																		   .ToString()
																		   .Split(new char[]
																				  {
																					  ','
																				  }, StringSplitOptions.None)
																		   .Select(x => int.Parse(x))
																		   .ToList();
														  int secIdx = path[0];
														  int rowIdx = path[1];


														  Section section = Element.Model.GetSection(secIdx);
														  Section destSection = Element.Model.GetSection(destinationIndexPath.Section);
														  if ( !destSection.UseDragSort ) { return; }

														  // save scroll position
														  CGPoint offset = Control.ContentOffset;
														  UITableViewCell fromCell = Control.CellAt(NSIndexPath.FromRowSection(rowIdx, secIdx));

														  if ( section.ItemsSource == null )
														  {
															  // Don't use PerformBatchUpdates. Because can't cancel animations well.
															  Control.BeginUpdates();

															  Cell cell = section.DeleteCellWithoutNotify(rowIdx);
															  destSection.InsertCellWithoutNotify(cell, destinationIndexPath.Row);
															  Control.DeleteRows(GetPaths(secIdx, rowIdx, 1), UITableViewRowAnimation.None);
															  Control.InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

															  Control.EndUpdates();

															  Element.SendItemDropped(destSection, cell);
														  }
														  else
														  {
															  // Don't use PerformBatchUpdates. Because can't cancel animations well.
															  Control.BeginUpdates();

															  (Cell Cell, object Item) deletedSet = section.DeleteSourceItemWithoutNotify(rowIdx);
															  destSection.InsertSourceItemWithoutNotify(deletedSet.Cell, deletedSet.Item, destinationIndexPath.Row);
															  Control.DeleteRows(GetPaths(secIdx, rowIdx, 1), UITableViewRowAnimation.None);
															  Control.InsertRows(GetPaths(destinationIndexPath.Section, destinationIndexPath.Row, 1), UITableViewRowAnimation.None);

															  Control.EndUpdates();
															  Element.SendItemDropped(destSection, deletedSet.Cell);
														  }

														  // Cancel animations and restore the scroll position.
														  UITableViewCell toCell = Control.CellAt(destinationIndexPath);
														  toCell?.Layer?.RemoveAllAnimations();
														  fromCell?.Layer?.RemoveAllAnimations();
														  Control.Layer.RemoveAllAnimations();
														  Control.SetContentOffset(offset, false);

														  // nothing occur, even if use the following code.
														  //coordinator.DropItemToRow(coordinator.Items.First().DragItem, destinationIndexPath);
													  });
		}

		/// <summary>
		/// Ensure that the drop session contains a drag item with a data representation that the view can consume.
		/// </summary>
		[Export("tableView:canHandleDropSession:")]
		public bool CanHandleDropSession( UITableView tableView, IUIDropSession session ) => session.CanLoadObjects(typeof(NSString));

		[Export("tableView:dropSessionDidEnter:")]
		public void DropSessionDidEnter( UITableView tableView, IUIDropSession session ) { }

		[Export("tableView:dropSessionDidEnd:")]
		public void DropSessionDidEnd( UITableView tableView, IUIDropSession session ) { }

		[Export("tableView:dropSessionDidExit:")]
		public void DropSessionDidExit( UITableView tableView, IUIDropSession session ) { }

		/// <summary>
		/// A drop proposal from a table view includes two items: a drop operation,
		/// typically .move or .copy; and an intent, which declares the action the
		/// table view will take upon receiving the items. (A drop proposal from a
		/// custom view does includes only a drop operation, not an intent.)
		/// </summary>
		[Export("tableView:dropSessionDidUpdate:withDestinationIndexPath:")]
		public UITableViewDropProposal DropSessionDidUpdate( UITableView tableView, IUIDropSession session, NSIndexPath destinationIndexPath )
		{
			if ( destinationIndexPath == null ) { return new UITableViewDropProposal(UIDropOperation.Cancel); }

			// this dragging is from UITableView.
			if ( tableView.HasActiveDrag )
			{
				if ( session.Items.Length > 1 ) { return new UITableViewDropProposal(UIDropOperation.Cancel); }
				else { return new UITableViewDropProposal(UIDropOperation.Move, UITableViewDropIntent.Automatic); }
			}

			return new UITableViewDropProposal(UIDropOperation.Cancel);
		}
	}
}