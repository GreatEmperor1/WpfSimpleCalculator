using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double firstNumber, secondNumber, resultNumber = 0;
        bool calcDone = false;
        Operations operation = Operations.None;
        string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public MainWindow()
        {
            InitializeComponent();

            //Assign to the decimal button the separator from the current culture
            dec.Content = separator;
        }

        //List the possible numeric operations
        private enum Operations
        {
            None,
            Division,
            Multiplication,
            Subtraction,
            Sum
        }

        //Manage number buttons input
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (calcDone) //calculation already done
            {
                result.Content = $"{button.Content}";
                calcDone = false;
            }
            else //calculation not yet done
            {
                if (result.Content.ToString() == "0")
                {
                    result.Content = $"{button.Content}";
                }
                else
                {
                    result.Content = $"{result.Content}{button.Content}";
                }
            }

        }

        //Manage operation buttons input
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            //Evaluate button pressed
            switch (button.Content.ToString())
            {
                case "sin x":
                    if (!(result.Content.ToString() == "0"))
                    {
                        result.Content = Math.Sin(Convert.ToDouble(result.Content));
                    }
                    break;
                case "cos x":
                    if (!(result.Content.ToString() == "0"))
                    {
                        RadioButton radioButton = null;
                        radioButton = GetCheckedRadioButton(radioButtons.Children, "func");
                        //Determine checked radioButton
                        if (radioButton.Content.ToString() == "degrees")
                        {
                            result.Content = Convert.ToDouble(result.Content) * (Math.PI / 180);
                        }
                        else if (radioButton.Content.ToString() == "grad")
                        {
                            result.Content = Convert.ToDouble(result.Content) * (Math.PI / 200);
                        }
                        else
                        {
                            result.Content = result.Content;
                        }
                        result.Content = Math.Cos(Convert.ToDouble(result.Content));
                    }
                    break;
                case "tg x":
                    if (!(result.Content.ToString() == "0"))
                    {
                        result.Content = Math.Tan(Convert.ToDouble(result.Content));
                    }
                    break;
                case "ctg x":
                    if (!(result.Content.ToString() == "0"))
                    {
                        result.Content = 1.0 / Math.Tan(Convert.ToDouble(result.Content));
                    }
                    break;
                case "AC":
                    result.Content = "0";
                    break;
                case "+/-":
                    if (!(result.Content.ToString() == "0"))
                    {
                        result.Content = Convert.ToDouble(result.Content) * -1;
                    }
                    break;
                case "%":
                    if (!(result.Content.ToString() == "0"))
                    {
                        result.Content = Convert.ToDouble(result.Content) / 100;
                    }
                    break;
                case "÷":
                    firstNumber = Convert.ToDouble(result.Content);
                    operation = Operations.Division;
                    result.Content = "0";
                    break;
                case "×":
                    firstNumber = Convert.ToDouble(result.Content);
                    operation = Operations.Multiplication;
                    result.Content = "0";
                    break;
                case "-":
                    firstNumber = Convert.ToDouble(result.Content);
                    operation = Operations.Subtraction;
                    result.Content = "0";
                    break;
                case "+":
                    firstNumber = Convert.ToDouble(result.Content);
                    operation = Operations.Sum;
                    result.Content = "0";
                    break;
                case "=":
                    switch (operation)
                    {
                        case Operations.Division:
                            if (calcDone)
                            {
                                if (!(result.Content.ToString() == "ERROR"))
                                {
                                    result.Content = Convert.ToDouble(result.Content) / secondNumber;
                                }
                            }
                            else
                            {
                                //Check if division by 0
                                if (result.Content.ToString() == "0")
                                {
                                    result.Content = "ERROR";
                                    calcDone = true;
                                }
                                else
                                {
                                    secondNumber = Convert.ToDouble(result.Content);
                                    resultNumber = firstNumber / secondNumber;
                                    result.Content = resultNumber;
                                    calcDone = true;
                                }
                            }
                            break;
                        case Operations.Multiplication:
                            if (calcDone)
                            {
                                result.Content = Convert.ToDouble(result.Content) * secondNumber;
                            }
                            else
                            {
                                secondNumber = Convert.ToDouble(result.Content);
                                resultNumber = firstNumber * secondNumber;
                                result.Content = resultNumber;
                                calcDone = true;
                            }
                            break;
                        case Operations.Subtraction:
                            if (calcDone)
                            {
                                result.Content = Convert.ToDouble(result.Content) - secondNumber;
                            }
                            else
                            {
                                secondNumber = Convert.ToDouble(result.Content);
                                resultNumber = firstNumber - secondNumber;
                                result.Content = resultNumber;
                                calcDone = true;
                            }
                            break;
                        case Operations.Sum:
                            if (calcDone)
                            {
                                result.Content = Convert.ToDouble(result.Content) + secondNumber;
                            }
                            else
                            {
                                secondNumber = Convert.ToDouble(result.Content);
                                MessageBox.Show($"{firstNumber} + {secondNumber}");
                                resultNumber = firstNumber + secondNumber;
                                result.Content = resultNumber;
                                calcDone = true;
                            }
                            break;
                    }
                    break;
                default:
                    if (!result.Content.ToString().Contains(separator))
                    {
                        result.Content = $"{result.Content}{button.Content}";
                    }
                    break;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            //MessageBox.Show(pressed.Content.ToString());
        }

        private RadioButton GetCheckedRadioButton(UIElementCollection children, String groupName)
        {
            return children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true && rb.GroupName == groupName);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Min.Content.ToString() + " отмечен");
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Min.Content.ToString() + " не отмечен");
        }

        private void checkBox_Indeterminate(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Min.Content.ToString() + " в неопределенном состоянии");
        }
    }
}
