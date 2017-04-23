using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using RoutingAlgorithmProject.Utility;
using Esri.ArcGISRuntime.Geometry;
using RoutingAlgorithmProject.Graph;
using RoutingAlgorithmProject.Routing.Models;

namespace RoutingAlgorithmProject
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            IsMovingStartPoint = true;
            FindRouteAStarCommand = new RelayCommand(AStarCommandExecuted, CanRouteExecute);
            FindRouteDijikstraCommand = new RelayCommand(DijikstraCommandExecuted, CanRouteExecute);
            OsmUtility.TestGraph();
            var topLeftCorner = new MapPoint(Databounds.XMin, Databounds.Extent.YMax, SpatialReferences.Wgs84);
            var bottomRightCorner = new MapPoint(Databounds.XMax, Databounds.YMin, SpatialReferences.Wgs84);

            WholeGraph = OsmUtility.ReadOsmData(topLeftCorner.ToCoordinates(), bottomRightCorner.ToCoordinates(), new AStarGraph());
        }
        public RoutingGraph<AStarVertex> WholeGraph;
        public static Envelope Databounds = new Envelope(-77.1201, 38.7913, -76.9091, 38.996);

        private bool CanRouteExecute()
        {
            return StartLocation != null && EndLocation != null;
        }

        private Graph.RoutingGraph<T> CreateGraph<T>(RoutingGraph<T> initialGraph) where T : Vertex
        {
            //var start = new Coordinates(38.903671f, -77.000038f);
            //var end = new Coordinates(38.902446f, -76.997449f);
            // Create graph using start and end locations to build bounding box
            return OsmUtility.ReadOsmData(StartLocation.ToCoordinates(), EndLocation.ToCoordinates(), initialGraph);
        }

        private void DijikstraCommandExecuted()
        {
            RoutingGraph<Vertex> graph = new Graph.Graph();

            var dpf = new PathFinder.DijkstraPathFinder(CreateGraph(graph));
            var path = dpf.FindShortestPath(StartLocation.ToCoordinates(), EndLocation.ToCoordinates());
            //TODO display path to user
        }

        private void AStarCommandExecuted()
        {
            var dpf = new PathFinder.AStarPathFinder(WholeGraph);
            var path = dpf.FindShortestPath(StartLocation.ToCoordinates(), EndLocation.ToCoordinates());
            //TODO display path to user
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


        public RelayCommand FindRouteAStarCommand { get; private set; }
        public RelayCommand FindRouteDijikstraCommand { get; private set; }
    }
}
