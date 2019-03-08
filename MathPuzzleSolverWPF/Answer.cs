using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPuzzleSolverWPF
{
   public class Answer
   {
      private AnswerVM _answerVM;
      public List<string> Equations = new List<string>();
      public int Number { get; private set; }

      public Answer( int number )
      {
         Number = number;

         _answerVM = new AnswerVM( Number )
         {
            CurrentSolution = -1,
            Solutions = new ObservableCollection<string>()
         };
      }
      public AnswerVM GetVM()
      {
         return _answerVM;
      }

      public void UpdateVM()
      {
         var solutions = new ObservableCollection<string>();

         foreach( var equation in Equations)
         {
            if ( !_answerVM.Solutions.Contains( equation ) )
            {
               _answerVM.Solutions.Add( equation );
            }
         }
         if ( _answerVM.CurrentSolution == -1 )
         {
            _answerVM.CurrentSolution = 0;
         }
      }
   }
}
