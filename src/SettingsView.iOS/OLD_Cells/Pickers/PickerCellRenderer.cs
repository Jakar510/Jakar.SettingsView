using System.Collections.Specialized;
using System.Linq;
using Foundation;
using GameKit;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// [assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
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

			if ( e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName )
			{
				if ( _PickerCell.UseAutoValueText ) { UpdateSelectedItems(); }
				else { UpdateValueText(); }
			}

			if ( e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems();
			}
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( _PickerCell.ItemsSource == null )
			{
				tableView.DeselectRow(indexPath, true);
				return;
			}

			_pickerVC?.Dispose();

			UINavigationController naviCtrl = GetUINavigationController(UIApplication.SharedApplication.KeyWindow.RootViewController);
			if ( naviCtrl is ShellSectionRenderer shell )
			{
				// When use Shell, the NativeView is wrapped in a Forms.ContentPage.
				_pickerVC = new PickerTableViewController(this, tableView, shell.ShellSection.Navigation);
				// Fix height broken. For some reason, TableView ContentSize is broken.
				_pickerVC.TableView.ContentInset = new UIEdgeInsets(44, 0, 44, 0);
				var page = new ContentPage();
				page.Content = _pickerVC.TableView.ToView();
				;
				page.Title = _PickerCell.PageTitle;

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

			public NavDelegate( ShellSectionRenderer renderer ) => _self = renderer;

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
			if ( !_PickerCell.UseAutoValueText ) { return; }

			if ( _selectedCollection != null ) { _selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

			_selectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;

			if ( _selectedCollection != null ) { _selectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }

			ValueLabel.Text = _PickerCell.GetSelectedItemsText();
		}

		private void UpdateCollectionChanged()
		{
			if ( _notifyCollection != null ) { _notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			_notifyCollection = _PickerCell.ItemsSource as INotifyCollectionChanged;

			if ( _notifyCollection != null )
			{
				_notifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
				ItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		protected override void UpdateIsEnabled()
		{
			if ( _PickerCell.ItemsSource != null &&
				 _PickerCell.ItemsSource.Count == 0 ) { return; }

			base.UpdateIsEnabled();
		}

		private void ItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}

		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_pickerVC?.Dispose();
				_pickerVC = null;

				if ( _notifyCollection != null )
				{
					_notifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
					_notifyCollection = null;
				}

				if ( _selectedCollection != null )
				{
					_selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
					_selectedCollection = null;
				}
			}

			base.Dispose(disposing);
		}

#nullable enable
		private UINavigationController? GetUINavigationController( UIViewController? controller )
		{
			// Refer to https://forums.xamarin.com/discussion/comment/294088/#Comment_294088
			switch ( controller )
			{
				case null: return null;

				case UINavigationController navigation: return navigation;

				case UITabBarController tabBarController:
				{
					//in case Root->Tab->Navi->Page
					return GetUINavigationController(tabBarController.SelectedViewController);
				}

				default:
				{
					if ( controller.PresentedViewController is UINavigationController navigationCtl )
					{
						// on modal page
						return GetUINavigationController(navigationCtl);
					}

					break;
				}
			}

			return controller.ChildViewControllers.Any()
					   ? controller.ChildViewControllers.Select(GetUINavigationController).FirstOrDefault(child => child is not null)
					   : null;
		}
	}
}