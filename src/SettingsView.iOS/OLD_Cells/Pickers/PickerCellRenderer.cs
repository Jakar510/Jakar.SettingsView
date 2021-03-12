using System.Collections.Specialized;
using System.Linq;
using Foundation;
using GameKit;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.iOS.OLD_Cells;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Misc;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// [assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.OLD_Cells
{
	// [Foundation.Preserve(AllMembers = true)] public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

	[Foundation.Preserve(AllMembers = true)]
	public class PickerCellView : LabelCellView
	{
		protected PickerCell _PickerCell => Cell as PickerCell;
		protected PickerTableViewController? _PickerVC { get; set; }
		protected INotifyCollectionChanged _NotifyCollection { get; set; }
		protected INotifyCollectionChanged _SelectedCollection { get; set; }


		public PickerCellView( Cell formsCell ) : base(formsCell)
		{
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			SetRightMarginZero();
		}

		public override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.IsOneOf(PickerCell.SelectedItemsProperty,
						   PickerCell.SelectedItemProperty,
						   PickerCell.DisplayMemberProperty,
						   PickerCell.UseNaturalSortProperty,
						   PickerCell.SelectedItemsOrderKeyProperty
						  ) ) { UpdateSelectedItems(); }

			//else if ( e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName )
			// {
			// 	if ( _PickerCell.UseAutoValueText ) { UpdateSelectedItems(); }
			// 	else { UpdateValueText(); }
			// }

			else if ( e.IsEqual(PickerCell.ItemsSourceProperty.PropertyName) )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems();
			}
			else { base.CellPropertyChanged(sender, e); }
		}

		public override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( _PickerCell.ItemsSource == null )
			{
				tableView.DeselectRow(indexPath, true);
				return;
			}

			_PickerVC?.Dispose();

			UINavigationController? navigationController = GetUINavigationController(UIApplication.SharedApplication.KeyWindow.RootViewController);
			if ( navigationController is ShellSectionRenderer shell )
			{
				// When use Shell, the NativeView is wrapped in a Forms.ContentPage.
				_PickerVC = new PickerTableViewController(_PickerCell, tableView, shell.ShellSection.Navigation)
							{
								TableView =
								{
									ContentInset = new UIEdgeInsets(44, 0, 44, 0)
								}
							};
				// Fix height broken. For some reason, TableView ContentSize is broken.
				var page = new ContentPage
						   {
							   Content = _PickerVC.TableView.ToView()
						   };
				;
				page.Title = _PickerCell.Prompt.Title;

				// Fire manually because INavigation.PushAsync does not work ViewDidAppear and ViewWillAppear.
				_PickerVC.ViewDidAppear(false);
				BeginInvokeOnMainThread(async () =>
										{
											await shell.ShellSection.Navigation.PushAsync(page, true);
											_PickerVC.Initialize();
										}
									   );
			}
			else
			{
				// When use traditional navigation.
				_PickerVC = new PickerTableViewController(_PickerCell, tableView);
				BeginInvokeOnMainThread(() => navigationController?.PushViewController(_PickerVC, true));
			}

			if ( !_PickerCell.KeepSelectedUntilBack ) { tableView.DeselectRow(indexPath, true); }
		}

		protected class NavDelegate : UINavigationControllerDelegate
		{
			protected readonly ShellSectionRenderer _self;

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

			if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

			_SelectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;

			if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }

			ValueLabel.Text = _PickerCell.GetSelectedItemsText();
		}

		protected void UpdateCollectionChanged()
		{
			if ( _NotifyCollection != null ) { _NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			_NotifyCollection = _PickerCell.ItemsSource as INotifyCollectionChanged;

			if ( _NotifyCollection != null )
			{
				_NotifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
				ItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		protected override void UpdateIsEnabled()
		{
			if ( _PickerCell.ItemsSource != null &&
				 _PickerCell.ItemsSource.Count == 0 ) { return; }

			base.UpdateIsEnabled();
		}

		protected void ItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}

		protected void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(); }

		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_PickerVC?.Dispose();
				_PickerVC = null;

				if ( _NotifyCollection != null )
				{
					_NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
					_NotifyCollection = null;
				}

				if ( _SelectedCollection != null )
				{
					_SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
					_SelectedCollection = null;
				}
			}

			base.Dispose(disposing);
		}

#nullable enable
		protected UINavigationController? GetUINavigationController( UIViewController? controller )
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