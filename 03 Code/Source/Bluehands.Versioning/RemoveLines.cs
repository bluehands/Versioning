using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Bluehands.Versioning
{
    public class RemoveLines : Task
    {
        [Required]
        public string[] Files { get; set; }

        [Required]
        public string[] Patterns { get; set; }

        public override bool Execute()
        {
            var patterns = Patterns.Select(pattern => new Regex(pattern)).ToArray();
            foreach (var file in Files)
            {
                var lines = File.ReadAllLines(file);
                var remaining = lines.Where(line =>
                {
                    foreach (var pattern in patterns)
                    {
                        if (pattern.IsMatch(line))
                        {
                            Log.LogMessage($"Found line in file \"{file}\" that matched the pattern \"{pattern}\":{Environment.NewLine}{line}");
                            return false;
                        }
                    }
                    return true;
                }).ToArray();
                var removedCount = lines.Length - remaining.Length;
                if (removedCount > 0)
                {
                    File.WriteAllLines(file, remaining);
                    Log.LogMessage($"Removed {removedCount} lines from file \"{file}\".");
                }
            }
            return true;
        }
    }
}
