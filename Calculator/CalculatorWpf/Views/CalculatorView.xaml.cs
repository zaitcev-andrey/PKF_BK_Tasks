using System.Windows;
using CalculatorWpf.ViewModels;

namespace CalculatorWpf
{
    public partial class CalculatorView : Window
    {
        public CalculatorView()
        {
            InitializeComponent();

            DataContext = new CalculatorViewModel();
        }
    }
}
