//Johnny Le, u0748407 CS 3500
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Evaluates arithmetic expressions written using standard infix
    /// notation.
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Accepts an outside method that takes the input of a string
        /// and returns an int. In this class, it is used to find integer
        /// equivalents for variables.
        /// </summary>
        /// <param name="v"> v is a string made up of characters that has an equivalent value</param>
        /// <returns> It will return an integer. </returns>
        public delegate int Lookup(String v);

        /// <summary>
        /// The primary method in which is called to evaluate the expression. It takes in
        /// the expression as a string and a Lookup function designated for determining 
        /// variable equivalence in integer form.
        /// </summary>
        /// <param name="exp"> The arithmetic expression in infix notation. </param>
        /// <param name="variableEvaluator"> The method/function used to convert the variable into an integer. </param>
        /// <returns> The result of the evaluation is returned as an integer </returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            // Global Variables

            int result = 0;
            var values = new Stack<int>();
            var operators = new Stack<string>();

            // Conversion of the string into substrings

            string s = exp;
            string[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            // Analysis of substrings. Substrings are divided into stacks of values and
            // operators in order to determine the result.

            for (int i = 0; i < substrings.Length; i++)
            {
                // The substrings are trimmed before processing.

                substrings[i] = substrings[i].Trim();

                // If substrings[i] is an integer.

                int value;
                if (int.TryParse(substrings[i], out value))
                {
                    if (operators.Count != 0 && operators.Peek().Equals("*"))
                    {
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("The value stack is empty");
                        }
                        int popv = values.Pop();
                        operators.Pop();
                        int evalv = popv * value;
                        values.Push(evalv);
                    }
                    else if (operators.Count != 0 && operators.Peek().Equals("/"))
                    {
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("The value stack is empty");
                        }
                        int popv = values.Pop();
                        operators.Pop();

                        if (value == 0)
                        {
                            throw new ArgumentException("A division by zero results");
                        }

                        int evalv = popv / value;
                        values.Push(evalv);
                    }
                    else
                        values.Push(value);
                }

                // If substrings[i] is a variable.

                else if (Regex.IsMatch(substrings[i], @"^[a-z]*[A-Z]*\d+$"))
                {
                    int variable_value = variableEvaluator(substrings[i]);
                    if (operators.Count != 0 && operators.Peek().Equals("*"))
                    {
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("The value stack is empty");
                        }
                        int popv = values.Pop();
                        operators.Pop();
                        int evalv = popv * variable_value;
                        values.Push(evalv);
                    }
                    else if (operators.Count != 0 && operators.Peek().Equals("/"))
                    {
                        if (values.Count == 0)
                        {
                            throw new ArgumentException("The value stack is empty");
                        }
                        int popv = values.Pop();
                        operators.Pop();

                        if (value == 0)
                        {
                            throw new ArgumentException("A division by zero results");
                        }

                        int evalv = popv / variable_value;
                        values.Push(evalv);
                    }
                    else
                        values.Push(variable_value);
                }

                // If substrings[i] is + or -, the stacks are evaluated if there are + or - operators on top.
                // The substrings[i] is pushed onto the stack afterwards.

                else if (substrings[i].Equals("+") | substrings[i].Equals("-"))
                {

                    if (operators.Count != 0 && operators.Peek().Equals("+"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Value stack contains fewer than 2 values!");
                        }
                        int popv = values.Pop();
                        int popv2 = values.Pop();
                        operators.Pop();
                        int evalv = popv + popv2;
                        values.Push(evalv);
                    }
                    else if (operators.Count != 0 && operators.Peek().Equals("-"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Value stack contains fewer than 2 values!");
                        }
                        int popv = values.Pop();
                        int popv2 = values.Pop();
                        operators.Pop();
                        int evalv = popv - popv2;
                        values.Push(evalv);
                    }

                    operators.Push(substrings[i]);
                }

                // If substrings[i] is * or /, the substrings[i] is pushed on the operators stack.

                else if (substrings[i].Equals("*") | substrings[i].Equals("/"))
                {

                    operators.Push(substrings[i]);
                }

                // If substrings[i] is a left parenthesis, the substrings[i] is pushed on the operators stack.

                else if (substrings[i].Equals("("))
                {
                    operators.Push(substrings[i]);
                }


                // If substrings[i] is a right parenthesis, the stack is checked for + or -.

                else if (substrings[i].Equals(")"))
                {
                    if (operators.Count != 0 && operators.Peek().Equals("+"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("The value stack contains fewer than 2 values during the first step");
                        }
                        int popv = values.Pop();
                        int popv2 = values.Pop();
                        operators.Pop();
                        int evalv = popv + popv2;
                        values.Push(evalv);
                    }
                    else if (operators.Count != 0 && operators.Peek().Equals("-"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("The value stack contains fewer than 2 values during the first step");
                        }
                        int popv = values.Pop();
                        int popv2 = values.Pop();
                        operators.Pop();
                        int evalv = popv - popv2;
                        values.Push(evalv);
                    }


                    if (operators.Count != 0 && !(operators.Peek().Equals("(")))
                    {
                        throw new ArgumentException("A ( isn't found where expected");
                    }

                    operators.Pop();
                    if (operators.Count != 0 && operators.Peek().Equals("*"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("The value stack contains fewer than 2 values during the second step");
                        }
                        int popv = values.Pop();
                        int popv2 = values.Pop();
                        operators.Pop();
                        int evalv = popv * popv2;
                        values.Push(evalv);
                    }
                    else if (operators.Count != 0 && operators.Peek().Equals("/"))
                    {
                        if (values.Count < 2)
                        {
                            throw new ArgumentException("Value stack contains fewer than 2 values!");
                        }
                        int popv = values.Pop();
                        int popv2 = values.Pop();
                        operators.Pop();

                        if (value == 0)
                        {
                            throw new ArgumentException("A division by zero results");
                        }

                        int evalv = popv / popv2;
                        values.Push(evalv);
                    }
                }

            }

            // End of for loop.
            // If operators stack is empty, return the result.
            if (operators.Count == 0)
            {
                if (values.Count != 1)
                {
                    throw new ArgumentException("There isn't exactly one value on the value stack");
                }
                result = values.Pop();
            }

            //If operators stack is not empty, evaluate bottom of stack and return result.
            else
            {
                if (operators.Count != 1 | values.Count != 2)
                {
                    throw new ArgumentException("There isn't exactly one operator on the operator stack or exactly two numbers on the value stack");
                }
                if (operators.Count != 0 && operators.Peek().Equals("+"))
                {
                    int popv = values.Pop();
                    int popv2 = values.Pop();
                    operators.Pop();
                    int evalv = popv + popv2;
                    result = evalv;
                }
                else if (operators.Count != 0 && operators.Peek().Equals("-"))
                {
                    int popv = values.Pop();
                    int popv2 = values.Pop();
                    operators.Pop();
                    int evalv = popv - popv2;
                    result = evalv;
                }
            }

            // Return final result.
            return result;
        }

    }
}
