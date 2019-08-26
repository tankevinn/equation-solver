using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    /// <summary>
    /// This is a class that contains Shunting Yard Algorithm logic
    /// to parse infix expression to postfix expression so that it can 
    /// be read easily by computer
    /// </summary>
    class ShuntingYard
    {
        /// <summary>
        /// Parse infix to postfix using shunting yard algorithm
        /// Source: https://en.wikipedia.org/wiki/Shunting-yard_algorithm
        /// </summary>
        public static List<string> ParseInfixToPostfix(List<string> tokenList)
        {
            TokenChecker tokenChecker = new TokenChecker();
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            // While there are tokens to be read
            foreach (string token in tokenList)
            {
                string stringToken = token.ToString();
                // If the token is a number, push it to the queue
                if (tokenChecker.IsNumber(stringToken))
                {
                    outputQueue.Enqueue(stringToken);
                }
                // If the token is 'x', push it to the stack
                else if (stringToken.ToLower().Contains("x"))
                {
                    operatorStack.Push(stringToken);
                }
                // If the token is an operator, do some logic by checking
                // operator precedence
                else if (tokenChecker.IsOperator(stringToken))
                {
                    if (operatorStack.Count != 0)
                    {
                        string topOp = operatorStack.Peek();
                        while (topOp.ToLower().Contains("x") || (Operator.IsGreaterOp(topOp, stringToken)) && topOp != "(")
                        {
                            var currentOp = operatorStack.Pop();
                            outputQueue.Enqueue(currentOp);

                            if (operatorStack.Count != 0)
                            {
                                topOp = operatorStack.Peek();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    operatorStack.Push(stringToken);
                }
                // If the token is '(', push it to the stack
                else if (token.Equals("("))
                    operatorStack.Push(stringToken);
                // If the token is ')', push tokens in stack to the queue
                // until the token from the stack equals '('
                else if (token.Equals(")"))
                {
                    var currentToken = operatorStack.Peek();
                    while (!currentToken.Equals("("))
                    {
                        var opToken = operatorStack.Pop();
                        outputQueue.Enqueue(opToken);
                        currentToken = operatorStack.Peek();
                    }

                    if (currentToken.Equals("("))
                        operatorStack.Pop();
                }
            }

            // Push the rest tokens in the stack to the queue
            while (operatorStack.Count != 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }
            List<string> postfixExp = new List<string>();
            while (outputQueue.Any())
            {
                var val = outputQueue.Dequeue();

                postfixExp.Add(val);
            }

            return postfixExp;
        }
    }
}
