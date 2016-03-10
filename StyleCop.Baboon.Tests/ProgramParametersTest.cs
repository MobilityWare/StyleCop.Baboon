namespace StyleCop.Baboon.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class ProgramParametersTest
    {
        const string SETTING_SWITCH_OPTION = "--settings-path";
        const string ANALYZE_PATH_OPTION = "--analyze-path";
        const string IGNORED_PATH_OPTION = "--ignored-path";
        const string CHECKSTYLE_OUTPUT_PATH_OPTION = "--checkstyle-output-path";

        const string EXPECTED_SETTINGS_VALUE = "SettingPath";
        const string EXPECTED_ANALYZE_VALUE = "AnalyzePath";
        const string EXPECTED_IGNORED_PATH_1_VALUE = "IgnoredPath1";
        const string EXPECTED_IGNORED_PATH_2_VALUE = "IgnoredPath2";
        const string EXPECTED_CHECKSTYLE_PATH_VALUE = "CheckStylePath";

        [Test]
        public void LegacyCommandLineParsedCorrectly()
        {
            List<string> args = new List<string>();
            args.Add(EXPECTED_SETTINGS_VALUE);
            args.Add(EXPECTED_ANALYZE_VALUE);
            args.Add(EXPECTED_IGNORED_PATH_1_VALUE);
            args.Add(EXPECTED_IGNORED_PATH_2_VALUE);

            ProgramParameters result = ProgramParameters.ExtractParameters(args.ToArray());

            Assert.AreEqual(EXPECTED_SETTINGS_VALUE, result.Settings);
            Assert.AreEqual(EXPECTED_ANALYZE_VALUE, result.ProjectPath);
            Assert.AreEqual(2, result.IgnoredPaths.Length);
            Assert.AreEqual(EXPECTED_IGNORED_PATH_1_VALUE, result.IgnoredPaths[0]);
            Assert.AreEqual(EXPECTED_IGNORED_PATH_2_VALUE, result.IgnoredPaths[1]);
            Assert.AreEqual(null, result.CheckStyleOutputFile);
        }

        [Test]
        public void ThrowsExceptionWithWithTooFewLegacyArguments()
        {
            List<string> args = new List<string>();
            args.Add(EXPECTED_SETTINGS_VALUE);
            args.Add(EXPECTED_ANALYZE_VALUE);

            bool exceptionThrown = false;
            try
            {
                ProgramParameters.ExtractParameters(args.ToArray());
            }
            catch(Exception) {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
            

        [Test]
        public void ParsesParametersCorrectly()
        {
            List<string> args = new List<string>();
            args.Add(String.Format("{0}={1}", SETTING_SWITCH_OPTION, EXPECTED_SETTINGS_VALUE));
            args.Add(String.Format("{0}={1}", ANALYZE_PATH_OPTION, EXPECTED_ANALYZE_VALUE));
            args.Add(String.Format("{0}={1};{2}", IGNORED_PATH_OPTION, EXPECTED_IGNORED_PATH_1_VALUE, EXPECTED_IGNORED_PATH_2_VALUE));
            args.Add(String.Format("{0}={1}", CHECKSTYLE_OUTPUT_PATH_OPTION, EXPECTED_CHECKSTYLE_PATH_VALUE));

            ProgramParameters result = ProgramParameters.ExtractParameters(args.ToArray());

            Assert.AreEqual(EXPECTED_SETTINGS_VALUE, result.Settings);
            Assert.AreEqual(EXPECTED_ANALYZE_VALUE, result.ProjectPath);
            Assert.AreEqual(2, result.IgnoredPaths.Length);
            Assert.AreEqual(EXPECTED_IGNORED_PATH_1_VALUE, result.IgnoredPaths[0]);
            Assert.AreEqual(EXPECTED_IGNORED_PATH_2_VALUE, result.IgnoredPaths[1]);
            Assert.AreEqual(EXPECTED_CHECKSTYLE_PATH_VALUE , result.CheckStyleOutputFile);
        }

        [Test]
        public void ThrowExceptionOnOptionWithoutValue()
        {
            List<string> args = new List<string>();
            args.Add(String.Format("{0}=", SETTING_SWITCH_OPTION, EXPECTED_SETTINGS_VALUE));
            args.Add(String.Format("{0}={1}", ANALYZE_PATH_OPTION, EXPECTED_ANALYZE_VALUE));
            args.Add(String.Format("{0}={1};{2}", IGNORED_PATH_OPTION, EXPECTED_IGNORED_PATH_1_VALUE, EXPECTED_IGNORED_PATH_2_VALUE));
            args.Add(String.Format("{0}={1}", CHECKSTYLE_OUTPUT_PATH_OPTION, EXPECTED_CHECKSTYLE_PATH_VALUE));

            bool exceptionThrown = false;
            try{
            ProgramParameters.ExtractParameters(args.ToArray());
            }
            catch(Exception) {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void ThrowExceptionOnOptionWithoutEqualsSign()
        {
            List<string> args = new List<string>();
            args.Add(String.Format("{0}", SETTING_SWITCH_OPTION, EXPECTED_SETTINGS_VALUE));
            args.Add(String.Format("{0}={1}", ANALYZE_PATH_OPTION, EXPECTED_ANALYZE_VALUE));
            args.Add(String.Format("{0}={1};{2}", IGNORED_PATH_OPTION, EXPECTED_IGNORED_PATH_1_VALUE, EXPECTED_IGNORED_PATH_2_VALUE));
            args.Add(String.Format("{0}={1}", CHECKSTYLE_OUTPUT_PATH_OPTION, EXPECTED_CHECKSTYLE_PATH_VALUE));

            bool exceptionThrown = false;
            try{
                ProgramParameters.ExtractParameters(args.ToArray());
            }
            catch(Exception) {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
