using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SclParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var scanner = new Scanner(@"..\..\..\scl-draft.scl");
            var parser = new Parser(scanner);
            parser.Parse();

            Console.WriteLine("Errors: {0}", parser.errors.count);

            Print(parser.root, 0);
        }

        public static void Print(Parser.SclElement el, int i)
        {
            string indent = "".PadLeft(i*2, ' ');

            Console.WriteLine(indent + el);

            foreach (Parser.SclElement cel in el.Children)
            {
                Print(cel, i + 1);
            }
        }
    }
}