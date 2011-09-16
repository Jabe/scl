using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scl.Grammar;

namespace Scl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var root = SclObject.Parse(@"..\..\..\scl-draft.scl");

            /*
             *  Root.Element("Multiple")  -> ["a0", "a1", "a2"];
             *  Root.Elements("Multiple") -> [["a0", "a1", "a2"], ["b0", "b1", "b2"]];
             *  Root.Value("Whitespace")  -> "spaces";
             *  Root.Value("EmptyVal")    -> ""; Root.Value("Blah") -> NULL;
             * 
             *  Root.Elements("Context")  -> [["foo", "foo2", "foo3"]];
             * 
             * 
             *  Root.Context("FooContext").Value("Ssl") -> "0";
             * 
             * 
             * 
             * */

            root.Element("Multiple");
            root.Elements("Multiple");
            root.Value("Whitespace");
            root.Value("EmptyVal");
            root.Value("Blah");
            root.Elements("Context");
            root.Context("FooContext").Value("Ssl");

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