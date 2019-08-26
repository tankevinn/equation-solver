using System;

namespace Assignment1
{
    /// <summary>
    /// This is a class that contain main method to run the 
    /// equation solver program
    /// </summary>
    class Program
    {
        /// <summary>
        /// This is a main method that contains core of the program
        /// </summary>
        static void Main(string[] args)
        {
            string userInput = string.Empty;
            TokenChecker tokenChecker = new TokenChecker();
            EquationSolver eqSolver = new EquationSolver();

            // Do the equation solver until user type "exit"
            do
            {
                Console.WriteLine("Run the program with keyword 'calc <equation>' or exit to exit the program.");
                userInput = Console.ReadLine();

                // Validate the input before calculating it
                tokenChecker.ValidateInput(userInput);
                tokenChecker.ModifyTokenFromInput(userInput, out string output);
                
                string[] equations = output.Split('=');
                eqSolver.CalculateEquation(equations[0], equations[1]);
            }
            while (!userInput.Equals("exit"));
        }

    }

    
}
