using System.Windows.Input;

namespace PickerDecisionGuideSample.ViewModels
{
    /// <summary>
    /// Main ViewModel exposing navigation commands for sample pages.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Navigates to the DateTimePickersPage.
        /// </summary>
        public ICommand NavigateDateTimePickerCommand { get; }

        /// <summary>
        /// Navigates to the PickerPage.
        /// </summary>
        public ICommand NavigatePickerCommand { get; }

        /// <summary>
        /// Navigates to the DatePickerPage.
        /// </summary>
        public ICommand NavigateDatePickerCommand { get; }

        /// <summary>
        /// Navigates to the TimePickerPage.
        /// </summary>
        public ICommand NavigateTimePickerCommand { get; }

        /// <summary>
        /// Initializes the navigation commands with Shell navigation routes.
        /// </summary>
        public MainViewModel()
        {
            NavigateDateTimePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//DateTimePickersPage"));
            NavigatePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//PickerPage"));
            NavigateDatePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//DatePickerPage"));
            NavigateTimePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//TimePickerPage"));
        }
    }
}
