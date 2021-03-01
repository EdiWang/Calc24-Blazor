using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calc24Blazor.Pages
{
    public partial class Index
    {
        public int Num1 { get; set; } = 1;
        public int Num2 { get; set; } = 2;
        public int Num3 { get; set; } = 3;
        public int Num4 { get; set; } = 4;
        public bool Loading { get; set; }

        public List<string> ResultList { get; set; } = new();

        private async Task GetResults()
        {
            ResultList.Clear();

            Loading = true;
            
            var list = await Task.Run(DoCalc);
            ResultList = list;

            Loading = false;
        }

        private List<string> DoCalc()
        {
            var tempList = new List<string>();

            var numbers = new List<double> { Num1, Num2, Num3, Num4 };

            var operators = new List<Func<Expression, Expression, BinaryExpression>>
            {
                Expression.Add,
                Expression.Subtract,
                Expression.Multiply,
                Expression.Divide
            };

            foreach (var operatorCombination in OperatorPermute(operators))
            {
                foreach (var node in AllBinaryTrees(3))
                {
                    foreach (var permuteOfNumbers in FullPermute(numbers))
                    {
                        var expression = Build(node, permuteOfNumbers, operatorCombination.ToList());
                        var compiled = Expression.Lambda<Func<double>>(expression).Compile();
                        try
                        {
                            var value = compiled();
                            if (Math.Abs(value - 24) < 0.01)
                            {
                                tempList.Add($"{expression} = {value}");
                            }
                        }
                        catch (DivideByZeroException)
                        {
                        }
                    }
                }
            }

            return tempList;
        }

        private static IEnumerable<IEnumerable<Func<Expression, Expression, BinaryExpression>>> OperatorPermute(IReadOnlyCollection<Func<Expression, Expression, BinaryExpression>> operators)
        {
            return from operator1 in operators
                   from operator2 in operators
                   from operator3 in operators
                   select new[] { operator1, operator2, operator3 };
        }

        private static IEnumerable<List<T>> FullPermute<T>(List<T> elements)
        {
            if (elements.Count == 1)
                return EnumerableOfOneElement(elements);

            IEnumerable<List<T>> result = null;
            foreach (var first in elements)
            {
                var remaining = elements.ToArray().ToList();
                remaining.Remove(first);
                var fullPermuteOfRemaining = FullPermute(remaining);

                foreach (var permute in fullPermuteOfRemaining)
                {
                    var arr = new List<T> { first };
                    arr.AddRange(permute);

                    var seq = EnumerableOfOneElement(arr);
                    result = result?.Union(seq) ?? seq;
                }
            }
            return result;
        }

        private static IEnumerable<T> EnumerableOfOneElement<T>(T element)
        {
            yield return element;
        }

        private static Expression Build(Node node, IReadOnlyList<double> numbers, IReadOnlyList<Func<Expression, Expression, BinaryExpression>> operators)
        {
            var iNum = 0;
            var iOprt = 0;

            Expression Func(Node n)
            {
                Expression exp;
                if (n == null)
                {
                    exp = Expression.Constant(numbers[iNum++]);
                }
                else
                {
                    var left = Func(n.Left);
                    var right = Func(n.Right);
                    exp = operators[iOprt++](left, right);
                }

                return exp;
            }

            return Func(node);
        }

        private static IEnumerable<Node> AllBinaryTrees(int size)
        {
            if (size == 0)
            {
                return new Node[] { null };
            }

            return from i in Enumerable.Range(0, size)
                   from left in AllBinaryTrees(i)
                   from right in AllBinaryTrees(size - 1 - i)
                   select new Node(left, right);
        }
    }

    internal class Node
    {
        public Node Left { get; }
        public Node Right { get; }
        public Node(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
    }
}
