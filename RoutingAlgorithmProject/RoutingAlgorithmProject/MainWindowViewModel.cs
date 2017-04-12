using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;

namespace RoutingAlgorithmProject
{
     public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            FindRouteAStarCommand = new RelayCommand(AStarCommandExecuted);
            FindRouteDijikstraCommand = new RelayCommand(DijikstraCommandExecuted);
        }

        private void DijikstraCommandExecuted()
        {
            throw new NotImplementedException();
        }

        private void AStarCommandExecuted()
        {
            throw new NotImplementedException();
        }

        public RelayCommand FindRouteAStarCommand { get; private set; }
        public RelayCommand FindRouteDijikstraCommand { get; private set; }
    }
}
