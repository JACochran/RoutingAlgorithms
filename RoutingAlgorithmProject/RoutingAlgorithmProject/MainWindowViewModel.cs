using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using RoutingAlgorithmProject.Utility;
using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;
using RoutingAlgorithmProject.Routing;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System.Windows.Media;
using System.Windows;

namespace RoutingAlgorithmProject
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            DCGraph = OsmUtility.ReadOsmData(@"..\..\Resources\district-of-columbia-latest.osm.pbf");
            var runTest = true;
            if (runTest)
            {
                ShortestPathTester.TestPathFinders(DCGraph);
                //var VAGraph = OsmUtility.ReadOsmData(@"..\..\Resources\Virginia.osm.pbf");
                //ShortestPathTester.TestPathFinders(VAGraph);
            }
            IsMovingStartPoint = true;
            FindRouteAStarCommand = new RelayCommand<MapView>(AStarCommandExecuted, CanRouteExecute);
            FindRouteDijikstraCommand = new RelayCommand<MapView>(DijikstraCommandExecuted, CanRouteExecute);
        }
        public RoutingGraph DCGraph;
        public static Envelope Databounds = new Envelope(-77.1201, 38.7913, -76.9091, 38.996);

        private bool CanRouteExecute(MapView mapView)
        {
            return StartLocation != null && EndLocation != null;
        }



        private void DijikstraCommandExecuted(MapView mapView)
        {
            var dpf = new DijkstraApproximateBucketPathFinder(DCGraph);
            //var path = dpf.FindShortestPath(StartLocation.ToCoordinates(), EndLocation.ToCoordinates());

            Coordinates a = new Coordinates(38.93764f, -76.99521f);
            Coordinates b = new Coordinates(38.89585f, -76.97469f);

            float pathLength = 0;
            var path = dpf.FindShortestPath(b, a, ref pathLength);
            if (path != null && path.Count > 0)
            {
                DisplayPath(path, mapView);
            }
            else
            {
                MessageBox.Show("No Route Found!");
            }
        }

        private void AStarCommandExecuted(MapView mapView)
        {
            Coordinates a = new Coordinates(38.89394f, -76.97941f);
            Coordinates b = new Coordinates(38.89585f, -76.97469f);

            var dpf = new AStarApproximateBucketPathFinder(DCGraph);
            // var path = dpf.FindShortestPath(StartLocation.ToCoordinates(), EndLocation.ToCoordinates());
            float pathlength = 0;
            var path = dpf.FindShortestPath(a, b, ref pathlength);
            if (path != null && path.Count > 0)
            {
                DisplayPath(path, mapView);
            }
            else
            {
                MessageBox.Show("No Route Found!");
            }
        }

        private void DisplayPath(List<Vertex> path, MapView mapView)
        {

            var location = path.ToPolyline(StartLocation, EndLocation);
            //get the graphics layer
            var graphicsLayer = mapView.Map.Layers["MyGraphics"] as GraphicsLayer;
            if (graphicsLayer != null)
            {
                //update current graphic otherwise create a new one
                var routeGraphic = CurrentRoute ?? new Graphic()
                {
                    Symbol = new SimpleLineSymbol()
                    {
                        Color = Colors.Blue,
                        Width = 2.0,
                        Style = SimpleLineStyle.Dash
                    }
                };
                routeGraphic.Geometry = location;
                //remove the old route
                if (graphicsLayer.Graphics.Contains(routeGraphic))
                {
                    graphicsLayer.Graphics.Remove(routeGraphic);
                }
                //add the new route graphic
                graphicsLayer.Graphics.Add(routeGraphic);
                CurrentRoute = routeGraphic;
            }
            //zoom to route location
            mapView.SetViewAsync(location, new System.TimeSpan(0, 0, 1), new System.Windows.Thickness(100));
        }

        private Graphic _currentRoute;

        public Graphic CurrentRoute
        {
            get
            {
                return _currentRoute;
            }
            set
            {
                _currentRoute = value;
                RaisePropertyChanged(() => CurrentRoute);
            }
        }

        public string MovingPointText
        {
            get
            {
                return IsMovingStartPoint ? "Move Start Point" : "Move End Point";
            }
        }

        private bool _isMovingStartPoint;
        public bool IsMovingStartPoint
        {
            get { return _isMovingStartPoint; }
            set
            {
                _isMovingStartPoint = value;
                RaisePropertyChanged(() => IsMovingStartPoint);
                RaisePropertyChanged(() => MovingPointText);
            }
        }

        private MapPoint _startLocation;
        private MapPoint _endLoaction;

        public MapPoint StartLocation
        {
            get
            {
                return _startLocation;
            }
            set
            {
                _startLocation = value;
                RaisePropertyChanged(() => StartLocation);
                IsMovingStartPoint = !IsMovingStartPoint;
                FindRouteAStarCommand.RaiseCanExecuteChanged();
                FindRouteDijikstraCommand.RaiseCanExecuteChanged();
            }
        }

        public MapPoint EndLocation
        {
            get
            {
                return _endLoaction;
            }
            set
            {
                _endLoaction = value;
                RaisePropertyChanged(() => EndLocation);
                FindRouteAStarCommand.RaiseCanExecuteChanged();
                FindRouteDijikstraCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand<MapView> FindRouteAStarCommand { get; private set; }
        public RelayCommand<MapView> FindRouteDijikstraCommand { get; private set; }
    }
}
