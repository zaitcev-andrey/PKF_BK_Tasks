using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorWpf
{
    internal class CalculationExpressions
    {
        public static string ArithmeticToPolishNotation(string arithmeticExpression)
        {
            // будет 1 стек с операциями и 1 стек с результатом
            Stack<string> resultStack = new Stack<string>();
            Stack<char> operations = new Stack<char>();

            int startPosition = 0;
            int operandLength = 0;
            int leftBracket = 0;
            int rightBracket = 0;

            int operationsCounter = 0;
            // обрабатываем исходную строку на лишние пробелы
            arithmeticExpression = String.Concat(arithmeticExpression.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < arithmeticExpression.Length; i++)
            {
                char c = arithmeticExpression[i];
                if ((c >= '0' && c <= '9') || c == ',')
                {
                    if (operandLength == 0)
                        startPosition = i;

                    operandLength++;
                    if (i == arithmeticExpression.Length - 1)
                    {
                        resultStack.Push(arithmeticExpression.Substring(startPosition, operandLength));
                    }
                }
                else // если пришла операция
                {
                    // сначала перекидываем число(операнд) в результирующий стек
                    if (operandLength != 0)
                    {
                        resultStack.Push(arithmeticExpression.Substring(startPosition, operandLength));
                        operandLength = 0;
                        operationsCounter = 0;
                    }

                    // проверяем на '('
                    if (c == '(')
                    {
                        operations.Push(c);
                        leftBracket++;
                        operationsCounter = 0;
                        continue;
                    }

                    // Здесь проверяем на наличие подряд идущих операций.

                    operationsCounter++;
                    if (arithmeticExpression.Length - 4 >= i && arithmeticExpression.Substring(i, 4) == "sqrt")
                    {
                        if ((i > 0 && arithmeticExpression[i - 1] == ')'))
                            throw new Exception("Ошибка, операция взятия корня идёт сразу после закрывающей скобки!!!");
                        operationsCounter = 1;
                    }
                    if (operationsCounter == 2)
                        throw new Exception("Ошибка, в выражение есть подряд идущие операции!!!");
                    // эта проверка обязательно идёт после предыдущей
                    if (c == ')' || c == '%')
                        operationsCounter = 0;

                    // проверяем на остальные операции
                    // % - приоритет 4
                    if (c == '%')
                    {
                        if (operations.Count > 0 && operations.Peek() == '%')
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        operations.Push(c);
                    }
                    // ^ и sqrt - приоритет 3
                    else if (c == '^' ||
                        (arithmeticExpression.Length - 4 >= i && arithmeticExpression.Substring(i, 4) == "sqrt"))
                    {
                        if (operations.Count > 0 && operations.Peek() == '%')
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }

                        int tmp_ind = i;
                        if (arithmeticExpression.Length - 4 >= i && arithmeticExpression.Substring(i, 4) == "sqrt")
                            i += 3;

                        if (operations.Count > 0 && (operations.Peek() == '^' || operations.Peek() == 's'))
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }

                        if (tmp_ind == i)
                            operations.Push('^');
                        else operations.Push('s');
                    }
                    // * и / - приоритет 2
                    else if (c == '*' || c == '/')
                    {
                        if (operations.Count > 0 && operations.Peek() == '%')
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        if (operations.Count > 0 && (operations.Peek() == '^' || operations.Peek() == 's'))
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        if (operations.Count > 0 && (operations.Peek() == '*' || operations.Peek() == '/'))
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        operations.Push(c);
                    }
                    // - и + - приоритет 1
                    else if (c == '-' || c == '+')
                    {
                        // нужно посмотреть на вершину стека с операциями и перекинуть все операции от самого
                        // высокого приоритета к самому низкому по порядку
                        if (operations.Count > 0 && operations.Peek() == '%')
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        if (operations.Count > 0 && (operations.Peek() == '^' || operations.Peek() == 's'))
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        if (operations.Count > 0 && (operations.Peek() == '*' || operations.Peek() == '/'))
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        if (operations.Count > 0 && (operations.Peek() == '+' || operations.Peek() == '-' || operations.Peek() == '_'))
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }

                        // Обратываем отрицание. Обозначим через '_' унарный минус
                        if (c == '-')
                        {
                            if ((i > 0 && arithmeticExpression[i - 1] == '(') ||
                                (i < arithmeticExpression.Length - 1 && arithmeticExpression[i + 1] == '(') || i == 0)
                            {
                                operations.Push('_');
                                continue;
                            }
                        }

                        operations.Push(c);
                    }

                    else if (c == ')')
                    {
                        rightBracket++;
                        if (rightBracket > leftBracket)
                            throw new Exception("Ошибка!!! Количество открывающихся скобок меньше числа закрывающихся");
                        while (operations.Peek() != '(')
                        {
                            resultStack.Push(operations.Pop().ToString());
                        }
                        // удаляем '('
                        operations.Pop();
                    }

                    // в случае, если пришёл совсем не тот символ
                    else
                    {
                        throw new Exception("Ошибка! В арифметическом выражении допущена ошибка. Возможно используется неверный символ");
                    }
                }
            }
            // перекидываем остатки
            while (operations.Count > 0)
            {
                resultStack.Push(operations.Pop().ToString());
            }

            // Теперь, имея результирующий стек с операндами и с операциями, мы можем создать обратную польскую запись
            string result = String.Join(" ", resultStack.Reverse());
            return result;
        }

        public static double? PolishToValue(string polishNotation)
        {
            if (string.IsNullOrEmpty(polishNotation))
                throw new Exception("Входная строка обратной польской записи пустая или указывает на null");

            Stack<double> result = new Stack<double>();
            string[] input = polishNotation.Split();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == "+" || input[i] == "-" || input[i] == "_" || input[i] == "*" ||
                    input[i] == "/" || input[i] == "^" || input[i] == "s" || input[i] == "%")
                {
                    double value1, value2 = 1.0;
                    // только для взятия корня и процента нужно извлечь 1 число
                    if (input[i] == "s")
                    {
                        value1 = result.Pop();
                        if (value1 < 0)
                            throw new Exception("Ошибка, попытка взятия корня из отрицательного числа!");
                    }
                    else if(input[i] == "%")
                    {
                        value2 = result.Pop();
                        if (result.Count > 0)
                        {
                            value1 = result.Pop();
                            i++; // по следующему индексу точно будет операция
                            switch (input[i])
                            {
                                case "+":
                                    result.Push(value1 + value1 / 100 * value2);
                                    break;
                                case "-":
                                    result.Push(value1 - value1 / 100 * value2);
                                    break;
                                case "*":
                                    result.Push(value1 * (value2 / 100));
                                    break;
                                case "/":
                                    if (value2 == 0)
                                        throw new Exception("Ошибка, деление на нулевой процент!");
                                    result.Push(value1 / (value2 / 100));
                                    break;
                                case "^":
                                    result.Push(Math.Pow(value1, value2 / 100));
                                    break;
                            }
                        }
                        else
                        {
                            result.Push(0);
                        }
                        continue;
                    }
                    else
                    {
                        value2 = result.Pop();
                        // обрабатываем отрицание
                        if (input[i] == "_")
                        {
                            // кладём инвертируемое значение
                            result.Push(-value2);
                            continue;
                        }
                        value1 = result.Pop();
                    }
                    switch (input[i])
                    {
                        case "+":
                            result.Push(value1 + value2);
                            break;
                        case "-":
                            result.Push(value1 - value2);
                            break;
                        case "*":
                            result.Push(value1 * value2);
                            break;
                        case "/":
                            if (value2 == 0)
                                throw new Exception("Ошибка, деление на ноль!");
                            result.Push(value1 / value2);
                            break;
                        case "^":
                            result.Push(Math.Pow(value1, value2));
                            break;
                        case "s":
                            result.Push(Math.Sqrt(value1));
                            break;
                    }
                }
                else
                {
                    double value;
                    if (double.TryParse(input[i], out value))
                    {
                        result.Push(value);
                    }
                    else throw new Exception("Ошибка, Исходное выражение не соответствует обратной Польской записи");
                }
            }
            return result.Peek();
        }

        public static double? ArithmeticToValue(string arithmeticExpression)
        {
            return PolishToValue(ArithmeticToPolishNotation(arithmeticExpression));
        }
    }
}
