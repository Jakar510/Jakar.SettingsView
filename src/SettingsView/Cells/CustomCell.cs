using System.Windows.Input;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	/// <summary>
	/// Custom cell.
	/// </summary>
	[ContentProperty("Content")]
	public class CustomCell : CommandCell
	{
		/// <summary>
		/// The show arrow indicator property.
		/// </summary>
		public static BindableProperty ShowArrowIndicatorProperty = BindableProperty.Create(nameof(ShowArrowIndicator), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CustomCell"/> show arrow indicator.
		/// </summary>
		/// <value><c>true</c> if show arrow indicator; otherwise, <c>false</c>.</value>
		public bool ShowArrowIndicator
		{
			get => (bool) GetValue(ShowArrowIndicatorProperty);
			set => SetValue(ShowArrowIndicatorProperty, value);
		}

		/// <summary>
		/// The content property.
		/// </summary>
		public static BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(CustomCell), default(View), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		public View Content
		{
			get => (View) GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		/// <summary>
		/// The is selectable property.
		/// </summary>
		public static BindableProperty IsSelectableProperty = BindableProperty.Create(nameof(IsSelectable), typeof(bool), typeof(CustomCell), true, defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CustomCell"/> is selectable.
		/// </summary>
		/// <value><c>true</c> if is selectable; otherwise, <c>false</c>.</value>
		public bool IsSelectable
		{
			get => (bool) GetValue(IsSelectableProperty);
			set => SetValue(IsSelectableProperty, value);
		}

		/// <summary>
		/// The is measure once property.
		/// </summary>
		public static BindableProperty IsMeasureOnceProperty = BindableProperty.Create(nameof(IsMeasureOnce), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);

		internal bool IsForceLayout { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CustomCell"/> is measure once.
		/// </summary>
		/// <value><c>true</c> if is measure once; otherwise, <c>false</c>.</value>
		public bool IsMeasureOnce
		{
			get => (bool) GetValue(IsMeasureOnceProperty);
			set => SetValue(IsMeasureOnceProperty, value);
		}

		/// <summary>
		/// The use full size property.
		/// </summary>
		public static BindableProperty UseFullSizeProperty = BindableProperty.Create(nameof(UseFullSize), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Jakar.SettingsView.Shared.Cells.CustomCell"/> use full size.
		/// </summary>
		/// <value><c>true</c> if use full size; otherwise, <c>false</c>.</value>
		public bool UseFullSize
		{
			get => (bool) GetValue(UseFullSizeProperty);
			set => SetValue(UseFullSizeProperty, value);
		}

		public static BindableProperty LongCommandProperty = BindableProperty.Create(nameof(LongCommand), typeof(ICommand), typeof(CustomCell), default(ICommand), defaultBindingMode: BindingMode.OneWay);

		public ICommand LongCommand
		{
			get => (ICommand) GetValue(LongCommandProperty);
			set => SetValue(LongCommandProperty, value);
		}

		/// <summary>
		/// Ons the binding context changed.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if ( Content != null ) { Content.BindingContext = BindingContext; }
		}

		/// <summary>
		/// Ons the parent set.
		/// </summary>
		protected override void OnParentSet()
		{
			base.OnParentSet();
			if ( Content != null ) { Content.Parent = Parent; }
		}

		public override void Reload()
		{
			IsForceLayout = true;
			base.Reload();
		}

		public void SendLongCommand()
		{
			if ( LongCommand == null ) { return; }

			if ( LongCommand.CanExecute(BindingContext) ) { LongCommand.Execute(BindingContext); }
		}
	}
}