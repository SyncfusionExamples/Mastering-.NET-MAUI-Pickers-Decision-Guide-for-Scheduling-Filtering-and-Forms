using System.Globalization;
using Syncfusion.Maui.Toolkit.Picker;

namespace PickerDecisionGuideSample.Pages
{
    public partial class DateTimePickersPage : ContentPage
    {
        public DateTimePickersPage()
        {
            InitializeComponent();
            this.reminderDateTimePicker.HeaderView.Height = 50;
            this.reminderDateTimePicker.FooterView.Height = 40;
        }

        // Open the picker; optionally preselect from current text if it’s a valid value
        private void OnDateTimeFieldTapped(object sender, EventArgs e)
        {
            // Try to parse existing text only if not empty and not just the placeholder
            var txt = dateTimeEntry.Text;
            if (!string.IsNullOrWhiteSpace(txt) && !string.Equals(txt, "Select date and time", StringComparison.Ordinal))
            {
                if (DateTime.TryParseExact(txt, "dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                {
                    reminderDateTimePicker.SelectedDate = dt;
                }
                else
                {
                    reminderDateTimePicker.SelectedDate = DateTime.Now;
                }
            }
            else
            {
                reminderDateTimePicker.SelectedDate = DateTime.Now;
            }

            reminderDateTimePicker.IsOpen = true;
        }

        // After OK, write back to the actual Entry
        private void OnDateTimePickerOkButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker picker && picker.SelectedDate != null)
            {
                dateTimeEntry.Text = picker.SelectedDate.Value.ToString("dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);
            }
            reminderDateTimePicker.IsOpen = false;
        }

        private void OnDateTimePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker p)
                p.IsOpen = false;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var title = titleEntry.Text;
            var dateTimeString = dateTimeEntry.Text;
            await DisplayAlert("Reminder", $"Title: {title}\nDateTime: {dateTimeString}", "OK");
        }
    }
}