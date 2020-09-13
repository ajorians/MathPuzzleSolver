using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MathPuzzleSolverWPF
{
   public class AnswerVM : INotifyPropertyChanged
   {
      private readonly Answer _answer;
      public AnswerVM(Answer answer)
      {
         _answer = answer;

         _answer.EquationAdded += EquationAdded;
      }

      private void EquationAdded(object sender, string solutionEquation)
      {
         Solutions.Add(solutionEquation);
         if (CurrentSolution == -1)
         {
            CurrentSolution = 0;
         }
      }

      public int Number
      {
         get
         {
            return _answer.Number;
         }
      }

      public ObservableCollection<string> Solutions { get; } = new ObservableCollection<string>();

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
