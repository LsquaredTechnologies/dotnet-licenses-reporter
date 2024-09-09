// <copyright file="NullFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetLicensesReporter.Collectors;
using Lsquared.DotnetLicensesReporter.Formatters;

namespace Lsquared.DotnetLicensesReporter;

public sealed partial class NullFormatter : IOutputFormatter
{
    public static readonly NullFormatter Instance = new();

    public string Name => "<none>>";

    private NullFormatter() { }

    public Task Write(Stream stream, IReadOnlyList<PackageLicense> packages) =>
        Task.CompletedTask;
}
