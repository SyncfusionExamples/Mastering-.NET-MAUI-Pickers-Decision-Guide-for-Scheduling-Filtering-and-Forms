using PickerDecisionGuideSample.ViewModels;

namespace PickerDecisionGuideSample.Pages
{
    /// <summary>
    /// Code-behind for the DatePickerPage. Manages Syncfusion SfDatePicker and popup interactions,
    /// handling taps, picker open/close, and updating the bound ToDoDetails model.
    /// </summary>
    public partial class DatePickerPage : ContentPage
    {
        /// <summary>
        /// Holds the currently selected ToDo item while its date is being edited.
        /// </summary>
        private ToDoDetails? toDoDetails;

        /// <summary>
        /// Initializes the page and configures picker header/footer for each platform.
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
        /// Opens the informational popup before the date picker interaction.
        /// </summary>
        /// <param name="sender">Tap source.</param>
        /// <param name="e">Event data.</param>
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
        /// Opens the date picker for the tapped ToDo item and preselects its current date.
        /// </summary>
        /// <param name="sender">Grid bound to a ToDoDetails.</param>
        /// <param name="e">Event data.</param>
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
        /// Confirms the selected date, updates the active ToDo item, and closes the picker.
        /// </summary>
        /// <param name="sender">SfDatePicker instance.</param>
        /// <param name="e">Event data.</param>
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
        /// Clears edit state when the date picker is closed.
        /// </summary>
        /// <param name="sender">Picker.</param>
        /// <param name="e">Event data.</param>
        private void OnDatePickerClosed(object sender, EventArgs e)
        {
            this.toDoDetails = null;
        }

        /// <summary>
        /// Cancels editing and closes the picker without applying changes.
        /// </summary>
        /// <param name="sender">SfDatePicker instance.</param>
        /// <param name="e">Event data.</param>
        private void OnDatePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Picker.SfDatePicker picker)
            {
                this.toDoDetails = null;
                picker.IsOpen = false;
            }
        }

        /// <summary>
        /// Adds dynamically created ToDoDetails items to the view-model's data source.
        /// </summary>
        /// <param name="sender">The created ToDoDetails item.</param>
        /// <param name="e">Event data.</param>
        private void OnPopupItemCreated(object sender, EventArgs e)
        {
            if (this.BindingContext != null && this.BindingContext is DatePickerCustomizationViewModel bindingContext && sender is ToDoDetails details)
            {
                bindingContext.DataSource.Add(details);
            }
        }
    }
}
