using System;
using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.Shared.Cells;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#nullable enable
[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

	[Preserve(AllMembers = true)]
	public class PickerCellView : LabelCellView
	{
		private PickerCell _PickerCell => Cell as PickerCell ?? throw new NullReferenceException(nameof(_PickerCell));
		private PickerTableViewController? _PickerVC { get; set; }
		private INotifyCollectionChanged? _NotifyCollection { get; set; }
		private INotifyCollectionChanged? _SelectedCollection { get; set; }


		public PickerCellView( Cell formsCell ) : base(formsCell)
		{
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			SetRightMarginZero(_MainStack);
		}


		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);
			if ( e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
				 e.PropertyName == PickerCell.SelectedItemProperty.PropertyName ||
				 e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
				 e.PropertyName == PickerCell.UseNaturalSortProperty.PropertyName ||
				 e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName ) { UpdateSelectedItems(); }

			if ( e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems();
			}
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( _PickerCell.ItemsSource is null )
			{
				tableView.DeselectRow(indexPath, true);
				return;
			}

			_PickerVC?.Dispose();

			UINavigationController? navigationController = GetUINavigationController(UIApplication.SharedApplication.KeyWindow.RootViewController);
			if ( navigationController is ShellSectionRenderer shell )
			{
				// When use Shell, the NativeView is wrapped in a Forms.ContentPage.
				_PickerVC = new PickerTableViewController(this, tableView, shell.ShellSection.Navigation)
							{
								TableView =
								{
									ContentInset = new UIEdgeInsets(44, 0, 44, 0)
								}
							};

				// Fix height broken. For some reason, TableView ContentSize is broken.
				var page = new ContentPage
						   {
							   Content = _PickerVC.TableView.ToView(),
							   Title = _PickerCell.Prompt.Properties.Title
						   };


				// Fire manually because INavigation.PushAsync does not work ViewDidAppear and ViewWillAppear.
				_PickerVC.ViewDidAppear(false);
				_PickerVC.InitializeView();
				BeginInvokeOnMainThread(async () =>
										{
											await shell.ShellSection.Navigation.PushAsync(page, true);
											_PickerVC.InitializeScroll();
										}
									   );
			}
			else
			{
				// When use traditional navigation.
				_PickerVC = new PickerTableViewController(this, tableView);
				BeginInvokeOnMainThread(() => navigationController?.PushViewController(_PickerVC, true));
			}


			if ( !_PickerCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		protected class NavDelegate : UINavigationControllerDelegate
		{
			protected ShellSectionRenderer _Self { get; set; }

			public NavDelegate( ShellSectionRenderer renderer ) => _Self = renderer;

			public override void DidShowViewController( UINavigationController navigationController, [Transient] UIViewController viewController, bool animated ) { }

			public override void WillShowViewController( UINavigationController navigationController, [Transient] UIViewController viewController, bool animated ) { navigationController.SetNavigationBarHidden(false, true); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdateSelectedItems();
			UpdateCollectionChanged();
		}

		public void UpdateSelectedItems()
		{
			if ( _SelectedCollection is not null ) { _SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

			_SelectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;

			if ( _SelectedCollection is not null ) { _SelectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }

			ValueLabel.Text = _PickerCell.GetSelectedItemsText();
		}

		private void UpdateCollectionChanged()
		{
			if ( _NotifyCollection is not null ) { _NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			_NotifyCollection = _PickerCell.ItemsSource as INotifyCollectionChanged;

			if ( _NotifyCollection is not null )
			{
				_NotifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
				ItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		protected override void UpdateIsEnabled()
		{
			if ( _PickerCell.ItemsSource is not null &&
				 _PickerCell.ItemsSource.Count == 0 ) { return; }

			base.UpdateIsEnabled();
		}

		private void ItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( _CellBase is null ) { throw new NullReferenceException(nameof(_CellBase)); }

			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource?.Count > 0);
		}

		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_PickerVC?.Dispose();
				_PickerVC = null;

				if ( _NotifyCollection is not null )
				{
					_NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
					_NotifyCollection = null;
				}

				if ( _SelectedCollection is not null )
				{
					_SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
					_SelectedCollection = null;
				}
			}

			base.Dispose(disposing);
		}

		// Refer to https://forums.xamarin.com/discussion/comment/294088/#Comment_294088
		protected static UINavigationController? GetUINavigationController( UIViewController? controller )
		{
			// if ( controller is null ) return null;

			// if ( controller.PresentedViewController is not null ) { return GetUINavigationController(controller.PresentedViewController); } // on modal page

			return controller switch
				   {
					   null => null,
					   UINavigationController navigation => navigation,
					   UITabBarController tabBar => GetUINavigationController(tabBar.SelectedViewController), // in case Root->Tab->Navi->Page
					   // ReSharper disable once ConditionIsAlwaysTrueOrFalse
					   { } when controller.PresentedViewController is not null => GetUINavigationController(controller.PresentedViewController),
					   _ => ( from child in controller.ChildViewControllers let result = GetUINavigationController(child) where child is not null select result ).FirstOrDefault()
				   };

			// var count = controller.ChildViewControllers.Count();
			// for ( int c = 0; c < count; c++ )
			// {
			// 	UINavigationController? child = GetUINavigationController(controller.ChildViewControllers[c]);
			// 	if ( child is not null ) { return child; }
			// }
		}
	}
}