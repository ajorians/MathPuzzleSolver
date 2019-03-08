using System;
using System.Collections.Generic;
using System.Linq;

namespace MathPuzzleSolver
{
   public class EquationCreator
   {
      public int[] Digits { get; private set; }
      public List<MathOperation> Operations { get; set; } = new List<MathOperation>()
      {
         MathOperation.Addition,
         MathOperation.Subtraction,
         MathOperation.Multiplication,
         MathOperation.Division,
         MathOperation.Exponent,
         MathOperation.Factorial,
         MathOperation.SquareRoot
      };

      public EquationCreator( int[] digits )
      {
         Digits = digits;
      }

      public IEnumerable<string> GetEquations()
      {
         int[] currentCombination = { };
         foreach ( var digitCombination in GetDigitCombinations( Digits, currentCombination, new List<int>() ) )
         {
            //Let's get all grouping combinations
            foreach ( var grouping in GetGroupingCombinations( digitCombination.Length ) )
            {
               var items = GetDigitsInGroups( digitCombination, grouping );

               //var temp = ConstructEquations( Operations, items ).ToList();

               foreach ( var equation in ConstructEquations( Operations, items ) )
               {
                  yield return equation;
               }
            }
         }
      }

      private static IEnumerable<string> ConstructEquations( List<MathOperation> operations, IEnumerable<string> numberGroups )
      {
         foreach ( var equation in GetEquation( numberGroups.First(), numberGroups.Skip( 1 ) ) )
         {
            yield return equation;
         }
      }

      private static IEnumerable<string> GetEquation( string firstPart, IEnumerable<string> additionalParts )
      {
         int numAdditionalParts = additionalParts.Count();

         if ( additionalParts.Any() )
         {
            foreach ( var equation in GetEquation( additionalParts.First(), additionalParts.Skip( 1 ) ) )
            {
               yield return $"{firstPart} + {equation}";
               yield return $"{firstPart} - {equation}";
               yield return $"{firstPart} * {equation}";
               yield return $"{firstPart} / {equation}";

               yield return $"Power({firstPart},{equation})";
            }

            for ( int i = 1; i < numAdditionalParts; i++ )
            {
               if ( numAdditionalParts > i )
               {
                  foreach ( var equation in GetEquation( firstPart, additionalParts.Take( i ) ) )
                  {
                     foreach ( var restOfEquation in GetEquation( equation, additionalParts.Skip( i ) ) )
                     {
                        yield return $"{restOfEquation}";
                     }
                  }
               }
            }

         }
         else
         {
            yield return firstPart;
            yield return $"Sqrt({firstPart})";
            yield return $"Factorial({firstPart})";
         }
      }

      private List<string> GetDigitsInGroups( int[] digitCombination, List<int> grouping )
      {
         var ret = new List<string>();
         int currentIndex = 0;
         foreach ( var group in grouping )
         {
            string strGroup = string.Empty;
            for ( int i = 0; i < group; i++ )
            {
               strGroup += digitCombination[currentIndex++].ToString();
            }
            ret.Add( strGroup );
         }

         return ret;
      }

      public static IEnumerable<List<int>> GetGroupingCombinations( int length )
      {
         yield return new List<int>() { length };

         for ( int i = 1; i < length; i++ )
         {
            foreach ( var subGrouping in GetGroupingCombinations( length - i ) )
            {
               yield return Combine( i, subGrouping );
            }
         }
      }

      private static List<int> Combine( int firstValue, List<int> rest )
      {
         rest.Insert( 0, firstValue );
         return rest;
      }
      private static List<int> Combine( List<int> firstValues, int rest )
      {
         firstValues.Add( rest );
         return firstValues;
      }

      private IEnumerable<int[]> GetDigitCombinations( int[] digits, int[] currentCombination, List<int> list )
      {
         if ( currentCombination.Length == digits.Length )
         {
            yield return currentCombination;
         }

         for ( int i = 0; i < digits.Length; i++ )
         {
            if ( list.Contains( i ) )
               continue;

            int[] combo = currentCombination.Concat( new int[] { digits[i] } ).ToArray();

            foreach ( var item in GetDigitCombinations( digits, combo, new List<int>( list ) { i } ) )
            {
               yield return item;
            }
         }

         yield break;
      }
   }
}

