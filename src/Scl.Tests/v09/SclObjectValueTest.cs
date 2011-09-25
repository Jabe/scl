using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Scl.Tests.v09
{
    [TestFixture]
    public class SclObjectValueTest
    {
        [Test]
        public void Test1()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Value("Whitespace")  -> "spaces";

            var result = obj.Value("Whitespace");

            Assume.That(result, Is.EqualTo("spaces"));
        }

        [Test]
        public void Test2()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Value("EmptyVal")    -> "";

            var result = obj.Value("EmptyVal");

            Assume.That(result, Is.EqualTo(""));
        }

        [Test]
        public void Test3()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Value("Blah") -> NULL;

            var result = obj.Value("Blah");

            Assume.That(result, Is.Null);
        }

        [Test]
        public void Test4()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Value("Multiple") -> "b0";

            var result = obj.Value("Multiple");

            Assume.That(result, Is.EqualTo("b0"));
        }
    }
}