using System;

using Xamarin.Forms;

namespace Global.InputForms
{
    public class Switch : Grid
    {
        /// <summary>
        ///     The grid color property.
        /// </summary>
        public static readonly BindableProperty GridColorProperty = BindableProperty.Create(nameof(GridColor),
            typeof(Color), typeof(Switch), null);

        /// <summary>
        ///     The switch height request property.
        /// </summary>
        public static readonly BindableProperty SwitchHightRequestProperty = BindableProperty.Create(nameof(SwitchHeightRequest),
            typeof(double), typeof(Switch), 0d, propertyChanged: SwitchHeightRequestChanged);

        /// <summary>
        ///     The switch width request property.
        /// </summary>
        public static readonly BindableProperty SwitchWidthRequestProperty = BindableProperty.Create(nameof(SwitchWidthRequest),
            typeof(double), typeof(Switch), 0d, propertyChanged: SwitchWidthRequestChanged);

        /// <summary>
        ///     The background height request property.
        /// </summary>
        public static readonly BindableProperty BackgroundHeightRequestProperty = BindableProperty.Create(nameof(BackgroundHeightRequest),
            typeof(double), typeof(Switch), 0d, propertyChanged: BackgroundHeightRequestChanged);

        /// <summary>
        ///     The background width request property.
        /// </summary>
        public static readonly BindableProperty BackgroundWidthRequestProperty = BindableProperty.Create(nameof(BackgroundWidthRequest),
            typeof(double), typeof(Switch), 0d, propertyChanged: BackgroundWidthRequestChanged);

        /// <summary>
        ///     The background color property.
        /// </summary>
        public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
            typeof(Color), typeof(Switch), null);

        /// <summary>
        ///     The background content property.
        /// </summary>
        public static readonly BindableProperty BackgroundContentProperty = BindableProperty.Create(nameof(BackgroundContent),
            typeof(View), typeof(Switch), null);

        /// <summary>
        ///     The icon switch content property.
        /// </summary>
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content),
            typeof(View), typeof(Switch), propertyChanged: ToggleChanged);

        /// <summary>
        ///     The isToggled property.
        /// </summary>
        public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled),
            typeof(bool), typeof(Switch), propertyChanged: IsToggledChanged);

        public enum SwitchHorizontalState
        {
            Right,
            Left,
        }

        public enum SwitchVerticalState
        {
            Top,
            Bot,
        }

        Frame _principalFrame;
        Frame _iconSwitch;
        View _rightView;
        View _leftView;
        double x = 0;
        double _posActuel;
        bool _isFirstTime;
        double _translation;
        bool isBusy;
        double TmpTotalX;
        SwitchHorizontalState _state;
        SwitchVerticalState _stateVertical;

        public Frame IconSwitch { get => _iconSwitch; set => _leftView = value; }
        public View RightView { get => _rightView; set => _rightView = value; }
        public View LeftView { get => _leftView; set => _leftView = value; }

        public SwitchHorizontalState State { get => _state; set => _state = value; }
        public SwitchVerticalState StateVertical { get => _stateVertical; set => _stateVertical = value; }

        public Switch()
        {
            _isFirstTime = true;
            _posActuel = 0;
            _translation = 0;
            State = SwitchHorizontalState.Right;
            StateVertical = SwitchVerticalState.Top;

            this.BackgroundColor = Color.Yellow;
            this.SetBinding(Grid.BackgroundColorProperty,
                new Binding(nameof(GridColor)) { Source = this, Mode = BindingMode.OneWay });
            this.SetBinding(Switch.IsToggledProperty,
                new Binding(nameof(IsToggled)) { Source = this, Mode = BindingMode.OneWay });

            _principalFrame = new Frame
            {
                IsClippedToBounds = true,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            this.Children.Add(_principalFrame, 0, 0);
            Grid.SetColumnSpan(_principalFrame, 2);

            _principalFrame.SetBinding(Frame.HeightRequestProperty,
                new Binding(nameof(BackgroundHeightRequest)) { Source = this, Mode = BindingMode.OneWay });
            _principalFrame.SetBinding(Frame.WidthRequestProperty,
                new Binding(nameof(BackgroundWidthRequest)) { Source = this, Mode = BindingMode.OneWay });
            _principalFrame.SetBinding(Frame.BackgroundColorProperty,
               new Binding(nameof(BackgroundColor)) { Source = this, Mode = BindingMode.OneWay });
            _principalFrame.SetBinding(Frame.ContentProperty,
               new Binding(nameof(BackgroundContent)) { Source = this, Mode = BindingMode.OneWay });

            this.ColumnDefinitions = new ColumnDefinitionCollection
            {
              new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
              new ColumnDefinition { Width =  new GridLength (1, GridUnitType.Star)},
           };

            _iconSwitch = new Frame
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsClippedToBounds = true,
                Padding = 0,
                BackgroundColor = Color.Gray,
            };
            _iconSwitch.SetBinding(HeightRequestProperty,
                new Binding(nameof(SwitchHeightRequest)) { Source = this, Mode = BindingMode.OneWay });
            _iconSwitch.SetBinding(Frame.ContentProperty,
                new Binding(nameof(Content)) { Source = this, Mode = BindingMode.OneWay });
            this.Children.Add(_iconSwitch, 1, 0);

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnTapLabel;
            this.GestureRecognizers.Add(tapGestureRecognizer);

            IsToggled = true;
        }

        /// <summary>
        ///     Gets or sets the Grid Color.
        /// </summary>
        /// <value>The Grid Color.</value>
        public Color GridColor
        {
            get => (Color)GetValue(GridColorProperty);
            set => SetValue(GridColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Switch Height Request.
        /// </summary>
        /// <value>The Switch Height Request.</value>
        public double SwitchHeightRequest
        {
            get => (double)GetValue(SwitchHightRequestProperty);
            set => SetValue(SwitchHightRequestProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Switch Width Request.
        /// </summary>
        /// <value>The Switch Width Request.</value>
        public double SwitchWidthRequest
        {
            get => (double)GetValue(SwitchWidthRequestProperty);
            set => SetValue(SwitchWidthRequestProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Background Height Request.
        /// </summary>
        /// <value>The Background Height Request.</value>
        public double BackgroundHeightRequest
        {
            get => (double)GetValue(BackgroundHeightRequestProperty);
            set => SetValue(BackgroundHeightRequestProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Background Width Request.
        /// </summary>
        /// <value>The Background width Request.</value>
        public double BackgroundWidthRequest
        {
            get => (double)GetValue(BackgroundWidthRequestProperty);
            set => SetValue(BackgroundWidthRequestProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Background Color.
        /// </summary>
        /// <value>The Background Color.</value>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Background Content.
        /// </summary>
        /// <value>The Background Content.</value>
        public View BackgroundContent
        {
            get => (View)GetValue(BackgroundContentProperty);
            set => SetValue(BackgroundContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Switch Content.
        /// </summary>
        /// <value>The Switch Content.</value>
        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the position of  the Toggle.
        /// </summary>
        /// <value>The Toggled Position.</value>
        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        /// <summary>
        ///     The Switch Height Request property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SwitchHeightRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Switch view)
            {
                //viewTemplate._iconview.HeightRequest = 10;
                //view.RightView.HeightRequest = (double)newValue;
                //view.LeftView.HeightRequest = (double)newValue;
                view._iconSwitch.HeightRequest = (double)newValue;
                view._iconSwitch.CornerRadius = (float)view.SwitchHeightRequest / 2;
            }
        }

        /// <summary>
        ///     The Switch Width Request property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void SwitchWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Switch view)
            {
                view.SwitchWidthRequest = (double)newValue;
                //view.RightView.WidthRequest = (double)newValue;
                //view.LeftView.WidthRequest = (double)newValue;

                if (2 * view.SwitchWidthRequest > view.BackgroundWidthRequest)
                    view.WidthRequest = 2 * view.SwitchWidthRequest;
                else
                    view.WidthRequest = view.BackgroundWidthRequest;
            }
        }

        /// <summary>
        ///     The Background Height Request property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void BackgroundHeightRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Switch view)
            {
                view.BackgroundHeightRequest = (double)newValue;
                view._principalFrame.CornerRadius = (float)(view.BackgroundHeightRequest / 2);
            }
        }

        /// <summary>
        ///     The Background Width Request property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void BackgroundWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Switch view)
            {
                view.BackgroundWidthRequest = (double)newValue;
                if (2 * view.SwitchWidthRequest > view.BackgroundWidthRequest)
                    view.WidthRequest = 2 * view.SwitchWidthRequest;
                else
                    view.WidthRequest = view.BackgroundWidthRequest;
            }
        }

        /// <summary>
        ///     The Content property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void ToggleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Switch view)
            {
                view.RightView.WidthRequest = view.SwitchWidthRequest;
                view.LeftView.WidthRequest = view.SwitchWidthRequest;
                view.RightView.HeightRequest = view._iconSwitch.HeightRequest;
                view.LeftView.HeightRequest = view._iconSwitch.HeightRequest;
                view.Toggled?.Invoke(view, new ToggledEventArgs(view.IsToggled));
            }
        }

        /// <summary>
        ///     The IsToggled property changed.
        /// </summary>
        /// <param name="bindable">The object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void IsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Switch view)
            {
                if (view._isFirstTime)
                {
                    view._isFirstTime = false;
                }
                else if ((bool)newValue)
                    view.GoToRight();
                else
                    view.GoToLeft();
            }
        }

        private async void OnTapLabel(object sender, EventArgs e)
        {
            if (!isBusy)
            {
                isBusy = true;
                if (State == SwitchHorizontalState.Right)
                {
                    IsToggled = false;
                    State = SwitchHorizontalState.Left;
                    StateVertical = SwitchVerticalState.Bot;
                    //  GoToLeft();
                }
                else
                {
                    IsToggled = true;
                    State = SwitchHorizontalState.Right;
                    StateVertical = SwitchVerticalState.Top;
                    //  GoToRight();
                }
            }
            isBusy = false;
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (!isBusy)
            {
                isBusy = true;

                var dragX = e.TotalX - TmpTotalX;

                switch (e.StatusType)
                {
                    case GestureStatus.Running:

                        double _previewDifference = _iconSwitch.TranslationX + e.TotalX;

                        _iconSwitch.TranslationX = Math.Min(0, Math.Max(-this.Width / 2 - 3, _iconSwitch.TranslationX + dragX));
                        _posActuel = e.TotalX;
                        TmpTotalX = e.TotalX;
                        break;

                    case GestureStatus.Completed:
                        // Store the translation applied during the pan
                        TmpTotalX = 0;
                        if (State == SwitchHorizontalState.Right)
                        {
                            if (_posActuel < -this.BackgroundWidthRequest / 2 / 2)
                            {
                                IsToggled = false;
                                State = SwitchHorizontalState.Left;
                                StateVertical = SwitchVerticalState.Bot;
                                // GoToLeft();
                            }
                            else
                            {
                                IsToggled = true;
                                State = SwitchHorizontalState.Right;
                                StateVertical = SwitchVerticalState.Top;
                                GoToRight();
                            }
                        }
                        else
                        {
                            if (_posActuel > this.BackgroundWidthRequest / 2 / 2)
                            {
                                IsToggled = true;
                                State = SwitchHorizontalState.Right;
                                StateVertical = SwitchVerticalState.Top;
                                //   GoToRight();
                            }
                            else
                            {
                                IsToggled = false;
                                State = SwitchHorizontalState.Left;
                                StateVertical = SwitchVerticalState.Bot;
                                GoToLeft();
                            }
                        }
                        _posActuel = 0;
                        break;
                }
            }
            isBusy = false;
        }

        private void GoToLeft()
        {
            _iconSwitch.Content = LeftView;
            _translation = x + this.WidthRequest / 2 + 3;
            _iconSwitch.TranslateTo(-_translation, 0, 100);
            State = SwitchHorizontalState.Left;
            StateVertical = SwitchVerticalState.Bot;
            this.Toggled?.Invoke(this, new ToggledEventArgs(IsToggled));
        }

        private void GoToRight()
        {
            _iconSwitch.Content = RightView;
            _iconSwitch.TranslateTo(-x, 0, 100);
            State = SwitchHorizontalState.Right;
            StateVertical = SwitchVerticalState.Top;
            this.Toggled?.Invoke(this, new ToggledEventArgs(IsToggled));
        }

        public event EventHandler<ToggledEventArgs> Toggled;
    }
}