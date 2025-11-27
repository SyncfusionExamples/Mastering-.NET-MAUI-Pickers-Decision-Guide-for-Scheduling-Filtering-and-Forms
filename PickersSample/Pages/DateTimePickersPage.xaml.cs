using System.Globalization;
using Syncfusion.Maui.Picker;

namespace PickersSample.Pages
{
    public partial class DateTimePickersPage : ContentPage
    {
        public DateTimePickersPage()
        {
            InitializeComponent();

            dateTimeText.Text = DateTime.Now.ToString("dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);

#if ANDROID || IOS
            this.reminderDateTimePicker.HeaderView.Height = 50;
            this.reminderDateTimePicker.FooterView.Height = 40;
#else
            this.reminderDateTimePicker.HeaderView.Height = 50;
            this.reminderDateTimePicker.FooterView.Height = 40;
#endif
        }

        private void OnDateTimeFieldTapped(object sender, EventArgs e)
        {
            this.reminderDateTimePicker.SelectedDate = DateTime.TryParseExact(dateTimeText.Text, "dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                ? dt
                : DateTime.Now;
            this.reminderDateTimePicker.IsOpen = true;
        }

        private void OnDateTimePickerOkButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker picker && picker.SelectedDate != null)
            {
                dateTimeText.Text = picker.SelectedDate.Value.ToString("dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);
            }
            this.reminderDateTimePicker.IsOpen = false;
        }

        private void OnDateTimePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker p)
            {
                p.IsOpen = false;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var title = titleEntry.Text;
            var dateTimeString = dateTimeText.Text;
            await DisplayAlert("Reminder", $"Title: {title}\nDateTime: {dateTimeString}", "OK");
        }
    }
}
