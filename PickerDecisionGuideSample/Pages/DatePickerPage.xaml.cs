using Syncfusion.Maui.Core;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PickerDecisionGuideSample.Pages
{
    /// <summary>
    /// Page that demonstrates task management with Syncfusion SfDatePicker, including adding tasks and editing due dates.
    /// </summary>
    public partial class DatePickerPage : ContentPage
    {
        private ToDoDetails? toDoDetails;
        /// <summary>
        /// Initializes a new instance of the <see cref="DatePickerPage"/> class and configures picker headers/footers per platform.
        /// </summary>
        public DatePickerPage()
        {
            InitializeComponent();
#if ANDROID || IOS
        this.datePicker1.HeaderView.Height = 50;
        this.datePicker1.HeaderView.Text = "Select the Date";

        this.datePicker1.FooterView.Height = 50;
#else
            this.datePicker.HeaderView.Height = 50;
            this.datePicker.HeaderView.Text = "Select the Date";

            this.datePicker.FooterView.Height = 50;
#endif
        }

        /// <summary>
        /// Handles the floating add button tap and opens the custom popup to create a new task.
        /// </summary>
        private void OnTapGestureTapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
        this.popup1.Reset();
        this.popup1.IsOpen = true;
#else
            this.popup.Reset();
            this.popup.IsOpen = true;
#endif
        }

        /// <summary>
        /// Handles tapping an item and opens SfDatePicker to edit the task due date.
        /// </summary>
        private void OnItemTapGestureTapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
        if (sender is Grid grid && grid.BindingContext != null && grid.BindingContext is ToDoDetails details)
        {
            this.datePicker1.SelectedDate = details.Date;
            this.toDoDetails = details;
        }

        this.datePicker1.IsOpen = true;
#else
            if (sender is Grid grid && grid.BindingContext != null && grid.BindingContext is ToDoDetails details)
            {
                this.datePicker.SelectedDate = details.Date;
                this.toDoDetails = details;
            }

            this.datePicker.IsOpen = true;
#endif
        }

        /// <summary>
        /// Applies the selected date to the tapped task and closes the picker.
        /// </summary>
        private void OnDatePickerOkButtonClicked(object sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Picker.SfDatePicker picker && this.toDoDetails != null && picker.SelectedDate?.Date != null)
            {
                if (this.toDoDetails.Date != picker.SelectedDate)
                {
                    this.toDoDetails.Date = picker.SelectedDate.Value.Date;
                }

                this.toDoDetails = null;
            }

#if ANDROID || IOS
        this.datePicker1.IsOpen = false;
#else
            this.datePicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Resets the current edit context when the picker closes.
        /// </summary>
        private void OnDatePickerClosed(object sender, EventArgs e)
        {
            this.toDoDetails = null;
        }

        /// <summary>
        /// Cancels editing and closes the picker.
        /// </summary>
        private void OnDatePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Picker.SfDatePicker picker)
            {
                this.toDoDetails = null;
                picker.IsOpen = false;
            }
        }

        /// <summary>
        /// Adds a newly created item from the popup to the collection.
        /// </summary>
        private void OnPopupItemCreated(object sender, EventArgs e)
        {
            if (this.BindingContext != null && this.BindingContext is DatePickerCustomizationViewModel bindingContext && sender is ToDoDetails details)
            {
                bindingContext.DataSource.Add(details);
            }
        }
    }

    /// <summary>
    /// ViewModel providing data for the DatePickerPage tasks list.
    /// </summary>
    public class DatePickerCustomizationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ToDoDetails> dataSource;

        /// <summary>
        /// Gets or sets the task items displayed in the page.
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

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes the ViewModel with sample task data.
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
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    /// <summary>
    /// Model representing a task item with subject and due date information.
    /// </summary>
    public class ToDoDetails : INotifyPropertyChanged
    {
        private string subject = string.Empty;

        /// <summary>
        /// Gets or sets the subject/title of the task.
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
        /// Gets or sets the due date of the task and updates DateString accordingly.
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
        /// Gets or sets a formatted string representing the due date.
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
    /// Converts a DateTime to a themed Color indicating status (today, past, future).
    /// </summary>
    public class DateTimeToColorConverter : IValueConverter
    {
        private bool isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        /// <summary>
        /// Converts DateTime to a status color using the current theme.
        /// </summary>
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
        /// Not used. Conversion back is not supported.
        /// </summary>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Custom popup used to create a new task with title and date using SfDatePicker.
    /// </summary>
    public class CustomPopUp : SfPopup
    {
        private Syncfusion.Maui.Picker.SfDatePicker picker;
        Entry entry;
        private SfTextInputLayout textInput;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPopUp"/> class and composes its UI.
        /// </summary>
        public CustomPopUp()
        {
            this.picker = new Syncfusion.Maui.Picker.SfDatePicker();
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
            this.picker.FooterView.Height = 40;
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
        /// Resets popup fields and closes the popup when cancel is clicked.
        /// </summary>
        private void OnPickerCancelButtonClicked(object? sender, EventArgs e)
        {
            this.Reset();
            this.IsOpen = false;
        }

        /// <summary>
        /// Raises OnCreated with the new task when OK is clicked and closes the popup.
        /// </summary>
        private void OnPickerOkButtonClicked(object? sender, EventArgs e)
        {
            if (picker.SelectedDate != null)
            {
                this.OnCreated?.Invoke(new ToDoDetails() { Date = this.picker.SelectedDate.Value, Subject = this.entry.Text == string.Empty ? "No Title" : this.entry.Text }, new EventArgs());
            }
            this.IsOpen = false;
        }

        /// <summary>
        /// Clears the editor and resets the date selection to today.
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
        /// Occurs when a new task is created from the popup.
        /// </summary>
        public event EventHandler? OnCreated;
    }
}
