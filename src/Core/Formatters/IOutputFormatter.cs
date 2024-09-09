// <copyright file="IOutputFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetLicensesReporter.Collectors;

namespace Lsquared.DotnetLicensesReporter.Formatters;

public interface IOutputFormatter
{
    string Name { get; }

    Task Write(Stream stream, IReadOnlyList<PackageLicense> packages);
}
