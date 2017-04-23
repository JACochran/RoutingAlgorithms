using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using RoutingAlgorithmProject.Utility;
using Esri.ArcGISRuntime.Geometry;

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
            
        }

        private bool CanRouteExecute()
        {
            return StartLocation != null && EndLocation != null;
        }

        private Graph.Graph CreateGraph()
        {
            var start = new MapPoint(38.903671f, -77.000038f, SpatialReferences.Wgs84);
            var end = new MapPoint(38.902446f, -76.997449f, SpatialReferences.Wgs84);
            // Create graph using start and end locations to build bounding box
            return OsmUtility.ReadOsmData(start, end);
        }

        private void DijikstraCommandExecuted()
        {

            PathFinder.PathFinder dpf = new PathFinder.DijkstraPathFinder(CreateGraph());
            var path = dpf.FindShortestPath(StartLocation, EndLocation);
            //TODO display path to user
        }

        private void AStarCommandExecuted()
        {

            PathFinder.PathFinder dpf = new PathFinder.AStarPathFinder(CreateGraph());
            var path = dpf.FindShortestPath(StartLocation, EndLocation);
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
