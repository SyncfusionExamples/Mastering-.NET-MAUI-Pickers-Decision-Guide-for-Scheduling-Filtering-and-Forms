using PickerDecisionGuideSample.ViewModels;

namespace PickerDecisionGuideSample
{
    /// <summary>
    /// Landing page that links to all picker samples.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Initializes components and sets the main ViewModel.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}
