namespace StyleCop.Baboon.Renderer
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using StyleCop.Baboon.Analyzer;

    public class CheckStyleRenderer : IRenderer
    {
        private readonly IOutputWriter outputWriter;

        public CheckStyleRenderer(IOutputWriter outputWriter)
        {
            this.outputWriter = outputWriter;
        }

        public void RenderViolationList(ViolationList violationList)
        {
            XmlDocument doc = this.RenderViolationListAsXmlDocument(violationList);

            this.outputWriter.WriteLine(doc.OuterXml);
        }

        public XmlDocument RenderViolationListAsXmlDocument(ViolationList violationList)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement checkstyleRoot = doc.CreateElement("checkstyle");
            checkstyleRoot.SetAttribute("version", "5.0");
            doc.AppendChild(checkstyleRoot);

            foreach (var violation in violationList.Violations)
            {
                var fileName = violation.Key;

                var fileElem = doc.CreateElement("file");
                fileElem.SetAttribute("name", fileName);
                checkstyleRoot.AppendChild(fileElem);

                this.RenderViolationsPerFile(doc, fileElem, violation.Value);
            }

            return doc;
        }

        private void RenderViolationsPerFile(XmlDocument doc, XmlElement fileElem, IList<Violation> violations)
        {
            foreach (var v in violations)
            {
                var errorElem = doc.CreateElement("error");

                this.RenderViolation(errorElem, v);

                fileElem.AppendChild(errorElem);
            }
        }

        private void RenderViolation(XmlElement errorElem, Violation violation)
        {
            errorElem.SetAttribute("line", violation.LineNumber.ToString());
            errorElem.SetAttribute("message", violation.Message);
            if (violation.ViolationIsWarning) 
            {
                errorElem.SetAttribute("severity", "warning");
            } 
            else 
            {
                errorElem.SetAttribute("severity", "error");
            }

            errorElem.SetAttribute("source", violation.ViolationSource);
        }
    }
}
