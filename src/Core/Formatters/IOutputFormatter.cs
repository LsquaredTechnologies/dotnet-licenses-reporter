// <copyright file="IOutputFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Collectors;

namespace Lsquared.DotnetTools.LicensesReporter.Formatters;

public interface IOutputFormatter
{
    string Name { get; }

    Task Write(Stream stream, IReadOnlyList<PackageLicense> packages);
}
