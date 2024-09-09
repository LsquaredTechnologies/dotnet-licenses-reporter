// <copyright file="OutputOptions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Formatters;

public sealed class OutputOptions
{
    public required DirectoryInfo? Directory { get; init; }

    public required IReadOnlyList<string> Formats { get; init; }

    public bool Silent { get; init; }

    public FileInfo? TemplateFile { get; init; }

    public bool OpenFile { get; init; }
}
