using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Scl.Tests.v09
{
    [TestFixture]
    public class SclObjectElementsTest
    {
        [Test]
        public void Test1()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Elements("Multiple") -> [["a0", "a1", "a2"], ["b0", "b1", "b2"]];

            var result = obj.Elements("Multiple").ToArray();

            Assume.That(result.Count(), Is.EqualTo(2));

            var first = result[0].ToArray();
            var second = result[1].ToArray();

            Assume.That(first.Count(), Is.EqualTo(3));
            Assume.That(second.Count(), Is.EqualTo(3));

            Assume.That(first, Is.EqualTo(new[] {"a0", "a1", "a2"}));
            Assume.That(second, Is.EqualTo(new[] {"b0", "b1", "b2"}));
        }

        [Test]
        public void Test2()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Elements("FooContext")  -> [["foo", "fo'o2", "foo3"]];

            var result = obj.Elements("FooContext").ToArray();

            Assume.That(result.Count(), Is.EqualTo(1));

            var first = result[0].ToArray();

            Assume.That(first.Count(), Is.EqualTo(3));

            Assume.That(first, Is.EqualTo(new[] {"foo", "fo'o2", "foo3"}));
        }
    }
}