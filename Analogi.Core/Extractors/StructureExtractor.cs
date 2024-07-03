using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Analogi.Core.Extractors
{
    public partial class StructureExtractor : IExtractor
    {
        private readonly Regex RoutineRegex = RoutineDeclarationRegex();
        private readonly Regex IncludeRegex = IncludePatternRegex();

        public void Run(ref PipelineData pd, string id, CodeFile data)
        {
            string file = data.ReadAll();
            int headerLen = 0, subroutineLen = 0;

            // Extract include statements
            MatchCollection includeMatches = IncludeRegex.Matches(file);
            List<string> includeFiles = [];
            foreach (Match match in includeMatches)
            {
                includeFiles.Add(match.Groups["file"].Value);
            }

            // Add include files to metadata
            pd.AddMetadata(Name, id + ".includes", includeFiles);

            // Extract routine declarations
            MatchCollection routineMatches = RoutineRegex.Matches(file);
            foreach (Match match in routineMatches)
            {
                int matchIndex = match.Index;
                string matchId = match.Groups["id"].Value;

                if (matchId.Equals("main", StringComparison.OrdinalIgnoreCase))
                {
                    headerLen = matchIndex;
                    continue;
                }

                if (headerLen == 0)
                {
                    headerLen = matchIndex;
                    continue;
                }

                if (headerLen > matchIndex)
                {
                    subroutineLen = headerLen;
                    headerLen = matchIndex;
                }
                else
                {
                    subroutineLen = matchIndex;
                }
            }

            if (headerLen > subroutineLen)
            {
                (subroutineLen, headerLen) = (headerLen, subroutineLen);
            }

            subroutineLen -= headerLen;

            pd.AddMetadata(Name, id + ".header", ProcessSection(file[..headerLen]));
            pd.AddMetadata(Name, id + ".main", ProcessSection(file.Substring(headerLen, subroutineLen)));
            pd.AddMetadata(Name, id + ".subroutine", ProcessSection(file[(headerLen + subroutineLen)..]));
        }

        public static List<string> ProcessSection(string section)
        {
            return new(section.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        }

        public string Name => "structure";

        private static readonly char[] separator = ['\n', '\r'];

        [GeneratedRegex(@"#\s*include\s*<(?<file>[a-zA-Z0-9_]+)>", RegexOptions.Compiled)]
        private static partial Regex IncludePatternRegex();

        [GeneratedRegex(@"(?<type>int|void|bool|float|double|string)\s+(?<id>[A-Za-z][A-Za-z0-9_]+)\((?<args>[^)]*)\)\s*\{?", RegexOptions.Compiled)]
        private static partial Regex RoutineDeclarationRegex();
    }
}
