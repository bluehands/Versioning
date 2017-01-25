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

        public bool FailOnError { get; set; } = true;

        public string ErrorMessage { get; set; } = $"The \"{nameof(RemoveLines)}\" task failed unexpectedly.";

        public override bool Execute()
        {
            try
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
            }
            catch (Exception ex)
            {
                var message = $"{ErrorMessage} More information:{Environment.NewLine}Files:{Environment.NewLine}{string.Join(Environment.NewLine, Files)}{Environment.NewLine}Patterns:{Environment.NewLine}{string.Join(Environment.NewLine, Patterns)}{Environment.NewLine}Exception:{Environment.NewLine}{ex}";
                if (FailOnError)
                {
                    Log.LogError(message);
                    return false;
                }
                Log.LogWarning(message);
            }
            return true;
        }
    }
}
