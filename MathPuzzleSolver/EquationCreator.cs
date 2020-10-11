using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MathPuzzleSolver
{
   public class EquationCreator
   {
      public event EventHandler<int> CurrentPass;
      public event EventHandler<List<string>> CurrentGroupCombination;

      public int[] Digits { get; private set; }
      public MathOperation Operations { get; set; } =
         MathOperation.Addition |
         MathOperation.Subtraction |
         MathOperation.Multiplication |
         MathOperation.Division |
         MathOperation.Exponent |
         MathOperation.Factorial |
         MathOperation.SquareRoot;

      public EquationCreator( int[] digits )
      {
         Digits = digits;
      }

      public IEnumerable<string> GetEquations()
      {
         int totalEquationsRan = 0;
         for (int pass = 0; pass < 3; pass++)
         {
            CurrentPass?.Invoke(this, pass);
            foreach (var digitCombination in GetDigitCombinations(Digits))
            {
               //for (MathOperation mathOperation = MathOperation.Addition; mathOperation < MathOperation.All; mathOperation++)
               {
                  //Let's get all grouping combinations
                  foreach (var grouping in GetGroupingCombinations(digitCombination.Length))
                  {
                     int totalEquationsForGrouping = 0;
                     var items = GetDigitsInGroups(digitCombination, grouping);

                     if (!ProceedWithGroupings(items))
                        continue;

                     CurrentGroupCombination?.Invoke(this, items);

                     //var temp = ConstructEquations( Operations, items, pass ).ToList();

                     foreach (var equation in ConstructEquations(Operations, items, pass))
                     {
                        totalEquationsForGrouping++;
                        totalEquationsRan++;
                        yield return equation;
                     }
                  }
               }
            }
         }
      }

      private bool ProceedWithGroupings(List<string> items)
      {
         return !items.Any(str => str.StartsWith("0") && str.Length > 1);
      }

      private static IEnumerable<string> ConstructEquations( MathOperation operations, IEnumerable<string> numberGroups, int pass )
      {
         foreach ( var equation in GetEquation(operations, numberGroups, pass ) )
         {
            yield return equation;
         }
      }

      private static string SmartParens( string equation )
      {
         if (equation.All(ch => char.IsDigit(ch)))
            return equation;

         if (equation.StartsWith("Power(") && equation.EndsWith(")"))
            return equation;

         if (equation.StartsWith("Sqrt(") && equation.EndsWith(")"))
            return equation;

         if (equation.StartsWith("Factorial(") && equation.EndsWith(")"))
            return equation;

         return $"({equation})";
      }

      private static IEnumerable<string> GetEquation( MathOperation mathOperations, IEnumerable<string> parts, int pass )
      {
         static string PowerEquation( string @base, string exponent ) => $"Power({@base},{exponent})";
         static string SqrtEquation( string input ) => $"Sqrt({input})";
         static string FactorialEquation( string input ) => $"Factorial({input})";

         static IEnumerable<string> CombinationSqrtAndFactorial(MathOperation mathOperations, string input, int pass)
         {
            List<string> sqrtAndFactorialCombination = new List<string>();

            if (mathOperations.HasFlag(MathOperation.SquareRoot))
            {
               string currentSqrt = input;
               for (int i = 0; i < pass; i++)
               {
                  currentSqrt = SqrtEquation(currentSqrt);

                  if (i != pass - 1)
                     continue;

                  sqrtAndFactorialCombination.Add(currentSqrt);
               }
            }

            if (mathOperations.HasFlag(MathOperation.Factorial))
            {
               string currentFactorial = input;
               for (int i = 0; i < pass; i++)
               {
                  currentFactorial = FactorialEquation(currentFactorial);

                  if (i != pass - 1)
                     continue;

                  sqrtAndFactorialCombination.Add(currentFactorial);
               }
            }

            foreach( var combination in sqrtAndFactorialCombination)
            {
               yield return combination;
            }
         }

         int numParts = parts.Count();
         for (int takeParts = 1; takeParts <= numParts; takeParts++)
         {
            IEnumerable<string> takenParts = parts.Take(takeParts);
            IEnumerable<string> takenAdditionalParts = parts.Skip(takeParts);

            if (takenParts.Any())
            {
               if (takenParts.Count() == 1 && !takenAdditionalParts.Any())
               {
                  yield return takenParts.Single();

                  foreach (var combo in CombinationSqrtAndFactorial(mathOperations, takenParts.Single(), pass) )
                     yield return combo;
               }

               if (takenAdditionalParts.Any())
               {
                  foreach (var leftEquation in GetEquation(mathOperations, takenParts, pass))
                  {
                     foreach (var rightEquation in GetEquation(mathOperations, takenAdditionalParts, pass))
                     {
                        if( mathOperations.HasFlag( MathOperation.Addition ))
                           yield return $"{SmartParens(leftEquation)} + {SmartParens(rightEquation)}";
                        if (mathOperations.HasFlag(MathOperation.Subtraction))
                           yield return $"{SmartParens(leftEquation)} - {SmartParens(rightEquation)}";
                        if (mathOperations.HasFlag(MathOperation.Multiplication))
                           yield return $"{SmartParens(leftEquation)} * {SmartParens(rightEquation)}";
                        if (mathOperations.HasFlag(MathOperation.Division))
                           yield return $"{SmartParens(leftEquation)} / {SmartParens(rightEquation)}";

                        if (mathOperations.HasFlag(MathOperation.Exponent))
                           yield return PowerEquation($"{SmartParens(leftEquation)}", $"{SmartParens(rightEquation)}");

                        //foreach (var combo in CombinationSqrtAndFactorial(mathOperations, $"{SmartParens(leftEquation)} + {SmartParens(rightEquation)}", pass))
                        //   yield return combo;
                        //foreach (var combo in CombinationSqrtAndFactorial(mathOperations, $"{SmartParens(leftEquation)} - {SmartParens(rightEquation)}", pass))
                        //   yield return combo;
                        //foreach (var combo in CombinationSqrtAndFactorial(mathOperations, $"{SmartParens(leftEquation)} * {SmartParens(rightEquation)}", pass))
                        //   yield return combo;
                        //foreach (var combo in CombinationSqrtAndFactorial(mathOperations, $"{SmartParens(leftEquation)} / {SmartParens(rightEquation)}", pass))
                        //   yield return combo;
                        //foreach (var combo in CombinationSqrtAndFactorial(mathOperations, PowerEquation($"{SmartParens(leftEquation)}", $"{SmartParens(rightEquation)}"), pass))
                        //   yield return combo;
                     }
                  }
               }
            }
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

      // Generating permutation using Heap Algorithm
      private static IEnumerable<int[]> heapPermutation(int[] a, int size, int n)
      {
         // if size becomes 1 then prints the obtained
         // permutation
         if (size == 1)
            yield return a;

         for (int i = 0; i < size; i++)
         {
            foreach (var permutation in heapPermutation(a, size - 1, n))
               yield return permutation;

            // if size is odd, swap 0th i.e (first) and
            // (size-1)th i.e (last) element
            if (size % 2 == 1)
            {
               int temp = a[0];
               a[0] = a[size - 1];
               a[size - 1] = temp;
            }

            // If size is even, swap ith and
            // (size-1)th i.e (last) element
            else
            {
               int temp = a[i];
               a[i] = a[size - 1];
               a[size - 1] = temp;
            }
         }
      }

      class DigitEqualityComparer : IEqualityComparer<int[]>
      {
         public bool Equals(int[] x, int[] y)
         {
            Debug.Assert(x.GetLength(0) == y.GetLength(0));

            return GetHashCode(x) == GetHashCode(y);
         }

         public int GetHashCode(int[] arr)
         {
            int result = 0;
            int length = arr.GetLength(0);
            for (int digit = length - 1; digit >= 0; digit--)
            {
               int significance = (int)Math.Pow(10, length - (digit + 1));
               result += arr[digit] * significance;
            }
            return result;
         }
      }

      private IEnumerable<int[]> GetDigitCombinations( int[] digits )
      {
         var comparer = new DigitEqualityComparer();

         foreach (var uniquePermutation in heapPermutation(digits, digits.Length, digits.Length).Distinct(comparer) )
         {
            yield return uniquePermutation;
         }
      }
   }
}

