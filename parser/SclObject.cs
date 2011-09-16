using System;
using System.IO;

namespace Scl
{
    public class SclObject
    {
        public static SclObject Parse(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                return Parse(stream);
            }
        }

        public static SclObject Parse(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Element(string key)
        {
            throw new NotImplementedException();
        }

        public void Elements(string key)
        {
            throw new NotImplementedException();
        }

        public void Value(string key)
        {
            throw new NotImplementedException();
        }

        public SclObject Context(string key)
        {
            throw new NotImplementedException();
        }
    }
}