using System.Windows;

namespace Renamer.Views;
/// <summary>
/// Interaction logic for RenameTextStyleView.xaml
/// </summary>
public partial class RenameAnnotationStyleView : Window
{
    private readonly ViewModels.RenameAnnotationStyleViewModel _viewModel;

    public RenameAnnotationStyleView()
    {
        InitializeComponent();

        //var viewModel = new MainViewModel();
        //this.DataContext = viewModel;

        //enable this line of code if XAML Behaviors is used
        //var _ = new Microsoft.Xaml.Behaviors.DefaultTriggerAttribute(typeof(Trigger), typeof(Microsoft.Xaml.Behaviors.TriggerBase), null);

        _viewModel = (ViewModels.RenameAnnotationStyleViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }
}
