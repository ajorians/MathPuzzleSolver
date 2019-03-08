using System;
using MathPuzzleSolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MathPuzzleSolverTests
{
   [TestClass]
   public class EquationCreatorTests
   {
      [TestMethod]
      public void GetGroupingCombinations_VariousInputs_ReturnsExpected()
      {
         var one = EquationCreator.GetGroupingCombinations( 1 ).ToList();
         var two = EquationCreator.GetGroupingCombinations( 2 ).ToList();
         var three = EquationCreator.GetGroupingCombinations( 3 ).ToList();
         var four = EquationCreator.GetGroupingCombinations( 4 ).ToList();
      }
   }
}
