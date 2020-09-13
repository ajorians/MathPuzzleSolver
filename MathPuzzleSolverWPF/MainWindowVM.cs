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
      private readonly Controller _controller;
      public MainWindowVM( Controller controller)
      {
         _controller = controller;

         //_digits = GetDigitsString( _controller.GetDigits() );
         //_start = _controller.GetStart();
         //_end = _controller.GetEnd();

         _controller.AnswerItemsChanged += AnswerItemsChanged;
         _controller.StartChanged += StartChanged;
         _controller.EndChanged += EndChanged;
         _controller.DigitsChanged += DigitsChanged;
      }

      private static string GetDigitsString(int[] digits)
      {
         var result = string.Join(", ", digits);
         return result;
      }

      private void DigitsChanged(object sender, int[] digits)
      {
         Digits = GetDigitsString(digits);
      }

      private void StartChanged(object sender, int newStart)
      {
         Start = newStart;
      }

      private void EndChanged(object sender, int newEnd)
      {
         End = newEnd;
      }

      private void AnswerItemsChanged(object sender, EventArgs e)
      {
         _answers.Clear();

         //for( int i=_controller.GetStart(); i<= _controller.GetEnd(); i++)
         foreach( var answer in _controller.Answers )
         {
            _answers.Add(new AnswerVM(answer));
         }

         //List<AnswerVM> answersToRemove = new List<AnswerVM>();
         //foreach( var answer in _answers )
         //{
         //   if( answer.Number < _controller.GetStart() || answer.Number > _controller.GetEnd() )
         //   {
         //      answersToRemove.Add(answer);
         //   }
         //}
         //answersToRemove.ForEach(ans => _answers.Remove(ans));

         //foreach (var answer in _answers)
         //{

         //}
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
