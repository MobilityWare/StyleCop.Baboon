namespace StyleCop.Baboon.Tests.Analyzer
{
    using System;
    using NUnit.Framework;
    using StyleCop.Baboon.Analyzer;

    [TestFixture]
    public class ViolationTest
    {
        [Test]
        public void ToStringReturnsViolationAsLine()
        {
            var expectedResult = "Line 666: [SA666] You cannot do this.";

            var violation = new Violation("SA666", "You cannot do this.", 666, "my.error.namespace", "MyError", false);

            Assert.AreEqual(expectedResult, violation.ToString());
        }

        [Test]
        public void ViolationSourceReturnsNamespaceAndName()
        {
            var expectedResult = "my.error.namespace.MyError";

            var violation = new Violation("SA666", "You cannot do this.", 666, "my.error.namespace", "MyError", false);

            Assert.AreEqual(expectedResult, violation.ViolationSource);
        }

        [Test]
        public void ViolationSourceDoesNotThrowExceptionOnNullOrEmpty()
        {
            var expectedResult = ".";

            var violation = new Violation("SA666", "You cannot do this.", 666, "", null, false);

            Assert.AreEqual(expectedResult, violation.ViolationSource);
        }            
    }
}
