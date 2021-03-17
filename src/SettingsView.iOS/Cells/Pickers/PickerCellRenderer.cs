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

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)]
	public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

	[Preserve(AllMembers = true)]
	public class PickerCellView : LabelCellView
	{
		private PickerCell _PickerCell => Cell as PickerCell;
		private PickerTableViewController _pickerVC;
		private INotifyCollectionChanged _notifyCollection;
		private INotifyCollectionChanged _selectedCollection;

		public PickerCellView( Cell formsCell ) : base(formsCell)
		{
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			SetRightMarginZero();
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

			_pickerVC?.Dispose();

			var naviCtrl = GetUINavigationController(UIApplication.SharedApplication.KeyWindow.RootViewController);
			if ( naviCtrl is ShellSectionRenderer shell )
			{
				// When use Shell, the NativeView is wrapped in a Forms.ContentPage.
				_pickerVC = new PickerTableViewController(this, tableView, shell.ShellSection.Navigation);
				// Fix height broken. For some reason, TableView ContentSize is broken.
				_pickerVC.TableView.ContentInset = new UIEdgeInsets(44, 0, 44, 0);
				var page = new ContentPage();
				page.Content = _pickerVC.TableView.ToView();
				;
				page.Title = _PickerCell.Prompt.Properties.Title;

				// Fire manually because INavigation.PushAsync does not work ViewDidAppear and ViewWillAppear.
				_pickerVC.ViewDidAppear(false);
				_pickerVC.InitializeView();
				BeginInvokeOnMainThread(async () =>
										{
											await shell.ShellSection.Navigation.PushAsync(page, true);
											_pickerVC.InitializeScroll();
										}
									   );
			}
			else
			{
				// When use traditional navigation.
				_pickerVC = new PickerTableViewController(this, tableView);
				BeginInvokeOnMainThread(() => naviCtrl.PushViewController(_pickerVC, true));
			}


			if ( !_PickerCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		private class NavDelegate : UINavigationControllerDelegate
		{
			private readonly ShellSectionRenderer _self;

			public NavDelegate( ShellSectionRenderer renderer ) { _self = renderer; }

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
			if ( _selectedCollection is not null ) { _selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

			_selectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;

			if ( _selectedCollection is not null ) { _selectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }

			ValueLabel.Text = _PickerCell.GetSelectedItemsText();
		}

		private void UpdateCollectionChanged()
		{
			if ( _notifyCollection is not null ) { _notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			_notifyCollection = _PickerCell.ItemsSource as INotifyCollectionChanged;

			if ( _notifyCollection is not null )
			{
				_notifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
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
			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}

		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_pickerVC?.Dispose();
				_pickerVC = null;

				if ( _notifyCollection is not null )
				{
					_notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
					_notifyCollection = null;
				}

				if ( _selectedCollection is not null )
				{
					_selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
					_selectedCollection = null;
				}
			}

			base.Dispose(disposing);
		}

		// Refer to https://forums.xamarin.com/discussion/comment/294088/#Comment_294088
		private UINavigationController GetUINavigationController( UIViewController controller )
		{
			if ( controller is not null )
			{
				if ( controller.PresentedViewController is not null )
				{
					// on modal page
					return GetUINavigationController(controller.PresentedViewController);
				}

				if ( controller is UINavigationController ) { return ( controller as UINavigationController ); }

				if ( controller is UITabBarController )
				{
					//in case Root->Tab->Navi->Page
					var tabCtrl = controller as UITabBarController;
					return GetUINavigationController(tabCtrl.SelectedViewController);
				}

				if ( controller.ChildViewControllers.Count() != 0 )
				{
					var count = controller.ChildViewControllers.Count();

					for ( int c = 0; c < count; c++ )
					{
						var child = GetUINavigationController(controller.ChildViewControllers[c]);
						if ( child is null )
						{
							//TODO: Analytics...
						}
						else if ( child is UINavigationController ) { return ( child as UINavigationController ); }
					}
				}
			}

			return null;
		}
	}
}