namespace StyleCop.Baboon.Renderer
{
    using System;
    using System.IO;

    public class FileOutputWriter : IOutputWriter
    {
        private readonly TextWriter textWriter;

        public FileOutputWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void WriteLine(string content)
        {
            this.textWriter.WriteLine(content);
        }

        public void WriteColoredLine(string content, ConsoleColor color)
        {
            this.textWriter.WriteLine(content);
        }

        public void WriteLineWithSeparator(string content, string separator)
        {
            this.textWriter.WriteLine(separator);
            this.textWriter.WriteLine(content);
        }
    }
}
