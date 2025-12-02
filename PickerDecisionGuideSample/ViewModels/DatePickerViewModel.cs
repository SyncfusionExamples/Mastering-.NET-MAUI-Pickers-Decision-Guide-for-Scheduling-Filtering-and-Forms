using Syncfusion.Maui.Core;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PickerDecisionGuideSample.ViewModels
{
    public class DatePickerCustomizationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ToDoDetails> dataSource;

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

    public class ToDoDetails : INotifyPropertyChanged
    {
        private string subject = string.Empty;

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

    public class CustomPopUp : SfPopup
    {
        private Syncfusion.Maui.Picker.SfDatePicker picker;
        Entry entry;
        private SfTextInputLayout textInput;

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

        private void OnPickerCancelButtonClicked(object? sender, EventArgs e)
        {
            this.Reset();
            this.IsOpen = false;
        }

        private void OnPickerOkButtonClicked(object? sender, EventArgs e)
        {
            if (picker.SelectedDate != null)
            {
                this.OnCreated?.Invoke(new ToDoDetails() { Date = this.picker.SelectedDate.Value, Subject = this.entry.Text == string.Empty ? "No Title" : this.entry.Text }, new EventArgs());
            }
            this.IsOpen = false;
        }

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

        public event EventHandler? OnCreated;
    }

    public class DateTimeToColorConverter : IValueConverter
    {
        private bool isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

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

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
