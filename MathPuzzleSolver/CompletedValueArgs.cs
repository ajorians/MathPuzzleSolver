namespace MathPuzzleSolver
{
   public class CompletedValueArgs
   {
      public string Equation { get; }
      public int Value { get; }
      public CompletedValueArgs(string equation, int value)
      {
         Equation = equation;
         Value = value;
      }
   }
}