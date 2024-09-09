// <copyright file="OutputFormatterExtensions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using System.Diagnostics;
using System.Runtime.Serialization;

using Lsquared.DotnetLicensesReporter.Collectors;

namespace Lsquared.DotnetLicensesReporter.Formatters;

internal static class OutputFormatterExtensions
{
    public static async Task Render(this List<IOutputFormatter> formatters, List<PackageLicense> packages, OutputOptions options)
    {
        if (formatters.Count is 0)
            return;

        if (!options.Silent)
        {
            var standardOutput = System.Console.OpenStandardOutput();
            if (formatters.Count is 1 && options.TemplateFile is null)
            {
                await formatters[0].Write(standardOutput, packages);
            }
            else
            {
                var consoleOutputFormatter = formatters.Find(o => o is not IFileOutputFormatter) ?? formatters.First();
                await consoleOutputFormatter.Write(standardOutput, packages);
            }
        }

        options.Directory?.Create();
        var writtenFiles = formatters
            .OfType<IFileOutputFormatter>()
            .Select(async (formatter) =>
            {
                var file = formatter.CreateFile(options.Directory ?? new(Environment.CurrentDirectory));
                using Stream stream = file.Open(FileMode.Create);
                await formatter.Write(stream, packages);
                return file;
            })
            .ToList();

        _ = await Task.WhenAll(writtenFiles);
        if (options.OpenFile && writtenFiles.Count is 1)
            Process.Start(new ProcessStartInfo(writtenFiles[0].Result.FullName) { UseShellExecute = true });
    }
}
