using Syncfusion.Maui.Toolkit.Popup;
using System.Globalization;

namespace PickerDecisionGuideSample.Pages
{
    public partial class PickerPage : ContentPage
    {
        // Nullable dates to represent "no selection".
        private DateTime? from;
        private DateTime? to;

        // Current From/To selection [Country, City] (empty until user picks).
        private List<string> fromList;
        private List<string> toList;

        // Master data.
        private static readonly List<string> countries = new() { "UK", "USA", "India", "UAE", "Germany" };
        private static readonly List<string> ukCities = new() { "London", "Manchester", "Cambridge", "Edinburgh", "Glasgow", "Birmingham" };
        private static readonly List<string> usaCities = new() { "New York", "Seattle", "Washington", "Chicago", "Boston", "Los Angles" };
        private static readonly List<string> indiaCities = new() { "Mumbai", "Bengaluru", "Chennai", "Pune", "Jaipur", "Delhi" };
        private static readonly List<string> uaeCities = new() { "Dubai", "Abu Dhabi", "Fujairah", "Sharjah", "Ajman", "AL Ain" };
        private static readonly List<string> germanyCities = new() { "Berlin", "Munich", "Frankfurt", "Hamburg", "Cologne", "Bonn" };

        private const string PlacePlaceholder = "Select place";
        private const string DatePlaceholder = "Select date";

        public PickerPage()
        {
            InitializeComponent();

            if (this.popup != null)
            {
                popup.FooterTemplate = this.GetFooterTemplate(popup);
                popup.ContentTemplate = this.GetContentTemplate(popup);
            }

            // Start with no selections.
            from = null;
            to = null;
            fromList = new List<string>();
            toList = new List<string>();

            // Set friendly placeholders.
#if ANDROID || IOS
            if (mobileFromLabel != null) mobileFromLabel.Text = PlacePlaceholder;
            if (mobileToLabel != null) mobileToLabel.Text = PlacePlaceholder;
            if (mobileDepartureDateLabel != null) mobileDepartureDateLabel.Text = DatePlaceholder;
            if (mobileReturnDateLabel != null) mobileReturnDateLabel.Text = DatePlaceholder;
#else
            if (fromLabel != null) fromLabel.Text = PlacePlaceholder;
            if (toLabel != null) toLabel.Text = PlacePlaceholder;
            if (departureDateLabel != null) departureDateLabel.Text = DatePlaceholder;
            if (returnDateLabel != null) returnDateLabel.Text = DatePlaceholder;
#endif

            // Build pickers with NO preselected rows (SelectedIndex = -1).
            var fromCountryColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn()
            {
                HeaderText = "Country",
                SelectedIndex = -1,
                ItemsSource = countries,
                Width = 150
            };
            var fromCityColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn()
            {
                HeaderText = "City",
                SelectedIndex = -1,
                ItemsSource = new List<string>(),
                Width = 150
            };

            var toCountryColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn()
            {
                HeaderText = "Country",
                SelectedIndex = -1,
                ItemsSource = countries,
                Width = 150
            };
            var toCityColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn()
            {
                HeaderText = "City",
                SelectedIndex = -1,
                ItemsSource = new List<string>(),
                Width = 150
            };

#if ANDROID || IOS
            mobileFromPicker.Columns = new System.Collections.ObjectModel.ObservableCollection<Syncfusion.Maui.Toolkit.Picker.PickerColumn>() { fromCountryColumn, fromCityColumn };
            mobileToPicker.Columns = new System.Collections.ObjectModel.ObservableCollection<Syncfusion.Maui.Toolkit.Picker.PickerColumn>() { toCountryColumn, toCityColumn };
            this.mobileGrid.IsVisible = true;
#else
            fromPicker.Columns = new System.Collections.ObjectModel.ObservableCollection<Syncfusion.Maui.Toolkit.Picker.PickerColumn>() { fromCountryColumn, fromCityColumn };
            toPicker.Columns = new System.Collections.ObjectModel.ObservableCollection<Syncfusion.Maui.Toolkit.Picker.PickerColumn>() { toCountryColumn, toCityColumn };
            this.frame.IsVisible = true;
#endif

            // Wire date pickers (no defaults; don’t set SelectedDate in Opened).
#if ANDROID || IOS
            mobileDepartureDatePicker.OkButtonClicked += DepartureDatePicker_OkButtonClicked;
            mobileReturnDatePicker.OkButtonClicked += ReturnDatePicker_OkButtonClicked;
            mobileDepartureDatePicker.CancelButtonClicked += DepartureDatePicker_CancelButtonClicked;
            mobileReturnDatePicker.CancelButtonClicked += ReturnDatePicker_CancelButtonClicked;

            mobileDepartureDatePicker.Opened += DepartureDatePicker_OnPopUpOpened;
            mobileReturnDatePicker.Opened += ReturnDatePicker_OnPopUpOpened;

            mobileDepartureDatePicker.MinimumDate = DateTime.Today;

            mobileFromPicker.HeaderView.Height = 40;
            mobileFromPicker.HeaderView.Text = "FROM";
            mobileFromPicker.FooterView.Height = 40;

            mobileToPicker.HeaderView.Height = 40;
            mobileToPicker.HeaderView.Text = "TO";
            mobileToPicker.FooterView.Height = 40;

            mobileDepartureDatePicker.HeaderView.Height = 40;
            mobileDepartureDatePicker.HeaderView.Text = "Select a date";
            mobileDepartureDatePicker.FooterView.Height = 40;

            mobileReturnDatePicker.HeaderView.Height = 40;
            mobileReturnDatePicker.HeaderView.Text = "Select a date";
            mobileReturnDatePicker.FooterView.Height = 40;
#else
            departureDatePicker.OkButtonClicked += DepartureDatePicker_OkButtonClicked;
            returnDatePicker.OkButtonClicked += ReturnDatePicker_OkButtonClicked;
            departureDatePicker.CancelButtonClicked += DepartureDatePicker_CancelButtonClicked;
            returnDatePicker.CancelButtonClicked += ReturnDatePicker_CancelButtonClicked;

            departureDatePicker.Opened += DepartureDatePicker_OnPopUpOpened;
            returnDatePicker.Opened += ReturnDatePicker_OnPopUpOpened;

            departureDatePicker.MinimumDate = DateTime.Today;

            fromPicker.HeaderView.Height = 40;
            fromPicker.HeaderView.Text = "FROM";
            fromPicker.FooterView.Height = 40;

            toPicker.HeaderView.Height = 40;
            toPicker.HeaderView.Text = "TO";
            toPicker.FooterView.Height = 40;

            departureDatePicker.HeaderView.Height = 40;
            departureDatePicker.HeaderView.Text = "Select a date";
            departureDatePicker.FooterView.Height = 40;

            returnDatePicker.HeaderView.Height = 40;
            returnDatePicker.HeaderView.Text = "Select a date";
            returnDatePicker.FooterView.Height = 40;
#endif
        }

        // Popup-open handlers (do not preselect, do not set SelectedDate).
        private void DepartureDatePicker_OnPopUpOpened(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileDepartureDatePicker.IsOpen = true;
            mobileDepartureDatePicker.MinimumDate = DateTime.Today;
#else
            departureDatePicker.IsOpen = true;
            departureDatePicker.MinimumDate = DateTime.Today;
#endif
        }

        private void ReturnDatePicker_OnPopUpOpened(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileReturnDatePicker.IsOpen = true;
            mobileReturnDatePicker.MinimumDate = from ?? DateTime.Today;
#else
            returnDatePicker.IsOpen = true;
            returnDatePicker.MinimumDate = from ?? DateTime.Today;
#endif
        }

        // Departure OK: set value and label only if user picked a date.
        private void DepartureDatePicker_OkButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            if (mobileDepartureDatePicker.SelectedDate.HasValue)
            {
                from = mobileDepartureDatePicker.SelectedDate.Value.Date;
                if (mobileDepartureDateLabel != null)
                    mobileDepartureDateLabel.Text = from.Value.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);

                // If return exists and is invalid now, clear it back to placeholder.
                if (to.HasValue && to.Value.Date < from.Value.Date)
                {
                    to = null;
                    if (mobileReturnDateLabel != null) mobileReturnDateLabel.Text = DatePlaceholder;
                }
            }
            mobileDepartureDatePicker.IsOpen = false;
#else
            if (departureDatePicker.SelectedDate.HasValue)
            {
                from = departureDatePicker.SelectedDate.Value.Date;
                if (departureDateLabel != null)
                    departureDateLabel.Text = from.Value.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);

                if (to.HasValue && to.Value.Date < from.Value.Date)
                {
                    to = null;
                    if (returnDateLabel != null) returnDateLabel.Text = DatePlaceholder;
                }
            }
            departureDatePicker.IsOpen = false;
#endif
        }

        // Return OK: set value and label only if user picked a date.
        private void ReturnDatePicker_OkButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            if (mobileReturnDatePicker.SelectedDate.HasValue)
            {
                to = mobileReturnDatePicker.SelectedDate.Value.Date;
                if (mobileReturnDateLabel != null)
                    mobileReturnDateLabel.Text = to.Value.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
            }
            mobileReturnDatePicker.IsOpen = false;
#else
            if (returnDatePicker.SelectedDate.HasValue)
            {
                to = returnDatePicker.SelectedDate.Value.Date;
                if (returnDateLabel != null)
                    returnDateLabel.Text = to.Value.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
            }
            returnDatePicker.IsOpen = false;
#endif
        }

        private void DepartureDatePicker_CancelButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileDepartureDatePicker.IsOpen = false;
#else
            departureDatePicker.IsOpen = false;
#endif
        }

        private void ReturnDatePicker_CancelButtonClicked(object? sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileReturnDatePicker.IsOpen = false;
#else
            returnDatePicker.IsOpen = false;
#endif
        }

        // Tap to open From picker (no preselect).
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileFromPicker.IsOpen = true;
#else
            fromPicker.IsOpen = true;
#endif
        }

        // Tap to open To picker (no preselect).
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileToPicker.IsOpen = true;
#else
            toPicker.IsOpen = true;
#endif
        }

        // Cascading: when Country changes, rebuild City column. No auto selection.
        private void FromPicker_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.Picker.PickerSelectionChangedEventArgs e)
        {
            if (e.ColumnIndex == 1) return; // ignore city changes here

            if (e.NewValue < 0)
            {
                var emptyCityColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn
                {
                    HeaderText = "City",
                    SelectedIndex = -1,
                    ItemsSource = new List<string>(),
                    Width = 150
                };
#if ANDROID || IOS
                mobileFromPicker.Columns[1] = emptyCityColumn;
#else
                fromPicker.Columns[1] = emptyCityColumn;
#endif
                return;
            }

            string country = countries[e.NewValue];
            var cities = GetCityList(country);
            var cityColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn
            {
                HeaderText = "City",
                SelectedIndex = -1,
                ItemsSource = cities,
                Width = 150
            };
#if ANDROID || IOS
            mobileFromPicker.Columns[1] = cityColumn;
#else
            fromPicker.Columns[1] = cityColumn;
#endif
        }

        private void ToPicker_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.Picker.PickerSelectionChangedEventArgs e)
        {
            if (e.ColumnIndex == 1) return;

            if (e.NewValue < 0)
            {
                var emptyCityColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn
                {
                    HeaderText = "City",
                    SelectedIndex = -1,
                    ItemsSource = new List<string>(),
                    Width = 150
                };
#if ANDROID || IOS
                mobileToPicker.Columns[1] = emptyCityColumn;
#else
                toPicker.Columns[1] = emptyCityColumn;
#endif
                return;
            }

            string country = countries[e.NewValue];
            var cities = GetCityList(country);
            var cityColumn = new Syncfusion.Maui.Toolkit.Picker.PickerColumn
            {
                HeaderText = "City",
                SelectedIndex = -1,
                ItemsSource = cities,
                Width = 150
            };
#if ANDROID || IOS
            mobileToPicker.Columns[1] = cityColumn;
#else
            toPicker.Columns[1] = cityColumn;
#endif
        }

        // Cancel buttons for pickers
        private void FromPicker_CancelButtonClicked(object sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileFromPicker.IsOpen = false;
#else
            fromPicker.IsOpen = false;
#endif
        }

        private void ToPicker_CancelButtonClicked(object sender, EventArgs e)
        {
#if ANDROID || IOS
            mobileToPicker.IsOpen = false;
#else
            toPicker.IsOpen = false;
#endif
        }

        // OK buttons for pickers: only set label if both country and city chosen.
        private void FromPicker_OkButtonClicked(object sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Toolkit.Picker.SfPicker picker)
            {
                int countryIdx = picker.Columns[0].SelectedIndex;
                int cityIdx = picker.Columns[1].SelectedIndex;

                if (countryIdx < 0 || cityIdx < 0)
                {
#if ANDROID || IOS
                    mobileFromPicker.IsOpen = false;
#else
                    fromPicker.IsOpen = false;
#endif
                    return;
                }

                string country = countries[countryIdx];
                var cities = GetCityList(country);
                string city = cities[cityIdx];
                fromList = new List<string> { country, city };

#if ANDROID || IOS
                if (mobileFromLabel != null) mobileFromLabel.Text = $"{city}, {country}";
                mobileFromPicker.IsOpen = false;
#else
                if (fromLabel != null) fromLabel.Text = $"{city}, {country}";
                fromPicker.IsOpen = false;
#endif
            }
        }

        private void ToPicker_OkButtonClicked(object sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Toolkit.Picker.SfPicker picker)
            {
                int countryIdx = picker.Columns[0].SelectedIndex;
                int cityIdx = picker.Columns[1].SelectedIndex;

                if (countryIdx < 0 || cityIdx < 0)
                {
#if ANDROID || IOS
                    mobileToPicker.IsOpen = false;
#else
                    toPicker.IsOpen = false;
#endif
                    return;
                }

                string country = countries[countryIdx];
                var cities = GetCityList(country);
                string city = cities[cityIdx];
                toList = new List<string> { country, city };

#if ANDROID || IOS
                if (mobileToLabel != null) mobileToLabel.Text = $"{city}, {country}";
                mobileToPicker.IsOpen = false;
#else
                if (toLabel != null) toLabel.Text = $"{city}, {country}";
                toPicker.IsOpen = false;
#endif
            }
        }

        // Data helper
        private List<string> GetCityList(string country)
        {
            return country switch
            {
                "UK" => ukCities,
                "USA" => usaCities,
                "India" => indiaCities,
                "UAE" => uaeCities,
                "Germany" => germanyCities,
                _ => new List<string>()
            };
        }

        // Popup templates
        private Style GetOkButtonStyle()
        {
            return new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.CornerRadiusProperty, Value = 20 },
                    new Setter { Property = Button.BorderColorProperty, Value = Color.FromArgb("#6750A4") },
                    new Setter { Property = Button.BorderWidthProperty, Value = 1 },
                    new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromArgb("#6750a4") },
                    new Setter { Property = Button.TextColorProperty, Value = Colors.White },
                    new Setter { Property = Button.FontSizeProperty, Value = 14 },
                }
            };
        }

        private DataTemplate GetFooterTemplate(SfPopup popup)
        {
            var footerTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
                {
                    ColumnSpacing = 12,
                    Padding = new Thickness(24)
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                var oKButton = new Button
                {
                    Text = "OK",
                    Style = this.GetOkButtonStyle(),
                    WidthRequest = 96,
                    HeightRequest = 40
                };
                oKButton.Clicked += (sender, args) => popup.Dismiss();
                grid.Children.Add(oKButton);
                Grid.SetColumn(oKButton, 1);
                return grid;
            });
            return footerTemplate;
        }

        private DataTemplate GetContentTemplate(SfPopup popup)
        {
            var contentTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.1, GridUnitType.Star) });

                var label = new Label
                {
                    LineBreakMode = LineBreakMode.WordWrap,
                    Padding = new Thickness(24, 24, 0, 0),
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start
                };
                label.BindingContext = popup;
                label.SetBinding(Label.TextProperty, "Message");

                var divider = new StackLayout
                {
                    Margin = new Thickness(0, 10, 0, 0),
                    HeightRequest = 1,
                    BackgroundColor = Color.FromArgb("#611c1b1f")
                };

                grid.Children.Add(label);
                grid.Children.Add(divider);
                Grid.SetRow(label, 0);
                Grid.SetRow(divider, 1);
                return grid;
            });
            return contentTemplate;
        }

        // Search: block until everything is selected.
        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            if (this.popup == null) return;

            bool hasFrom = fromList.Count == 2;
            bool hasTo = toList.Count == 2;
            bool hasDates = from.HasValue && to.HasValue;

            if (!hasFrom || !hasTo || !hasDates)
            {
                this.popup.Message = "Please select From, To, Departure, and Return before searching.";
                this.popup.Show();
                return;
            }

            var randomNumber = new Random();
            int index = randomNumber.Next(0, 50);

#if ANDROID || IOS
            this.popup.Message = $"{index} flights available for the selected route and dates. From: {mobileFromLabel.Text}";
#else
            this.popup.Message = $"{index} flights available for the selected route and dates. From: {fromLabel.Text}";
#endif
            this.popup.Show();
        }
    }
}