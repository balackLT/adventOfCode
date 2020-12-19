using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using Sprache;
using Input = AdventOfCode.Executor.Input;

namespace AdventOfCode.Solutions2020.Day18
{
    public class Solution : ISolution
    {
        public int Day { get; } = 18;

        private static readonly Parser<long> Number = Parse.Number.Token().Select(long.Parse);
        private static readonly Parser<char> Operators = Parse.Chars('*', '+').Token();
        private static readonly Parser<long> NumberOrParentheses = Number.Or(Parse.Ref(() => Expr.Contained(Parse.Char('('), Parse.Char(')'))));
        private static readonly Parser<long> Expr = Parse.ChainOperator(Operators, NumberOrParentheses, (op, left, right) =>
        {
            return op switch
            {
                '*' => left * right,
                '+' => left + right
            };
        });
        
        private static readonly Parser<long> WeirdExpression = Number.Or(Parse.Ref(() => Multiply.Contained(Parse.Char('('), Parse.Char(')'))));

        private static readonly Parser<long> Add = 
            Parse.ChainOperator(Parse.Char('+').Token(), WeirdExpression, (op, left, right) => left + right);
        
        private static readonly Parser<long> Multiply = 
            Parse.ChainOperator(Parse.Char('*').Token(), Add, (op, left, right) => left * right);
        

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();
            
            Debug.Assert(Expr.Parse("1 + 2 + 3") == 6);
            Debug.Assert(Expr.Parse("1 + 2 * 3") == 9);
            Debug.Assert(Expr.Parse("1 + (2 * 3)") == 7);

            var result = lines.Sum(l => Expr.Parse(l));

            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();
            
            Debug.Assert(Multiply.Parse("1 + 2 * 3") == 9);
            Debug.Assert(Multiply.Parse("1 * 2 + 3") == 5);
            Debug.Assert(Multiply.Parse("1 + (2 * 3)") == 7);

            var result = lines.Sum(l => Multiply.Parse(l));

            return result.ToString();
        }
    }
}