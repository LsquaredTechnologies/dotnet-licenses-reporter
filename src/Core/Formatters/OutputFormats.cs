// <copyright file="OutputFormats.cs" company="Lsquared Technologies">
// Copyright © Lsquared Technologies
// SPDX-License-Identifier: MIT
// </copyright>

namespace Lsquared.DotnetLicensesReporter.Formatters;

public static class OutputFormats
{
    public static readonly IList<string> Defaults = [];

    public static IReadOnlyList<string> Get() =>
        Enumerable.Union(Defaults, Customs).ToList();

    public static void Add(string name) =>
        Customs.Add(name);

    private static readonly List<string> Customs = [];

}
