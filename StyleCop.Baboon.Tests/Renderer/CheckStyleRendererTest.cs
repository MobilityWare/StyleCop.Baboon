namespace StyleCop.Baboon.Tests
{
    using System;
    using System.Text.RegularExpressions;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using Moq;
    using NUnit.Framework;
    using StyleCop.Baboon.Analyzer;
    using StyleCop.Baboon.Renderer;
    using StyleCop.Baboon.Tests.TestHelper;

    [TestFixture]
    public class CheckStyleRendererTest
    {
        [Test]
        public void XmlValidationWithXsd()
        {
            var renderer = new CheckStyleRenderer(new Mock<IOutputWriter>().Object);
            XmlDocument docToTest = renderer.RenderViolationListAsXmlDocument(WithMultipleFiles());
                     
            XmlReader xmlReader = createXmlReader(docToTest);

            bool isValidXml = false;
            string validationError = null;
            try
            {
                // validate        
                using (xmlReader)
                {
                    while (xmlReader.Read())
                    {}
                }
                isValidXml = true;
            }
            catch (Exception ex)
            {
                validationError = ex.Message;
                isValidXml = false;
            }

            Assert.True(isValidXml, validationError);
        }

        private static XmlReader createXmlReader(XmlDocument xmlDocument)
        {
            StringReader xmlStringReader = convertXmlDocumentToStringReader(xmlDocument);
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings 
            { 
                ValidationType = ValidationType.Schema 
            };
            xmlReaderSettings.Schemas.Add("", "Renderer/Checkstyle.xsd");
            return XmlReader.Create(xmlStringReader, xmlReaderSettings);
        }

        private static StringReader convertXmlDocumentToStringReader(XmlDocument xmlDocument)
        {
            StringWriter sw = new StringWriter();
            xmlDocument.WriteTo(new XmlTextWriter(sw));
            return new StringReader(sw.ToString());
        }

        [Test]
        public void RenderValidationListWritesOuterXml()
        {
            var outputWriter = new Mock<IOutputWriter>();

            var renderer = new CheckStyleRenderer(outputWriter.Object);

            string expectedOuterXmlString = renderer.RenderViolationListAsXmlDocument(WithOneViolation()).OuterXml;

            renderer.RenderViolationList(WithOneViolation());

            outputWriter.Verify(o => o.WriteLine(expectedOuterXmlString), Times.Once);
        }

        //These tests don't need to be exact matches, since the xsd already verified format
        [Test]
        public void TestRenderWithNoViolations()
        {
            var renderer = new CheckStyleRenderer(new Mock<IOutputWriter>().Object);
            XmlDocument docToTest = renderer.RenderViolationListAsXmlDocument(WithNoViolation());

            string result = docToTest.OuterXml;

            Assert.IsTrue(result.Contains("checkstyle"));
            Assert.IsFalse(result.Contains("file"));
            Assert.IsFalse(result.Contains("error"));
        }

        [Test]
        public void TestRenderWithTwoViolations()
        {
            var renderer = new CheckStyleRenderer(new Mock<IOutputWriter>().Object);
            XmlDocument docToTest = renderer.RenderViolationListAsXmlDocument(WithTwoViolations());

            string result = docToTest.OuterXml;

            Assert.IsTrue(result.Contains("checkstyle"));
            Assert.AreEqual(1, getCountOfWord("<file", result));
            Assert.AreEqual(1, getCountOfWord(string.Format("name=\"{0}\"", ViolationSource.ViolationFileName), result));

            Assert.AreEqual(2, getCountOfWord("<error", result));
            Assert.AreEqual(1, getCountOfWord(string.Format("line=\"{0}\"", ViolationSource.FirstViolation.LineNumber), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("severity=\"{0}\"", getSeverityAsString(ViolationSource.FirstViolation.ViolationIsWarning)), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("message=\"{0}\"", ViolationSource.FirstViolation.Message), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("source=\"{0}\"", ViolationSource.FirstViolation.ViolationSource), result));

            Assert.AreEqual(1, getCountOfWord(string.Format("line=\"{0}\"", ViolationSource.SecondViolation.LineNumber), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("severity=\"{0}\"", getSeverityAsString(ViolationSource.SecondViolation.ViolationIsWarning)), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("message=\"{0}\"", ViolationSource.SecondViolation.Message), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("source=\"{0}\"", ViolationSource.SecondViolation.ViolationSource), result));
        }

        [Test]
        public void TestRenderWithWithMultipleFiles()
        {
            var renderer = new CheckStyleRenderer(new Mock<IOutputWriter>().Object);
            XmlDocument docToTest = renderer.RenderViolationListAsXmlDocument(WithMultipleFiles());

            string result = docToTest.OuterXml;

            Assert.IsTrue(result.Contains("checkstyle"));
            Assert.AreEqual(2, getCountOfWord("<file", result));
            Assert.AreEqual(1, getCountOfWord(string.Format("name=\"{0}\"", ViolationSource.ViolationFileName), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("name=\"{0}\"", ViolationSource.ViolationFileName2), result));

            Assert.AreEqual(2, getCountOfWord("<error", result));
            Assert.AreEqual(1, getCountOfWord(string.Format("line=\"{0}\"", ViolationSource.FirstViolation.LineNumber), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("severity=\"{0}\"", getSeverityAsString(ViolationSource.FirstViolation.ViolationIsWarning)), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("message=\"{0}\"", ViolationSource.FirstViolation.Message), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("source=\"{0}\"", ViolationSource.FirstViolation.ViolationSource), result));

            Assert.AreEqual(1, getCountOfWord(string.Format("line=\"{0}\"", ViolationSource.SecondViolation.LineNumber), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("severity=\"{0}\"", getSeverityAsString(ViolationSource.SecondViolation.ViolationIsWarning)), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("message=\"{0}\"", ViolationSource.SecondViolation.Message), result));
            Assert.AreEqual(1, getCountOfWord(string.Format("source=\"{0}\"", ViolationSource.SecondViolation.ViolationSource), result));
        }

        private static ViolationList WithOneViolation()
        {
            var violationList = new ViolationList();
            violationList.AddViolationToFile(ViolationSource.ViolationFileName, ViolationSource.FirstViolation);

            return violationList;
        }

        private static ViolationList WithTwoViolations()
        {
            var violationList = new ViolationList();
            violationList.AddViolationToFile(ViolationSource.ViolationFileName, ViolationSource.FirstViolation);
            violationList.AddViolationToFile(ViolationSource.ViolationFileName, ViolationSource.SecondViolation);

            return violationList;
        }

        private static ViolationList WithNoViolation()
        {
            return new ViolationList();
        }

        private static ViolationList WithMultipleFiles()
        {
            var violationList = new ViolationList();
            violationList.AddViolationToFile(ViolationSource.ViolationFileName, ViolationSource.FirstViolation);
            violationList.AddViolationToFile(ViolationSource.ViolationFileName2, ViolationSource.SecondViolation);

            return violationList;
        }

        private static int getCountOfWord(string wordToFind, string stringToFindIn)
        {
            return new Regex(Regex.Escape(wordToFind)).Matches(stringToFindIn).Count;
        }

        private static string getSeverityAsString(bool warning)
        {
            if (warning) {
                return "warning";
            } else{
                return "error";
            }
        }
    }
}
