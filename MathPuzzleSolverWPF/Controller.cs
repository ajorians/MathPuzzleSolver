using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MathPuzzleSolver;

namespace MathPuzzleSolverWPF
{
   public class Controller
   {
      private int[] _digits = new int[]{ 1, 2, 3, 4};
      private int _start = 0;
      private int _end = 20;

      private MainWindowVM _vm;
      private PuzzleSolver _puzzleSolver;
      private List<Answer> _answers;
      private Thread _computeThread;
      public MainWindowVM VM
      {
         get
         {
            return _vm;
         }
         set
         {
            _vm = value;
            RepopulateAnswers();
         }
      }

      public Controller()
      {
      }

      ~Controller()
      {
         CancelAnyComputations();
      }

      public string GetDigitsString()
      {
         var digits = GetDigits();
         var result = string.Join( ", ", digits );
         return result;
      }
      public int[] GetDigits() => _digits;
      public int GetStart() => _start;
      public int GetEnd() => _end;
      public void SetDigits( string digits )
      {
         char[] chDigits = digits.Where( ch => char.IsDigit( ch ) ).ToArray();
         int[] arrDigits = chDigits.Select( ch => (int)ch - '0' ).ToArray();
         SetDigits( arrDigits );
      }

      public void SetDigits( int[] digits )
      {
         _digits = digits;

         VM.Digits = GetDigitsString();

         CancelAnyComputations();
         RepopulateAnswers();
      }
      public void SetStart( int start )
      {
         _start = start;

         VM.Start = GetStart();

         CancelAnyComputations();
         RepopulateAnswers();
      }
      public void SetEnd( int end )
      {
         _end = end;

         VM.End = GetEnd();

         CancelAnyComputations();
         RepopulateAnswers();
      }

      private void RepopulateAnswers()
      {
         _answers = new List<Answer>( GetEnd() - GetStart() );
         for ( int i = _start; i <= _end; i++ )
         {
            _answers.Add( new Answer( i ) );
         }

         VM.Answers.Clear();
         foreach( var answer in _answers )
         {
            VM.Answers.Add( answer.GetVM() );
         }
      }

      private void InvokeOnMainThread( Action action )
      {
         Application.Current?.Dispatcher.Invoke( action );
      }

      private void ComputeNumbers()
      {
         _puzzleSolver = new PuzzleSolver( GetDigits() )
         {
            StartValue = GetStart(),
            EndValue = GetEnd()
         };
         _puzzleSolver.CompletedValue += delegate ( object sender, CompletedValueArgs e )
         {
            var answer = _answers.FirstOrDefault( an => an.Number == e.Value );
            if ( answer is null )
               return;
            answer.Equations.Add( e.Equation );

            InvokeOnMainThread( () =>
            {
               answer.UpdateVM();
            } );
         };
         _puzzleSolver.Solve();
      }

      public void CancelAnyComputations()
      {
         if ( !( _computeThread is null ) )
         {
            _puzzleSolver.Cancel();
            _computeThread.Join();
            _computeThread = null;
         }
      }

      public void StartComputing()
      {
         CancelAnyComputations();

         RepopulateAnswers();

         _computeThread = new Thread( ComputeNumbers );
         _computeThread.Start();
      }
   }
}
