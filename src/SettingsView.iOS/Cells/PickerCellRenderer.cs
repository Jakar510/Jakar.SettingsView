using System.Collections.Specialized;
using System.Linq;
using Foundation;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.iOS.Cells;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

namespace Jakar.SettingsView.iOS.Cells
{
	/// <summary>
	/// Picker cell renderer.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }

	/// <summary>
	/// Picker cell view.
	/// </summary>
	[Preserve(AllMembers = true)]
	public class PickerCellView : LabelCellView
	{
		private PickerCell _PickerCell => Cell as PickerCell;
		private PickerTableViewController _pickerVC;
		private INotifyCollectionChanged _notifyCollection;
		private INotifyCollectionChanged _selectedCollection;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.iOS.Cells.PickerCellView"/> class.
		/// </summary>
		/// <param name="formsCell">Forms cell.</param>
		public PickerCellView( Cell formsCell ) : base(formsCell)
		{
			Accessory = UITableViewCellAccessory.DisclosureIndicator;
			EditingAccessory = UITableViewCellAccessory.DisclosureIndicator;
			SelectionStyle = UITableViewCellSelectionStyle.Default;
			SetRightMarginZero();
		}

		/// <summary>
		/// Cells the property changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
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

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
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

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell( UITableView tableView )
		{
			base.UpdateCell(tableView);
			UpdateSelectedItems();
			UpdateCollectionChanged();
		}

		/// <summary>
		/// Updates the selected items.
		/// </summary>
		/// <param name="force">If set to <c>true</c> force.</param>
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

		/// <summary>
		/// Updates the is enabled.
		/// </summary>
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

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
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

		// Refer to https://forums.xamarin.com/discussion/comment/294088/#Comment_294088
		private UINavigationController GetUINavigationController( UIViewController controller )
		{
			if ( controller != null )
			{
				if ( controller.PresentedViewController != null )
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
					int count = controller.ChildViewControllers.Count();

					for ( var c = 0; c < count; c++ )
					{
						UINavigationController child = GetUINavigationController(controller.ChildViewControllers[c]);
						if ( child == null )
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