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
      private int[] _digits = new int[]{};
      private int _start = 0;
      private int _end = 0;

      private List<Answer> _answers = new List<Answer>();
      public ReadOnlyCollection<Answer> Answers
      {
         get
         {
            return _answers.AsReadOnly();
         }
      }

      private PuzzleSolver _puzzleSolver;
      private Thread _computeThread;

      public event EventHandler AnswerItemsChanged;
      public event EventHandler<int> StartChanged;
      public event EventHandler<int> EndChanged;
      public event EventHandler<int[]> DigitsChanged;
      public event EventHandler ComputationStatusChanged;

      public Controller()
      {
      }

      ~Controller()
      {
         CancelAnyComputations();
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

         CancelAnyComputations();
         DigitsChanged?.Invoke(this, GetDigits());
         RepopulateAnswers();
      }
      public void SetStart( int start )
      {
         _start = start;

         CancelAnyComputations();
         StartChanged?.Invoke(this, GetStart());
         RepopulateAnswers();
      }
      public void SetEnd( int end )
      {
         _end = end;

         CancelAnyComputations();
         EndChanged?.Invoke(this, GetEnd());
         RepopulateAnswers();
      }

      private void RepopulateAnswers()
      {
         _answers = new List<Answer>( GetEnd() - GetStart() );
         for ( int i = _start; i <= _end; i++ )
         {
            _answers.Add( new Answer( i ) );
         }

         AnswerItemsChanged?.Invoke(this, EventArgs.Empty);
      }

      private void InvokeOnMainThread( Action action )
      {
         Application.Current?.Dispatcher.BeginInvoke( action );
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

            InvokeOnMainThread(() => answer.AddEquation(e.Equation));
         };
         _puzzleSolver.FinishedComputing += delegate (object sender, EventArgs e)
         {
            CancelAnyComputations();
         };
         _puzzleSolver.Solve();
      }

      public void CancelAnyComputations()
      {
         if (IsComputationInProgress())
         {
            _puzzleSolver.Cancel();
            _computeThread.Join();
            _computeThread = null;
            ComputationStatusChanged?.Invoke(this, EventArgs.Empty);
         }
      }

      public void StartComputing()
      {
         CancelAnyComputations();

         RepopulateAnswers();

         _computeThread = new Thread( ComputeNumbers );
         _computeThread.Start();

         ComputationStatusChanged?.Invoke(this, EventArgs.Empty);
      }

      public void StopComputing()
      {
         CancelAnyComputations();
      }

      public bool IsComputationInProgress() => !(_computeThread is null);
   }
}
