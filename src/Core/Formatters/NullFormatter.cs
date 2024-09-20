// <copyright file="NullFormatter.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

using Lsquared.DotnetTools.LicensesReporter.Collectors;
using Lsquared.DotnetTools.LicensesReporter.Formatters;

namespace Lsquared.DotnetTools.LicensesReporter;

public sealed partial class NullFormatter : IOutputFormatter
{
    public static readonly NullFormatter Instance = new();

    public string Name => "<none>>";

    private NullFormatter() { }

    public Task Write(Stream stream, IReadOnlyList<PackageLicense> packages) =>
        Task.CompletedTask;
}
