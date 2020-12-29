using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPuzzleSolver
{
   static class Program
   {
      static bool SolvedAllValues<T>( this SortedDictionary<int, T> mapResultToEquation, int startValue, int endValue )
      {
         for ( int i = startValue; i < endValue; i++ )
         {
            if ( !mapResultToEquation.ContainsKey( i ) )
               return false;
         }
         return true;
      }
      static void Main( string[] args )
      {
         var mapResultToEquation = new SortedDictionary<int, List<string>>();

         var puzzleSolver = new PuzzleSolver( new int[] { 1, 2, 3, 4 } );

         puzzleSolver.CompletedValue += delegate(object? sender, CompletedValueArgs e)
         {
            if ( !mapResultToEquation.ContainsKey( e.Value ) )
            {
               mapResultToEquation.Add( e.Value, new List<string>()
               {
                  e.Equation
               } );

               if ( mapResultToEquation.SolvedAllValues( puzzleSolver.StartValue, puzzleSolver.EndValue ) )
               {
                  Console.WriteLine( "Completed! :)" );
                  puzzleSolver.Cancel();
               }
            }
            else
            {
               mapResultToEquation[e.Value].Add( e.Equation );
            }
         };
         puzzleSolver.Solve();

         int j = 0;
         j++;
      }
   }
}
