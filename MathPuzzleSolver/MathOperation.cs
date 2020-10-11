using System;

namespace MathPuzzleSolver
{
   [Flags]
   public enum MathOperation
   {
      Addition = 1 << 0,
      Subtraction = 1 << 1,
      Multiplication = 1 << 2,
      Division = 1 << 3,
      Exponent = 1 << 4,
      Factorial = 1 << 5,
      SquareRoot = 1 << 6,

      All = Addition | Subtraction | Multiplication | Division | Exponent | Factorial | SquareRoot
   }
}