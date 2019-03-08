using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPuzzleSolverWPF
{
   public class AnswerVM : INotifyPropertyChanged
   {
      public AnswerVM(int number)
      {
         _number = number;
      }

      private int _number;
      public int Number
      {
         get
         {
            return _number;
         }
         set
         {
            if( _number != value )
            {
               _number = value;
               PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( Number ) ) );
            }
         }
      }

      public ObservableCollection<string> Solutions { get; set; } = new ObservableCollection<string>();

      private int _currentSolution = -1;
      public int CurrentSolution
      {
         get
         {
            return _currentSolution;
         }
         set
         {
            if( _currentSolution != value )
            {
               _currentSolution = value;
               PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( nameof( CurrentSolution ) ) );
            }
         }
      }

      public event PropertyChangedEventHandler PropertyChanged;
   }
}
