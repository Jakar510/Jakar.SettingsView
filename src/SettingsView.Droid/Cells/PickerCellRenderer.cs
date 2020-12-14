using System;
using System.Collections.Specialized;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Droid.Cells.Base;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)] public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }


	[Preserve(AllMembers = true)]
	public class PickerCellView : LabelCellView, IDialogInterfaceOnShowListener, IDialogInterfaceOnDismissListener
	{
		protected PickerCell _PickerCell => Cell as PickerCell ?? throw new NullReferenceException(nameof(_PickerCell));
		protected AlertDialog? _Dialog { get; set; }
		protected AListView? _ListView { get; set; }
		protected PickerAdapter? _Adapter { get; set; }

		protected string _ValueTextCache { get; set; } = string.Empty;
		// private ImageView _IndicatorView { get; set; }
		// protected LinearLayout _AccessoryStack { get; set; }

		protected INotifyCollectionChanged? _NotifyCollection { get; set; }
		protected INotifyCollectionChanged? _SelectedCollection { get; set; }


		public PickerCellView( Context context, Cell cell ) : base(context, cell)
		{
			// if ( !CellParent.ShowArrowIndicatorForAndroid ) { return; }
			// _IndicatorView = new ImageView(context);
			// _IndicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);
			// AddAccessory(_IndicatorView);
		}
		public PickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			base.CellPropertyChanged(sender, e);

			if ( e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
				 e.PropertyName == PickerCell.SelectedItemProperty.PropertyName ||
				 e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
				 e.PropertyName == PickerCell.UseNaturalSortProperty.PropertyName ||
				 e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName ) { UpdateSelectedItems(true); }
			else if ( e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName ) { UpdateValueText(); }
			else if ( e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems(true);
			}
		}

		protected override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( _PickerCell.ItemsSource == null ||
				 _PickerCell.ItemsSource.Count == 0 ) { return; }

			if ( _PickerCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }

			ShowDialog();
		}

		protected override void UpdateCell()
		{
			base.UpdateCell();
			UpdateSelectedItems();
			UpdateCollectionChanged();
		}
		public void UpdateSelectedItems( bool force = false )
		{
			if ( !_PickerCell.UseAutoValueText ) { return; }

			if ( force || string.IsNullOrEmpty(_ValueTextCache) )
			{
				if ( _SelectedCollection != null ) { _SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

				if ( !( _PickerCell.SelectedItems is INotifyCollectionChanged collection ) ) return;
				_SelectedCollection = collection;
				_SelectedCollection.CollectionChanged += SelectedItems_CollectionChanged;

				_ValueTextCache = _PickerCell.GetSelectedItemsText();
			}

			_Value.Label.Text = _ValueTextCache;
		}
		private void UpdateCollectionChanged()
		{
			if ( _NotifyCollection != null ) { _NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			if ( !( _PickerCell.ItemsSource is INotifyCollectionChanged collection ) ) return;
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

		private void ItemsSourceCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			if ( !CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}
		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(true); }


		internal void ShowDialog() { CreateDialog(); }
		private void CreateDialog()
		{
			_ListView = new AListView(AndroidContext)
						{
							Focusable = false,
							DescendantFocusability = DescendantFocusability.AfterDescendants,
							ChoiceMode = _PickerCell.MaxSelectedNumber == 1 ? ChoiceMode.Single : ChoiceMode.Multiple
						};
			_ListView.SetDrawSelectorOnTop(true);
			_Adapter = new PickerAdapter(AndroidContext, _PickerCell, _ListView);
			_ListView.OnItemClickListener = _Adapter;
			_ListView.Adapter = _Adapter;

			_Adapter.CloseAction = () => { _Dialog?.GetButton((int) DialogButtonType.Positive)?.PerformClick(); };

			if ( _Dialog != null ) return;
			using ( var builder = new AlertDialog.Builder(AndroidContext) )
			{
				builder.SetTitle(_PickerCell.PageTitle);
				builder.SetView(_ListView);

				builder.SetNegativeButton(Android.Resource.String.Cancel, CancelEventHandler);
				builder.SetPositiveButton(Android.Resource.String.Ok, AcceptEventHandler);


				_Dialog = builder.Create();
			}


			if ( _Dialog is null ) return;
			_Dialog.SetCanceledOnTouchOutside(true);
			_Dialog.SetOnDismissListener(this);
			_Dialog.SetOnShowListener(this);
			_Dialog.Show();

			// Pending
			//var buttonTextColor = _PickerCell.AccentColor.IsDefault ? Xamarin.Forms.Color.Accent.ToAndroid() : _PickerCell.AccentColor.ToAndroid();
			//_dialog.GetButton((int)DialogButtonType.Positive).SetTextColor(buttonTextColor);
			//_dialog.GetButton((int)DialogButtonType.Negative).SetTextColor(buttonTextColor);
		}
		private void CancelEventHandler( object o, DialogClickEventArgs args ) { ClearFocus(); }
		private void AcceptEventHandler( object o, DialogClickEventArgs args )
		{
			_Adapter?.DoneSelect();
			UpdateValueText();
			_PickerCell.InvokeCommand();
			ClearFocus();
		}


		public void OnShow( IDialogInterface? dialog ) { _Adapter?.RestoreSelect(); }
		public void OnDismiss( IDialogInterface? dialog )
		{
			_Dialog?.SetOnShowListener(null);
			_Dialog?.SetOnDismissListener(null);
			_Dialog?.Dispose();
			_Dialog = null;
			_Adapter?.Dispose();
			_Adapter = null;
			_ListView?.Dispose();
			_ListView = null;

			Selected = false;
		}

		protected void UpdateValueText()
		{
			if ( _PickerCell.UseAutoValueText ) { UpdateSelectedItems(true); }
			else { _Value.UpdateText(); }
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
				_Dialog?.Dispose();
				_Dialog = null;
				_ListView?.Dispose();
				_ListView = null;
				_Adapter?.Dispose();
				_Adapter = null;
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

				// _IndicatorView?.RemoveFromParent();
				// _IndicatorView?.SetImageDrawable(null);
				// _IndicatorView?.SetImageBitmap(null);
				// _IndicatorView?.Dispose();
				// _IndicatorView = null;
			}

			base.Dispose(disposing);
		}
	}
}