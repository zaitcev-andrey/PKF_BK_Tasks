using CalculatorWpf.Commands;
using CalculatorWpf.Models;
using System;

namespace CalculatorWpf.ViewModels
{
    internal class CalculatorViewModel
    {
        #region private Members
        private CalculationModel _model;

        private int lastSymbol;
        private bool isCommaInNumber = false;
        private int openBracketCounter = 0;
        private int closeBracketCounter = 0;
        #endregion

        #region public Properties
        public CalculationModel Model 
        { 
            get { return _model; }
        }
        #endregion

        #region Constructors
        public CalculatorViewModel()
        {
            _model = new CalculationModel();

            lastSymbol = -1;
        }
        #endregion

        #region private Commands
        private RelayCommand _addNumber;

        private RelayCommand _addOperation;

        private RelayCommand _addComma;

        private RelayCommand _addPercent;
        private RelayCommand _addSquareRoot;

        private RelayCommand _addLeftBracket;
        private RelayCommand _addRightBracket;

        private RelayCommand _eraseAll;
        private RelayCommand _eraseOneSymbol;

        private RelayCommand _equality;
        #endregion

        #region public Commands
        public RelayCommand AddNumber
        {
            get
            {
                return _addNumber ??
                (_addNumber = new RelayCommand(obj =>
                {
                    if (lastSymbol != 2 && lastSymbol != 4)
                    {
                        lastSymbol = 0;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand AddOperation
        {
            get
            {
                return _addOperation ??
                (_addOperation = new RelayCommand(obj =>
                {
                    if (lastSymbol == 0 || lastSymbol == 2 || lastSymbol == 4)
                    {
                        lastSymbol = 1;
                        isCommaInNumber = false;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand AddComma
        {
            get
            {
                return _addComma ??
                (_addComma = new RelayCommand(obj =>
                {
                    if (lastSymbol == 0 && !isCommaInNumber)
                    {
                        lastSymbol = 3;
                        isCommaInNumber = true;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand AddPercent
        {
            get
            {
                return _addPercent ??
                (_addPercent = new RelayCommand(obj =>
                {
                    if (lastSymbol == 0 || lastSymbol == 2)
                    {
                        lastSymbol = 4;
                        isCommaInNumber = false;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand AddSquareRoot
        {
            get
            {
                return _addSquareRoot ??
                (_addSquareRoot = new RelayCommand(obj =>
                {
                    if (lastSymbol == 1 || lastSymbol == -1)
                    {
                        lastSymbol = 1;
                        isCommaInNumber = false;
                        openBracketCounter++;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand AddLeftBracket
        {
            get
            {
                return _addLeftBracket ??
                (_addLeftBracket = new RelayCommand(obj =>
                {
                    if (lastSymbol == 1 || lastSymbol == -1)
                    {
                        lastSymbol = 1;
                        isCommaInNumber = false;
                        openBracketCounter++;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand AddRightBracket
        {
            get
            {
                return _addRightBracket ??
                (_addRightBracket = new RelayCommand(obj =>
                {
                    if (closeBracketCounter != openBracketCounter && (lastSymbol == 0 || lastSymbol == 2 || lastSymbol == 4))
                    {
                        lastSymbol = 2;
                        isCommaInNumber = false;
                        closeBracketCounter++;
                        string c = (string)obj;
                        Model.ArithmeticExpression += c;
                    }
                }));
            }
        }

        public RelayCommand EraseAll
        {
            get
            {
                return _eraseAll ??
                (_eraseAll = new RelayCommand(obj =>
                {
                    lastSymbol = -1;
                    isCommaInNumber = false;
                    openBracketCounter = 0;
                    closeBracketCounter = 0;
                    Model.ArithmeticExpression = "";
                },
                (obj) => !string.IsNullOrEmpty(Model.ArithmeticExpression)));
            }
        }
        public RelayCommand EraseOneSymbol
        {
            get
            {
                return _eraseOneSymbol ??
                (_eraseOneSymbol = new RelayCommand(obj =>
                {
                    string s = Model.ArithmeticExpression;
                    if (s[s.Length - 1] == '(')
                        openBracketCounter--;
                    else if (s[s.Length - 1] == ')')
                        closeBracketCounter--;

                    Model.ArithmeticExpression = s.Substring(0, s.Length - 1);

                    if (s.Length == 1)
                        lastSymbol = -1;
                    else
                    {
                        char c = s[s.Length - 2];
                        if(c == 't')
                        {
                            Model.ArithmeticExpression = s.Substring(0, s.Length - 5);
                            if(s.Length - 6 >= 0)
                                c = s[s.Length - 6];
                            else
                            {
                                lastSymbol = -1;
                                return;
                            }
                        }

                        if (c >= '0' && c <= '9')
                            lastSymbol = 0;
                        else if (c == '(' || c == '/' || c == '*' || c == '-' || c == '+' || c == '^')
                            lastSymbol = 1;
                        else if (c == ')')
                            lastSymbol = 2;
                        else if (c == ',')
                            lastSymbol = 3;
                        else if (c == '%')
                            lastSymbol = 4;
                    }
                },
                (obj) => !string.IsNullOrEmpty(Model.ArithmeticExpression)));
            }
        }

        public RelayCommand Equality
        {
            get
            {
                return _equality ??
                (_equality = new RelayCommand(obj =>
                {
                    try
                    {
                        Model.PolishNotation = "";
                        Model.Result = null;
                        Model.PolishNotation = CalculationExpressions.ArithmeticToPolishNotation(Model.ArithmeticExpression);
                        Model.Result = CalculationExpressions.PolishToValue(Model.PolishNotation);
                        if (!string.IsNullOrEmpty(Model.LogError))
                            Model.LogError = "";
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Стек пуст.")
                            Model.LogError = "Ошибка, в исходном выражении или обратной польской записи была допущена ошибка!!!";
                        else
                            Model.LogError = ex.Message;
                    }

                    lastSymbol = -1;
                    isCommaInNumber = false;
                    openBracketCounter = 0;
                    closeBracketCounter = 0;
                    Model.ArithmeticExpression = "";
                },
                (obj) => !string.IsNullOrEmpty(Model.ArithmeticExpression)));
            }
        }
        #endregion

        #region Rules for last symbol
        /* Создадим переменную, которая будет запоминать в себе последний введённый символ (lastSymbol)
        Если она равна 0, то было введено число (0 - 9)
        Если равна 1, то операция или спец.символ ('-', '+', '*', '/', '^', '(' )
        Если равна 2, то была ')'
        Если равна 3, то была ','
        Если равна 4, то был '%'
        Если равна -1, то в строке ещё ничего нет
        */
        #endregion
    }
}
