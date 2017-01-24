using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Bluehands.Versioning
{
    public class ReadVersionFile : Task
    {
        [Required]
        public string[] Folders { get; set; }

        [Required]
        public string[] Filenames { get; set; }

        [Required]
        public int SegmentCount { get; set; }

        [Output]
        public string VersionPrefix { get; private set; }

        public override bool Execute()
        {
            if (Folders.Length < 1 || Filenames.Length < 1)
            {
                Log.LogError($"Please specify a location for the version file.");
                return false;
            }

            var path = FindVersionFile();
            if (path == null)
            {
                Log.LogError($"No version file could be found. Create a file named \"{string.Join("\" or \"", Filenames)}\" in one of the following directories: {Environment.NewLine}{string.Join(Environment.NewLine, Folders)})");
                return false;
            }

            try
            {
                var lines = File.ReadAllLines(path);
                if (lines.Length < 1)
                {
                    Log.LogError($"The version file at \"{path}\" is empty.");
                    return false;
                }

                VersionPrefix = GetVersion(lines);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError($"Could not read version information from \"{path}\" due to the following error: {ex.ToString()}");
                return false;
            }
        }

        string FindVersionFile()
        {
            // filenames are evaluated first so that if a filename with lower precedence (e.g. Version.txt) is used for another purpose, a filename with higher precedence (e.g. AssemblyVersionInfoTemplate.txt) can override it even if it's in a folder with lower precedence
            foreach (var filename in Filenames)
            {
                foreach (var folder in Folders)
                {
                    var path = Path.Combine(folder, filename);
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            return null;
        }

        string GetVersion(string[] lines)
        {
            var result = lines[0];
            var segmentCount = result.Count(c => c == '.');
            // make sure enough segments exist, but no validation is performed
            for (int i = 0; i < SegmentCount - 1 - segmentCount; i++)
            {
                result += ".0";
            }
            return result;
        }
    }
}
