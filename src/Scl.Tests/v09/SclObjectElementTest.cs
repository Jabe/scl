﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Scl.Tests.v09
{
    [TestFixture]
    public class SclObjectElementTest
    {
        [Test]
        public void Test1()
        {
            SclObject obj = SclObject.Parse(GrammarTests.File);

            // Root.Element("Multiple")  -> ["b0", "b1", "b2"];

            var result = obj.Element("Multiple").ToArray();

            Assume.That(result.Count(), Is.EqualTo(3));

            Assume.That(result, Is.EqualTo(new[] {"b0", "b1", "b2"}));
        }
    }
}