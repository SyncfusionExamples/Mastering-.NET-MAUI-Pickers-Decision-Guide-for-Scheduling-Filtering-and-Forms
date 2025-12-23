using Syncfusion.Maui.Core;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PickerDecisionGuideSample.ViewModels
{
    /// <summary>
    /// ViewModel that provides the data source and change notification for DatePicker samples.
    /// </summary>
    public class DatePickerCustomizationViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Backing field for the DataSource property.
        /// </summary>
        private ObservableCollection<ToDoDetails> dataSource;

        /// <summary>
        /// Gets or sets the list of to-do items bound to the UI.
        /// </summary>
        public ObservableCollection<ToDoDetails> DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
                RaisePropertyChanged("DataSource");
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes a new instance of the DatePickerCustomizationViewModel with sample data.
        /// </summary>
        public DatePickerCustomizationViewModel()
        {
            this.dataSource = new ObservableCollection<ToDoDetails>()
            {
                new ToDoDetails() {Subject = "Get quote from travel agent", Date= DateTime.Now.Date},
                new ToDoDetails() {Subject = "Book flight ticket", Date= DateTime.Now.Date.AddDays(2)},
                new ToDoDetails() {Subject = "Buy travel guide book", Date= DateTime.Now.Date},
                new ToDoDetails() {Subject = "Register for sky diving", Date= DateTime.Now.Date.AddDays(8)},
            };
        }

        /// <inheritdoc/>
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    /// <summary>
    /// Model representing a to-do item with a subject and due date.
    /// </summary>
    public class ToDoDetails : INotifyPropertyChanged
    {
        private string subject = string.Empty;

        /// <summary>
        /// Gets or sets the subject or title of the to-do item.
        /// </summary>
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                RaisePropertyChanged("Subject");
            }
        }

        private DateTime date = DateTime.Now.Date;

        /// <summary>
        /// Gets or sets the due date of the to-do item. Updates DateString when set.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                this.DateString = date.Date == DateTime.Now.Date ? "Due today" : date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                RaisePropertyChanged("Date");
            }
        }

        private string dateString = "Due today";

        /// <summary>
        /// Gets or sets the formatted date text shown in the UI.
        /// </summary>
        public string DateString
        {
            get
            {
                return dateString;
            }
            set
            {
                dateString = value;
                RaisePropertyChanged("DateString");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    /// <summary>
    /// Popup for creating a new to-do item with a date and subject.
    /// </summary>
    public class CustomPopUp : SfPopup
    {
        /// <summary>
        /// The internal date picker control used inside the popup.
        /// </summary>
        private Syncfusion.Maui.Toolkit.Picker.SfDatePicker picker;

        /// <summary>
        /// The entry used to capture the to-do subject text.
        /// </summary>
        Entry entry;

        /// <summary>
        /// Text input layout that wraps the subject entry.
        /// </summary>
        private SfTextInputLayout textInput;

        public CustomPopUp()
        {
            this.picker = new Syncfusion.Maui.Toolkit.Picker.SfDatePicker();
            StackLayout stack = new StackLayout();
            stack.Padding = 20;
            Label label = new Label();
            label.Text = "Subject";
            label.Margin = new Thickness(10, 4);
            label.FontSize = 12;
            stack.Add(label);
            this.textInput = new SfTextInputLayout();
            this.textInput.Hint = "Title";
            this.textInput.HelperText = "Enter Title";
            this.entry = new Entry();
            this.entry.HeightRequest = 40;
            this.entry.Margin = new Thickness(5, 0);
            this.textInput.Content = this.entry;
            stack.Add(this.textInput);
            Label label1 = new Label();
            label1.Text = "Select the date";
            label1.FontSize = 12;
            label1.Margin = new Thickness(10, 5);
            stack.Add(label1);
            this.picker.FooterView.Height = 50;
            this.picker.HeightRequest = 280;
            this.picker.OkButtonClicked += OnPickerOkButtonClicked;
            this.picker.CancelButtonClicked += OnPickerCancelButtonClicked;
            stack.Add(this.picker);
            stack.VerticalOptions = LayoutOptions.Center;
            this.ContentTemplate = new DataTemplate(() =>
            {
                return stack;
            });

            this.HeaderTemplate = new DataTemplate(() =>
            {
                return new Label() { Text = "Add a task", FontSize = 20, HeightRequest = 40, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            });

#if ANDROID || IOS || MACCATALYST
            this.HeightRequest = 490;
#else
            this.HeightRequest = 500;
#endif
            this.WidthRequest = 300;
            this.ShowFooter = false;
            this.ShowHeader = true;
            this.HeaderHeight = 40;
            this.PopupStyle.CornerRadius = new CornerRadius(5);
        }

        /// <summary>
        /// Handles the date picker Cancel button by resetting the input and closing the popup.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event data.</param>
        private void OnPickerCancelButtonClicked(object? sender, EventArgs e)
        {
            this.Reset();
            this.IsOpen = false;
        }

        /// <summary>
        /// Handles the date picker OK button by creating a new ToDoDetails and closing the popup.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event data.</param>
        private void OnPickerOkButtonClicked(object? sender, EventArgs e)
        {
            if (picker.SelectedDate != null)
            {
                this.OnCreated?.Invoke(new ToDoDetails() { Date = this.picker.SelectedDate.Value, Subject = this.entry.Text == string.Empty ? "No Title" : this.entry.Text }, new EventArgs());
            }
            this.IsOpen = false;
        }

        /// <summary>
        /// Resets the popup controls to their default values.
        /// </summary>
        public void Reset()
        {
            if (this.picker != null)
            {
                this.picker.SelectedDate = DateTime.Now.Date;
            }

            if (this.entry != null)
            {
                this.entry.Text = string.Empty;
            }
        }

        /// <summary>
        /// Raised when a new to-do item is created from the popup.
        /// </summary>
        public event EventHandler? OnCreated;
    }

    /// <summary>
    /// Converts a date value to a color based on whether it is today, past, or future.
    /// </summary>
    public class DateTimeToColorConverter : IValueConverter
    {
        /// <summary>
        /// Indicates whether the current app theme is light.
        /// </summary>
        private bool isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Converts a DateTime to a themed Color for display.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">Optional parameter.</param>
        /// <param name="culture">Culture info.</param>
        /// <returns>A Color representing the state of the date.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime date)
            {
                if (date.Date == DateTime.Now.Date)
                {
                    return isLightTheme ? Color.FromArgb("#B3261E") : Color.FromArgb("#F2B8B5");
                }
                else if (date.Date < DateTime.Now.Date)
                {
                    return isLightTheme ? Color.FromArgb("#49454F").WithAlpha(0.5f) : Color.FromArgb("#CAC4D0").WithAlpha(0.5f);
                }

                return isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0");
            }

            return isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0");
        }

        /// <summary>
        /// ConvertBack is not used for this converter.
        /// </summary>
        /// <returns>Always returns an empty string.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
