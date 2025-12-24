using Syncfusion.Maui.Buttons;
using PickerDecisionGuideSample.ViewModels;

namespace PickerDecisionGuideSample.Pages
{
    /// <summary>
    /// Code-behind for the TimePickerPage. Manages alarm list interactions and an SfTimePicker-based
    /// editor for adjusting alarm times, along with theme-aware visual updates.
    /// </summary>
    public partial class TimePickerPage : ContentPage
    {
        /// <summary>
        /// Holds the alarm currently being edited.
        /// </summary>
        private AlarmDetails? alarmDetails;

        /// <summary>
        /// Initializes the page and configures the time picker header and footer.
        /// </summary>
        public TimePickerPage()
        {
            InitializeComponent();
#if ANDROID || IOS
            this.alarmEditPicker1.HeaderView.Height = 40;
            this.alarmEditPicker1.HeaderView.Text = "Edit Alarm";

            this.alarmEditPicker1.FooterView.Height = 50;
            this.alarmEditPicker1.FooterView.OkButtonText = "Save";
#else
            this.alarmEditPicker.HeaderView.Height = 40;
            this.alarmEditPicker.HeaderView.Text = "Edit Alarm";

            this.alarmEditPicker.FooterView.Height = 50;
            this.alarmEditPicker.FooterView.OkButtonText = "Save";
#endif
        }
        /// <summary>
        /// Opens the alarm editor for the tapped alarm when it is enabled.
        /// </summary>
        /// <param name="sender">Tapped Border bound to an AlarmDetails.</param>
        /// <param name="e">Event data.</param>
        private void OnAlarmTapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
            if (sender is Border border && border.BindingContext != null && border.BindingContext is AlarmDetails alarmDetails)
            {
                if (alarmDetails.IsAlarmEnabled)
                {
                    this.alarmEditPicker1.SelectedTime = alarmDetails.AlarmTime;
                    this.alarmDetails = alarmDetails;
                    this.alarmEditPicker1.IsOpen = true;
                }
            }
#else
            if (sender is Border border && border.BindingContext != null && border.BindingContext is AlarmDetails alarmDetails)
            {
                if (alarmDetails.IsAlarmEnabled)
                {
                    this.alarmEditPicker.SelectedTime = alarmDetails.AlarmTime;
                    this.alarmDetails = alarmDetails;
                    this.alarmEditPicker.IsOpen = true;
                }
            }
#endif
        }

        /// <summary>
        /// Confirms the new time for the current alarm and closes the editor.
        /// </summary>
        /// <param name="sender">SfTimePicker.</param>
        /// <param name="e">Event data.</param>
        private void AlarmEditPicker_OkButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Syncfusion.Maui.Toolkit.Picker.SfTimePicker picker && this.alarmDetails != null)
            {
                if (picker.SelectedTime != null && this.alarmDetails.AlarmTime != picker.SelectedTime)
                {
                    this.alarmDetails.AlarmTime = picker.SelectedTime.Value;
                }

                this.alarmDetails = null;
            }

#if ANDROID || IOS
            this.alarmEditPicker1.IsOpen = false;
#else
            this.alarmEditPicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Cancels editing and closes the alarm editor popup.
        /// </summary>
        /// <param name="sender">Button or picker footer cancel action.</param>
        /// <param name="e">Event data.</param>
        private void alarmEditPicker_CancelButtonClicked(object sender, EventArgs e)
        {
#if ANDROID || IOS
            this.alarmEditPicker1.IsOpen = false;
#else
            this.alarmEditPicker.IsOpen = false;
#endif
        }

        /// <summary>
        /// Updates alarm text colors when the enable/disable switch is toggled.
        /// </summary>
        /// <param name="sender">SfSwitch bound to an AlarmDetails.</param>
        /// <param name="e">Toggle event data.</param>
        private void alarmSwitch_Toggled(object sender, SwitchStateChangedEventArgs e)
        {
            if (sender is SfSwitch toggleSwitch && toggleSwitch.BindingContext != null && toggleSwitch.BindingContext is AlarmDetails alarmDetails && e.NewValue.HasValue)
            {
                if (e.NewValue.Value)
                {
                    alarmDetails.AlarmTextColor = Colors.Black;
                    alarmDetails.AlarmSecondaryTextColor = Color.FromArgb("#49454F");
                }
                else
                {
                    alarmDetails.AlarmTextColor = Colors.Black.WithAlpha(0.5f);
                    alarmDetails.AlarmSecondaryTextColor = Color.FromArgb("#49454F").WithAlpha(0.5f);
                }
            }

        }

        /// <summary>
        /// Opens the add-alarm popup.
        /// </summary>
        /// <param name="sender">Tap source.</param>
        /// <param name="e">Event data.</param>
        private void OnAddAlarmTapped(object sender, EventArgs e)
        {
#if ANDROID || IOS
            this.alarmPopup1.IsOpen = true;
#else
            this.alarmPopup.IsOpen = true;
#endif
        }

        /// <summary>
        /// Adds a newly created alarm to the view model collection.
        /// </summary>
        /// <param name="sender">New AlarmDetails item.</param>
        /// <param name="e">Event data.</param>
        private void alarmPopup_OnCreated(object sender, EventArgs e)
        {
            if (this.BindingContext != null && this.BindingContext is TimePickerCustomizationViewModel bindingContext && sender is AlarmDetails details)
            {
                bindingContext.AlarmCollection.Add(details);
            }
        }
    }
}
