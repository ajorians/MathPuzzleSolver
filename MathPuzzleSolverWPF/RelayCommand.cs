using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MathPuzzleSolverWPF
{
   public class RelayCommand : ICommand
   {
      #region Fields

      readonly Action _execute;
      readonly Func<bool> _canExecute;

      #endregion // Fields

      #region Constructors

      /// <summary>
      /// Creates a new command that can always execute.
      /// </summary>
      /// <param name="execute">The execution logic.</param>
      public RelayCommand( Action execute )
         : this( execute, () => true )
      {
      }

      /// <summary>
      /// Creates a new command.
      /// </summary>
      /// <param name="execute">The execution logic.</param>
      /// <param name="canExecute">The execution status logic.</param>
      /// <exception cref="ArgumentNullException"><c>execute</c> is null.</exception>
      public RelayCommand( Action execute, Func<bool> canExecute )
      {
         if ( execute == null )
            throw new ArgumentNullException( nameof( execute ) );

         _execute = execute;
         _canExecute = canExecute;
      }

      #endregion // Constructors

      #region ICommand Members

      [DebuggerStepThrough]
      public bool CanExecute( object parameter )
      {
         return _canExecute == null ? true : _canExecute();
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            if ( _canExecute != null )
               CommandManager.RequerySuggested += value;
         }
         remove
         {
            if ( _canExecute != null )
               CommandManager.RequerySuggested -= value;
         }
      }

      public void Execute( object parameter )
      {
         _execute();
      }

      #endregion // ICommand Members
   }
}
