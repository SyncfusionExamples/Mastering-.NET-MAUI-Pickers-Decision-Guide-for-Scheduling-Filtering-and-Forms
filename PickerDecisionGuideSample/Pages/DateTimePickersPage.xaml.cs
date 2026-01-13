using System.Globalization;
using Syncfusion.Maui.Toolkit.Picker;

namespace PickerDecisionGuideSample.Pages
{
    /// <summary>
    /// Page demonstrating a single Syncfusion date-time picker integrated with a text entry field.
    /// Provides tap-to-open behavior, OK/Cancel actions, and a simple save confirmation.
    /// </summary>
    public partial class DateTimePickersPage : ContentPage
    {
        /// <summary>
        /// Initializes the page and configures header and footer sizes for the date-time picker popup.
        /// </summary>
        public DateTimePickersPage()
        {
            InitializeComponent();
            this.reminderDateTimePicker.HeaderView.Height = 50;
            this.reminderDateTimePicker.FooterView.Height = 40;
        }

        /// <summary>
        /// Opens the date-time picker. If the entry already has a valid value, preselect it; otherwise, use now.
        /// </summary>
        /// <param name="sender">The field being tapped.</param>
        /// <param name="e">Event data.</param>
        private void OnDateTimeFieldTapped(object sender, EventArgs e)
        {
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

        /// <summary>
        /// Confirms the selected date and time and updates the entry text. Closes the picker.
        /// </summary>
        /// <param name="sender">The picker raising the event.</param>
        /// <param name="e">Event data.</param>
        private void OnDateTimePickerOkButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker picker && picker.SelectedDate != null)
            {
                dateTimeEntry.Text = picker.SelectedDate.Value.ToString("dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);
            }
            reminderDateTimePicker.IsOpen = false;
        }

        /// <summary>
        /// Cancels date-time selection and closes the popup without changes.
        /// </summary>
        /// <param name="sender">The picker instance.</param>
        /// <param name="e">Event data.</param>
        private void OnDateTimePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker p)
                p.IsOpen = false;
        }

        /// <summary>
        /// Displays a confirmation dialog with the entered title and formatted date-time.
        /// </summary>
        /// <param name="sender">The save button.</param>
        /// <param name="e">Event data.</param>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var title = titleEntry.Text;
            var dateTimeString = dateTimeEntry.Text;
            await DisplayAlertAsync("Reminder", $"Title: {title}\nDateTime: {dateTimeString}", "OK");
        }
    }
}
