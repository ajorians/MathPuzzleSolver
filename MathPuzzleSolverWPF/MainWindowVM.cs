using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MathPuzzleSolverWPF
{
   public class MainWindowVM : INotifyPropertyChanged
   {
      private Controller _controller;
      public MainWindowVM( Controller controller)
      {
         _controller = controller;

         _digits = _controller.GetDigitsString();
         _start = _controller.GetStart();
         _end = _controller.GetEnd();
      }

      private void OnPropertyChanged( string propertyName )
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private string _digits = "1,2,3,4";
      public string Digits
      {
         get
         {
            return _digits;
         }
         set
         {
            if( _digits != value )
            {
               _digits = value;
               OnPropertyChanged( nameof( Digits ) );

               _controller.SetDigits( _digits );
               OnPropertyChanged(nameof(ComputationInProgress));
            }
         }
      }

      private int _start = 0;
      public int Start
      {
         get
         {
            return _start;
         }
         set
         {
            if( value != _start )
            {
               _start = value;
               OnPropertyChanged( nameof( Start ) );

               _controller.SetStart( _start );
               OnPropertyChanged(nameof(ComputationInProgress));
            }
         }
      }
      private int _end = 0;
      public int End
      {
         get
         {
            return _end;
         }
         set
         {
            if ( value != _end )
            {
               _end = value;
               OnPropertyChanged( nameof( End ) );

               _controller.SetEnd( _end );
               OnPropertyChanged(nameof(ComputationInProgress));
            }
         }
      }

      private ObservableCollection<AnswerVM> _answers = new ObservableCollection<AnswerVM>();
      public ObservableCollection<AnswerVM> Answers
      {
         get
         {
            return _answers;
         }
      }

      private void Compute()
      {
         _controller.StartComputing();
         OnPropertyChanged(nameof(ComputationInProgress));
      }

      private ICommand _computeCommand;
      public ICommand ComputeCommand => _computeCommand ?? ( _computeCommand = new RelayCommand( Compute ) );

      private void StopComputing()
      {
         _controller.StopComputing();
         OnPropertyChanged(nameof(ComputationInProgress));
      }

      private ICommand _stopComputingCommand;
      public ICommand StopComputingCommand => _stopComputingCommand ?? (_stopComputingCommand = new RelayCommand(StopComputing));

      public bool ComputationInProgress => _controller.IsComputationInProgress();

      public event PropertyChangedEventHandler PropertyChanged;
   }
}
