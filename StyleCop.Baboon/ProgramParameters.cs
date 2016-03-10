namespace StyleCop.Baboon
{
    using System;
    using System.Collections.Generic;

    public class ProgramParameters
    {
        private ProgramParameters()
        {
        }

        public string Settings 
        { 
            get; private set; 
        }

        public string ProjectPath 
        { 
            get; private set; 
        }

        public string[] IgnoredPaths 
        {
            get; private set; 
        }

        public string CheckStyleOutputFile 
        { 
            get; private set; 
        }

        public static ProgramParameters ExtractParameters(string[] args)
        {
            var parameters = new ProgramParameters();

            bool legacyProperties = true;
            foreach (var s in args) 
            {
                if (s.Contains("=")) 
                {
                    legacyProperties = false;
                    break;
                }
            }

            if (legacyProperties)
            {
                ParseLegacyParameters(args, parameters);
            } 
            else 
            {
                ParseParameters(args, parameters);
            }

            return parameters;
        }

        private static void ParseLegacyParameters(string[] args, ProgramParameters parameters)
        {
            if (args.Length < 3)
            {
                throw new Exception("Legacy argument count too small");
            }

            parameters.Settings = args[0];
            parameters.ProjectPath = args[1];
            int ignoredPathsLength = args.Length - 2;
            var ignoredPaths = new string[ignoredPathsLength];
            Array.Copy(args, 2, ignoredPaths, 0, ignoredPathsLength);
            parameters.IgnoredPaths = ignoredPaths;
        }

        private static void ParseParameters(string[] args, ProgramParameters parameters)
        {
            foreach (string s in args)
            {
                var argSplit = s.Split(new char[] { '=' });

                if (argSplit.Length != 2 || string.IsNullOrWhiteSpace(argSplit[1])) 
                {
                    throw new Exception("Value for '" + argSplit[0] + "' not set");
                }

                switch (argSplit[0].ToLower())
                { 
                    case "--settings-path":
                        parameters.Settings = argSplit[1];
                        break;
                    case "--analyze-path":
                        parameters.ProjectPath = argSplit[1];
                        break;
                    case "--checkstyle-output-path":
                        parameters.CheckStyleOutputFile = argSplit[1];
                        break;
                    case "--ignored-path":
                        parameters.IgnoredPaths = argSplit[1].Split(new char[] { ';' });
                        break;
                    default:
                        throw new Exception("Unrecognized argument : " + argSplit[0]);
                }
            }
        }
    }
}