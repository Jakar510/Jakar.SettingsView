﻿using System;
using System.Collections.Specialized;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Jakar.SettingsView.Shared.Cells;
using Jakar.SettingsView.Droid.Cells;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(PickerCell), typeof(PickerCellRenderer))]

namespace Jakar.SettingsView.Droid.Cells
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
	public class PickerCellView : LabelCellView, IDialogInterfaceOnShowListener, IDialogInterfaceOnDismissListener
	{
		private PickerCell _PickerCell => Cell as PickerCell;
		private AlertDialog _dialog;
		private AListView _listView;
		private PickerAdapter _adapter;
		private Context _context;
		private string _valueTextCache;
		private INotifyCollectionChanged _notifyCollection;
		private INotifyCollectionChanged _selectedCollection;
		private ImageView _indicatorView;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Jakar.SettingsView.Droid.Cells.PickerCellView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="cell">Cell.</param>
		public PickerCellView( Context context, Cell cell ) : base(context, cell)
		{
			_context = context;

			if ( !CellParent.ShowArrowIndicatorForAndroid ) { return; }

			_indicatorView = new ImageView(context);
			_indicatorView.SetImageResource(Resource.Drawable.ic_navigate_next);

			var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
						{ };

			using ( param ) { AccessoryStack.AddView(_indicatorView, param); }
		}

		public PickerCellView( IntPtr javaReference, JniHandleOwnership transfer ) : base(javaReference, transfer) { }

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
				 e.PropertyName == PickerCell.SelectedItemsOrderKeyProperty.PropertyName ) { UpdateSelectedItems(true); }
			else if ( e.PropertyName == PickerCell.UseAutoValueTextProperty.PropertyName )
			{
				if ( _PickerCell.UseAutoValueText ) { UpdateSelectedItems(true); }
				else { UpdateValueText(); }
			}
			else if ( e.PropertyName == PickerCell.ItemsSourceProperty.PropertyName )
			{
				UpdateCollectionChanged();
				UpdateSelectedItems(true);
			}
		}

		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="adapter">Adapter.</param>
		/// <param name="position">Position.</param>
		public override void RowSelected( SettingsViewRecyclerAdapter adapter, int position )
		{
			if ( _PickerCell.ItemsSource == null ) { return; }

			if ( _PickerCell.ItemsSource.Count == 0 ) { return; }

			if ( _PickerCell.KeepSelectedUntilBack ) { adapter.SelectedRow(this, position); }

			ShowDialog();
		}

		/// <summary>
		/// Updates the cell.
		/// </summary>
		public override void UpdateCell()
		{
			base.UpdateCell();
			UpdateSelectedItems();
			UpdateCollectionChanged();
		}

		/// <summary>
		/// Updates the selected items.
		/// </summary>
		/// <param name="force">If set to <c>true</c> force.</param>
		public void UpdateSelectedItems( bool force = false )
		{
			if ( !_PickerCell.UseAutoValueText ) { return; }


			if ( force || string.IsNullOrEmpty(_valueTextCache) )
			{
				if ( _selectedCollection != null ) { _selectedCollection.CollectionChanged -= SelectedItems_CollectionChanged; }

				_selectedCollection = _PickerCell.SelectedItems as INotifyCollectionChanged;
				if ( _selectedCollection != null ) { _selectedCollection.CollectionChanged += SelectedItems_CollectionChanged; }

				_valueTextCache = _PickerCell.GetSelectedItemsText();
			}

			VValueLabel.Text = _valueTextCache;
		}

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <returns>The dispose.</returns>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				_dialog?.Dispose();
				_dialog = null;
				_listView?.Dispose();
				_listView = null;
				_adapter?.Dispose();
				_adapter = null;
				_context = null;
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

				_indicatorView?.RemoveFromParent();
				_indicatorView?.SetImageDrawable(null);
				_indicatorView?.SetImageBitmap(null);
				_indicatorView?.Dispose();
				_indicatorView = null;
			}

			base.Dispose(disposing);
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
			if ( !_CellBase.IsEnabled ) { return; }

			SetEnabledAppearance(_PickerCell.ItemsSource.Count > 0);
		}

		private void SelectedItems_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) { UpdateSelectedItems(true); }


		internal void ShowDialog() { CreateDialog(); }

		private void CreateDialog()
		{
			_listView = new AListView(_context);
			_listView.Focusable = false;
			_listView.DescendantFocusability = DescendantFocusability.AfterDescendants;
			_listView.SetDrawSelectorOnTop(true);
			_listView.ChoiceMode = _PickerCell.MaxSelectedNumber == 1 ? ChoiceMode.Single : ChoiceMode.Multiple;
			_adapter = new PickerAdapter(_context, _PickerCell, _listView);
			_listView.OnItemClickListener = _adapter;
			_listView.Adapter = _adapter;

			_adapter.CloseAction = () => { _dialog.GetButton((int) DialogButtonType.Positive).PerformClick(); };

			if ( _dialog == null )
			{
				using ( var builder = new AlertDialog.Builder(_context) )
				{
					builder.SetTitle(_PickerCell.PageTitle);
					builder.SetView(_listView);

					builder.SetNegativeButton(Android.Resource.String.Cancel, ( o, args ) => { ClearFocus(); });
					builder.SetPositiveButton(Android.Resource.String.Ok, ( o, args ) =>
																				  {
																					  _adapter.DoneSelect();
																					  UpdateSelectedItems(true);
																					  _PickerCell.InvokeCommand();
																					  ClearFocus();
																				  });


					_dialog = builder.Create();
				}


				_dialog.SetCanceledOnTouchOutside(true);
				_dialog.SetOnDismissListener(this);
				_dialog.SetOnShowListener(this);
				_dialog.Show();

				// Pending
				//var buttonTextColor = _PickerCell.AccentColor.IsDefault ? Xamarin.Forms.Color.Accent.ToAndroid() : _PickerCell.AccentColor.ToAndroid();
				//_dialog.GetButton((int)DialogButtonType.Positive).SetTextColor(buttonTextColor);
				//_dialog.GetButton((int)DialogButtonType.Negative).SetTextColor(buttonTextColor);
			}
		}

		/// <summary>
		/// Ons the show.
		/// </summary>
		/// <param name="dialog">Dialog.</param>
		public void OnShow( IDialogInterface dialog ) { _adapter.RestoreSelect(); }

		/// <summary>
		/// Ons the dismiss.
		/// </summary>
		/// <param name="dialog">Dialog.</param>
		public void OnDismiss( IDialogInterface dialog )
		{
			_dialog?.SetOnShowListener(null);
			_dialog?.SetOnDismissListener(null);
			_dialog?.Dispose();
			_dialog = null;
			_adapter?.Dispose();
			_adapter = null;
			_listView?.Dispose();
			_listView = null;
			Selected = false;
		}
	}
}