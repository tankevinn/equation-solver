using System;
using System.Linq;

namespace Assignment1
{
    /// <summary>
    /// This is a class to check type of token 
    /// and validate input before being calculated 
    /// later in the EquationSolver
    /// </summary>
    class TokenChecker
    {
        /// <summary>
        /// Return true if input is an operator
        /// </summary>
        public bool IsOperator(string input)
        {
            string[] operators = { "+", "-", "*", "/" };

            if (operators.Contains(input))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return true if input is a number
        /// </summary>
        public bool IsNumber(string token)
        {
            bool isNumeric = int.TryParse(token, out int n);
            return isNumeric;
        }

        /// <summary>
        /// Validate input by checking if input contains 
        /// necessary keywords
        /// </summary>
        public void ValidateInput(string input)
        {
            if (!input.Contains("calc"))
            {
                Console.WriteLine("Input must be type with keyword 'calc'");
                throw new ArgumentException("input should have 'calc' keyword");
            }
            else if (!input.Contains("="))
            {
                Console.WriteLine("Invalid Input (this is not equation and invalid as well)");
                throw new ArgumentException("input should have equal symbol");
            }
        }

        /// <summary>
        /// Modify the string input so that it can be processed in the equation solver
        /// </summary>
        public void ModifyTokenFromInput(string input, out string output)
        {
            output = input.ToLower().Replace(" ", string.Empty).Replace("calc", string.Empty);
        }

        /// <summary>
        /// Check if the number is in integer range
        /// </summary>
        public void CheckIfNumberInIntegerRange(string number)
        {

        }
    }
}
