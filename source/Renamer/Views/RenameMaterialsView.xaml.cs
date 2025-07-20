using System.Windows;

namespace Renamer.Views
{
    /// <summary>
    /// Interaction logic for RenameMaterialsView.xaml
    /// </summary>
    public partial class RenameMaterialsView : Window
    {
        private readonly ViewModels.RenameMaterialsViewModel _viewModel;

        public RenameMaterialsView()
        {
            InitializeComponent();

            //var viewModel = new MainViewModel();
            //this.DataContext = viewModel;

            //enable this line of code if XAML Behaviors is used
            //var _ = new Microsoft.Xaml.Behaviors.DefaultTriggerAttribute(typeof(Trigger), typeof(Microsoft.Xaml.Behaviors.TriggerBase), null);

            _viewModel = (ViewModels.RenameMaterialsViewModel)this.DataContext;
            _viewModel.ClosingRequest += (sender, e) => this.Close();
        }
    }
}
