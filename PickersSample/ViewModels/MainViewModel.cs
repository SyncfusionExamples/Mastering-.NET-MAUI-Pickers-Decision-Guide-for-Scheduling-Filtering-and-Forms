using System.Windows.Input;

namespace PickersSample.ViewModels
{
    /// <summary>
    /// Main view model exposing navigation commands for sample pages.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Navigates to the DateTime picker sample.
        /// </summary>
        public ICommand NavigateDateTimePickerCommand { get; }
        /// <summary>
        /// Navigates to the multi-column picker sample.
        /// </summary>
        public ICommand NavigatePickerCommand { get; }
        /// <summary>
        /// Navigates to the date picker sample.
        /// </summary>
        public ICommand NavigateDatePickerCommand { get; }
        /// <summary>
        /// Navigates to the time picker sample.
        /// </summary>
        public ICommand NavigateTimePickerCommand { get; }

        /// <summary>
        /// Initializes commands bound to the MainPage buttons.
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
