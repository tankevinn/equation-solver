using System;
using System.Collections.Generic;

namespace Assignment1
{
    /// <summary>
    /// This is a class that contains every logic
    /// related to the operator
    /// </summary>
    class Operator
    {
        // a dictionary containing operator precedence
        private static readonly Dictionary<string, int> opPrecedence = new Dictionary<string, int>()
        {
            {"(", 0}, {")", 0},
            {"+", 1}, {"-", 1},
            {"*", 2}, {"/", 2},
        };
        
        /// <summary>
        /// Return true if the currentOperator has more precedence 
        /// than the newly taken Operator
        /// </summary>
        public static bool IsGreaterOp(string currentOp, string newOp)
        {
            if (currentOp.ToLower() == "x")
                return false;

            if (opPrecedence[currentOp] >= opPrecedence[newOp])
                return true;

            return false;
        }

        /// <summary>
        /// Apply calculation based on the operator input
        /// </summary>
        public static double ApplyOperator(double rightVal, double leftVal, string op)
        {
            double result = 0;
            switch (op)
            {
                case "+":
                    result = Add(leftVal, rightVal);
                    break;
                case "-":
                    result = Subtract(leftVal, rightVal);
                    break;
                case "*":
                    result = Multiply(leftVal, rightVal);
                    break;
                case "/":
                    if (rightVal == 0)
                        throw new Exception("Division by zero");
                    result = Math.Round(Divide(leftVal, rightVal));
                    break;
                default:
                    break;
            }
            return result;
        }
        
        /// <summary>
        /// Add leftValue and rightValue double to return result
        /// </summary>
        private static double Add(double leftVal, double rightVal)
        {
            return leftVal + rightVal;
        }
        
        /// <summary>
        /// Subtract leftValue and rightValue double to return result
        /// </summary>
        private static double Subtract(double leftVal, double rightVal)
        {
            return leftVal - rightVal;
        }

        /// <summary>
        /// Multiply leftValue and rightValue double to return result
        /// </summary>
        private static double Multiply(double leftVal, double rightVal)
        {
            return leftVal * rightVal;
        }

        /// <summary>
        /// Divide leftValue and rightValue double to return result
        /// </summary>
        private static double Divide(double leftVal, double rightVal)
        {
            return leftVal / rightVal;
        }

    }
}
