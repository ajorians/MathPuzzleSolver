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
         var consumer = new EquationEvaluator();

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

            if ( _CancelSource.IsCancellationRequested )
            {
               break;
            }
         }
      }
   }
}