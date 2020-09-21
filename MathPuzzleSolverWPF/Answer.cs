using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MathPuzzleSolverWPF
{
   public class Answer
   {
      private List<string> _equations = new List<string>();
      public ReadOnlyCollection<string> Equations
      {
         get => _equations.AsReadOnly();
      }
      public int Number { get; private set; }
      public event EventHandler<string> EquationAdded;

      public Answer( int number )
      {
         Number = number;
      }

      public void AddEquation( string equation )
      {
         if ( _equations.Contains( equation ) )
            return;

         _equations.Add(equation);
         EquationAdded?.Invoke(this, equation);
      }
   }
}
