using System.Windows.Input;
using Xamarin.Forms;

namespace Jakar.SettingsView.Shared.Cells
{
	[ContentProperty(nameof(Content))]
	public class CustomCell : CommandCell
	{
		public static BindableProperty ShowArrowIndicatorProperty = BindableProperty.Create(nameof(ShowArrowIndicator), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty LongCommandProperty = BindableProperty.Create(nameof(LongCommand), typeof(ICommand), typeof(CustomCell), default(ICommand), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(CustomCell), default(View), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty IsSelectableProperty = BindableProperty.Create(nameof(IsSelectable), typeof(bool), typeof(CustomCell), true, defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty IsMeasureOnceProperty = BindableProperty.Create(nameof(IsMeasureOnce), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty UseFullSizeProperty = BindableProperty.Create(nameof(UseFullSize), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);
		public static BindableProperty IsForceLayoutProperty = BindableProperty.Create(nameof(IsForceLayout), typeof(bool), typeof(CustomCell), default(bool), defaultBindingMode: BindingMode.OneWay);


		public bool ShowArrowIndicator
		{
			get => (bool) GetValue(ShowArrowIndicatorProperty);
			set => SetValue(ShowArrowIndicatorProperty, value);
		}


		public View Content
		{
			get => (View) GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}


		public bool IsSelectable
		{
			get => (bool) GetValue(IsSelectableProperty);
			set => SetValue(IsSelectableProperty, value);
		}


		internal bool IsForceLayout
		{
			get => (bool) GetValue(IsForceLayoutProperty);
			set => SetValue(IsForceLayoutProperty, value);
		}


		public bool IsMeasureOnce
		{
			get => (bool) GetValue(IsMeasureOnceProperty);
			set => SetValue(IsMeasureOnceProperty, value);
		}


		public bool UseFullSize
		{
			get => (bool) GetValue(UseFullSizeProperty);
			set => SetValue(UseFullSizeProperty, value);
		}


		public ICommand LongCommand
		{
			get => (ICommand) GetValue(LongCommandProperty);
			set => SetValue(LongCommandProperty, value);
		}


		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if ( Content != null ) { Content.BindingContext = BindingContext; }
		}

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