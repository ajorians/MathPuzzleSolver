﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MathPuzzleSolverWPF
{
   public class MainWindowVM : INotifyPropertyChanged
   {
      private readonly Controller _controller;
      private Config _config;

      public MainWindowVM( Controller controller)
      {
         _controller = controller;
         _config = new Config();

         _controller.AnswerItemsChanged += AnswerItemsChanged;
         _controller.StartChanged += StartChanged;
         _controller.EndChanged += EndChanged;
         _controller.DigitsChanged += DigitsChanged;
         _controller.ComputationStatusChanged += ComputationStatusChanged;
         _controller.NumberEquationComputed += NumberEquationsComputed;
         _controller.PassChanged += PassChanged;
         _controller.CurrentGroupingEquations += CurrentGroupingEquations;

         _stopComputingCommand = new RelayCommand( StopComputing );
      }

      private List<string> _curentGrouping = new List<string>();
      public List<string> CurrentGrouping
      {
         get
         {
            return _curentGrouping;
         }
         set
         {
            if (_curentGrouping != value)
            {
               _curentGrouping = value;
               OnPropertyChanged(nameof(CurrentGrouping));
               OnPropertyChanged(nameof(CurrentGroupingList));
            }
         }
      }
      public string CurrentGroupingList
      {
         get
         {
            return string.Join(", ", CurrentGrouping);
         }
      }
      private void CurrentGroupingEquations(object? sender, List<string> currentGrouping)
      {
         CurrentGrouping = currentGrouping;
      }

      private int _pass = 0;
      public int Pass
      {
         get
         {
            return _pass;
         }
         set
         {
            if (_pass != value)
            {
               _pass = value;
               OnPropertyChanged(nameof(Pass));
            }
         }
      }
      private void PassChanged(object? sender, int pass)
      {
         Pass = pass;
      }

      private int _numberEquationsCalculated = 0;
      public int NumberEquationsCalculated
      {
         get
         {
            return _numberEquationsCalculated;
         }
         set
         {
            if(_numberEquationsCalculated != value )
            {
               _numberEquationsCalculated = value;
               OnPropertyChanged(nameof(NumberEquationsCalculated));
            }
         }
      }

      private void NumberEquationsComputed(object? sender, int numberEquationsComputed)
      {
         NumberEquationsCalculated = numberEquationsComputed;
      }

      private void ComputationStatusChanged(object? sender, EventArgs e)
      {
         OnPropertyChanged(nameof(ComputationInProgress));
      }

      private static string GetDigitsString(int[] digits)
      {
         var result = string.Join(", ", digits);
         return result;
      }

      private void DigitsChanged(object? sender, int[] digits)
      {
         Digits = GetDigitsString(digits);
      }

      private void StartChanged(object? sender, int newStart)
      {
         Start = newStart;
      }

      private void EndChanged(object? sender, int newEnd)
      {
         End = newEnd;
      }

      private void AnswerItemsChanged(object? sender, EventArgs e)
      {
         _answers.Clear();

         foreach( var answer in _controller.Answers )
         {
            _answers.Add(new AnswerVM(answer, _config));
         }
      }

      private void OnPropertyChanged( string propertyName )
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private string _digits = "1,2,3,4";
      public string Digits
      {
         get
         {
            return _digits;
         }
         set
         {
            if( _digits != value )
            {
               _digits = value;
               OnPropertyChanged( nameof( Digits ) );

               _controller.SetDigits( _digits );
            }
         }
      }

      private int _start = 0;
      public int Start
      {
         get
         {
            return _start;
         }
         set
         {
            if( value != _start )
            {
               _start = value;
               OnPropertyChanged( nameof( Start ) );

               _controller.SetStart( _start );
            }
         }
      }
      private int _end = 0;
      public int End
      {
         get
         {
            return _end;
         }
         set
         {
            if ( value != _end )
            {
               _end = value;
               OnPropertyChanged( nameof( End ) );

               _controller.SetEnd( _end );
            }
         }
      }

      private ObservableCollection<AnswerVM> _answers = new ObservableCollection<AnswerVM>();
      public ObservableCollection<AnswerVM> Answers
      {
         get
         {
            return _answers;
         }
      }

      private void Compute()
      {
         try
         {
            _controller?.StartComputing();
         }
         catch (Exception ex)
         {

         }
      }

      private ICommand? _computeCommand;
      public ICommand ComputeCommand => _computeCommand ?? ( _computeCommand = new RelayCommand( Compute ) );

      private void StopComputing()
      {
         _controller.StopComputing();
      }

      private ICommand _stopComputingCommand;
      public ICommand StopComputingCommand => _stopComputingCommand;

      public bool ComputationInProgress => _controller.IsComputationInProgress();

      public event PropertyChangedEventHandler? PropertyChanged;
   }
}
