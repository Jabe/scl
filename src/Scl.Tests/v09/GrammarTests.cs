using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Scl.Grammar;

namespace Scl.Tests.v09
{
    [TestFixture]
    public class GrammarTests
    {
        public const string File = @"..\..\..\..\spec\scl-v0.9.scl";

        [Test]
        public void CanParseSpec()
        {
            var scanner = new Scanner(File);
            var parser = new Parser(scanner);
            parser.Parse();

            Assume.That(parser.errors.count, Is.EqualTo(0));
        }
    }
}