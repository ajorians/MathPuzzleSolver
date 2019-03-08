using System;
using System.Data;
using NCalc;

namespace MathPuzzleSolver
{
   public class EquationEvaluator
   {
      public EquationEvaluator()
      {
      }

      public int? Evaluate( string equation )
      {
         Expression e = new Expression( equation );
         e.EvaluateFunction += delegate ( string name, FunctionArgs args )
         {
            if ( name == "Factorial" )
            {
               var innerResult = args.Parameters[0].Evaluate();
               if ( innerResult is int )
               {
                  int n = (int)innerResult;
                  switch ( n )
                  {
                     case 0:
                        args.Result = 1;
                        return;
                     case 1:
                        args.Result = 1;
                        return;
                     case 2:
                        args.Result = 2;
                        return;
                     case 3:
                        args.Result = 6;
                        return;
                     case 4:
                        args.Result = 24;
                        return;
                     case 5:
                        args.Result = 120;
                        return;
                     case 6:
                        args.Result = 720;
                        return;
                     case 7:
                        args.Result = 5_040;
                        return;
                     case 8:
                        args.Result = 40_320;
                        return;
                     case 9:
                        args.Result = 362_880;
                        return;
                     case 10:
                        args.Result = 3_628_800;
                        return;
                  }
               }

               args.Result = double.NaN;
            }
            else if( name == "Power" )
            {
               var baseResult = args.Parameters[0].Evaluate();
               var exponentResult = args.Parameters[1].Evaluate();

               if ( baseResult is int @base && exponentResult is int exponent )
               {
                  bool acceptablePower = exponent < 10 && exponent > -10 && @base < 1000 && @base > -1000;
                  if ( !acceptablePower )
                  {
                     acceptablePower = @base < 10 && @base > 10;
                  }

                  if ( acceptablePower )
                  {
                     args.Result = new Expression( $"Pow({@base},{exponent})" ).Evaluate();
                     return;
                  }
               }
               if ( baseResult is double dBase && exponentResult is double dExponent )
               {
                  bool acceptablePower = dExponent < 10d && dExponent > -10d && dBase < 1000d && dBase > -1000d;
                  if ( !acceptablePower )
                  {
                     acceptablePower = dBase < 10d && dBase > 10d;
                  }

                  if ( acceptablePower )
                  {
                     args.Result = new Expression( $"Pow({dBase},{dExponent})" ).Evaluate();
                     return;
                  }
               }

               args.Result = double.NaN;
            }
         };

         var evaluatedResult = e.Evaluate();
         if ( evaluatedResult is int )
         {
            return (int)evaluatedResult;
         }
         if ( evaluatedResult is double )
         {
            double d = (double)evaluatedResult;
            int n = (int)d;
            if ( (double)n == d )
            {
               return n;
            }
         }
         return null;
      }
   }
}