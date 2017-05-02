using Esri.ArcGISRuntime.Data;
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
            //MyMapView.LayerLoaded += MyMapView_Initialized;
        }

        private void MyMapView_Initialized(object sender, System.EventArgs e)
        {
            //var points = new FeatureLayer(ShapefileTable.OpenAsync(@"..\..\Resources\WashingtonDCNodesLayer.shp").Result);

            //points.Opacity = 0.5;
            //var roads = new FeatureLayer(ShapefileTable.OpenAsync(@"..\..\Resources\WashingtonDCRoadsLayer.shp").Result);
            //roads.Opacity = 0.5;
            //MyMapView.LayerLoaded -= MyMapView_Initialized;
            
            //MyMapView.Map.Layers.Add(points);
            //MyMapView.Map.Layers.Add(roads);
            //MyMapView.Map.Layers.Move(2,4);
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
                    StartPointGraphic = UpdatePoint(e.Location, StartPointGraphic, true);
                    viewModel.StartLocation = e.Location;
                }
                else
                {
                    EndPointGraphic = UpdatePoint(e.Location, EndPointGraphic);
                    viewModel.EndLocation = e.Location;
                }
            }
        }


        private Graphic UpdatePoint(MapPoint newLocation, Graphic graphic, bool isMovingStartPoint = false)
        {
            var myGraphicLayer = MyMapView.Map.Layers["MyGraphics"] as GraphicsLayer;
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
                                                                        Style = SimpleMarkerStyle.X,
                                                                        Size = 16
                                                                    }
                                };
        }
    }
}
