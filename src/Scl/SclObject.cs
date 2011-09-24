using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scl.Grammar;

namespace Scl
{
    public class SclObject
    {
        private readonly Parser.SclElement _root;

        private SclObject(Parser.SclElement root)
        {
            _root = root;
        }

        public static SclObject Parse(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                return Parse(stream);
            }
        }

        public static SclObject Parse(Stream stream)
        {
            var scanner = new Scanner(@"..\..\..\..\spec\scl-v0.9.scl");
            var parser = new Parser(scanner);
            parser.Parse();

            return new SclObject(parser.root);
        }

        public IEnumerable<string> Element(string key)
        {
            foreach (Parser.SclElement el in _root.Children)
            {
                if (el.Name == key)
                {
                    foreach (Parser.SclVal val in el.Vals)
                    {
                        yield return val.Val;
                    }

                    break;
                }
            }
        }

        public IEnumerable<IEnumerable<string>> Elements(string key)
        {
            foreach (Parser.SclElement el in _root.Children)
            {
                if (el.Name == key)
                {
                    yield return el.Vals.Select(x => x.ToString());
                }
            }
        }

        public string Value(string key)
        {
            foreach (Parser.SclElement el in _root.Children)
            {
                if (el.Name == key)
                {
                    foreach (Parser.SclVal val in el.Vals)
                    {
                        return val.Val;
                    }

                    return "";
                }
            }

            return null;
        }

        public SclObject Context(string key)
        {
            throw new NotImplementedException();
        }
    }
}