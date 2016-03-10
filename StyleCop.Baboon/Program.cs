namespace StyleCop.Baboon
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using StyleCop.Baboon.Analyzer;
    using StyleCop.Baboon.Analyzer.StyleCop;
    using StyleCop.Baboon.Infrastructure;
    using StyleCop.Baboon.Renderer;

    public class MainClass
    {
        private const int NoViolationsFound = 0;
        private const int MissingArgumentsErrorCode = 1;
        private const int SettingsFileDoesNotExistErrorCode = 2;
        private const int InvalidPathToAnalyzeErrorCode = 3;
        private const int ViolationsFound = 4;

        public static int Main(string[] args)
        {
            ProgramParameters parameters;

            try
            {
                parameters = ProgramParameters.ExtractParameters(args);
            }
            catch (Exception) 
            {
                PrintUsage();
                return MissingArgumentsErrorCode;
            }

            return Analyze(parameters);
        }

        private static int Analyze(ProgramParameters parameters)
        {
            var fileSystemHandler = new FileSystemHandler();
            var outputWriter = new StandardOutputWriter();

            if (false == fileSystemHandler.Exists(parameters.Settings))
            {
                outputWriter.WriteLineWithSeparator("Given settings file does not exist. Exiting...", string.Empty);

                return SettingsFileDoesNotExistErrorCode;
            }

            if (false == fileSystemHandler.Exists(parameters.ProjectPath))
            {
                outputWriter.WriteLineWithSeparator("Given path to analyze does not exist. Exiting...", string.Empty);

                return InvalidPathToAnalyzeErrorCode;
            }

            var analyzer = new StyleCopAnalyzer();
            var projectFactory = new ProjectFactory(new FileSystemHandler());
            var project = projectFactory.CreateFromPathWithCustomSettings(parameters.ProjectPath, parameters.Settings, parameters.IgnoredPaths);
            var violations = analyzer.GetViolationsFromProject(project);

            var consoleRenderer = new ConsoleRenderer(outputWriter);
            consoleRenderer.RenderViolationList(violations);

            if (!string.IsNullOrWhiteSpace(parameters.CheckStyleOutputFile)) 
            {
                using (TextWriter textWriter = File.CreateText(parameters.CheckStyleOutputFile)) 
                {
                    IOutputWriter fileOutputWriter = new FileOutputWriter(textWriter);
                    var checkStyleRenderer = new CheckStyleRenderer(fileOutputWriter);
                    checkStyleRenderer.RenderViolationList(violations);
                }
            }

            if (violations.Empty)
            {
                return NoViolationsFound;
            }

            return ViolationsFound;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: StyleCop.Baboon.exe <stylecop-settings-path> <path-to-analyze> <ignored-path> [<optional-ignored-path> <optional-ignored-path>...]\r\n" +
                "OR\r\n" +
                "StyelCop.Baboon.exe --settings-path=<stylecop-settings-path> --analyze-path=<path-to-analyze> [--checkstyle-output-path=<optional-checktyle-output-path>] [--ignored-path=<semicolon-seperated-paths>]");
        }
    }
}
