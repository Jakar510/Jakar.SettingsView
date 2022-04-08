// unset


namespace Jakar.SettingsView.iOS.BaseCell
{
	[Foundation.Preserve(AllMembers = true)]
	public abstract class BaseValueCell<TValueView, TCell, TValueManager> : BaseCellView<TCell> where TValueView : UIView, new()
																								where TCell : ValueCellBase
																								where TValueManager : BaseViewManager<TValueView, TCell>
	{
		private IconView?        _icon;
		private UIStackView?     _titleStack;
		private TitleView?       _title;
		private DescriptionView? _description;
		private UIStackView?     _valueStack;
		private HintView?        _hint;
		private TValueManager?   _value;


		public ValueTextCellBase? ValueTextCell => Cell as ValueTextCellBase;

		public IUseConfiguration TitleConfiguration       => Cell.TitleConfig;
		public IUseConfiguration DescriptionConfiguration => Cell.DescriptionConfig;
		public IUseConfiguration HintConfiguration        => Cell.HintConfig;
		public IUseConfiguration ValueConfiguration       => Cell.ValueTextConfig;


		protected IconView _Icon
		{
			get => _icon ?? throw new NullReferenceException(nameof(_icon));
			set => _icon = value;
		}

		/// <summary>
		/// Vertical StackView that is arranged at right. ( put Title and Description ) 
		/// </summary>
		protected UIStackView _TitleStack
		{
			get => _titleStack ?? throw new NullReferenceException(nameof(_titleStack));
			set => _titleStack = value;
		}

		protected TitleView _Title
		{
			get => _title ?? throw new NullReferenceException(nameof(_title));
			set => _title = value;
		}

		protected DescriptionView _Description
		{
			get => _description ?? throw new NullReferenceException(nameof(_description));
			set => _description = value;
		}


		/// <summary>
		/// Vertical StackView that is arranged at left of the _TitleStack.( put Hint and ValueText)
		/// </summary>
		protected UIStackView _ValueStack
		{
			get => _valueStack ?? throw new NullReferenceException(nameof(_valueStack));
			set => _valueStack = value;
		}

		protected HintView _Hint
		{
			get => _hint ?? throw new NullReferenceException(nameof(_hint));
			set => _hint = value;
		}

		// private UIView _FieldWrapper { get; set; }

		protected TValueManager _Value => _value ?? throw new NullReferenceException(nameof(_value));


		protected BaseValueCell( TCell formsCell ) : base(formsCell)
		{
			_Icon = new IconView(this, Cell);

			_TitleStack  = Stack.Title();
			_Title       = new TitleView(this);
			_Description = new DescriptionView(this);

			_ValueStack = Stack.Value();
			_Hint       = new HintView(this);
			_value      = InstanceCreator<TValueManager>.Create(this);

			_Icon.Initialize(_MainStack);
			_Title.Initialize(_TitleStack);
			_Description.Initialize(_TitleStack);
			_MainStack.AddArrangedSubview(_TitleStack);
			_TitleStack.HeightOf(_MainStack, 1);
			_TitleStack.UpdateConstraintsIfNeeded();

			_Hint.Initialize(_ValueStack);
			_Value.Initialize(_ValueStack);
			_MainStack.AddArrangedSubview(_ValueStack);
			_ValueStack.HeightOf(_MainStack, 1);
			_ValueStack.UpdateConstraintsIfNeeded();

			_TitleStack.InBetween(_MainStack, _Icon.Control, _ValueStack);
			_ValueStack.RightExtended(_MainStack, _TitleStack);

			_Icon.Control.WidthOf(_MainStack, SvConstants.Layout.ColumnFactors.ICON);
			_TitleStack.WidthOf(_MainStack, SvConstants.Layout.ColumnFactors.TITLE_STACK);
			_ValueStack.RightExtended(_MainStack, _TitleStack);

			// _ValueStack.WidthOf(_MainStack, SvConstants.Layout.ColumnFactors.VALUE_STACK);

			_TitleStack.HuggingPriority(LayoutPriority.Low, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);
			_TitleStack.CompressionPriorities(LayoutPriority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);

			_ValueStack.HuggingPriority(LayoutPriority.Low, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);
			_ValueStack.CompressionPriorities(LayoutPriority.Highest, UILayoutConstraintAxis.Horizontal, UILayoutConstraintAxis.Vertical);

			this.SetContent(_MainStack);

			if ( !string.IsNullOrEmpty(Cell.AutomationId) ) { _MainStack.AccessibilityIdentifier = Cell.AutomationId; }

			UpdateConstraintsIfNeeded();
			SetNeedsLayout();
		}


		public override void CellPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Icon.Update(sender, e) ) { return; }

			if ( _Title.Update(sender, e) ) { return; }

			if ( _Description.Update(sender, e) ) { return; }

			if ( _Hint.Update(sender, e) ) { return; }

			if ( _Value.Update(sender, e) ) { return; }

			base.CellPropertyChanged(sender, e);
		}

		public override void ParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if ( _Icon.UpdateParent(sender, e) ) { return; }

			if ( _Title.UpdateParent(sender, e) ) { return; }

			if ( _Description.UpdateParent(sender, e) ) { return; }

			if ( _Hint.UpdateParent(sender, e) ) { return; }

			if ( _Value.UpdateParent(sender, e) ) { return; }

			else { base.CellPropertyChanged(sender, e); }
		}

		public override void UpdateCell( UITableView tableView )
		{
			_Icon.Update();
			_Title.Update();
			_Description.Update();
			_Hint.Update();
			_Value.Update();

			base.UpdateCell(tableView);
		}

		protected override void SetEnabledAppearance( bool isEnabled )
		{
			base.SetEnabledAppearance(isEnabled);

			if ( isEnabled )
			{
				_Icon.Enable();
				_Title.Enable();
				_Description.Enable();
				_Hint.Enable();
				_Value.Enable();
			}
			else
			{
				_Icon.Disable();
				_Title.Disable();
				_Description.Disable();
				_Hint.Disable();
				_Value.Disable();
			}
		}


		protected override void Dispose( bool disposing )
		{
			if ( disposing && !_disposed )
			{
				_icon?.Dispose();
				_icon = null;

				_title?.Dispose();
				_title = null;

				_description?.Dispose();
				_title = null;

				_hint?.Dispose();
				_hint = null;

				_value?.Dispose();
				_value = null;

				_title?.Dispose();
				_title = null;

				_titleStack?.Dispose();
				_titleStack = null;

				_valueStack?.Dispose();
				_valueStack = null;
			}

			base.Dispose(disposing);
		}
	}
}
