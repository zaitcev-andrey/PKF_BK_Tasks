using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalculatorWpf.Models
{
    internal class CalculationModel : INotifyPropertyChanged
    {
        #region private Members
        private string _arithmeticExpression;
        private string _polishNotation;
        private string _logError;
        private double? _result;
        #endregion

        #region public Properties
        public string ArithmeticExpression
        {
            get { return _arithmeticExpression; }
            set 
            { 
                _arithmeticExpression = value;
                OnPropertyChanged("ArithmeticExpression");
            }
        }

        public string PolishNotation
        {
            get { return _polishNotation; }
            set
            {
                _polishNotation = value;
                OnPropertyChanged("PolishNotation");
            }
        }

        public string LogError
        {
            get { return _logError; }
            set
            {
                _logError = value;
                OnPropertyChanged("LogError");
            }
        }

        public double? Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
