﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Exceptions;
using TestTask.Operators;

namespace TestTask
{
    public class NotationCalculator
    {
        private List<IUnaryOperation> unaryOperations;
        private List<IBinaryOperation> binaryOperations;

        Stack<float> stack;

        public float Result { get; set; }

        public NotationCalculator()
        {
            unaryOperations = new List<IUnaryOperation> { new Sin(), new Cos(), new UnaryMinus(), new UnaryPlus() };
            binaryOperations = new List<IBinaryOperation> { new Addition(), new Difference(), new Division(), new Muliply(), new Pow() };
            stack = new Stack<float>();
        }

        public void Calculate(Stack<string> expression)
        {
            stack.Clear();

            try
            {
                while (expression.Count != 0)
                {
                    string token = expression.Pop();

                    if (TokenInitializer.IsNumber(token))
                    {
                        stack.Push(int.Parse(token));
                        continue;
                    }
                    else if (TokenInitializer.IsUnary(token))
                    {
                        foreach (var unary in unaryOperations.Where(op => op.Name == token))
                        {
                            stack.Push(unary.Calculate(stack.Pop()));
                        }
                    }
                    else
                    {
                        foreach (var binary in binaryOperations.Where(op => op.Name == token))
                        {
                            stack.Push(binary.Calculate(stack.Pop(), stack.Pop()));
                        }
                    }
                }
                if (stack.Count != 1)
                {
                    throw new NumericVariableSurplusException();
                }
                else
                {
                    Result = stack.Pop();
                }
            }
            catch (InvalidOperationException)
            {
                throw new OperatorsSurplusException();
            }

        }
    }
}
