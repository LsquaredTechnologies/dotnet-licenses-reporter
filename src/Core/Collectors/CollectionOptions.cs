// <copyright file="CollectionOptions.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Collectors;

public sealed class CollectionOptions
{
    public required FileInfo ProjectFile { get; init; }

    public required bool ShouldRestore { get; init; }

    public required bool Unique { get; init; }

    public required bool IncludeTransitive { get; init; }

    public List<string> ExcludedProjects { get; } = [];

    public List<string> ExcludedPackages { get; } = [];
}
