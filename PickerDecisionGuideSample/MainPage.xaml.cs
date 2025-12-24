using PickerDecisionGuideSample.ViewModels;

namespace PickerDecisionGuideSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}
