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
               PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Digits ) ) );

               _controller.SetDigits( _digits );
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
               PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Start ) ) );

               _controller.SetStart( _start );
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
               PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( End ) ) );

               _controller.SetEnd( _end );
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
         set
         {
            if ( value != _answers )
            {
               _answers = value;
               PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Answers ) ) );
            }
         }
      }

      private void Compute()
      {
         _controller.StartComputing();
      }

      private ICommand _computeCommand;
      public ICommand ComputeCommand => _computeCommand ?? ( _computeCommand = new RelayCommand( Compute ) );

      public event PropertyChangedEventHandler PropertyChanged;
   }
}
