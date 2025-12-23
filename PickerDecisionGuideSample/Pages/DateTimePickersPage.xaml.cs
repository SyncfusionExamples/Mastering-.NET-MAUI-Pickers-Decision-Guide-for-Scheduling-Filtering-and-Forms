using System.Globalization;
using Syncfusion.Maui.Toolkit.Picker;

namespace PickerDecisionGuideSample.Pages
{
    /// <summary>
    /// Code-behind for the DateTimePickersPage. Provides an SfDateTimePicker-driven reminder editor
    /// with open/close handling and formatting logic.
    /// </summary>
    public partial class DateTimePickersPage : ContentPage
    {
        /// <summary>
        /// Initializes the page, sets default display text, and configures picker header/footer.
        /// </summary>
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

        /// <summary>
        /// Opens the date-time picker and preselects the current value from the text field.
        /// </summary>
        /// <param name="sender">Tap source.</param>
        /// <param name="e">Event data.</param>
        private void OnDateTimeFieldTapped(object sender, EventArgs e)
        {
            this.reminderDateTimePicker.SelectedDate = DateTime.TryParseExact(dateTimeText.Text, "dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                ? dt
                : DateTime.Now;
            this.reminderDateTimePicker.IsOpen = true;
        }

        /// <summary>
        /// Confirms the selection, formats it, and updates the text field; then closes the picker.
        /// </summary>
        /// <param name="sender">SfDateTimePicker.</param>
        /// <param name="e">Event data.</param>
        private void OnDateTimePickerOkButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker picker && picker.SelectedDate != null)
            {
                dateTimeText.Text = picker.SelectedDate.Value.ToString("dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);
            }
            this.reminderDateTimePicker.IsOpen = false;
        }

        /// <summary>
        /// Cancels the interaction and closes the picker.
        /// </summary>
        /// <param name="sender">SfDateTimePicker.</param>
        /// <param name="e">Event data.</param>
        private void OnDateTimePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is SfDateTimePicker p)
            {
                p.IsOpen = false;
            }
        }

        /// <summary>
        /// Displays a confirmation dialog with the reminder title and selected date-time.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">Event data.</param>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var title = titleEntry.Text;
            var dateTimeString = dateTimeText.Text;
            await DisplayAlertAsync("Reminder", $"Title: {title}\nDateTime: {dateTimeString}", "OK");
        }
    }
}
