using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace MathPuzzleSolver
{
   public class PuzzleSolver
   {
      public int StartValue { get; set; } = 0;
      public int EndValue { get; set; } = 20;

      public int[] Digits { get; private set; }

      public delegate void CompletedValueHandler( object sender, CompletedValueArgs e );

      public event CompletedValueHandler CompletedValue;
      public event EventHandler FinishedComputing;
      public event EventHandler<int> EquationsComputed;
      public event EventHandler<List<string>> CurrentGroupCombination;
      public event EventHandler<int> CurrentPass;

      private CancellationTokenSource _CancelSource;

      public PuzzleSolver(int[] digits)
      {
         Digits = digits;

         _CancelSource = new CancellationTokenSource();
      }

      ~PuzzleSolver()
      {
         _CancelSource.Dispose();
         _CancelSource = null;
      }

      public void Cancel()
      {
         _CancelSource.Cancel();
      }

      public void Solve()
      {
         var producer = new EquationCreator( Digits );
         producer.CurrentPass += Producer_CurrentPass;
         producer.CurrentGroupCombination += Producer_CurrentGroupCombination;
         var consumer = new EquationEvaluator();

         int numberEquationComputed = 0;
         foreach ( var equation in producer.GetEquations() )
         {
            int? result = consumer.Evaluate( equation );

            if ( result.HasValue )
            {
               if ( result >= StartValue && result <= EndValue )
               {
                  CompletedValue?.Invoke( this, new CompletedValueArgs( equation, result.Value ) );
               }
            }
            numberEquationComputed++;
            EquationsComputed?.Invoke(this, numberEquationComputed);

            if ( _CancelSource.IsCancellationRequested )
            {
               break;
            }
         }

         FinishedComputing?.BeginInvoke(this, EventArgs.Empty, null, null);
      }

      private void Producer_CurrentPass(object sender, int e)
      {
         CurrentPass?.Invoke(this, e);
      }

      private void Producer_CurrentGroupCombination(object sender, List<string> e)
      {
         CurrentGroupCombination?.Invoke(this, e);
      }
   }
}