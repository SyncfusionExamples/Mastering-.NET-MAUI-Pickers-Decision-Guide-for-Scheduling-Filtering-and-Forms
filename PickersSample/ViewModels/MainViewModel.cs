using System.Windows.Input;

namespace PickersSample.ViewModels
{
    public class MainViewModel
    {
        public ICommand NavigateDateTimePickerCommand { get; }
        public ICommand NavigatePickerCommand { get; }
        public ICommand NavigateDatePickerCommand { get; }
        public ICommand NavigateTimePickerCommand { get; }

        public MainViewModel()
        {
            NavigateDateTimePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//DateTimePickersPage"));
            NavigatePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//PickerPage"));
            NavigateDatePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//DatePickerPage"));
            NavigateTimePickerCommand = new Command(async () => await Shell.Current.GoToAsync("//TimePickerPage"));
        }
    }
}
