using System.Windows;

namespace RoutingAlgorithmProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyMapView_MapViewTapped(object sender, Esri.ArcGISRuntime.Controls.MapViewInputEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if(viewModel != null)
            {
                if(viewModel.IsMovingStartPoint)
                {
                    viewModel.StartLocation = e.Location;
                }
                else
                {
                    viewModel.EndLocation = e.Location;
                }
            }
        }
    }
}
