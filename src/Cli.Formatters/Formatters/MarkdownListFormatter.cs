// <copyright file="MarkdownListFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotNetLicensesReporter.Collectors;
using Lsquared.DotNetLicensesReporter.Formatters;

using Microsoft.Extensions.Logging;

namespace Lsquared.DotNetLicensesReporter;

internal sealed partial class MarkdownListFormatter(ILogger<MarkdownListFormatter> logger) : IFileOutputFormatter
{
    public string Name => "markdown-list";

    public FileInfo CreateFile(DirectoryInfo directory)
    {
        directory.Create();
        return new(Path.Combine(directory.FullName, "licenses.md"));
    }

    public async Task Write(Stream stream, IReadOnlyList<PackageLicense> packages)
    {
        using StreamWriter writer = new(stream);
        await writer.WriteLineAsync("Packages:");
        foreach (var package in packages)
        {
            await writer.WriteAsync("- ");
            if (package.ProjectUrl is not null)
            {
                await writer.WriteAsync('[');
                await writer.WriteAsync(package.PackageId);
                await writer.WriteAsync("](");
                await writer.WriteAsync(package.ProjectUrl.ToString());
                await writer.WriteAsync(')');
            }
            else
            {
                await writer.WriteAsync(package.PackageId);
            }

            await writer.WriteAsync(" v");
            await writer.WriteAsync(package.PackageVersion.ToString());
            await writer.WriteAsync(", ");

            if (package.LicenseUrl is not null)
            {
                await writer.WriteAsync('[');
                await writer.WriteAsync(package.License);
                await writer.WriteAsync("](");
                await writer.WriteAsync(package.LicenseUrl.ToString());
                await writer.WriteAsync(')');
            }
            else
            {
                await writer.WriteAsync(package.License);
            }

            await writer.WriteLineAsync();
        }
    }
}
