// <copyright file="CollectionOptions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotNetLicensesReporter.Collectors;

public sealed class CollectionOptions
{
    public required FileInfo ProjectFile { get; init; }

    public required bool ShouldRestore { get; init; }

    ////public required DirectoryInfo OutputDir { get; init; }

    ////public required string OutputBaseFileName { get; init; }

    public required bool Unique { get; init; }

    public required bool IncludeTransitive { get; init; }

    ////public required List<string> OutputFormats { get; init; }

    ////public required FileInfo? TemplateOutputFile { get; init; }

    public List<string> ExcludedProjects { get; } = [];

    public List<string> ExcludedPackages { get; } = [];
}
