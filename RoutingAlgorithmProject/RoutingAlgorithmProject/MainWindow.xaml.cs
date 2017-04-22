using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System.Windows;
using System.Windows.Media;

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
        
        public Graphic StartPointGraphic { get; set; }

        public Graphic EndPointGraphic { get; set; }

        private void MyMapView_MapViewTapped(object sender, Esri.ArcGISRuntime.Controls.MapViewInputEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            if(viewModel != null)
            {
                if(viewModel.IsMovingStartPoint)
                {
                    StartPointGraphic = UpdatePoint(e.Location, viewModel.StartLocation, StartPointGraphic, true);
                }
                else
                {
                    EndPointGraphic = UpdatePoint(e.Location, viewModel.EndLocation, EndPointGraphic);
                }
            }
        }


        private Graphic UpdatePoint(MapPoint newLocation, MapPoint oldLocation, Graphic graphic, bool isMovingStartPoint = false)
        {
            var myGraphicLayer = MyMapView.Map.Layers["MyGraphics"] as GraphicsLayer;
            oldLocation = newLocation;
            if(graphic == null)
            {
                graphic = CreateGraphic(isMovingStartPoint);
                graphic.Geometry = newLocation;
                myGraphicLayer.Graphics.Add(graphic);
                return graphic;
            }
            else
            {
                myGraphicLayer.Graphics.Remove(graphic);
                graphic.Geometry = newLocation;
                myGraphicLayer.Graphics.Add(graphic);
                return graphic;
            }
            
        }

        private Graphic CreateGraphic(bool useStartPointGraphic)
        {
            return new Graphic(){
                                   Symbol = new SimpleMarkerSymbol(){
                                                                        Color =  useStartPointGraphic ? Colors.Green : Colors.Red,
                                                                        Style = SimpleMarkerStyle.Circle,
                                                                        Size = 16
                                                                    }
                                };
        }
    }
}
