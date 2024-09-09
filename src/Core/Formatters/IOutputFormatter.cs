// <copyright file="IOutputFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotNetLicensesReporter.Collectors;

namespace Lsquared.DotNetLicensesReporter.Formatters;

public interface IOutputFormatter
{
    string Name { get; }

    Task Write(Stream stream, IReadOnlyList<PackageLicense> packages);
}
