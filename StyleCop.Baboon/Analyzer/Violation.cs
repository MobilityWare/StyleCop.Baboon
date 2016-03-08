namespace StyleCop.Baboon.Analyzer
{
    using System;

    public class Violation
    {
        private readonly string id;
        private readonly string message;
        private readonly int lineNumber;
        private readonly string violationNamespace;
        private readonly string violationName;
        private readonly bool violationIsWarning;

        public Violation(string id, string message, int lineNumber, string violationNamespace, string violationName, bool violationIsWarning)
        {
            this.id = id;
            this.message = message;
            this.lineNumber = lineNumber;
            this.violationName = violationName;
            this.violationNamespace = violationNamespace;
            this.violationIsWarning = violationIsWarning;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        public string ViolationName 
        {
            get 
            {
                return this.violationName;
            }
        }

        public string ViolationNamespace 
        {
            get 
            {
                return this.violationNamespace;
            }
        }

        public bool ViolationIsWarning 
        {
            get 
            {
                return this.violationIsWarning;
            }
        }

        public string ViolationSource
        {
            get
            {
                return string.Format("{0}.{1}", this.ViolationNamespace, this.ViolationName);
            }
        }

        public override string ToString()
        {
            return string.Format(string.Format("Line {0}: [{1}] {2}", this.LineNumber, this.Id, this.Message));
        }
    }
}
