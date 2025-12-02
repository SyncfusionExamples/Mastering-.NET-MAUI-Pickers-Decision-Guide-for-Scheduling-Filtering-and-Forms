using PickerDecisionGuideSample.ViewModels;

namespace PickerDecisionGuideSample.Pages
{
    public partial class DatePickerPage : ContentPage
    {
        private ToDoDetails? toDoDetails;

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

        private void OnDatePickerClosed(object sender, EventArgs e)
        {
            this.toDoDetails = null;
        }

        private void OnDatePickerCancelButtonClicked(object sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Picker.SfDatePicker picker)
            {
                this.toDoDetails = null;
                picker.IsOpen = false;
            }
        }

        private void OnPopupItemCreated(object sender, EventArgs e)
        {
            if (this.BindingContext != null && this.BindingContext is DatePickerCustomizationViewModel bindingContext && sender is ToDoDetails details)
            {
                bindingContext.DataSource.Add(details);
            }
        }
    }
}
