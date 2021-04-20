using System;
using System.Collections.Specialized;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Droid.BaseCell;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Jakar.SettingsView.Shared.Enumerations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AListView = Android.Widget.ListView;
using Switch = Android.Widget.Switch;


[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

#nullable enable
namespace Jakar.SettingsView.Droid.Cells
{
	[Preserve(AllMembers = true)]
	public class PickerCellRenderer : CellBaseRenderer<PickerCellView> { }



	[Preserve(AllMembers = true)]
	public class PickerCellView : BaseAiValueCell, IDialogInterfaceOnShowListener, IDialogInterfaceOnDismissListener
	{
		protected PickerCell     _PickerCell => Cell as PickerCell ?? throw new NullReferenceException(nameof(_PickerCell));
		protected AlertDialog?   _Dialog     { get; set; }
		protected AListView?     _ListView   { get; set; }
		protected PickerAdapter? _Adapter    { get; set; }
		protected TextView?      _TitleLabel { get; set; }

		protected string _ValueTextCache { get; set; } = string.Empty;

		protected INotifyCollectionChanged? _NotifyCollection   { get; set; }
		protected INotifyCollectionChanged? _SelectedCollection { get; set; }


		public PickerCellView( Context context, Cell cell ) : base(context, cell)
		{
			// if ( !CellParent.ShowArrowIndicatorForAndroid ) { return; }
			// _IndicatorView = new ImageView(context);
			// _IndicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);
			// AddAccessory(_IndicatorView);
		}

		public PickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

		protected internal override void CellPropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if ( e.PropertyName == PickerCell.SelectedItemsProperty.PropertyName ||
				 e.PropertyName == PickerCell.SelectedItemProperty.PropertyName ||
				 e.PropertyName == PickerCell.DisplayMemberProperty.PropertyName ||
				 e.PropertyName == PickerCell.UseNaturalSortProperty.PropertyName ||
				 e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName ) { UpdateSelectedItems(true); }
			else if ( e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems(true);
			}
			else { base.CellPropertyChanged(sender, e); }
		}

		protected internal override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( _PickerCell.ItemsSource == null ||
				 _PickerCell.ItemsSource.Count == 0 ) { return; }

			if ( _PickerCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }

			ShowDialog();
		}

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
			if ( _NotifyCollection != null ) { _NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged; }

			if ( !( _PickerCell.ItemsSource is INotifyCollectionChanged collection ) ) return;
			_NotifyCollection                   =  collection;
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
			if ( !Cell.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource?.Count > 0);
		}

		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(true); }


		internal void ShowDialog() { CreateDialog(); }

		protected void CreateDialog()
		{
			_TitleLabel?.Dispose();
			_ListView?.Dispose();
			_Adapter?.Dispose();

			_ListView = new AListView(AndroidContext)
						{
							Focusable              = false,
							DescendantFocusability = DescendantFocusability.AfterDescendants,
							ChoiceMode = _PickerCell.SelectionMode switch
										 {
											 SelectMode.Single => ChoiceMode.Single,
											 _                 => ChoiceMode.Multiple,
										 }
						};

			_ListView.SetDrawSelectorOnTop(true);
			_Adapter = new PickerAdapter(AndroidContext, this, _PickerCell, _ListView);

			_ListView.OnItemClickListener = _Adapter;
			_ListView.Adapter             = _Adapter;

			_TitleLabel = new TextView(AndroidContext)
						  {
							  Text    = _PickerCell.Prompt.Title,
							  Gravity = GravityFlags.Center
						  };

			_TitleLabel.SetBackgroundColor(_Adapter.BackgroundColor);
			_TitleLabel.SetTextColor(_Adapter.TitleTextColor);
			_TitleLabel.SetTextSize(ComplexUnitType.Sp, _Adapter.FontSize);


			if ( _Dialog is not null ) return;

			using ( var builder = new AlertDialog.Builder(AndroidContext) )
			{
				// builder.SetTitle(_PickerCell.PopupTitle);
				builder.SetCustomTitle(_TitleLabel);
				builder.SetView(_ListView);

				builder.SetNegativeButton(_PickerCell.Prompt.Cancel, CancelEventHandler);
				builder.SetPositiveButton(_PickerCell.Prompt.Accept, AcceptEventHandler);

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

		protected internal void CloseAction() { _Dialog?.GetButton((int) DialogButtonType.Positive)?.PerformClick(); }
		protected void CancelEventHandler( object o, DialogClickEventArgs args ) { ClearFocus(); }

		protected void AcceptEventHandler( object o, DialogClickEventArgs args )
		{
			_Adapter?.DoneSelect();
			UpdateSelectedItems(true);

			_PickerCell.SendValueChanged();
			_PickerCell.InvokeSelectedCommand();
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

				_TitleLabel?.Dispose();
				_TitleLabel = null;

				if ( _NotifyCollection != null )
				{
					_NotifyCollection.CollectionChanged -= ItemsSourceCollectionChanged;
					_NotifyCollection                   =  null;
				}

				if ( _SelectedCollection != null )
				{
					_SelectedCollection.CollectionChanged -= SelectedItems_CollectionChanged;
					_SelectedCollection                   =  null;
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
