namespace StyleCop.Baboon.Tests.TestHelper
{
    using System;
    using StyleCop.Baboon.Analyzer;

    public class ViolationSource
    {
        public const string ViolationFileName = "Test.cs";
        public const string ViolationFileName2 = "Test2.cs";
        public const string FirstViolationMessage = "You can't do this.";
        public static readonly Violation FirstViolation = new Violation("SA666", FirstViolationMessage, 666, "my.namespace1", "myerror1", false);
        public static readonly Violation SecondViolation = new Violation("SA777", "You should.", 777, "my.namespace2", "myerror2", true);
    }
}
