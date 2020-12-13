public class SectionHeader : Grid
	{
		public bool IsExpanded { get; set; }
		protected Label _Label { get; set; }
		protected Image _Icon { get; set; }
		public ImageSource Collapsed { get; set; }
		public ImageSource Expanded { get; set; }


		public SectionHeader( Action command ) : this(new Command(command)) { }
		public SectionHeader( ICommand command ) => Init(command);
		~SectionHeader()
		{
			ClearGesture(this);
			ClearGesture(_Label);
			ClearGesture(_Icon);
		}
		protected void Init( ICommand command )
		{
			CreateView();
			AddGesture(_Label, command);
			AddGesture(this, command);
			AddGesture(_Icon, command);
		}
		protected virtual void CreateView()
		{
			MinimumHeightRequest = 30;
			//HeightRequest = 40;
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;


			ColumnDefinitions = new ColumnDefinitionCollection()
			{
				new ColumnDefinition() { Width = new GridLength(0.1, GridUnitType.Star) },
				new ColumnDefinition() { Width = GridLength.Star },
				new ColumnDefinition() { Width = new GridLength(0.1, GridUnitType.Star) },
			};

			SetDynamicResource(BackgroundColorProperty, "HeaderBackgroundColor");


			_Label = new Label()
			{
				Padding = new Thickness(0, 10, 0, 10),
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

			};
			_Label.SetDynamicResource(Label.FontSizeProperty, "HeaderFontSize");
			_Label.SetDynamicResource(Label.TextColorProperty, "HeaderTextColor");
			_Label.SetDynamicResource(Label.BackgroundColorProperty, "HeaderBackgroundColor");
			SetColumn(_Label, 1);


			_Icon = new Image()
			{
				Aspect = Aspect.AspectFit,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			SetColumn(_Icon, 0);
			_Icon.SetDynamicResource(Image.BackgroundColorProperty, "HeaderBackgroundColor");


			Children.Add(_Label);
			Children.Add(_Icon);
		}


		protected static void AddGesture( View view, ICommand command )
		{
			if ( view is null )
				throw new ArgumentNullException(nameof(view));

			if ( command is null )
				throw new ArgumentNullException(nameof(command));

			var tap = new TapGestureRecognizer
			{
				NumberOfTapsRequired = 1,
				Command = command
			};

			view.GestureRecognizers.Add(tap);
		}
		protected static void ClearGesture( View view )
		{
			if ( view is null )
				throw new ArgumentNullException(nameof(view));

			view.GestureRecognizers.Clear();
		}

		public void Toggle()
		{
			bool check = !IsExpanded;
			IsExpanded = check;

			_Icon.Source = IsExpanded ? Collapsed :  Expanded;
		}
		public void SetText( string text ) => _Label.Text = text;
	}