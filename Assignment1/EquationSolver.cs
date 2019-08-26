using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    /// <summary>
    /// This is a core class that solve linear expression
    /// </summary>
    class EquationSolver
    {
        readonly TokenChecker tokenChecker = new TokenChecker();

        /// <summary>
        /// This is a main procedure to run other procedures in 
        /// order to solve the equation
        /// </summary>
        public void CalculateEquation(string leftExpression, string rightExpression)
        {
            char[] leftExp = leftExpression.ToCharArray();
            char[] rightExp = rightExpression.ToCharArray();

            List<string> leftEq = CheckIfTokenised(leftExp);
            List<string> rightEq = CheckIfTokenised(rightExp);

            leftEq = ShuntingYard.ParseInfixToPostfix(leftEq);
            rightEq = ShuntingYard.ParseInfixToPostfix(rightEq);

            List<int> leftXIndexes = new List<int>();
            List<int> rightXIndexes = new List<int>();
            leftXIndexes = GetXPosition(leftEq);
            rightXIndexes = GetXPosition(rightEq);

            bool answerFound = EvaluateX(leftXIndexes, rightXIndexes, leftEq, rightEq, out int answer);

            if (answerFound)
            {
                Console.WriteLine("x = {0}", answer);
            }
            else
                Console.WriteLine("Cannot find answer");

            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        /// Check if the expression needs to be tokenised in multiple cases
        /// e.g. Check if there is unary, check if parenthesis has multiply symbol
        /// </summary>
        private List<string> CheckIfTokenised(char[] tokenList)
        {
            List<string> newList = new List<string>();

            for (int i = 0; i < tokenList.Length; i++)
            {
                char token = tokenList[i];
                if (newList.Count != 0)
                {
                    string topNewToken = newList[newList.Count - 1];
                    // If more than one digit, then combine with the previous digit 
                    if (char.IsDigit(token) && tokenChecker.IsNumber(topNewToken))
                    {
                        newList[newList.Count - 1] += token;
                    }
                    // If there is an X after a number
                    else if (token.Equals('x') && tokenChecker.IsNumber(topNewToken))
                    {
                        newList.Add("*");
                        newList.Add(token.ToString());
                    }
                    // Add multiply symbol if the token meets one of those cases
                    else if (token.Equals('(') && (tokenChecker.IsNumber(topNewToken) || topNewToken.Equals(")") || topNewToken.Equals("x")))
                    {
                        newList.Add("*");
                        newList.Add(token.ToString());
                    }
                    else
                    {
                        newList.Add(token.ToString());
                    }
                }
                else
                {
                    newList.Add(token.ToString());
                }
            }
            
            // Those methods below can only be executed after adding 
            // symbol process is completed
            CheckLastBlankNumber(ref newList);
            CheckMultipleOperators(ref newList);
            CheckUnary(ref newList);

            return newList;
        }

        /// <summary>
        /// Check if last token in the expression is an operator without number
        /// </summary>
        private void CheckLastBlankNumber(ref List<string> tokenList)
        {
            string lastToken = tokenList[tokenList.Count - 1];
            switch (lastToken)
            {
                case "+":
                case "-":
                    tokenList.Add("0");
                    break;
                case "*":
                case "/":
                    throw new ArgumentException("Invalid input");
            }
        }

        /// <summary>
        /// Check if multiple operators occured in the expression
        /// </summary>
        private void CheckMultipleOperators(ref List<string> tokenList)
        {
            for (int i = 0; i < tokenList.Count; i++)
            {
                string token = tokenList[i];
                if (tokenChecker.IsOperator(token))
                {
                    if (tokenList[i + 1].Equals(token))
                    {
                        tokenList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Check if the expression has unary operator
        /// </summary>
        private void CheckUnary(ref List<string> tokenList)
        {
            for (int i = 0; i < tokenList.Count; i++)
            {
                if (i == 0 || tokenList[i - 1].Equals("(") || tokenChecker.IsOperator(tokenList[i - 1]))
                {
                    // If it meets minus symbol, add '(', '0', and ')' to construct a  
                    if (tokenList[i].Equals("-"))
                    {
                        tokenList.Insert(i, "(");
                        tokenList.Insert(i + 1, "0");
                        tokenList.Insert(i + 4, ")");
                    }
                    // If it meets plus symbol, remove the current token which is plus symbol
                    else if (tokenList[i].Equals("+"))
                    {
                        tokenList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Return true if left expression match result with right expression
        /// </summary>
        private bool EvaluateX(List<int> leftXIndexes, List<int> rightXIndexes, List<string> leftExpression, List<string> rightExpression, out int answer)
        {
            int xNumber = -999;
            bool answerFound = false;
            int leftResult = 0;
            int rightResult = 0;

            while (!answerFound)
            {
                if (xNumber == 1000)
                    break;

                foreach (int xIndex in leftXIndexes)
                {
                    leftExpression[xIndex] = xNumber.ToString();
                }

                foreach (int xIndex in rightXIndexes)
                {
                    rightExpression[xIndex] = xNumber.ToString();
                }

                leftResult = EvaluatePostfix(leftExpression);
                rightResult = EvaluatePostfix(rightExpression);

                if (leftResult == rightResult)
                {
                    answerFound = true;
                    answer = xNumber;
                    return true;
                }
                else
                    xNumber++;
            }
            answer = 0;
            return false;
        }

        /// <summary>
        /// Evaluate postfix expression to return an answer
        /// </summary>
        private int EvaluatePostfix(List<string> expression)
        {
            Stack<string> numberStack = new Stack<string>();
            List<string> outputList = new List<string>();

            foreach (string token in expression)
            {
                //If the token is a number or an operand, push it to the stack
                if (tokenChecker.IsNumber(token) || token.Equals("x"))
                {
                    tokenChecker.CheckIfNumberInIntegerRange(token);
                    numberStack.Push(token);
                }
                //Else the token is an operator, calculate the other two numbers from the top of the stack
                else
                {
                    string firstStackValue = numberStack.Peek();
                    string secondStackValue = numberStack.Skip(1).First();

                    if (firstStackValue.Contains("x") || secondStackValue.Contains("X") || tokenChecker.IsOperator(firstStackValue) || tokenChecker.IsOperator(secondStackValue))
                    {
                        numberStack.Push(token);
                        continue;
                    }

                    double rightToken = Convert.ToDouble(numberStack.Pop());
                    double leftToken = Convert.ToDouble(numberStack.Pop());

                    double result = Operator.ApplyOperator(rightToken, leftToken, token);
                    numberStack.Push(result.ToString());
                }
            }

            outputList = numberStack.Reverse().ToList();
            return Convert.ToInt32(outputList[0]);
        }

        /// <summary>
        /// Get list of indexes of x (position of x in the expression)
        /// </summary>
        private List<int> GetXPosition(List<string> expression)
        {
            List<int> xPositions = new List<int>();

            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i].Equals("x"))
                    xPositions.Add(i);
            }

            return xPositions;
        }

    }
}
