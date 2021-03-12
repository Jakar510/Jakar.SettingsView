using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Foundation;
using Jakar.SettingsView.iOS.BaseCell;
using Jakar.SettingsView.iOS.Cells;
using Jakar.SettingsView.iOS.Cells.Sources;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Shared.Enumerations;
using Jakar.SettingsView.Shared.Misc;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.iOS.Cells
{
	[Preserve(AllMembers = true)] public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }


	[Preserve(AllMembers = true)]
	public class PickerCellView : BasePickerCell
	{
		protected PickerCell _PickerCell => Cell as PickerCell ?? throw new NullReferenceException(nameof(_PickerCell));

		protected string _ValueTextCache { get; set; } = string.Empty;
		protected PickerTableViewController? _PickerVC { get; set; }

		protected INotifyCollectionChanged? _NotifyCollection { get; set; }
		protected INotifyCollectionChanged? _SelectedCollection { get; set; }


		public PickerCellView( Cell cell ) : base(cell)
		{
			// if ( !CellParent.ShowArrowIndicatorForAndroid ) { return; }
			// _IndicatorView = new ImageView(context);
			// _IndicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);
			// AddAccessory(_IndicatorView);
		}

		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
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

		protected internal override void RowSelected( UITableView tableView, NSIndexPath indexPath )
		{
			if ( _PickerCell.ItemsSource == null )
			{
				tableView.DeselectRow(indexPath, true);
				return;
			}

			SetUp(tableView, indexPath);
		}
		protected override void SetUp( UITableView tableView, NSIndexPath indexPath )
		{
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
		protected UINavigationController? GetUINavigationController( UIViewController? controller )
		{
			// Refer to https://forums.xamarin.com/discussion/comment/294088/#Comment_294088
			switch ( controller )
			{
				case null:
					return null;

				case UINavigationController navigation:
					return navigation;

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


		public void UpdateSelectedItems()
		{
			if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

			_SelectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;

			if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }

			_Value.Text = _PickerCell.GetSelectedItemsText();
		}


		private void ItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}

		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(); }

		protected internal override void UpdateCell()
		{
			base.UpdateCell();
			UpdateSelectedItems(false);
			UpdateCollectionChanged();
		}
		public void UpdateSelectedItems( bool force )
		{
			if ( force || string.IsNullOrWhiteSpace(_ValueTextCache) )
			{
				if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

				if ( _PickerCell.SelectedItems is INotifyCollectionChanged collection ) { _SelectedCollection = collection; }

				_ValueTextCache = _PickerCell.GetSelectedItemsText();

				if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }
			}

			_Value.UpdateText(_ValueTextCache);
		}
		private void UpdateCollectionChanged()
		{
			if ( _NotifyCollection is not null ) { _NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			if ( _PickerCell.ItemsSource is not INotifyCollectionChanged collection ) return;
			_NotifyCollection = collection;
			_NotifyCollection.CollectionChanged += ItemsSourceCollectionChanged;
			ItemsSourceCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		protected override void UpdateIsEnabled()
		{
			if ( _PickerCell.ItemsSource != null &&
				 _PickerCell.ItemsSource.Count == 0 ) { return; }

			base.UpdateIsEnabled();
		}


		protected internal override void UpdateCell( UITableView? tableView )
		{
			base.UpdateCell(tableView);
			UpdateSelectedItems();
			UpdateCollectionChanged();
		}
		protected override void EnableCell()
		{
			base.EnableCell();
			_Title.Enable();
			_Description.Enable();
		}
		protected override void DisableCell()
		{
			base.DisableCell();
			_Title.Disable();
			_Description.Disable();
		}



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
	}
}