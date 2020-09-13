using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathPuzzleSolverWPF
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();

         var controller = new Controller();
         var vm = new MainWindowVM( controller );

         controller.SetStart(0);
         controller.SetEnd(20);
         controller.SetDigits(new int[] { 1, 2, 3, 4 });

         this.DataContext = vm;

         this.Closing += delegate ( object sender, CancelEventArgs e )
         {
            controller.CancelAnyComputations();
         };
      }
   }
}
