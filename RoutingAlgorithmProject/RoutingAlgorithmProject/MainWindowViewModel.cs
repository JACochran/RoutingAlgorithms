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
        }

        private bool CanRouteExecute()
        {
            return StartLocation != null && EndLocation != null;
        }

        private void DijikstraCommandExecuted()
        {
            throw new NotImplementedException();
        }

        private void AStarCommandExecuted()
        {
            OsmUtility.ReadOsmData();
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
