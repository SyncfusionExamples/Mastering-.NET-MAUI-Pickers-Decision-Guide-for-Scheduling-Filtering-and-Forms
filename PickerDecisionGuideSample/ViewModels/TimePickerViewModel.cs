using Syncfusion.Maui.Core;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PickerDecisionGuideSample.ViewModels
{
    public class TimePickerCustomizationViewModel : INotifyPropertyChanged
    {
        private bool isLightTheme = Application.Current?.RequestedTheme == AppTheme.Light;

        private ObservableCollection<AlarmDetails> alarmCollection;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<AlarmDetails> AlarmCollection
        {
            get
            {
                return alarmCollection;
            }
            set
            {
                alarmCollection = value;
                RaisePropertyChanged("AlarmCollection");
            }
        }

        public TimePickerCustomizationViewModel()
        {
            this.alarmCollection = new ObservableCollection<AlarmDetails>()
            {
                new AlarmDetails() { AlarmTime = new TimeSpan(4, 0, 0), AlarmMessage = "Wake Up", IsAlarmEnabled = true, AlarmTextColor = isLightTheme ? Colors.Black : Colors.White, AlarmSecondaryTextColor= isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0") },
                new AlarmDetails() { AlarmTime = new TimeSpan(5, 0, 0), AlarmMessage = "Morning Workout", IsAlarmEnabled = true, AlarmTextColor = isLightTheme ? Colors.Black : Colors.White, AlarmSecondaryTextColor= isLightTheme ? Color.FromArgb("#49454F") : Color.FromArgb("#CAC4D0") },
                new AlarmDetails() { AlarmTime = new TimeSpan(13, 0, 0), AlarmMessage = "No Alarm Message", IsAlarmEnabled = false, AlarmTextColor = isLightTheme ? Colors.Black.WithAlpha(0.5f) : Colors.White.WithAlpha(0.5f), AlarmSecondaryTextColor = isLightTheme ? Color.FromArgb("#49454F").WithAlpha(0.5f) : Color.FromArgb("#CAC4D0").WithAlpha(0.5f) },
            };
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlarmTimer : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                timeSpan = currentTime.Hours >= timeSpan.Hours ? timeSpan.Add(new TimeSpan(24, 0, 0)) : timeSpan;
                var timeDifference = timeSpan.Subtract(currentTime);
                if (timeDifference.Minutes == 0 && timeDifference.Hours == 0)
                {
                    return $"Alarm in {timeDifference.Seconds} seconds";
                }
                else
                {
                    return $"Alarm in {timeDifference.Hours} hours {timeDifference.Minutes} minutes";
                }
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class AlarmDetails : INotifyPropertyChanged
    {
        private TimeSpan alarmTime;
        private string alarmMessage = string.Empty;
        private bool isAlarmEnabled = true;
        private Color alarmTextColor = Colors.Black;
        private Color alarmSecondaryTextColor = Color.FromArgb("#49454F");

        public event PropertyChangedEventHandler? PropertyChanged;

        public TimeSpan AlarmTime
        {
            get
            {
                return alarmTime;
            }
            set
            {
                alarmTime = value;
                RaisePropertyChanged("AlarmTime");
            }
        }

        public string AlarmMessage
        {
            get
            {
                return alarmMessage;
            }
            set
            {
                alarmMessage = value;
                RaisePropertyChanged("AlarmMessage");
            }
        }

        public bool IsAlarmEnabled
        {
            get
            {
                return isAlarmEnabled;
            }
            set
            {
                isAlarmEnabled = value;
                RaisePropertyChanged("IsAlarmEnabled");
            }
        }

        public Color AlarmTextColor
        {
            get
            {
                return alarmTextColor;
            }
            set
            {
                alarmTextColor = value;
                RaisePropertyChanged("AlarmTextColor");
            }
        }

        public Color AlarmSecondaryTextColor
        {
            get
            {
                return alarmSecondaryTextColor;
            }
            set
            {
                alarmSecondaryTextColor = value;
                RaisePropertyChanged("AlarmSecondaryTextColor");
            }
        }


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlarmPopup : SfPopup
    {
        private Syncfusion.Maui.Picker.SfTimePicker alarmTimePicker;
        private Entry alarmMessageEntry;
        private SfTextInputLayout alarmTextInput;

        public event EventHandler? OnCreated;
        public AlarmPopup()
        {
            this.alarmTimePicker = new Syncfusion.Maui.Picker.SfTimePicker();
            StackLayout stack = new StackLayout();
            stack.Padding = 15;
            Label label = new Label();
            label.Text = "Alarm Message";
            label.Margin = new Thickness(10, 4);
            label.FontSize = 12;
            stack.Add(label);
            this.alarmTextInput = new SfTextInputLayout();
            this.alarmTextInput.Padding = new Thickness(0);
            this.alarmTextInput.Hint = "Alarm Message";
            this.alarmTextInput.HelperText = "Enter Alarm Message";
            this.alarmMessageEntry = new Entry();
            this.alarmMessageEntry.HeightRequest = 40;
            this.alarmMessageEntry.Text = string.Empty;
            this.alarmMessageEntry.Margin = new Thickness(5, 0);
            this.alarmTextInput.Content = this.alarmMessageEntry;
            stack.Add(this.alarmTextInput);
            Label label1 = new Label();
            label1.Text = "Alarm Time";
            label1.FontSize = 12;
            label1.Margin = new Thickness(10, 5);
            stack.Add(label1);
            this.alarmTimePicker.FooterView.Height = 50;
            this.alarmTimePicker.HeightRequest = 300;
            this.alarmTimePicker.Format = Syncfusion.Maui.Picker.PickerTimeFormat.h_mm_tt;
            this.alarmTimePicker.OkButtonClicked += AlarmTimePicker_OkButtonClicked;
            this.alarmTimePicker.CancelButtonClicked += AlarmTimePicker_CancelButtonClicked;
            stack.Add(this.alarmTimePicker);
            stack.VerticalOptions = LayoutOptions.Center;
            this.ContentTemplate = new DataTemplate(() =>
            {
                return stack;
            });

            this.HeaderTemplate = new DataTemplate(() =>
            {
                return new Label() { Text = "Set Alarm", FontSize = 20, HeightRequest = 40, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
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

        private void AlarmTimePicker_CancelButtonClicked(object? sender, EventArgs e)
        {
            this.Reset();
            this.IsOpen = false;
        }

        private void AlarmTimePicker_OkButtonClicked(object? sender, EventArgs e)
        {
            if (this.alarmTimePicker.SelectedTime != null)
            {
                this.OnCreated?.Invoke(new AlarmDetails() { AlarmTime = this.alarmTimePicker.SelectedTime.Value, AlarmMessage = this.alarmMessageEntry.Text == string.Empty ? "No Alarm Message" : this.alarmMessageEntry.Text, IsAlarmEnabled = true }, new EventArgs());

            }
            this.IsOpen = false;
            this.Reset();
        }

        public void Reset()
        {
            this.alarmTimePicker.SelectedTime = DateTime.Now.TimeOfDay;
            this.alarmMessageEntry.Text = string.Empty;
            this.alarmMessageEntry.Placeholder = "Enter Alarm Message";
        }
    }

    public class TimeSpanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                TimeSpan twelveHrsTime = timeSpan.Hours > 12 || timeSpan.Hours == 0 ? timeSpan.Subtract(new TimeSpan(12, 0, 0)) : timeSpan;
                if (timeSpan.Hours > 12)
                {
                    twelveHrsTime = timeSpan.Subtract(new TimeSpan(12, 0, 0));
                }
                else if (timeSpan.Hours == 0)
                {
                    twelveHrsTime = new TimeSpan(12, 0, 0);
                }

                if (parameter is Boolean parameterValue)
                {
                    if (parameterValue)
                    {
                        return $"{twelveHrsTime.Hours}:{timeSpan.Minutes:D2}";
                    }
                    else
                    {
                        return $"{((timeSpan.Hours < 12) ? " AM" : " PM")}";
                    }
                }
                else
                {
                    return $"{twelveHrsTime.Hours}:{timeSpan.Minutes:D2} {((timeSpan.Hours < 12) ? " AM" : " PM")}";
                }
            }

            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
